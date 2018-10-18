
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CustomerReference' AND ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CustomerReference' AND ObjectTableId = (select Id from ObjectTables where Name = 'Master'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CustomerReference' AND ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from ObjectFields where FieldName = 'CustomerReference' AND ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')
go

delete from ObjectFields where FieldName = 'CustomerReference' AND ObjectTableId = (select Id from ObjectTables where Name = 'Master')
go

delete from ObjectFields where FieldName = 'CustomerReference' AND ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

delete from TextCodes where Code = 'Shipment.F.CustomerReference'
delete from TextCodes where Code = 'Shipment.CH.CustomerReferenceListLable'
delete from TextCodes where Code = 'Shipment.CustomerReferenceHelpText'

delete from TextCodes where Code = 'Master.F.CustomerReference'
delete from TextCodes where Code = 'Master.CH.CustomerReferenceListLable'
delete from TextCodes where Code = 'Master.CustomerReferenceHelpText'

delete from TextCodes where Code = 'Quote.F.CustomerReference'
delete from TextCodes where Code = 'Quote.CH.CustomerReferenceListLable'
delete from TextCodes where Code = 'Quote.CustomerReferenceHelpText'

