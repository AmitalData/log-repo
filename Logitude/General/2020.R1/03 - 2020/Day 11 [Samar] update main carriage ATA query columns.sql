
--After executing please do update shipment and build Zip files

declare @oldFieldId as varchar(15)
declare @newFieldId as varchar(15)
declare @tableId as varchar(15)

set @tableId = (select Id from ObjectTables where Name = 'Shipment')
set @oldFieldId = (select Id from ObjectFields where FieldName = 'MainCarriageATA' and ObjectTableId = @tableId)
set @newFieldId = (select Id from ObjectFields where FieldName = 'MainCarriageFinalDestinationATA' and ObjectTableId = @tableId)

delete from QueryColumns where ObjectFieldId = @newFieldId
update QueryColumns set ObjectFieldId = @newFieldId where ObjectFieldId = @oldFieldId


select * from QueryColumns where ObjectFieldId = @oldFieldId
select * from QueryColumns where ObjectFieldId = @newFieldId






