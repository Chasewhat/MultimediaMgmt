using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class CardLogEx
    {
        public int Id { get; set; }
        public string HexCode { get; set; }
        public string CardNum { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
        public int RoomId { get; set; }
        public string RoomNum { get; set; }
        public string BuildingName { get; set; }
        public string IdentifyMode { get; set; }
        public string Location { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        //A.管理员卡
        //T.教师卡
        //S.学生卡
        public string CardType { get; set; }
        //0、无效IC卡
        //1、上课刷卡
        //2、下课刷卡
        //3、无权刷卡
        //4、无效刷卡，短时间内连续刷卡
        //5、已挂失卡
        //6、教室已被其他人使用
        //7、忘记刷下课卡，卡片锁定
        //8、管理员刷卡
        public int State { get; set; }
        public DateTime? LogTime { get; set; }
    }
}
