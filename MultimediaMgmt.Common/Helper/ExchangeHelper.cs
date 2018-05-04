using System;

namespace Common.Helper
{
    /// <summary>
    /// 交易所前缀解析帮助类
    /// </summary>
    public class ExchangeHelper
    {
        /// <summary>
        /// 上交所证券代码（前3位）
        /// </summary>
        private static readonly int[] SHDM = { 120, 122, 124, 127, 130, 132, 136, 190, 201, 202, 204, 500, 501, 502, 510, 511, 512, 513, 518, 519, 600, 601, 603, 900 };
        /// <summary>
        /// 根据证券代码获取交易所标示,0--深交所/1--上交所
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <returns></returns>
        public static int GetIDByCode(string code)
        {
            return Array.IndexOf(SHDM, int.Parse(code.Substring(0, 3))) > -1 ? 1 : 0;
        }
        /// <summary>
        /// 根据证券代码获取交易所前缀,SH/SZ
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <returns></returns>
        public static string GetExchangeByCode(string code)
        {
            return Array.IndexOf(SHDM, int.Parse(code.Substring(0, 3))) > -1 ? "SH" : "SZ";
        }

        public static string GetExchangeByID(int id)
        {
            return id == 1 ? "SH" : "SZ";
        }

        public static int GetIDByExchange(string exchange)
        {
            return exchange.ToUpper() == "SH" ? 1 : 0;
        }
    }
}
