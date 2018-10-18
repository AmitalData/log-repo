--RUN THIS SCRIPT AFTER UPDATE

select Tenant,FreightPrepaidCollectId,OtherPrepaidCollectId,AWBChargesCodeCode from shipments 
where AWBChargesCodeCode is null

update shipments 
set AWBChargesCodeCode = 'PP'
where FreightPrepaidCollectId = 'P' and OtherPrepaidCollectId = 'P' and AWBChargesCodeCode is  null

update shipments 
set AWBChargesCodeCode = 'CC'
where FreightPrepaidCollectId = 'C' and OtherPrepaidCollectId = 'C' and AWBChargesCodeCode is  null

update shipments 
set AWBChargesCodeCode = 'PC'
where FreightPrepaidCollectId = 'P' and OtherPrepaidCollectId = 'C' and AWBChargesCodeCode is  null

update shipments 
set AWBChargesCodeCode = 'PC'
where FreightPrepaidCollectId = 'C' and OtherPrepaidCollectId = 'P' and AWBChargesCodeCode is  null