using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MultimediaMgmt.View.PopWindows;

namespace MultimediaMgmt.View.Controls
{
    /// <summary>
    /// ucCourseControl.xaml 的交互逻辑
    /// </summary>
    public partial class ucCourseControl : UserControl
    {
        private CourseControlViewModel courseControlViewModel;
        public ucCourseControl()
        {
            InitializeComponent();
            this.DataContext = courseControlViewModel = ViewModelSource.Create<CourseControlViewModel>();
            courseControlViewModel.FileSave = () => { return FileSave(); };
            courseControlViewModel.FileOpen = (s) => { FileOpen(s); };
            courseControlViewModel.MessageShow = (s) =>
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(s, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
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
            wndFileChoose wfc = new wndFileChoose(info);
            bool? rs = wfc.ShowDialog();
            if (rs.HasValue && rs.Value)
            {
                bool isOverride = false;
                string file = string.Empty;
                wfc.GetResult(ref file, ref isOverride);
                courseControlViewModel.ImportExcel(file, isOverride);
            }
        }

        public void SelectChangedExec(CommonTree classRoom)
        {
            courseControlViewModel.RoomId = classRoom.ID.Value;
            courseControlViewModel.Query();
        }

        public void NotChange()
        {
            courseControlViewModel.NotChange();
        }

        private void gridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            courseControlViewModel.UpdatePerson(e.Column.FieldName);
        }
    }
}