﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace MapTestApp
{
    public class OleHelper
    {
        private OleDbConnection StyleConnection;

        #region 构造方法
        private OleHelper()
        {
            #region 初始化连接信息
            string DBPath=Path.Combine(Application.StartupPath,"data.mdb");
             string strConStr= "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + DBPath;
             StyleConnection = new OleDbConnection(strConStr);
            #endregion
        }
        #endregion

        #region 单例
        private static OleHelper _instance = null;

        /// <summary>
        /// PGHelper的实例
        /// </summary>
        public static OleHelper Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new OleHelper();
                }
                return _instance;
            }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 从型号基础库获取数据
        /// </summary>
        /// <param name="SQL">查询的SQL语句</param>
        /// <returns>查询的结果</returns>
        public DataTable GetDataTable(String SQL)
        {
            OleDbCommand cmd = new OleDbCommand(SQL, StyleConnection);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        #endregion

    }
}
