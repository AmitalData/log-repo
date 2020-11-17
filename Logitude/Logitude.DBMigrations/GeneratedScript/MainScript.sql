-- Add New Column With Name SendInterfaceAutomationFailedXml
ALTER TABLE [dbo].[EntityChanges] ADD [SendInterfaceAutomationFailedXml] NVARCHAR(MAX) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e3905bec-0289-4e61-a77c-7cf9a925e845', 'EntityChange.dxml', 'EntityChanges', 'SendInterfaceAutomationFailedXml', 'Add Column', GETDATE(), '-- Add New Column With Name SendInterfaceAutomationFailedXmlALTER TABLE [dbo].[EntityChanges] ADD [SendInterfaceAutomationFailedXml] NVARCHAR(MAX) NULL;');

-- Add New Column With Name SendInterfaceAutomationSsucceedXml
ALTER TABLE [dbo].[EntityChanges] ADD [SendInterfaceAutomationSsucceedXml] NVARCHAR(MAX) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c46309e8-72e9-4282-98dd-9d2bbe7d658b', 'EntityChange.dxml', 'EntityChanges', 'SendInterfaceAutomationSsucceedXml', 'Add Column', GETDATE(), '-- Add New Column With Name SendInterfaceAutomationSsucceedXmlALTER TABLE [dbo].[EntityChanges] ADD [SendInterfaceAutomationSsucceedXml] NVARCHAR(MAX) NULL;');


