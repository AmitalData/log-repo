

update ChargesTypes set IATACodeCode = 'MB' where IATACodeCode is null
go

update ShipmentAWBPrintOnlies set IATACodeCode = 'MB' where IATACodeCode is null
go

update ShipmentReceivables set IATACodeCode = 'MB' where IATACodeCode is null
go 

update ShipmentPayables set IATACodeCode = 'MB' where IATACodeCode is null
go 