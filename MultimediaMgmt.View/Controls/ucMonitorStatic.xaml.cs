using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucMonitorStatic.xaml 的交互逻辑
    /// </summary>
    public partial class ucMonitorStatic : UserControl
    {
        public ucMonitorStatic()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new System.Uri(@"test.WMV", UriKind.Relative);
            mediaElement.Stop();
            mediaElement.Play();
        }
    }
}