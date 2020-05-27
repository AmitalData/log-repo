
update WarehouseEntries 
set LastStatusUpdateDate = 
(select top(1) EventDateTime from TraceEvents where EntityId = WarehouseEntries.Id and ObjectTableId=(select id from ObjectTables where Name = 'WarehouseEntry') order by TraceEvents.EventDateTime desc)
 where LastStatusUpdateDate is null
