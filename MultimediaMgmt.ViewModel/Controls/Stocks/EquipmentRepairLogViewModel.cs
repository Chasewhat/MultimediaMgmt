using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using MultimediaMgmt.Common.Helper;
using MultimediaMgmt.ViewModel.Notice;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Reflection;
using System.Collections;
using System.Transactions;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentRepairLogViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentRepairLog> EquipmentRepairLogs { get; set; }
        public virtual EquipmentRepairLog SelectedEquipmentRepairLog { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string WaitIndiContent { get; set; }
        public virtual bool IsLoad { get; set; }
        public Func<string> FileSave;
        public Action<string> FileOpen;
        public Action<string> MessageShow;
        public Func<int, MemoryStream> GetExport;

        public EquipmentRepairLogViewModel()
        {
            IsLoad = false;
            //订阅设备状态查询事件
            NOTICE.GetEvent<EquipmentStatusInquiryEvent>().Subscribe(QueryStatus);
        }

        public void QueryStatus(string number)
        {
            SerialNumber = number;
            Query();
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.EquipmentRepairLog
                       select b;
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);

            EquipmentRepairLogs = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            SerialNumber = null;
        }

        [Command]
        public void ImportExcel()
        {
            FileOpen("");
        }
        public void ImportExcel(string file, bool isOverride)
        {
            WaitIndiContent = "正在导入...";
            IsLoad = true;
            try
            {
                string result = string.Empty;
                bool task = ImportFromExcel(isOverride, file, ref result);
                //task.Wait();
                IsLoad = false;
                if (!task)
                {
                    MessageShow(result);
                }
            }
            catch (Exception e)
            {
                MessageShow(e.Message);
            }
            finally
            {
                IsLoad = false;
            }
            Query();
        }
        [Command]
        public void ExportExcel()
        {
            WaitIndiContent = "正在导出...";
            IsLoad = true;
            try
            {
                string filePath = FileSave();
                if (string.IsNullOrEmpty(filePath))
                    return;
                bool task = ExportToExcel(filePath);
                //task.Wait();
                if (!task)
                {
                    MessageShow("导出时发生异常");
                }
            }
            catch (Exception e)
            {
                MessageShow(e.Message);
            }
            finally
            {
                IsLoad = false;
            }
        }

        private List<string> Fields = new List<string>() { "SerialNumber", "Fault", "Proposer", "DeclarationDate", "RepairDate", "RepairPrice", "RepairComment",
                                                                "ApproverOfficer","Operator","Remarks"};

        public bool ImportFromExcel(bool overwrite, string file, ref string result)
        {
            //return new Task<bool>(() =>
            //{
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew,
                               new System.TimeSpan(1, 30, 0)))
                {
                    try
                    {
                        if (overwrite)
                        {
                            multimediaEntities.Database.ExecuteSqlCommand("delete from [EquipmentRepairLog]");
                        }
                        IWorkbook workbook = WorkbookFactory.Create(file);
                        IEnumerator rows;
                        PropertyInfo[] pis = typeof(EquipmentRepairLog).GetProperties();
                        string sheetName = "设备维修";
                        ISheet sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            rows = sheet.GetRowEnumerator();
                            rows.MoveNext();
                            while (rows.MoveNext())
                            {
                                HSSFRow row = (HSSFRow)rows.Current;
                                EquipmentRepairLog card = new EquipmentRepairLog();
                                for (int i = 0; i < row.LastCellNum; i++)
                                {
                                    PropertyInfo pi = pis.Where(p => p.Name.Equals(Fields[i])).FirstOrDefault();
                                    if (null != pi)
                                    {
                                        ICell cell = row.GetCell(i);
                                        if (cell != null)
                                        {
                                            string s = cell.ToString();
                                            pi.SetValue(card, Convert.ChangeType(cell.ToString(), pi.PropertyType), null);
                                        }
                                    }
                                }
                                multimediaEntities.EquipmentRepairLog.Add(card);
                            }
                            multimediaEntities.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                        return false;
                    }
                    ts.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
            //});
        }

        public bool ExportToExcel(string filename)
        {

            FileStream stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                MemoryStream mstream;
                IWorkbook workbook;
                if (Path.GetExtension(filename).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    mstream = GetExport(1);
                    if (mstream == null)
                        return false;
                    mstream.WriteTo(stream);
                    workbook = new XSSFWorkbook();
                    mstream.Close();
                }
                else
                {
                    mstream = GetExport(0);
                    if (mstream == null)
                        return false;
                    mstream.WriteTo(stream);
                    workbook = new HSSFWorkbook();
                    mstream.Close();
                }
                workbook.Write(stream);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                stream.Close();
            }
            //});
        }
    }
}
