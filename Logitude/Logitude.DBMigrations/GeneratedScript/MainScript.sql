-- Rename Column From OnSendPolpulateDateFieldName To OnSendPopulateDateFieldName
EXEC SP_RENAME 'dbo.DocumentTypes.OnSendPolpulateDateFieldName', 'OnSendPopulateDateFieldName', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('23cb4ab2-937f-4e8f-b149-9f95c5d260bb', 'DocumentType.dxml', 'DocumentTypes', 'OnSendPolpulateDateFieldName', 'Rename Column', GETDATE(), '-- Rename Column From OnSendPolpulateDateFieldName To OnSendPopulateDateFieldNameEXEC SP_RENAME ''dbo.DocumentTypes.OnSendPolpulateDateFieldName'', ''OnSendPopulateDateFieldName'', ''COLUMN'';');


