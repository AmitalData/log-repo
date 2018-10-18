insert into FieldDataTypes
values('SigDouble','SigDouble','SigDouble,SigDouble')

update ObjectFields 
set DataTypeCode = 'SigDouble'
where FieldName = 'UnitPrice' and ObjectTableId = (select Id from ObjectTables where Name = 'ShipmentReceivable')