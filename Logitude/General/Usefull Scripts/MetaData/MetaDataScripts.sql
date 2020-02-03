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
----update Queries set UniqueCode = ((select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.Code) where tenant!=0
----update Queries set UniqueCode = ((select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.Code) where tenant =0
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


----*--Delete--*--
----ObjectFields
----alter table advancedqueryfilters drop constraint FK_ObjectFieldAdvancedQueryFilter
--delete from objectfields where tenant = 0 and (FieldCode not like 'customs.%' or FieldCode is null)
--delete from querycolumns where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null
--delete from ScreenFields where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) 
--delete from AdvancedQueryFilters where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null
--delete from RuleConditionFields where tenant = 0  and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and systemlevel = 1
--delete from ObjectTableRuleFields where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and systemlevel = 1
--delete from ObjectTableRules where tenant = 0 and (TriggerFieldCode not like 'customs.%' or TriggerFieldCode is null) and systemlevel = 1

----Screens
--delete from screens where tenant = 0 and (Code not like 'customs.%' or Code is null)

----Queries
--delete from Queries where tenant = 0 and (Code not like 'customs.%' or Code is null) and userid is null

----TextCodes
--delete from MenuButtons where tenant = 0 and (LabelTextCodeCode not like 'customs.%' or LabelTextCodeCode is null)
--delete from ObjectTableTabs where tenant = 0 and (TabNameTextCodeCode not like 'customs.%' or TabNameTextCodeCode is null)
--delete from textcodes where tenant = 0 and (code not like 'customs.%' or code is null)
--delete from Tips where tenant = 0 and (ShortTextCodeCode not like 'customs.%' or ShortTextCodeCode is null)

----Features
--delete from Features where tenant = 0 and (NameTextCodeCode not like 'customs.%' or NameTextCodeCode is null)
----delete from MenusTables where tenant = 0 and (FeatureUniqeCode not like 'customs.%' or FeatureUniqeCode is null)


----*--After Delete--*--
----ObjectFields
----delete from querycolumns where objectfieldcode not in(select fieldcode from ObjectFields)
--update querycolumns set ObjectFieldId = (select Id from objectfields where FieldCode=querycolumns.ObjectFieldCode)
----delete from ScreenFields where objectfieldcode not in(select fieldcode from ObjectFields)
--update ScreenFields set ObjectFieldId = (select Id from objectfields where FieldCode=ScreenFields.ObjectFieldCode)
----delete from AdvancedQueryFilters where objectfieldcode not in(select fieldcode from ObjectFields)
--update AdvancedQueryFilters set ObjectFieldId = (select Id from objectfields where FieldCode=AdvancedQueryFilters.ObjectFieldCode)
--update RuleConditionFields set ObjectFieldId = (select Id from objectfields where FieldCode=RuleConditionFields.ObjectFieldCode)
--update ObjectTableRuleFields set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectTableRuleFields.ObjectFieldCode)
--update ObjectTableRules set TriggerFieldId = (select Id from objectfields where FieldCode=ObjectTableRules.TriggerFieldCode)
--update AirlineMessagingRules set RuleFieldId = (select Id from objectfields where FieldCode=AirlineMessagingRules.RuleFieldCode)
--update restrictions  set ObjectFieldId = (select Id from objectfields where FieldCode=restrictions.ObjectFieldCode)
--update CustomerFieldsUpdateSettings  set ObjectFieldId = (select Id from objectfields where FieldCode=CustomerFieldsUpdateSettings.ObjectFieldCode)
--update ObjectFieldValidations  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldValidations.ObjectFieldCode)
----delete from ObjectFieldModifications where objectfieldcode not in(select fieldcode from ObjectFields)
--update ObjectFieldModifications  set ObjectFieldId = (select Id from objectfields where FieldCode=ObjectFieldModifications.ObjectFieldCode)

----Screens
----delete from ScreenModifications where ScreenCode not in(select code from Screens)
--update ScreenModifications set ScreenId = (select Id from Screens where code=ScreenModifications.ScreenCode)
----delete from ScreenFields where ScreenCode not in(select code from Screens)
--update ScreenFields set ScreenId = (select Id from Screens where code=ScreenFields.ScreenCode)
--update ObjectTables set HeaderScreenId = (select Id from Screens where code=ObjectTables.HeaderScreenCode)
----select ScreenCode,tenant,count(*) from ScreenModifications group by ScreenCode,tenant having count(*)>1
----select * from ScreenModifications where ScreenCode = 'Master.HeaderScreen'
----delete from ScreenModifications where id='??'

----Queries
--update Queries set OriginalQueryId =  (select q1.Id from Queries q1 where q1.UniqueCode = Queries.OriginalQueryCode)
--update AdvancedQueryFilters set QueryId = (select Id from Queries where UniqueCode = AdvancedQueryFilters.QueryCode)
--update QueryColumns set QueryId = (select Id from Queries where UniqueCode = QueryColumns.QueryCode)
--update SharedUserQueries set QueryId = (select Id from Queries where UniqueCode = SharedUserQueries.QueryCode)

----TextCodes
--update Queries set NameTextCodeId = (select Id from TextCodes where Code=Queries.NameTextCodeCode and tenant = Queries.Tenant)
--update ObjectTables set DescriptionTextCodeId = (select Id from TextCodes where Code=ObjectTables.DescriptionTextCodeCode and tenant = ObjectTables.Tenant)
--update ObjectTables set NewButtonTextCodeId = (select Id from TextCodes where Code=ObjectTables.NewButtonTextCodeCode)
--update Features set NameTextCodeId = (select Id from TextCodes where Code=Features.NameTextCodeCode and tenant = Features.Tenant)
--update Tips set ShortTextCode = (select Id from TextCodes where Code=Tips.ShortTextCodeCode)
--update ObjectTableTabs set TabNameTextCodeId = (select Id from TextCodes where Code=ObjectTableTabs.TabNameTextCodeCode and tenant = ObjectTableTabs.Tenant)
--update MenuButtons set LabelTextCodeId = (select Id from TextCodes where Code=MenuButtons.LabelTextCodeCode)
--update ObjectFields set FullNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.FullNameTextCodeCode and tenant = ObjectFields.Tenant) 
--update ObjectFields set HelpTextCodeId = (select Id from TextCodes where Code=ObjectFields.HelpTextCodeCode and tenant = ObjectFields.Tenant)
--update ObjectFields set ShortNameTextCodeId = (select Id from TextCodes where Code=ObjectFields.ShortNameTextCodeCode and tenant = ObjectFields.Tenant)
--update ObjectFields set ListTextCodeId = (select Id from TextCodes where Code=ObjectFields.ListTextCodeCode and tenant = ObjectFields.Tenant)
----delete from Translations where TextCodeCode not in (select code from TextCodes)
--update Translations set TextCodeId = (select Id from TextCodes where Code=Translations.TextCodeCode and (tenant =  Translations.Tenant or tenant = 0)) where TextCodeCode in (select code from textcodes)

----Features
--update MenusTables set FeatureId = (select Id from features where FeatureUniqeCode=MenusTables.FeatureUniqeCode)
--update Queries set FeatureId = (select Id from features where FeatureUniqeCode=Queries.FeatureUniqeCode)
--update ObjectTableTabs set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableTabs.FeatureUniqeCode)
--update ObjectTableHelperControls set FeatureId = (select Id from features where FeatureUniqeCode=ObjectTableHelperControls.FeatureUniqeCode)
--update MenuButtons set FeatureId = (select Id from features where FeatureUniqeCode=MenuButtons.FeatureUniqeCode)
----delete from Reports where FeatureUniqeCode not in (select FeatureUniqeCode from Features)
--update Reports set FeatureId = (select Id from features where FeatureUniqeCode=Reports.FeatureUniqeCode)
----delete from PackageFeatures where FeatureUniqeCode not in (select FeatureUniqeCode from Features)
----delete from PackageFeatures where FeatureUniqeCode is null
--update PackageFeatures set FeatureId = (select Id from features where FeatureUniqeCode=PackageFeatures.FeatureUniqeCode)
----delete from RoleFeatures where FeatureUniqeCode not in (select FeatureUniqeCode from Features)
----delete from RoleFeatures where FeatureUniqeCode is null
--update RoleFeatures set FeatureId = (select Id from features where FeatureUniqeCode=RoleFeatures.FeatureUniqeCode)

-------------
--delete from Screens where code='Customs.AccountingPartner.HeaderScreen'
--update ScreenModifications set NumberOfRows=3 where NumberOfRows>3 and ScreenCode like '%headerscreen%'