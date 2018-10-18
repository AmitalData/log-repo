 
 
declare @Tenant as int
declare @EntityId as varchar(15)
declare @EntityNumber as varchar(25)
 
declare @ParentPayableId as varchar(15)
declare @ExpectedAmount as float
declare @SumOfExpectedAmount as float
 
declare @MemoryTable table
(
  Tenant int not null,
  ShipmentNumber varchar(25) not null
)
 
BEGIN
delete from @MemoryTable
 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, ShipmentNumber
	FROM Shipments
	WHERE ShipmentLevelCode = 'C' and tenant = 211
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @EntityNumber
	WHILE @@FETCH_STATUS = 0
	BEGIN
 
		if exists (select * from Shipments where ShipmentLevelCode = 'H' AND MasterShipmentDataId = @EntityId AND Tenant = @Tenant)
		begin
			if exists (select * from ShipmentPayables where Tenant = @Tenant AND ShipmentId = @EntityId)
			BEGIN
				DECLARE ParentPayablesCursor CURSOR READ_ONLY
				FOR
				SELECT Id, ExpectedAmount
				FROM ShipmentPayables
				WHERE ShipmentId = @EntityId AND Tenant = @Tenant and ShipmentPayableAmountTypeCode <> 'NEXP'
				OPEN ParentPayablesCursor FETCH NEXT FROM ParentPayablesCursor INTO @ParentPayableId, @ExpectedAmount
				WHILE @@FETCH_STATUS = 0
				BEGIN
				 
					set @SumOfExpectedAmount = (select sum(isnull(ExpectedAmount,0)) from ShipmentPayables where ShipmentPayableParentId = @ParentPayableId)
					if(round(isnull(@ExpectedAmount,0),2) <> round(isnull(@SumOfExpectedAmount,0),2))
					begin
						if not exists (select * from @MemoryTable where ShipmentNumber = @EntityNumber)
						begin
						insert into @MemoryTable (Tenant, ShipmentNumber) values (@Tenant, @EntityNumber)
						end
					end
 
				FETCH NEXT FROM ParentPayablesCursor INTO @ParentPayableId, @ExpectedAmount
				END
				CLOSE ParentPayablesCursor
				DEALLOCATE ParentPayablesCursor
			END
		end
 
	FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @EntityNumber
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END
 
select * from @MemoryTable
