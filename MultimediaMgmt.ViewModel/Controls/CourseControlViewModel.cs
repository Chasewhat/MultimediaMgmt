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
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using Common.Helper;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Transactions;
using System.Collections;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class CourseControlViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<CourseEx> CourseExs { get; set; }
        public virtual CourseEx SelectedCourseEx { get; set; }
        public virtual DateTime? Date { get; set; }
        protected void OnDateChanged()
        {
            if (Date.HasValue && Date.Value != default(DateTime))
                GetCurrWeek();
        }

        public virtual string Week1 { get; set; }
        public virtual string Week2 { get; set; }
        public virtual string Week3 { get; set; }
        public virtual string Week4 { get; set; }
        public virtual string Week5 { get; set; }
        public virtual string Week6 { get; set; }
        public virtual string Week7 { get; set; }

        public virtual bool IsChange { get; set; }
        public virtual bool IsEnable { get; set; }

        public virtual string WaitIndiContent { get; set; }
        public virtual bool IsLoad { get; set; }

        private List<CourseChange> ChangedCourses = new List<CourseChange>();
        private List<KeyValuePair<int, string>> weeks = new List<KeyValuePair<int, string>>();

        public int RoomId = 0;

        public Func<string> FileSave;
        public Action<string> FileOpen;
        public Action<string> MessageShow;

        public CourseControlViewModel()
        {
            Date = DateTime.Now.Date;
            IsChange = true;
            IsEnable = false;
            IsLoad = false;
        }

        public void NotChange()
        {
            IsChange = false;
        }

        [Command]
        public void Query(int roomId = 0)
        {
            if (RoomId <= 0 || !Date.HasValue || Date.Value == default(DateTime)
                || weeks.Count != 7)
                return;
            IsEnable = false;
            ChangedCourses.Clear();
            //先获取WeeklyCourseTable
            if (IsChange)
                CourseExs = GetCourses(RoomId).ToSmartObservableCollection();
            else
                CourseExs = GetCoursesReservation(RoomId).ToSmartObservableCollection();
        }

        private IEnumerable<CourseEx> GetCourses(int roomid)
        {
            var weekCourses = (from w in multimediaEntities.WeeklyCourseTable
                               join p in multimediaEntities.Person on w.PersonId equals p.PersonId into temp
                               from t in temp.DefaultIfEmpty()
                               where w.RoomId == roomid
                               select new
                               {
                                   w.RoomId,
                                   w.ClassOrd,
                                   w.Date,
                                   w.PersonId,
                                   w.BeginTime,
                                   w.EndTime,
                                   Name = t == null ? "" : t.Name,
                                   w.CourseName
                               }).AsEnumerable().Where(s => weeks.Any(w => w.Value == s.Date)).Select(s => new
                                CourseWeekEx
                               {
                                   RoomId = s.RoomId,
                                   ClassOrd = s.ClassOrd,
                                   DayOfWeek = (byte)weeks.First(w => w.Value == s.Date).Key,
                                   PersonId = s.PersonId,
                                   BeginTime = s.BeginTime,
                                   EndTime = s.EndTime,
                                   Name = s.Name,
                                   CourseName = s.CourseName
                               }).ToList();
            var stdCourses = from s in multimediaEntities.StdCourseTable
                             join p in multimediaEntities.Person on s.PersonId equals p.PersonId into temp
                             from t in temp.DefaultIfEmpty()
                             where s.RoomId == roomid
                             select new CourseWeekEx
                             {
                                 RoomId = s.RoomId,
                                 ClassOrd = s.ClassOrd,
                                 DayOfWeek = s.DayOfWeek,
                                 PersonId = s.PersonId,
                                 BeginTime = s.BeginTime,
                                 EndTime = s.EndTime,
                                 Name = t == null ? "" : t.Name,
                                 CourseName = s.CourseName,
                             };
            foreach (var week in weekCourses)
            {
                if (week.PersonId.Equals("UNRESERVED", StringComparison.OrdinalIgnoreCase))
                {
                    week.PersonId = "";
                    week.Name = "";
                    week.CourseName = "";
                }
            }
            foreach (var std in stdCourses)
            {
                if (weekCourses.FirstOrDefault(s => s.DayOfWeek == std.DayOfWeek &&
                            s.ClassOrd == std.ClassOrd) == null)
                    weekCourses.Add(std);
            }

            var data = (from p in multimediaEntities.StdClassPeriod
                        select p).AsEnumerable().Select(
                       p => new CourseEx()
                       {
                           RoomId = roomid,
                           ClassOrd = p.ClassOrd,
                           BeginTime = p.BeginTime,
                           EndTime = p.EndTime,
                           PersonId1 = weekCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name1 = weekCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName1 = weekCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId2 = weekCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name2 = weekCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName2 = weekCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId3 = weekCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name3 = weekCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName3 = weekCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId4 = weekCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name4 = weekCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName4 = weekCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId5 = weekCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name5 = weekCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName5 = weekCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId6 = weekCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name6 = weekCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName6 = weekCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId7 = weekCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name7 = weekCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName7 = weekCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault()
                       });
            return data.OrderBy(s => s.ClassOrd);
        }

        private IEnumerable<CourseEx> GetCoursesReservation(int roomid)
        {
            var test = multimediaEntities.ReservationCourseTable.FirstOrDefault();
                var reserCourses = (from w in multimediaEntities.ReservationCourseTable
                                    join p in multimediaEntities.Person on w.PersonId equals p.PersonId into temp
                                    from t in temp.DefaultIfEmpty()
                                    where w.RoomId == roomid
                                 &&
                                    w.ClassroomReservation.ReservationState != 1 &&
                                 w.ClassroomReservation.ClassroomReservationApproval.Count(a => a.ApprovalLevel == 1 && a.ApprovalState == 1) > 0
                                    select new
                                    {
                                        w.RoomId,
                                        w.ClassOrd,
                                        w.Date,
                                        w.PersonId,
                                        w.BeginTime,
                                        w.EndTime,
                                        Name = t == null ? "" : t.Name,
                                        w.CourseName
                                    }).AsEnumerable().Where(s => weeks.Any(w => w.Value == s.Date)).Select(s => new
                                     CourseWeekEx
                                    {
                                        RoomId = s.RoomId,
                                        ClassOrd = s.ClassOrd,
                                        DayOfWeek = (byte)weeks.First(w => w.Value == s.Date).Key,
                                        PersonId = s.PersonId,
                                        BeginTime = s.BeginTime,
                                        EndTime = s.EndTime,
                                        Name = s.Name,
                                        CourseName = s.CourseName
                                    }).ToList();
            var weekCourses = (from w in multimediaEntities.WeeklyCourseTable
                               join p in multimediaEntities.Person on w.PersonId equals p.PersonId into temp
                               from t in temp.DefaultIfEmpty()
                               where w.RoomId == roomid
                               select new
                               {
                                   w.RoomId,
                                   w.ClassOrd,
                                   w.Date,
                                   w.PersonId,
                                   w.BeginTime,
                                   w.EndTime,
                                   Name = t == null ? "" : t.Name,
                                   w.CourseName
                               }).AsEnumerable().Where(s => weeks.Any(w => w.Value == s.Date)).Select(s => new
                                CourseWeekEx
                               {
                                   RoomId = s.RoomId,
                                   ClassOrd = s.ClassOrd,
                                   DayOfWeek = (byte)weeks.First(w => w.Value == s.Date).Key,
                                   PersonId = s.PersonId,
                                   BeginTime = s.BeginTime,
                                   EndTime = s.EndTime,
                                   Name = s.Name,
                                   CourseName = s.CourseName
                               }).ToList();
            var stdCourses = from s in multimediaEntities.StdCourseTable
                             join p in multimediaEntities.Person on s.PersonId equals p.PersonId into temp
                             from t in temp.DefaultIfEmpty()
                             where s.RoomId == roomid
                             select new CourseWeekEx
                             {
                                 RoomId = s.RoomId,
                                 ClassOrd = s.ClassOrd,
                                 DayOfWeek = s.DayOfWeek,
                                 PersonId = s.PersonId,
                                 BeginTime = s.BeginTime,
                                 EndTime = s.EndTime,
                                 Name = t == null ? "" : t.Name,
                                 CourseName = s.CourseName,
                             };
            foreach (var week in weekCourses)
            {
                if (week.PersonId.Equals("UNRESERVED", StringComparison.OrdinalIgnoreCase))
                {
                    week.PersonId = "";
                    week.Name = "";
                    week.CourseName = "";
                }
            }
            foreach (var week in weekCourses)
            {
                if (reserCourses.FirstOrDefault(s => s.DayOfWeek == week.DayOfWeek &&
                            s.ClassOrd == week.ClassOrd) == null)
                    reserCourses.Add(week);
            }
            foreach (var std in stdCourses)
            {
                if (reserCourses.FirstOrDefault(s => s.DayOfWeek == std.DayOfWeek &&
                            s.ClassOrd == std.ClassOrd) == null)
                    reserCourses.Add(std);
            }

            var data = (from p in multimediaEntities.StdClassPeriod
                        select p).AsEnumerable().Select(
                       p => new CourseEx()
                       {
                           RoomId = roomid,
                           ClassOrd = p.ClassOrd,
                           BeginTime = p.BeginTime,
                           EndTime = p.EndTime,
                           PersonId1 = reserCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name1 = reserCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName1 = reserCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId2 = reserCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name2 = reserCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName2 = reserCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId3 = reserCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name3 = reserCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName3 = reserCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId4 = reserCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name4 = reserCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName4 = reserCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId5 = reserCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name5 = reserCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName5 = reserCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId6 = reserCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name6 = reserCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName6 = reserCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId7 = reserCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           Name7 = reserCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName7 = reserCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault()
                       });
            return data.OrderBy(s => s.ClassOrd);
        }

        private void GetCurrWeek()
        {
            DateTime week1 = Date.Value.AddDays(1 -
                (Date.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)Date.Value.DayOfWeek));//本周周一
            weeks.Clear();
            for (int i = 0; i < 7; i++)
            {
                weeks.Add(new KeyValuePair<int, string>(i + 1,
                    week1.AddDays(i).ToString("yyyy-MM-dd")));
            }

            Week1 = string.Format("星期一({0})", weeks[0].Value);
            Week2 = string.Format("星期二({0})", weeks[1].Value);
            Week3 = string.Format("星期三({0})", weeks[2].Value);
            Week4 = string.Format("星期四({0})", weeks[3].Value);
            Week5 = string.Format("星期五({0})", weeks[4].Value);
            Week6 = string.Format("星期六({0})", weeks[5].Value);
            Week7 = string.Format("星期日({0})", weeks[6].Value);
        }

        [Command]
        public void Cancel()
        {
            Query();
        }

        [Command]
        public void Save()
        {
            if (RoomId <= 0)
                return;
            SaveExec(RoomId);
        }

        private void SaveExec(int roomid)
        {
            if (ChangedCourses.Count <= 0)
                return;
            var stdcourses = multimediaEntities.StdCourseTable.AsEnumerable();
            var courses = multimediaEntities.WeeklyCourseTable.AsEnumerable();
            var period = multimediaEntities.StdClassPeriod.AsEnumerable();
            StdClassPeriod per = null;
            WeeklyCourseTable wct = null;
            StdCourseTable sct = null;
            string date = string.Empty;
            foreach (CourseChange cc in ChangedCourses)
            {
                per = period.FirstOrDefault(s => s.ClassOrd == cc.ClassOrd);
                date = weeks[cc.Index - 1].Value;
                if (per == null || string.IsNullOrEmpty(date))
                    continue;
                wct = courses.FirstOrDefault(s => s.RoomId == roomid &&
                    s.ClassOrd == cc.ClassOrd && s.Date == date);
                sct = stdcourses.FirstOrDefault(s => s.RoomId == roomid &&
                    s.ClassOrd == cc.ClassOrd && s.DayOfWeek == cc.Index);
                if (sct != null)
                {
                    //如果标准课表有记录，周课表没记录则设置为未占用
                    if (string.IsNullOrEmpty(cc.PersonId) && !string.IsNullOrEmpty(sct.PersonId))
                    {
                        cc.PersonId = "UNRESERVED";
                        cc.CourseName = "";
                    }
                    //如果标准课表记录与周课表记录一致则取标准课表的记录，周课表不做记录
                    else if (!string.IsNullOrEmpty(cc.PersonId))
                    {
                        if (cc.PersonId == sct.PersonId &&
                            cc.CourseName == sct.CourseName)
                        {
                            continue;
                        }
                    }
                }
                if (wct != null)
                {
                    wct.PersonId = cc.PersonId;
                    wct.CourseName = cc.CourseName;
                    multimediaEntities.Entry(wct).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    wct = new WeeklyCourseTable()
                    {
                        Date = date,
                        ClassOrd = cc.ClassOrd,
                        BeginTime = per.BeginTime,
                        EndTime = per.EndTime,
                        RoomId = roomid,
                        PersonId = cc.PersonId,
                        CourseName = cc.CourseName
                    };
                    multimediaEntities.WeeklyCourseTable.Add(wct);
                }
            }
            multimediaEntities.SaveChanges();

            IsEnable = false;
            ChangedCourses.Clear();
        }

        public string GetPerson(string personId)
        {
            var person = multimediaEntities.Person.FirstOrDefault(s => s.PersonId == personId);
            if (person != null)
                return person.Name;
            else
                return null;
        }

        public void UpdatePerson(string field)
        {
            if (SelectedCourseEx == null)
                return;
            string name = string.Empty;
            int index = 0;
            CourseExs.BeginUpdate();
            switch (field)
            {
                case "Name1":
                    name = GetPerson(SelectedCourseEx.Name1);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.Name1 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId1 = SelectedCourseEx.Name1;
                        SelectedCourseEx.Name1 = name;
                        index = 1;
                    }
                    break;
                case "Name2":
                    name = GetPerson(SelectedCourseEx.Name2);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.Name2 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId2 = SelectedCourseEx.Name2;
                        SelectedCourseEx.Name2 = name;
                        index = 2;
                    }
                    break;
                case "Name3":
                    name = GetPerson(SelectedCourseEx.Name3);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.Name3 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId3 = SelectedCourseEx.Name3;
                        SelectedCourseEx.Name3 = name;
                        index = 3;
                    }
                    break;
                case "Name4":
                    name = GetPerson(SelectedCourseEx.Name4);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.Name4 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId4 = SelectedCourseEx.Name4;
                        SelectedCourseEx.Name4 = name;
                        index = 4;
                    }
                    break;
                case "Name5":
                    name = GetPerson(SelectedCourseEx.Name5);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.Name5 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId5 = SelectedCourseEx.Name5;
                        SelectedCourseEx.Name5 = name;
                        index = 5;
                    }
                    break;
                case "Name6":
                    name = GetPerson(SelectedCourseEx.Name6);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.Name6 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId6 = SelectedCourseEx.Name6;
                        SelectedCourseEx.Name6 = name;
                        index = 6;
                    }
                    break;
                case "Name7":
                    name = GetPerson(SelectedCourseEx.Name7);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.Name7 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId7 = SelectedCourseEx.Name7;
                        SelectedCourseEx.Name7 = name;
                        index = 7;
                    }
                    break;
                default:
                    if (field.IndexOf("CourseName") >= 0)
                    {
                        index = field.Replace("CourseName", "").ToInt();
                    }
                    break;
            }
            CourseExs.EndUpdate();
            if (index > 0)
            {
                switch (index)
                {
                    case 1:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId1, SelectedCourseEx.Name1,
                            SelectedCourseEx.CourseName1);
                        break;
                    case 2:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId2, SelectedCourseEx.Name2,
                            SelectedCourseEx.CourseName2);
                        break;
                    case 3:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId3, SelectedCourseEx.Name3,
                            SelectedCourseEx.CourseName3);
                        break;
                    case 4:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId4, SelectedCourseEx.Name4,
                            SelectedCourseEx.CourseName4);
                        break;
                    case 5:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId5, SelectedCourseEx.Name5,
                            SelectedCourseEx.CourseName5);
                        break;
                    case 6:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId6, SelectedCourseEx.Name6,
                            SelectedCourseEx.CourseName6);
                        break;
                    case 7:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId7, SelectedCourseEx.Name7,
                            SelectedCourseEx.CourseName7);
                        break;
                }

                IsEnable = true;
            }
        }

        private void ChangeSet(byte classOrd, int index, string pid, string pname, string cname)
        {
            CourseChange change = ChangedCourses.FirstOrDefault(s => s.ClassOrd == classOrd && s.Index == index);
            if (change != null)
                ChangedCourses.Remove(change);
            ChangedCourses.Add(new CourseChange()
            {
                ClassOrd = classOrd,
                Index = index,
                PersonId = pid,
                Name = pname,
                CourseName = cname
            });
        }

        [Command]
        public void ImportExcel()
        {
            FileOpen(string.Format("周一({0}) ~ 周日({1})", weeks[0].Value, weeks[6].Value));
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
                        List<ClassRoomEx> classRooms = (from c in multimediaEntities.ClassRoom
                                                        join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                                                        orderby b.id, c.Id
                                                        select new ClassRoomEx()
                                                        {
                                                            Id = c.Id,
                                                            BuildingId = b.id,
                                                            BuildingName = b.BuildingName,
                                                            TerminalId = c.TerminalId,
                                                            RoomName = c.RoomName
                                                        }).ToList();
                        ICollection<StdClassPeriod> stdClassPeriods = multimediaEntities.StdClassPeriod.OrderBy(o => o.ClassOrd).ToList();
                        PropertyInfo[] pis = typeof(CourseEx).GetProperties();
                        List<string> lstCols = new List<string>();
                        int iHeadRows = ExcelHelper.GetHeaderInfo(ExcelHelper.GetJsonHeaderFromString(HeaderCfg), lstCols);
                        IWorkbook workbook = WorkbookFactory.Create(file);
                        foreach (ClassRoomEx classRoom in classRooms)
                        {
                            ChangedCourses.Clear();
                            string sheetName = classRoom.BuildingName + "_" + classRoom.RoomName +
                                "(" + classRoom.Id + ")";
                            ISheet sheet = workbook.GetSheet(sheetName);
                            if (null == sheet)
                            {
                                //如果是覆盖导入，则删除不存在数据的教室课程表
                                if (overwrite)
                                {
                                    multimediaEntities.Database.ExecuteSqlCommand("delete from WeeklyCourseTable where RoomId = {0} and [date] between {1} and {2}",
                                        new object[] { classRoom.Id, weeks[0].Value, weeks[6].Value });
                                }
                                continue;
                            }
                            IEnumerator rows = sheet.GetRowEnumerator();
                            int iRows = iHeadRows;
                            string importDate = "";
                            while (iRows > 0)
                            {
                                rows.MoveNext();
                                //第二行表头
                                if (iHeadRows - iRows == 1)
                                {
                                    importDate = ((HSSFRow)rows.Current).GetCell(0).ToString();
                                }
                                iRows--;
                            }
                            if (weeks[0].Value + "~" + weeks[6].Value != importDate)
                            {
                                throw (new Exception("选择导入数据的时间不正确，导入失败！"));
                            }
                            ICollection<CourseEx> stdRoomCourseTables = new List<CourseEx>();
                            while (rows.MoveNext())
                            {
                                HSSFRow row = (HSSFRow)rows.Current;
                                CourseEx stdRoomCourseTable = new CourseEx();
                                for (int i = 0; i < row.LastCellNum; i++)
                                {
                                    PropertyInfo pi = pis.Where(p => p.Name.Equals(lstCols[i])).FirstOrDefault();
                                    if (null != pi)
                                    {
                                        ICell cell = row.GetCell(i);
                                        if (cell != null)
                                        {
                                            string s = cell.ToString();
                                            pi.SetValue(stdRoomCourseTable, Convert.ChangeType(cell.ToString(), pi.PropertyType), null);
                                        }
                                    }
                                }
                                StdClassPeriod stdClassPeriod = stdClassPeriods.Where(p => p.ClassOrd.Equals(stdRoomCourseTable.ClassOrd)).FirstOrDefault();
                                if (null != stdRoomCourseTable)
                                {
                                    stdRoomCourseTable.BeginTime = stdClassPeriod.BeginTime;
                                    stdRoomCourseTable.EndTime = stdClassPeriod.EndTime;
                                    stdRoomCourseTable.Date1 = weeks[0].Value;
                                    stdRoomCourseTable.Date2 = weeks[1].Value;
                                    stdRoomCourseTable.Date3 = weeks[2].Value;
                                    stdRoomCourseTable.Date4 = weeks[3].Value;
                                    stdRoomCourseTable.Date5 = weeks[4].Value;
                                    stdRoomCourseTable.Date6 = weeks[5].Value;
                                    stdRoomCourseTable.Date7 = weeks[6].Value;
                                }
                                stdRoomCourseTables.Add(stdRoomCourseTable);
                            }
                            //multimediaEntities.Database.ExecuteSqlCommand("delete from WeeklyCourseTable where RoomId = {0} and [date] between {1} and {2}",
                            //    new object[] { classRoom.Id, weeks[0].Value, weeks[6].Value });
                            foreach (CourseEx c in stdRoomCourseTables)
                            {
                                ChangeSet(c.ClassOrd, 1, c.PersonId1, c.Name1, c.CourseName1);
                                ChangeSet(c.ClassOrd, 2, c.PersonId2, c.Name2, c.CourseName2);
                                ChangeSet(c.ClassOrd, 3, c.PersonId3, c.Name3, c.CourseName3);
                                ChangeSet(c.ClassOrd, 4, c.PersonId4, c.Name4, c.CourseName4);
                                ChangeSet(c.ClassOrd, 5, c.PersonId5, c.Name5, c.CourseName5);
                                ChangeSet(c.ClassOrd, 6, c.PersonId6, c.Name6, c.CourseName6);
                                ChangeSet(c.ClassOrd, 7, c.PersonId7, c.Name7, c.CourseName7);
                            }
                            SaveExec(classRoom.Id);
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
            //return new Task<bool>(() =>
            //{
            FileStream stream = File.Create(filename, 4096, FileOptions.Asynchronous);
            try
            {
                JObject jsonHeader = JObject.Parse(HeaderCfg);
                if (jsonHeader == null)
                    return false;
                IWorkbook workbook;
                if (Path.GetExtension(filename).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new XSSFWorkbook();
                }
                else
                {
                    workbook = new HSSFWorkbook();
                }
                ICellStyle cellstyle = workbook.CreateCellStyle();
                ICellStyle titleCellStyle = workbook.CreateCellStyle();
                NPOI.SS.UserModel.IFont titleFont = workbook.CreateFont();
                ICellStyle headerCellStyle = workbook.CreateCellStyle();
                NPOI.SS.UserModel.IFont headerFont = workbook.CreateFont();
                List<ClassRoomEx> classRooms = (from c in multimediaEntities.ClassRoom
                                                join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.id
                                                orderby b.id, c.Id
                                                select new ClassRoomEx()
                                                {
                                                    Id = c.Id,
                                                    BuildingId = b.id,
                                                    BuildingName = b.BuildingName,
                                                    TerminalId = c.TerminalId,
                                                    RoomName = c.RoomName
                                                }).ToList();
                ICollection<StdClassPeriod> stdClassPeriods = multimediaEntities.StdClassPeriod.OrderBy(o => o.ClassOrd).ToList();
                foreach (ClassRoomEx classRoom in classRooms)
                {
                    jsonHeader["header"] = "[" + classRoom.BuildingName + "_" + classRoom.RoomName + "]课程表";
                    jsonHeader["cols"][0]["caption"] = weeks[0].Value + "~" + weeks[6].Value;
                    jsonHeader["cols"][1]["caption"] = "星期一(" + weeks[0].Value + ")";
                    jsonHeader["cols"][2]["caption"] = "星期二(" + weeks[1].Value + ")";
                    jsonHeader["cols"][3]["caption"] = "星期三(" + weeks[2].Value + ")";
                    jsonHeader["cols"][4]["caption"] = "星期四(" + weeks[3].Value + ")";
                    jsonHeader["cols"][5]["caption"] = "星期五(" + weeks[4].Value + ")";
                    jsonHeader["cols"][6]["caption"] = "星期六(" + weeks[5].Value + ")";
                    jsonHeader["cols"][7]["caption"] = "星期日(" + weeks[6].Value + ")";
                    string sheetName = classRoom.BuildingName + "_" + classRoom.RoomName +
                        "(" + classRoom.Id + ")";
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    ExcelHelper.SaveModelDataToExcelSheetNPOI(workbook, sheet, cellstyle, titleCellStyle, titleFont, headerCellStyle, headerFont, GetCourses(classRoom.Id).ToList<dynamic>(), jsonHeader);
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

        public static string HeaderCfg
        {
            get
            {
                return @"{""header"":""课程表信息"",""filedcount"":8,""rowspan"":1,
	                        ""cols"":[		
		                        {
			                        ""filedName"":"""",""caption"":"""",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [	
		                                {
			                                ""filedName"":""ClassOrd"",""caption"":""课序"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""BeginTime"",""caption"":""上课\n时间"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""EndTime"",""caption"":""下课\n时间"",""row"":1,""colspan"":1,""rowspan"":1, ""locked"":true
		                                }	
                                    ]
		                        },		
		                        {
			                        ""filedName"":"""",""caption"":""星期一"",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [
		                                {
			                                ""filedName"":""PersonId1"",""caption"":""老师工号"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""Name1"",""caption"":""老师姓名"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""CourseName1"",""caption"":""课程名称"",""row"":1,""colspan"":1,""rowspan"":1
		                                }
                                    ]
		                        },			
		                        {
			                        ""filedName"":"""",""caption"":""星期二"",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [
		                                {
			                                ""filedName"":""PersonId2"",""caption"":""老师工号"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""Name2"",""caption"":""老师姓名"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""CourseName2"",""caption"":""课程名称"",""row"":1,""colspan"":1,""rowspan"":1
		                                }
                                    ]
		                        },			
		                        {
			                        ""filedName"":"""",""caption"":""星期三"",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [
		                                {
			                                ""filedName"":""PersonId3"",""caption"":""老师工号"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""Name3"",""caption"":""老师姓名"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""CourseName3"",""caption"":""课程名称"",""row"":1,""colspan"":1,""rowspan"":1
		                                }
                                    ]
		                        },			
		                        {
			                        ""filedName"":"""",""caption"":""星期四"",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [
		                                {
			                                ""filedName"":""PersonId4"",""caption"":""老师工号"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""Name4"",""caption"":""老师姓名"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""CourseName4"",""caption"":""课程名称"",""row"":1,""colspan"":1,""rowspan"":1
		                                }
                                    ]
		                        },			
		                        {
			                        ""filedName"":"""",""caption"":""星期五"",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [
		                                {
			                                ""filedName"":""PersonId5"",""caption"":""老师工号"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""Name5"",""caption"":""老师姓名"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""CourseName5"",""caption"":""课程名称"",""row"":1,""colspan"":1,""rowspan"":1
		                                }
                                    ]
		                        },			
		                        {
			                        ""filedName"":"""",""caption"":""星期六"",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [
		                                {
			                                ""filedName"":""PersonId6"",""caption"":""老师工号"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""Name6"",""caption"":""老师姓名"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""CourseName6"",""caption"":""课程名称"",""row"":1,""colspan"":1,""rowspan"":1
		                                }
                                    ]
		                        },			
		                        {
			                        ""filedName"":"""",""caption"":""星期日"",""row"":1,""colspan"":3,""rowspan"":1,
                                    ""cols"" : [
		                                {
			                                ""filedName"":""PersonId7"",""caption"":""老师工号"",""row"":1,""colspan"":1,""rowspan"":1
		                                },		
		                                {
			                                ""filedName"":""Name7"",""caption"":""老师姓名"",""row"":1,""colspan"":1,""rowspan"":1
		                                },
		                                {
			                                ""filedName"":""CourseName7"",""caption"":""课程名称"",""row"":1,""colspan"":1,""rowspan"":1
		                                }
                                    ]
		                        }
	                        ]
                        }";
            }
        }
    }

    public class CourseChange
    {
        public byte ClassOrd { get; set; }
        public int Index { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
    }
}
