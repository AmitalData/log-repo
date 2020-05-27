
--alter table RuleConditionFields drop constraint [FK_RuleConditionFieldsObjectTableRule]
--alter table Customs.CustomsRequiredFields drop constraint [FK_Customs.CustomsRequiredFields_dbo.ObjectFields_ObjectfieldId]
----MetaData All Scripts: Never Apply these scripts
--alter table querycolumns drop constraint [FK_QueryColumnObjectField]
--alter table RuleConditionFields drop constraint FK_RuleConditionFieldsObjectTableRule
--alter table ObjectFields drop constraint FK_TextCodeObjectField1
--alter table Queries drop constraint FK_QueryTextCode
--alter table AdvancedQueryFilters drop constraint FK_ObjectFieldAdvancedQueryFilter
----*--Delete--*--
----ObjectFields
delete from objectfields where tenant = 0 and (FieldCode  like 'customs.%' or FieldCode is null)
delete from querycolumns where tenant = 0 and (ObjectFieldCode  like 'customs.%' or ObjectFieldCode is null) and userid is null
delete from ScreenFields where tenant = 0 and (ObjectFieldCode  like 'customs.%' or ObjectFieldCode is null) 
delete from AdvancedQueryFilters where tenant = 0 and (ObjectFieldCode  like 'customs.%' or ObjectFieldCode is null) and userid is null
delete from RuleConditionFields where tenant = 0  and (ObjectFieldCode  like 'customs.%' or ObjectFieldCode is null) and ObjectTableRuleId not in (select id from ObjectTableRules where SystemLevel=0 )
delete from ObjectTableRuleFields where tenant = 0 and (ObjectFieldCode  like 'customs.%' or ObjectFieldCode is null) and systemlevel = 1
delete from ObjectTableRules where tenant = 0 and (TriggerFieldCode  like 'customs.%' or TriggerFieldCode is null) and systemlevel = 1

----Screens
delete from screens where tenant = 0 and (Code  like 'customs.%' or Code is null)

----Queries
delete from Queries where tenant = 0 and (Code  like 'customs.%' or Code is null) and userid is null and systemlevel = 1

----textcodes
delete from MenuButtons where tenant = 0 and (LabelTextCodeCode like 'customs.%' or LabelTextCodeCode is null)
delete from ObjectTableTabs where tenant = 0 and (TabNameTextCodeCode like 'customs.%' or TabNameTextCodeCode is null)
delete from textcodes where tenant = 0 and (code like 'customs.%' or code is null) and code not in (select NameTextCodeCode from queries where tenant=0 and userid is not null and SystemLevel=0 and NameTextCodeCode is not null)

---- features
delete from Features where tenant = 0 and (NameTextCodeCode like 'customs.%' or NameTextCodeCode is null)



