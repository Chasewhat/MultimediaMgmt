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
    
    public partial class ClassRoom
    {
        public int Id { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string IdentifyMode { get; set; }
        public string RoomNum { get; set; }
        public int BuildingId { get; set; }
        public string VedioAddress { get; set; }
        public Nullable<int> Floor { get; set; }
    
        public virtual ClassroomBuilding ClassroomBuilding { get; set; }
    }
}
