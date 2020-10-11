-- Drop Column DefultAttachmentsXML
EXEC SP_RENAME 'dbo.DocumentTypes.DefultAttachmentsXML', 'Drop_DefultAttachmentsXML', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a655fd19-4192-45ee-ad20-a1808f51b78c', 'DocumentType.dxml', 'DocumentTypes', 'DefultAttachmentsXML', 'Drop Column', GETDATE(), '-- Drop Column DefultAttachmentsXMLEXEC SP_RENAME ''dbo.DocumentTypes.DefultAttachmentsXML'', ''Drop_DefultAttachmentsXML'', ''COLUMN'';');


-- Add New Column With Name DefultAttachmentsXML
ALTER TABLE [dbo].[DocumentTypeTemplates] ADD [DefultAttachmentsXML] NVARCHAR(MAX) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fc598053-9dab-4cd4-80e3-8d20931d5240', 'DocumentTypeTemplate.dxml', 'DocumentTypeTemplates', 'DefultAttachmentsXML', 'Add Column', GETDATE(), '-- Add New Column With Name DefultAttachmentsXMLALTER TABLE [dbo].[DocumentTypeTemplates] ADD [DefultAttachmentsXML] NVARCHAR(MAX) NULL;');


