delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where  FieldName = 'ItemId')
delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where  FieldName = 'ItemId' and ObjectTableId = (select id from ObjectTables where name ='customs.item'))
delete from ObjectFields where FieldName = 'ItemId'