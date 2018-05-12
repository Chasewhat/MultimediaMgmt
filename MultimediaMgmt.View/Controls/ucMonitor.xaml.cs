using DevExpress.Xpf.Bars;
using MultimediaMgmt.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Vlc.DotNet.Core;
using Vlc.DotNet.Wpf;
using System.Windows.Threading;
using MultimediaMgmt.ViewModel.Controls;
using DevExpress.Mvvm.POCO;
using System.Threading.Tasks;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucMonitor.xaml 的交互逻辑
    /// </summary>
    public partial class ucMonitor : UserControl
    {
        private MonitorViewModel monitorViewModel;
        public delegate void StatusChangedEvent(ucMonitor uc, bool isDetail);
        public event StatusChangedEvent StatusChanged;
        private bool isSet = false;
        public string MediaUrl = string.Empty;
        public int Id = 0;
        public ucMonitor(string info,string mediaUrl,int id)
        {
            InitializeComponent();
            this.DataContext = monitorViewModel = ViewModelSource.Create<MonitorViewModel>();
            monitorInfo.Content = info;
            MediaUrl = mediaUrl;
            Id = id;
        }

        private void volumnChange_EditValueChanged(object sender, RoutedEventArgs e)
        {
            if (this.vlcTest != null && this.vlcTest.SourceProvider != null
                && this.vlcTest.SourceProvider.MediaPlayer != null
                && this.vlcTest.SourceProvider.MediaPlayer.Audio != null)
                this.vlcTest.SourceProvider.MediaPlayer.Audio.Volume = int.Parse(this.volumnChange.EditValue.ToString());
        }

        public void Dispose()
        {
            //if (this.vlcTest.SourceProvider.MediaPlayer != null)
            //{
            //if (this.vlcTest.SourceProvider.MediaPlayer.IsPlaying())
            //    this.vlcTest.SourceProvider.MediaPlayer.Pause();
            //this.vlcTest.SourceProvider.MediaPlayer.GetMedia().Dispose();
            //this.vlcTest.SourceProvider.MediaPlayer.Stop();
            //this.vlcTest.SourceProvider.MediaPlayer.Dispose();
            //}
            //this.vlcTest.SourceProvider.Dispose();
            this.vlcTest.Dispose();
            this.volumnChange.EditValue = 0;
            monitorViewModel.Image = Constants.Images["imagePlay"];
            GC.Collect();
        }

        private void showDetail_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (statusChange.IsChecked == null)
                return;
            if (!isSet)
                StatusChange(statusChange.IsChecked.Value);
            isSet = false;
        }

        private void StatusChange(bool isDetail)
        {
            StatusChangedEvent handler = StatusChanged;
            handler?.Invoke(this, isDetail);
        }

        public void StatusSet()
        {
            isSet = true;
            statusChange.IsChecked = false;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //e.Handled = true;
        }

        public void Play()
        {
            play_ItemClick(null, null);
        }

        private void play_ItemClick(object sender, ItemClickEventArgs e)
        {
            Task.Run(() =>
            {
                PlayTask();
            });         
        }

        private void PlayTask()
        {
            this.vlcTest.Dispatcher.Invoke(() => {
                if (this.vlcTest.SourceProvider.MediaPlayer == null)
                {
                    if (string.IsNullOrEmpty(MediaUrl))
                        return;
                    // Default installation path of VideoLAN.LibVLC.Windows
                    var libDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
                    string[] arguments = { "-I", "--dummy-quiet",
                        "--ignore-config", "--no-video-title","--rtsp-tcp",
                        "--no-sub-autodetect-file", "--loop"};
                    this.vlcTest.SourceProvider.CreatePlayer(libDirectory, arguments);
                    this.vlcTest.SourceProvider.MediaPlayer.Play(new Uri(MediaUrl));
                    this.vlcTest.SourceProvider.disposedValue = false;
                    this.vlcTest.SourceProvider.MediaPlayer.Audio.Volume = 0;
                    this.vlcTest.SourceProvider.MediaPlayer.Audio.IsMute = true;
                    monitorViewModel.Image = Constants.Images["imagePause"];
                }
                else
                {
                    if (this.vlcTest.SourceProvider.MediaPlayer.IsPlaying())
                    {
                        this.vlcTest.SourceProvider.MediaPlayer.Pause();
                        monitorViewModel.Image = Constants.Images["imagePlay"];
                    }
                    else
                    {
                        this.vlcTest.SourceProvider.MediaPlayer.Play();
                        monitorViewModel.Image = Constants.Images["imagePause"];
                    }
                }
            });
        }
    }

}