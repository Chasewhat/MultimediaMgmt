using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using MultimediaMgmt.View.PopWindows;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucEquipmentScrapLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentScrapLog : UserControl
    {
        private EquipmentScrapLogViewModel equipmentScrapLogViewModel;
        public ucEquipmentScrapLog()
        {
            InitializeComponent();
            this.DataContext = equipmentScrapLogViewModel = ViewModelSource.Create<EquipmentScrapLogViewModel>();
            equipmentScrapLogViewModel.FileSave = () => { return FileSave(); };
            equipmentScrapLogViewModel.FileOpen = (s) => { FileOpen(s); };
            equipmentScrapLogViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            equipmentScrapLogViewModel.GetExport = (type) =>
            {
                MemoryStream ms = new MemoryStream();
                if (type == 1)
                    gridView.ExportToXlsx(ms);
                else
                    gridView.ExportToXls(ms);
                return ms;
            };
        }

        public string FileSave()
        {
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return null;
            return dialog.FileName;
        }

        public void FileOpen(string info)
        {
            wndFileChoose wfc = new wndFileChoose(info, 5);
            bool? rs = wfc.ShowDialog();
            if (rs.HasValue && rs.Value)
            {
                bool isOverride = false;
                string file = string.Empty;
                wfc.GetResult(ref file, ref isOverride);
                equipmentScrapLogViewModel.ImportExcel(file, isOverride);
            }
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentScrapLogViewModel.SelectedEquipmentScrapLog == null)
                return;
            new PopWindows.wndEquipmentScrapLogAddEdit(equipmentScrapLogViewModel.SelectedEquipmentScrapLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentScrapLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentScrapLogAddEdit(0, equipmentScrapLogViewModel.SerialNumber)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentScrapLogViewModel.Query();
        }
    }
}