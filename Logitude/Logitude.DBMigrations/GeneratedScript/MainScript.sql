-- Add New Column With Name DimensionDataViewName
ALTER TABLE [dbo].[DWObjectFields] ADD [DimensionDataViewName] NVARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8b0ab317-6109-4491-8c10-08c7a3875a79', 'DWObjectField.dxml', 'DWObjectFields', 'DimensionDataViewName', 'Add Column', GETDATE(), '-- Add New Column With Name DimensionDataViewNameALTER TABLE [dbo].[DWObjectFields] ADD [DimensionDataViewName] NVARCHAR(200) NULL;');


