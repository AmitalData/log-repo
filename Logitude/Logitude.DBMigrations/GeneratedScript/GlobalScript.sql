-- Add Default Value For Column GLSHKURL
ALTER TABLE [dbo].[Settings] ADD DEFAULT '0' FOR [GLSHKURL];

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('429325b8-7857-4358-98a0-c02e78efc4d3', 'Setting.dxml', 'Settings', 'GLSHKURL', 'Add Default Value', GETDATE(), '-- Add Default Value For Column GLSHKURLALTER TABLE [dbo].[Settings] ADD DEFAULT ''0'' FOR [GLSHKURL];');

-- Set Nullable For Column GLSHKURL
ALTER TABLE [dbo].[Settings] ALTER COLUMN [GLSHKURL] VARCHAR(1000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bddbd8cb-b9bf-4ef6-ae33-655bb8534b6a', 'Setting.dxml', 'Settings', 'GLSHKURL', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column GLSHKURLALTER TABLE [dbo].[Settings] ALTER COLUMN [GLSHKURL] VARCHAR(1000) NULL;');

-- Set Nullable For Column GLSHKEnv
ALTER TABLE [dbo].[Settings] ALTER COLUMN [GLSHKEnv] VARCHAR(20) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c7f186f5-78f8-4821-b952-a74e2bbd1ec3', 'Setting.dxml', 'Settings', 'GLSHKEnv', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column GLSHKEnvALTER TABLE [dbo].[Settings] ALTER COLUMN [GLSHKEnv] VARCHAR(20) NULL;');


-- Add New Column With Name MainColor
ALTER TABLE [dbo].[TenantManagements] ADD [MainColor] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0afe6a82-7cc7-4839-acff-ddd9af88920b', 'TenantManagement.dxml', 'TenantManagements', 'MainColor', 'Add Column', GETDATE(), '-- Add New Column With Name MainColorALTER TABLE [dbo].[TenantManagements] ADD [MainColor] VARCHAR(100) NULL;');

-- Add New Column With Name SecondaryColor
ALTER TABLE [dbo].[TenantManagements] ADD [SecondaryColor] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6b2c70e0-b902-42b3-8597-dcca50ad9f40', 'TenantManagement.dxml', 'TenantManagements', 'SecondaryColor', 'Add Column', GETDATE(), '-- Add New Column With Name SecondaryColorALTER TABLE [dbo].[TenantManagements] ADD [SecondaryColor] VARCHAR(100) NULL;');

-- Add New Column With Name BackgroundId
ALTER TABLE [dbo].[TenantManagements] ADD [BackgroundId] VARCHAR(15) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('95508d2f-e593-4b19-ba41-9f163f1f3ecd', 'TenantManagement.dxml', 'TenantManagements', 'BackgroundId', 'Add Column', GETDATE(), '-- Add New Column With Name BackgroundIdALTER TABLE [dbo].[TenantManagements] ADD [BackgroundId] VARCHAR(15) NULL;');


