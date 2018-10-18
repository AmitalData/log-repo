

declare @Tenant as int
declare @MasterId as varchar(15)
declare @ShipmentId as varchar(15)
declare @ShipmentId_PARAM as varchar(15)
declare @ShipmentLevelCode as varchar(1)

set @ShipmentId_PARAM = '1-1'
set @ShipmentId = @ShipmentId_PARAM


select 
@Tenant = Tenant,
@ShipmentLevelCode = ShipmentLevelCode,
@MasterId = MasterShipmentDataId
from Shipments where Id = @ShipmentId_PARAM

if (@ShipmentLevelCode = 'D' OR (@ShipmentLevelCode = 'H' AND @MasterId is null))
BEGIN
	EXECUTE usp_UpdateShipmentProfitFunction @Tenant, @ShipmentId, 0
END

else
BEGIN

	EXECUTE usp_UpdateShipmentProfitFunction @Tenant, @MasterId, 1

	-- Loop Houses
	declare @HouseId as varchar(15)
	DECLARE HousesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Shipments
	WHERE ShipmentLevelCode = 'H' AND MasterShipmentDataId = @MasterId
	OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		EXECUTE usp_UpdateShipmentProfitFunction @Tenant, @HouseId, 0

	FETCH NEXT FROM HousesCursor INTO @HouseId
	END
	CLOSE HousesCursor
	DEALLOCATE HousesCursor

END


select 
ProfitInLocalCurrency as ProfitLocal,
ProfitInProfitCurrency as ProfitProfit,
OpenPayablesInLocalCurrency as OpenPayablesLocal,
OpenPayablesInProfitCurrency as OpenPayablesProfit,
OpenReceivablesInLocalCurrency as OpenReceivablesLocal,
OpenReceivablesInProfitCurrency as OpenReceivablesProfit,
AccountedPayablesInLocalCurrency as ACTPayablesLocal,
AccountedPayablesInProfitCurrency as ACTPayablesProfit,
AccountedReceivablesInLocalCurrency as ACTReceivablesLocal,
AccountedReceivablesInProfitCurrency as ACTReceivablesProfit,
ShipmentPayableStatusCode as Payable,
ShipmentReceivableStatusCode as Receivable,
ARInvoiceIssued,
CreditNoteIssued
from Shipments where Tenant = 1 and Id = '1-1'


