-- Add New Column With Name RecordType
ALTER TABLE [dbo].[DWObjectTables] ADD [RecordType] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('42290cd1-85f6-464d-b608-c0103860745a', 'DWObjectTable.dxml', 'DWObjectTables', 'RecordType', 'Add Column', GETDATE(), '-- Add New Column With Name RecordTypeALTER TABLE [dbo].[DWObjectTables] ADD [RecordType] VARCHAR(100) NULL;');

-- Add New Column With Name ParentFactCode
ALTER TABLE [dbo].[DWObjectTables] ADD [ParentFactCode] VARCHAR(50) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b1f99daa-56ed-4147-b1b7-a1177a2c1600', 'DWObjectTable.dxml', 'DWObjectTables', 'ParentFactCode', 'Add Column', GETDATE(), '-- Add New Column With Name ParentFactCodeALTER TABLE [dbo].[DWObjectTables] ADD [ParentFactCode] VARCHAR(50) NULL;');


