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
    public class ClassRoomViewModel : BaseViewModel
    {
        public virtual List<CommonTree> ClassRooms { get; set; }
        public virtual List<CommonTree> SelectedClassRooms { get; set; }

        public ClassRoomViewModel()
        {
            try
            {
                List<CommonTree> temp = new List<CommonTree>();
                foreach (ClassroomBuilding build in multimediaEntities.ClassroomBuilding)
                {
                    CommonTree tr = new CommonTree()
                    {
                        ID = build.id,
                        Name = build.BuildingName,
                        Image = Constants.Images["build16"],
                        IsChecked = false,
                        Items = new List<CommonTree>()
                    };
                    foreach (var data in multimediaEntities.ClassRoom.Where(r => r.BuildingId == build.id).GroupBy(r => r.Floor))
                    {
                        tr.Items.Add(new CommonTree()
                        {
                            ID = data.Key,
                            Name = string.Format("{0}层", data.Key),
                            Image = Constants.Images["floor16"],
                            IsChecked = false,
                            Items = (data.Select(c => new CommonTree()
                            {
                                ID = c.Id,
                                Name = c.RoomName,
                                Image = Constants.Images["home16"],
                                IsChecked = false,
                                Items = null
                            })).ToList()
                        });
                    }
                    temp.Add(tr);
                }
                ClassRooms = temp;
            }
            catch(Exception ex) { }
            SelectedClassRooms = new List<CommonTree>();
        }
    }
}
