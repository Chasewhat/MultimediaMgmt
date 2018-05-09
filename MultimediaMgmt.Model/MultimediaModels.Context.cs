﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultimediaMgmt.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MultimediaEntities : DbContext
    {
        public MultimediaEntities()
            : base("name=MultimediaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccessCardLog> AccessCardLog { get; set; }
        public virtual DbSet<AlarmInfo> AlarmInfo { get; set; }
        public virtual DbSet<BlackList> BlackList { get; set; }
        public virtual DbSet<ClassGrade> ClassGrade { get; set; }
        public virtual DbSet<ClassRoom> ClassRoom { get; set; }
        public virtual DbSet<ClassroomBuilding> ClassroomBuilding { get; set; }
        public virtual DbSet<ClassRoomPermit> ClassRoomPermit { get; set; }
        public virtual DbSet<ClassroomReservation> ClassroomReservation { get; set; }
        public virtual DbSet<ClassroomReservationApproval> ClassroomReservationApproval { get; set; }
        public virtual DbSet<DisplayConfig> DisplayConfig { get; set; }
        public virtual DbSet<EquipmentInStock> EquipmentInStock { get; set; }
        public virtual DbSet<EquipmentLoanLog> EquipmentLoanLog { get; set; }
        public virtual DbSet<EquipmentRepairLog> EquipmentRepairLog { get; set; }
        public virtual DbSet<EquipmentScrapLog> EquipmentScrapLog { get; set; }
        public virtual DbSet<EquipmentTransferLog> EquipmentTransferLog { get; set; }
        public virtual DbSet<IcCard> IcCard { get; set; }
        public virtual DbSet<LossCard> LossCard { get; set; }
        public virtual DbSet<Majors> Majors { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<ReservationCourseTable> ReservationCourseTable { get; set; }
        public virtual DbSet<StdClassPeriod> StdClassPeriod { get; set; }
        public virtual DbSet<StdCourseTable> StdCourseTable { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<SwipeCardLog> SwipeCardLog { get; set; }
        public virtual DbSet<SysParameter> SysParameter { get; set; }
        public virtual DbSet<TerminalActiveTime> TerminalActiveTime { get; set; }
        public virtual DbSet<TerminalCurrentInfo> TerminalCurrentInfo { get; set; }
        public virtual DbSet<TerminalInfo> TerminalInfo { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<WeeklyCourseTable> WeeklyCourseTable { get; set; }
        public virtual DbSet<LessonLog> LessonLog { get; set; }
        public virtual DbSet<SurveillanceLog> SurveillanceLog { get; set; }
        public virtual DbSet<EquipmentType> EquipmentType { get; set; }
    }
}
