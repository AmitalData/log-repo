
declare @Tenant as int
declare @PayableId as varchar(15)
declare @ShipmentId as varchar(15)
declare @ShipmentNumber as varchar(25)
declare @ShipmentLevelCode as varchar(1)
declare @ExpectedAmount as float
declare @AccountedAmount as float
declare @OpenAmount as float
declare @CorrectionAmount as float
declare @LineStatus as varchar(5)

declare @MemoryTable table
(
  Tenant int,
  ShipmentId varchar(15),
  PayableId varchar(15),
  ShipmentNumber varchar(25),
  ShipmentLevelCode varchar(1),
  ExpectedAmount float,
  AccountedAmount float,
  OpenAmount float,
  CorrectionAmount float,
  LineStatus varchar(5)
)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, ShipmentId, ExpectedAmount, AccountedAmount, OpenAmount, CorrectionAmount, ShipmentPayableLineStatusCode
		FROM ShipmentPayables
		where ShipmentPayableAmountTypeCode = 'NEXP' and ShipmentPayableParentId is null and tenant = 211
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @PayableId, @Tenant, @ShipmentId, @ExpectedAmount, @AccountedAmount, @OpenAmount, @CorrectionAmount, @LineStatus
		WHILE @@FETCH_STATUS = 0
		BEGIN

		if not exists (select * from APInvoiceLines where EntityPayableId = @PayableId)
		begin
			
			select
			@ShipmentNumber = ShipmentNumber,
			@ShipmentLevelCode = ShipmentLevelCode
			from Shipments where Tenant = @Tenant and Id = @ShipmentId

			insert into @MemoryTable
			values
			(
			@Tenant,
			@ShipmentId,
			@PayableId,
			@ShipmentNumber,
			@ShipmentLevelCode,
			isnull(@ExpectedAmount,0),
			isnull(@AccountedAmount,0),
			isnull(@OpenAmount,0),
			isnull(@CorrectionAmount,0),
			@LineStatus
			)
		end

		FETCH NEXT FROM DataCursor INTO @PayableId, @Tenant, @ShipmentId, @ExpectedAmount, @AccountedAmount, @OpenAmount, @CorrectionAmount, @LineStatus
		END
		CLOSE DataCursor
		DEALLOCATE DataCursor

		select * from @MemoryTable
END