﻿using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucIcCardMgmt.xaml 的交互逻辑
    /// </summary>
    public partial class ucIcCardMgmt : UserControl
    {
        public ucIcCardMgmt()
        {
            InitializeComponent();
        }

        private void docGroup_SelectedItemChanged(object sender, DevExpress.Xpf.Docking.Base.SelectedItemChangedEventArgs e)
        {

        }
    }
}