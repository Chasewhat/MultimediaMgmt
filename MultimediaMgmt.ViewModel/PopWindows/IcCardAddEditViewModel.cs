using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MultimediaMgmt.ViewModel.PopWindows
{
    [POCOViewModel]
    public class IcCardAddEditViewModel : BaseViewModel
    {
        public virtual IcCard CurrIcCard { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        [Required(ErrorMessage = "IC卡号不能为空")]
        public virtual string HexCode { get; set; }
        [Required(ErrorMessage = "工号不能为空")]
        public virtual string PersonId { get; set; }
        protected void OnPersonIdChanged()
        {
            Task.Run(() =>
            {
                var person = multimediaEntities.IcCard.FirstOrDefault(s => s.PersonId == PersonId);
                if (person == null)
                {
                    var student = multimediaEntities.Student.FirstOrDefault(s => s.PersonId == PersonId);
                    if (student != null)
                    {
                        Name = student.Name;
                        Sex = student.Sex;
                        ClassId = student.ClassID;
                        Email = student.Email;
                        Phone = student.Phone;
                        return;
                    }
                }
                else
                {
                    Name = person.Name;
                    Sex = person.Sex;
                    //ClassId = person.ClassId;
                    Career = person.Career;
                    Email = person.Email;
                    Phone = person.Phone;
                    return;
                }
                IsSyncPerson = true;
            });
        }
        [Required(ErrorMessage = "卡类别不能为空")]
        public virtual string CardType { get; set; }
        [Required(ErrorMessage = "姓名不能为空")]
        public virtual string Name { get; set; }
        [Required(ErrorMessage = "性别不能为空")]
        public virtual string Sex { get; set; }
        [Required(ErrorMessage = "班级不能为空")]
        public virtual string ClassId { get; set; }

        public virtual string Career { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual bool? IsSyncPerson { get; set; }

        public virtual Dictionary<string, string> CardTypes { get; set; }
        public virtual Dictionary<string, string> Sexs { get; set; }

        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public IcCardAddEditViewModel(int id)
        {
            IsSyncPerson = true;
            currId = id;
            if (id > 0)
            {
                CurrIcCard = multimediaEntities.IcCard.FirstOrDefault(s => s.Id == id);
                PersonId = CurrIcCard.PersonId;
                WindowTitle = "IC卡信息编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "IC卡信息新增";
                ButtonContent = "增加";
            }
            if (CurrIcCard == null)
                CurrIcCard = new IcCard();
            HexCode = CurrIcCard.HexCode;
            CardType = CurrIcCard.CardType;
            CardTypes = Constants.CardTypes;
            Sexs = Constants.Sexs;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(HexCode) ||
                string.IsNullOrEmpty(CardType) ||
                string.IsNullOrEmpty(PersonId))
            {
                MessageShow("请确认必填项");
                return;
            }

            if ((IsSyncPerson.HasValue && IsSyncPerson.Value) && (string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Sex) ||
                string.IsNullOrEmpty(ClassId)))
            {
                MessageShow("请确认必填项");
                return;
            }
            try
            {
                CurrIcCard.HexCode = HexCode;
                CurrIcCard.CardType = CardType;
                CurrIcCard.PersonId = PersonId;
                if (currId > 0)
                    multimediaEntities.Entry(CurrIcCard).State = EntityState.Modified;
                else
                    multimediaEntities.IcCard.Add(CurrIcCard);
                if (IsSyncPerson.HasValue && IsSyncPerson.Value)
                {
                    //同步维护Person 老数据库不再同步维护

                    //bool isAdd = true;
                    //Person person = multimediaEntities.Person.FirstOrDefault(s => s.PersonId == PersonId);
                    //if (person == null)
                    //    person = new Person() { PersonId = PersonId };
                    //else
                    //    isAdd = false;
                    //person.Name = Name;
                    //person.Sex = Sex;
                    //person.ClassId = ClassId;
                    //person.FacultyId = FacultyId;
                    //person.Email = Email;
                    //person.Phone = Phone;
                    //if (isAdd)
                    //    multimediaEntities.Person.Add(person);
                    //else
                    //    multimediaEntities.Entry(person).State = EntityState.Modified;

                    //同步维护Student
                    bool isAdd = true;
                    Student student = multimediaEntities.Student.FirstOrDefault(s => s.PersonId == PersonId);
                    if (student == null)
                        student = new Student() { PersonId = PersonId };
                    else
                        isAdd = false;
                    student.Name = Name;
                    student.Sex = Sex;
                    student.ClassID = ClassId;
                    student.Email = Email;
                    student.Phone = Phone;
                    if (isAdd)
                        multimediaEntities.Student.Add(student);
                    else
                        multimediaEntities.Entry(student).State = EntityState.Modified;
                }

                multimediaEntities.SaveChanges();
                MessageShow("保存成功!");
            }
            catch (Exception ex)
            {
                MessageShow(string.Format("保存失败:{0}", ex.Message));
            }
            if (currId > 0)
                CloseWindow();
        }
    }
}
