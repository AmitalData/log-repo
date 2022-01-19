 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Container' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Containers )

 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

  declare @Id as varchar(15)
 declare @Tenant as int
 declare @SourceTenant as int
 declare @ParentTenant as int
 declare @ActualEmptyPickupDate as datetime
 declare @emptyPickupLocationPort as int
 declare @EstimatedEmptyPickupDate as datetime
 declare @preCarriageLocationPort as int
 declare @pOLLocationPort as int
 declare @EstimatedPOLArrival as datetime
 declare @ActualPOLArrival as datetime
 declare @EstimatedPOLLoaded as datetime
 declare @ActualPOLLoaded as datetime
 declare @EstimatedPOLVesselDeparture as datetime
 declare @ActualPOLVesselDeparture as datetime
 declare @transshipment1LocationPort as int
 declare @EstimatedTrans1VesselArrival as datetime
 declare @ActualTransshipment1VesselArrival as datetime
 declare @EstimatedTransshipment1Discharge as datetime
 declare @ActualTransshipment1Discharge as datetime
 declare @EstimatedTransshipment1Loaded as datetime
 declare @ActualTransshipment1Loaded as datetime
 declare @EstimatedTrans1VesselDeparture as datetime
 declare @ActualTrans1VesselDeparture as datetime
 declare @transshipment2LocationPort as int
 declare @EstimatedTrans2VesselArrival as datetime
 declare @ActualTransshipment2VesselArrival as datetime
 declare @EstimatedTransshipment2Discharge as datetime
 declare @ActualTransshipment2Discharge as datetime
 declare @EstimatedTransshipment2Loaded as datetime
 declare @ActualTransshipment2Loaded as datetime
 declare @EstimatedTrans2VesselDeparture as datetime
 declare @ActualTrans2VesselDeparture as datetime
 declare @transshipment3LocationPort as int
 declare @EstimatedTrans3VesselArrival as datetime
 declare @ActualTransshipment3VesselArrival as datetime
 declare @EstimatedTransshipment3Discharge as datetime
 declare @ActualTransshipment3Discharge as datetime
 declare @EstimatedTransshipment3Loaded as datetime
 declare @ActualTransshipment3Loaded as datetime
 declare @EstimatedTrans3VesselDeparture as datetime
 declare @ActualTrans3VesselDeparture as datetime
 declare @transshipment4LocationPort as int
 declare @EstimatedTrans4VesselArrival as datetime
 declare @ActualTransshipment4VesselArrival as datetime
 declare @EstimatedTransshipment4Discharge as datetime
 declare @ActualTransshipment4Discharge as datetime
 declare @EstimatedTransshipment4Loaded as datetime
 declare @ActualTransshipment4Loaded as datetime
 declare @EstimatedTrans4VesselDeparture as datetime
 declare @ActualTrans4VesselDeparture as datetime
 declare @pODLocationPort as int
 declare @EstimatedPODVesselArrival as datetime
 declare @EstimatedPODDischarge as datetime
 declare @ActualPODVesselArrival as datetime
 declare @ActualPODDischarge as datetime
 declare @EstimatedPODDeparture as datetime
 declare @ActualPODDeparture as datetime
 declare @emptyReturnLocationPort as int
 declare @EstimatedEmptyReturn as datetime
 declare @ActualEmptyReturn as datetime
 declare @MainCarriageATA as datetime
 declare @MainCarriageETA as datetime
 declare @MainCarriageETD as datetime
 declare @MainCarriageATD as datetime
 declare @PreCarriageATD as datetime
 declare @onCarriageLocationPort as int
 declare @EstimatedOnCarriageDeparture as datetime
 declare @OnCarriageETD as datetime
 declare @ActualOnCarriageDeparture as datetime
 declare @OnCarriageATD as datetime
 declare @lIFLocationPort as int
 declare @ActualLIFArrival as datetime
 declare @PreCarriageETD as datetime
 declare @EstimatedLIFArrival as datetime
 declare @CreateDate as datetime
 declare @UpdateDate as datetime
 declare @CurrentStatus as varchar(100)
 declare @CurrentStatusDate as datetime
 declare @ClosedDate as datetime
 declare @IsClosed as bit
 declare @status as int
 declare @createdByUser as int
 declare @updatedByUser as int 
 declare @ShipmentId as varchar(15)
 declare @LastFreeDayDate as datetime  
 declare @GateOut as datetime
 declare @FreeDays as int

  DECLARE ContainersCursor CURSOR READ_ONLY
	FOR
	SELECT  dw_Containers.Id,dw_Containers.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number], dw_Containers.ActualEmptyPickupDate, emptyPickupLocationPort.Id_Number, dw_Containers.EstimatedEmptyPickupDate, preCarriageLocationPort.Id_Number, pOLLocationPort.Id_Number, dw_Containers.EstimatedPOLArrival,
	dw_Containers.ActualPOLArrival, dw_Containers.EstimatedPOLLoaded, dw_Containers.ActualPOLLoaded, dw_Containers.EstimatedPOLVesselDeparture, dw_Containers.ActualPOLVesselDeparture, transshipment1LocationPort.Id_Number, dw_Containers.EstimatedTrans1VesselArrival, dw_Containers.ActualTransshipment1VesselArrival,
	dw_Containers.EstimatedTransshipment1Discharge, dw_Containers.ActualTransshipment1Discharge, dw_Containers.EstimatedTransshipment1Loaded, dw_Containers.ActualTransshipment1Loaded, dw_Containers.EstimatedTrans1VesselDeparture, dw_Containers.ActualTrans1VesselDeparture, transshipment2LocationPort.Id_Number,
	dw_Containers.EstimatedTrans2VesselArrival, dw_Containers.ActualTransshipment2VesselArrival, dw_Containers.EstimatedTransshipment2Discharge, dw_Containers.ActualTransshipment2Discharge, dw_Containers.EstimatedTransshipment2Loaded, dw_Containers.ActualTransshipment2Loaded, dw_Containers.EstimatedTrans2VesselDeparture, 
	dw_Containers.ActualTrans2VesselDeparture, transshipment3LocationPort.Id_Number, dw_Containers.EstimatedTrans3VesselArrival, dw_Containers.ActualTransshipment3VesselArrival, dw_Containers.EstimatedTransshipment3Discharge, dw_Containers.ActualTransshipment3Discharge, dw_Containers.EstimatedTransshipment3Loaded, 
	dw_Containers.ActualTransshipment3Loaded, dw_Containers.EstimatedTrans3VesselDeparture, dw_Containers.ActualTrans3VesselDeparture, transshipment4LocationPort.Id_Number, dw_Containers.EstimatedTrans4VesselArrival, dw_Containers.ActualTransshipment4VesselArrival, dw_Containers.EstimatedTransshipment4Discharge,
	dw_Containers.ActualTransshipment4Discharge, dw_Containers.EstimatedTransshipment4Loaded, dw_Containers.ActualTransshipment4Loaded, dw_Containers.EstimatedTrans4VesselDeparture, dw_Containers.ActualTrans4VesselDeparture, pODLocationPort.Id_Number, dw_Containers.EstimatedPODVesselArrival, dw_Containers.EstimatedPODDischarge, 
	dw_Containers.ActualPODVesselArrival, dw_Containers.ActualPODDischarge, dw_Containers.EstimatedPODDeparture, dw_Containers.ActualPODDeparture, emptyReturnLocationPort.Id_Number, dw_Containers.EstimatedEmptyReturn, dw_Containers.ActualEmptyReturn, dw_Containers.MainCarriageATA, dw_Containers.MainCarriageETA, dw_Containers.MainCarriageETD, 
	dw_Containers.MainCarriageATD, dw_Containers.PreCarriageATD, onCarriageLocationPort.Id_Number, dw_Containers.EstimatedOnCarriageDeparture, dw_Containers.OnCarriageETD, dw_Containers.ActualOnCarriageDeparture, dw_Containers.OnCarriageATD, lIFLocationPort.Id_Number, dw_Containers.ActualLIFArrival, dw_Containers.PreCarriageETD, 
	dw_Containers.EstimatedLIFArrival, dw_Containers.CreateDate, createdByUser.Id_Number, dw_Containers.UpdateDate, updatedByUser.Id_Number, dw_Containers.CurrentStatus, dw_Containers.CurrentStatusDate, dw_Containers.ClosedDate, dw_Containers.IsClosed, status.Id_Number,dw_Containers.ShipmentId,dw_Containers.LastFreeDayDate,dw_Containers.GateOut,dw_Containers.FreeDays

  From dw_Containers

    inner JOIN DIM_Tenants SourceTenant ON dw_Containers .Tenant = SourceTenant.[Tenant Number]
    inner JOIN dw_DWHSettings ON dw_Containers.Tenant = dw_DWHSettings.Tenant
    inner JOIN DIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
    inner JOIN DIM_Ports emptyPickupLocationPort ON dw_Containers.EmptyPickupLocationPortId = emptyPickupLocationPort.Id
    inner JOIN DIM_Ports preCarriageLocationPort ON dw_Containers.PreCarriageLocationPortId = preCarriageLocationPort.Id
	inner JOIN DIM_Ports pOLLocationPort ON dw_Containers.POLLocationPortId = pOLLocationPort.Id
	inner JOIN DIM_Ports transshipment1LocationPort ON dw_Containers.Transshipment1LocationPortId = transshipment1LocationPort.Id
	inner JOIN DIM_Ports transshipment2LocationPort ON dw_Containers.Transshipment2LocationPortId = transshipment2LocationPort.Id
	inner JOIN DIM_Ports transshipment3LocationPort ON dw_Containers.Transshipment3LocationPortId = transshipment3LocationPort.Id
	inner JOIN DIM_Ports transshipment4LocationPort ON dw_Containers.Transshipment4LocationPortId = transshipment4LocationPort.Id
	inner JOIN DIM_Ports pODLocationPort ON dw_Containers.PODLocationPortId = pODLocationPort.Id
	inner JOIN DIM_Ports emptyReturnLocationPort ON dw_Containers.EmptyReturnLocationPortId = emptyReturnLocationPort.Id
	inner JOIN DIM_Ports onCarriageLocationPort ON dw_Containers.OnCarriageLocationPortId = onCarriageLocationPort.Id
	inner JOIN DIM_Ports lIFLocationPort ON dw_Containers.LIFLocationPortId = lIFLocationPort.Id
	inner JOIN DIM_Users createdByUser ON dw_Containers.CreatedByUserId = createdByUser.Id
	inner JOIN DIM_Users updatedByUser ON dw_Containers.UpdatedByUserId = updatedByUser.Id
	inner JOIN DIM_ShipmentStatuses status ON dw_Containers.StatusId = status.Id

	 where dw_Containers.AutomaticLastUpdateDate > @LastUpdateDate

	 	OPEN ContainersCursor FETCH NEXT FROM ContainersCursor   into  @Id ,@Tenant , @SourceTenant, @ParentTenant,@ActualEmptyPickupDate, @emptyPickupLocationPort, @EstimatedEmptyPickupDate, @preCarriageLocationPort, @pOLLocationPort, @EstimatedPOLArrival,
	@ActualPOLArrival, @EstimatedPOLLoaded, @ActualPOLLoaded, @EstimatedPOLVesselDeparture, @ActualPOLVesselDeparture, @transshipment1LocationPort, @EstimatedTrans1VesselArrival, @ActualTransshipment1VesselArrival,
	@EstimatedTransshipment1Discharge, @ActualTransshipment1Discharge, @EstimatedTransshipment1Loaded, @ActualTransshipment1Loaded, @EstimatedTrans1VesselDeparture, @ActualTrans1VesselDeparture, @transshipment2LocationPort,
	@EstimatedTrans2VesselArrival, @ActualTransshipment2VesselArrival, @EstimatedTransshipment2Discharge, @ActualTransshipment2Discharge, @EstimatedTransshipment2Loaded, @ActualTransshipment2Loaded, @EstimatedTrans2VesselDeparture,
	@ActualTrans2VesselDeparture, @transshipment3LocationPort, @EstimatedTrans3VesselArrival, @ActualTransshipment3VesselArrival, @EstimatedTransshipment3Discharge, @ActualTransshipment3Discharge, @EstimatedTransshipment3Loaded,
	@ActualTransshipment3Loaded, @EstimatedTrans3VesselDeparture, @ActualTrans3VesselDeparture, @transshipment4LocationPort, @EstimatedTrans4VesselArrival, @ActualTransshipment4VesselArrival, @EstimatedTransshipment4Discharge,
	@ActualTransshipment4Discharge, @EstimatedTransshipment4Loaded, @ActualTransshipment4Loaded, @EstimatedTrans4VesselDeparture, @ActualTrans4VesselDeparture, @pODLocationPort, @EstimatedPODVesselArrival, @EstimatedPODDischarge, 
	@ActualPODVesselArrival, @ActualPODDischarge, @EstimatedPODDeparture, @ActualPODDeparture, @emptyReturnLocationPort, @EstimatedEmptyReturn, @ActualEmptyReturn, @MainCarriageATA, @MainCarriageETA, @MainCarriageETD,
	@MainCarriageATD, @PreCarriageATD, @onCarriageLocationPort, @EstimatedOnCarriageDeparture, @OnCarriageETD, @ActualOnCarriageDeparture, @OnCarriageATD, @lIFLocationPort, @ActualLIFArrival, @PreCarriageETD, @EstimatedLIFArrival,
	@CreateDate, @createdByUser, @UpdateDate, @updatedByUser, @CurrentStatus, @CurrentStatusDate, @ClosedDate, @IsClosed, @status, @ShipmentId,@LastFreeDayDate,@GateOut,@FreeDays
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 
	 BEGIN TRY  

	 	 insert into Fact_Containers ([Id], [Source Tenant], [Parent Tenant], [Actual Empty Pickup Date], [Empty Pickup Location Port], [Estimated Empty Pickup Date], [Pre Carriage Location Port], [POL Location Port], [Estimated POL Arrival],
	 [Actual POL Arrival], [Estimated POL Loaded], [Actual POL Loaded], [Estimated POL Vessel Departure], [Actual POL Vessel Departure], [Transshipment1 Location Port], [Estimated Transshipment1 Vessel Arrival], [Actual Transshipment1 Vessel Arrival],
	 [Estimated Transshipment1 Discharge], [Actual Transshipment1 Discharge], [Estimated Transshipment1 Loaded], [Actual Transshipment1 Loaded], [Estimated Transshipment1 Vessel Departure], [Actual Transshipment1 Vessel Departure], 
	 [Transshipment2 Location Port], [Estimated Transshipment2 Vessel Arrival], [Actual Transshipment2 Vessel Arrival], [Estimated Transshipment2 Discharge], [Actual Transshipment2 Discharge], [Estimated Transshipment2 Loaded],
	 [Actual Transshipment2 Loaded], [Estimated Transshipment2 Vessel Departure], [Actual Transshipment2 Vessel Departure ], [Transshipment3 Location Port], [Estimated Transshipment3 Vessel Arrival], [Actual Transshipment3 Vessel Arrival], [Estimated Transshipment3 Discharge],
	 [Actual Transshipment3 Discharge], [Estimated Transshipment3 Loaded], [Actual Transshipment3 Loaded], [Estimated Transshipment3 Vessel Departure], [Actual Transshipment3 Vessel Departure ], [Transshipment4 Location Port], [Estimated Transshipment4 Vessel Arrival], [Actual Transshipment4 Vessel Arrival], [Estimated Transshipment4 Discharge], [Actual Transshipment4 Discharge], [Estimated Transshipment4 Loaded],
	 [Actual Transshipment4 Loaded], [Estimated Transshipment4 Vessel Departure], [Actual Transshipment4 Vessel Departure ], [POD Location Port], [Estimated POD Vessel Arrival], [Estimated POD Discharge], [Actual POD Vessel Arrival], [Actual POD Discharge],
	 [Estimated POD Departure], [Actual POD Departure], [Empty Return Location Port], [Estimated Empty Return], [Actual Empty Return], [Container Main Carriage ATA], [Container Main Carriage ETA], [Container Main Carriage ETD], 
	 [Container Main Carriage ATD], [Container Pre Carriage ETD], [On Carriage Location Port], [Estimated On Carriage Departure], [Container On Carriage ETD], [Actual On Carriage Departure], [Container On Carriage ATD], [LIF Location Port], [Actual LIF Arrival], [Container Pre Carriage ATD], [Estimated LIF Arrival],
	 [Create Date], [Created By], [Update Date], [Updated By], [Current Status], [Current Status Date], [Closed Date], [Is Closed], [Status], [Shipment Id],[Last Free Day],[POD Gate Out],[Free Days])

	 values(@Id, @SourceTenant, @ParentTenant,dbo.GetDateFormateAsNumber(@ActualEmptyPickupDate), @emptyPickupLocationPort, dbo.GetDateFormateAsNumber(@EstimatedEmptyPickupDate), @preCarriageLocationPort, @pOLLocationPort, dbo.GetDateFormateAsNumber(@EstimatedPOLArrival),
	 dbo.GetDateFormateAsNumber(@ActualPOLArrival), dbo.GetDateFormateAsNumber(@EstimatedPOLLoaded), dbo.GetDateFormateAsNumber(@ActualPOLLoaded), dbo.GetDateFormateAsNumber(@EstimatedPOLVesselDeparture), dbo.GetDateFormateAsNumber(@ActualPOLVesselDeparture), @transshipment1LocationPort, dbo.GetDateFormateAsNumber(@EstimatedTrans1VesselArrival), dbo.GetDateFormateAsNumber(@ActualTransshipment1VesselArrival),
	 dbo.GetDateFormateAsNumber(@EstimatedTransshipment1Discharge), dbo.GetDateFormateAsNumber(@ActualTransshipment1Discharge), dbo.GetDateFormateAsNumber(@EstimatedTransshipment1Loaded), dbo.GetDateFormateAsNumber(@ActualTransshipment1Loaded), dbo.GetDateFormateAsNumber(@EstimatedTrans1VesselDeparture), dbo.GetDateFormateAsNumber(@ActualTrans1VesselDeparture),
	 @transshipment2LocationPort, dbo.GetDateFormateAsNumber(@EstimatedTrans2VesselArrival), dbo.GetDateFormateAsNumber(@ActualTransshipment2VesselArrival), dbo.GetDateFormateAsNumber(@EstimatedTransshipment2Discharge), dbo.GetDateFormateAsNumber(@ActualTransshipment2Discharge), dbo.GetDateFormateAsNumber(@EstimatedTransshipment2Loaded),
	 dbo.GetDateFormateAsNumber(@ActualTransshipment2Loaded), dbo.GetDateFormateAsNumber(@EstimatedTrans2VesselDeparture), dbo.GetDateFormateAsNumber(@ActualTrans2VesselDeparture), @transshipment3LocationPort, dbo.GetDateFormateAsNumber(@EstimatedTrans3VesselArrival), dbo.GetDateFormateAsNumber(@ActualTransshipment3VesselArrival), dbo.GetDateFormateAsNumber(@EstimatedTransshipment3Discharge),
	 dbo.GetDateFormateAsNumber(@ActualTransshipment3Discharge), dbo.GetDateFormateAsNumber(@EstimatedTransshipment3Loaded), dbo.GetDateFormateAsNumber(@ActualTransshipment3Loaded), dbo.GetDateFormateAsNumber(@EstimatedTrans3VesselDeparture), dbo.GetDateFormateAsNumber(@ActualTrans3VesselDeparture), @transshipment4LocationPort, dbo.GetDateFormateAsNumber(@EstimatedTrans4VesselArrival), dbo.GetDateFormateAsNumber(@ActualTransshipment4VesselArrival), dbo.GetDateFormateAsNumber(@EstimatedTransshipment4Discharge), dbo.GetDateFormateAsNumber(@ActualTransshipment4Discharge), dbo.GetDateFormateAsNumber(@EstimatedTransshipment4Loaded),
	 dbo.GetDateFormateAsNumber(@ActualTransshipment4Loaded), dbo.GetDateFormateAsNumber(@EstimatedTrans4VesselDeparture), dbo.GetDateFormateAsNumber(@ActualTrans4VesselDeparture), @pODLocationPort, dbo.GetDateFormateAsNumber(@EstimatedPODVesselArrival), dbo.GetDateFormateAsNumber(@EstimatedPODDischarge), dbo.GetDateFormateAsNumber(@ActualPODVesselArrival), dbo.GetDateFormateAsNumber(@ActualPODDischarge),
	 dbo.GetDateFormateAsNumber(@EstimatedPODDeparture), dbo.GetDateFormateAsNumber(@ActualPODDeparture), @emptyReturnLocationPort, dbo.GetDateFormateAsNumber(@EstimatedEmptyReturn), dbo.GetDateFormateAsNumber(@ActualEmptyReturn), dbo.GetDateFormateAsNumber(@MainCarriageATA), dbo.GetDateFormateAsNumber(@MainCarriageETA), dbo.GetDateFormateAsNumber(@MainCarriageETD),
	 dbo.GetDateFormateAsNumber(@MainCarriageATD), dbo.GetDateFormateAsNumber(@PreCarriageETD), @onCarriageLocationPort, dbo.GetDateFormateAsNumber(@EstimatedOnCarriageDeparture), dbo.GetDateFormateAsNumber(@OnCarriageETD), dbo.GetDateFormateAsNumber(@ActualOnCarriageDeparture), dbo.GetDateFormateAsNumber(@OnCarriageATD), @lIFLocationPort, dbo.GetDateFormateAsNumber(@ActualLIFArrival), dbo.GetDateFormateAsNumber(@PreCarriageATD), dbo.GetDateFormateAsNumber(@EstimatedLIFArrival),
	 dbo.GetDateFormateAsNumber(@CreateDate), @createdByUser, dbo.GetDateFormateAsNumber(@UpdateDate), @updatedByUser, @CurrentStatus, dbo.GetDateFormateAsNumber(@CurrentStatusDate), dbo.GetDateFormateAsNumber(@ClosedDate), @IsClosed, @status,@ShipmentId, dbo.GetDateFormateAsNumber(@LastFreeDayDate), dbo.GetDateFormateAsNumber(@GateOut), @FreeDays)

		END TRY 
BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (ContainerId: ' + @Id +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

FETCH NEXT FROM ContainersCursor INTO @Id,@Tenant, @SourceTenant, @ParentTenant,@ActualEmptyPickupDate, @emptyPickupLocationPort, @EstimatedEmptyPickupDate, @preCarriageLocationPort, @pOLLocationPort, @EstimatedPOLArrival,
	@ActualPOLArrival, @EstimatedPOLLoaded, @ActualPOLLoaded, @EstimatedPOLVesselDeparture, @ActualPOLVesselDeparture, @transshipment1LocationPort, @EstimatedTrans1VesselArrival, @ActualTransshipment1VesselArrival,
	@EstimatedTransshipment1Discharge, @ActualTransshipment1Discharge, @EstimatedTransshipment1Loaded, @ActualTransshipment1Loaded, @EstimatedTrans1VesselDeparture, @ActualTrans1VesselDeparture, @transshipment2LocationPort,
	@EstimatedTrans2VesselArrival, @ActualTransshipment2VesselArrival, @EstimatedTransshipment2Discharge, @ActualTransshipment2Discharge, @EstimatedTransshipment2Loaded, @ActualTransshipment2Loaded, @EstimatedTrans2VesselDeparture,
	@ActualTrans2VesselDeparture, @transshipment3LocationPort, @EstimatedTrans3VesselArrival, @ActualTransshipment3VesselArrival, @EstimatedTransshipment3Discharge, @ActualTransshipment3Discharge, @EstimatedTransshipment3Loaded,
	@ActualTransshipment3Loaded, @EstimatedTrans3VesselDeparture, @ActualTrans3VesselDeparture, @transshipment4LocationPort, @EstimatedTrans4VesselArrival, @ActualTransshipment4VesselArrival, @EstimatedTransshipment4Discharge,
	@ActualTransshipment4Discharge, @EstimatedTransshipment4Loaded, @ActualTransshipment4Loaded, @EstimatedTrans4VesselDeparture, @ActualTrans4VesselDeparture, @pODLocationPort, @EstimatedPODVesselArrival, @EstimatedPODDischarge, 
	@ActualPODVesselArrival, @ActualPODDischarge, @EstimatedPODDeparture, @ActualPODDeparture, @emptyReturnLocationPort, @EstimatedEmptyReturn, @ActualEmptyReturn, @MainCarriageATA, @MainCarriageETA, @MainCarriageETD,
	@MainCarriageATD, @PreCarriageATD, @onCarriageLocationPort, @EstimatedOnCarriageDeparture, @OnCarriageETD, @ActualOnCarriageDeparture, @OnCarriageATD, @lIFLocationPort, @ActualLIFArrival, @PreCarriageETD, @EstimatedLIFArrival,
	@CreateDate, @createdByUser, @UpdateDate, @updatedByUser, @CurrentStatus, @CurrentStatusDate, @ClosedDate, @IsClosed, @status, @ShipmentId,@LastFreeDayDate,@GateOut,@FreeDays

			End
	CLOSE ContainersCursor
	DEALLOCATE ContainersCursor

		end