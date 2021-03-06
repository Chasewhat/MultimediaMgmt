﻿using DevExpress.Mvvm.DataAnnotations;
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
    public class MainMgmtViewModel : BaseViewModel
    {
        //public virtual List<DataStandard> EnergyConsumptions { get; protected set; }
        public virtual List<KeyValuePair<int, string>> Buildings { get; set; }
        public virtual List<KeyValuePair<int, string>> ClassRooms { get; set; }

        public MainMgmtViewModel()
        {
            Buildings = multimediaEntities.ClassroomBuilding.Select(s => new
            {
                Key = s.Id,
                Value = s.BuildingName
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).ToList();
            ClassRooms= multimediaEntities.ClassRoom.Select(s => new
            {
                Key = s.Id,
                Value = s.RoomNum
            }).AsEnumerable().Select(s =>
                            new KeyValuePair<int, string>(s.Key, s.Value)).OrderBy(s=>s.Value).ToList();

            
        }

        public string GetRoomNum(int id)
        {
            ClassRoom room = multimediaEntities.ClassRoom.FirstOrDefault(s => s.Id == id);
            if (room != null)
                return room.RoomNum;
            else
                return "";
        }

        public List<DataStandard> GetPowerData()
        {
            List<DataStandard> temp = new List<DataStandard>();
            Random rd = new Random();
            for (int i = 20; i > 0; i--)
            {
                temp.Add(new DataStandard(DateTime.Now.AddDays(-i), rd.Next(10, 50) + rd.NextDouble()));
            }

            return temp;
        }
    }

    public class DataPie
    {
        public string Key { get; set; }

        public int Value { get; set; }

        public SolidColorBrush PColor { get; set; }

        public DataPie(string key, int value, SolidColorBrush pcolor)
        {
            this.Key = key;
            this.Value = value;
            this.PColor = pcolor;
        }
    }
}
