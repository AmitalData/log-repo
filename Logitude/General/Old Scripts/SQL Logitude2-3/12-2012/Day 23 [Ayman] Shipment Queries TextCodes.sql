-- ASK IHAB IF WE SHOULD DELETE QUERIES !!! 

-- later
delete from AdvancedQueryFilters
delete from QueryColumns
delete from Queries

delete from TextCodes where Code = 'Shipment.Q.OperationalOpen'
delete from TextCodes where Code = 'Shipment.Q.AccountingOpen'
delete from TextCodes where Code = 'Shipment.Q.AllFollowUps'
delete from TextCodes where Code = 'Shipment.Q.LastWeeksUpdate'
delete from TextCodes where Code = 'Shipment.Q.AllShipment'
delete from TextCodes where Code = 'Shipment.Q.CancelledShipment'

delete from ObjectFields where FieldName = 'OpenShipments' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')
delete from ObjectFields where FieldName = 'AccountingOpen' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')
delete from ObjectFields where FieldName = 'LastWeeksUpdate' and ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment')
