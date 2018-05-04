using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class WarnInfoEntity
    {
        public string Code { get; set; }
        public string TechBuild { get; set; }
        public string Location { get; set; }
        public string TerminalCode{ get; set; }
        public string TerminalIp { get; set; }
        public string VideoAddress { get; set; }
        public string UseMode { get; set; }
    }
}
