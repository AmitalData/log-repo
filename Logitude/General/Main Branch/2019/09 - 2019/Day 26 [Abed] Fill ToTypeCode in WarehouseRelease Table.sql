update WarehouseReleases set ToTypeCode = 'PORT' where ToTypeCode is null

select * from PickUpDeliveryFromToTypes