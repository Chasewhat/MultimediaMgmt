using DevExpress.Mvvm.DataAnnotations;
using MultimediaMgmt.Model;
using MultimediaMgmt.ViewModel.Notice;
using System.Windows.Media;

namespace MultimediaMgmt.ViewModel
{
    [POCOViewModel]
    public class NotifyViewModel
    {
        [BindableProperty]
        public virtual string Title { get; set; }

        [BindableProperty]
        public virtual string Message { get; set; }

        [BindableProperty]
        public virtual ImageSource ImgUri { get; set; }

        [BindableProperty]
        public virtual bool AllowClose { get; set; }

        public NotifyViewModel(Notify notify)
        {
            Init(notify);
        }

        private void Init(Notify notify)
        {
            this.Title = notify.Title;
            this.Message = notify.Message;
            this.AllowClose = notify.AllowClose;
            switch (notify.Type)
            {
                case NotifyType.Prompt:
                    ImgUri = Constants.Images["promptnotify"];
                    break;
                case NotifyType.Warn:
                    ImgUri = Constants.Images["warnnotify"];
                    break;
                case NotifyType.Error:
                    ImgUri = Constants.Images["errornotify"];
                    break;
                default:
                    ImgUri = Constants.Images["promptnotify"];
                    break;
            }
        }
    }
}
