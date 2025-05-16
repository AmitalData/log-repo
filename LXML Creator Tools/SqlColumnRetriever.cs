using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    internal class SqlColumnRetriever
    {
        private readonly string _connectionString;

        public SqlColumnRetriever(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ColumnInfo> GetAllColumns()
        {
            var columns = new List<ColumnInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"SELECT 
                                     C.TABLE_NAME, 
                                     C.COLUMN_NAME, 
                                     C.DATA_TYPE, 
                                     C.IS_NULLABLE, 
                                     C.CHARACTER_MAXIMUM_LENGTH,
                                     C.ORDINAL_POSITION,
                                     CASE WHEN TC.CONSTRAINT_TYPE = 'PRIMARY KEY' THEN 1 ELSE 0 END AS IS_PRIMARY,
                                     CASE WHEN TC.CONSTRAINT_TYPE = 'FOREIGN KEY' THEN 1 ELSE 0 END AS IS_FOREIGN
                                     FROM INFORMATION_SCHEMA.COLUMNS C
                                     LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU
                                         ON C.TABLE_NAME = KCU.TABLE_NAME 
                                         AND C.COLUMN_NAME = KCU.COLUMN_NAME
                                     LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                                         ON KCU.CONSTRAINT_NAME = TC.CONSTRAINT_NAME";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var columnInfo = new ColumnInfo
                            {
                                TableName = reader["TABLE_NAME"].ToString(),
                                ColumnName = reader["COLUMN_NAME"].ToString(),
                                DataType = reader["DATA_TYPE"].ToString(),
                                IsNullable = reader["IS_NULLABLE"].ToString() == "YES",
                                MaxLength = reader["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value
                                            ? (int?)reader["CHARACTER_MAXIMUM_LENGTH"]
                                            : null,
                                OrdinalPosition = (int)reader["ORDINAL_POSITION"],
                                IsPrimary = (int)reader["IS_PRIMARY"] == 1,
								IsForegin = (int)reader["IS_FOREIGN"] == 1
							};
                            columns.Add(columnInfo);
                        }
                    }
                }
            }

            return columns;
        }
    }

    internal class ColumnInfo
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
        public int? MaxLength { get; set; }
        public int OrdinalPosition { get; set; }
        public bool IsPrimary { get; set; }
		public bool IsForegin { get; set; }

	}

	internal class TableRetriever
    {
        private readonly string _connectionString;

        public TableRetriever(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TableInfo> GetAllTables()
        {
            var tables = new List<TableInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT 
                        TABLE_SCHEMA,
                        TABLE_NAME ,
                        TABLE_CATALOG
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_TYPE = 'BASE TABLE'";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tableInfo = new TableInfo
                            {
                                SchemaName = reader["TABLE_SCHEMA"].ToString(),
                                TableName = reader["TABLE_NAME"].ToString(),
                                TableCatalog = reader["TABLE_CATALOG"].ToString()
                            };
                            tables.Add(tableInfo);
                        }
                    }
                }
            }

            return tables;
        }
    }

    internal class TableInfo
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string TableCatalog { get; set; }
    }

    internal class RelationsRetriever
    {
        private readonly string _connectionString;

        public RelationsRetriever(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<RelationInfo> GetAllRelations()
        {
            var relations = new List<RelationInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT distinct
                        FK.TABLE_NAME AS FK_Table,
                        CU.COLUMN_NAME AS FK_Column,
                        PK.TABLE_NAME AS PK_Table,
                        PT.COLUMN_NAME AS PK_Column
                    FROM 
                        INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
                    INNER JOIN 
                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK 
                        ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                    INNER JOIN 
                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK 
                        ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                    INNER JOIN 
                        INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU 
                        ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
                    INNER JOIN 
                        (
                            SELECT 
                                i1.TABLE_NAME, 
                                i2.COLUMN_NAME
                            FROM 
                                INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                            INNER JOIN 
                                INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 
                                ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                            WHERE 
                                i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                        ) PT 
                        ON PT.TABLE_NAME = PK.TABLE_NAME";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var relationInfo = new RelationInfo
                            {
                                ForeignKeyTable = reader["FK_Table"].ToString(),
                                ForeignKeyColumn = reader["FK_Column"].ToString(),
                                PrimaryKeyTable = reader["PK_Table"].ToString(),
                                PrimaryKeyColumn = reader["PK_Column"].ToString()
                            };
                            relations.Add(relationInfo);
                        }
                    }
                }
            }
            string oldHTML = "";
            string newHTML = oldHTML.Replace("url", "new");
            return relations;
        }
    }

    internal class RelationInfo
    {
        public string ForeignKeyTable { get; set; }
        public string ForeignKeyColumn { get; set; }
        public string PrimaryKeyTable { get; set; }
        public string PrimaryKeyColumn { get; set; }
    }
}
