delete from menubuttons
where eventcode='SendFNA'
go

delete from menubuttons
where eventcode='SendFSA'
go

delete from menubuttons
where eventcode='SendFMA'
go

delete from textcodes
where code='Shipment.B.SendFSA'

delete from textcodes
where code='Shipment.B.SendFNA'

delete from textcodes
where code='Shipment.B.SendFMA'

delete from textcodes
where code='Master.B.SendFSA'

delete from textcodes
where code='Master.B.SendFNA'

delete from textcodes
where code='Master.B.SendFMA'

delete from textcodes
where code='Shipment.Features.SendFSA'

delete from textcodes
where code='Shipment.Features.SendFNA'

delete from textcodes
where code='Shipment.Features.SendFMA'

delete from textcodes
where code='Master.Features.SendFSA'

delete from textcodes
where code='Master.Features.SendFNA'

delete from textcodes
where code='Master.Features.SendFMA'

delete from RoleFeatures where featureid=(select id from features where code='SENDFSA' and objecttableid=(select id from objecttables where name='Shipment'))
delete from RoleFeatures where featureid=(select id from features where code='SENDFSA' and objecttableid=(select id from objecttables where name='Master'))


delete from RoleFeatures where featureid=(select id from features where code='SENDFNA' and objecttableid=(select id from objecttables where name='Shipment'))
delete from RoleFeatures where featureid=(select id from features where code='SENDFNA' and objecttableid=(select id from objecttables where name='Master'))


delete from RoleFeatures where featureid=(select id from features where code='SENDFMA' and objecttableid=(select id from objecttables where name='Shipment'))
delete from RoleFeatures where featureid=(select id from features where code='SENDFMA' and objecttableid=(select id from objecttables where name='Master'))

delete from packageFeatures where featureid=(select id from features where code='SENDFSA' and objecttableid=(select id from objecttables where name='Shipment'))
delete from packageFeatures where featureid=(select id from features where code='SENDFSA' and objecttableid=(select id from objecttables where name='Master'))


delete from packageFeatures where featureid=(select id from features where code='SENDFNA' and objecttableid=(select id from objecttables where name='Shipment'))
delete from packageFeatures where featureid=(select id from features where code='SENDFNA' and objecttableid=(select id from objecttables where name='Master'))


delete from packageFeatures where featureid=(select id from features where code='SENDFMA' and objecttableid=(select id from objecttables where name='Shipment'))
delete from packageFeatures where featureid=(select id from features where code='SENDFMA' and objecttableid=(select id from objecttables where name='Master'))


delete from features where code='SENDFSA'
delete from features where code='SENDFNA'
delete from features where code='SENDFMA'