

IF OBJECT_ID('[dbo].[usp_ReconnectObjectTableMetadata]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata]
GO
create PROCEDURE [dbo].[usp_ReconnectObjectTableMetadata]
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







 End