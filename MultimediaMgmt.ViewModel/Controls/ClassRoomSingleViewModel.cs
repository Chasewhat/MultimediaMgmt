﻿using DevExpress.Mvvm.DataAnnotations;
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
    public class ClassRoomSingleViewModel : BaseViewModel
    {
        public virtual List<CommonTree> ClassRoomSingles { get; set; }
        public virtual List<CommonTree> SelectedClassRoomSingles { get; set; }

        public ClassRoomSingleViewModel()
        {
            try
            {
                List<CommonTree> temp = new List<CommonTree>();
                var rooms = multimediaEntities.ClassRoom.AsEnumerable();
                foreach (ClassroomBuilding build in multimediaEntities.ClassroomBuilding)
                {
                    CommonTree tr = new CommonTree()
                    {
                        ID = build.Id,
                        Name = string.Format("{0}({1})", build.BuildingName,rooms.Where(s=>s.BuildingId==build.Id).Count()),
                        Image = Constants.Images["build16"],
                        IsChecked = false,
                        Items = new List<CommonTree>()
                    };
                    foreach (var data in multimediaEntities.ClassRoom.Where(r => r.BuildingId == build.Id).GroupBy(r => r.Floor))
                    {
                        tr.Items.Add(new CommonTree()
                        {
                            ID = data.Key,
                            Name = string.Format("{0}层({1})", data.Key,data.Count()),
                            Image = Constants.Images["floor16"],
                            IsChecked = false,
                            Items = (data.Select(c => new CommonTree()
                            {
                                ID = c.Id,
                                Name = c.RoomNum,
                                Image = Constants.Images["home16"],
                                IsChecked = false,
                                Items = null
                            })).ToList()
                        });
                    }
                    temp.Add(tr);
                }
                ClassRoomSingles = temp;
            }
            catch { }
            SelectedClassRoomSingles = new List<CommonTree>();
        }
    }
}
