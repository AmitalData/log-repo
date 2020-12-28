-- General Script From 202012021000_UpdateNullableGLAccountFields.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update glaccountmoredatas set balanceinlocalcurrency = 0 where balanceinlocalcurrency is null
update glaccountmoredatas set LocalBalanceInDue = 0 where LocalBalanceInDue is null
ALTER TABLE glaccountmoredatas ADD CONSTRAINT DF_DefaultValue_Balance DEFAULT 0 FOR balanceinlocalcurrency
ALTER TABLE glaccountmoredatas ADD CONSTRAINT DF_DefaultValue_LocalBalance DEFAULT 0 FOR LocalBalanceInDue
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202012021000_UpdateNullableGLAccountFields.sxml', GETDATE(), 'update glaccountmoredatas set balanceinlocalcurrency = 0 where balanceinlocalcurrency is null
update glaccountmoredatas set LocalBalanceInDue = 0 where LocalBalanceInDue is null
ALTER TABLE glaccountmoredatas ADD CONSTRAINT DF_DefaultValue_Balance DEFAULT 0 FOR balanceinlocalcurrency
ALTER TABLE glaccountmoredatas ADD CONSTRAINT DF_DefaultValue_LocalBalance DEFAULT 0 FOR LocalBalanceInDue', DATEDIFF(MS,@StartTime,@EndTime), 'f6a32ac16065ffe3a675f99af1129e2e', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- Unset Nullable For Column BalanceInLocalCurrency
ALTER TABLE [dbo].[GLAccountMoreDatas] ALTER COLUMN [BalanceInLocalCurrency] DECIMAL(16, 2) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5c1e19b7-bf90-4c05-b8a4-98870b3034c0', 'GLAccountMoreData.dxml', 'GLAccountMoreDatas', 'BalanceInLocalCurrency', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column BalanceInLocalCurrencyALTER TABLE [dbo].[GLAccountMoreDatas] ALTER COLUMN [BalanceInLocalCurrency] DECIMAL(16, 2) NOT NULL;');

-- Unset Nullable For Column LocalBalanceInDue
ALTER TABLE [dbo].[GLAccountMoreDatas] ALTER COLUMN [LocalBalanceInDue] DECIMAL(16, 2) NOT NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('848c63d8-f887-4480-b7e6-192e7f206526', 'GLAccountMoreData.dxml', 'GLAccountMoreDatas', 'LocalBalanceInDue', 'Unset Column Nullable', GETDATE(), '-- Unset Nullable For Column LocalBalanceInDueALTER TABLE [dbo].[GLAccountMoreDatas] ALTER COLUMN [LocalBalanceInDue] DECIMAL(16, 2) NOT NULL;');


-- Change Size From -1 To 6 For Column CustomsMetaDataCode
ALTER TABLE [dbo].[DocumentsMetaDataTypes] ALTER COLUMN [CustomsMetaDataCode] NVARCHAR(6);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a041d499-f8ce-4dd0-9e0c-699c123d3ccc', 'DocumentsMetaDataType.dxml', 'DocumentsMetaDataTypes', 'CustomsMetaDataCode', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 6 For Column CustomsMetaDataCodeALTER TABLE [dbo].[DocumentsMetaDataTypes] ALTER COLUMN [CustomsMetaDataCode] NVARCHAR(6);');


-- Change Size From 8000 To -1 For Column Notes
ALTER TABLE [dbo].[FeatureChanges] ALTER COLUMN [Notes] VARCHAR(MAX);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('64127d23-0d00-4844-88a7-158991acb553', 'FeatureChange.dxml', 'FeatureChanges', 'Notes', 'Alter Column Size', GETDATE(), '-- Change Size From 8000 To -1 For Column NotesALTER TABLE [dbo].[FeatureChanges] ALTER COLUMN [Notes] VARCHAR(MAX);');


-- Change Size From -1 To 500 For Column SplitComponentPath
ALTER TABLE [dbo].[ObjectTables] ALTER COLUMN [SplitComponentPath] NVARCHAR(500);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('3f53ed6f-b19e-43ab-a071-e9a5f40d2cc7', 'ObjectTable.dxml', 'ObjectTables', 'SplitComponentPath', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 500 For Column SplitComponentPathALTER TABLE [dbo].[ObjectTables] ALTER COLUMN [SplitComponentPath] NVARCHAR(500);');


-- Change Size From 250 To 1000 For Column Notes
ALTER TABLE [dbo].[Tariffs] ALTER COLUMN [Notes] NVARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('83cc961a-c539-4f4b-b182-0e8d6f03f117', 'Tariff.dxml', 'Tariffs', 'Notes', 'Alter Column Size', GETDATE(), '-- Change Size From 250 To 1000 For Column NotesALTER TABLE [dbo].[Tariffs] ALTER COLUMN [Notes] NVARCHAR(1000);');


-- Change Size From 500 To 1000 For Column Notes
ALTER TABLE [dbo].[TariffLines] ALTER COLUMN [Notes] NVARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('eb2e6e34-a716-4de5-8f83-378a7e8f8d88', 'TariffLine.dxml', 'TariffLines', 'Notes', 'Alter Column Size', GETDATE(), '-- Change Size From 500 To 1000 For Column NotesALTER TABLE [dbo].[TariffLines] ALTER COLUMN [Notes] NVARCHAR(1000);');


-- Create Unique Constraint On TariffLinesContainersPrices Table
EXEC('ALTER TABLE [dbo].[TariffLinesContainersPrices] ADD CONSTRAINT [UQ_TariffLinesContainersPrices_Tenant_TariffId_TariffLineId_SurchargeId] UNIQUE([Tenant],[TariffId],[TariffLineId],[SurchargeId])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('21de274e-96ab-4f03-9019-bfa7498d76ca', 'TariffLinesContainersPrice.dxml', 'TariffLinesContainersPrices', 'Tenant,TariffId,TariffLineId,SurchargeId', 'Create Unique Constraint', GETDATE(), '-- Create Unique Constraint On TariffLinesContainersPrices TableEXEC(''ALTER TABLE [dbo].[TariffLinesContainersPrices] ADD CONSTRAINT [UQ_TariffLinesContainersPrices_Tenant_TariffId_TariffLineId_SurchargeId] UNIQUE([Tenant],[TariffId],[TariffLineId],[SurchargeId])'');');


-- Procedure Script From usp_UpdateCardSearchFunction.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCardSearchFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCardSearchFunction] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateCardSearchFunction]
(
@CardId varchar(15)
)
AS
declare  @Tenant int
declare  @EnglishName varchar(70)
declare  @LocalName nvarchar(100)
declare  @VatNumber varchar(20)
declare  @CityName nvarchar(25)
declare  @CountryName varchar(120)
declare  @Code varchar(15)
declare  @ReceivablesAccountingCard varchar(25)
declare  @PayablesAccountingCard varchar(25)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
declare  @IsCustomer bit
if (@CardId is not null)
begin
delete CardSearches where CardId = @CardId
select
@Tenant = Tenant,
@Code = Code,
@EnglishName = EnglishName,
@LocalName = LocalName,
@VatNumber = VatNumber,
@CityName = CityName,
@CountryName =CountryName,
@ReceivablesAccountingCard = ReceivablesAccountingCard,
@PayablesAccountingCard = PayablesAccountingCard,
@CreateDate = CreateDate,
@UpdateDate = UpdateDate,
@PartnerTypeId = PartnerTypeId,
@InActive = InActive,
@IsCustomer= IsCustomer
from Cards
where Id = @CardId
set @Weight = 0
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
if (@Code is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@Code , 90 , 90) t where KeyWord !='' '' end
if (@EnglishName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@EnglishName , 100 , 90) t where KeyWord !='' '' end
if (@LocalName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  @IsCustomer,t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@LocalName , 100 , 90) t where KeyWord !='' '' end
if (@VatNumber is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , IsCustomer,Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@VatNumber , 100 , 100) t where KeyWord !='' '' end
if (@CountryName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , IsCustomer,Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,@IsCustomer,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CountryName , 50 , 50) t where KeyWord !='' '' end
if (@CityName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , IsCustomer,Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CityName , 40 , 40) t where KeyWord !='' '' end
if (@ReceivablesAccountingCard is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  @IsCustomer,t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ReceivablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
if (@PayablesAccountingCard is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  @IsCustomer,t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@PayablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
if(@IsCustomer = 1  or @PartnerTypeId = ''CS'' or @PartnerTypeId = ''PO'')
begin
declare  @ContactName varchar(60)
declare  @ContactEmail varchar(70)
DECLARE ContactsCursor CURSOR READ_ONLY
FOR
SELECT EnglishName , Email
From Contacts
where Id in (select ContactId from CardContacts where CardId = @CardId and Tenant = @Tenant)
OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO  @ContactName ,@ContactEmail
WHILE @@FETCH_STATUS = 0
BEGIN
begin
if (@ContactName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ContactName , 30 , 30) t where KeyWord !='' '' end
if (@ContactEmail is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ContactEmail , 30 , 30) t where KeyWord !='' '' end
end
FETCH NEXT FROM ContactsCursor INTO  @ContactName ,@ContactEmail
END
CLOSE ContactsCursor
DEALLOCATE ContactsCursor
end
end');


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


-- General Script From 202011101133_AvailableForScheduling.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update reports set AvailableForScheduling=1 where code='CSSR'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202011101133_AvailableForScheduling.sxml', GETDATE(), 'update reports set AvailableForScheduling=1 where code=''CSSR''', DATEDIFF(MS,@StartTime,@EndTime), '82e2a98e8a0da883b61defa92463acb6', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

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

-- General Script From 202011190832_UpdateEORInumberInCustomerToField1Value.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update customers set EORInumber = Field1 where Tenant = 2264
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202011190832_UpdateEORInumberInCustomerToField1Value.sxml', GETDATE(), 'update customers set EORInumber = Field1 where Tenant = 2264', DATEDIFF(MS,@StartTime,@EndTime), '03a869ba6c4d4cf9ac7fb5b2e97587c4', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202012080440_FillContactCardSearchData.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete CardSearches WHERE Weight = 30
If(OBJECT_ID('tempdb..#temp_CardSearches') Is Not Null)
Begin
Drop Table #temp_CardSearches
End
CREATE TABLE #temp_CardSearches
(
[Tenant] [int] NOT NULL,
[RecordDate] [datetime] NOT NULL,
[Keyword] [nvarchar](100) NULL,
[Weight] [int] NOT NULL,
[CardId] [varchar](15) NULL,
[PartnerTypeId] [varchar](2) not NULL,
[InActive] bit,
[IsCustomer] bit
)
declare  @Tenant int
declare @Count as int
set @Count = 0;
declare  @CardId varchar(15)
declare  @EnglishName varchar(70)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
declare  @ContactName varchar(70)
declare  @ContactEmail varchar(70)
declare  @IsCustomer bit
DECLARE ContactsCursor CURSOR READ_ONLY
FOR
select Cards.Id ,Cards.Tenant , Cards.InActive,Cards.CreateDate , Cards.UpdateDate ,Cards.IsCustomer , Cards.PartnerTypeId,Contacts.EnglishName,Contacts.Email
from Contacts
INNER JOIN CardContacts ON CardContacts.ContactId = Contacts.Id
INNER JOIN Cards ON Cards.Id = CardContacts.CardId
where Contacts.Id in (select ContactId from CardContacts where CardId = Cards.Id and Tenant = Cards.Tenant) and (Cards.IsCustomer = 1  or Cards.PartnerTypeId = 'CS' or Cards.PartnerTypeId = 'PO')
OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO  @CardId,@Tenant, @InActive,  @CreateDate , @UpdateDate ,@IsCustomer,  @PartnerTypeId , @ContactName , @ContactEmail
WHILE @@FETCH_STATUS = 0
BEGIN
begin
set @Weight = 0
DECLARE  @newId varchar(100) ;
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
BEGIN TRY
if (@ContactName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ContactName , 30 , 30) t where KeyWord !=' ' end
if (@ContactEmail is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ContactEmail , 30 , 30) t where KeyWord !=' ' end
set @Count = @Count + 1;
if(@Count = 500000)
begin
insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive, IsCustomer) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive,IsCustomer from #temp_CardSearches
truncate table #temp_CardSearches
set @Count = 0
end
END TRY
BEGIN CATCH
declare @Exception as varchar(4000)
set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);
set @Exception = @Exception + ' (@CardId: ' + @CardId +') '+ ' (@Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
RAISERROR(@Exception, 16, 3);
RETURN;
END CATCH
end
FETCH NEXT FROM ContactsCursor INTO  @CardId,@Tenant, @InActive,  @CreateDate , @UpdateDate ,@IsCustomer,  @PartnerTypeId , @ContactName , @ContactEmail
END
CLOSE ContactsCursor
DEALLOCATE ContactsCursor
if (@Count > 0) begin  insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive , IsCustomer) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive , IsCustomer from #temp_CardSearches end
drop table #temp_CardSearches
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202012080440_FillContactCardSearchData.sxml', GETDATE(), 'delete CardSearches WHERE Weight = 30
If(OBJECT_ID(''tempdb..#temp_CardSearches'') Is Not Null)
Begin
Drop Table #temp_CardSearches
End
CREATE TABLE #temp_CardSearches
(
[Tenant] [int] NOT NULL,
[RecordDate] [datetime] NOT NULL,
[Keyword] [nvarchar](100) NULL,
[Weight] [int] NOT NULL,
[CardId] [varchar](15) NULL,
[PartnerTypeId] [varchar](2) not NULL,
[InActive] bit,
[IsCustomer] bit
)
declare  @Tenant int
declare @Count as int
set @Count = 0;
declare  @CardId varchar(15)
declare  @EnglishName varchar(70)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
declare  @ContactName varchar(70)
declare  @ContactEmail varchar(70)
declare  @IsCustomer bit
DECLARE ContactsCursor CURSOR READ_ONLY
FOR
select Cards.Id ,Cards.Tenant , Cards.InActive,Cards.CreateDate , Cards.UpdateDate ,Cards.IsCustomer , Cards.PartnerTypeId,Contacts.EnglishName,Contacts.Email
from Contacts
INNER JOIN CardContacts ON CardContacts.ContactId = Contacts.Id
INNER JOIN Cards ON Cards.Id = CardContacts.CardId
where Contacts.Id in (select ContactId from CardContacts where CardId = Cards.Id and Tenant = Cards.Tenant) and (Cards.IsCustomer = 1  or Cards.PartnerTypeId = ''CS'' or Cards.PartnerTypeId = ''PO'')
OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO  @CardId,@Tenant, @InActive,  @CreateDate , @UpdateDate ,@IsCustomer,  @PartnerTypeId , @ContactName , @ContactEmail
WHILE @@FETCH_STATUS = 0
BEGIN
begin
set @Weight = 0
DECLARE  @newId varchar(100) ;
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
BEGIN TRY
if (@ContactName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ContactName , 30 , 30) t where KeyWord !='' '' end
if (@ContactEmail is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer, t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ContactEmail , 30 , 30) t where KeyWord !='' '' end
set @Count = @Count + 1;
if(@Count = 500000)
begin
insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive, IsCustomer) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive,IsCustomer from #temp_CardSearches
truncate table #temp_CardSearches
set @Count = 0
end
END TRY
BEGIN CATCH
declare @Exception as varchar(4000)
set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);
set @Exception = @Exception + '' (@CardId: '' + @CardId +'') ''+ '' (@Tenant: '' + CAST(@Tenant as varchar(100)) + '' )''
RAISERROR(@Exception, 16, 3);
RETURN;
END CATCH
end
FETCH NEXT FROM ContactsCursor INTO  @CardId,@Tenant, @InActive,  @CreateDate , @UpdateDate ,@IsCustomer,  @PartnerTypeId , @ContactName , @ContactEmail
END
CLOSE ContactsCursor
DEALLOCATE ContactsCursor
if (@Count > 0) begin  insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive , IsCustomer) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive , IsCustomer from #temp_CardSearches end
drop table #temp_CardSearches', DATEDIFF(MS,@StartTime,@EndTime), '2d3dac014f7ba1d0603783dde25ad9b7', 4);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202010270950_AddIsRegionalTAXToQuoteTemplateTextCode.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @QuoteTemplateId as varchar(15)
declare @QuoteTemplateTextCodeId as varchar(15)
declare @Tenant as int
declare @NewEntityId as varchar(15)
DECLARE QuoteTemplateCursor CURSOR READ_ONLY
FOR
SELECT Id , Tenant
From QuoteTemplates
OPEN QuoteTemplateCursor FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @QuoteTemplateTextCodeId = (select Id from QuoteTemplateTextCodes where TextCode = 'ISREGIONALTAXPACKAGES' AND QuoteTemplateId = @QuoteTemplateId AND Tenant = @Tenant )
if(@QuoteTemplateTextCodeId is null)
begin
EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'ISREGIONALTAXPACKAGES', 'Is Regional Tax', 'Is Regional Tax' , @QuoteTemplateId ,'Packages' , 'Is Regional Tax', 'Is Regional Tax' );
EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'ISREGIONALTAXCONTAINERS',  'Is Regional Tax', 'Is Regional Tax', @QuoteTemplateId ,'Containers' , 'Is Regional Tax', 'Is Regional Tax' );
end
FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
End
CLOSE QuoteTemplateCursor
DEALLOCATE QuoteTemplateCursor
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202010270950_AddIsRegionalTAXToQuoteTemplateTextCode.sxml', GETDATE(), 'declare @QuoteTemplateId as varchar(15)
declare @QuoteTemplateTextCodeId as varchar(15)
declare @Tenant as int
declare @NewEntityId as varchar(15)
DECLARE QuoteTemplateCursor CURSOR READ_ONLY
FOR
SELECT Id , Tenant
From QuoteTemplates
OPEN QuoteTemplateCursor FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @QuoteTemplateTextCodeId = (select Id from QuoteTemplateTextCodes where TextCode = ''ISREGIONALTAXPACKAGES'' AND QuoteTemplateId = @QuoteTemplateId AND Tenant = @Tenant )
if(@QuoteTemplateTextCodeId is null)
begin
EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,''QuoteTemplateTextCode''
INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , ''ISREGIONALTAXPACKAGES'', ''Is Regional Tax'', ''Is Regional Tax'' , @QuoteTemplateId ,''Packages'' , ''Is Regional Tax'', ''Is Regional Tax'' );
EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,''QuoteTemplateTextCode''
INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , ''ISREGIONALTAXCONTAINERS'',  ''Is Regional Tax'', ''Is Regional Tax'', @QuoteTemplateId ,''Containers'' , ''Is Regional Tax'', ''Is Regional Tax'' );
end
FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant
End
CLOSE QuoteTemplateCursor
DEALLOCATE QuoteTemplateCursor', DATEDIFF(MS,@StartTime,@EndTime), '4f8f3e01defe4c9710ea7ebe67d27440', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009152130_AddNewTariffProducts.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @Code = 'EXP'
set @Name = 'Express'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'DOM'
set @Name = 'Domestic'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'LAN'
set @Name = 'Live Animals'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'PER'
set @Name = 'Perishables'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'TEP'
set @Name = 'Temperature Control'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'HUM'
set @Name = 'Human Remains'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'PRS'
set @Name = 'Personal Effects'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'AUT'
set @Name = 'Automotive Vehicles'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'VAL'
set @Name = 'Valuable Goods'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'ULD'
set @Name = 'ULD'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'PAR'
set @Name = 'Pharma'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'VUL'
set @Name = 'Vulnerable '
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'DIP'
set @Name = 'Diplomatic'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'AER'
set @Name = 'Aerospace '
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
set @Code = 'SML'
set @Name = 'Small Packages'
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffProduct'
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + ',' + @Name)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'TariffProduct')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009152130_AddNewTariffProducts.sxml', GETDATE(), 'declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @Code = ''EXP''
set @Name = ''Express''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''DOM''
set @Name = ''Domestic''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''LAN''
set @Name = ''Live Animals''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''PER''
set @Name = ''Perishables''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''TEP''
set @Name = ''Temperature Control''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''HUM''
set @Name = ''Human Remains''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''PRS''
set @Name = ''Personal Effects''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''AUT''
set @Name = ''Automotive Vehicles''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''VAL''
set @Name = ''Valuable Goods''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''ULD''
set @Name = ''ULD''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''PAR''
set @Name = ''Pharma''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''VUL''
set @Name = ''Vulnerable ''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''DIP''
set @Name = ''Diplomatic''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''AER''
set @Name = ''Aerospace ''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
set @Code = ''SML''
set @Name = ''Small Packages''
if not exists (select * from TariffProducts where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''TariffProduct''
insert into TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
values ( @NewId,@Tenant, @Code, @Name, @Name, 0, @Code + '','' + @Name)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''TariffProduct'')', DATEDIFF(MS,@StartTime,@EndTime), 'be7115e43b48bd5a2a8dee1d6e3deaa5', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

