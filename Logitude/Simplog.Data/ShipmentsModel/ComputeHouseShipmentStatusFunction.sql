USE [Logitude2-5_Main]
GO
/****** Object:  StoredProcedure [dbo].[usp_ComputeHouseShipmentStatusFunction]    Script Date: 04/27/2016 14:59:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[usp_ComputeHouseShipmentStatusFunction]
(
	
	@ShipmentId varchar(15)

)
AS

declare @MasterDataId as varchar(15)
declare @CustomFileId as varchar(15)
declare @ComputedStatusId as varchar(15)
declare @ShipmentStatusId as varchar(35)
declare @ShipmentDeclarationNumber as varchar(35)
declare @ShipmentStatusWeight as int
declare @CustomsDeclarationNumber as varchar(35)
declare @ComputedStatusDate as datetime

declare @MasterStatusWeight as int
declare @MasterStatusId as varchar(15)
declare @Tenant as int

declare @CustomStatusWeight as int
declare @CustomStatusId as varchar(15)
declare @SearchFields as varchar(1000)

if (@ShipmentId is not null)

begin 

select
@Tenant = Tenant,
@MasterDataId = MasterShipmentDataId,
@ShipmentStatusId = StatusId,
@CustomFileId = CustomFileId,
@ComputedStatusDate = StatusDate,
@ShipmentDeclarationNumber =CustomsDeclarationNumber,
@ComputedStatusId = StatusId,
@SearchFields = SearchFields
from Shipments
where Id = @ShipmentId


set @ShipmentStatusWeight = (select StatusWeight from EntityStatus where Id = @ShipmentStatusId AND Tenant = @Tenant)


  if(@MasterDataId is not null)
	Begin 


	 select @MasterStatusId = StatusId  from Shipments  where  Id = @MasterDataId AND Tenant = @Tenant
    
	 set @MasterStatusWeight = (select StatusWeight from EntityStatus where Id = @MasterStatusId AND Tenant = @Tenant)
	  if(@MasterStatusWeight > @ShipmentStatusWeight)

	   begin 
	      set @ShipmentStatusWeight = @MasterStatusWeight
	      set @ComputedStatusId = @MasterStatusId
	      set @ComputedStatusDate = ( select StatusDate from Shipments where Id = @MasterStatusId AND Tenant = @Tenant)
	   end

	 End



  if (@CustomFileId is not null)
	Begin
	 

  select @CustomsDeclarationNumber = CustomsDeclarationNumber ,@CustomStatusId = StatusId from Shipments where Id = @CustomFileId AND Tenant = @Tenant
  set @CustomStatusWeight = (select StatusWeight from EntityStatus where Id = @CustomStatusId AND Tenant = @Tenant)  

		if(@CustomStatusWeight > @ShipmentStatusWeight)
	
		begin
	
		 set @ShipmentStatusWeight = @CustomStatusWeight
		 set @ComputedStatusId = @CustomStatusId
		
		 set @ComputedStatusDate = ( select StatusDate from Shipments where Id = @CustomFileId AND Tenant = @Tenant)
	     end


      END



 if(@CustomsDeclarationNumber is not null and (@ShipmentDeclarationNumber is null or   @ShipmentDeclarationNumber !=@CustomsDeclarationNumber   ))
 begin
 
 set @SearchFields = left((@SearchFields + ',' + @CustomsDeclarationNumber ),1000);
  update Shipments set
        ComputedStatusDate =@ComputedStatusDate ,
		ComputedStatusId=  @ComputedStatusId ,
		CustomsDeclarationNumber =@CustomsDeclarationNumber ,
		SearchFields = @SearchFields 
     where Id = @ShipmentId and Tenant = @Tenant
  end 

  else
   
  begin

  update Shipments set ComputedStatusDate =@ComputedStatusDate , 
  ComputedStatusId=  @ComputedStatusId 
  where Id = @ShipmentId and Tenant = @Tenant
 

  end

  end 