using HiDb.DataProvider.Dtos.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using HiDb.DataProvider.MySql.Models;

namespace HiDb.DataProvider.MySql
{
    /// <summary>
    /// TableDataProvider
    /// </summary>
    public class TableDataProvider : MainDataProvider, ITableDataProvider
    {
        public async Task<List<TableColumnFullOutput>> GetDbColumnListAsync(TableColumnInput input,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync<TableColumnFullOutput>(@$"SELECT 
                                COLUMN_NAME AS Name,
                                DATA_TYPE AS Type,
                                CASE COLUMN_KEY
                                    WHEN 'PRI' THEN 1
                                    ELSE 0 END AS IsKey,
                                CASE COLUMN_KEY
                                    WHEN 'MUL' THEN 1
                                    ELSE 0 END AS IsForeignKey,
                                IS_NULLABLE AS AllowNull,
                                COLUMN_DEFAULT AS DftValue,
                                NUMERIC_PRECISION AS NumSize,
                                ORDINAL_POSITION AS OrderNo,
                                CHARACTER_MAXIMUM_LENGTH AS MaxLength,
                                COLUMN_COMMENT AS Remark
                            FROM 
                                INFORMATION_SCHEMA.COLUMNS
                            WHERE 
                                table_schema= '{input.DataBase}'
                                AND table_name= '{input.Table}'
                            order by ORDINAL_POSITION;", cancellationToken, input.DataBase);
        }
        
        public async Task<List<TableDbTypeOutput>> GetDbTypeListAsync(CancellationToken cancellationToken = default)
        {
            var res = await GetListAsync<MySqlDbTypeList>( @$"SELECT DISTINCT COLUMN_TYPE 
                                                    FROM information_schema.COLUMNS;", cancellationToken);
            return res.Select(c => new TableDbTypeOutput() {Name = c.COLUMN_TYPE }).ToList();
        }

        public async Task<bool> DeleteTableAsync(string database, string mode, string table, 
            CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            return await connection.ExecuteAsync(@$"DROP TABLE {database}.{table};") > 1;
        }

        public async Task<bool> UpdateColumnConfigAsync(UpdateTableColumnInput input,
            CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, input.DataBase);
            var sql = @$"ALTER TABLE `{input.DataBase}`.`{input.Table}`
                        MODIFY COLUMN `column` [{input.Type}] {(input.Required ? "NOT NULL" : "NULL")}";
            return await connection.ExecuteAsync(sql) > 1;
        }

        public async Task<bool> ClearTableAsync(string database, string mode, string table,
            CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            return await connection.ExecuteAsync(@$"TRUNCATE TABLE {database}.{table}") > 1;
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
