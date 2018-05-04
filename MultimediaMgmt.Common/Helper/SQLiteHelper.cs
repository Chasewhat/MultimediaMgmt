using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Common.Helper
{
    /// <summary>
    /// SQLite数据库DML操作帮助类
    /// </summary>
    public class SQLiteHelper
    {
        /// <summary>
        /// DB连接字符串,从config文件获取
        /// </summary>
        private static string connectionString = "";

        /// <summary>   
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="sql">要执行的增删改的SQL语句。</param>   
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public static int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            return ExecuteNonQuery(connectionString, sql, parameters);
        }

        /// <summary>   
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。   
        /// </summary>   
        /// <param name="connStr">连接字符串。</param>   
        /// <param name="sql">要执行的增删改的SQL语句。</param>   
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public static int ExecuteNonQuery(string connStr, string sql, SQLiteParameter[] parameters)
        {
            int affectedRows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = sql;
                        if (parameters != null && parameters.Length != 0)
                            command.Parameters.AddRange(parameters);
                        affectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception) { }
                }
            }
            return affectedRows;
        }

        /// <summary>  
        /// 批量处理数据操作语句。  
        /// </summary>  
        /// <param name="sql">SQL语句</param>  
        /// <param name="list">SQL语句参数集合</param>  
        /// <exception cref="Exception"></exception>  
        public static void ExecuteNonQueryTran(string sql, List<SQLiteParameter[]> list)
        {
            ExecuteNonQueryTran(connectionString, sql, list);
        }

        /// <summary>  
        /// 批量处理数据操作语句。  
        /// </summary>  
        /// <param name="connStr">连接字符串。</param>   
        /// <param name="sql">SQL语句</param>  
        /// <param name="list">SQL语句参数集合</param>  
        /// <exception cref="Exception"></exception>  
        public static void ExecuteNonQueryTran(string connStr, string sql, List<SQLiteParameter[]> list)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                try { conn.Open(); }
                catch { }
                using (SQLiteTransaction tran = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        try
                        {
                            foreach (var item in list)
                            {
                                cmd.CommandText = sql;
                                if (item != null)
                                    cmd.Parameters.AddRange(item);
                                cmd.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch (Exception) { tran.Rollback(); }
                    }
                }
            }
        }

        public static bool ExecuteSqlListNonQueryTran(List<string> sqlList)
        {
            return ExecuteSqlListNonQueryTran(connectionString, sqlList);
        }

        public static bool ExecuteSqlListNonQueryTran(string connStr, List<string> sqlList)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                try { conn.Open(); }
                catch { }
                using (SQLiteTransaction tran = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        try
                        {
                            foreach (var sql in sqlList)
                            {
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                            }
                            tran.Commit();
                            return true;
                        }
                        catch (Exception) { tran.Rollback(); return false; }
                    }
                }
            }
        }

        /// <summary>  
        /// 执行查询语句，并返回第一个结果。  
        /// </summary>  
        /// <param name="sql">查询语句。</param>  
        /// <returns>查询结果。</returns>  
        /// <exception cref="Exception"></exception>  
        public static string ExecuteScalar(string connStr, string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        if (parameters != null && parameters.Length != 0)
                            cmd.Parameters.AddRange(parameters);
                        object scalar = cmd.ExecuteScalar();
                        return scalar == null ? null : scalar.ToString();
                    }
                    catch { return null; }
                }
            }
        }

        public static string ExecuteScalar(string sql, SQLiteParameter[] parameters)
        {
            return ExecuteScalar(connectionString, sql, parameters);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable。   
        /// </summary>   
        /// <param name="sql">要执行的查询语句。</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public static DataTable ExecuteQuery(string sql, SQLiteParameter[] parameters)
        {
            return ExecuteQuery(connectionString, sql, parameters);
        }

        /// <summary>   
        /// 执行一个查询语句，返回一个包含查询结果的DataTable。   
        /// </summary>   
        /// <param name="connStr">连接字符串。</param>   
        /// <param name="sql">要执行的查询语句。</param>   
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准。</param>   
        /// <returns></returns>   
        /// <exception cref="Exception"></exception>  
        public static DataTable ExecuteQuery(string connStr, string sql, SQLiteParameter[] parameters)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connStr))
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null && parameters.Length != 0)
                        command.Parameters.AddRange(parameters);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable data = new DataTable();
                    try { adapter.Fill(data); }
                    catch (Exception) { }
                    return data;
                }
            }
        }
    }
}