
IF OBJECT_ID('[dbo].[usp_UpdateShipmentProfit]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateShipmentProfit]
GO

Create PROCEDURE [dbo].[usp_UpdateShipmentProfit]
(
	@ShipmentId varchar(15)
)
AS

declare @Tenant as int
declare @MasterId as varchar(15)
declare @ShipmentLevelCode as varchar(1)

select 
@Tenant = Tenant,
@MasterId = MasterShipmentDataId,
@ShipmentLevelCode = ShipmentLevelCode
from Shipments where Id = @ShipmentId

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