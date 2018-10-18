

delete from ObjectFields where FieldName = 'QuoteId' and ObjectTableId = (select Id from ObjectTables where Name = 'OpportunityProduct')
go

delete from TextCodes where Code like '%QuoteId%' and ObjectTableId = (select Id from ObjectTables where Name = 'OpportunityProduct')
go

delete from ObjectFields where FieldName = 'QuoteId' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code like '%QuoteId%' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go