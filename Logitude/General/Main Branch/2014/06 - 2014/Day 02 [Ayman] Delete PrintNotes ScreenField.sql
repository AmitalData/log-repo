

delete from ScreenFields 
where 
ScreenId = (select Id from Screens where Code = 'ARInvoice.GeneralTabScreen' and ObjectTableId = (select Id from ObjectTables where Name = 'arinvoice'))
and
ObjectFieldId = (select Id from ObjectFields where FieldName = 'PrintNotes' and ObjectTableId = (select Id from ObjectTables where Name = 'arinvoice'))
go
