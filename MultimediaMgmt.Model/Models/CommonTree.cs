using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class CommonTree
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public ImageSource Image { get; set; }
        public List<CommonTree> Items { get; set; }
    }
}
