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


-- Change Size From 100 To 250 For Column CustomerCareIP
ALTER TABLE [dbo].[Settings] ALTER COLUMN [CustomerCareIP] VARCHAR(250) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5dac47b5-8e26-45a3-9bcc-d26de17fe924', 'Setting.dxml', 'Settings', 'CustomerCareIP', 'Alter Column Size', GETDATE(), '-- Change Size From 100 To 250 For Column CustomerCareIPALTER TABLE [dbo].[Settings] ALTER COLUMN [CustomerCareIP] VARCHAR(250) NOT NULL;');


