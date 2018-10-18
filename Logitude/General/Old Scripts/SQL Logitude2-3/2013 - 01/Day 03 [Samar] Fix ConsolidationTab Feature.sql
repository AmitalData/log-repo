


update ObjectTableTabs set FeatureId = (Select Id from Features where Code = 'CONSOLIDATION' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
where Code = 'SHCO'