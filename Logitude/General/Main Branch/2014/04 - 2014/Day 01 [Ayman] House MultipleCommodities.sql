
update Shipments set IsMultipleCommodities = 0 where ShipmentLevelCode = 'H' AND IsMultipleCommodities = 1
go