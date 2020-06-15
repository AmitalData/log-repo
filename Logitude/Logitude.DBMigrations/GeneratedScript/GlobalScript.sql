-- Unset Nullable For Column CreateDate
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [CreateDate] DATETIME NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6d0e439f-cb3e-4c04-b468-0d836686894e', 'HelpResource.dxml', 'HelpResources', 'CreateDate', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column CreateDateALTER TABLE [dbo].[HelpResources] ALTER COLUMN [CreateDate] DATETIME NOT NULL;');

-- Unset Nullable For Column UpdateDate
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [UpdateDate] DATETIME NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('76669f8e-51e3-47ee-8cc2-0e25052779f4', 'HelpResource.dxml', 'HelpResources', 'UpdateDate', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column UpdateDateALTER TABLE [dbo].[HelpResources] ALTER COLUMN [UpdateDate] DATETIME NOT NULL;');

-- Set Nullable For Column Language
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Language] VARCHAR(2) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0be3d4f3-b657-4911-9f1f-3553a17ba1bb', 'HelpResource.dxml', 'HelpResources', 'Language', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column LanguageALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Language] VARCHAR(2) NULL;');

-- Set Nullable For Column Type
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Type] VARCHAR(3) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5c85d1f2-9db4-4470-9111-e23f4626f98e', 'HelpResource.dxml', 'HelpResources', 'Type', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column TypeALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Type] VARCHAR(3) NULL;');

-- Set Nullable For Column Category
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Category] VARCHAR(3) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f3e72137-6a46-4520-aa9a-e97c6cbb06dc', 'HelpResource.dxml', 'HelpResources', 'Category', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column CategoryALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Category] VARCHAR(3) NULL;');

-- Set Nullable For Column Tenant
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Tenant] INT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('dbf61e91-d995-4db6-b8df-b7409c2d2eb6', 'HelpResource.dxml', 'HelpResources', 'Tenant', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column TenantALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Tenant] INT NULL;');

-- Drop Column Tenant
EXEC SP_RENAME 'dbo.HelpResources.Tenant', 'Drop_Tenant', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f9a6998c-2137-4e7e-bdca-5be213003d67', 'HelpResource.dxml', 'HelpResources', 'Tenant', 'Drop Column', GETDATE(), '-- Drop Column TenantEXEC SP_RENAME ''dbo.HelpResources.Tenant'', ''Drop_Tenant'', ''COLUMN'';');


