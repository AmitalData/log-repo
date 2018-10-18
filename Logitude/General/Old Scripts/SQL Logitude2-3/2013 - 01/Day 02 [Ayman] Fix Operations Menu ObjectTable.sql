
update MenusTables set ObjectTableId = (Select Id from ObjectTables where Name = 'Shipment') where Code = 'SHT'
