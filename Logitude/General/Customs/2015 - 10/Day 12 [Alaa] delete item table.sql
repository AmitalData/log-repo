update TextCodes set code = REPLACE (Code, '.Item.', '.CustomsPartnersItem.') where code like '%.Item.%' and objecttableid = (select id from ObjectTables where Name ='customs.CustomsPartnersItem')

select * from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.item')

delete from ScreenFields where ObjectFieldId in (select id from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.item'))

delete from QueryColumns where QueryId = (select id from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.item'))

delete from Queries where ObjectTableId = (select id from ObjectTables where name ='customs.item')

delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name ='customs.item')

delete from RoleFeatures where FeatureId in ( select id from Features where ObjectTableId = (select id from ObjectTables where name ='customs.item') ) 

delete from PackageFeatures where FeatureId in ( select id from Features where ObjectTableId = (select id from ObjectTables where name ='customs.item') ) 

update Queries set FeatureId = (select Id from Features where ObjectTableId = (select id from objecttables where name ='customs.CustomsPartnersItem' ) and code ='ITEM') where objecttableid = (select id from objecttables where name ='customs.CustomsPartnersItem') and code ='CustomsPartnersItem'


delete from Features where ObjectTableId = (select id from ObjectTables where name ='customs.item')

update ObjectTableTabs set TabNameTextCodeId =(select id from textcodes where code = 'Customs.CustomsPartnersItem.TH.General' and objecttableid =(select id from objecttables where name ='customs.CustomsPartnersItem')) where code ='ITGN'  and ObjectTableId =(select id from objecttables where name ='customs.CustomsPartnersItem')
update ObjectTableTabs set TabNameTextCodeId =(select id from textcodes where code = 'Customs.CustomsPartnersItem.TH.Events' and objecttableid =(select id from objecttables where name ='customs.CustomsPartnersItem')) where code ='ITEV'  and ObjectTableId =(select id from objecttables where name ='customs.CustomsPartnersItem')
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name ='customs.item')

update ObjectTables set HeaderScreenId = null where Id =(select id from ObjectTables where name ='customs.item')
delete from   Screens where objecttableid =(select id from ObjectTables where name ='customs.item') 

delete from ObjectTables where name ='customs.item'




delete from TextCodes where code ='Customs.Bank.Features.Banks'  and ObjectTableId = (select id from ObjectTables where Name = 'Customs.CurrencyType')
delete from TextCodes where code ='Customs.Bank.Features.Banks'  and ObjectTableId = (select id from ObjectTables where Name = 'Customs.ReturnCondition')


