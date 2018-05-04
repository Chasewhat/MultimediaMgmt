using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaMgmt.Model.Models
{
    public class DataStandard
    {
        public DateTime Date { get; set; }

        public double Value { get; set; }

        public DataStandard(DateTime date, double value)
        {
            this.Date = date;
            this.Value = value;
        }

        public DataStandard()
        { }
    }
}
