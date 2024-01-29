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
        public async Task<TableColumnFullOutput> GetDbColumnFullInfoAsync(TableColumnFullInput input,
            CancellationToken cancellationToken = default)
        {
            var use = @$"use [{input.DataBase}];";
            return await GetFirstAsync<TableColumnFullOutput>(use + @$"SELECT 
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
                                AND TABLE_NAME = '{input.Table}';", cancellationToken);
        }

        public async Task<List<TableColumnOutput>> GetDbColumnListAsync(TableColumnInput input,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync<TableColumnOutput>(@$"SELECT 
                                COLUMN_NAME AS Name,
                                DATA_TYPE AS Type,
                                IS_NULLABLE AS AllowNullStr
                            FROM 
                                INFORMATION_SCHEMA.COLUMNS
                            WHERE 
                                TABLE_CATALOG = '{input.DataBase}'
                                AND TABLE_SCHEMA = '{input.Mode}'
                                AND TABLE_NAME = '{input.Table}';", cancellationToken, input.DataBase);
        }

        public async Task<List<TableDbTypeOutput>> GetDbTypeListAsync(CancellationToken cancellationToken = default)
        {
            return await GetListAsync<TableDbTypeOutput>( @$"SELECT name as Name
                FROM sys.types
                WHERE is_user_defined = 0
                ORDER BY name desc", cancellationToken);
        }

        public async Task<bool> DeleteTableAsync(string database, string mode, string table,
            CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken);
            return await connection.ExecuteAsync(@$"DROP TABLE {database}.{mode}.{table}") > 1;
        }

        public async Task<bool> UpdateColumnConfigAsync(UpdateTableColumnInput input, CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken);
            var sql = @$"ALTER TABLE [{input.DataBase}].[{input.Mode}].[{input.Table}]
                         ALTER COLUMN [{input.Column}] [{input.Type}] {(input.Required ? "NOT NULL" : "NULL")}";
            return await connection.ExecuteAsync(sql) > 1;
        }
        
        public async Task<bool> ClearTableAsync(string database, string mode, string table,
            CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken);
            return await connection.ExecuteAsync(@$"TRUNCATE TABLE {database}.{mode}.{table}") > 1;
        }

        public async Task<bool> AddColumnConfigAsync(AddTableColumnInput input, CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken);
            var sql = @$"ALTER TABLE [{input.DataBase}].[{input.Mode}].[{input.Table}]
                         ADD [{input.Column}] [{input.Type}] {(input.Required ? "NOT NULL" : "NULL")}";
            return await connection.ExecuteAsync(sql) > 1;
        }

        public async Task<bool> DeleteColumnConfigAsync(DeleteTableColumnInput input, CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken);
            var sql = @$"ALTER TABLE [{input.DataBase}].[{input.Mode}].[{input.Table}]
                         DROP COLUMN [{input.Column}]";
            return await connection.ExecuteAsync(sql) > 1;
        }
    }
}
