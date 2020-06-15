-- Set Nullable For Column CreateDate
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [CreateDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('73b75a58-9c1d-400e-984a-95fc2c7d2f33', 'HelpResource.dxml', 'HelpResources', 'CreateDate', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column CreateDateALTER TABLE [dbo].[HelpResources] ALTER COLUMN [CreateDate] DATETIME NULL;');

-- Set Nullable For Column UpdateDate
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [UpdateDate] DATETIME NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ff68d1fc-603b-4b75-a066-99ebf2310d7a', 'HelpResource.dxml', 'HelpResources', 'UpdateDate', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column UpdateDateALTER TABLE [dbo].[HelpResources] ALTER COLUMN [UpdateDate] DATETIME NULL;');

-- Unset Nullable For Column Language
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Language] VARCHAR(2) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8137072e-a0d8-433c-bd4e-93141e7b6849', 'HelpResource.dxml', 'HelpResources', 'Language', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column LanguageALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Language] VARCHAR(2) NOT NULL;');

-- Unset Nullable For Column Type
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Type] VARCHAR(3) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2da59a44-2abc-46c9-904c-ad0e65ffc330', 'HelpResource.dxml', 'HelpResources', 'Type', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column TypeALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Type] VARCHAR(3) NOT NULL;');

-- Unset Nullable For Column Category
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Category] VARCHAR(3) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2d9dcdd1-8fd1-4c05-b429-0a9fb112c102', 'HelpResource.dxml', 'HelpResources', 'Category', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column CategoryALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Category] VARCHAR(3) NOT NULL;');

-- Unset Nullable For Column Tenant
ALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Tenant] INT NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('75be03c6-8aa5-4f49-b72f-c2f767d1fb13', 'HelpResource.dxml', 'HelpResources', 'Tenant', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column TenantALTER TABLE [dbo].[HelpResources] ALTER COLUMN [Tenant] INT NOT NULL;');


-- General Script From 202005311656_DeleteDuplicatedQueuesWR Batch Definition.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
INSERT INTO [dbo].[BatchServicesDefinitions]
([Code]
,[ClassName]
,[Parameter1]
,[Parameter2])
VALUES
('DeleteDuplicatedQueuesWR'
,'DeleteDuplicatedQueuesWR'
,NULL
,NULL)
INSERT INTO [dbo].[BatchServicesDefinitionMods]
([Code]
,[InActive]
,[NumberOfThreads])
VALUES
('DeleteDuplicatedQueuesWR'
,0
,1)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202005311656_DeleteDuplicatedQueuesWR Batch Definition.sxml', GETDATE(), 'INSERT INTO [dbo].[BatchServicesDefinitions]
([Code]
,[ClassName]
,[Parameter1]
,[Parameter2])
VALUES
(''DeleteDuplicatedQueuesWR''
,''DeleteDuplicatedQueuesWR''
,NULL
,NULL)
INSERT INTO [dbo].[BatchServicesDefinitionMods]
([Code]
,[InActive]
,[NumberOfThreads])
VALUES
(''DeleteDuplicatedQueuesWR''
,0
,1)', DATEDIFF(MS,@StartTime,@EndTime), 'b7bc70a5dc075e5ed022cc86e62af1db', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

