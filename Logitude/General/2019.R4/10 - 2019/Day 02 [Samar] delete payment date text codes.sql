

declare @ObjectTableId varchar (15)
declare @ObjectFieldId varchar (15)

set @ObjectTableId = (select Id from ObjectTables where Name = 'ARPayment')
set @ObjectFieldId = (select Id from ObjectFields where FieldName = 'PaymentDate' and ObjectTableId = @ObjectTableId)


delete from QueryColumns where ObjectFieldId = @ObjectFieldId
delete from ObjectFields where Id = @ObjectFieldId
delete from TextCodes where ObjectTableId = @ObjectTableId and Code like '%PaymentDate%'
