using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Text;
using NPOI.SS.UserModel;

namespace Common.Helper
{
    /// <summary>
    /// Excel读取帮助类,执行将CSV/Excel文件读取至DataTable中
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 将CSV格式字符流数据读取到DataTable中
        /// </summary>
        /// <param name="path">CSV文件路径</param>
        /// <returns>返回读取了CSV文件的DataTable</returns>
        public static DataTable CSV2DataTable(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    return CSV2DataTable(fs);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将CSV格式字符流数据读取到DataTable中
        /// </summary>
        /// <param name="csvStream">CSV格式字符流</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable CSV2DataTable(Stream csvStream)
        {
            int intColCount = 0;
            bool blnFlag = true;
            DataTable mydt = new DataTable();
            DataColumn mydc;
            DataRow mydr;
            string strline;
            string[] aryline;
            StreamReader mysr = new StreamReader(csvStream, Encoding.Default);
            while ((strline = mysr.ReadLine()) != null)
            {
                List<string> ss = new List<string>();
                aryline = strline.Split(new char[] { ',' });
                if (strline.IndexOf("\"") > -1)
                {
                    for (int i = 0; i < aryline.Length; i++)
                    {
                        if (aryline[i].Contains("\""))
                        {
                            ss.Add(aryline[i].Replace("\"", "") + aryline[++i].Replace("\"", ""));
                            if (i == aryline.Length - 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            ss.Add(aryline[i]);
                        }
                    }
                }
                else
                {
                    ss.AddRange(aryline);
                }
                if (blnFlag)
                {
                    blnFlag = false;
                    intColCount = ss.Count;
                    for (int i = 0; i < intColCount; i++)
                    {
                        mydc = new DataColumn(ss[i].Trim());
                        mydt.Columns.Add(mydc);
                    }
                }
                else
                {
                    if (intColCount == ss.Count)
                    {
                        mydr = mydt.NewRow();
                        for (int i = 0; i < intColCount; i++)
                        {
                            mydr[i] = ss[i].Trim();
                        }
                        mydt.Rows.Add(mydr);
                    }
                }
            }
            return mydt;
        }

        /// <summary>
        /// 将字符串读取到DataTable中
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>返回DataTable</returns>
        public static DataTable Text2DataTable(string text)
        {
            DataTable mydt = new DataTable();

            text = text.Replace("\r\n", "#").Replace("\t", "*");
            string[] rows = text.Split('#');
            for (int i = 0; i < rows.Length; i++)
            {
                string[] cols = rows[i].Split('*');
                if (i == 0)
                {
                    foreach (string col in cols)
                    {
                        DataColumn dc = new DataColumn(col.TrimStart().TrimEnd());
                        mydt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = mydt.NewRow();
                    for (int j = 0; j < cols.Length; j++)
                    {
                        dr[j] = cols[j];
                    }
                    mydt.Rows.Add(dr);
                }
            }
            return mydt;
        }

        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable Excel2DataTable(string fileName, string sheetName, bool isFirstRowColumn)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName))
                {
                    return null;
                }
                //根据指定路径读取文件
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //IWorkbook workbook = new HSSFWorkbook(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null)
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        //由于某些空白列的问题  导致excel列数与dt列数不匹配  导致解析出错
                        //此处循环以两者最小值为循环数
                        var tempCount = Math.Min(data.Columns.Count, cellCount);
                        for (int j = row.FirstCellNum; j < tempCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}