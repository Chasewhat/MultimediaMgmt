using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model;
using MultimediaMgmt.Common.Helper;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MultimediaMgmt.ViewModel.PopWindows
{
    [POCOViewModel]
    public class SurveillanceLogAddEditViewModel : BaseViewModel
    {
        public virtual SurveillanceLog CurrSurveillanceLog { get; set; }
        public virtual string WindowTitle { get; set; }
        public virtual string ButtonContent { get; set; }

        [Required(ErrorMessage = "内容不能为空")]
        public virtual string LogContent { get; set; }
        [Required(ErrorMessage = "日期不能为空")]
        public virtual DateTime LogDate { get; set; }


        public Action CloseWindow;
        public Action<string> MessageShow;

        private int currId = 0;
        public SurveillanceLogAddEditViewModel(int id)
        {
            currId = id;
            if (id > 0)
            {
                CurrSurveillanceLog = multimediaEntities.SurveillanceLog.FirstOrDefault(s => s.ID == id);
                WindowTitle = "巡查日志编辑";
                ButtonContent = "更新";
            }
            else
            {
                WindowTitle = "巡查日志新增";
                ButtonContent = "增加";
            }
            if (CurrSurveillanceLog == null)
                CurrSurveillanceLog = new SurveillanceLog() { LogDate = DateTime.Now.Date };
            LogContent = CurrSurveillanceLog.LogContent;
            LogDate = CurrSurveillanceLog.LogDate;
        }

        [Command]
        public void Confirm()
        {
            if (string.IsNullOrEmpty(LogContent) ||
                LogDate == default(DateTime))
            {
                MessageShow("请确认必填项");
                return;
            }
            try
            {
                CurrSurveillanceLog.LogContent = LogContent;
                CurrSurveillanceLog.LogDate = LogDate;
                if (currId > 0)
                {
                    multimediaEntities.Entry(CurrSurveillanceLog).State = EntityState.Modified;
                    multimediaEntities.SaveChanges();
                }
                else
                {
                    multimediaEntities.SurveillanceLog.Add(CurrSurveillanceLog);
                    multimediaEntities.SaveChanges();
                }
                MessageShow("保存成功!");
            }
            catch (Exception ex)
            {
                MessageShow(string.Format("保存失败:{0}", ex.Message));
            }
            if (currId > 0)
                CloseWindow();
        }
    }
}
