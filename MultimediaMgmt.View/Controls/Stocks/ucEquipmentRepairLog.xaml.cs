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
    /// ucEquipmentRepairLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentRepairLog : UserControl
    {
        private EquipmentRepairLogViewModel equipmentRepairLogViewModel;
        public ucEquipmentRepairLog()
        {
            InitializeComponent();
            this.DataContext = equipmentRepairLogViewModel = ViewModelSource.Create<EquipmentRepairLogViewModel>();
            equipmentRepairLogViewModel.FileSave = () => { return FileSave(); };
            equipmentRepairLogViewModel.FileOpen = (s) => { FileOpen(s); };
            equipmentRepairLogViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            equipmentRepairLogViewModel.GetExport = (type) =>
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
            wndFileChoose wfc = new wndFileChoose(info, 3);
            bool? rs = wfc.ShowDialog();
            if (rs.HasValue && rs.Value)
            {
                bool isOverride = false;
                string file = string.Empty;
                wfc.GetResult(ref file, ref isOverride);
                equipmentRepairLogViewModel.ImportExcel(file, isOverride);
            }
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentRepairLogViewModel.SelectedEquipmentRepairLog == null)
                return;
            new PopWindows.wndEquipmentRepairLogAddEdit(equipmentRepairLogViewModel.SelectedEquipmentRepairLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentRepairLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentRepairLogAddEdit(0, equipmentRepairLogViewModel.SerialNumber)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentRepairLogViewModel.Query();
        }
    }
}