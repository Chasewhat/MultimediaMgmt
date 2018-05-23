using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultimediaMgmt.Common.Extend;
using MultimediaMgmt.Model.Models;
using MultimediaMgmt.Model;
using System.Windows.Media;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace MultimediaMgmt.ViewModel.Controls
{
    [POCOViewModel]
    public class PieControlViewModel : BaseViewModel
    {
        public virtual List<DataPie> Rates { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual bool TitleVisible { get; set; }

        public PieControlViewModel()
        {
            TitleVisible = true;
        }

        public async void Init(int buildingId, int type)
        {
            int count = 0, tcount = 0;
            ClassroomBuilding building = null;
            string url = string.Empty;

            switch (type)
            {
                case 1:
                    building = multimediaEntities.ClassroomBuilding.FirstOrDefault(s => s.Id == buildingId);
                    if (building == null)
                        return;
                    tcount = multimediaEntities.ClassRoom.Where(s => s.BuildingId == buildingId).Count();
                    #region 从Web获取IsConnected状态
                    url = Common.Helper.ConfigHelper.Main.WebUrl;
                    if (!string.IsNullOrEmpty(url))
                    {
                        IRestConnection restConnection = new RestConnection(url);
                        ICollection<WebClassRoom> classrooms = multimediaEntities.ClassRoom.Where(s => s.BuildingId == buildingId).Select(
                            s => new WebClassRoom() { TerminalId = s.TerminalId }).ToList();
                        await Task.Run(() =>
                        {
                            try
                            {
                                JObject jo = restConnection.Post("api/TerminalInfo/QueryLastTerminalInfos", classrooms);
                                if (jo.Value<bool>("success"))
                                {
                                    JArray ja = jo.Value<JArray>("data");
                                    if (ja != null)
                                    {
                                        Collection<WebTerminalInfo> terminalInfos = ja.ToObject<Collection<WebTerminalInfo>>();
                                        count = terminalInfos.Where(s => s.IsConnected).Count();
                                    }
                                }
                            }
                            catch { }
                        });
                    }
                    #endregion

                    //#region 从数据库获取
                    //count = (from c in multimediaEntities.ClassRoom
                    //         join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.Id
                    //         join t in (from tt in multimediaEntities.TerminalInfo
                    //                    group tt by new
                    //                    {
                    //                        tt.TerminalId
                    //                    } into g
                    //                    select g.Where(p => p.LogTime == g.Max(m => m.LogTime)).FirstOrDefault()) on c.TerminalId equals t.TerminalId
                    //         where b.Id == buildingId &&
                    //           t.System.HasValue && t.System.Value
                    //         select c.Id).Count();
                    //#endregion
                    Rates = new List<DataPie>() {
                        new DataPie("在线设备", count, Brushes.DarkGreen),
                        new DataPie("离线设备", tcount-count, Brushes.DarkRed)
                    };
                    Title = string.Format("{0}:{1}%", building.BuildingName,
                        (tcount == 0 ? 0 : Math.Round((double)count / tcount * 100, 2)));
                    break;
                case 2:
                    building = multimediaEntities.ClassroomBuilding.FirstOrDefault(s => s.Id == buildingId);
                    if (building == null)
                        return;
                    tcount = multimediaEntities.ClassRoom.Where(s => s.BuildingId == buildingId).Count();
                    #region 从Web获取IsConnected状态
                    url = Common.Helper.ConfigHelper.Main.WebUrl;
                    if (!string.IsNullOrEmpty(url))
                    {
                        IRestConnection restConnection = new RestConnection(url);
                        ICollection<WebClassRoom> classrooms = multimediaEntities.ClassRoom.Where(s => s.BuildingId == buildingId).Select(
                            s => new WebClassRoom() { TerminalId = s.TerminalId }).ToList();
                        await Task.Run(() =>
                        {
                            try
                            {
                                JObject jo = restConnection.Post("api/TerminalInfo/QueryLastTerminalInfos", classrooms);
                                if (jo.Value<bool>("success"))
                                {
                                    JArray ja = jo.Value<JArray>("data");
                                    if (ja != null)
                                    {
                                        Collection<WebTerminalInfo> terminalInfos = ja.ToObject<Collection<WebTerminalInfo>>();
                                        count = terminalInfos.Where(s => s.IsConnected && s.System.HasValue && s.System.Value).Count();
                                    }
                                }
                            }
                            catch { }
                        });
                    }
                    #endregion
                    //#region 从数据库获取
                    //count = (from c in multimediaEntities.ClassRoom
                    //         join b in multimediaEntities.ClassroomBuilding on c.BuildingId equals b.Id
                    //         join t in (from tt in multimediaEntities.TerminalInfo
                    //                    group tt by new
                    //                    {
                    //                        tt.TerminalId
                    //                    } into g
                    //                    select g.Where(p => p.LogTime == g.Max(m => m.LogTime)).FirstOrDefault()) on c.TerminalId equals t.TerminalId
                    //         where b.Id == buildingId &&
                    //           t.System.HasValue && t.System.Value
                    //         select c.Id).Count();
                    //#endregion
                    Rates = new List<DataPie>() {
                        new DataPie("上课教室", count, Brushes.DarkGreen),
                        new DataPie("未上课教室", tcount-count, Brushes.DarkRed)
                    };
                    Title = string.Format("{0}:{1}%", building.BuildingName,
                        (tcount == 0 ? 0 : Math.Round((double)count / tcount * 100, 2)));
                    break;
                case 3:
                    TitleVisible = false;
                    //EquipmentType etype = multimediaEntities.EquipmentType.AsEnumerable().FirstOrDefault(
                    //    s => s.EquipmentCategory == "中控");
                    //if (etype == null)
                    //{
                    //    tcount =count = 0;
                    //}
                    //else
                    //{
                    //    string typeName = etype.EquipmentName;

                    //实际设备总数=入库设备总数-报废总数
                    tcount = (from e in multimediaEntities.EquipmentInStock
                              join i in multimediaEntities.EquipmentScrapLog on e.SerialNumber equals i.SerialNumber into temp
                              from t in temp.DefaultIfEmpty()
                              where t == null
                              //&& i.Name == typeName
                              select e.ID).Count();
                    //设备在修率=设备在修总数/实际设备总数
                    count = (from e in multimediaEntities.EquipmentRepairLog
                             join i in multimediaEntities.EquipmentInStock on e.SerialNumber equals i.SerialNumber into temp
                             from t in temp.DefaultIfEmpty()
                             where (!e.RepairDate.HasValue || e.RepairDate.HasValue && e.RepairDate.Value > DateTime.Now)
                             //&& i.Name == typeName
                             select e.ID).Count();
                    //}
                    Rates = new List<DataPie>() {
                        new DataPie("正常设备", tcount-count, Brushes.DarkGreen),
                        new DataPie("在修设备", count, Brushes.DarkRed),
                    };
                    break;
            }
        }
    }
}
