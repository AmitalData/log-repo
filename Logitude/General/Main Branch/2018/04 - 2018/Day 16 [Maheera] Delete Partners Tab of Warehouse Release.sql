delete from ObjectTableTabs where Code= 'PARE'
delete from  Features where ObjectTableId = (select id from ObjectTables where name = 'warehouserelease') and Code ='WarehouseRelease.Tab.Partners'