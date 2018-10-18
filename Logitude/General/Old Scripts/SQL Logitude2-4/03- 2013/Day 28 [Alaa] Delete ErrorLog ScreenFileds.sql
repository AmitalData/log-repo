delete from ScreenFields where ScreenId in (select id from Screens where code='ErrorLog.GeneralTabScreen') and
 ObjectFieldId = (select id from ObjectFields where FieldName='Tier' and ObjectTableId = (select id from ObjectTables where name='errorLog'))

 delete from ScreenFields where ScreenId in (select id from Screens where code='ErrorLog.GeneralTabScreen') and
 ObjectFieldId = (select id from ObjectFields where FieldName='UserName' and ObjectTableId = (select id from ObjectTables where name='errorLog'))

 delete from ScreenFields where ScreenId in (select id from Screens where code='ErrorLog.GeneralTabScreen') and
 ObjectFieldId = (select id from ObjectFields where FieldName='LogDate' and ObjectTableId = (select id from ObjectTables where name='errorLog'))