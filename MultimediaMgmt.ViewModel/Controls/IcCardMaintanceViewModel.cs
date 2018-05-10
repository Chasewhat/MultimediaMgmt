using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using System.Transactions;
using NPOI.SS.UserModel;
using System.Reflection;
using System.Collections;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class IcCardMaintanceViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<IcCardInfo> IcCards { get; set; }
        public virtual IcCardInfo SelectedIcCard { get; set; }
        public virtual Dictionary<int, string> CardStatuss { get; set; }
        public virtual string SelectedCardStatus { get; set; }
        public virtual Dictionary<string, string> CardTypes { get; set; }
        public virtual string SelectedCardType { get; set; }
        public virtual Dictionary<string, string> Sexs { get; set; }
        public virtual string SelectedSex { get; set; }

        public virtual string HexCode { get; set; }

        public virtual string CardNum { get; set; }

        public virtual string PersonId { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string WaitIndiContent { get; set; }
        public virtual bool IsLoad { get; set; }
        public Func<string> FileSave;
        public Action<string> FileOpen;
        public Action<string> MessageShow;
        public Func<int,MemoryStream> GetExport;
        public IcCardMaintanceViewModel()
        {
            IsLoad = false;
            Sexs = Constants.Sexs;
            CardTypes = Constants.CardTypes;
            CardStatuss = Constants.CardStatuss;
        }

        [Command]
        public void Query()
        {
            var data = from i in multimediaEntities.IcCard
                       join p in multimediaEntities.Person on i.PersonId equals p.PersonId
                       select new IcCardInfo()
                       {
                           Id = i.Id,
                           HexCode = i.HexCode,
                           CardNum = i.CardNum,
                           PersonId = i.PersonId,
                           CardType = i.CardType,
                           Status = i.Status,
                           Name = p.Name,
                           Sex = p.Sex,
                           FacultyId = p.FacultyId,
                           Email = p.Email,
                           Phone = p.Phone
                       };
            if (!string.IsNullOrEmpty(PersonId))
                data = data.Where(s => s.PersonId == PersonId);
            if (!string.IsNullOrEmpty(HexCode))
                data = data.Where(s => s.HexCode == HexCode);
            if (!string.IsNullOrEmpty(CardNum))
                data = data.Where(s => s.CardNum == CardNum);
            if (!string.IsNullOrEmpty(PersonName))
                data = data.Where(s => s.Name == PersonName);
            if (!string.IsNullOrEmpty(SelectedCardStatus))
                data = data.Where(s => s.Status == SelectedCardStatus);
            if (!string.IsNullOrEmpty(SelectedCardType))
                data = data.Where(s => s.CardType == SelectedCardType);
            if (!string.IsNullOrEmpty(SelectedSex))
                data = data.Where(s => s.Sex == SelectedSex);
            IcCards = data.ToSmartObservableCollection();
        }

        [Command]
        public void Delete()
        {
            if (SelectedIcCard == null)
                return;
            IcCard card = multimediaEntities.IcCard.FirstOrDefault(s => s.Id == SelectedIcCard.Id);
            multimediaEntities.IcCard.Remove(card);
            multimediaEntities.SaveChanges();
            Query();
        }

        [Command]
        public void DeleteAll()
        {
            if (SelectedIcCard == null)
                return;
            IcCard card = multimediaEntities.IcCard.FirstOrDefault(s => s.Id == SelectedIcCard.Id);
            multimediaEntities.IcCard.Remove(card);
            //同步删除Person Student 验证
            if (multimediaEntities.IcCard.Count(s => s.PersonId == SelectedIcCard.PersonId && s.Id != SelectedIcCard.Id) > 0)
            {
                MessageShow("当前用户存在其他关联IC卡");
                return;
            }
            //同步删除Person
            Person person = multimediaEntities.Person.FirstOrDefault(s => s.PersonId == SelectedIcCard.PersonId);
            multimediaEntities.Person.Remove(person);
            //同步删除Student
            Student student = multimediaEntities.Student.FirstOrDefault(s => s.PersonId == SelectedIcCard.PersonId);
            multimediaEntities.Student.Remove(student);

            multimediaEntities.SaveChanges();
            Query();
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
                IsLoad = false;
                MessageShow(e.Message);
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
                bool task = ExportToExcel(filePath);
                //task.Wait();
                IsLoad = false;
                if (!task)
                {
                    MessageShow("导出时发生异常");
                }
            }
            catch (Exception e)
            {
                IsLoad = false;
                MessageShow(e.Message);
            }
        }

        private List<string> IcCardField = new List<string>() { "HexCode", "CardNum", "PersonId", "CardType", "Status" };
        private List<string> PersonField = new List<string>() { "PersonId", "Name", "Account", "Password", "Sex", "FacultyId", "ClassId", "Email", "Phone", "EntryTime", "DepartureDate" };
        private List<string> StudentField = new List<string>() { "PersonId", "Name", "Account", "Password", "Sex", "ClassId", "Email", "Phone", "EntryTime" };

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
                            multimediaEntities.Database.ExecuteSqlCommand("delete from [IcCard]");
                            multimediaEntities.Database.ExecuteSqlCommand("delete from [Person]");
                            multimediaEntities.Database.ExecuteSqlCommand("delete from [Student]");
                        }
                        IWorkbook workbook = WorkbookFactory.Create(file);
                        IEnumerator rows;
                        PropertyInfo[] pis = typeof(IcCard).GetProperties();
                        string sheetName = "IC卡号表";
                        ISheet sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            rows = sheet.GetRowEnumerator();
                            rows.MoveNext();
                            while (rows.MoveNext())
                            {
                                HSSFRow row = (HSSFRow)rows.Current;
                                IcCard card = new IcCard();
                                for (int i = 0; i < row.LastCellNum; i++)
                                {
                                    PropertyInfo pi = pis.Where(p => p.Name.Equals(IcCardField[i])).FirstOrDefault();
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
                                multimediaEntities.IcCard.Add(card);
                            }
                        }
                        pis = typeof(Person).GetProperties();
                        sheetName = "老师名单";
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            rows = sheet.GetRowEnumerator();
                            rows.MoveNext();
                            while (rows.MoveNext())
                            {
                                HSSFRow row = (HSSFRow)rows.Current;
                                Person person = new Person();
                                for (int i = 0; i < row.LastCellNum; i++)
                                {
                                    PropertyInfo pi = pis.Where(p => p.Name.Equals(IcCardField[i])).FirstOrDefault();
                                    if (null != pi)
                                    {
                                        ICell cell = row.GetCell(i);
                                        if (cell != null)
                                        {
                                            string s = cell.ToString();
                                            pi.SetValue(person, Convert.ChangeType(cell.ToString(), pi.PropertyType), null);
                                        }
                                    }
                                }
                                multimediaEntities.Person.Add(person);
                            }
                        }
                        pis = typeof(Student).GetProperties();
                        sheetName = "学生名单";
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet != null)
                        {
                            rows = sheet.GetRowEnumerator();
                            rows.MoveNext();
                            while (rows.MoveNext())
                            {
                                HSSFRow row = (HSSFRow)rows.Current;
                                Student student = new Student();
                                for (int i = 0; i < row.LastCellNum; i++)
                                {
                                    PropertyInfo pi = pis.Where(p => p.Name.Equals(IcCardField[i])).FirstOrDefault();
                                    if (null != pi)
                                    {
                                        ICell cell = row.GetCell(i);
                                        if (cell != null)
                                        {
                                            string s = cell.ToString();
                                            pi.SetValue(student, Convert.ChangeType(cell.ToString(), pi.PropertyType), null);
                                        }
                                    }
                                }
                                multimediaEntities.Student.Add(student);
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
