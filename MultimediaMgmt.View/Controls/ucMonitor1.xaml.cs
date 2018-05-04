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

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucMonitor1.xaml 的交互逻辑
    /// </summary>
    public partial class ucMonitor1 : UserControl
    {
        public delegate void StatusChangedEvent(ucMonitor1 uc, bool isDetail);
        public event StatusChangedEvent StatusChanged;
        private bool isSet = false;
        public ucMonitor1()
        {
            InitializeComponent();
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
            this.mediaPlay.Glyph = Constants.Images["imagePlay"];
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

        private void play_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (this.vlcTest.SourceProvider.MediaPlayer == null)
            {
                // Default installation path of VideoLAN.LibVLC.Windows
                var libDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
                this.vlcTest.SourceProvider.CreatePlayer(libDirectory);
                this.vlcTest.SourceProvider.MediaPlayer.Play(new Uri("rtsp://184.72.239.149/vod/mp4:BigBuckBunny_115k.mov"));
                this.vlcTest.SourceProvider.disposedValue = false;
                this.vlcTest.SourceProvider.MediaPlayer.Audio.Volume = 0;
                this.mediaPlay.Glyph = Constants.Images["imagePause"];
            }
            else
            {
                if (this.vlcTest.SourceProvider.MediaPlayer.IsPlaying())
                {
                    this.vlcTest.SourceProvider.MediaPlayer.Pause();
                    this.mediaPlay.Glyph = Constants.Images["imagePlay"];
                }
                else
                {
                    this.vlcTest.SourceProvider.MediaPlayer.Play();
                    this.mediaPlay.Glyph = Constants.Images["imagePause"];
                }
            }
        }
    }

}