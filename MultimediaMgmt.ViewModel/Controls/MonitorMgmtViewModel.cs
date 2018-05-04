using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class MonitorMgmtViewModel:BaseViewModel
    {
        public Action<int> ShowCountExec;
        public MonitorMgmtViewModel()
        {

        }

        [Command]
        public void ShowCount(int count)
        {
            ShowCountExec?.Invoke(count);
        }
    }
}
