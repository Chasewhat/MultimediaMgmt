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
using System.Transactions;
using NPOI.SS.UserModel;
using System.Reflection;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentInStockViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<EquipmentInStock> EquipmentInStocks { get; set; }
        public virtual EquipmentInStock SelectedEquipmentInStock { get; set; }
        protected void OnSelectedEquipmentInStockChanged()
        {
            StatusInquiry();
        }
        public virtual string SerialNumber { get; set; }
        public virtual DateTime? InBegin { get; set; }
        public virtual DateTime? InEnd { get; set; }
        public virtual DateTime? UseBegin { get; set; }
        public virtual DateTime? UseEnd { get; set; }
        public virtual string Name { get; set; }
        public virtual string UsePlace { get; set; }
        public virtual List<KeyValuePair<string, string>> EquipmentTypes { get; set; }
        public virtual string WaitIndiContent { get; set; }
        public virtual bool IsLoad { get; set; }
        public Func<string> FileSave;
        public Action<string> FileOpen;
        public Action<string> MessageShow;
        public Func<int, MemoryStream> GetExport;

        public EquipmentInStockViewModel()
        {
            IsLoad = false;
            EquipmentTypes = multimediaEntities.EquipmentType.Select(s => new
            {
                Key = s.EquipmentName,
                Value = s.EquipmentCategory
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<string, string>(
                                s.Key,
                                s.Key)).ToList();
        }

        [Command]
        public void Query()
        {
            var data = from b in multimediaEntities.EquipmentInStock
                       select b;
            if (!string.IsNullOrEmpty(SerialNumber))
                data = data.Where(s => s.SerialNumber == SerialNumber);
            if (InBegin.HasValue && InBegin.Value != default(DateTime))
                data = data.Where(s => s.Intime >= InBegin);
            if (InEnd.HasValue && InEnd.Value != default(DateTime))
                data = data.Where(s => s.Intime <= InEnd);

            if (UseBegin.HasValue && UseBegin.Value != default(DateTime))
                data = data.Where(s => s.UseDate >= UseBegin);
            if (UseEnd.HasValue && UseEnd.Value != default(DateTime))
                data = data.Where(s => s.UseDate <= UseEnd);
            if (!string.IsNullOrEmpty(Name))
                data = data.Where(s => s.Name == Name);
            if (!string.IsNullOrEmpty(UsePlace))
                data = data.Where(s => s.Place == UsePlace);
            EquipmentInStocks = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            SerialNumber = Name = null;
            InBegin = InEnd = UseBegin = UseEnd = null;
        }

        //[Command]
        public void StatusInquiry()
        {
            if (SelectedEquipmentInStock != null)
                //发布设备状态查询事件
                NOTICE.GetEvent<EquipmentStatusInquiryEvent>().Publish(SelectedEquipmentInStock.SerialNumber);
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

        private List<string> Fields = new List<string>() { "SerialNumber", "Name", "Manufacturer", "SaleCompany", "Type", "Configuration", "ProduceDate",
                                                                "UserDepartment","Place","Keeper","Price","IncreaseType","UseDate","Intime","Remarks"};

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
                            multimediaEntities.Database.ExecuteSqlCommand("delete from [EquipmentInStock]");
                        }
                        IWorkbook workbook = WorkbookFactory.Create(file);
                        IEnumerator rows;
                        PropertyInfo[] pis = typeof(EquipmentInStock).GetProperties();
                        string sheetName = "设备入库";
                        ISheet sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            rows = sheet.GetRowEnumerator();
                            rows.MoveNext();
                            while (rows.MoveNext())
                            {
                                HSSFRow row = (HSSFRow)rows.Current;
                                EquipmentInStock card = new EquipmentInStock();
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
                                multimediaEntities.EquipmentInStock.Add(card);
                            }
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
