using System;
namespace MultimediaMgmt.Model.Models
{
    public partial class Person
    {
        public int ID { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Sex { get; set; }
        public string FacultyId { get; set; }
        public string ClassId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string Career { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? DepartureDate { get; set; }
    }
}
