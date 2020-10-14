-- Create Index On Documents Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Documents_HasFile] ON [dbo].[Documents]([HasFile])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('bda6f9a5-e21a-4458-9453-38d2d1ae35e4', 'Document.dxml', 'Documents', 'HasFile', 'Create Index', GETDATE(), '-- Create Index On Documents TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Documents_HasFile] ON [dbo].[Documents]([HasFile])'');');


-- Create Index On DocumentsFilings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_CreateDate] ON [dbo].[DocumentsFilings]([CreateDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4a403913-4bd9-46e3-bd0c-0b84b19f5e2b', 'DocumentsFiling.dxml', 'DocumentsFilings', 'CreateDate', 'Create Index', GETDATE(), '-- Create Index On DocumentsFilings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_CreateDate] ON [dbo].[DocumentsFilings]([CreateDate])'');');

-- Create Index On DocumentsFilings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_EntityId_ChildEntityId] ON [dbo].[DocumentsFilings]([EntityId],[ChildEntityId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0db19597-332a-4b08-b455-0c07242e988c', 'DocumentsFiling.dxml', 'DocumentsFilings', 'EntityId,ChildEntityId', 'Create Index', GETDATE(), '-- Create Index On DocumentsFilings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_EntityId_ChildEntityId] ON [dbo].[DocumentsFilings]([EntityId],[ChildEntityId])'');');

-- Create Index On DocumentsFilings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_ChildEntityId] ON [dbo].[DocumentsFilings]([ChildEntityId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4c7a0a7a-b6cb-4110-9f3c-95c329ba4c26', 'DocumentsFiling.dxml', 'DocumentsFilings', 'ChildEntityId', 'Create Index', GETDATE(), '-- Create Index On DocumentsFilings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_ChildEntityId] ON [dbo].[DocumentsFilings]([ChildEntityId])'');');


-- Change Size From -1 To 6 For Column CustomsMetaDataCode
ALTER TABLE [dbo].[DocumentsMetaDataTypes] ALTER COLUMN [CustomsMetaDataCode] NVARCHAR(6);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('fa8bdbf3-348e-43d0-89a8-e5e99b3380c4', 'DocumentsMetaDataType.dxml', 'DocumentsMetaDataTypes', 'CustomsMetaDataCode', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 6 For Column CustomsMetaDataCodeALTER TABLE [dbo].[DocumentsMetaDataTypes] ALTER COLUMN [CustomsMetaDataCode] NVARCHAR(6);');


-- Add New Column With Name DefultAttachmentsXML
ALTER TABLE [dbo].[DocumentTypeTemplates] ADD [DefultAttachmentsXML] NVARCHAR(MAX) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ee27c6ed-b96b-4ae4-8456-2fff2f35fb46', 'DocumentTypeTemplate.dxml', 'DocumentTypeTemplates', 'DefultAttachmentsXML', 'Add Column', GETDATE(), '-- Add New Column With Name DefultAttachmentsXMLALTER TABLE [dbo].[DocumentTypeTemplates] ADD [DefultAttachmentsXML] NVARCHAR(MAX) NULL;');


-- Change Size From 8000 To -1 For Column Notes
ALTER TABLE [dbo].[FeatureChanges] ALTER COLUMN [Notes] VARCHAR(MAX);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('1c8169b3-7e9f-424d-b9c7-20d8db8ba37b', 'FeatureChange.dxml', 'FeatureChanges', 'Notes', 'Alter Column Size', GETDATE(), '-- Change Size From 8000 To -1 For Column NotesALTER TABLE [dbo].[FeatureChanges] ALTER COLUMN [Notes] VARCHAR(MAX);');


-- Change Type From nvarchar To varchar For Column Description
ALTER TABLE [dbo].[Reports] ALTER COLUMN [Description] VARCHAR(250);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0e552ba8-b45a-4023-8113-c2df3fff04c3', 'Report.dxml', 'Reports', 'Description', 'Alter Column Type', GETDATE(), '-- Change Type From nvarchar To varchar For Column DescriptionALTER TABLE [dbo].[Reports] ALTER COLUMN [Description] VARCHAR(250);');

-- Change Type From nvarchar To varchar For Column SearchFields
ALTER TABLE [dbo].[Reports] ALTER COLUMN [SearchFields] VARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('cf934256-d491-458d-9cf5-478a08403952', 'Report.dxml', 'Reports', 'SearchFields', 'Alter Column Type', GETDATE(), '-- Change Type From nvarchar To varchar For Column SearchFieldsALTER TABLE [dbo].[Reports] ALTER COLUMN [SearchFields] VARCHAR(1000);');

-- Unset Nullable For Column Code
ALTER TABLE [dbo].[Reports] ALTER COLUMN [Code] VARCHAR(4) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ad32d950-f555-48eb-a680-75a0ff268aa1', 'Report.dxml', 'Reports', 'Code', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column CodeALTER TABLE [dbo].[Reports] ALTER COLUMN [Code] VARCHAR(4) NOT NULL;');


-- Add New Column With Name SearchWindowFiltersIndex
ALTER TABLE [dbo].[ObjectFields] ADD [SearchWindowFiltersIndex] INT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8c732471-cb13-4f51-a6fb-7a36d72304f4', 'ObjectField.dxml', 'ObjectFields', 'SearchWindowFiltersIndex', 'Add Column', GETDATE(), '-- Add New Column With Name SearchWindowFiltersIndexALTER TABLE [dbo].[ObjectFields] ADD [SearchWindowFiltersIndex] INT DEFAULT(0) NOT NULL;');

-- Add New Column With Name AllowedInCustFieldsSettings
ALTER TABLE [dbo].[ObjectFields] ADD [AllowedInCustFieldsSettings] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a33c3310-d332-4600-bafc-ad466c32b069', 'ObjectField.dxml', 'ObjectFields', 'AllowedInCustFieldsSettings', 'Add Column', GETDATE(), '-- Add New Column With Name AllowedInCustFieldsSettingsALTER TABLE [dbo].[ObjectFields] ADD [AllowedInCustFieldsSettings] BIT DEFAULT(0) NOT NULL;');

-- Set Nullable For Column DisplayInSearchWindowFiltersIndex
ALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [DisplayInSearchWindowFiltersIndex] INT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('44c560d5-61b1-4a62-9b11-e9680ed923c2', 'ObjectField.dxml', 'ObjectFields', 'DisplayInSearchWindowFiltersIndex', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column DisplayInSearchWindowFiltersIndexALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [DisplayInSearchWindowFiltersIndex] INT NULL;');

-- Drop Column DisplayInSearchWindowFiltersIndex
EXEC SP_RENAME 'dbo.ObjectFields.DisplayInSearchWindowFiltersIndex', 'Drop_DisplayInSearchWindowFiltersIndex', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6f8c955b-f05b-47f3-8559-b8fff7b5cce2', 'ObjectField.dxml', 'ObjectFields', 'DisplayInSearchWindowFiltersIndex', 'Drop Column', GETDATE(), '-- Drop Column DisplayInSearchWindowFiltersIndexEXEC SP_RENAME ''dbo.ObjectFields.DisplayInSearchWindowFiltersIndex'', ''Drop_DisplayInSearchWindowFiltersIndex'', ''COLUMN'';');

-- Set Nullable For Column AllowedInCustomerFieldsSettings
ALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [AllowedInCustomerFieldsSettings] BIT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c5c460be-78bb-4529-847c-9ef43292dc5b', 'ObjectField.dxml', 'ObjectFields', 'AllowedInCustomerFieldsSettings', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column AllowedInCustomerFieldsSettingsALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [AllowedInCustomerFieldsSettings] BIT NULL;');

-- Drop Column AllowedInCustomerFieldsSettings
EXEC SP_RENAME 'dbo.ObjectFields.AllowedInCustomerFieldsSettings', 'Drop_AllowedInCustomerFieldsSettings', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6f814e14-b1c1-4bd4-868d-7d9b83de9118', 'ObjectField.dxml', 'ObjectFields', 'AllowedInCustomerFieldsSettings', 'Drop Column', GETDATE(), '-- Drop Column AllowedInCustomerFieldsSettingsEXEC SP_RENAME ''dbo.ObjectFields.AllowedInCustomerFieldsSettings'', ''Drop_AllowedInCustomerFieldsSettings'', ''COLUMN'';');


-- Change Size From -1 To 500 For Column SplitComponentPath
ALTER TABLE [dbo].[ObjectTables] ALTER COLUMN [SplitComponentPath] NVARCHAR(500);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2db07dbc-efa6-45cc-a714-1df82a403d78', 'ObjectTable.dxml', 'ObjectTables', 'SplitComponentPath', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 500 For Column SplitComponentPathALTER TABLE [dbo].[ObjectTables] ALTER COLUMN [SplitComponentPath] NVARCHAR(500);');


-- Add Foreign Key Constraint For Column CreatedByUserId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('46c0dbaa-2fd3-48dc-9ce5-6b12190b46f2', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'CreatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CreatedByUserId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On CalculatedChartsOfAccountsLines Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_CreatedByUserId] ON [dbo].[CalculatedChartsOfAccountsLines]([CreatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('169575aa-7c6e-4d63-aff0-d6aa1964c8f2', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'CreatedByUserId', 'Create Index', GETDATE(), '-- Create Index On CalculatedChartsOfAccountsLines TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_CreatedByUserId] ON [dbo].[CalculatedChartsOfAccountsLines]([CreatedByUserId])'');');

-- Add Foreign Key Constraint For Column UpdatedByUserId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9528e1d7-0ba2-489a-9c1f-b57b1e23d8dc', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'UpdatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UpdatedByUserId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On CalculatedChartsOfAccountsLines Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_UpdatedByUserId] ON [dbo].[CalculatedChartsOfAccountsLines]([UpdatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('17989341-04a3-4e5a-80e7-f102fcb212f6', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'UpdatedByUserId', 'Create Index', GETDATE(), '-- Create Index On CalculatedChartsOfAccountsLines TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_UpdatedByUserId] ON [dbo].[CalculatedChartsOfAccountsLines]([UpdatedByUserId])'');');

-- Add Foreign Key Constraint For Column GLAccountId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table GLAccounts
EXEC('ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_GLAccounts_GLAccountId] FOREIGN KEY([GLAccountId]) REFERENCES [dbo].[GLAccounts]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4e879f37-c67c-4001-a93e-fc2a566f2928', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'GLAccountId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column GLAccountId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table GLAccountsEXEC(''ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_GLAccounts_GLAccountId] FOREIGN KEY([GLAccountId]) REFERENCES [dbo].[GLAccounts]([Id])'');');

-- Create Index On CalculatedChartsOfAccountsLines Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_GLAccountId] ON [dbo].[CalculatedChartsOfAccountsLines]([GLAccountId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ac27801e-0829-41c1-9cdc-48b4f2fa5bda', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'GLAccountId', 'Create Index', GETDATE(), '-- Create Index On CalculatedChartsOfAccountsLines TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_GLAccountId] ON [dbo].[CalculatedChartsOfAccountsLines]([GLAccountId])'');');

-- Add Foreign Key Constraint For Column ChartOfAccountId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table ChartOfAccounts
EXEC('ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_ChartOfAccounts_ChartOfAccountId] FOREIGN KEY([ChartOfAccountId]) REFERENCES [dbo].[ChartOfAccounts]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('56cb7719-6c61-4d13-bd6d-101b4da723ec', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'ChartOfAccountId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column ChartOfAccountId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table ChartOfAccountsEXEC(''ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_ChartOfAccounts_ChartOfAccountId] FOREIGN KEY([ChartOfAccountId]) REFERENCES [dbo].[ChartOfAccounts]([Id])'');');

-- Create Index On CalculatedChartsOfAccountsLines Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_ChartOfAccountId] ON [dbo].[CalculatedChartsOfAccountsLines]([ChartOfAccountId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8075b0db-95e4-42f6-a4b6-79e266d3d239', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'ChartOfAccountId', 'Create Index', GETDATE(), '-- Create Index On CalculatedChartsOfAccountsLines TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_ChartOfAccountId] ON [dbo].[CalculatedChartsOfAccountsLines]([ChartOfAccountId])'');');

-- Add Foreign Key Constraint For Column CalculatedChartsOfAccountsId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table ChartOfAccounts
EXEC('ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_ChartOfAccounts_CalculatedChartsOfAccountsId] FOREIGN KEY([CalculatedChartsOfAccountsId]) REFERENCES [dbo].[ChartOfAccounts]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4e303384-fbe9-4811-aca0-275694e976ed', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'CalculatedChartsOfAccountsId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CalculatedChartsOfAccountsId In Table CalculatedChartsOfAccountsLines As Reference To Column Id In Table ChartOfAccountsEXEC(''ALTER TABLE [dbo].[CalculatedChartsOfAccountsLines] ADD CONSTRAINT [FK_CalculatedChartsOfAccountsLines_ChartOfAccounts_CalculatedChartsOfAccountsId] FOREIGN KEY([CalculatedChartsOfAccountsId]) REFERENCES [dbo].[ChartOfAccounts]([Id])'');');

-- Create Index On CalculatedChartsOfAccountsLines Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_CalculatedChartsOfAccountsId] ON [dbo].[CalculatedChartsOfAccountsLines]([CalculatedChartsOfAccountsId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f2470f68-91c2-4de9-aae0-a7efb89d4060', 'CalculatedChartsOfAccountsLine.dxml', 'CalculatedChartsOfAccountsLines', 'CalculatedChartsOfAccountsId', 'Create Index', GETDATE(), '-- Create Index On CalculatedChartsOfAccountsLines TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CalculatedChartsOfAccountsLines_CalculatedChartsOfAccountsId] ON [dbo].[CalculatedChartsOfAccountsLines]([CalculatedChartsOfAccountsId])'');');


-- Add Foreign Key Constraint For Column CreatedByUserId In Table UserDefinedReports As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[UserDefinedReports] ADD CONSTRAINT [FK_UserDefinedReports_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e38ef317-11c8-4eb9-81be-13de7b8f2ae2', 'UserDefinedReport.dxml', 'UserDefinedReports', 'CreatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CreatedByUserId In Table UserDefinedReports As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[UserDefinedReports] ADD CONSTRAINT [FK_UserDefinedReports_Users_CreatedByUserId] FOREIGN KEY([CreatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On UserDefinedReports Table
EXEC('CREATE NONCLUSTERED INDEX [IX_UserDefinedReports_CreatedByUserId] ON [dbo].[UserDefinedReports]([CreatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3156a742-9919-48c9-a800-deeb00a4c9f7', 'UserDefinedReport.dxml', 'UserDefinedReports', 'CreatedByUserId', 'Create Index', GETDATE(), '-- Create Index On UserDefinedReports TableEXEC(''CREATE NONCLUSTERED INDEX [IX_UserDefinedReports_CreatedByUserId] ON [dbo].[UserDefinedReports]([CreatedByUserId])'');');

-- Add Foreign Key Constraint For Column UpdatedByUserId In Table UserDefinedReports As Reference To Column Id In Table Users
EXEC('ALTER TABLE [dbo].[UserDefinedReports] ADD CONSTRAINT [FK_UserDefinedReports_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('18e6deac-4d76-4863-999a-87acb8877741', 'UserDefinedReport.dxml', 'UserDefinedReports', 'UpdatedByUserId', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column UpdatedByUserId In Table UserDefinedReports As Reference To Column Id In Table UsersEXEC(''ALTER TABLE [dbo].[UserDefinedReports] ADD CONSTRAINT [FK_UserDefinedReports_Users_UpdatedByUserId] FOREIGN KEY([UpdatedByUserId]) REFERENCES [dbo].[Users]([Id])'');');

-- Create Index On UserDefinedReports Table
EXEC('CREATE NONCLUSTERED INDEX [IX_UserDefinedReports_UpdatedByUserId] ON [dbo].[UserDefinedReports]([UpdatedByUserId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('039ad880-8154-4e74-ad34-4801e552b455', 'UserDefinedReport.dxml', 'UserDefinedReports', 'UpdatedByUserId', 'Create Index', GETDATE(), '-- Create Index On UserDefinedReports TableEXEC(''CREATE NONCLUSTERED INDEX [IX_UserDefinedReports_UpdatedByUserId] ON [dbo].[UserDefinedReports]([UpdatedByUserId])'');');


-- Procedure Script From IdCounter GetIdRange Procedure.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetNextTableIdsRange]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetNextTableIdsRange] END');
EXEC('Create PROCEDURE [dbo].[usp_GetNextTableIdsRange]
(
@pEndNumber   int OUTPUT,
@pStartNumber int OutPUT,
@DBStringNumber varchar(50) OutPUT,
@pTableName    varchar(40),
@pNumberOfIds int
)
AS
DECLARE @COUNTERINIT AS INT
SET @COUNTERINIT = @pNumberOfIds
SET @pStartNumber = 1
Declare @Current As Int
Declare @DataBaseNumber As Int
--Declare @DBStringNumber As varchar(50)
Declare @StartStrNumber As varchar(50)
Declare @EndStrNumber As varchar(50)
SELECT TOP 1 @DataBaseNumber = DataBaseNumber FROM DataBaseProperties
Set @DBStringNumber = CONVERT(varchar(50) , @DataBaseNumber)
IF NOT EXISTS (SELECT TableName
FROM DBIdCounters (UPDLOCK) WHERE TableName =@pTableName)
BEGIN;
INSERT INTO DBIdCounters
(
TableName,
LastIdNumber
)
VALUES
(
@pTableName,
@COUNTERINIT
)
Set @Current = 1
End
Else
BEGIN;
Set @Current	= (SELECT  LastIdNumber
FROM DBIdCounters with (UPDLOCK) WHERE  TableName =@pTableName)
set @pEndNumber = @Current + @pNumberOfIds
set @pStartNumber =  @Current + 1
Update DBIdCounters
set LastIdNumber = LastIdNumber + @pNumberOfIds
Where TableName = @pTableName
End;');


-- Procedure Script From usp_DeleteObjectTableMetadata.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteObjectTableMetadata]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteObjectTableMetadata] END');
EXEC('create PROCEDURE [dbo].[usp_DeleteObjectTableMetadata]
(
@pTableName    varchar(50)
)
AS
Declare @ObjectTableId As varchar(15)
Begin
SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
--*--Delete--*--
--ObjectFields
delete from objectfields where tenant = 0 and ObjectTableId = @ObjectTableId
delete from querycolumns where tenant = 0 and  userid is null  and QueryCode in (select UniqueCode from Queries where ObjectTableId = @ObjectTableId)
delete from AdvancedQueryFilters where tenant = 0  and userid is null and QueryCode in  (select UniqueCode from Queries where ObjectTableId = @ObjectTableId)
delete from RuleConditionFields where tenant = 0   and ObjectTableRuleId not in (select id from ObjectTableRules where SystemLevel=0 )  and ObjectTableRuleId in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
delete from ObjectTableRuleFields where tenant = 0  and systemlevel = 1 and ObjectTableRuleId in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
delete from ObjectTableRules where tenant = 0  and systemlevel = 1 and Id in (select id from ObjectTableRules  where ObjectTableId = @ObjectTableId)
--Screens
delete from ScreenFields where tenant = 0  and ScreenCode in (select code from Screens where ObjectTableId = @ObjectTableId)
delete from screens where tenant = 0  and ObjectTableId = @ObjectTableId
--Queries
delete from Queries where tenant = 0  and userid is null and systemlevel = 1 and ObjectTableId = @ObjectTableId
--TextCodes
delete from MenuButtons where tenant = 0 and  MenuButtonGroupId in (select id from MenuButtonGroups where ObjectTableId = @ObjectTableId)
delete from ObjectTableTabs where tenant = 0  and ObjectTableId = @ObjectTableId
delete from textcodes where tenant = 0 and code not in (select NameTextCodeCode from queries where tenant=0 and userid is not null and SystemLevel=0 and NameTextCodeCode is not null) and code not in (select ShortTextCodeCode from tips)  and ObjectTableId = @ObjectTableId
--Features
delete from Features where tenant = 0  and ObjectTableId = @ObjectTableId
-----------
End');


-- Procedure Script From usp_PreDeleteMetadata.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_PreDeleteMetadata]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_PreDeleteMetadata] END');
EXEC('create PROCEDURE [dbo].[usp_PreDeleteMetadata]
(
@pTableName    varchar(50)
)
AS
Declare @ObjectTableId As varchar(15)
Begin
SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
----*--Before Delete--*--
if object_id(''dbo.TempOldFeatures'') is not null
drop table TempOldFeatures
select * into TempOldFeatures
from (select Code,FeatureUniqeCode,IsOld,Tenant
from features
where IsOld = 1) as t
--select * from TempOldFeatures
if object_id(''dbo.TempIsSpellCheckedTextCodes'') is not null
drop table TempIsSpellCheckedTextCodes
select * into TempIsSpellCheckedTextCodes
from (select *
from TextCodes
where IsSpellChecked = 1) as t
--select * from TempIsSpellCheckedTextCodes
End');


-- Procedure Script From usp_ReconnectObjectTableMetadata.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ReconnectObjectTableMetadata]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata] END');
EXEC('create PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata]
(
@pTableName    varchar(50)
)
AS
Declare @ObjectTableId As varchar(15)
Begin
SELECT TOP 1 @ObjectTableId = Id FROM ObjectTables where name = @pTableName
----MetaData All Scripts: Never Apply these scripts
--*--After Delete--*--
--ObjectFields
update querycolumns set ObjectFieldId = (select Id from objectfields where FieldCode=querycolumns.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update ScreenFields set ObjectFieldId = (select Id from objectfields where FieldCode=ScreenFields.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update AdvancedQueryFilters set ObjectFieldId = (select Id from objectfields where FieldCode=AdvancedQueryFilters.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
update RuleConditionFields set ObjectFieldId = (select Id from objectfields where FieldCode=RuleConditionFields.ObjectFieldCode)
update ObjectTableRuleFields set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectTableRuleFields.ObjectFieldCode)
update ObjectTableRules set TriggerFieldId = (select Id from objectfields where FieldCode=ObjectTableRules.TriggerFieldCode)
update AirlineMessagingRules set RuleFieldId = (select Id from objectfields where FieldCode=AirlineMessagingRules.RuleFieldCode)
update restrictions  set ObjectFieldId = (select Id from objectfields where FieldCode=restrictions.ObjectFieldCode)
update CustomerFieldsUpdateSettings  set ObjectFieldId = (select Id from objectfields where FieldCode=CustomerFieldsUpdateSettings.ObjectFieldCode)
update ObjectFieldValidations  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldValidations.ObjectFieldCode)
update ObjectFieldModifications  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldModifications.ObjectFieldCode) where objectfieldcode in (select fieldcode from ObjectFields)
--Screens
update ScreenModifications set ScreenId = (select Id from Screens where code=ScreenModifications.ScreenCode) where screencode in (select code from screens)
update ScreenFields set ScreenId = (select Id from Screens where code=ScreenFields.ScreenCode) where screencode in (select code from screens)
update ObjectTables set HeaderScreenId = (select Id from Screens where code=ObjectTables.HeaderScreenCode)
--Queries
update Queries set OriginalQueryId =  (select q1.Id from Queries q1 where q1.UniqueCode = Queries.OriginalQueryCode)
update AdvancedQueryFilters set QueryId = (select Id from Queries where UniqueCode = AdvancedQueryFilters.QueryCode) where QueryCode in (select UniqueCode from Queries)
update QueryColumns set QueryId = (select Id from Queries where UniqueCode = QueryColumns.QueryCode) where QueryCode in (select UniqueCode from Queries)
update SharedUserQueries set QueryId = (select Id from Queries where UniqueCode = SharedUserQueries.QueryCode)
--TextCodes
update Queries set NameTextCodeId = (select Id from TextCodes where Code=Queries.NameTextCodeCode and tenant = Queries.Tenant)
update ObjectTables set DescriptionTextCodeId = (select Id from TextCodes where Code=ObjectTables.DescriptionTextCodeCode and tenant = ObjectTables.Tenant)
update ObjectTables set NewButtonTextCodeId = (select Id from TextCodes where Code=ObjectTables.NewButtonTextCodeCode)
update Features set NameTextCodeId = (select Id from TextCodes where Code=Features.NameTextCodeCode and tenant = Features.Tenant)
update Tips set ShortTextCode = (select Id from TextCodes where Code=Tips.ShortTextCodeCode)
update ObjectTableTabs set TabNameTextCodeId = (select Id from TextCodes where Code=ObjectTableTabs.TabNameTextCodeCode and tenant = ObjectTableTabs.Tenant)
update MenuButtons set LabelTextCodeId = (select Id from TextCodes where Code=MenuButtons.LabelTextCodeCode)
update ObjectFields set FullNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.FullNameTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set HelpTextCodeId = (select Id from TextCodes where Code=ObjectFields.HelpTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set ShortNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.ShortNameTextCodeCode and tenant = ObjectFields.Tenant)
update ObjectFields set ListTextCodeId = (select Id from TextCodes where Code=ObjectFields.ListTextCodeCode and tenant = ObjectFields.Tenant)
update Translations set TextCodeId = (select Id from TextCodes where Code=Translations.TextCodeCode and (tenant =  Translations.Tenant or tenant = 0)) where TextCodeCode in (select code from textcodes)
--Features
update MenusTables set FeatureId = (select Id from features where FeatureUniqeCode=MenusTables.FeatureUniqeCode)
update Queries set FeatureId = (select Id from features where FeatureUniqeCode=Queries.FeatureUniqeCode)
update ObjectTableTabs set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableTabs.FeatureUniqeCode)
update ObjectTableHelperControls set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableHelperControls.FeatureUniqeCode)
update MenuButtons set FeatureId = (select Id from features where FeatureUniqeCode=MenuButtons.FeatureUniqeCode)
update Reports set FeatureId = (select Id from features where FeatureUniqeCode=Reports.FeatureUniqeCode)
update PackageFeatures set FeatureId = (select Id from features where FeatureUniqeCode=PackageFeatures.FeatureUniqeCode) where PackageFeatures.FeatureUniqeCode in (select FeatureUniqeCode from Features)
update RoleFeatures set FeatureId = (select Id from features where FeatureUniqeCode=RoleFeatures.FeatureUniqeCode) where RoleFeatures.FeatureUniqeCode in (select FeatureUniqeCode from Features)
-------------
UPDATE f
SET f.IsOld = temp.IsOld
FROM Features f
JOIN TempOldFeatures temp
ON f.FeatureUniqeCode = temp.FeatureUniqeCode
update tc
set tc.DefaultText = temp.DefaultText, tc.DefaultTextPlural = temp.DefaultTextPlural, tc.SpellCheckDate = temp.SpellCheckDate,
tc.SpellCheckedByUserId = temp.SpellCheckedByUserId,tc.LocalDefaultText = temp.LocalDefaultText,IsSpellChecked = temp.IsSpellChecked
from TextCodes tc
Join TempIsSpellCheckedTextCodes temp
on tc.Code = temp.Code
where tc.ObjectTableId = temp.ObjectTableId
End');


-- General Script From InsertDBMigrationSettingsData.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
INSERT INTO [dbo].[DBMigrationSettings]([Mode], [ModulesList]) VALUES('Exclude', 'Customs');
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('InsertDBMigrationSettingsData.sxml', GETDATE(), 'INSERT INTO [dbo].[DBMigrationSettings]([Mode], [ModulesList]) VALUES(''Exclude'', ''Customs'');', DATEDIFF(MS,@StartTime,@EndTime), '4fa490a8fc4e701f6a1cdef3ff29f3a2', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From BuildSearchKeywordFunction.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N'[dbo].[BuildSearchKeywordFunction]'))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = 'CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit nvarchar(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name nvarchar(MAX)
DECLARE @pos INT
if(@stringToSplit!='' '') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('' '', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('' '', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='' '')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='' '')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END'
EXEC(@dateString)
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N''[dbo].[BuildSearchKeywordFunction]''))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = ''CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit nvarchar(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name nvarchar(MAX)
DECLARE @pos INT
if(@stringToSplit!='''' '''') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('''' '''', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('''' '''', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='''' '''')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='''' '''')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END''
EXEC(@dateString)', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '598fa43f8677d92057a73ea7ffba534c', [Version] = 2 WHERE [SxmlFileName] = 'BuildSearchKeywordFunction.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

