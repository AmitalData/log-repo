-- Add New Column With Name TMPersonalAccessExpirationDate
ALTER TABLE [dbo].[Settings] ADD [TMPersonalAccessExpirationDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b0e57a8f-3e01-4bef-8385-ca8de2dc10e4', 'Setting.dxml', 'Settings', 'TMPersonalAccessExpirationDate', 'Add Column', GETDATE(), '-- Add New Column With Name TMPersonalAccessExpirationDateALTER TABLE [dbo].[Settings] ADD [TMPersonalAccessExpirationDate] DATETIME NULL;');


-- Add New Column With Name MainColor
ALTER TABLE [dbo].[TenantManagements] ADD [MainColor] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b7b7873f-862a-492b-9e8f-0ecf45a0a0cb', 'TenantManagement.dxml', 'TenantManagements', 'MainColor', 'Add Column', GETDATE(), '-- Add New Column With Name MainColorALTER TABLE [dbo].[TenantManagements] ADD [MainColor] VARCHAR(100) NULL;');

-- Add New Column With Name SecondaryColor
ALTER TABLE [dbo].[TenantManagements] ADD [SecondaryColor] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('b3734f6e-c686-4a07-ab16-c33e0703c725', 'TenantManagement.dxml', 'TenantManagements', 'SecondaryColor', 'Add Column', GETDATE(), '-- Add New Column With Name SecondaryColorALTER TABLE [dbo].[TenantManagements] ADD [SecondaryColor] VARCHAR(100) NULL;');

-- Add New Column With Name BackgroundId
ALTER TABLE [dbo].[TenantManagements] ADD [BackgroundId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6215761e-cc0e-44c8-b91f-b1596a4a7b74', 'TenantManagement.dxml', 'TenantManagements', 'BackgroundId', 'Add Column', GETDATE(), '-- Add New Column With Name BackgroundIdALTER TABLE [dbo].[TenantManagements] ADD [BackgroundId] VARCHAR(15) NULL;');


