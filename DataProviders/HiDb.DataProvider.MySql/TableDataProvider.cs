﻿using HiDb.DataProvider.Dtos.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.MySql
{
    /// <summary>
    /// TableDataProvider
    /// </summary>
    public class TableDataProvider : MainDataProvider, ITableDataProvider
    {
        public TableColumnFullOutput GetDbColumnFullInfo(TableColumnFullInput input)
        {
            var use = @$"use [{input.DataBase}];";
            return GetFirst<TableColumnFullOutput>(use + @$"SELECT 
                                COLUMN_NAME AS Name,
                                DATA_TYPE AS Type,
                                IS_NULLABLE AS AllowNull,
                                COLUMN_DEFAULT AS DftValue,
                                NUMERIC_PRECISION AS NumericPrecision，
                                ORDINAL_POSITION AS OrderNo,
                                CHARACTER_MAXIMUM_LENGTH AS NumericPrecision,
                                NUMERIC_SCALE AS Size
                            FROM 
                                INFORMATION_SCHEMA.COLUMNS
                            LEFT JOIN 
								sys.extended_properties ep ON ep.major_id = OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME) 
                            AND ep.minor_id = c.ORDINAL_POSITION 
                            AND ep.name = 'MS_Description'
                            WHERE 
                                TABLE_CATALOG = '{input.DataBase}'
                                AND TABLE_SCHEMA = '{input.Mode}'
                                AND TABLE_NAME = '{input.Table}';");
        }

        public List<TableColumnOutput> GetDbColumnList(TableColumnInput input)
        {
            var use = @$"use [{input.DataBase}];";
            return GetList<TableColumnOutput>(use + @$"SELECT 
                                COLUMN_NAME AS Name,
                                DATA_TYPE AS Type,
                                IS_NULLABLE AS AllowNull
                            FROM 
                                INFORMATION_SCHEMA.COLUMNS
                            WHERE 
                                TABLE_CATALOG = '{input.DataBase}'
                                AND TABLE_SCHEMA = '{input.Mode}'
                                AND TABLE_NAME = '{input.Table}';");
        }
    }
}