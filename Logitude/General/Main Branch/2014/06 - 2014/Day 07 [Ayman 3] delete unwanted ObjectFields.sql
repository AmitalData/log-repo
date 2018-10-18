

-- Run this Script then Update Tenants

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CustomerReference' AND ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'PartnerRefrenceNo' AND ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice'))
go

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'ARInvoice.GeneralTabScreen')
go

delete from ObjectFields where FieldName = 'CustomerReference'
go

delete from ObjectFields where FieldName = 'PartnerRefrenceNo'
go

delete from TextCodes where Code like '%CustomerReference%' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')
go

delete from TextCodes where Code like '%PartnerRefrenceNo%' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')
go
