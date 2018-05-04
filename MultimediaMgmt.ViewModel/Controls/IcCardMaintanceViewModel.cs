using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;

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
        public Action<string> MessageShow;
        public IcCardMaintanceViewModel()
        {
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
                data = data.Where(s => s.HexCode==HexCode);
            if (!string.IsNullOrEmpty(CardNum))
                data = data.Where(s => s.CardNum==CardNum);
            if (!string.IsNullOrEmpty(PersonName))
                data = data.Where(s => s.Name==PersonName);
            if (!string.IsNullOrEmpty(SelectedCardStatus))
                data = data.Where(s => s.Status == SelectedCardStatus);
            if (!string.IsNullOrEmpty(SelectedCardType))
                data = data.Where(s => s.CardType== SelectedCardType);
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
    }
}
