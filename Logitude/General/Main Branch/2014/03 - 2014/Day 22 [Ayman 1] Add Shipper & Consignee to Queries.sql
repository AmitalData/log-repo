

-- Run this Script then update Tenants

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Airlines Updates' AND ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
go

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Expected Departures' AND ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
go