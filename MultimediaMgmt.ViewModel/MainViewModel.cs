using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;

namespace MultimediaMgmt.ViewModel
{
    [POCOViewModel]
    public class MainViewModel : BaseViewModel
    {
        public virtual SmartObservableCollection<CommonTree> ClassRooms { get; set; }
        public virtual string WaitIndiContent { get; set; }
        public virtual string CurrOper { get; set; }
        public virtual bool IsLoad { get; set; }
        public virtual string UserName { get; set; }

        public Action<int> OperationSelectAction;
        public MainViewModel()
        {
            IsLoad = false;
            if (Constants.CurrUser != null)
                UserName = string.Format("当前用户:{0}", Constants.CurrUser.UserName);
            else
                UserName = "未获取到登录用户";
        }

        [Command]
        public void OperationSelect(string param)
        {
            if (param.IndexOf('|') < 0)
                return;
            WaitIndiContent = "正在加载...";
            IsLoad = true;
            string[] paras = param.Split('|');
            CurrOper = paras[1];
            OperationSelectAction?.Invoke(int.Parse(paras[0]));
            IsLoad = false;
        }
    }
}
