
--1)
update shipments
set AWBChargeRate = AWBPrintRate
where AWBChargeRate is null

--2)
alter table shipments drop column AWBPrintRate
alter table ShipmentDataViews drop column AWBPrintRate

--3)
drop view ShipmentDataView

--4)
-- run the script again "ShipmentDataView.sql"

