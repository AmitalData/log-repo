

update Queries
set SpotlightDataTemplate = null
where ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go