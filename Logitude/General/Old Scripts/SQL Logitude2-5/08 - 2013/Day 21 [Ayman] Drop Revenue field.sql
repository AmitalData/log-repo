
delete from ObjectFields where FieldName = 'Revenue' and ObjectTableId = (Select Id from ObjectTables where Name = 'OpportunityProduct')
go

delete from TextCodes where Code like '%OpportunityProduct%Revenue%'
go
