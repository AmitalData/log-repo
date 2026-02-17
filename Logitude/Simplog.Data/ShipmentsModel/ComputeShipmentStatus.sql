




IF OBJECT_ID('[dbo].[usp_ComputeShipmentStatus]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_ComputeShipmentStatus]
GO

Create PROCEDURE [dbo].[usp_ComputeShipmentStatus]
(
	@ShipmentId varchar(15)
)
AS


declare @Tenant as int
declare @MasterDataId as varchar(15)
declare @CustomFileId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @ShipmentStatusId as varchar(15)
declare @ShipmentStatusWeight as int
declare @ShipmentDeclarationNumber as varchar(35)
declare @CustomDeclarationNumber as varchar(35)


declare @HouseId   as varchar(15)
declare @CustomId as varchar(15)

declare @IsConnect as bit
declare @ComputedStatusDate as datetime
declare @ComputedStatusId as varchar(15)


select
@Tenant = Tenant,
@MasterDataId = MasterShipmentDataId,
@ShipmentLevelCode= ShipmentLevelCode,
@ShipmentStatusId = StatusId,
@CustomFileId = CustomFileId,
@ComputedStatusDate = StatusDate,
@ShipmentDeclarationNumber =CustomsDeclarationNumber,
@ComputedStatusId = StatusId
from Shipments
where Id = @ShipmentId

 set @ShipmentStatusWeight = (select StatusWeight from EntityStatus where Id = @ShipmentStatusId AND Tenant = @Tenant)
set  @IsConnect = 0

   if(@ShipmentLevelCode = 'C')
    BEGIN 
	DECLARE HousesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Shipments
	where ShipmentLevelCode = 'H' AND MasterShipmentDataId = @MasterDataId
	OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		begin
	
         EXECUTE usp_ComputeHouseShipmentStatusFunction  @HouseId
			
		end

	FETCH NEXT FROM HousesCursor INTO  @HouseId
	END
	CLOSE HousesCursor
	DEALLOCATE HousesCursor

    END

   if(@ShipmentLevelCode = 'A')


    BEGIN 
	DECLARE HousesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Shipments
	where CustomFileId = @ShipmentId
	OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @CustomId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		begin
		set @IsConnect = 1
	     EXECUTE usp_ComputeHouseShipmentStatusFunction  @CustomId
	
		end

	FETCH NEXT FROM HousesCursor INTO @CustomId
	END
	CLOSE HousesCursor
	DEALLOCATE HousesCursor
    END

 

	
	 if(@ShipmentLevelCode ='A' or @ShipmentLevelCode ='C')
	 begin
	
	  update Shipments set CustomConnectToShipment = @IsConnect , ComputedStatusDate =@ComputedStatusDate , ComputedStatusId=  @ComputedStatusId where Id = @ShipmentId and Tenant = @Tenant
	  set @IsConnect = 0
      end

   if(@ShipmentLevelCode = 'H'  OR @ShipmentLevelCode = 'D')

	begin 

	EXECUTE usp_ComputeHouseShipmentStatusFunction  @ShipmentId

	end 

