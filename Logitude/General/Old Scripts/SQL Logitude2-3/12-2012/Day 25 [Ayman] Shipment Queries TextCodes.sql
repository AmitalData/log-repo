
-- ASK IHAB IF WE SHOULD DELETE QUERIES !!! 
-- later
delete from AdvancedQueryFilters
delete from QueryColumns
delete from Queries

delete from TextCodes where Code = 'Shipment.Q.LastWeekUpdate'
delete from ObjectFields where FieldName = 'LastWeekUpdate' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')