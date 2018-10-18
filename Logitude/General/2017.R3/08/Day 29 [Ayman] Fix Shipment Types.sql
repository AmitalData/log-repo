

update Quotes set ShipmentTypeId = null where TransportModeId = 'A' and ShipmentTypeId in ('Dirc','Cons')
update Quotes set ShipmentTypeId = 'FCLD' where TransportModeId = 'O' and ShipmentTypeId in ('Dirc','Cons')
update Quotes set ShipmentTypeId = 'FTL' where TransportModeId = 'I' and ShipmentTypeId in ('Dirc','Cons')

update Shipments set ShipmentTypeId = null where TransportModeId = 'A' and ShipmentTypeId in ('Dirc','Cons')
update Shipments set ShipmentTypeId = 'FCLD' where TransportModeId = 'O' and ShipmentTypeId in ('Dirc','Cons')
update Shipments set ShipmentTypeId = 'FTL' where TransportModeId = 'I' and ShipmentTypeId in ('Dirc','Cons')

delete from ShipmentTypes where Id in ('Dirc','Cons')
update ShipmentTypes set Name = 'My Groupage Ocean' where Id = 'MyGO'
update ShipmentTypes set Name = 'My Groupage Inland' where Id = 'MyGI'

select * from ShipmentTypes
