

-- update database migration to add field "ProductCode"
-- then Run this Script

update Shipments set ProductCode = 'CI' where DirectionId = 'C'
go

update Shipments set ProductCode = TransportModeId + DirectionId where DirectionId != 'C'
go