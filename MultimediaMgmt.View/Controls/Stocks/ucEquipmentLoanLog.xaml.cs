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
    /// ucEquipmentLoanLog.xaml 的交互逻辑
    /// </summary>
    public partial class ucEquipmentLoanLog : UserControl
    {
        private EquipmentLoanLogViewModel equipmentLoanLogViewModel;
        public ucEquipmentLoanLog()
        {
            InitializeComponent();
            this.DataContext = equipmentLoanLogViewModel = ViewModelSource.Create<EquipmentLoanLogViewModel>();
            equipmentLoanLogViewModel.FileSave = () => { return FileSave(); };
            equipmentLoanLogViewModel.FileOpen = (s) => { FileOpen(s); };
            equipmentLoanLogViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            equipmentLoanLogViewModel.GetExport = (type) =>
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
            wndFileChoose wfc = new wndFileChoose(info, 4);
            bool? rs = wfc.ShowDialog();
            if (rs.HasValue && rs.Value)
            {
                bool isOverride = false;
                string file = string.Empty;
                wfc.GetResult(ref file, ref isOverride);
                equipmentLoanLogViewModel.ImportExcel(file, isOverride);
            }
        }

        private void Edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (equipmentLoanLogViewModel.SelectedEquipmentLoanLog == null)
                return;
            new PopWindows.wndEquipmentLoanLogAddEdit(equipmentLoanLogViewModel.SelectedEquipmentLoanLog.ID)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentLoanLogViewModel.Query();
        }

        private void Add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            new PopWindows.wndEquipmentLoanLogAddEdit(0, equipmentLoanLogViewModel.SerialNumber)
            {
                Owner = Window.GetWindow(this),
                ShowInTaskbar = false
            }.ShowDialog();
            equipmentLoanLogViewModel.Query();
        }
    }
}