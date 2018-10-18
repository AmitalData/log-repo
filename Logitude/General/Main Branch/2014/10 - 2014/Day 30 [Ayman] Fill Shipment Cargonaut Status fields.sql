

update Shipments set CargonautFHLStatusCode = 'NSEN' where CargonautFHLStatusCode is null
go

update ShipmentMasterDatas set CargonautFWBStatusCode = 'NSEN' where CargonautFWBStatusCode is null
go
