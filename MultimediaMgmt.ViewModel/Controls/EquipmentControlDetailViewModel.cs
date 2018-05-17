using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using System.Windows.Media;
using MultimediaMgmt.Common.Helper;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class EquipmentControlDetailViewModel : BaseViewModel
    {
        public virtual ClassRoomEx CurrClassRoom { get; set; }
        public virtual Dictionary<byte, string> Signals { get; set; }
        public virtual List<DataStandard> EnergyConsumptions { get; protected set; }
        public Action<string> MessageShow;

        public EquipmentControlDetailViewModel()
        {
            List<DataStandard> temp = new List<DataStandard>();
            Random rd = new Random();
            for (int i = 50; i > 0; i--)
            {
                temp.Add(new DataStandard(DateTime.Now.AddDays(-i), rd.Next(10, 50) + rd.NextDouble()));
            }
            EnergyConsumptions = temp;

            Signals = Constants.Signals;
        }

        public void Init(ClassRoomEx cr)
        {
            if (cr == null)
                return;
            CurrClassRoom = null;
            cr.PropertyChanged -= TerminalSet;
            CurrClassRoom = cr;
            CurrClassRoom.PropertyChanged += TerminalSet;
        }

        public async void TerminalSet(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (CurrClassRoom == null)
                return;
            try
            {
                PropertyInfo pro = CurrClassRoom.GetType().GetProperty(e.PropertyName);
                if (pro == null)
                {
                    MessageShow("获取教室状态属性异常");
                    return;
                }
                bool result = await GetExec(CurrClassRoom.TerminalIp,
                    pro.GetValue(CurrClassRoom, null).ToString(), e.PropertyName);
                if (result)
                    MessageShow("执行设置命令成功!");
                else
                    MessageShow("执行设置命令失败!");
            }
            catch (Exception ex)
            {
                MessageShow(ex.Message);
            }
        }

        private Task<bool> GetExec(string ip, string status, string target)
        {
            return Task.Run<bool>(() =>
             {
                 try
                 {
                     string url = string.Format("http://{0}/TERMINAL_STATUS?{1}={2}",
                         ip, target, status);
                     string response = WebHelper.Get(url, 2000);
                     JObject jo = JObject.Parse(response);
                     if ((bool)jo["success"])
                         return true;
                     else
                         return false;
                 }
                 catch
                 {
                     return false;
                 }
             });
        }
    }
}
