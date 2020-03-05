

declare @MemoryTable table
(
  Id varchar(15) not null,  
  UserId varchar(15) null,  
  UserName varchar(16) null
)

declare @MemoryTable2 table
(
  Id varchar(15) not null,  
  UserId varchar(15) null,  
  UserName varchar(16) null
)

insert into @MemoryTable
select Shipments.Id, Shipments.OperationalClosedByUserId, Contacts.EnglishName
from Shipments
left outer join Contacts
on Shipments.OperationalClosedByUserId = Contacts.Id


	declare @Id as varchar(15)
	declare @UserId as varchar(15)
	declare @UserName as varchar(60)
	declare @Count as int
	set @Count = 0;

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, UserId, UserName
	FROM @MemoryTable	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @UserId, @UserName
	WHILE @@FETCH_STATUS = 0
	BEGIN

		insert into @MemoryTable2(Id, UserId, UserName) values(@Id, @UserId, @UserName)

		set @Count = @Count + 1;
		if(@Count = 1000)
		begin
		
		update ShipmentComputedFields
		set
		OperationallyClosedByUserId = T.UserId,
		OperationallyClosedByUserName = t.UserName
		FROM ShipmentComputedFields
		INNER JOIN @MemoryTable2 T on ShipmentComputedFields.Id = T.Id

		--truncate table @MemoryTable2
		delete from @MemoryTable2
		set @Count = 0
		end
	FETCH NEXT FROM DataCursor INTO @Id, @UserId, @UserName
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor


	if (@Count > 0)
	begin
		update ShipmentComputedFields
		set
		OperationallyClosedByUserId = T.UserId,
		OperationallyClosedByUserName = t.UserName
		FROM ShipmentComputedFields
		INNER JOIN @MemoryTable2 T on ShipmentComputedFields.Id = T.Id
	end

--select * from @MemoryTable

--			insert into @MemoryTable
--			SELECT
--			CustomerId,
--			ProductCode,
--			Year(CreateDateTime),
--			Month(CreateDateTime),
--			sum(isnull(TEU,0)),
--			sum(ISNULL(OpenReceivablesInProfitCurrency,0) + ISNULL(AccountedReceivablesInProfitCurrency,0)),
--			sum(isnull(ChargeableWeightInKG,0)),
--			count(*),
--			CountryForStatisticsId,
--			max(CreateDateTime)			
--			From Shipments
--			Where IsCancelled = 0 AND Tenant = @Tenant AND ProductCode is not null AND CustomerId is not null
--			group by CustomerId, ProductCode, Month(CreateDateTime), Year(CreateDateTime), CountryForStatisticsId

--select Shipments.Id, Contacts.EnglishName from Shipments
--left outer join Contacts on Shipments.OperationalClosedByUserId = Contacts.Id
--go

--select OperationallyClosedByUserId, OperationallyClosedByUserName, * from ShipmentComputedFields

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
--select Shipments.Id, count(ShipmentPickUpDeliveries.Id)
--from Shipments
--left outer join ShipmentPickUpDeliveries on ShipmentPickUpDeliveries.ShipmentId = Shipments.Id
--where ShipmentPickUpDeliveries.PickUpDeliveryTypeCode = 'DELV'
--group by Shipments.Id

-- Last Pickup Fields
-- 1 Update All Table Set to null
-- 2 Select existing data

-- 1-90
-- 1-91
--select * from ShipmentPickUpDeliveries where PickUpDeliveryTypeCode = 'PICK' and ShipmentId = '1-90'


--select ShipmentId, MAX(Id)
--from ShipmentPickUpDeliveries
--where PickUpDeliveryTypeCode = 'PICK' and ShipmentId = '1-90'
--group by ShipmentId