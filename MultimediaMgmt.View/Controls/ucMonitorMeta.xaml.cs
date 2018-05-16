﻿using DevExpress.Xpf.Bars;
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
    /// ucMonitorMeta.xaml 的交互逻辑
    /// </summary>
    public partial class ucMonitorMeta : UserControl
    {
        private MonitorViewModel monitorViewModel;
        public delegate void StatusChangedEvent(ucMonitorMeta uc, bool isDetail);
        public event StatusChangedEvent StatusChanged;
        private bool isSet = false;
        public string MediaUrl = string.Empty;
        public int Id = 0;
        private bool isLoad = false;
        public ucMonitorMeta(string info, string mediaUrl, int id)
        {
            InitializeComponent();
            this.DataContext = monitorViewModel = ViewModelSource.Create<MonitorViewModel>();
            monitorInfo.Content = info;
            MediaUrl = mediaUrl;
            Id = id;
        }

        private void volumnChange_EditValueChanged(object sender, RoutedEventArgs e)
        {
            if (vlcPlayer != null)
                vlcPlayer.Volume = int.Parse(this.volumnChange.EditValue.ToString());
        }

        public void Dispose()
        {
            //if (vlcPlayer.SourceProvider.MediaPlayer != null)
            //{
            //if (vlcPlayer.SourceProvider.MediaPlayer.IsPlaying())
            //    vlcPlayer.SourceProvider.MediaPlayer.Pause();
            //vlcPlayer.SourceProvider.MediaPlayer.GetMedia().Dispose();
            //vlcPlayer.SourceProvider.MediaPlayer.Stop();
            //vlcPlayer.SourceProvider.MediaPlayer.Dispose();
            //}
            //vlcPlayer.SourceProvider.Dispose();
            this.volumnChange.EditValue = 0;
            monitorViewModel.Image = Constants.Images["imagePlay"];

            Task.Run(() =>
            {
                vlcPlayer.Dispose();
                Meta.Vlc.Wpf.ApiManager.ReleaseAll();
                GC.Collect();
            });
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
            vlcPlayer.Dispatcher.Invoke(() =>
            {
                if (vlcPlayer.VlcMediaPlayer.Media == null)
                {
                    if (string.IsNullOrEmpty(MediaUrl))
                        return;
                    //vlcPlayer.IsMute = true;
                    vlcPlayer.Volume = 0;
                    vlcPlayer.LoadMedia(new Uri(MediaUrl));
                    vlcPlayer.Play();
                    monitorViewModel.Image = Constants.Images["imagePause"];
                }
                else
                {
                    if (vlcPlayer.VlcMediaPlayer.IsPlaying)
                    {
                        vlcPlayer.PauseOrResume();
                        monitorViewModel.Image = Constants.Images["imagePlay"];
                    }
                    else
                    {
                        vlcPlayer.Play();
                        monitorViewModel.Image = Constants.Images["imagePause"];
                    }
                }
            });
        }
    }

}