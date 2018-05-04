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
        public virtual ClassRoomEx CurrClassRoom { get; set; }
        public virtual string ClassRoomInfo { get; set; }
        public virtual string CourseName { get; set; }
        public virtual string PersonName { get; set; }
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

        private int flag = 1;

        public EquipmentControlViewModel()
        {
        }

        public void Init(ClassRoomEx cr)
        {
            if (cr == null)
                return;
            Task.Run(() =>
            {
                flag = 1;
                CurrClassRoom = cr;
                ClassRoomInfo = string.Format("{0}{1}", cr.BuildingName, cr.TerminalId);
                CourseName = cr.CourseName;
                PersonName = cr.PersonName;
                if (cr.System.HasValue && cr.System.Value)
                {
                    HeadColor = "DarkGreen";
                    Status1 = Constants.Images["Systemo"];
                }
                else
                {
                    HeadColor = "DarkRed";
                    Status1 = Constants.Images["Systemc"];
                }

                flag++;
                DisplayConfig dc = multimediaEntities.DisplayConfig.FirstOrDefault(s => s.TerminalId == cr.TerminalId);
                if (dc != null)
                {
                    if (dc.FPD.HasValue && dc.FPD.Value)
                    {
                        StatusSet((cr.FPD.HasValue && cr.FPD.Value) ?
                            "FPDo" : "FPDc");
                    }
                    if (dc.ComputerStatus.HasValue && dc.ComputerStatus.Value)
                    {
                        StatusSet((cr.ComputerStatus.HasValue && cr.ComputerStatus.Value) ?
                            "ComputerStatuso" : "ComputerStatusc");
                    }
                    if (dc.Projector.HasValue && dc.Projector.Value)
                    {
                        StatusSet((cr.Projector.HasValue && cr.Projector.Value) ?
                            "Projectoro" : "Projectorc");
                    }
                    if (dc.ProjectionScreen.HasValue && dc.ProjectionScreen.Value)
                    {
                        StatusSet((cr.ProjectorScreen.HasValue && cr.ProjectorScreen.Value) ?
                            "ProjectorScreeno" : "ProjectorScreenc");
                    }
                    if (dc.Curtain.HasValue && dc.Curtain.Value)
                    {
                        StatusSet((cr.Curtain.HasValue && cr.Curtain.Value) ?
                            "Curtaino" : "Curtainc");
                    }
                    if (dc.Lamp.HasValue && dc.Lamp.Value)
                    {
                        StatusSet((cr.Lamp.HasValue && cr.Lamp.Value) ?
                            "Lampo" : "Lampc");
                    }
                    if (dc.Volume.HasValue && dc.Volume.Value)
                    {
                        StatusSet((cr.Volume.HasValue && cr.Volume.Value) ?
                            "Volumeo" : "Volumec");
                    }
                    if (dc.Record.HasValue && dc.Record.Value)
                    {
                        StatusSet((cr.Record.HasValue && cr.Record.Value) ?
                            "Recordo" : "Recordc");
                    }
                    if (dc.LockStatus.HasValue && dc.LockStatus.Value)
                    {
                        StatusSet((cr.Lock_Status.HasValue && cr.Lock_Status.Value) ?
                            "Lock_Statuso" : "Lock_Statusc");
                    }
                    if (dc.DoorStatus.HasValue && dc.DoorStatus.Value)
                    {
                        StatusSet((cr.Door_Status.HasValue && cr.Door_Status.Value) ?
                            "Door_Statuso" : "Door_Statusc");
                    }
                    if (dc.LargeScreen.HasValue && dc.LargeScreen.Value)
                    {
                        StatusSet((cr.Large_Screen.HasValue && cr.Large_Screen.Value) ?
                            "Large_Screeno" : "Large_Screenc");
                    }
                    if (dc.ACRelay1.HasValue && dc.ACRelay1.Value)
                    {
                        StatusSet((cr.ACRelay1.HasValue && cr.ACRelay1.Value) ?
                            "ACRelay1o" : "ACRelay1c");
                    }
                    if (dc.AirConditioner.HasValue && dc.AirConditioner.Value)
                    {
                        StatusSet((cr.AirConitioner.HasValue && cr.AirConitioner.Value) ?
                            "AirConitionero" : "AirConitionerc");
                    }
                }
                else
                {
                    StatusSet((cr.FPD.HasValue && cr.FPD.Value) ?
                        "FPDo" : "FPDc");
                    StatusSet((cr.ComputerStatus.HasValue && cr.ComputerStatus.Value) ?
                        "ComputerStatuso" : "ComputerStatusc");
                    StatusSet((cr.Projector.HasValue && cr.Projector.Value) ?
                        "Projectoro" : "Projectorc");
                    StatusSet((cr.ProjectorScreen.HasValue && cr.ProjectorScreen.Value) ?
                        "ProjectorScreeno" : "ProjectorScreenc");
                    StatusSet((cr.Curtain.HasValue && cr.Curtain.Value) ?
                        "Curtaino" : "Curtainc");
                    StatusSet((cr.Lamp.HasValue && cr.Lamp.Value) ?
                        "Lampo" : "Lampc");
                    StatusSet((cr.Volume.HasValue && cr.Volume.Value) ?
                        "Volumeo" : "Volumec");
                    StatusSet((cr.Record.HasValue && cr.Record.Value) ?
                        "Recordo" : "Recordc");
                }
            });          
        }

        public void StatusSet(string image)
        {
            if (flag > 9)
                return;
            switch (flag)
            {
                case 1:
                    Status1 = Constants.Images[image];
                    break;
                case 2:
                    Status2 = Constants.Images[image];
                    break;
                case 3:
                    Status3 = Constants.Images[image];
                    break;
                case 4:
                    Status4 = Constants.Images[image];
                    break;
                case 5:
                    Status5 = Constants.Images[image];
                    break;
                case 6:
                    Status6 = Constants.Images[image];
                    break;
                case 7:
                    Status7 = Constants.Images[image];
                    break;
                case 8:
                    Status8 = Constants.Images[image];
                    break;
                case 9:
                    Status9 = Constants.Images[image];
                    break;
            }
            flag++;
        }
    }
}
