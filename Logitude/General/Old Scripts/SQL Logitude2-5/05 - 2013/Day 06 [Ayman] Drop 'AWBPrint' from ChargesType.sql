

alter table ChargesTypes drop column AWBPrint
go

delete from AdvancedQueryFilters where QueryId = (Select Id from Queries where Code = 'Charges types')
go

delete from ObjectFields where FieldName = 'AWBPrint' AND ObjectTableId = (Select Id from ObjectTables where name = 'ChargesType')
go

delete from TextCodes where Code = 'ChargesType.F.AWBPrint'
go

delete from TextCodes where Code = 'ChargesType.AWBPrintHelpText'
go