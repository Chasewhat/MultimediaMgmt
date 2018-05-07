using System.ComponentModel;

namespace MultimediaMgmt.ViewModel.Notice
{
    public enum NotifyType
    {
        [Description("提示")]
        Prompt = 0,
        [Description("警告")]
        Warn = 1,
        [Description("错误")]
        Error = 2
    }
    public class Notify
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容描述
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 显示类型
        /// </summary>
        public NotifyType Type { get; set; }
        /// <summary>
        /// 是否允许关闭
        /// </summary>
        public bool AllowClose { get; set; }
        /// <summary>
        /// 显示秒数
        /// </summary>
        public int ShowSeconds { get; set; }

        public Notify(string title, string message, int second = 5, NotifyType type = NotifyType.Prompt, bool allowClose = true)
        {
            this.Title = title;
            this.Message = message;
            this.ShowSeconds = second;
            this.Type = type;
            this.AllowClose = allowClose;
        }
    }
}