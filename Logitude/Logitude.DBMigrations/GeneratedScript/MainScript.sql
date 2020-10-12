-- Create Index On Documents Table
EXEC('CREATE NONCLUSTERED INDEX [IX_Documents_HasFile] ON [dbo].[Documents]([HasFile])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5b2f0b5e-554a-4b51-a482-c4f812427119', 'Document.dxml', 'Documents', 'HasFile', 'Create Index', GETDATE(), '-- Create Index On Documents TableEXEC(''CREATE NONCLUSTERED INDEX [IX_Documents_HasFile] ON [dbo].[Documents]([HasFile])'');');


-- Create Index On DocumentsFilings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_CreateDate] ON [dbo].[DocumentsFilings]([CreateDate])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4e917c55-2987-484e-b4da-8f9d62f3bb8c', 'DocumentsFiling.dxml', 'DocumentsFilings', 'CreateDate', 'Create Index', GETDATE(), '-- Create Index On DocumentsFilings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_CreateDate] ON [dbo].[DocumentsFilings]([CreateDate])'');');

-- Create Index On DocumentsFilings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_EntityId_ChildEntityId] ON [dbo].[DocumentsFilings]([EntityId],[ChildEntityId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a6f65705-cbf4-41a8-a5e4-fd2c3d99843f', 'DocumentsFiling.dxml', 'DocumentsFilings', 'EntityId,ChildEntityId', 'Create Index', GETDATE(), '-- Create Index On DocumentsFilings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_EntityId_ChildEntityId] ON [dbo].[DocumentsFilings]([EntityId],[ChildEntityId])'');');

-- Create Index On DocumentsFilings Table
EXEC('CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_ChildEntityId] ON [dbo].[DocumentsFilings]([ChildEntityId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('e740cb68-7dbf-40d9-8bf0-0295a0256218', 'DocumentsFiling.dxml', 'DocumentsFilings', 'ChildEntityId', 'Create Index', GETDATE(), '-- Create Index On DocumentsFilings TableEXEC(''CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_ChildEntityId] ON [dbo].[DocumentsFilings]([ChildEntityId])'');');


-- Change Size From -1 To 6 For Column CustomsMetaDataCode
ALTER TABLE [dbo].[DocumentsMetaDataTypes] ALTER COLUMN [CustomsMetaDataCode] NVARCHAR(6);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a2c6de87-4eb2-4b0b-8f2c-2e1e0d175402', 'DocumentsMetaDataType.dxml', 'DocumentsMetaDataTypes', 'CustomsMetaDataCode', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 6 For Column CustomsMetaDataCodeALTER TABLE [dbo].[DocumentsMetaDataTypes] ALTER COLUMN [CustomsMetaDataCode] NVARCHAR(6);');


-- Change Size From 8000 To -1 For Column Notes
ALTER TABLE [dbo].[FeatureChanges] ALTER COLUMN [Notes] VARCHAR(MAX);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('6067fe5c-b9c0-4fce-bed2-4e299d1ce172', 'FeatureChange.dxml', 'FeatureChanges', 'Notes', 'Alter Column Size', GETDATE(), '-- Change Size From 8000 To -1 For Column NotesALTER TABLE [dbo].[FeatureChanges] ALTER COLUMN [Notes] VARCHAR(MAX);');


-- Change Type From nvarchar To varchar For Column Description
ALTER TABLE [dbo].[Reports] ALTER COLUMN [Description] VARCHAR(250);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('97bd7c3b-388e-40e1-944f-0ebaf479d474', 'Report.dxml', 'Reports', 'Description', 'Alter Column Type', GETDATE(), '-- Change Type From nvarchar To varchar For Column DescriptionALTER TABLE [dbo].[Reports] ALTER COLUMN [Description] VARCHAR(250);');

-- Change Type From nvarchar To varchar For Column SearchFields
ALTER TABLE [dbo].[Reports] ALTER COLUMN [SearchFields] VARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('ed0fe9e1-60b1-48a9-a1e7-6b0ea67a6b83', 'Report.dxml', 'Reports', 'SearchFields', 'Alter Column Type', GETDATE(), '-- Change Type From nvarchar To varchar For Column SearchFieldsALTER TABLE [dbo].[Reports] ALTER COLUMN [SearchFields] VARCHAR(1000);');

-- Unset Nullable For Column Code
ALTER TABLE [dbo].[Reports] ALTER COLUMN [Code] VARCHAR(4) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('46a36d69-e66a-4757-939c-3e07e6735bb4', 'Report.dxml', 'Reports', 'Code', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column CodeALTER TABLE [dbo].[Reports] ALTER COLUMN [Code] VARCHAR(4) NOT NULL;');


-- Add New Column With Name SearchWindowFiltersIndex
ALTER TABLE [dbo].[ObjectFields] ADD [SearchWindowFiltersIndex] INT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('97554e03-4497-46d2-a286-d7b79472a8bb', 'ObjectField.dxml', 'ObjectFields', 'SearchWindowFiltersIndex', 'Add Column', GETDATE(), '-- Add New Column With Name SearchWindowFiltersIndexALTER TABLE [dbo].[ObjectFields] ADD [SearchWindowFiltersIndex] INT DEFAULT(0) NOT NULL;');

-- Add New Column With Name AllowedInCustFieldsSettings
ALTER TABLE [dbo].[ObjectFields] ADD [AllowedInCustFieldsSettings] BIT DEFAULT(0) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('47fe53da-2028-4867-be14-8827af3047e9', 'ObjectField.dxml', 'ObjectFields', 'AllowedInCustFieldsSettings', 'Add Column', GETDATE(), '-- Add New Column With Name AllowedInCustFieldsSettingsALTER TABLE [dbo].[ObjectFields] ADD [AllowedInCustFieldsSettings] BIT DEFAULT(0) NOT NULL;');

-- Set Nullable For Column DisplayInSearchWindowFiltersIndex
ALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [DisplayInSearchWindowFiltersIndex] INT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('2924e392-18a0-4424-8544-dc2806c177f3', 'ObjectField.dxml', 'ObjectFields', 'DisplayInSearchWindowFiltersIndex', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column DisplayInSearchWindowFiltersIndexALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [DisplayInSearchWindowFiltersIndex] INT NULL;');

-- Drop Column DisplayInSearchWindowFiltersIndex
EXEC SP_RENAME 'dbo.ObjectFields.DisplayInSearchWindowFiltersIndex', 'Drop_DisplayInSearchWindowFiltersIndex', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5f56a36b-9bfb-457b-9889-404c8075fbb5', 'ObjectField.dxml', 'ObjectFields', 'DisplayInSearchWindowFiltersIndex', 'Drop Column', GETDATE(), '-- Drop Column DisplayInSearchWindowFiltersIndexEXEC SP_RENAME ''dbo.ObjectFields.DisplayInSearchWindowFiltersIndex'', ''Drop_DisplayInSearchWindowFiltersIndex'', ''COLUMN'';');

-- Set Nullable For Column AllowedInCustomerFieldsSettings
ALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [AllowedInCustomerFieldsSettings] BIT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7339c385-1d67-4288-aef7-89ac485e4ea7', 'ObjectField.dxml', 'ObjectFields', 'AllowedInCustomerFieldsSettings', 'Set Column Nullable', GETDATE(), '-- Set Nullable For Column AllowedInCustomerFieldsSettingsALTER TABLE [dbo].[ObjectFields] ALTER COLUMN [AllowedInCustomerFieldsSettings] BIT NULL;');

-- Drop Column AllowedInCustomerFieldsSettings
EXEC SP_RENAME 'dbo.ObjectFields.AllowedInCustomerFieldsSettings', 'Drop_AllowedInCustomerFieldsSettings', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3fd2053f-22fc-4803-a7e5-a4c99c826158', 'ObjectField.dxml', 'ObjectFields', 'AllowedInCustomerFieldsSettings', 'Drop Column', GETDATE(), '-- Drop Column AllowedInCustomerFieldsSettingsEXEC SP_RENAME ''dbo.ObjectFields.AllowedInCustomerFieldsSettings'', ''Drop_AllowedInCustomerFieldsSettings'', ''COLUMN'';');


-- Change Size From -1 To 500 For Column SplitComponentPath
ALTER TABLE [dbo].[ObjectTables] ALTER COLUMN [SplitComponentPath] NVARCHAR(500);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('8baf4712-50f8-4684-9c32-8e98a263bda4', 'ObjectTable.dxml', 'ObjectTables', 'SplitComponentPath', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 500 For Column SplitComponentPathALTER TABLE [dbo].[ObjectTables] ALTER COLUMN [SplitComponentPath] NVARCHAR(500);');


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

