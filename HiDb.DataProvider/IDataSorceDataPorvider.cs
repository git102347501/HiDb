﻿using HiDb.DataProvider.Dtos.Connect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider
{
    public interface IDataSorceDataPorvider
    {
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public ConnectDbOutput ConnectDb(string dataSource);

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public ConnectDbOutput ConnectDb(ConnectDbInput input);
    }
}
