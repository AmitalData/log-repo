
update Queries
set DefaultSortColumn = 'CreateDateTime'
where Code = 'Expected Departures' and ObjectTableId = (select Id from ObjectTables where Name = 'shipment')
