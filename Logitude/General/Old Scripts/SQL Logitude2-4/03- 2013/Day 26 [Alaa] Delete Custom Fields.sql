delete from ScreenFields where objectfieldid= ( select id from objectfields where FieldName='FileNo' and objecttableid = ( select id from objecttables where Name='physicalcheck'))
go
delete from QueryColumns where objectfieldid= ( select id from objectfields where FieldName='FileNo' and objecttableid = ( select id from objecttables where Name='physicalcheck'))
go
delete from objectfields where fieldname='FileNo' and objecttableid= ( select id from objecttables where name='physicalcheck')
go

delete from QueryColumns where objectfieldid= ( select id from objectfields where FieldName='CheckQueueTypeCode' and objecttableid = ( select id from objecttables where Name='physicalcheck'))
go
delete from ScreenFields where objectfieldid= ( select id from objectfields where FieldName='CheckQueueTypeCode' and objecttableid = ( select id from objecttables where Name='physicalcheck'))
go
delete from ObjectFields where FieldName= 'CheckQueueTypeCode'
go

delete from QueryColumns where objectfieldid= ( select id from objectfields where FieldName='CheckQueueTypeName' and objecttableid = ( select id from objecttables where Name='physicalcheck'))
go
delete from ScreenFields where objectfieldid= ( select id from objectfields where FieldName='CheckQueueTypeName' and objecttableid = ( select id from objecttables where Name='physicalcheck'))
go
delete from ObjectFields where FieldName= 'CheckQueueTypeName'
go

delete from TextCodes where DefaultText='Queue Type Code'
delete from TextCodes where Code= 'PhysicalCheck.CH.QueueTypeNameListLable'
delete from TextCodes where code= 'PhysicalCheck.F.QueueTypeName'


delete from ObjectFields where FieldName= 'QueueTypeName'
go
delete from ObjectFields where FieldName= 'QueueTypeCode'
go
delete from TextCodes where code= 'PhysicalCheck.QueueTypeCodeHelpText'
delete from TextCodes where code='PhysicalCheck.QueueTypeNameHelpText'
delete from TextCodes where code = 'PhysicalCheck.F.QueueTypeCode'
delete from TextCodes where code = 'PhysicalCheck.F.QueueTypeName'
