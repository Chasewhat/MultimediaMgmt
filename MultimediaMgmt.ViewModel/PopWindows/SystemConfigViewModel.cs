using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model;
using MultimediaMgmt.Common.Helper;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MultimediaMgmt.ViewModel.PopWindows
{
    [POCOViewModel]
    public class SystemConfigViewModel : BaseViewModel
    {
        public virtual string WebUrl { get; set; }
        protected void OnWebUrlChanged()
        {
            ConfigHelper.Main.WebUrl = WebUrl;
        }

        public SystemConfigViewModel()
        {
            WebUrl = ConfigHelper.Main.WebUrl;
        }
    }
}
