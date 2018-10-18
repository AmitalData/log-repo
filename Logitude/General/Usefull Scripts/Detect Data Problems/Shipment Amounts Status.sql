
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @ShipmentNumber as varchar(50)
declare @TransportModeId as varchar(1)
declare @CreateDate as datetime
declare @UpdateDate as datetime
declare @ShipmentPayableStatusCode as varchar(5)
declare @ShipmentReceivableStatusCode as varchar(5)
declare @OpenPayablesInLocalCurrency as float
declare @AccountedPayablesInLocalCurrency as float
declare @OpenReceivablesInLocalCurrency as float
declare @AccountedReceivablesInLocalCurrency as float
declare @ComputedPayableStatusCode as varchar(5)
declare @ComputedReceivableStatusCode as varchar(5)

declare @MemoryTable table
(
  Id varchar(15) not null,
  Tenant int,
  ShipmentNumber varchar(50) not null,
  TransportModeId varchar(1),
  CreateDate datetime null,
  UpdateDate datetime null,
  PayableStatusCode varchar(5),
  ComputedPayableStatusCode varchar(5),  
  ReceivableStatusCode varchar(5),
  ComputedReceivableStatusCode varchar(5)
)

set @Tenant = 1

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, ShipmentNumber, TransportModeId, CreateDateTime, LastUpdateDate, ShipmentPayableStatusCode, ShipmentReceivableStatusCode, OpenPayablesInLocalCurrency, AccountedPayablesInLocalCurrency, OpenReceivablesInLocalCurrency, AccountedReceivablesInLocalCurrency
	FROM Shipments
	WHERE TransportModeId = 'A' AND CreateDateTime >= '2017-07-01'
	--WHERE CreateDateTime >= '2017-01-01' and Tenant = 194
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @ShipmentNumber, @TransportModeId, @CreateDate, @UpdateDate, @ShipmentPayableStatusCode, @ShipmentReceivableStatusCode, @OpenPayablesInLocalCurrency, @AccountedPayablesInLocalCurrency, @OpenReceivablesInLocalCurrency, @AccountedReceivablesInLocalCurrency
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- Compute Payables Status
		BEGIN
			
			if (@OpenPayablesInLocalCurrency is null)
			set @OpenPayablesInLocalCurrency = 0

			if (@AccountedPayablesInLocalCurrency is null)
			set @AccountedPayablesInLocalCurrency = 0

			if (@OpenPayablesInLocalCurrency = 0 AND @AccountedPayablesInLocalCurrency = 0)
			begin
			set @ComputedPayableStatusCode = 'NOPA'
			end

			else if (@OpenPayablesInLocalCurrency = 0 AND @AccountedPayablesInLocalCurrency <> 0)
			begin
			set @ComputedPayableStatusCode = 'CLSD'
			end

			else
			begin
			set @ComputedPayableStatusCode = 'OPEN'
			end
		END

		-- Compute Receivables Status
		BEGIN

			if (@OpenReceivablesInLocalCurrency is null)
			set @OpenReceivablesInLocalCurrency = 0

			if (@AccountedReceivablesInLocalCurrency is null)
			set @AccountedReceivablesInLocalCurrency = 0

			if (@OpenReceivablesInLocalCurrency = 0 AND @AccountedReceivablesInLocalCurrency = 0)
			begin
			set @ComputedReceivableStatusCode = 'NORE'
			end

			else if (@OpenReceivablesInLocalCurrency = 0 AND @AccountedReceivablesInLocalCurrency <> 0)
			begin
			set @ComputedReceivableStatusCode = 'CLSD'
			end

			else
			begin
			set @ComputedReceivableStatusCode = 'OPEN'
			end

		END

		if (@ComputedPayableStatusCode <> @ShipmentPayableStatusCode OR @ComputedReceivableStatusCode <> @ShipmentReceivableStatusCode)
		begin
			insert into @MemoryTable(Id, Tenant, ShipmentNumber, TransportModeId, CreateDate, UpdateDate, PayableStatusCode, ComputedPayableStatusCode, ReceivableStatusCode, ComputedReceivableStatusCode)
			values
			(
			@ShipmentId,
			@Tenant,
			@ShipmentNumber,
			@TransportModeId,
			@CreateDate,
			@UpdateDate,
			@ShipmentPayableStatusCode,
			@ComputedPayableStatusCode,
			@ShipmentReceivableStatusCode,
			@ComputedReceivableStatusCode
			)
		end
		
	FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @ShipmentNumber, @TransportModeId, @CreateDate, @UpdateDate, @ShipmentPayableStatusCode, @ShipmentReceivableStatusCode, @OpenPayablesInLocalCurrency, @AccountedPayablesInLocalCurrency, @OpenReceivablesInLocalCurrency, @AccountedReceivablesInLocalCurrency
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor

	select * from @MemoryTable
END



--select Id, ShipmentNumber, TransportModeId, IsMultipleCommodities, CreateDateTime, ShipmentPayableStatusCode, OpenPayablesInLocalCurrency, AccountedPayablesInLocalCurrency, ShipmentReceivableStatusCode, OpenReceivablesInLocalCurrency, AccountedReceivablesInLocalCurrency
--from Shipments where Id in ('1-498238', '1-502591', '1-502921', '1-502926')
