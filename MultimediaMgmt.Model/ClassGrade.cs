//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ClassGrade
    {
        public int ID { get; set; }
        public string ClassID { get; set; }
        public int MajorID { get; set; }
        public Nullable<int> FaucultyID { get; set; }
        public string ClassName { get; set; }
        public int StudentSum { get; set; }
        public Nullable<System.DateTime> EntryTime { get; set; }
        public Nullable<System.DateTime> LeaveTime { get; set; }
    }
}
