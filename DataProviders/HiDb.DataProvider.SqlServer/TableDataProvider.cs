using HiDb.DataProvider.Dtos.Tables;
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
        public async Task<List<TableColumnFullOutput>> GetDbColumnListAsync(TableColumnInput input,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync<TableColumnFullOutput>(@$"SELECT 
                                COLUMN_NAME AS Name,
                                (SELECT count(1)
                                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                WHERE CONSTRAINT_CATALOG = '{input.DataBase}'  
                                AND TABLE_SCHEMA = '{input.Mode}'
                                AND TABLE_NAME = '{input.Table}'
                                AND COLUMN_NAME = c.COLUMN_NAME ) as IsKey,
                                (case when (SELECT CONSTRAINT_NAME
                                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                WHERE CONSTRAINT_CATALOG = '{input.DataBase}'  
                                AND TABLE_SCHEMA = '{input.Mode}'
                                AND TABLE_NAME = '{input.Table}'
                                AND COLUMN_NAME = c.COLUMN_NAME ) is not null then 1 else 0 end) as IsForeignKey,
                                DATA_TYPE AS Type,
                                IS_NULLABLE AS AllowNull,
                                COLUMN_DEFAULT AS DftValue,
                                NUMERIC_PRECISION AS NumericPrecision,
                                ORDINAL_POSITION AS OrderNo,
                                CHARACTER_MAXIMUM_LENGTH AS MaxLength,
                                NUMERIC_SCALE AS NumSize
                            FROM 
                                INFORMATION_SCHEMA.COLUMNS AS c
                            LEFT JOIN 
								sys.extended_properties ep ON ep.major_id = OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME) 
                            AND ep.minor_id = c.ORDINAL_POSITION 
                            AND ep.name = 'MS_Description'
                            WHERE 
                                TABLE_CATALOG = '{input.DataBase}'
                                AND TABLE_SCHEMA = '{input.Mode}'
                                AND TABLE_NAME = '{input.Table}'
                            order by ORDINAL_POSITION;", cancellationToken, input.DataBase);
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
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            return await connection.ExecuteAsync(@$"DROP TABLE {database}.{mode}.{table}") > 1;
        }

        public async Task<bool> UpdateColumnConfigAsync(UpdateTableColumnInput input, CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, input.DataBase);
            var sql = @$"ALTER TABLE [{input.DataBase}].[{input.Mode}].[{input.Table}]
                         ALTER COLUMN [{input.Column}] [{FormatType(input.Type, input.NumericPrecision, input.NumSize)}] {GetRequiredSql(input)} {GetDftValueSql(input)}";
            var res = await connection.ExecuteAsync(sql) > 1;
            if (!res) return res;
            if (!string.IsNullOrWhiteSpace((input.Remark)))
            {
                return await connection.ExecuteAsync(GetUpdateRemarkSql(input)) > 1;
            }
            return res;
        }

        /// <summary>
        /// 获取更新备注sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetUpdateRemarkSql(ChangeTableColumnInput input)
        {
            var sql = @$"EXEC sp_addextendedproperty N'MS_Description', N'{input.Remark}'
                ,N'SCHEMA', N'{input.Mode}',N'TABLE', N'{input.Table}'
                ,N'COLUMN', N'{input.Column}';";
            
            return sql;
        }

        private string GetDftValueSql(ChangeTableColumnInput input)
        {
            return $"{(string.IsNullOrWhiteSpace(input.DftValue) ? "": $" DEFAULT '{input.DftValue}'")}";
        }

        /// <summary>
        /// 获取必填更新sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetRequiredSql(ChangeTableColumnInput input)
        {
            return input.Required ? "NOT NULL" : "NULL";
        }

        private string FormatType(string type, int? numericPrecision, int? numSize)
        {
            if (type.Contains("decimal") || type.Contains("numeric"))
            {
                if (numericPrecision is > 0)
                {
                    if (numSize is > 0)
                    {
                        return type + $"({numericPrecision.Value},{numSize.Value})";
                    }
                    else
                    {
                        return type + $"({numericPrecision.Value})";
                    }
                }
            }
            return type;
        }

        public async Task<bool> ClearTableAsync(string database, string mode, string table,
            CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            return await connection.ExecuteAsync(@$"TRUNCATE TABLE {database}.{mode}.{table}") > 1;
        }

        public async Task<bool> AddColumnConfigAsync(AddTableColumnInput input, CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, input.DataBase);
            var sql = @$"ALTER TABLE [{input.DataBase}].[{input.Mode}].[{input.Table}]
                         ADD [{input.Column}] [{FormatType(input.Type, input.NumericPrecision, input.NumSize)}] {GetRequiredSql(input)} {GetDftValueSql(input)}";
            var res = await connection.ExecuteAsync(sql) > 1;
            if (!res) return res;
            if (!string.IsNullOrWhiteSpace((input.Remark)))
            {
                return await connection.ExecuteAsync(GetUpdateRemarkSql(input)) > 1;
            }
            return res;
        }

        public async Task<bool> DeleteColumnConfigAsync(DeleteTableColumnInput input, CancellationToken cancellationToken = default)
        {
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, input.DataBase);
            var sql = @$"ALTER TABLE [{input.DataBase}].[{input.Mode}].[{input.Table}]
                         DROP COLUMN [{input.Column}]";
            return await connection.ExecuteAsync(sql) > 1;
        }
    }
}
