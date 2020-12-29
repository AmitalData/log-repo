-- Drop Foreign Key Constraint For Column Code In Table BatchServicesDefinitionMods That Reference To Column Code In Table BatchServicesDefinitions
EXEC('IF (OBJECT_ID(''[dbo].[FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitionMods] DROP CONSTRAINT [FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('96572a33-1b3b-4d5f-8a61-29dca6887db8', 'BatchServicesDefinition.dxml', 'BatchServicesDefinitionMods', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column Code In Table BatchServicesDefinitionMods That Reference To Column Code In Table BatchServicesDefinitionsEXEC(''IF (OBJECT_ID(''''[dbo].[FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitionMods] DROP CONSTRAINT [FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code] END'');');

-- Drop Primary Key Constraint
EXEC('IF (OBJECT_ID(''[dbo].[PK_dbo.BatchServicesDefinitions]'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitions] DROP CONSTRAINT [PK_dbo.BatchServicesDefinitions] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('04603e94-1ad2-4158-878c-6f4a81045ae7', 'BatchServicesDefinition.dxml', 'BatchServicesDefinitions', NULL, 'Drop Primary Key', GETDATE(), '-- Drop Primary Key ConstraintEXEC(''IF (OBJECT_ID(''''[dbo].[PK_dbo.BatchServicesDefinitions]'''', ''''PK'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitions] DROP CONSTRAINT [PK_dbo.BatchServicesDefinitions] END'');');

-- Change Size From 40 To 50 For Column Code
ALTER TABLE [dbo].[BatchServicesDefinitions] ALTER COLUMN [Code] VARCHAR(50) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('db21a42d-4ab1-43f5-8ca0-059da21103fd', 'BatchServicesDefinition.dxml', 'BatchServicesDefinitions', 'Code', 'Alter Column Size', GETDATE(), '-- Change Size From 40 To 50 For Column CodeALTER TABLE [dbo].[BatchServicesDefinitions] ALTER COLUMN [Code] VARCHAR(50) NOT NULL;');

-- Add Primary Key Constraint
EXEC('ALTER TABLE [dbo].[BatchServicesDefinitions] ADD CONSTRAINT [PK_dbo.BatchServicesDefinitions] PRIMARY KEY ([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('69d7301d-fceb-48fa-bdb1-aaf330d960bd', 'BatchServicesDefinition.dxml', 'BatchServicesDefinitions', 'Code', 'Add Primary Key', GETDATE(), '-- Add Primary Key ConstraintEXEC(''ALTER TABLE [dbo].[BatchServicesDefinitions] ADD CONSTRAINT [PK_dbo.BatchServicesDefinitions] PRIMARY KEY ([Code])'');');


-- Drop Primary Key Constraint
EXEC('IF (OBJECT_ID(''[dbo].[PK_dbo.BatchServicesDefinitionMods]'', ''PK'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitionMods] DROP CONSTRAINT [PK_dbo.BatchServicesDefinitionMods] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bcdcbccf-42a3-4657-914a-7d21d096d04a', 'BatchServicesDefinitionMods.dxml', 'BatchServicesDefinitionMods', NULL, 'Drop Primary Key', GETDATE(), '-- Drop Primary Key ConstraintEXEC(''IF (OBJECT_ID(''''[dbo].[PK_dbo.BatchServicesDefinitionMods]'''', ''''PK'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitionMods] DROP CONSTRAINT [PK_dbo.BatchServicesDefinitionMods] END'');');

-- Drop Foreign Key Constraint For Column Code In Table BatchServicesDefinitionMods That Reference To Column Code In Table BatchServicesDefinitions
EXEC('IF (OBJECT_ID(''[dbo].[FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code]'', ''F'') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitionMods] DROP CONSTRAINT [FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('07353d75-55df-4b37-9d7d-2932a06d85dd', 'BatchServicesDefinitionMods.dxml', 'BatchServicesDefinitionMods', NULL, 'Drop Relation', GETDATE(), '-- Drop Foreign Key Constraint For Column Code In Table BatchServicesDefinitionMods That Reference To Column Code In Table BatchServicesDefinitionsEXEC(''IF (OBJECT_ID(''''[dbo].[FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code]'''', ''''F'''') IS NOT NULL) BEGIN ALTER TABLE [dbo].[BatchServicesDefinitionMods] DROP CONSTRAINT [FK_dbo.BatchServicesDefinitionMods_dbo.BatchServicesDefinitions_Code] END'');');

-- Drop Index IX_Code From Table BatchServicesDefinitionMods
EXEC('IF EXISTS (SELECT * FROM sys.indexes WHERE name=''IX_Code'' AND object_id = OBJECT_ID(''[dbo].[BatchServicesDefinitionMods]'', ''U'')) BEGIN DROP INDEX [IX_Code] ON [dbo].[BatchServicesDefinitionMods] END');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('30be5165-338a-420c-adde-71dc01ff0507', 'BatchServicesDefinitionMods.dxml', 'BatchServicesDefinitionMods', NULL, 'Drop Index', GETDATE(), '-- Drop Index IX_Code From Table BatchServicesDefinitionModsEXEC(''IF EXISTS (SELECT * FROM sys.indexes WHERE name=''''IX_Code'''' AND object_id = OBJECT_ID(''''[dbo].[BatchServicesDefinitionMods]'''', ''''U'''')) BEGIN DROP INDEX [IX_Code] ON [dbo].[BatchServicesDefinitionMods] END'');');

-- Change Size From 40 To 50 For Column Code
ALTER TABLE [dbo].[BatchServicesDefinitionMods] ALTER COLUMN [Code] VARCHAR(50) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a87f7ac2-9378-4600-9945-b8ca1c9e7f96', 'BatchServicesDefinitionMods.dxml', 'BatchServicesDefinitionMods', 'Code', 'Alter Column Size', GETDATE(), '-- Change Size From 40 To 50 For Column CodeALTER TABLE [dbo].[BatchServicesDefinitionMods] ALTER COLUMN [Code] VARCHAR(50) NOT NULL;');

-- Add Primary Key Constraint
EXEC('ALTER TABLE [dbo].[BatchServicesDefinitionMods] ADD CONSTRAINT [PK_dbo.BatchServicesDefinitionMods] PRIMARY KEY ([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('34753e55-2ef9-4594-9c91-64065845df98', 'BatchServicesDefinitionMods.dxml', 'BatchServicesDefinitionMods', 'Code', 'Add Primary Key', GETDATE(), '-- Add Primary Key ConstraintEXEC(''ALTER TABLE [dbo].[BatchServicesDefinitionMods] ADD CONSTRAINT [PK_dbo.BatchServicesDefinitionMods] PRIMARY KEY ([Code])'');');


-- Set Nullable For Column SharedDWConnection
ALTER TABLE [dbo].[GlobalDBs] ALTER COLUMN [SharedDWConnection] NVARCHAR(512) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3b23a3dd-c637-4b68-b2f9-0eef23e72572', 'GlobalDB.dxml', 'GlobalDBs', 'SharedDWConnection', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column SharedDWConnectionALTER TABLE [dbo].[GlobalDBs] ALTER COLUMN [SharedDWConnection] NVARCHAR(512) NULL;');

-- Set Nullable For Column SecondaryAzureDBConnection
ALTER TABLE [dbo].[GlobalDBs] ALTER COLUMN [SecondaryAzureDBConnection] NVARCHAR(512) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('af909391-8109-4749-87b1-8fe220675df3', 'GlobalDB.dxml', 'GlobalDBs', 'SecondaryAzureDBConnection', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column SecondaryAzureDBConnectionALTER TABLE [dbo].[GlobalDBs] ALTER COLUMN [SecondaryAzureDBConnection] NVARCHAR(512) NULL;');


-- Set Nullable For Column ChampURL
ALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampURL] VARCHAR(1000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('417a85ee-a466-49a0-8b4e-f6c3be42b8aa', 'Setting.dxml', 'Settings', 'ChampURL', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ChampURLALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampURL] VARCHAR(1000) NULL;');

-- Set Nullable For Column ChampTestAPIURL
ALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampTestAPIURL] VARCHAR(1000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a4750e52-3569-4e9c-a1db-7235bb20e570', 'Setting.dxml', 'Settings', 'ChampTestAPIURL', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ChampTestAPIURLALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampTestAPIURL] VARCHAR(1000) NULL;');

-- Set Nullable For Column ChampTestAPIPassword
ALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampTestAPIPassword] VARCHAR(40) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('df8c8279-cd34-41bb-92e6-178c0f587e01', 'Setting.dxml', 'Settings', 'ChampTestAPIPassword', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ChampTestAPIPasswordALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampTestAPIPassword] VARCHAR(40) NULL;');

-- Set Nullable For Column ChampProdAPIURL
ALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampProdAPIURL] VARCHAR(1000) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('06f31524-7dcc-4d70-a053-e22cb73dc0f4', 'Setting.dxml', 'Settings', 'ChampProdAPIURL', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ChampProdAPIURLALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampProdAPIURL] VARCHAR(1000) NULL;');

-- Set Nullable For Column ChampProdAPIPassword
ALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampProdAPIPassword] VARCHAR(40) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('113d8ee0-083b-4945-9fed-f6b44c36c770', 'Setting.dxml', 'Settings', 'ChampProdAPIPassword', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column ChampProdAPIPasswordALTER TABLE [dbo].[Settings] ALTER COLUMN [ChampProdAPIPassword] VARCHAR(40) NULL;');


-- Add Foreign Key Constraint For Column Code In Table BatchServicesDefinitionMods As Reference To Column Code In Table BatchServicesDefinitions
EXEC('ALTER TABLE [dbo].[BatchServicesDefinitionMods] ADD CONSTRAINT [FK_BatchServicesDefinitionMods_BatchServicesDefinitions_Code] FOREIGN KEY([Code]) REFERENCES [dbo].[BatchServicesDefinitions]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('cf3eec7f-ec73-4e2e-82d3-59566231af69', 'BatchServicesDefinitionMods.dxml', 'BatchServicesDefinitionMods', 'Code', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column Code In Table BatchServicesDefinitionMods As Reference To Column Code In Table BatchServicesDefinitionsEXEC(''ALTER TABLE [dbo].[BatchServicesDefinitionMods] ADD CONSTRAINT [FK_BatchServicesDefinitionMods_BatchServicesDefinitions_Code] FOREIGN KEY([Code]) REFERENCES [dbo].[BatchServicesDefinitions]([Code])'');');

-- Create Index On BatchServicesDefinitionMods Table
EXEC('CREATE NONCLUSTERED INDEX [IX_BatchServicesDefinitionMods_Code] ON [dbo].[BatchServicesDefinitionMods]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a59e017e-08ea-4e7b-9baa-fb8fbea195e9', 'BatchServicesDefinitionMods.dxml', 'BatchServicesDefinitionMods', 'Code', 'Create Index', GETDATE(), '-- Create Index On BatchServicesDefinitionMods TableEXEC(''CREATE NONCLUSTERED INDEX [IX_BatchServicesDefinitionMods_Code] ON [dbo].[BatchServicesDefinitionMods]([Code])'');');


-- General Script From 202012100811_QueryExportExecutionLogWorkerRole.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete BatchServicesDefinitionMods where Code ='QueryExportExecutionLogWR'
delete BatchServicesDefinitions where Code ='QueryExportExecutionLogWR'
INSERT INTO [dbo].[BatchServicesDefinitions]
([Code]
,[ClassName]
,[Parameter1]
,[Parameter2] ,
[QueueDefinitionCode])
VALUES
('QueryExportExecutionLogWR'
,'QueryExportExecutionLogWR'
,NULL
,NULL
,'QueryExportExecutionLogQueue')
INSERT INTO [dbo].[BatchServicesDefinitionMods]
([Code]
,[InActive]
,[NumberOfThreads])
VALUES
('QueryExportExecutionLogWR'
,0
,1)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202012100811_QueryExportExecutionLogWorkerRole.sxml', GETDATE(), 'delete BatchServicesDefinitionMods where Code =''QueryExportExecutionLogWR''
delete BatchServicesDefinitions where Code =''QueryExportExecutionLogWR''
INSERT INTO [dbo].[BatchServicesDefinitions]
([Code]
,[ClassName]
,[Parameter1]
,[Parameter2] ,
[QueueDefinitionCode])
VALUES
(''QueryExportExecutionLogWR''
,''QueryExportExecutionLogWR''
,NULL
,NULL
,''QueryExportExecutionLogQueue'')
INSERT INTO [dbo].[BatchServicesDefinitionMods]
([Code]
,[InActive]
,[NumberOfThreads])
VALUES
(''QueryExportExecutionLogWR''
,0
,1)', DATEDIFF(MS,@StartTime,@EndTime), '18836d99a01ec00bf4c262270d834a3f', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From UpdateAutomationWorkerRoleName.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete BatchServicesDefinitionMods where Code ='DelayAutomationWR'
delete BatchServicesDefinitions where Code ='DelayAutomationWR'
INSERT INTO [dbo].[BatchServicesDefinitions]
([Code]
,[ClassName]
,[Parameter1]
,[Parameter2] ,
[QueueDefinitionCode])
VALUES
('AutomationWR'
,'AutomationWorkerRole'
,0
,NULL
,'AutomationQueue')
INSERT INTO [dbo].[BatchServicesDefinitionMods]
([Code]
,[InActive]
,[NumberOfThreads])
VALUES
('AutomationWR'
,0
,1)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('UpdateAutomationWorkerRoleName.sxml', GETDATE(), 'delete BatchServicesDefinitionMods where Code =''DelayAutomationWR''
delete BatchServicesDefinitions where Code =''DelayAutomationWR''
INSERT INTO [dbo].[BatchServicesDefinitions]
([Code]
,[ClassName]
,[Parameter1]
,[Parameter2] ,
[QueueDefinitionCode])
VALUES
(''AutomationWR''
,''AutomationWorkerRole''
,0
,NULL
,''AutomationQueue'')
INSERT INTO [dbo].[BatchServicesDefinitionMods]
([Code]
,[InActive]
,[NumberOfThreads])
VALUES
(''AutomationWR''
,0
,1)', DATEDIFF(MS,@StartTime,@EndTime), 'ef667edfcb0c5b514bb9f2a50077c314', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202010151325_InsertDocumentApprovalQueue.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
insert into BatchServicesDefinitions values ('DocumentApproval','DocumentApprovalWorkerRole',NULL,NULL,'DocumentApprovalQueue')
insert into BatchServicesDefinitionMods values ('DocumentApproval',0,1)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202010151325_InsertDocumentApprovalQueue.sxml', GETDATE(), 'insert into BatchServicesDefinitions values (''DocumentApproval'',''DocumentApprovalWorkerRole'',NULL,NULL,''DocumentApprovalQueue'')
insert into BatchServicesDefinitionMods values (''DocumentApproval'',0,1)', DATEDIFF(MS,@StartTime,@EndTime), '22626ac33a418f8003bffea0923e63a5', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

