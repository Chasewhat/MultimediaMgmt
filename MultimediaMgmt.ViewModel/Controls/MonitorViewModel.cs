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

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class MonitorViewModel : BaseViewModel
    {
        public virtual ImageSource Image { get; set; }
        public virtual string MediaUrl { get; set; }
        public MonitorViewModel()
        {
            Image = Constants.Images["imagePlay"];
        }      
    }
}
