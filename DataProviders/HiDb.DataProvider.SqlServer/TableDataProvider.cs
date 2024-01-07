﻿using HiDb.DataProvider.Dtos.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HiDb.DataProvider.SqlServer
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
            return GetList<TableColumnOutput>(@$"SELECT 
                                COLUMN_NAME AS Name,
                                DATA_TYPE AS Type,
                                IS_NULLABLE AS AllowNullStr
                            FROM 
                                INFORMATION_SCHEMA.COLUMNS
                            WHERE 
                                TABLE_CATALOG = '{input.DataBase}'
                                AND TABLE_SCHEMA = '{input.Mode}'
                                AND TABLE_NAME = '{input.Table}';", input.DataBase);
        }

        public List<TableDbTypeOutput> GetDbTypeList()
        {
            return GetList<TableDbTypeOutput>( @$"SELECT name as Name
                FROM sys.types
                WHERE is_user_defined = 0
                ORDER BY name desc");
        }

        public bool DeleteTable(string database, string mode, string table)
        {
            using var connection = SqlConnectionFactory.Get().CreateConnection();
            return connection.Execute(@$"DROP TABLE {database}.{mode}.{table}") > 1;
        }

        public bool UpdateColumnConfig(UpdateTableColumnInput input)
        {
            using var connection = SqlConnectionFactory.Get().CreateConnection();
            var sql = @$"ALTER TABLE [{input.DataBase}].[{input.Mode}].[{input.Table}];";
            var update = @$"ALTER COLUMN [{input.Column}] [{input.Type}] {(input.Required ? "NOT NULL" : "NULL")}";
            return connection.Execute(sql + update) > 1;
        }
        
        public bool ClearTable(string database, string mode, string table)
        {
            using var connection = SqlConnectionFactory.Get().CreateConnection();
            return connection.Execute(@$"TRUNCATE TABLE {database}.{mode}.{table}") > 1;
        }
    }
}
