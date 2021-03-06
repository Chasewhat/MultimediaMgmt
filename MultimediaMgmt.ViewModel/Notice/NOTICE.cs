﻿using System.Collections.Generic;
using Prism.Events;

namespace MultimediaMgmt.ViewModel.Notice
{
    public class NOTICE
    {
        private static EventAggregator EA = new EventAggregator();

        public static TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            return EA.GetEvent<TEventType>();
        }

        /// <summary>
        /// 弹出右下角提示窗
        /// </summary>
        public static void Publish_Notify(Notify notify)
        {
            GetEvent<NotifyShowEvent>().Publish(notify);
        }
    }
    /// <summary>
    /// 主窗口状态栏信息更改事件
    /// </summary>
    public class StatusInfoChangedEvent : Prism.Events.PubSubEvent<string> { }

    /// <summary>
    /// 右下角提示窗事件
    /// </summary>
    public class NotifyShowEvent : Prism.Events.PubSubEvent<Notify> { }

    /// <summary>
    /// 设备状态查询事件
    /// </summary>
    public class EquipmentStatusInquiryEvent : Prism.Events.PubSubEvent<string> { }
}