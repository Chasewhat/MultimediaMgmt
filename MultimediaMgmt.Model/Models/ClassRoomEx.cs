using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MultimediaMgmt.Model.Models
{
    public class ClassRoomEx: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int Id { get; set; }
        public string TerminalId { get; set; }
        public string TerminalIp { get; set; }
        public string RoomName { get; set; }
        public int Floor { get; set; }
        public int BuildingId { get; set; }
        public string VedioAddress { get; set; }
        public string BuildingName { get; set; }
        public string Location { get; set; }
        public string PersonId { get; set; }
        public string CourseName { get; set; }
        public string PersonName { get; set; }
        public string ClassName { get; set; }
        public int StudentSum { get; set; }
        public int RealStudentSum { get; set; }
        public string Temperature { get; set; }

        private bool? system;
        public bool? System
        {
            get { return system; }
            set
            {
                system = value;
                if (PropertyChanged != null && system.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("System"));
            }
        }
        private bool? fpd;
        public bool? FPD
        {
            get { return fpd; }
            set
            {
                fpd = value;
                if (PropertyChanged != null && fpd.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("FPD"));
            }
        }
        private bool? computerStatus;
        public bool? ComputerStatus
        {
            get { return computerStatus; }
            set
            {
                computerStatus = value;
                if (PropertyChanged != null && computerStatus.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ComputerStatus"));
            }
        }
        private bool? projector;
        public bool? Projector
        {
            get { return projector; }
            set
            {
                projector = value;
                if (PropertyChanged != null && projector.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Projector"));
            }
        }
        private bool? projectorScreen;
        public bool? ProjectorScreen
        {
            get { return projectorScreen; }
            set
            {
                projectorScreen = value;
                if (PropertyChanged != null && projectorScreen.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ProjectorScreen"));
            }
        }
        private bool? curtain;
        public bool? Curtain
        {
            get { return curtain; }
            set
            {
                curtain = value;
                if (PropertyChanged != null && curtain.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Curtain"));
            }
        }
        private bool? lamp;
        public bool? Lamp
        {
            get { return lamp; }
            set
            {
                lamp = value;
                if (PropertyChanged != null && lamp.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Lamp"));
            }
        }
        private bool? volume;
        public bool? Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (PropertyChanged != null && volume.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Volume"));
            }
        }
        private bool? record;
        public bool? Record
        {
            get { return record; }
            set
            {
                record = value;
                if (PropertyChanged != null && record.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Record"));
            }
        }
        private bool? lock_Status;
        public bool? Lock_Status
        {
            get { return lock_Status; }
            set
            {
                lock_Status = value;
                if (PropertyChanged != null && lock_Status.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Lock_Status"));
            }
        }
        private bool? door_Status;
        public bool? Door_Status
        {
            get { return door_Status; }
            set
            {
                door_Status = value;
                if (PropertyChanged != null && door_Status.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Door_Status"));
            }
        }
        private bool? aCRelay1;
        public bool? ACRelay1
        {
            get { return aCRelay1; }
            set
            {
                aCRelay1 = value;
                if (PropertyChanged != null && aCRelay1.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ACRelay1"));
            }
        }
        private bool? large_Screen;
        public bool? Large_Screen
        {
            get { return large_Screen; }
            set
            {
                large_Screen = value;
                if (PropertyChanged != null && large_Screen.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Large_Screen"));
            }
        }
        private bool? airConitioner;
        public bool? AirConitioner
        {
            get { return airConitioner; }
            set
            {
                airConitioner = value;
                if (PropertyChanged != null && airConitioner.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("AirConitioner"));
            }
        }
        private byte? projectionSignal;
        public byte? ProjectionSignal
        {
            get { return projectionSignal; }
            set
            {
                projectionSignal = value;
                if (PropertyChanged != null && projectionSignal.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ProjectionSignal"));
            }
        }
        private byte? computerSignal;
        public byte? ComputerSignal
        {
            get { return computerSignal; }
            set
            {
                computerSignal = value;
                if (PropertyChanged != null && computerSignal.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ComputerSignal"));
            }
        }
        private bool? lAN1;
        public bool? LAN1
        {
            get { return lAN1; }
            set
            {
                lAN1 = value;
                if (PropertyChanged != null && lAN1.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LAN1"));
            }
        }
        private bool? lAN2;
        public bool? LAN2
        {
            get { return lAN2; }
            set
            {
                lAN2 = value;
                if (PropertyChanged != null && lAN2.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LAN2"));
            }
        }
        private bool? lAN3;
        public bool? LAN3
        {
            get { return lAN3; }
            set
            {
                lAN3 = value;
                if (PropertyChanged != null && lAN3.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LAN3"));
            }
        }
        private bool? lAN4;
        public bool? LAN4
        {
            get { return lAN4; }
            set
            {
                lAN4 = value;
                if (PropertyChanged != null && lAN4.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("LAN4"));
            }
        }
        private bool? alarm_In1;
        public bool? Alarm_In1
        {
            get { return alarm_In1; }
            set
            {
                alarm_In1 = value;
                if (PropertyChanged != null && alarm_In1.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Alarm_In1"));
            }
        }
       
        private bool? alarm_In2;
        public bool? Alarm_In2
        {
            get { return alarm_In2; }
            set
            {
                alarm_In2 = value;
                if (PropertyChanged != null && alarm_In2.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Alarm_In2"));
            }
        }
        private bool? alarm_In3;
        public bool? Alarm_In3
        {
            get { return alarm_In3; }
            set
            {
                alarm_In3 = value;
                if (PropertyChanged != null && alarm_In3.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Alarm_In3"));
            }
        }
        private bool? alarm_In4;
        public bool? Alarm_In4
        {
            get { return alarm_In4; }
            set
            {
                alarm_In4 = value;
                if (PropertyChanged != null && alarm_In4.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Alarm_In4"));
            }
        }
        private bool? alarm_Control;
        public bool? Alarm_Control
        {
            get { return alarm_Control; }
            set
            {
                alarm_Control = value;
                if (PropertyChanged != null && alarm_Control.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Alarm_Control"));
            }
        }
        private bool? iN_STATUS1;
        public bool? IN_STATUS1
        {
            get { return iN_STATUS1; }
            set
            {
                iN_STATUS1 = value;
                if (PropertyChanged != null && iN_STATUS1.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IN_STATUS1"));
            }
        }
        private bool? iN_STATUS2;
        public bool? IN_STATUS2
        {
            get { return iN_STATUS2; }
            set
            {
                iN_STATUS2 = value;
                if (PropertyChanged != null && iN_STATUS2.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IN_STATUS2"));
            }
        }
        private bool? iN_STATUS3;
        public bool? IN_STATUS3
        {
            get { return iN_STATUS3; }
            set
            {
                iN_STATUS3 = value;
                if (PropertyChanged != null && iN_STATUS3.HasValue)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IN_STATUS3"));
            }
        }
        public string HexCode { get; set; }
        public byte? Opereate_Last { get; set; }
        public bool IsConnected { get; set; }
    }
}
