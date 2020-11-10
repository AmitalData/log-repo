-- Add New Column With Name SendInterfaceAutomationSsucceedXml
ALTER TABLE [dbo].[EntityChanges] ADD [SendInterfaceAutomationSsucceedXml] NVARCHAR(MAX) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('df8fcff5-2254-423c-979d-23e094533e0e', 'EntityChange.dxml', 'EntityChanges', 'SendInterfaceAutomationSsucceedXml', 'Add Column', GETDATE(), '-- Add New Column With Name SendInterfaceAutomationSsucceedXmlALTER TABLE [dbo].[EntityChanges] ADD [SendInterfaceAutomationSsucceedXml] NVARCHAR(MAX) NULL;');

-- Drop Column SendInterfacekAutomationSsucceedXml
EXEC SP_RENAME 'dbo.EntityChanges.SendInterfacekAutomationSsucceedXml', 'Drop_SendInterfacekAutomationSsucceedXml', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('82b788a5-0267-4afa-af59-33757e3b48ba', 'EntityChange.dxml', 'EntityChanges', 'SendInterfacekAutomationSsucceedXml', 'Drop Column', GETDATE(), '-- Drop Column SendInterfacekAutomationSsucceedXmlEXEC SP_RENAME ''dbo.EntityChanges.SendInterfacekAutomationSsucceedXml'', ''Drop_SendInterfacekAutomationSsucceedXml'', ''COLUMN'';');


