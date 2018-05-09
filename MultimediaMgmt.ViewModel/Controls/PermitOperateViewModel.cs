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

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class PermitOperateViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<PermitOperateEx> Permits { get; set; }
        public virtual PermitOperateEx SelectedPermit { get; set; }
        public virtual int? BuildingId { get; set; }
        public virtual string TerminalId { get; set; }
        public virtual string PersonId { get; set; }
        public virtual string PersonName { get; set; }

        public virtual List<KeyValuePair<int, string>> Buildings { get; set; }

        public PermitOperateViewModel()
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.id,
                Value = s.BuildingName
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).ToList();
        }

        [Command]
        public void Query()
        {
            var data = from p in multimediaEntities.ClassRoomPermit
                       join c in multimediaEntities.ClassRoom on p.TerminalId equals c.TerminalId
                       join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                       select new PermitOperateEx()
                       {
                           Id = p.ID,
                           BuildingId = b.id,
                           TerminalId = p.TerminalId,
                           RoomName = c.RoomName,
                           BuildingName = b.BuildingName,
                           PersonId = p.PersonId,
                           PermitTime = p.PermitTime
                       };
            if (BuildingId.HasValue && BuildingId.Value > 0)
                data = data.Where(s => s.BuildingId == BuildingId.Value);
            if (!string.IsNullOrEmpty(TerminalId))
                data = data.Where(s => s.TerminalId == TerminalId);
            if (!string.IsNullOrEmpty(PersonId))
                data = data.Where(s => s.PersonId.IndexOf(PersonId) >= 0);
            var temp = data.ToList();
            List<string> personNames;
            foreach (PermitOperateEx po in temp)
            {
                personNames = new List<string>();
                string[] personids = po.PersonId.Split(';');

                foreach (string id in personids)
                {
                    Person p = multimediaEntities.Person.FirstOrDefault(s => s.PersonId == id);
                    if (p != null)
                        personNames.Add(p.Name);
                }
                po.PersonName = string.Join(";", personNames.ToArray());
            }
            Permits = temp.ToSmartObservableCollection();
        }

        [Command]
        public void Delete()
        {
            if (SelectedPermit == null)
                return;
            ClassRoomPermit permit = multimediaEntities.ClassRoomPermit.FirstOrDefault(s => s.ID == SelectedPermit.Id);
            if (permit == null)
                return;
            multimediaEntities.ClassRoomPermit.Remove(permit);
            multimediaEntities.SaveChanges();
        }
    }
}
