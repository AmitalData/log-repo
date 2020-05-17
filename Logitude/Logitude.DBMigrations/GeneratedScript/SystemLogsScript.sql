-- Create New Table With Name DBMigrationsHistory
CREATE TABLE [dbo].[DBMigrationsHistory](
[Id] NVARCHAR(128) NOT NULL,
[DxmlFileName] VARCHAR(500) NOT NULL,
[TableName] VARCHAR(500) NOT NULL,
[ColumnName] VARCHAR(MAX) NULL,
[MigrationType] VARCHAR(50) NOT NULL,
[ExecutionDate] DATETIME NOT NULL,
[MigrationScript] NVARCHAR(MAX) NOT NULL,
CONSTRAINT [PK_DBMigrationsHistory] PRIMARY KEY([Id])
);


