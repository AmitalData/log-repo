-- Add New Column With Name WaitingItems
ALTER TABLE [dbo].[BatchServicesLogs] ADD [WaitingItems] INT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6aa55822-cd0b-4aab-a94a-6b611e5b3d6c', 'BatchServicesLog.dxml', 'BatchServicesLogs', 'WaitingItems', 'Add Column', GETDATE(), '-- Add New Column With Name WaitingItemsALTER TABLE [dbo].[BatchServicesLogs] ADD [WaitingItems] INT DEFAULT(0) NOT NULL;');

-- Add New Column With Name FailedItems
ALTER TABLE [dbo].[BatchServicesLogs] ADD [FailedItems] INT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c27ef466-9f7a-41b9-b58b-b1a98e6ade0c', 'BatchServicesLog.dxml', 'BatchServicesLogs', 'FailedItems', 'Add Column', GETDATE(), '-- Add New Column With Name FailedItemsALTER TABLE [dbo].[BatchServicesLogs] ADD [FailedItems] INT DEFAULT(0) NOT NULL;');

-- Add New Column With Name RelatedQueueMessage
ALTER TABLE [dbo].[BatchServicesLogs] ADD [RelatedQueueMessage] VARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('69960ffd-8e36-4602-aeb9-bb660abeb425', 'BatchServicesLog.dxml', 'BatchServicesLogs', 'RelatedQueueMessage', 'Add Column', GETDATE(), '-- Add New Column With Name RelatedQueueMessageALTER TABLE [dbo].[BatchServicesLogs] ADD [RelatedQueueMessage] VARCHAR(200) NULL;');


-- Add Default Value For Column IsSharedLogisticsContact
ALTER TABLE [dbo].[ContactActivityLogs] ADD DEFAULT 0 FOR [IsSharedLogisticsContact];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3bc680d8-c44f-4ea3-86de-7bac753f1704', 'ContactActivityLog.dxml', 'ContactActivityLogs', 'IsSharedLogisticsContact', 'Add Default Value', GETDATE(), '-- Add Default Value For Column IsSharedLogisticsContactALTER TABLE [dbo].[ContactActivityLogs] ADD DEFAULT 0 FOR [IsSharedLogisticsContact];');

-- Create Index On ContactActivityLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_Tenant_GMTLogDateTime] ON [dbo].[ContactActivityLogs]([Tenant],[GMTLogDateTime])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0b0037de-bd3d-4899-ad1a-514ce8f62502', 'ContactActivityLog.dxml', 'ContactActivityLogs', 'Tenant,GMTLogDateTime', 'Create Index', GETDATE(), '-- Create Index On ContactActivityLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_Tenant_GMTLogDateTime] ON [dbo].[ContactActivityLogs]([Tenant],[GMTLogDateTime])'');');

-- Create Index On ContactActivityLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_Tenant_IsSharedLogisticsContact] ON [dbo].[ContactActivityLogs]([Tenant],[IsSharedLogisticsContact])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7bfd915e-4194-492c-b18d-f56e015622d6', 'ContactActivityLog.dxml', 'ContactActivityLogs', 'Tenant,IsSharedLogisticsContact', 'Create Index', GETDATE(), '-- Create Index On ContactActivityLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_Tenant_IsSharedLogisticsContact] ON [dbo].[ContactActivityLogs]([Tenant],[IsSharedLogisticsContact])'');');

-- Create Index On ContactActivityLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_GMTLogDateTime] ON [dbo].[ContactActivityLogs]([GMTLogDateTime])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d5ee5b9b-6886-4e46-b956-64e76370ecb3', 'ContactActivityLog.dxml', 'ContactActivityLogs', 'GMTLogDateTime', 'Create Index', GETDATE(), '-- Create Index On ContactActivityLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_GMTLogDateTime] ON [dbo].[ContactActivityLogs]([GMTLogDateTime])'');');

-- Create Index On ContactActivityLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_Tenant_LogDateTime_PartnerTypeId] ON [dbo].[ContactActivityLogs]([Tenant],[LogDateTime],[PartnerTypeId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c4b0fda9-8851-4941-8f4d-f4b6ac575cca', 'ContactActivityLog.dxml', 'ContactActivityLogs', 'Tenant,LogDateTime,PartnerTypeId', 'Create Index', GETDATE(), '-- Create Index On ContactActivityLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ContactActivityLogs_Tenant_LogDateTime_PartnerTypeId] ON [dbo].[ContactActivityLogs]([Tenant],[LogDateTime],[PartnerTypeId])'');');


-- Unset Nullable For Column Exception
ALTER TABLE [dbo].[ErrorLogs] ALTER COLUMN [Exception] VARCHAR(7000) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('d9819bbb-da38-44a2-a34c-1f0796c9080c', 'ErrorLog.dxml', 'ErrorLogs', 'Exception', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column ExceptionALTER TABLE [dbo].[ErrorLogs] ALTER COLUMN [Exception] VARCHAR(7000) NOT NULL;');

-- Create Index On ErrorLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ErrorLogs_LogDate] ON [dbo].[ErrorLogs]([LogDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3a5271ef-8315-4dd5-9edd-b3c599cde62a', 'ErrorLog.dxml', 'ErrorLogs', 'LogDate', 'Create Index', GETDATE(), '-- Create Index On ErrorLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ErrorLogs_LogDate] ON [dbo].[ErrorLogs]([LogDate])'');');

-- Create Index On ErrorLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_ErrorLogs_Tenant] ON [dbo].[ErrorLogs]([Tenant])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7089619d-35f8-4181-b9b8-61c93fc08fca', 'ErrorLog.dxml', 'ErrorLogs', 'Tenant', 'Create Index', GETDATE(), '-- Create Index On ErrorLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_ErrorLogs_Tenant] ON [dbo].[ErrorLogs]([Tenant])'');');


-- Add New Column With Name Reason
ALTER TABLE [dbo].[FailedLoginLogs] ADD [Reason] VARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8aa7af4e-b9f8-444e-a3c4-5368d54b4fd9', 'FailedLoginLog.dxml', 'FailedLoginLogs', 'Reason', 'Add Column', GETDATE(), '-- Add New Column With Name ReasonALTER TABLE [dbo].[FailedLoginLogs] ADD [Reason] VARCHAR(200) NULL;');

-- Create Index On FailedLoginLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_FailedLoginLogs_GMTDateTime] ON [dbo].[FailedLoginLogs]([GMTDateTime])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2ffbabde-ff20-4796-835b-31b6266ea2fb', 'FailedLoginLog.dxml', 'FailedLoginLogs', 'GMTDateTime', 'Create Index', GETDATE(), '-- Create Index On FailedLoginLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_FailedLoginLogs_GMTDateTime] ON [dbo].[FailedLoginLogs]([GMTDateTime])'');');


-- Create Index On FailedTokenLogs Table
EXEC('CREATE NONCLUSTERED INDEX [IX_FailedTokenLogs_GMTDateTime] ON [dbo].[FailedTokenLogs]([GMTDateTime])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('43f0828c-c348-40d5-bf96-a903cea0abca', 'FailedTokenLog.dxml', 'FailedTokenLogs', 'GMTDateTime', 'Create Index', GETDATE(), '-- Create Index On FailedTokenLogs TableEXEC(''CREATE NONCLUSTERED INDEX [IX_FailedTokenLogs_GMTDateTime] ON [dbo].[FailedTokenLogs]([GMTDateTime])'');');


