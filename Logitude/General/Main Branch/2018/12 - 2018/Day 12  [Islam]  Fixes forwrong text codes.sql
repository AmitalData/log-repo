update textcodes set code = 'ShipmentPayable.F.ChargesTypeId' where code = 'ShipmentPayable.f.chargestype' and objecttableid in (select id from objecttables where name = 'ShipmentPayable')
update textcodes set code = 'ShipmentReceivable.F.ChargesTypeId' where code = 'ShipmentReceivable.f.chargestype' and objecttableid in (select id from objecttables where name = 'ShipmentReceivable')


update textcodes set code = 'ShipmentPayable.F.MeasurementId' where code = 'ShipmentPayable.f.Measurement' and objecttableid in (select id from objecttables where name = 'ShipmentPayable')
update textcodes set code = 'ShipmentReceivable.F.MeasurementId' where code = 'ShipmentReceivable.f.Measurement' and objecttableid in (select id from objecttables where name = 'ShipmentReceivable')

update textcodes set code = 'Card.F.EnglishName' where code = 'Card.f.English Name' and objecttableid in (select id from objecttables where name = 'Card')

 