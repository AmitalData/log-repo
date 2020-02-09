----MetaData All Scripts: Never Apply these scripts

----*--Delete--*--
----ObjectFields
--delete from objectfields where tenant = 0 and (FieldCode not like 'customs.%' or FieldCode is null)
--delete from querycolumns where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null
--delete from ScreenFields where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) 
--delete from AdvancedQueryFilters where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null
--delete from RuleConditionFields where tenant = 0  and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and ObjectTableRuleId not in (select id from ObjectTableRules where SystemLevel=0 )
--delete from ObjectTableRuleFields where tenant = 0 and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and systemlevel = 1
--delete from ObjectTableRules where tenant = 0 and (TriggerFieldCode not like 'customs.%' or TriggerFieldCode is null) and systemlevel = 1

----Screens
--delete from screens where tenant = 0 and (Code not like 'customs.%' or Code is null)

----Queries
--delete from Queries where tenant = 0 and (Code not like 'customs.%' or Code is null) and userid is null and systemlevel = 1

----TextCodes
--delete from MenuButtons where tenant = 0 and (LabelTextCodeCode not like 'customs.%' or LabelTextCodeCode is null)
--delete from ObjectTableTabs where tenant = 0 and (TabNameTextCodeCode not like 'customs.%' or TabNameTextCodeCode is null)
--delete from textcodes where tenant = 0 and (code not like 'customs.%' or code is null)

----Features
--delete from Features where tenant = 0 and (NameTextCodeCode not like 'customs.%' or NameTextCodeCode is null)
----delete from MenusTables where tenant = 0 and (FeatureUniqeCode not like 'customs.%' or FeatureUniqeCode is null)

--delete from Screens where code='Customs.AccountingPartner.HeaderScreen'
-------------
