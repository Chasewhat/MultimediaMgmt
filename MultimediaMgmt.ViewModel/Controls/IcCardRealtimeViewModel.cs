using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class IcCardRealtimeViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<CardLogEx> CardLogExs { get; set; }
        public virtual Dictionary<int, string> CardStatuss { get; set; }
        public virtual Dictionary<int, string> SwCardTypes { get; set; }
        public virtual int SelectedSwCardType { get; set; }
        protected void OnSelectedSwCardTypeChanged()
        {
            if (SelectedSwCardType == 1)
                CardStatuss = Constants.AccessCardStatuss;
            else
                CardStatuss = Constants.CardStatuss;
        }

        private IRestConnection restConnection = null;

        public IcCardRealtimeViewModel()
        {
            SwCardTypes = Constants.SwCardTypes;
            CardStatuss = Constants.CardStatuss;
            SelectedSwCardType = 0;
            string url = MultimediaMgmt.Common.Helper.ConfigHelper.Main.WebUrl;
            if (!string.IsNullOrEmpty(url))
                restConnection = new RestConnection(url);
            //每隔30秒刷新一次
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += (s, se) => { Refresh(); };
            timer.Start();
        }

        [Command]
        public void Refresh()
        {
            if (restConnection == null)
                return;
            try
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                int total = 0;
                if (SelectedSwCardType == 1)
                {
                    Collection<CardLogEx> logs =
                         restConnection.GetPageValues<CardLogEx>("api/SwipeAccessCardLog/QueryAccessCardLogs"
                         , 1, 0, 1000000, parameters, ref total);
                    if (logs != null)
                        CardLogExs = logs.ToSmartObservableCollection();              
                }
                else
                {
                    Collection<CardLogEx> logs =
                         restConnection.GetPageValues<CardLogEx>("api/SwipeCardLog/QuerySwipeCardLogs"
                         , 1, 0, 1000000, parameters, ref total);
                    if (logs != null)
                        CardLogExs = logs.ToSmartObservableCollection();
                }
            }
            catch(Exception ex) { }
        }
    }
}
