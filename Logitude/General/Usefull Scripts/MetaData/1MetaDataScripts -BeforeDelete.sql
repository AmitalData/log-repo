----MetaData All Scripts: Never Apply these scripts

----*--Before Delete--*--
----ObjectFields
--update querycolumns set ObjectFieldCode = (select fieldcode from objectfields where id=querycolumns.ObjectFieldId) where tenant!=0
--update ScreenFields set ObjectFieldCode = (select fieldcode from objectfields where id=ScreenFields.ObjectFieldId) where tenant!=0
--update AdvancedQueryFilters set ObjectFieldCode = (select fieldcode from objectfields where id=AdvancedQueryFilters.ObjectFieldId) where tenant!=0
--update RuleConditionFields set ObjectFieldCode = (select fieldcode from objectfields where id=RuleConditionFields.ObjectFieldId) where tenant!=0
--update ObjectTableRuleFields set ObjectFieldCode = (select fieldcode from objectfields where id=ObjectTableRuleFields.ObjectFieldId) where tenant!=0
--update ObjectTableRules set TriggerFieldCode = (select fieldcode from objectfields where id=ObjectTableRules.TriggerFieldId) where tenant!=0
--update AirlineMessagingRules set RuleFieldCode = (select fieldcode from objectfields where id=AirlineMessagingRules.RuleFieldId) where tenant!=0
--update restrictions  set ObjectFieldCode = (select fieldcode from objectfields where id=restrictions.ObjectFieldId) where tenant!=0
--update CustomerFieldsUpdateSettings  set ObjectFieldCode = (select fieldcode from objectfields where id=CustomerFieldsUpdateSettings.ObjectFieldId) where tenant!=0
--update ObjectFieldValidations  set ObjectFieldCode = (select fieldcode from objectfields where id=ObjectFieldValidations.ObjectFieldId) where tenant!=0
--update ObjectFieldModifications  set ObjectFieldCode = (select fieldcode from objectfields where id=ObjectFieldModifications.ObjectFieldId) where tenant!=0

----Screens
--update ScreenModifications set ScreenCode = (select Code from Screens where Id=ScreenModifications.ScreenId) where tenant!=0
--update ScreenFields set ScreenCode = (select Code from Screens where Id=ScreenFields.ScreenId) where tenant!=0
--update ObjectTables set HeaderScreenCode = (select Code from Screens where Id=ObjectTables.HeaderScreenId) where tenant!=0

----Queries
--update Queries set OriginalQueryCode = (select q1.UniqueCode from Queries q1 where q1.Id = Queries.OriginalQueryId)
--update AdvancedQueryFilters set QueryCode = (select Queries.UniqueCode from Queries where Id = AdvancedQueryFilters.QueryId)
--update QueryColumns set QueryCode = (select Queries.UniqueCode from Queries where Id = QueryColumns.QueryId)
--update SharedUserQueries set QueryCode = (select Queries.UniqueCode from Queries where Id = SharedUserQueries.QueryId)

----TextCodes
--update Queries set NameTextCodeCode = (select Code from TextCodes where Id=Queries.NameTextCodeId) where tenant!=0
--update ObjectTables set DescriptionTextCodeCode = (select Code from TextCodes where Id=ObjectTables.DescriptionTextCodeId) where tenant!=0
--update ObjectTables set NewButtonTextCodeCode = (select Code from TextCodes where Id=ObjectTables.NewButtonTextCodeId) where tenant!=0
--update Features set NameTextCodeCode = (select Code from TextCodes where Id=Features.NameTextCodeId) where tenant!=0
--update Tips set ShortTextCodeCode = (select Code from TextCodes where Id=Tips.ShortTextCode) where tenant!=0
--update ObjectTableTabs set TabNameTextCodeCode = (select Code from TextCodes where Id=ObjectTableTabs.TabNameTextCodeId) where tenant!=0
--update MenuButtons set LabelTextCodeCode = (select Code from TextCodes where Id=MenuButtons.LabelTextCodeId) where tenant!=0
--update Translations set TextCodeCode = (select Code from TextCodes where Id=Translations.TextCodeId) where tenant!=0
--update ObjectFields set FullNameTextCodeCode = (select Code from TextCodes where Id=ObjectFields.FullNameTextCodeId) where tenant!=0
--update ObjectFields set HelpTextCodeCode = (select Code from TextCodes where Id=ObjectFields.HelpTextCodeId) where tenant!=0
--update ObjectFields set ShortNameTextCodeCode = (select Code from TextCodes where Id=ObjectFields.ShortNameTextCodeId) where tenant!=0
--update ObjectFields set ListTextCodeCode = (select Code from TextCodes where Id=ObjectFields.ListTextCodeId) where tenant!=0

----Features
--update MenusTables set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=MenusTables.FeatureId) where tenant!=0
--update Queries set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=Queries.FeatureId) where tenant!=0
--update PackageFeatures set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=PackageFeatures.FeatureId) where tenant!=0
--update RoleFeatures set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=RoleFeatures.FeatureId) where tenant!=0
--update Reports set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=Reports.FeatureId) where tenant!=0
--update ObjectTableTabs set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=ObjectTableTabs.FeatureId) where tenant!=0
--update ObjectTableHelperControls set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=ObjectTableHelperControls.FeatureId) where tenant!=0
--update MenuButtons set FeatureUniqeCode = (select FeatureUniqeCode from features where Id=MenuButtons.FeatureId) where tenant!=0



if object_id('tempdb..#TempOldFeatures') is not null
drop table #TempOldFeatures

select * into #TempOldFeatures
from (select Code,FeatureUniqeCode,Tenant
		from features 
		where IsOld = 1) as t

		--select * from #TempOldFeatures


		if object_id('tempdb..#TempIsSpellCheckedTextCodes') is not null
drop table #TempIsSpellCheckedTextCodes

select * into #TempIsSpellCheckedTextCodes
from (select *
		from TextCodes 
		where IsSpellChecked = 1) as t

		--select * from #TempIsSpellCheckedTextCodes