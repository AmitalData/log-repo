 
-- -- -- -- -- -- -- --  Run only on Cloud
----select count(*) from addresses where externalid is null
------- update addresses set externalid = id  where externalid is null
------ALTER TABLE [dbo].[Addresses] ADD  CONSTRAINT [UQ_Addresses_ExternalId_Tenant] UNIQUE NONCLUSTERED 
------(
------	[Tenant] ASC,
------	[ExternalId] ASC
------)

--BEGIN;

--declare @Tenant as int
--declare @ExternalId as varchar(15)
--declare @AddressId as varchar(15)

--	DECLARE AddressesCursor CURSOR READ_ONLY
--	FOR
--	SELECT ExternalId,Tenant
--	From Addresses
--	where ExternalId is not NULL --and tenant = 10
--	group by ExternalId,Tenant having count(*) >1
--    OPEN AddressesCursor FETCH NEXT FROM AddressesCursor INTO @ExternalId,@Tenant
--	WHILE @@FETCH_STATUS = 0
--	BEGIN
	
--   begin
--	print @ExternalId 
--	set @AddressId = (select top 1 id from Addresses where ExternalId = @ExternalId and Tenant = @Tenant)
	
--	update shipments set customeraddressid = @AddressId where tenant = @Tenant and customeraddressid in (select id from  Addresses where ExternalId = @ExternalId and Tenant = @Tenant and id <> @AddressId)
--	update shipments set ConsigneeAddressId = @AddressId where tenant = @Tenant and ConsigneeAddressId in (select id from  Addresses where ExternalId = @ExternalId and Tenant = @Tenant and id <> @AddressId)  
--	update shipments set ShipperAddressId = @AddressId where tenant = @Tenant and ShipperAddressId in (select id from  Addresses where ExternalId = @ExternalId and Tenant = @Tenant and id <> @AddressId)  
	
--	delete from Addresses where ExternalId = @ExternalId and Tenant = @Tenant and Id <> @AddressId
	
--	print @AddressId 
--   end
--	FETCH NEXT FROM AddressesCursor INTO @ExternalId,@Tenant
--	END
--	CLOSE AddressesCursor
--	DEALLOCATE AddressesCursor
--END