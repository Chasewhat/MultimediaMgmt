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
using System.Data.Entity;

namespace MultimediaMgmt.ViewModel.PopWindows
{
    [POCOViewModel]
    public class PermitAddEditViewModel : BaseViewModel
    {
        public virtual PermitOperateEx CurrPermit { get; set; }
        public virtual List<KeyValuePair<int, string>> Buildings { get; set; }
        public virtual Dictionary<string, string> Sexs { get; set; }

        public virtual SmartObservableCollection<string> Times { get; set; } = new SmartObservableCollection<string>();
        public virtual SmartObservableCollection<string> SelectedTimes { get; set; }

        public virtual SmartObservableCollection<Person> Persons { get; set; }
        public virtual SmartObservableCollection<Person> SelectedPersons { get; set; }

        public virtual SmartObservableCollection<Person> ChoosedPersons { get; set; } = new SmartObservableCollection<Person>();
        public virtual SmartObservableCollection<Person> SelectedChoosedPersons { get; set; }

        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        public virtual DateTime? TimeBegin { get; set; }
        public virtual DateTime? TimeEnd { get; set; }

        public virtual string CollegeName { get; set; }
        public virtual string MajorsName { get; set; }
        public virtual string Sex { get; set; }
        public virtual string ClassId { get; set; }

        public virtual int? BuildingId { get; set; }
        protected void OnBuildingIdChanged()
        {
            if (BuildingId.HasValue && BuildingId.Value > 0)
            {
                TerminalIds = multimediaEntities.ClassRoom.
                    Where(s => s.BuildingId == BuildingId.Value).
                    Select(s =>
                    new { s.RoomNum, s.TerminalId }).AsEnumerable().Select(s =>
                             new KeyValuePair<string, string>(s.TerminalId, s.RoomNum)).ToList();
            }
        }
        public virtual string TerminalId { get; set; }

        public virtual List<KeyValuePair<string, string>> TerminalIds { get; set; }

        public Action<string> MessageShow;
        public Action CloseWindow;

        private int currId = 0;
        public PermitAddEditViewModel(int id)
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.Id,
                Value = s.BuildingName
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).ToList();
            Sexs = Constants.Sexs;
            currId = id;
            if (id > 0)
            {
                CurrPermit = (from p in multimediaEntities.ClassRoomPermit
                              join c in multimediaEntities.ClassRoom on p.TerminalId equals c.TerminalId
                              join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.Id
                              where p.ID == id
                              select new PermitOperateEx()
                              {
                                  Id = p.ID,
                                  BuildingId = b.Id,
                                  TerminalId = p.TerminalId,
                                  RoomName = c.RoomNum,
                                  BuildingName = b.BuildingName,
                                  PersonId = p.PersonId,
                                  PermitTime = p.PermitTime
                              }).FirstOrDefault();
                WindowTitle = "卡控编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "卡控新增";
                ButtonContent = "增加";
            }
            if (CurrPermit == null)
                CurrPermit = new PermitOperateEx();
            else
            {
                Times = CurrPermit.PermitTime.Split(';').ToSmartObservableCollection();
                List<Person> temp = new List<Person>();
                foreach (var p in CurrPermit.PersonId.Split(';'))
                {
                    IcCard tp = multimediaEntities.IcCard.FirstOrDefault(s => s.PersonId == p);
                    if (tp != null)
                        temp.Add(new Person() { PersonId = p, Name = tp.Name });
                }
                ChoosedPersons = temp.ToSmartObservableCollection();
            }
            BuildingId = CurrPermit.BuildingId;
            TerminalId = CurrPermit.TerminalId;
            SelectedTimes = new SmartObservableCollection<string>();
            SelectedPersons = new SmartObservableCollection<Person>();
            SelectedChoosedPersons = new SmartObservableCollection<Person>();
        }

        [Command]
        public void Confirm()
        {
            CurrPermit.PermitTime = string.Join(";", Times.Distinct().ToArray());
            CurrPermit.PersonId = string.Join(";", ChoosedPersons.Select(s => s.PersonId).Distinct().ToArray());
            if (string.IsNullOrEmpty(TerminalId) ||
                string.IsNullOrEmpty(CurrPermit.PersonId) ||
                string.IsNullOrEmpty(CurrPermit.PermitTime))
            {
                MessageShow("请确认必填项");
                return;
            }
            try
            {
                if (currId > 0)
                {
                    ClassRoomPermit permit = multimediaEntities.ClassRoomPermit.FirstOrDefault(s => s.ID == currId);
                    if (permit != null)
                    {
                        permit.TerminalId = TerminalId;
                        permit.PermitTime = CurrPermit.PermitTime;
                        permit.PersonId = CurrPermit.PersonId;
                        multimediaEntities.Entry(permit).State = EntityState.Modified;
                    }
                }
                else
                {
                    ClassRoomPermit permit = new ClassRoomPermit()
                    {
                        TerminalId = TerminalId,
                        PersonId = CurrPermit.PersonId,
                        PermitTime = CurrPermit.PermitTime
                    };
                    multimediaEntities.Entry(permit).State = EntityState.Added;
                    //multimediaEntities.ClassRoomPermit.Add(permit);
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

        [Command]
        public void GetTerminalId()
        {
            if (string.IsNullOrEmpty(CurrPermit.RoomName) || CurrPermit.BuildingId <= 0)
                return;
            ClassRoom temp = multimediaEntities.ClassRoom.FirstOrDefault(s => s.BuildingId == CurrPermit.BuildingId &&
                    s.RoomNum == CurrPermit.RoomName);
            if (temp != null)
                CurrPermit.TerminalId = temp.TerminalId;
        }

        [Command]
        public void TimeAdd()
        {
            if (TimeBegin == null || TimeEnd == null)
                return;
            string time = string.Format("{0}-{1}",
                TimeBegin.Value.ToString("HH:mm"), TimeEnd.Value.ToString("HH:mm"));
            if (Times.FirstOrDefault(s => s == time) == null)
                Times.Add(time);
        }

        [Command]
        public void TimeDelete()
        {
            if (SelectedTimes.Count <= 0)
                return;
            Times.RemoveRange(SelectedTimes);
        }

        [Command]
        public void QueryPerson()
        {
            Persons = (from p in multimediaEntities.IcCard
                       join m in multimediaEntities.Majors on p.FacultyId.ToString() equals m.FacultyId into temp
                       from t in temp.DefaultIfEmpty()
                       where //(string.IsNullOrEmpty(ClassId) || p.ClassId == ClassId) &&
                            (string.IsNullOrEmpty(Sex) || p.Sex == Sex) &&
                            (string.IsNullOrEmpty(CollegeName) || (p.Faculty != null && p.Faculty.FacultyName == CollegeName)) &&
                            (string.IsNullOrEmpty(MajorsName) || (t != null && t.MajorsName == MajorsName))
                       select new Person()
                       {
                           PersonId = p.PersonId,
                           Name = p.Name
                       }).ToSmartObservableCollection();
        }

        [Command]
        public void Reset()
        {
            ClassId = Sex = CollegeName = MajorsName = null;
        }

        [Command]
        public void PersonAdd()
        {
            ChoosedPersons.AddRange(SelectedPersons.Where(s => ChoosedPersons.FirstOrDefault(f => f.PersonId == s.PersonId) == null));
        }

        [Command]
        public void PersonDelete()
        {
            ChoosedPersons.RemoveRange(SelectedChoosedPersons);
        }
    }
}
