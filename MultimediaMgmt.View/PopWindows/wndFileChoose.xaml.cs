using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndFileChoose.xaml
    /// </summary>
    public partial class wndFileChoose : DevExpress.Xpf.Core.DXWindow
    {
        private string showInfo = "";
        private int tpType = 0;
        public wndFileChoose(string info = "", int etype = 0)
        {
            InitializeComponent();
            showInfo = info;
            tpType = etype;
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.isOverrideCk.IsChecked = false;
            if (!string.IsNullOrEmpty(showInfo))
            {
                this.showInfoText.Text = showInfo;
                this.showInfoText.Visibility = Visibility.Visible;
            }
            else
            {
                this.showInfoText.Visibility = Visibility.Collapsed;
            }
        }

        private void txtDataPath_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            fileText.EditValue = dialog.FileName;
        }

        private void btnConfrim_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public void GetResult(ref string filePath, ref bool isOverride)
        {
            filePath = this.fileText.EditValue.ToString();
            isOverride = this.isOverrideCk.IsChecked.Value;
        }

        private void btnTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (tpType == 0)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("当前导入项未配置模板", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Excel 文件(*.xls)|*.xls|Excel 文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string sourcePath = "";
            switch (tpType)
            {
                case 1:
                    sourcePath = Path.Combine(Environment.CurrentDirectory, "ExcelTemplate", "IC卡导入模板.xlsx");
                    break;
                case 2:
                    sourcePath = Path.Combine(Environment.CurrentDirectory, "ExcelTemplate", "设备入库导入模板.xlsx");
                    break;
                case 3:
                    sourcePath = Path.Combine(Environment.CurrentDirectory, "ExcelTemplate", "设备维修导入模板.xlsx");
                    break;
                case 4:
                    sourcePath = Path.Combine(Environment.CurrentDirectory, "ExcelTemplate", "设备借出导入模板.xlsx");
                    break;
                case 5:
                    sourcePath = Path.Combine(Environment.CurrentDirectory, "ExcelTemplate", "设备报废导入模板.xlsx");
                    break;
                case 6:
                    sourcePath = Path.Combine(Environment.CurrentDirectory, "ExcelTemplate", "设备转移导入模板.xlsx");
                    break;

            }
            if (!string.IsNullOrEmpty(sourcePath))
                try
                {
                    File.Copy(sourcePath, dialog.FileName, true);
                }
                catch { }
        }
    }
}
