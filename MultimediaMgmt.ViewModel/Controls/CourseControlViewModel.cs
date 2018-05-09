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
    public class CourseControlViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<CourseEx> CourseExs { get; set; }
        public virtual CourseEx SelectedCourseEx { get; set; }
        public virtual DateTime? Date { get; set; }

        public virtual string Week1 { get; set; }
        public virtual string Week2 { get; set; }
        public virtual string Week3 { get; set; }
        public virtual string Week4 { get; set; }
        public virtual string Week5 { get; set; }
        public virtual string Week6 { get; set; }
        public virtual string Week7 { get; set; }

        public virtual bool IsChange { get; set; }
        public virtual bool IsEnable { get; set; }

        private List<CourseChange> ChangedCourses = new List<CourseChange>();
        private List<KeyValuePair<int, string>> weeks = new List<KeyValuePair<int, string>>();

        public int RoomId = 0;

        public CourseControlViewModel()
        {
            Date = DateTime.Now.Date;
            IsChange = true;
            IsEnable = false;
        }

        public void NotChange()
        {
            IsChange = false;
        }

        [Command]
        public void Query()
        {
            if (RoomId <= 0 || !Date.HasValue || Date.Value == default(DateTime))
                return;
            IsEnable = false;
            ChangedCourses.Clear();
            //先获取WeeklyCourseTable
            weeks = GetCurrWeek();
            var weekCourses = (from w in multimediaEntities.WeeklyCourseTable
                               join p in multimediaEntities.Person on w.PersonId equals p.PersonId into temp
                               from t in temp.DefaultIfEmpty()
                               where w.RoomId == RoomId
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
                             where s.RoomId == RoomId
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
                           RoomId = RoomId,
                           ClassOrd = p.ClassOrd,
                           BeginTime = p.BeginTime,
                           EndTime = p.EndTime,
                           PersonId1 = weekCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           PersonName1 = weekCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName1 = weekCourses.Where(s => s.DayOfWeek == 1 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId2 = weekCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           PersonName2 = weekCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName2 = weekCourses.Where(s => s.DayOfWeek == 2 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId3 = weekCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           PersonName3 = weekCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName3 = weekCourses.Where(s => s.DayOfWeek == 3 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId4 = weekCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           PersonName4 = weekCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName4 = weekCourses.Where(s => s.DayOfWeek == 4 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId5 = weekCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           PersonName5 = weekCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName5 = weekCourses.Where(s => s.DayOfWeek == 5 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId6 = weekCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           PersonName6 = weekCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName6 = weekCourses.Where(s => s.DayOfWeek == 6 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault(),
                           PersonId7 = weekCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.PersonId).FirstOrDefault(),
                           PersonName7 = weekCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.Name).FirstOrDefault(),
                           CourseName7 = weekCourses.Where(s => s.DayOfWeek == 7 && s.ClassOrd == p.ClassOrd).Select(s => s.CourseName).FirstOrDefault()
                       });
            CourseExs = data.OrderBy(s => s.ClassOrd).ToSmartObservableCollection();
        }

        private List<KeyValuePair<int, string>> GetCurrWeek()
        {
            DateTime week1 = Date.Value.AddDays(1 -
                (Date.Value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)Date.Value.DayOfWeek));//本周周一
            List<KeyValuePair<int, string>> weeks = new List<KeyValuePair<int, string>>();
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

            return weeks;
        }

        [Command]
        public void Cancel()
        {
            Query();
        }

        [Command]
        public void Save()
        {
            if (RoomId <= 0 || ChangedCourses.Count<=0)
                return;
            var courses = multimediaEntities.WeeklyCourseTable.AsEnumerable();
            var period = multimediaEntities.StdClassPeriod.AsEnumerable();
            StdClassPeriod per = null;
            string date = string.Empty;
            foreach (CourseChange cc in ChangedCourses)
            {
                per = period.FirstOrDefault(s => s.ClassOrd == cc.ClassOrd);
                date = weeks[cc.Index - 1].Value;
                if (per == null||string.IsNullOrEmpty(date))
                    continue;
                WeeklyCourseTable wct = courses.FirstOrDefault(s => s.RoomId == RoomId &&
                s.ClassOrd == cc.ClassOrd && s.Date == date);
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
                        RoomId = RoomId,
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
                case "PersonName1":
                    name = GetPerson(SelectedCourseEx.PersonName1);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.PersonName1 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId1 = SelectedCourseEx.PersonName1;
                        SelectedCourseEx.PersonName1 = name;
                        index = 1;
                    }
                    break;
                case "PersonName2":
                    name = GetPerson(SelectedCourseEx.PersonName2);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.PersonName2 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId2 = SelectedCourseEx.PersonName2;
                        SelectedCourseEx.PersonName2 = name;
                        index = 2;
                    }
                    break;
                case "PersonName3":
                    name = GetPerson(SelectedCourseEx.PersonName3);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.PersonName3 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId3 = SelectedCourseEx.PersonName3;
                        SelectedCourseEx.PersonName3 = name;
                        index = 3;
                    }
                    break;
                case "PersonName4":
                    name = GetPerson(SelectedCourseEx.PersonName4);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.PersonName4 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId4 = SelectedCourseEx.PersonName4;
                        SelectedCourseEx.PersonName4 = name;
                        index = 4;
                    }
                    break;
                case "PersonName5":
                    name = GetPerson(SelectedCourseEx.PersonName5);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.PersonName5 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId5 = SelectedCourseEx.PersonName5;
                        SelectedCourseEx.PersonName5 = name;
                        index = 5;
                    }
                    break;
                case "PersonName6":
                    name = GetPerson(SelectedCourseEx.PersonName6);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.PersonName6 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId6 = SelectedCourseEx.PersonName6;
                        SelectedCourseEx.PersonName6 = name;
                        index = 6;
                    }
                    break;
                case "PersonName7":
                    name = GetPerson(SelectedCourseEx.PersonName7);
                    if (string.IsNullOrEmpty(name))
                    {
                        SelectedCourseEx.PersonName7 = "";
                    }
                    else
                    {
                        SelectedCourseEx.PersonId7 = SelectedCourseEx.PersonName7;
                        SelectedCourseEx.PersonName7 = name;
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
                            SelectedCourseEx.PersonId1, SelectedCourseEx.PersonName1, 
                            SelectedCourseEx.CourseName1);
                        break;
                    case 2:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId2, SelectedCourseEx.PersonName2,
                            SelectedCourseEx.CourseName2);
                        break;
                    case 3:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId3, SelectedCourseEx.PersonName3,
                            SelectedCourseEx.CourseName3);
                        break;
                    case 4:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId4, SelectedCourseEx.PersonName4,
                            SelectedCourseEx.CourseName4);
                        break;
                    case 5:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId5, SelectedCourseEx.PersonName5,
                            SelectedCourseEx.CourseName5);
                        break;
                    case 6:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId6, SelectedCourseEx.PersonName6,
                            SelectedCourseEx.CourseName6);
                        break;
                    case 7:
                        ChangeSet(SelectedCourseEx.ClassOrd, index,
                            SelectedCourseEx.PersonId7, SelectedCourseEx.PersonName7,
                            SelectedCourseEx.CourseName7);
                        break;
                }

                IsEnable = true;
            }
        }

        private void ChangeSet(byte classOrd,int index,string pid,string pname,string cname)
        {
            CourseChange change = ChangedCourses.FirstOrDefault(s => s.ClassOrd == classOrd && s.Index == index);
            if (change != null)
                ChangedCourses.Remove(change);
            ChangedCourses.Add(new CourseChange() {
                ClassOrd = classOrd,
                Index = index,
                PersonId = pid,
                PersonName = pname,
                CourseName = cname
            });
        }


    }

    public class CourseChange
    {
        public byte ClassOrd { get; set; }
        public int Index { get; set; }
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string CourseName { get; set; }
    }
}
