using System.Collections.Generic;
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
    }
    /// <summary>
    /// 主窗口状态栏信息更改事件
    /// </summary>
    public class StatusInfoChangedEvent : Prism.Events.PubSubEvent<string> { }
}