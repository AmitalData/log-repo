
select OperationallyClosedByUserId, OperationallyClosedByUserName, * from ShipmentComputedFields

-- OperationallyClosedByUserName
-- 1 Update All Table Set to null
-- 2 Select existing data
--select Shipments.Id, Contacts.EnglishName from Shipments
--join Contacts on Shipments.OperationalClosedByUserId = Contacts.Id
--where Shipments.IsOperationalClosed = 1 
--and Shipments.OperationalClosedByUserId is not null

-- NumberOfDeliveries
-- 1 Update All Table Set to 0
-- 2 Select existing data
select Shipments.Id, count(ShipmentPickUpDeliveries.Id)
from Shipments
left outer join ShipmentPickUpDeliveries on ShipmentPickUpDeliveries.ShipmentId = Shipments.Id
where ShipmentPickUpDeliveries.PickUpDeliveryTypeCode = 'DELV'
group by Shipments.Id

-- Last Pickup Fields
-- 1 Update All Table Set to null
-- 2 Select existing data

-- 1-90
-- 1-91
select * from ShipmentPickUpDeliveries where PickUpDeliveryTypeCode = 'PICK' and ShipmentId = '1-90'


select ShipmentId, MAX(Id)
from ShipmentPickUpDeliveries
where PickUpDeliveryTypeCode = 'PICK' and ShipmentId = '1-90'
group by ShipmentId