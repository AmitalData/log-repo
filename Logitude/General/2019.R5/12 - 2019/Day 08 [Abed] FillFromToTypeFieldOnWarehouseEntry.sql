update WarehouseEntries SET FromTypeCode = 'PORT' where FromTypeCode is null
update WarehouseEntries SET ToTypeCode = 'PORT' where ToTypeCode is null