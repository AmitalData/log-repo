delete from ObjectFields where fieldname='id' and objecttableid = ( select id from objecttables where name='customs.declaration')
delete from ObjectFields where fieldname = 'Name' and objecttableid = ( select id from objecttables where name='Customs.Cargoidentifiretype')
delete from ObjectFields where FieldName= 'ImporterNumber' and ObjectTableId=( select id from objecttables where name='customs.declaration')
delete from QueryColumns where ObjectFieldId = ( select id from ObjectFields where FieldName='ImporterNumber' and objecttableid= ( select id from objecttables where name='customs.declaration'))
delete from ScreenFields where ObjectFieldId = ( select id from ObjectFields where FieldName='ImporterNumber' and objecttableid= ( select id from objecttables where name='customs.declaration'))
Update TextCodes set DefaultText= 'Declaration' where code='Customs.Declaration'

