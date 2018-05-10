using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MultimediaMgmt.View.PopWindows
{
    /// <summary>
    /// Interaction logic for wndFileChoose.xaml
    /// </summary>
    public partial class wndFileChoose : DevExpress.Xpf.Core.DXWindow
    {
        private string showInfo = "";
        public wndFileChoose(string info = "")
        {
            InitializeComponent();
            showInfo = info;
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
    }
}
