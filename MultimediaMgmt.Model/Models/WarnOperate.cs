using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class WarnOperate: IDXDataErrorInfo
    {
        public int ClassRoomId { get; set; }
        public int BuildingId { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string RoomNum { get; set; }
        public string BuildingName { get; set; }
        public bool? Alarm_In1 { get; set; }
        public bool? Alarm_In2 { get; set; }
        public bool? Alarm_In3 { get; set; }
        public bool? Alarm_In4 { get; set; }
        public bool? Alarm_Control { get; set; }
        public DateTime ReportTime { get; set; }
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
