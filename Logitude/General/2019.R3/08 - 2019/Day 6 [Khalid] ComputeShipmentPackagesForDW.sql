update ShipmentComputedFields set ContainersNumbers = null

declare @Tenant integer
declare @ShipmentId varchar(15)
declare @ContainerNumbers varchar(4000)

declare @MemoryTable table
(
  Tenant int not null,
  ShipmentId varchar(15) not null,
  ContainerNumbers varchar(4000) not null
)

insert into @MemoryTable
SELECT Tenant, ShipmentId, ContainerNumber = 
    STUFF((SELECT ', ' + ContainerNumber
           FROM ShipmentPackages b 
           WHERE b.ShipmentId = a.ShipmentId 
          FOR XML PATH('')), 1, 2, '')
FROM ShipmentPackages a
where ContainerNumber is not null
group by Tenant, ShipmentId


       DECLARE DataCursor CURSOR READ_ONLY
       FOR
       SELECT Tenant, ShipmentId, ContainerNumbers
       From @MemoryTable
       OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant, @ShipmentId, @ContainerNumbers
       WHILE @@FETCH_STATUS = 0
       BEGIN
              update ShipmentComputedFields set ContainersNumbers = @ContainerNumbers where Id = @ShipmentId and Tenant = @Tenant
       FETCH NEXT FROM DataCursor INTO @Tenant, @ShipmentId, @ContainerNumbers    
       END
       CLOSE DataCursor
       DEALLOCATE DataCursor

