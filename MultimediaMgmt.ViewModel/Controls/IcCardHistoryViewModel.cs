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
    public class IcCardHistoryViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<CardLogEx> CardLogExs { get; set; }

        public virtual List<KeyValuePair<int, string>> Buildings { get; set; }
        public virtual string BuildingName { get; set; }
        public virtual Dictionary<int, string> CardStatuss { get; set; }
        public virtual int? SelectedCardStatus { get; set; } = null;
        public virtual Dictionary<int, string> SwCardTypes { get; set; }
        public virtual int SelectedSwCardType { get; set; }
        protected void OnSelectedSwCardTypeChanged()
        {
            if (SelectedSwCardType == 1)
                CardStatuss = Constants.AccessCardStatuss;
            else
                CardStatuss = Constants.CardStatuss;
        }
        public virtual Dictionary<string, string> CardTypes { get; set; }
        public virtual DateTime? BeginDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual string HexCode { get; set; }
        public virtual string CardNum { get; set; }
        public virtual string PersonId { get; set; }
        public virtual string PersonName { get; set; }
        public virtual string RoomNum { get; set; }
        public virtual string Location { get; set; }

        public IcCardHistoryViewModel()
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.Id,
                Value = s.BuildingName
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).ToList();
            SwCardTypes = Constants.SwCardTypes;
            CardStatuss = Constants.CardStatuss;
            CardTypes = Constants.CardTypes;
            SelectedSwCardType = 0;
            BeginDate = EndDate = DateTime.Now.Date;
        }

        [Command]
        public void Query()
        {
            IEnumerable<CardLogEx> data;
            if (SelectedSwCardType == 1)
                data = from l in multimediaEntities.AccessCardLog
                       join c in multimediaEntities.IcCard on l.HexCode equals c.HexCode
                       join r in multimediaEntities.ClassRoom on l.TerminalId equals r.TerminalId
                       join b in multimediaEntities.ClassroomBuilding on r.BuildingId equals b.Id
                       join p in multimediaEntities.IcCard on c.PersonId equals p.PersonId
                       select new CardLogEx()
                       {
                           Id = (int)l.Id,
                           HexCode = l.HexCode,
                           CardNum = c.CardNum,
                           PersonId = c.PersonId,
                           Name = p.Name,
                           RoomId = r.Id,
                           RoomNum = r.RoomNum,
                           BuildingName = b.BuildingName,
                           Location = b.Location,
                           IdentifyMode = r.IdentifyMode,
                           TerminalId = r.TerminalId,
                           TerminalIp = r.TerminalIp,
                           CardType = c.CardType,
                           State = l.State,
                           LogTime = l.LogTime
                       };
            else
                data = from l in multimediaEntities.SwipeCardLog
                       join c in multimediaEntities.IcCard on l.HexCode equals c.HexCode
                       join r in multimediaEntities.ClassRoom on l.TerminalId equals r.TerminalId
                       join b in multimediaEntities.ClassroomBuilding on r.BuildingId equals b.Id
                       join p in multimediaEntities.IcCard on c.PersonId equals p.PersonId
                       select new CardLogEx()
                       {
                           Id = l.Id,
                           HexCode = l.HexCode,
                           CardNum = c.CardNum,
                           PersonId = c.PersonId,
                           Name = p.Name,
                           RoomId = r.Id,
                           RoomNum = r.RoomNum,
                           BuildingName = b.BuildingName,
                           Location = b.Location,
                           IdentifyMode = r.IdentifyMode,
                           TerminalId = r.TerminalId,
                           TerminalIp = r.TerminalIp,
                           CardType = c.CardType,
                           State = l.State,
                           LogTime = l.LogTime
                       };

            if (!string.IsNullOrEmpty(HexCode))
                data = data.Where(s => s.HexCode == HexCode);
            if (!string.IsNullOrEmpty(CardNum))
                data = data.Where(s => s.CardNum == CardNum);
            if (!string.IsNullOrEmpty(PersonId))
                data = data.Where(s => s.PersonId == PersonId);
            if (!string.IsNullOrEmpty(PersonName))
                data = data.Where(s => s.Name == PersonName);
            if (!string.IsNullOrEmpty(BuildingName))
                data = data.Where(s => s.BuildingName == BuildingName);
            if (!string.IsNullOrEmpty(Location))
                data = data.Where(s => s.Location == Location);
            if (!string.IsNullOrEmpty(RoomNum))
                data = data.Where(s => s.RoomNum == RoomNum);
            if (SelectedCardStatus.HasValue)
                data = data.Where(s => s.State == SelectedCardStatus.Value);
            if (BeginDate.HasValue && BeginDate.Value != default(DateTime))
                data = data.Where(s => s.LogTime >= BeginDate);
            if (EndDate.HasValue && EndDate.Value != default(DateTime))
                data = data.Where(s => s.LogTime <= EndDate);

            CardLogExs = data.ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            HexCode = CardNum = PersonId = PersonName = BuildingName =
                Location = RoomNum = null;
            SelectedCardStatus = null;
            BeginDate = EndDate = null;
        }
    }
}
