using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace MultimediaMgmt.Model
{
    public class Constants
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static UserProfile CurrUser = null;

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
        /// 公共图片资源
        /// </summary>
        public static Dictionary<string, ImageSource> Images = new Dictionary<string, ImageSource>()
        {
            { "home16", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/home16.png") as ImageSource },
            { "build16", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/build16.png") as ImageSource },
            { "floor16", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/floor16.png") as ImageSource },
            { "imagePlay", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/play.png") as ImageSource },
            { "imagePause", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/pause.png") as ImageSource },
            { "zkc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zkc.png") as ImageSource },
            { "zko", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zko.png") as ImageSource },
            { "dsc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dsc.png") as ImageSource },
            { "dso", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dso.png") as ImageSource },
            { "dnc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dnc.png") as ImageSource },
            { "dno", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dno.png") as ImageSource },
            { "tyjc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/tyjc.png") as ImageSource },
            { "tyjo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/tyjo.png") as ImageSource },
            { "mbc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mbc.png") as ImageSource },
            { "mbo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mbo.png") as ImageSource },
            { "clc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/clc.png") as ImageSource },
            { "clo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/clo.png") as ImageSource },
            { "zmc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zmc.png") as ImageSource },
            { "zmo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/zmo.png") as ImageSource },
            { "yxc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/yxc.png") as ImageSource },
            { "yxo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/yxo.png") as ImageSource },
            { "lbc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/lbc.png") as ImageSource },
            { "lbo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/lbo.png") as ImageSource },
            { "dgmc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dgmc.png") as ImageSource },
            { "dgmo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dgmo.png") as ImageSource },
            { "mjc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mjc.png") as ImageSource },
            { "mjo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/mjo.png") as ImageSource },
            { "dpc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dpc.png") as ImageSource },
            { "dpo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dpo.png") as ImageSource },
            { "dyc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dyc.png") as ImageSource },
            { "dyo", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/dyo.png") as ImageSource },
            { "ktc", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/ktc.png") as ImageSource },
            { "kto", new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Resources/kto.png") as ImageSource }
        };
    }
}