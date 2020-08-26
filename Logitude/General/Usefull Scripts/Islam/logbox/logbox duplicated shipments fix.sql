 
declare @ForwarderShipmentNumber as varchar(20)
declare @ForwarderPartnerId as varchar(15)
declare @Tenant as int
declare @Count as int
declare duplicatedRecordsCursor Cursor read_only
for



select forwardershipmentnumber,forwarderpartnerid,tenant,count(*) from shipments 
--inner join shipmentcomputedfields on (shipmentcomputedfields.id=shipments.id)
where  forwardershipmentnumber is not null and iscancelled=0 
--and shipments.id in (select entityid from documentsfilings where createdate>'07.19.20')
group by forwardershipmentnumber,forwarderpartnerid,tenant
having count(*) >1
order by forwarderpartnerid



open duplicatedRecordsCursor
fetch next from duplicatedRecordsCursor into @ForwarderShipmentNumber,@ForwarderPartnerId,@Tenant,@Count
while @@FETCH_STATUS = 0
begin
print '--------------------------------------------------------------------------------------------------------------------'
print  + '@Count:'  + cast(@Count as varchar(20))   +' @ForwarderShipmentNumber: '+@ForwarderShipmentNumber + ' @ForwarderPartnerId:' + @ForwarderPartnerId + ' @Tenant:'  + cast(@Tenant as varchar(20))



select id,CreateDateTime from shipments
where ForwarderShipmentNumber = @ForwarderShipmentNumber and ForwarderPartnerId = @ForwarderPartnerId and Tenant = @Tenant
order by CreateDateTime desc



declare @EmptyDocsRecordsCount as int
set @EmptyDocsRecordsCount = (select count(*) from shipments
where  ForwarderShipmentNumber = @ForwarderShipmentNumber and ForwarderPartnerId = @ForwarderPartnerId and Tenant = @Tenant
and id not in (select entityid from DocumentsFilings where  EntityId is not null and  ObjectTableId in (select id from objecttables where Name = 'shipment' or name = 'master') and tenant = @Tenant)
)



if(@Count = @EmptyDocsRecordsCount)
begin
print @ForwarderShipmentNumber + ': all shipments have no documents! <====================================='

update shipments set IsCancelled = 1
where  ForwarderShipmentNumber = @ForwarderShipmentNumber and ForwarderPartnerId = @ForwarderPartnerId and Tenant = @Tenant
and id not in (select entityid from DocumentsFilings where  EntityId is not null and  ObjectTableId in (select id from objecttables where Name = 'shipment' or name = 'master') and tenant = @Tenant)


-- we cancel all
-- which one should be cancelled?
end
else
begin
if(@EmptyDocsRecordsCount = 0)
begin
print 'all shipments are connected to documents @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'



declare @TopEntityId as varchar(15)



set @TopEntityId = (select top 1 EntityId from DocumentsFilings 
where tenant = @Tenant
and EntityId  in (select id from shipments where ForwarderShipmentNumber = @ForwarderShipmentNumber and ForwarderPartnerId = @ForwarderPartnerId and Tenant = @Tenant)
group by EntityId
order by count(*) desc)



update  DocumentsFilings  set EntityId = @TopEntityId
where tenant = @Tenant and EntityId <> @TopEntityId
and EntityId  in (select id from shipments where ForwarderShipmentNumber = @ForwarderShipmentNumber and ForwarderPartnerId = @ForwarderPartnerId and Tenant = @Tenant)




print @TopEntityId



--1-4495667 --1-4382605 --1-4495666



update shipments set IsCancelled = 1
where  ForwarderShipmentNumber = @ForwarderShipmentNumber and ForwarderPartnerId = @ForwarderPartnerId and Tenant = @Tenant
and id not in (select entityid from DocumentsFilings where  EntityId is not null and  ObjectTableId in (select id from objecttables where Name = 'shipment' or name = 'master') and tenant = @Tenant)




end
else
begin
print cast(@EmptyDocsRecordsCount as varchar(20)) + ' of ' +  cast(@Count as varchar(20)) + ' is cancelled'



update shipments set IsCancelled = 1
where  ForwarderShipmentNumber = @ForwarderShipmentNumber and ForwarderPartnerId = @ForwarderPartnerId and Tenant = @Tenant
and id not in (select entityid from DocumentsFilings where  EntityId is not null and  ObjectTableId in (select id from objecttables where Name = 'shipment' or name = 'master') and tenant = @Tenant)




end



end

fetch next from duplicatedRecordsCursor into @ForwarderShipmentNumber,@ForwarderPartnerId,@Tenant,@Count
end
close duplicatedRecordsCursor
deallocate duplicatedRecordsCursor
































----ALTER TABLE shipments ADD  CONSTRAINT [UQ_ForwarderShipmentNumber_ForwarderPartnerId_Tenant_Shipments] UNIQUE NONCLUSTERED 
----(
----    [ForwarderShipmentNumber] ASC,
----    [ForwarderPartnerId] ASC,
------    [Tenant] ASC
----)




--select ForwarderPartnerId,ForwarderShipmentNumber,Tenant,count(*) from shipments 
--where ForwarderShipmentNumber is null
--group by ForwarderPartnerId,ForwarderShipmentNumber,Tenant
--having count(*) > 1




--select forwardershipmentnumber,forwarderpartnerid,tenant from shipments 
----inner join shipmentcomputedfields on (shipmentcomputedfields.id=shipments.id)
--where  forwardershipmentnumber is not null and iscancelled=0 
----and shipments.id in (select entityid from documentsfilings where createdate>'07.19.20')
--group by forwardershipmentnumber,forwarderpartnerid,tenant
--having count(*) >1
--order by forwarderpartnerid




----select * from DocumentsFilings where EntityReference = '76072968'




----select * from Shipments where id in ('1-6449665','1-6449667','1-6449666')



--select * from documentsfilings where entityid in ('1-4498406','1-4495667') 
--select ShipmentNumber,forwardershipmentnumber,id,tenant,ForwarderPartnerId from Shipments where id in ('1-4498406','1-4495667')



--select * from shipments where ForwarderShipmentNumber = '4205020619'



----select id from shipments
----where ForwarderShipmentNumber = '4207418799' and ForwarderPartnerId = '1-2' and Tenant = 1369
----and id not in (select entityid from DocumentsFilings where  ObjectTableId in (select id from objecttables where Name = 'shipment' or name = 'master') and tenant = 1369)



