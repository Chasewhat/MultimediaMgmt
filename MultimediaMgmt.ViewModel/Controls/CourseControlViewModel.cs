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
        public virtual List<CourseEx> CourseExs { get; set; }
        public virtual CourseEx SelectedCourseEx { get; set; }
        public virtual DateTime? Date { get; set; }

        public virtual string Week1 { get; set; }
        public virtual string Week2 { get; set; }
        public virtual string Week3 { get; set; }
        public virtual string Week4 { get; set; }
        public virtual string Week5 { get; set; }
        public virtual string Week6 { get; set; }
        public virtual string Week7 { get; set; }

        public int RoomId = 0;

        public CourseControlViewModel()
        {
            Date = DateTime.Now.Date;
        }

        [Command]
        public void Query()
        {
            if (RoomId <= 0 || !Date.HasValue || Date.Value == default(DateTime))
                return;
            //先获取WeeklyCourseTable
            List<KeyValuePair<int, string>> weeks = GetCurrWeek();
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
            CourseExs = data.OrderBy(s=>s.ClassOrd).ToList();
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
    }
}
