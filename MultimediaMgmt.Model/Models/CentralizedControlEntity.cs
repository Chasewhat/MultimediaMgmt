using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class CentralizedControlEntity
    {
        public string Code { get; set; }
        public string TechBuild { get; set; }
        public bool ControlSwitch { get; set; }
        public bool AirConditionerSwitch { get; set; }
        public bool LightingSwitch { get; set; }
        public string ExecResult { get; set; }
    }
}
