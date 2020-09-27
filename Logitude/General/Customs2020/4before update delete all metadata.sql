----MetaData All Scripts: Never Apply these scripts

----*--Delete--*--
----ObjectFields
delete from objectfields where tenant = 0 or FieldCode is null;--and (FieldCode not like 'customs.%' or FieldCode is null);
delete from querycolumns where tenant = 0 and userid is null or ObjectFieldCode is null; --and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null;
delete from ScreenFields where tenant = 0 or ObjectFieldCode is null; --and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) ;
delete from AdvancedQueryFilters where tenant = 0 and userid is null or ObjectFieldCode is null;--and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and userid is null;
delete from RuleConditionFields where tenant = 0  and ObjectTableRuleId not in (select id from ObjectTableRules where SystemLevel=0 ) or ObjectFieldCode is null;--and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and ObjectTableRuleId not in (select id from ObjectTableRules where SystemLevel=0 );
delete from ObjectTableRuleFields where tenant = 0 and systemlevel = 1 or ObjectFieldCode is null;--and (ObjectFieldCode not like 'customs.%' or ObjectFieldCode is null) and systemlevel = 1;
delete from ObjectTableRules where tenant = 0 and systemlevel = 1 or TriggerFieldCode is null;--and (TriggerFieldCode not like 'customs.%' or TriggerFieldCode is null) and systemlevel = 1;

----Screens
delete from screens where tenant = 0 or Code is null;--and (Code not like 'customs.%' or Code is null);

----Queries
delete from Queries where tenant = 0 and userid is null and systemlevel = 1 or Code is null;--and (Code not like 'customs.%' or Code is null) and userid is null and systemlevel = 1;
----customs queries
delete from Queries where tenant = 0 and userid is null and systemlevel = 1 or Code is null;--and (Code like 'customs.%' or Code is null) and userid is null and systemlevel = 1;


----TextCodes
delete from MenuButtons where tenant = 0 or LabelTextCodeCode is null;--and (LabelTextCodeCode not like 'customs.%' or LabelTextCodeCode is null);
delete from ObjectTableTabs where tenant = 0 or TabNameTextCodeCode is null;--and (TabNameTextCodeCode not like 'customs.%' or TabNameTextCodeCode is null);
delete from textcodes where tenant = 0  and code not in (select NameTextCodeCode from queries where tenant=0 and userid is not null and SystemLevel=0 and NameTextCodeCode is not null) or code is null;--and (code not like 'customs.%' or code is null) and code not in (select NameTextCodeCode from queries where tenant=0 and userid is not null and SystemLevel=0 and NameTextCodeCode is not null);



----Features
delete from Features where tenant = 0 or NameTextCodeCode is null;--and (NameTextCodeCode not like 'customs.%' or NameTextCodeCode is null);



----delete from MenusTables where tenant = 0 and (FeatureUniqeCode not like 'customs.%' or FeatureUniqeCode is null)


delete from Screens where code='Customs.AccountingPartner.HeaderScreen';
