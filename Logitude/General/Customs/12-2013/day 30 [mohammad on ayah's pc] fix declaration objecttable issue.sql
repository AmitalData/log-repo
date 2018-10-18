delete from EntityLastActivities where ObjectTableId=(select id from objecttables where name='declaration')
delete from QueryColumns where ObjectFieldId in (select id from ObjectFields where ObjectTableId=(select id from objecttables where name='Declaration'))
delete from AdvancedQueryFilters where ObjectFieldId in (select id from ObjectFields where ObjectTableId=(select id from objecttables where name='Declaration'))
delete from ScreenFields where ObjectFieldId in (select id from ObjectFields where ObjectTableId=(select id from objecttables where name='Declaration'))
delete from Screens where ObjectTableId=(select id from objecttables where name='Declaration')
delete from Queries where ObjectTableId=(select id from objecttables where name='Declaration')
delete from objectfields where ObjectTableId=(select id from objecttables where name='Declaration')
delete from RoleFeatures where FeatureId in (select id from Features where ObjectTableId=(select id from objecttables where name='Declaration'))
delete from ObjectTableTabs where ObjectTableId=(select id from objecttables where name='Declaration')
update Queries set FeatureId=(select id from Features where ObjectTableId=(select id from objecttables where name='Customs.Declaration') and Code='DECLARATION') where objecttableid=(select id from objecttables where name='Customs.Declaration')
delete from Features where ObjectTableId=(select id from objecttables where name='Declaration')
delete from TextCodes where ObjectTableId=(select id from objecttables where name='Declaration')
delete from MenusTables where ObjectTableId=(select id from objecttables where name='Declaration')
delete from ObjectTables where name='Declaration'






