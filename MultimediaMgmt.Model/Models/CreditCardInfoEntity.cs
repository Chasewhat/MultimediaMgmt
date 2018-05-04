using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class CreditCardInfoEntity
    {
        public string CardCode { get; set; }
        public string CardNumber { get; set; }
        public string UserNumber { get; set; }
        public string UserName { get; set; }
        public string ClassRoomNumber { get; set; }
        public string TechBuild { get; set; }
        public string Location { get; set; }
        public string TerminalCode { get; set; }
        public string TerminalIp { get; set; }
        public string CreditStatus { get; set; }
        public DateTime CreditTime { get; set; }
    }
}
