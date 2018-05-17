using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class IcCardInfo
    {
        public int Id { get; set; }
        public string HexCode { get; set; }
        public string CardNum { get; set; }
        public string PersonId { get; set; }
        public string CardType { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int? FacultyId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Career { get; set; }
    }
}
