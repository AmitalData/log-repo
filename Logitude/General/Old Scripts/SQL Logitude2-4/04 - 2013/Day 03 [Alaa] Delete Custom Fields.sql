

delete from ScreenFields where ObjectFieldId= ( select id from objectfields where FieldName='CheckQueueTypeCode')
go
delete from ObjectFields where FieldName='CheckQueueTypeCode'
go


delete from ScreenFields where ObjectFieldId= ( select id from objectfields where FieldName='CheckQueueTypeName')
go
delete from ObjectFields where FieldName='CheckQueueTypeName'
go

delete from ObjectFields where fieldname='StatusMessage'
go
delete from QueryColumns where ObjectFieldId = ( select id from ObjectFields where FieldName='StatusMessage')
go


delete from ObjectFields where FieldName= 'TypeDestination'
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where FieldName='TypeDestination')

delete from QueryColumns where ObjectFieldId in ( select id from ObjectFields where FieldName='DeclerationNo')
delete from ScreenFields where ObjectFieldId= (select id from ObjectFields where FieldName='DeclerationNo')
delete from ObjectFields where FieldName='DeclerationNo'

delete from QueryColumns where ObjectFieldId =( select id from ObjectFields where FieldName= 'CargoIdentifierTypeId')
delete from ObjectFields where FieldName='CargoIdentifierTypeId'

delete from QueryColumns where ObjectFieldId=(select id from ObjectFields where FieldName='InitiatorTypeId')
delete from ObjectFields where FieldName='InitiatorTypeId'
delete from QueryColumns where ObjectFieldId = ( select id from ObjectFields where FieldName= 'QueueTypeId')
delete from ObjectFields where FieldName= 'QueueTypeId'
delete from QueryColumns where ObjectFieldId = ( select id from ObjectFields where FieldName= 'StorageSiteId')
delete from ObjectFields where FieldName= 'StorageSiteId'

