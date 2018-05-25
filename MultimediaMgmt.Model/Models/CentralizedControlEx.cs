using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class CentralizedControlEx : IDXDataErrorInfo
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int? Floor { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string RoomName { get; set; }
        public string BuildingName { get; set; }
        public bool? System { get; set; }
        public bool? AirConitioner { get; set; }
        public bool? Lamp { get; set; }
        public string ExecResult { get; set; }
        public bool ExecStatus { get; set; }
        public void GetPropertyError(string propertyName, ErrorInfo info)
        {
            if (propertyName == "ExecResult" && !string.IsNullOrEmpty(ExecResult))
                SetErrorInfo(info, ExecResult,
                    (ExecStatus ? ErrorType.Information : ErrorType.Critical));
        }
        public void GetError(ErrorInfo info)
        {
            return;
        }

        protected void SetErrorInfo(ErrorInfo info, string errorText, ErrorType errorType)
        {
            info.ErrorText = errorText;
            info.ErrorType = errorType;
        }
    }
}
