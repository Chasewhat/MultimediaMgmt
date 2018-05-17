using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace MultimediaMgmt.Model
{
    public class Constants
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static UserProfile CurrUser = null;

        public static readonly string LogPath = Path.Combine(Environment.CurrentDirectory, "Log");

        /// <summary>
        /// 信号源
        /// </summary>
        public static readonly Dictionary<byte, string> Signals = new Dictionary<byte, string>() {
            { 1,"台式电脑" },
            { 2,"手提电脑" },
            { 3,"展台" },
            { 4,"扩展" }};

        /// <summary>
        /// 性别
        /// </summary>
        public static readonly Dictionary<string, string> Sexs = new Dictionary<string, string>() {
            { "男","男" },
            { "女","女" }};
        /// <summary>
        /// 刷卡类型
        /// </summary>
        public static readonly Dictionary<int, string> SwCardTypes = new Dictionary<int, string>() {
            { 0,"设备刷卡" },
            { 1,"门禁刷卡" }};
        /// <summary>
        /// 设备刷卡状态
        /// </summary>
        public static readonly Dictionary<int, string> CardStatuss = new Dictionary<int, string>() {
            {0,"无效IC卡" },
            {1,"上课刷卡" },
            {2,"下课刷卡" },
            {3,"无权刷卡" },
            {4,"无效刷卡" },
            {5,"已挂失卡" },
            {6,"他人已刷" },
            {7,"卡片锁定" },
            {8,"管理员刷卡" }
        };
        /// <summary>
        /// 门禁刷卡状态
        /// </summary>
        public static readonly Dictionary<int, string> AccessCardStatuss = new Dictionary<int, string>() {
            {0,"无效IC卡" },
            {1,"开门禁" }
        };
        /// <summary>
        /// 卡片类别
        /// </summary>
        public static readonly Dictionary<string, string> CardTypes = new Dictionary<string, string>() {
            {"T","(T)教师卡" },
            {"S","(S)学生卡" },
            {"A","(A)管理员卡" },
            {"L","(L)临时卡" }
        };

        /// <summary>
        /// 日期选项
        /// </summary>
        public static readonly Dictionary<int, string> DateItems = new Dictionary<int, string>() {
            //{ 0,"全部" },
            { 1,"预约日期" },
            { 2,"课程日期" }};
        /// <summary>
        /// 教室选项
        /// </summary>
        public static readonly Dictionary<int, string> RoomItems = new Dictionary<int, string>() {
            //{ 0,"全部教室" },
            { 1,"当前教室" }};
        /// <summary>
        /// 预约状态
        /// </summary>
        public static readonly Dictionary<byte, string> ReserveState = new Dictionary<byte, string>() {
            //{ 2,"全部" },
            { 0,"预约中" },
            { 1,"取消" }};
        /// <summary>
        /// 审批状态
        /// </summary>
        public static readonly Dictionary<byte, string> ApproveState = new Dictionary<byte, string>() {
            //{ 0,"全部" },
            { 0,"待审批" },
            { 1,"已审批" },
            { 2,"未批准" }};
        /// <summary>
        /// 公共图片资源
        /// </summary>
        public static Dictionary<string, ImageSource> Images = new Dictionary<string, ImageSource>()
        {
            { "warnnotify", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/warn1.png") as ImageSource },
            { "errornotify", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/error.png") as ImageSource },
            { "promptnotify", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/prompt.png") as ImageSource },
            { "home16", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/home16.png") as ImageSource },
            { "build16", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/build16.png") as ImageSource },
            { "floor16", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/floor16.png") as ImageSource },
            { "imagePlay", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/play.png") as ImageSource },
            { "imagePause", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/pause.png") as ImageSource },
            { "Systemc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zkc.png") as ImageSource },
            { "Systemo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zko.png") as ImageSource },
            { "FPDc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dsc.png") as ImageSource },
            { "FPDo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dso.png") as ImageSource },
            { "ComputerStatusc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dnc.png") as ImageSource },
            { "ComputerStatuso", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dno.png") as ImageSource },
            { "Projectorc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/tyjc.png") as ImageSource },
            { "Projectoro", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/tyjo.png") as ImageSource },
            { "ProjectorScreenc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mbc.png") as ImageSource },
            { "ProjectorScreeno", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mbo.png") as ImageSource },
            { "Curtainc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/clc.png") as ImageSource },
            { "Curtaino", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/clo.png") as ImageSource },
            { "Lampc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zmc.png") as ImageSource },
            { "Lampo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zmo.png") as ImageSource },
            { "Volumec", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/yxc.png") as ImageSource },
            { "Volumeo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/yxo.png") as ImageSource },
            { "Recordc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/lbc.png") as ImageSource },
            { "Recordo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/lbo.png") as ImageSource },
            { "Lock_Statusc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dgmc.png") as ImageSource },
            { "Lock_Statuso", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dgmo.png") as ImageSource },
            { "Door_Statusc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mjc.png") as ImageSource },
            { "Door_Statuso", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mjo.png") as ImageSource },
            { "Large_Screenc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dpc.png") as ImageSource },
            { "Large_Screeno", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dpo.png") as ImageSource },
            { "ACRelay1c", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dyc.png") as ImageSource },
            { "ACRelay1o", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dyo.png") as ImageSource },
            { "AirConditionerc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/ktc.png") as ImageSource },
            { "AirConditionero", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/kto.png") as ImageSource },
            { "Connect", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/connect.png") as ImageSource },
            { "UnConnect", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/unconnect.png") as ImageSource }
        };
    }
}