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
    public class EquipmentControlViewModel : BaseViewModel
    {
        public virtual ImageSource Status1 { get; set; }
        public virtual ImageSource Status2 { get; set; }
        public virtual ImageSource Status3 { get; set; }
        public virtual ImageSource Status4 { get; set; }
        public virtual ImageSource Status5 { get; set; }
        public virtual ImageSource Status6 { get; set; }
        public virtual ImageSource Status7 { get; set; }
        public virtual ImageSource Status8 { get; set; }
        public virtual ImageSource Status9 { get; set; }

        public virtual string HeadColor { get; set; }

        public EquipmentControlViewModel()
        {
            Status1 = Constants.Images["zko"];
            Status2 = Constants.Images["dsc"];
            Status3 = Constants.Images["dno"];
            Status4 = Constants.Images["tyjo"];
            Status5 = Constants.Images["clc"];
            Status6 = Constants.Images["mjc"];
            Status7 = Constants.Images["dyo"];
            Status8 = Constants.Images["lbc"];
            Status9 = Constants.Images["yxc"];

            Random rd = new Random();
            if (rd.Next(0, 2) > 0)
                HeadColor = "DarkRed";
            else
                HeadColor = "DarkGreen";
        }
    }
}
