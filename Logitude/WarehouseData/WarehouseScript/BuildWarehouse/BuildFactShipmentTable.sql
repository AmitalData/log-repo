

   declare @Id as varchar(15)
   declare @SourceTenant as int
   declare @ParentTenant as int
   declare @Direction as varchar(40)
   declare @TransportMode as varchar(13)
   declare @DirectHouse as varchar(40)
   declare @Type as varchar(40)
   declare @OBLType as varchar(25)
   declare @Department as int
   declare @Branch as int
   declare @ShipmentNumber as varchar(15)
   declare @House as varchar(20)
   declare @Master as varchar(30)
   declare @AirlinePrefix as varchar(3)
   declare @Shipper as int
   declare @Consignee as int
   declare @Agent as int
   declare @Customer as int
   declare @Incoterm as int
   declare @TotalGrossWeightInKG as float
   declare @TotalChargeableWeightInKG as float
   declare @TotalVolumeInCBM as float 
   declare @NumberOfPackages as int
   declare @DangerousGoods as bit
   declare @NumberOfContainers as int
   declare @Salesman as int
   declare @AccountManager as int
   declare @TotalProfitInLocalCurrency as float
   declare @TotalProfitInProfitCurrency as float
   declare @LocalCurrency as int
   declare @ProfitCurrency as int
   declare @OperationallyClosed as bit
   declare @AccountingClosed as bit
   declare @Location as nvarchar(40)
   declare @MainCarriageFromPort as int
   declare @FinalDestination as int
   declare @IsDeparted as bit
   declare @MainCarriageATD as datetime
   declare @IsArrived as bit
   declare @ArrivedDate as datetime
   declare @IsCustomsCleared as bit
   declare @LastUpdateDate as datetime
   declare @Tenant as int
   declare @MasterDataId as varchar(15)
   declare @Transshipment3ToPortId  as int
   declare @Transshipment2ToPortId  as int
   declare @Transshipment1ToPortId  as int
   declare @MainCarriageToPortId  as int
   declare @DirectionId as varchar(1)
   declare @TransportModeId as varchar(1)
   declare @ToPortId as int
   declare @CreateDate as datetime
   declare @OperationalDate as datetime
   declare @OperationalCloseDate as datetime
   declare @AccountingCloseDate as datetime
   declare @OpenReceivablesInLocalCurrency as float
   declare @OpenReceivablesInProfitCurrency as float
   declare @AccountedReceivablesInLocalCurrency as float 
   declare @AccountedReceivablesInProfitCurrency as float
   declare @OpenPayablesInLocalCurrency as float
   declare @OpenPayablesInProfitCurrency as float
   declare @AccountedPayablesInLocalCurrency as float
   declare @AccountedPayablesInProfitCurrency as float 
   declare @AgentReference1 as varchar(50)
   declare @AgentReference2 as varchar(50)
   declare @CustomerReference1 as varchar(50)
   declare @CustomerReference2 as varchar(50)
   declare @ShipperReference1 as varchar(50)
   declare @ShipperReference2 as varchar(50)
   declare @ConsigneeReference1 as varchar(50)
   declare @ConsigneeReference2 as varchar(50)
   declare @MainHarmonize as varchar(18)
   declare @AMSBL as varchar(17)
   declare @CreatedBy  as int
   declare @CustomAgent  as int
   declare @FirstPickupDate as datetime
   declare @FreightPC as  varchar(1)
   declare @CarrierDate  as datetime
   declare @Carrier  as int
   declare @CarrierNumber as  varchar(15)
   declare @ProjectNumber as  varchar(100)
   declare @OtherChargePC as varchar(1)
   declare @TEU as float 
   declare @ValueOfGoods as float 
   declare @ValueOfGoodsCurrency as int 
   declare @Warehouse as int 
   declare @FreightForwarder as int 
   declare @FirstPickupETA  as datetime
   declare @FirstPickupETD  as datetime
   declare @MainCarriageFinalDestinationATA  as datetime
   declare @MainCarriageFinalDestinationETA  as datetime
   declare @CustomAgentImportId as varchar(15)
   declare @CustomAgentExportId as varchar(15)
   declare @BookingConfirmationNumber as varchar(25)
   declare @MainCarriageETD  as datetime
   declare @MainCarriageATA as datetime
   declare @MAWBOBLDate as datetime
   declare @CustomsDeclarationNumber as varchar(35)
   declare @FirstOperationalCloseDate as datetime
   declare @EstimatedFinalArrivalDate as datetime
   declare @ActualFinalArrivalDate as datetime
   declare @Routing as varchar(100)
   declare @DescriptionOfGoods as nvarchar(2000)
   declare @PreCarriageETD as datetime
   declare @MainCarriageETA as datetime
   declare @MoveType as int
   declare @Vessel as int
   declare @SpecialServices as int
   declare @ARInvoices as varchar(1000)
   declare @MasterShipmentNumber as varchar(20)
  --@[DeclareCustomFieldsVariable]
  declare @CutoffDate as datetime
  declare @Consolidator as int


        declare @ComputedStatus as int
	    declare @ComputedStatusDate as DateTime






  declare @ConsolidatorRef1 as varchar(50)
  declare @ShipmentNotes as nvarchar(1000)
  declare @Notify1 as int
  declare @Notify1Ref1 as varchar(50)
  declare @Notify2 as int
  declare @Notify2Ref1 as varchar(50)
  declare @Coloader as int
  declare @ColoaderRef1 as varchar(50)
  declare @ShipperNotExporter as int
  declare @ShipperNotExporterRef1 as varchar(50)
  declare @ReleasingAgent as int
  declare @ReleasingAgentRef1 as varchar(50)

  declare @IncludesCustoms as bit
  declare @DeclarationNumber as varchar(40)
  declare @DeclarationDate as datetime
  declare @CustomsClearanceDate as datetime
  declare @TerminalAvailable as datetime
  declare @WarehouseLegLastFreeDate as datetime
  declare @WarehouseLegEntryDate as datetime
  declare @WarehouseLegReleaseDate as datetime
  declare @WarehouseLegActualEntryDate as datetime
  declare @WarehouseLegExpectedEntryDate as datetime
  declare @WarehouseLegActualReleaseDate as datetime
  declare @WarehouseLegExpectedReleaseDate as datetime
  declare @FinalVolumetricWeight as varchar(40)
  declare @ChargeableWeightUnitCode as varchar(3)
  declare @FinalRatio as varchar(20)
  declare @FirstPickupATD as datetime
  declare @FirstPickupATA as datetime
  declare @FinalDeliveryETD as datetime
  declare @FinalDeliveryETA as datetime
  declare @FinalDeliveryATD as datetime
  declare @FinalDeliveryATA as datetime
  declare @Transshipment1ETA as datetime
  declare @Transshipment1ETD as datetime
  declare @Transshipment1ATA as datetime
  declare @Transshipment1ATD as datetime
  declare @Transshipment1AdditionalMAWBOBLBL as nvarchar(20)
  declare @FirstPickupLocation  as nvarchar(40)
  declare @ContainersNumbers as nvarchar(1000)
  declare @VolumetricWeight as float
  declare @Ratio as float
  declare @Transshipment1Vessel as int
  declare @Transshipment1Carrier as int

  declare @OrderGrossWeight as  float
  declare @BookingVolume as  float
  declare @BookingNumberOfPackages as  int
  declare @OrderChargeableWeight as float
  declare @EstimateProfitInProfitCurrency as float
  declare @EstimateProfitInLocalCurrency as float
   declare @GrossWeightUnitCode as  varchar(3)
  declare @VolumeUnitCode as  varchar(3)
  declare @OrderGrossWeightWithUnitCode as  varchar(40)
    declare @OrderVolumeWithUnitCode as  varchar(40)
    declare @OrderNumberOfPackagesWithUnitCode as  varchar(15)

	declare @ConsigneeNotImporter as   varchar(15)
    declare @IssuingCarrierAgent  as   varchar(15)
    declare @OnCarriageTransportMode as varchar(13)
    declare @FirstARInvoiceApprovalDate as  datetime
    declare @BookingConfirmationNotes as  nvarchar(250)
    declare @BookingConfirmedBy as  varchar(40)



 declare @NumberOfDeliveries as  int
 declare @OperationallyClosedByUser as  int
 declare @LastPickupATA as  datetime
 declare @LastPickupATD as  datetime
 declare @LastPickupETA as  datetime
 declare @LastPickupETD as  datetime
 declare @DeliveryToPort as  int
 declare @DeliveryFrom as  varchar(40)
 declare @DeliveryTo as  varchar(40)
 declare @PickupFrom as  varchar(40)
 declare @PickupTo as  varchar(40)
 declare @FreightRelease as  datetime

 declare @PayableStatus as varchar(40)
 declare @ReceivableStatus as varchar(40)
 declare @CarrierLastStatusDate as datetime
 declare @AWBPrint as bit
 declare @ExceptionDescription as nvarchar(500)
 declare @HasException as bit
 declare @ExceptionResolvedDescription as nvarchar(500)
 declare @LastExceptionDescription as nvarchar(2000)
 declare @RegistryDate as datetime
 declare @GrossWeightPerTon as float
 declare @NextETA as datetime
 declare @NextETD as datetime
 declare @Commodity as nvarchar(15)
 declare @TrailerNumber as varchar(15)
 declare @FromLocation as varchar(100)
 declare @ToLocation as varchar(100)

        declare @FirstPickupTruckerId as VARCHAR(15) 
	   declare @FirstPickupTruckerNumber as VARCHAR(15) 
	   declare @FirstPickupDriver as  VARCHAR(40) 
	   declare @FirstPickupTrailerNumber as VARCHAR(15) 
	   declare @FirstPickupNotes as NVARCHAR(2000) 
	   declare @FinalDeliveryTruckerId as VARCHAR(15) 
	   declare @FinalDeliveryTruckerNumber as  VARCHAR(15) 
	   declare @FinalDeliveryDriver  as VARCHAR(40) 
	   declare @FinalDeliveryTrailerNumber  as VARCHAR(15) 
	   declare @FinalDeliveryNotes as NVARCHAR(2000) 
	   declare @DocumentsClosingDate as   datetime

 declare @DeliveryDate as datetime
 declare @OnHandDate as datetime
 declare @PODDate as datetime
 declare @InWarehouseDate as datetime

 declare @BookingConfirmationSent as datetime
 declare @PreAlertSent as datetime
 declare @DeliveryNoticeSent as datetime
 declare @ExpectedArrivalNoticeSent as datetime
 declare @T1Received as datetime
 declare @ArrivalNoticeSent as datetime

 declare @ContainersNumbersAndTypesArray as nvarchar(1000)
 declare @UnNumber as  VARCHAR(4)  
 
    declare @PreCarriageFromPort as int
	declare @PreCarriageToPort as int
	declare @OnCarriageFromPort as int
	declare @OnCarriageToPort as int
	declare @Transshipment1FromPort as int
	declare @Transshipment2FromPort as int
	declare @Transshipment3FromPort as int
	declare @Transshipment1ToPort as int
	declare @Transshipment2ToPort as int
	declare @Transshipment3ToPort as int
	declare @PreCarriageCarrier as int
	declare @OnCarriageCarrier as int
   
   declare @PreCarriageCarrierNumber as varchar(15)
   declare @OnCarriageCarrierNumber as varchar(15)
   declare @PreCarriageTransportMode as varchar(13)
   declare @PreCarriageETA as datetime
   declare @PreCarriageATD as datetime
   declare @PreCarriageATA as datetime
   declare @OnCarriageETD as datetime
   declare @OnCarriageETA as datetime
   declare @OnCarriageATD as datetime
   declare @OnCarriageATA as datetime
   declare @Transshipment2ATA as datetime
   declare @Transshipment2ETA as datetime
   declare @Transshipment2ATD as datetime
   declare @Transshipment2ETD as datetime
   declare @Transshipment3ATA as datetime
   declare @Transshipment3ETA as datetime
   declare @Transshipment3ATD as datetime
   declare @Transshipment3ETD as datetime 
   declare @Transshipment2Carrier as int
   declare @Transshipment3Carrier as int
   declare @Transshipment2AdditionalMAWBOBLBL as nvarchar(20)
   declare @Transshipment3AdditionalMAWBOBLBL as nvarchar(20)
   declare @QuoteNumber as nvarchar(20)
    
   declare @PreForwardingETD as datetime
   declare @PreForwardingETA as datetime
   declare @PreForwardingATA as datetime
   declare @PreForwardingATD as datetime

   declare @PreForwardingCarrier as int 
   declare @PreForwardingCarrierNumber as varchar(15)
   declare @PreForwardingFromPort as int
   declare @PreForwardingToPort as int
  declare @PreForwardingTransportMode as varchar(13)

   declare @ShipmentLevelCode as varchar(1)
    
   declare @MasterOnCarriageATA as datetime 
   declare @MasterOnCarriageATD as datetime
   declare @MasterOnCarriageETD as datetime
   declare @MasterOnCarriageETA as datetime  

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Shipments.Id, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number] ,  NewDIM_Directions.Name, TransportModes.Name ,NewDIM_Levels.Name, NewDIM_Types.Name, NewDIM_OBLTypes.Name, NewDIM_Departments.Id_Number ,NewDIM_Branches.Id_Number , dw_Shipments.ShipmentNumber, dw_Shipments.House ,dw_ShipmentMasterDatas.Master,shipperPartners.Id_Number, consigneePartners.Id_Number,
	agentPartners.Id_Number,customerPartners.Id_Number , NewDIM_Incoterms.Id_Number, dw_Shipments.GrossWeightInKG ,  dw_Shipments.ChargeableWeightInKG , dw_Shipments.VolumeInCBM ,  dw_Shipments.NumberOfPackages, dw_Shipments.IsDangerous, dw_Shipments.NumberOfContainers,SalesmanUser.Id_Number, AccountManagerUser.Id_Number,
	dw_Shipments.ProfitInLocalCurrency,dw_Shipments.ProfitInProfitCurrency,  LocalCurrency.Id_Number  ,  ProfitCurrency.Id_Number,dw_Shipments.IsOperationalClosed,
	dw_Shipments.IsAccountingClosed ,  dw_Shipments.StatusLocation  ,  dw_ShipmentMasterDatas.MainCarriageATD ,dw_Shipments.FinalArrivalDate , dw_ShipmentMasterDatas.Id ,mainCarriageToPort.Id_Number ,  transshipment1ToPort.Id_Number ,transshipment2ToPort.Id_Number,transshipment3ToPort.Id_Number,fromPort.Id_Number, toPort.Id_Number , dw_Shipments.DirectionId ,dw_Shipments.TransportModeId, dw_Shipments.Tenant,
	dw_Shipments.CreateDateTime ,dw_Shipments.LastUpdateDate,  dw_Shipments.OperationalDate,dw_Shipments.OperationalCloseDate, dw_Shipments.AccountingCloseDate,
	dw_Shipments.OpenReceivablesInLocalCurrency,dw_Shipments.OpenReceivablesInProfitCurrency,dw_Shipments.AccountedReceivablesInLocalCurrency,dw_Shipments.AccountedReceivablesInProfitCurrency , dw_Shipments.OpenPayablesInLocalCurrency,dw_Shipments.OpenPayablesInProfitCurrency,dw_Shipments.AccountedPayablesInLocalCurrency,dw_Shipments.AccountedPayablesInProfitCurrency,
	dw_Shipments.AgentReference1, dw_Shipments.AgentReference2,dw_Shipments.CustomerReference1,dw_Shipments.CustomerReference2,dw_Shipments.ShipperReference1,dw_Shipments.ShipperReference2,dw_Shipments.ConsigneeReference1,dw_Shipments.ConsigneeReference2,dw_Shipments.MainHarmonize,dw_Shipments.AMSBL,
	dw_Shipments.FirstPickupETA,dw_Shipments.FirstPickupETD,dw_Shipments.FreightPrepaidCollectId,dw_ShipmentMasterDatas.MainCarriageETD,dw_ShipmentMasterDatas.MainCarriageFinalDestinationATA,dw_ShipmentMasterDatas.MainCarriageFinalDestinationETA,
	dw_ShipmentMasterDatas.MainCarriageCarrierNumber,dw_Shipments.ProjectNumber,dw_Shipments.OtherPrepaidCollectId,dw_Shipments.TEU,dw_Shipments.ValueOfGoods,
	mainCarriageCarrierPartners.Id_Number,valueOfGoodsCurrency.Id_Number,WarehouseLegWarehousePartners.Id_Number,freightForwarder.Id_Number,createdByUser.Id_Number,customerAgentImportPartners.Id_Number , customerAgentExportPartners.Id_Number, dw_ShipmentMasterDatas.AirlinePrefix,
	dw_ShipmentMasterDatas.BookingConfirmationNumber , dw_ShipmentMasterDatas.MainCarriageATA , dw_ShipmentMasterDatas.MAWBOBLDate , dw_Shipments.CustomsDeclarationNumber ,  dw_Shipments.FirstOperationalCloseDate,
	dw_Shipments.EstimatedFinalArrivalDate , dw_Shipments.ActualFinalArrivalDate,  dw_Shipments.Routing , dw_Shipments.DescriptionOfGoods , dw_ShipmentMasterDatas.PreCarriageETD , dw_ShipmentMasterDatas.MainCarriageETA , NewDIM_MoveTypes.Id_Number , NewDIM_Vessels.Id_Number , NewDIM_SpecialServicesTypes.Id_Number,
	dw_ShipmentMasterDatas.MasterShipmentNumber , dw_Shipments.ARInvoices , @dw_Shipments.CustomFieldsVariable, dw_ShipmentMasterDatas.CutoffDate , ConsolidatorIdPartners.Id_Number,dw_Shipments.ConsolidatorReference,dw_Shipments.Notes ,Notify1Partners.Id_Number,dw_Shipments.Notify1Reference, Notify2Partners.Id_Number,dw_Shipments.Notify2Reference,ColoaderPartners.Id_Number,dw_Shipments.ColoaderReference1,ShipperNotExporterPartners.Id_Number,dw_Shipments.ShipperNotExporterReference,ReleasingAgentPartners.Id_Number,dw_Shipments.ReleasingAgentReference1,
	dw_Shipments.IncludesCustoms , dw_Shipments.DeclarationNumber , dw_Shipments.DeclarationDate, dw_Shipments.CustomsClearanceDate,dw_Shipments.TerminalAvailable,dw_Shipments.WarehouseLegLastFreeDate,  dw_Shipments.WarehouseLegActualEntryDate, dw_Shipments.WarehouseLegExpectedEntryDate, dw_Shipments.WarehouseLegActualReleaseDate, dw_Shipments.WarehouseLegExpectedReleaseDate,  dw_Shipments.ChargeableWeightUnitCode,
	dw_ShipmentComputedFields.FirstPickupATD, dw_ShipmentComputedFields.FirstPickupATA,dw_ShipmentComputedFields.FinalDeliveryETD, dw_ShipmentComputedFields.FinalDeliveryETA,dw_ShipmentComputedFields.FinalDeliveryATD,dw_ShipmentComputedFields.FinalDeliveryATA, dw_ShipmentMasterDatas.Transshipment1ETA , dw_ShipmentMasterDatas.Transshipment1ETD , dw_ShipmentMasterDatas.Transshipment1ATA ,  dw_ShipmentMasterDatas.Transshipment1ATD ,Transshipment1Vessel.Id_Number,Transshipment1Carrier.Id_Number,dw_ShipmentMasterDatas.Transshipment1AdditionalMAWBOBLBL, dw_ShipmentComputedFields.FirstPickupLocation, dw_ShipmentComputedFields.ContainersNumbers , dw_Shipments.Ratio , dw_Shipments.VolumetricWeight
	,dw_Shipments.OrderGrossWeight,dw_Shipments.BookingVolume,dw_Shipments.BookingNumberOfPackages,dw_Shipments.OrderChargeableWeight,dw_Shipments.EstimateProfitInProfitCurrency,dw_Shipments.EstimateProfitInLocalCurrency , dw_Shipments.GrossWeightUnitCode,dw_Shipments.VolumeUnitCode,
	ConsigneeNotImporter.Id_Number, IssuingCarrierAgent.Id_Number, OnCarriageTransportModes.Name ,dw_Shipments.FirstARInvoiceApprovalDate , dw_ShipmentMasterDatas.BookingConfirmationNotes , dw_ShipmentMasterDatas.BookingConfirmedBy,dw_ShipmentComputedFields.NumberOfDeliveries,OperationallyClosedByUser.Id_Number,dw_ShipmentComputedFields.LastPickupATA,dw_ShipmentComputedFields.LastPickupATD,dw_ShipmentComputedFields.LastPickupETA,dw_ShipmentComputedFields.LastPickupETD,DeliveryToPort.Id_Number,dw_ShipmentComputedFields.DeliveryFrom,dw_ShipmentComputedFields.DeliveryTo,dw_ShipmentComputedFields.PickupFrom,dw_ShipmentComputedFields.PickupTo,dw_Shipments.FreightRelease,
    ShipmentPayableStatuses.Name, ShipmentReceivableStatuses.Name, dw_Shipments.CarrierLastStatusDate, dw_Shipments.AWBPrint, dw_Shipments.ExceptionDescription, dw_Shipments.HasException, dw_Shipments.ExceptionResolvedDescription, dw_Shipments.LastExceptionDescription, dw_Shipments.RegistryDate, dw_Shipments.GrossWeightPerTon, dw_Shipments.NextETA, dw_Shipments.NextETD,
    dw_ShipmentComputedFields.Commodity, dw_ShipmentMasterDatas.TrailerNumber, dw_ShipmentMasterDatas.MainCarriageFromAddressId, dw_ShipmentMasterDatas.MainCarriageToAddressId,
	fisrtPickupTruckerPartners.Id_Number,dw_ShipmentComputedFields.PickupTruckerNumber,dw_ShipmentComputedFields.PickupDriver,dw_ShipmentComputedFields.PickupTrailerNumber,dw_ShipmentComputedFields.PickupNotes,finalDeliveryTruckerIdPartners.Id_Number, dw_ShipmentComputedFields.DeliveryTruckerNumber,dw_ShipmentComputedFields.DeliveryDriver,dw_ShipmentComputedFields.DeliveryTrailerNumber,dw_ShipmentComputedFields.DeliveryNotes ,dw_ShipmentMasterDatas.DocumentsClosingDate,dw_ShipmentComputedFields.DeliveryDate,dw_ShipmentComputedFields.OnHandDate,dw_ShipmentComputedFields.PODDate,dw_Shipments.WarehouseLegActualEntryDate, dw_ShipmentComputedFields.BookingConfirmationSent, dw_ShipmentComputedFields.PreAlertSent, dw_ShipmentComputedFields.DeliveryNoticeSent, dw_ShipmentComputedFields.ExpectedArrivalNoticeSent, dw_ShipmentComputedFields.T1Received, dw_ShipmentComputedFields.ArrivalNoticeSent, dw_ShipmentComputedFields.ContainersNumbersAndTypesArray
	,NewDIM_ShipmentStatuses.Id_Number , dw_Shipments.ComputedStatusDate, dw_Shipments.DangerousUnNumber,

		dw_ShipmentMasterDatas.PreCarriageCarrierNumber, dw_Shipments.OnForwardingCarrierNumber, PreCarriageTransportModes.Name,PreCarriageFromPort.Id_Number,
	OnCarriageFromPort.Id_Number, PreCarriageToPort.Id_Number,OnCarriageToPort.Id_Number, Transshipment1FromPort.Id_Number,Transshipment2FromPort.Id_Number,
	Transshipment3FromPort.Id_Number,Transshipment1ToPort.Id_Number, Transshipment2ToPort.Id_Number, Transshipment3ToPort.Id_Number,PreCarriageCarrier.Id_Number,
	OnCarriageCarrier.Id_Number,dw_ShipmentMasterDatas.PreCarriageETA, dw_ShipmentMasterDatas.PreCarriageATD,dw_ShipmentMasterDatas.PreCarriageATA,dw_Shipments.OnForwardingETD, dw_Shipments.OnForwardingETA, 
	dw_Shipments.OnForwardingATD, dw_Shipments.OnForwardingATA, dw_ShipmentMasterDatas.Transshipment2ATA, dw_ShipmentMasterDatas.Transshipment3ATA, dw_ShipmentMasterDatas.Transshipment2ETA , 
	dw_ShipmentMasterDatas.Transshipment3ETA,dw_ShipmentMasterDatas.Transshipment2ATD, dw_ShipmentMasterDatas.Transshipment3ATD, dw_ShipmentMasterDatas.Transshipment2ETD , 
	dw_ShipmentMasterDatas.Transshipment3ETD,dw_ShipmentMasterDatas.Transshipment2AdditionalMAWBOBLBL, dw_ShipmentMasterDatas.Transshipment3AdditionalMAWBOBLBL,
	Transshipment2Carrier.Id_Number,Transshipment3Carrier.Id_Number, dw_Shipments.QuoteNumber,

	 dw_Shipments.ShipmentLevelCode,dw_Shipments.PreForwardingETD, dw_Shipments.PreForwardingETA, dw_Shipments.PreForwardingATA,dw_Shipments.PreForwardingATD,
	 dw_Shipments.PreForwardingCarrierNumber, PreForwardingCarrier.Id_Number, PreForwardingFromPort.Id_Number,PreForwardingToPort.Id_Number, PreForwardingTransportModes.Name,
	 dw_ShipmentMasterDatas.OnCarriageATA, dw_ShipmentMasterDatas.OnCarriageATD, dw_ShipmentMasterDatas.OnCarriageETD, dw_ShipmentMasterDatas.OnCarriageETA
	  
	 

    From dw_Shipments
	inner JOIN NewDIM_Tenants SourceTenant ON dw_Shipments.Tenant = SourceTenant.[Tenant Number]
	inner JOIN dw_DWHSettings ON dw_Shipments.Tenant = dw_DWHSettings.Tenant
	inner JOIN NewDIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
    inner JOIN dw_ShipmentComputedFields ON dw_Shipments.Id = dw_ShipmentComputedFields.Id

	inner JOIN NewDIM_Directions ON dw_Shipments.DirectionId = NewDIM_Directions.Code
    inner JOIN NewDIM_TransportModes TransportModes ON dw_Shipments.TransportModeId = TransportModes.Code
	inner JOIN NewDIM_Levels ON dw_Shipments.ShipmentLevelCode = NewDIM_Levels.Code
	inner JOIN NewDIM_Types ON dw_Shipments.ShipmentTypeId = NewDIM_Types.Code

	inner JOIN NewDIM_Departments ON dw_Shipments.DepartmentId = NewDIM_Departments.Id
	inner JOIN NewDIM_Branches ON dw_Shipments.BranchId =NewDIM_Branches.Id
    inner JOIN dw_ShipmentMasterDatas ON dw_Shipments.MasterShipmentDataId = dw_ShipmentMasterDatas.Id
	inner JOIN NewDIM_Partners shipperPartners ON dw_Shipments.ShipperId = shipperPartners.Id
	inner JOIN NewDIM_Partners consigneePartners ON dw_Shipments.ConsigneeId = consigneePartners.Id
	inner JOIN NewDIM_Partners agentPartners ON dw_Shipments.AgentComputed = agentPartners.Id
	inner JOIN NewDIM_Partners customerPartners ON dw_Shipments.CustomerId = customerPartners.Id
	inner JOIN NewDIM_Incoterms  ON dw_Shipments.IncotermId = NewDIM_Incoterms.Id
	inner JOIN NewDIM_Users SalesmanUser ON dw_Shipments.SalesmanUserId = SalesmanUser.Id
	inner JOIN NewDIM_Users AccountManagerUser ON dw_Shipments.AccountManagerUserId = AccountManagerUser.Id
	
	inner JOIN NewDIM_Currencies ProfitCurrency ON dw_Shipments.ProfitCurrencyId = ProfitCurrency.Id
    inner JOIN NewDIM_OBLTypes ON dw_ShipmentMasterDatas.OBLTypeCode = NewDIM_OBLTypes.Code

	inner JOIN dw_Tenants  ON dw_Shipments.Tenant = dw_Tenants.Id
	inner JOIN NewDIM_Currencies LocalCurrency ON dw_Tenants.CurrencyId = LocalCurrency.Id
    inner JOIN NewDIM_Ports fromPort  ON dw_Shipments.FromPortId = FromPort.Id
	inner JOIN NewDIM_Ports toPort  ON dw_Shipments.ToPortId = toPort.Id
	inner JOIN NewDIM_Ports mainCarriageToPort  ON dw_ShipmentMasterDatas.MainCarriageToPortId = mainCarriageToPort.Id
	inner JOIN NewDIM_Ports transshipment1ToPort  ON dw_ShipmentMasterDatas.Transshipment1ToPortId = transshipment1ToPort.Id
	inner JOIN NewDIM_Ports transshipment2ToPort  ON dw_ShipmentMasterDatas.Transshipment2ToPortId = transshipment2ToPort.Id
	inner JOIN NewDIM_Ports transshipment3ToPort  ON dw_ShipmentMasterDatas.Transshipment3ToPortId = transshipment3ToPort.Id

		inner JOIN NewDIM_ShipmentStatuses  ON dw_Shipments.ComputedStatusId = NewDIM_ShipmentStatuses.Id

	inner JOIN NewDIM_Partners freightForwarder ON dw_Shipments.FreightForwarderId = freightForwarder.Id
    inner JOIN NewDIM_Partners customerAgentImportPartners ON dw_Shipments.CustomAgentImportId = customerAgentImportPartners.Id
	inner JOIN NewDIM_Partners customerAgentExportPartners ON dw_Shipments.CustomAgentExportId = customerAgentExportPartners.Id
	inner JOIN NewDIM_Partners mainCarriageCarrierPartners ON dw_ShipmentMasterDatas.MainCarriageCarrierId = mainCarriageCarrierPartners.Id
	inner JOIN NewDIM_Partners WarehouseLegWarehousePartners ON dw_Shipments.WarehouseLegWarehouseId = WarehouseLegWarehousePartners.Id
	inner JOIN NewDIM_Users createdByUser ON dw_Shipments.CreatedByUserId = createdByUser.Id
	inner JOIN NewDIM_Currencies valueOfGoodsCurrency ON dw_Shipments.ValueOfGoodsCurrencyId = valueOfGoodsCurrency.Id
	inner JOIN NewDIM_MoveTypes  ON dw_Shipments.MoveTypeId = NewDIM_MoveTypes.Id
    inner JOIN NewDIM_Vessels   ON dw_ShipmentMasterDatas.MainCarriageVesselId = NewDIM_Vessels.Id
	inner JOIN NewDIM_SpecialServicesTypes   ON dw_Shipments.SpecialServicesTypeId = NewDIM_SpecialServicesTypes.Id
	inner JOIN dw_CustomObjectFields  ON dw_Shipments.Tenant = dw_CustomObjectFields.Tenant and dw_CustomObjectFields.ObjectTableName = 'Shipment'
	inner JOIN NewDIM_Partners ConsolidatorIdPartners ON dw_Shipments.ConsolidatorId = ConsolidatorIdPartners.Id
	inner JOIN NewDIM_Partners Notify1Partners ON dw_Shipments.Notify1Id = Notify1Partners.Id
	inner JOIN NewDIM_Partners Notify2Partners ON dw_Shipments.Notify2Id = Notify2Partners.Id
	inner JOIN NewDIM_Partners ColoaderPartners ON dw_Shipments.ColoaderId = ColoaderPartners.Id
	inner JOIN NewDIM_Partners ShipperNotExporterPartners ON dw_Shipments.ShipperNotExporterId = ShipperNotExporterPartners.Id
	inner JOIN NewDIM_Partners ReleasingAgentPartners ON dw_Shipments.ReleasingAgentId = ReleasingAgentPartners.Id
    inner JOIN NewDIM_Vessels Transshipment1Vessel ON dw_ShipmentMasterDatas.Transshipment1VesselId = Transshipment1Vessel.Id
	inner JOIN NewDIM_Partners Transshipment1Carrier ON dw_ShipmentMasterDatas.Transshipment1CarrierId = Transshipment1Carrier.Id
	inner JOIN NewDIM_TransportModes  OnCarriageTransportModes ON dw_Shipments.OnForwardingTransportModeId = OnCarriageTransportModes.Code
	inner JOIN NewDIM_Partners ConsigneeNotImporter ON dw_Shipments.ConsigneeNotImporterId = ConsigneeNotImporter.Id
	inner JOIN NewDIM_Partners IssuingCarrierAgent ON dw_Shipments.IssuingCarrierAgentId = IssuingCarrierAgent.Id
	inner JOIN NewDIM_Ports DeliveryToPort  ON dw_ShipmentComputedFields.DeliveryToPortId = DeliveryToPort.Id
	inner JOIN NewDIM_Users OperationallyClosedByUser ON dw_ShipmentComputedFields.OperationallyClosedByUserId = OperationallyClosedByUser.Id

    inner JOIN NewDIM_ShipmentPayableStatuses ShipmentPayableStatuses ON dw_Shipments.ShipmentPayableStatusCode = ShipmentPayableStatuses.Code
    inner JOIN NewDIM_ShipmentReceivableStatuses ShipmentReceivableStatuses ON dw_Shipments.ShipmentReceivableStatusCode = ShipmentReceivableStatuses.Code
	

    inner JOIN NewDIM_Partners fisrtPickupTruckerPartners ON dw_ShipmentComputedFields.PickupTruckerId = fisrtPickupTruckerPartners.Id
    inner JOIN NewDIM_Partners finalDeliveryTruckerIdPartners ON dw_ShipmentComputedFields.DeliveryTruckerId = finalDeliveryTruckerIdPartners.Id
	 
   inner JOIN NewDIM_TransportModes  PreCarriageTransportModes ON dw_ShipmentMasterDatas.PreCarriageTransportModeId = PreCarriageTransportModes.Code
   inner JOIN NewDIM_Ports PreCarriageFromPort  ON dw_ShipmentMasterDatas.PreCarriageFromPortId = PreCarriageFromPort.Id
   inner JOIN NewDIM_Ports PreCarriageToPort  ON dw_ShipmentMasterDatas.PreCarriageToPortId = PreCarriageToPort.Id
   inner JOIN NewDIM_Ports OnCarriageFromPort  ON dw_ShipmentMasterDatas.OnCarriageFromPortId = OnCarriageFromPort.Id
   inner JOIN NewDIM_Ports OnCarriageToPort  ON dw_ShipmentMasterDatas.OnCarriageToPortId = OnCarriageToPort.Id

   inner JOIN NewDIM_Ports Transshipment1FromPort  ON dw_ShipmentMasterDatas.Transshipment1FromPortId = Transshipment1FromPort.Id
   inner JOIN NewDIM_Ports Transshipment2FromPort  ON dw_ShipmentMasterDatas.Transshipment2FromPortId = Transshipment2FromPort.Id
   inner JOIN NewDIM_Ports Transshipment3FromPort  ON dw_ShipmentMasterDatas.Transshipment3FromPortId = Transshipment3FromPort.Id 
   inner JOIN NewDIM_Partners PreCarriageCarrier  ON dw_ShipmentMasterDatas.PreCarriageCarrierId = PreCarriageCarrier.Id
   inner JOIN NewDIM_Partners OnCarriageCarrier  ON dw_Shipments.OnForwardingCarrierId = OnCarriageCarrier.Id   
   inner JOIN NewDIM_Partners Transshipment2Carrier ON dw_ShipmentMasterDatas.Transshipment2CarrierId = Transshipment2Carrier.Id
   inner JOIN NewDIM_Partners Transshipment3Carrier ON dw_ShipmentMasterDatas.Transshipment3CarrierId = Transshipment3Carrier.Id

   inner JOIN NewDIM_Partners PreForwardingCarrier  ON dw_Shipments.PreForwardingCarrierId = PreForwardingCarrier.Id
   inner JOIN NewDIM_Ports PreForwardingFromPort  ON dw_Shipments.PreForwardingFromPortId = PreForwardingFromPort.Id
   inner JOIN NewDIM_Ports PreForwardingToPort  ON dw_Shipments.PreForwardingToPortId = PreForwardingToPort.Id
   inner JOIN NewDIM_TransportModes  PreForwardingTransportModes ON dw_Shipments.PreForwardingTransportModeId = PreForwardingTransportModes.Code

	 where dw_Shipments.IsCancelled = 0 and dw_Shipments.ShipmentLevelCode in ('H','D' , 'C') 

	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO    @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type, @OBLType , @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages ,@DangerousGoods , @NumberOfContainers , @Salesman ,@AccountManager,@TotalProfitInLocalCurrency ,
	 @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency  , @OperationallyClosed , @AccountingClosed  , @Location   ,
    @MainCarriageATD ,  @ArrivedDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @MainCarriageFromPort ,@ToPortId,@DirectionId,@TransportModeId , @Tenant ,@CreateDate, @LastUpdateDate,@OperationalDate,@OperationalCloseDate,@AccountingCloseDate,
    @OpenReceivablesInLocalCurrency,@OpenReceivablesInProfitCurrency,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency,@OpenPayablesInLocalCurrency,@OpenPayablesInProfitCurrency,@AccountedPayablesInLocalCurrency,@AccountedPayablesInProfitCurrency,
    @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 , @ShipperReference1,@ShipperReference2,@ConsigneeReference1,@ConsigneeReference2,@MainHarmonize,@AMSBL,@FirstPickupETA,@FirstPickupETD,
	@FreightPC,@MainCarriageETD,@MainCarriageFinalDestinationATA ,@MainCarriageFinalDestinationETA,@CarrierNumber,@ProjectNumber,@OtherChargePC,@TEU,@ValueOfGoods,
	@Carrier,@ValueOfGoodsCurrency,@Warehouse,@FreightForwarder,@CreatedBy,@CustomAgentImportId , @CustomAgentExportId , @AirlinePrefix , 
	@BookingConfirmationNumber, @MainCarriageATA, @MAWBOBLDate, @CustomsDeclarationNumber, @FirstOperationalCloseDate, @EstimatedFinalArrivalDate, @ActualFinalArrivalDate, @Routing, @DescriptionOfGoods, @PreCarriageETD, @MainCarriageETA,@MoveType,@Vessel , @SpecialServices,
	@MasterShipmentNumber,@ARInvoices, @CursorCustomFieldsVariable , @CutoffDate , @Consolidator ,@ConsolidatorRef1, @ShipmentNotes, @Notify1, @Notify1Ref1, @Notify2, @Notify2Ref1, @Coloader, @ColoaderRef1, @ShipperNotExporter, @ShipperNotExporterRef1, @ReleasingAgent , @ReleasingAgentRef1,
	@IncludesCustoms,@DeclarationNumber,@DeclarationDate,@CustomsClearanceDate,@TerminalAvailable,@WarehouseLegLastFreeDate,@WarehouseLegActualEntryDate,@WarehouseLegExpectedEntryDate,@WarehouseLegActualReleaseDate,@WarehouseLegExpectedReleaseDate,@ChargeableWeightUnitCode,@FirstPickupATD, 
    @FirstPickupATA,@FinalDeliveryETD,@FinalDeliveryETA,@FinalDeliveryATD,@FinalDeliveryATA, @Transshipment1ETA,@Transshipment1ETD,@Transshipment1ATA,@Transshipment1ATD,@Transshipment1Vessel,@Transshipment1Carrier, 
    @Transshipment1AdditionalMAWBOBLBL,@FirstPickupLocation,@ContainersNumbers,@Ratio,@VolumetricWeight,
	@OrderGrossWeight,@BookingVolume,@BookingNumberOfPackages,@OrderChargeableWeight,@EstimateProfitInProfitCurrency,@EstimateProfitInLocalCurrency,  @GrossWeightUnitCode,@VolumeUnitCode,
    @ConsigneeNotImporter,@IssuingCarrierAgent,@OnCarriageTransportMode,@FirstARInvoiceApprovalDate,@BookingConfirmationNotes,@BookingConfirmedBy, @NumberOfDeliveries,@OperationallyClosedByUser,@LastPickupATA,@LastPickupATD,@LastPickupETA,@LastPickupETD,@DeliveryToPort,@DeliveryFrom,@DeliveryTo,@PickupFrom,@PickupTo,@FreightRelease,
    @PayableStatus, @ReceivableStatus, @CarrierLastStatusDate, @AWBPrint, @ExceptionDescription, @HasException, @ExceptionResolvedDescription, @LastExceptionDescription, @RegistryDate, @GrossWeightPerTon, @NextETA, @NextETD,
    @Commodity, @TrailerNumber, @FromLocation, @ToLocation,@FirstPickupTruckerId, @FirstPickupTruckerNumber, @FirstPickupDriver, @FirstPickupTrailerNumber, @FirstPickupNotes,@FinalDeliveryTruckerId, @FinalDeliveryTruckerNumber, @FinalDeliveryDriver, @FinalDeliveryTrailerNumber, @FinalDeliveryNotes,@DocumentsClosingDate,@DeliveryDate,@OnHandDate,@PODDate,@InWarehouseDate, @BookingConfirmationSent, @PreAlertSent, @DeliveryNoticeSent, @ExpectedArrivalNoticeSent, @T1Received, @ArrivalNoticeSent, @ContainersNumbersAndTypesArray
	,@ComputedStatus , @ComputedStatusDate, @UnNumber,
	
	  @PreCarriageCarrierNumber, @OnCarriageCarrierNumber, @PreCarriageTransportMode, @PreCarriageFromPort, @OnCarriageFromPort, @PreCarriageToPort, @OnCarriageToPort,
	  @Transshipment1FromPort, @Transshipment2FromPort, @Transshipment3FromPort, @Transshipment1ToPort, @Transshipment2ToPort,@Transshipment3ToPort, @PreCarriageCarrier, 
	  @OnCarriageCarrier,@PreCarriageETA, @PreCarriageATD, @PreCarriageATA, @OnCarriageETD, @OnCarriageETA,@OnCarriageATD, @OnCarriageATA, @Transshipment2ATA,
	  @Transshipment3ATA, @Transshipment2ETA, @Transshipment3ETA, @Transshipment2ATD, @Transshipment3ATD, @Transshipment2ETD, @Transshipment3ETD,
      @Transshipment2AdditionalMAWBOBLBL, @Transshipment3AdditionalMAWBOBLBL, @Transshipment2Carrier,@Transshipment3Carrier, @QuoteNumber,

	  @ShipmentLevelCode,
	  @PreForwardingETD,@PreForwardingETA,@PreForwardingATA,@PreForwardingATD, 
	  @PreForwardingCarrierNumber, @PreForwardingCarrier, @PreForwardingFromPort, @PreForwardingToPort, @PreForwardingTransportMode, @MasterOnCarriageATA, @MasterOnCarriageATD,@MasterOnCarriageETD,@MasterOnCarriageETA




	WHILE @@FETCH_STATUS = 0
	BEGIN
	


	 --   declare @RecordType as varchar(100)
		--set @RecordType = 'Master';
		--if(@DirectHouse = 'House' or @DirectHouse = 'Direct') begin  set @RecordType = 'Shipment'; end


		   declare @percentage as   float=1000
		   declare @OrderGrossWeightinTon as   float =null
		   if(@OrderGrossWeight is not null and @OrderGrossWeight!=0)
		   begin set @OrderGrossWeightinTon = @OrderGrossWeight / @percentage   end


	--------------Long Master Number------------------
	 if(@TransportModeId = 'A' and @Master is not null and @AirlinePrefix is not null)
	 BEGIN
	 SET @Master =  @AirlinePrefix + '-'+@Master  ;
	 end
    ----------------------------------------------
	
	
	--------------Shipment Type------------------
	if(@TransportModeId = 'A' and @type ='Not Specified')
	 BEGIN
	 set @Type = 'Air';
	 end
    ----------------------------------------------

	  ------- Pre Carriage & On Carriage for house shipment ----
	  if(@ShipmentLevelCode = 'H')
	   begin
	  SET @PreCarriageCarrierNumber = @PreForwardingCarrierNumber;
	  SET @PreCarriageCarrier = @PreForwardingCarrier;
	  SET @PreCarriageETD = @PreForwardingETD;
	  SET @PreCarriageETA = @PreForwardingETA;
	  SET @PreCarriageATA = @PreForwardingATA;
	  SET @PreCarriageATD = @PreForwardingATD;
	  SET @PreCarriageFromPort = @PreForwardingFromPort;
	  SET @PreCarriageToPort = @PreForwardingToPort; 
	  SET @PreCarriageTransportMode = @PreForwardingTransportMode;   
	   end
	   
	     ------- On Carriage for direct and master shipment ----
	  if(@ShipmentLevelCode = 'D' or @ShipmentLevelCode = 'C')
	   begin
	       
      SET @OnCarriageATA= @MasterOnCarriageATA
      SET @OnCarriageATD= @MasterOnCarriageATD
      SET @OnCarriageETD= @MasterOnCarriageETD
      SET @OnCarriageETA=  @MasterOnCarriageETA
	  
	   end





	--------------ToPort-------------------------
    if(@DirectionId != 'D' or @TransportModeId != 'I')
		BEGIN

				--Master
				  if(@MasterDataId is not null)
				
				  BEGIN

					if(@MainCarriageToPortId is not null and @MainCarriageToPortId!=1 )BEGIN set @ToPortId = @MainCarriageToPortId;END
					if(@Transshipment1ToPortId is not null and @Transshipment1ToPortId!=1 )BEGIN set @ToPortId = @Transshipment1ToPortId;END
		        	if(@Transshipment2ToPortId is not null and @Transshipment2ToPortId!=1 )BEGIN set @ToPortId = @Transshipment2ToPortId;END
		    	    if(@Transshipment3ToPortId is not null and @Transshipment3ToPortId!=1 )BEGIN set @ToPortId = @Transshipment3ToPortId;END
				  End
		 End
	----------------------------------------------
	if(@DirectionId != 'I')
	    BEGIN
            set @InWarehouseDate = null
        End

	--------------CarrierDate CustomAgent----------
	 if(@DirectionId = 'I' OR @DirectionId = 'C')
	 BEGIN
	  SET @CustomAgent = @CustomAgentImportId;
	  SET @CarrierDate = @MainCarriageFinalDestinationATA;
	
  	  if(@CarrierDate is null) begin SET @CarrierDate = @MainCarriageFinalDestinationETA; 	end
	
	 END

	 else
	 begin     

           SET @CustomAgent = @CustomAgentExportId;
	       SET @CarrierDate = @MainCarriageATD;
	
	       if(@CarrierDate is null)
	          begin SET @CarrierDate = @MainCarriageETD; 	end

	  end
	----------------------------------------------


	 --------------FirstPickupDate-------------------
	 SET  @FirstPickupDate = @FirstPickupETA;
	 if(@FirstPickupDate is null) begin 	SET  @FirstPickupDate = @FirstPickupETD; end 
	 -------------------------------------------------
	
	
	 --------------@FinalDestination-------------------
	 set @FinalDestination = @ToPortId;
	 if(@FinalDestination is null) begin set @FinalDestination= 1; end
	  -------------------------------------------------


	  --------------Other-------------------
     set @IsArrived = 1;set @IsDeparted = 1;set @IsCustomsCleared = 1;
     if(@ArrivedDate is null) begin set @IsArrived= 0; end
	 if(@MainCarriageATD is null) begin set @IsDeparted=0; end
     if(@CustomsClearanceDate is null) begin set @IsCustomsCleared= 0 end
	 -------------------------------------------------

	 ----------------Entry And Release-------------------
	 SET  @WarehouseLegEntryDate = @WarehouseLegActualEntryDate;
	 if(@WarehouseLegEntryDate is null) begin 	SET  @WarehouseLegEntryDate = @WarehouseLegExpectedEntryDate; end 

	 SET  @WarehouseLegReleaseDate = @WarehouseLegActualReleaseDate;
	 if(@WarehouseLegReleaseDate is null) begin 	SET  @WarehouseLegReleaseDate = @WarehouseLegExpectedReleaseDate; end
	 ----------------------------------------------------

	
	 ---------------------VolumetricWeight---------------
	 SET @FinalVolumetricWeight =  CAST(CAST(@VolumetricWeight AS FLOAT(20)) AS VARCHAR(36))  +' ('+ @ChargeableWeightUnitCode +')';
	 ----------------------------------------------------


	 ---------------------Ratio---------------
  SET @FinalRatio = '1:' +  CAST(CAST(@Ratio AS FLOAT(20)) AS VARCHAR(100))  ;
	 ----------------------------------------------------
	    SET @OrderGrossWeightWithUnitCode =  CAST(CAST(@OrderGrossWeight AS FLOAT(20)) AS VARCHAR(36))  +' ('+ @ChargeableWeightUnitCode +')';
		SET @OrderVolumeWithUnitCode =  CAST(CAST(@BookingVolume AS FLOAT(20)) AS VARCHAR(36))  +' ('+ @VolumeUnitCode +')';
	    SET @OrderNumberOfPackagesWithUnitCode= CAST(@BookingNumberOfPackages AS VARCHAR(11)) +  +' ('+ @GrossWeightUnitCode +')'



	 ------------Resolve Custom Field Data Type Code-------------------
            
			    --@[ResolveCustomFieldDataTypeCodeVariable]

	 ----------------------------------------------------
	 
	 BEGIN TRY  
      insert into #Fact_ShipmentsTemp ([Id],[Source Tenant],[Parent Tenant],[Direction],[Transport Mode],[DirectHouse],[Type],[OBL Type],[Department],[Branch],[Shipment Number],[House],[Master],[Shipper],[Consignee],[Agent],[Customer],[Incoterm],[Gross Weight (KG)],[Chargeable Weight (KG)],[Total Volume (CBM)],[Number of Packages],[Dangerous Goods],[Number of Containers],[Salesman],[Account Manager],[Profit ( Local )],[Profit],[Local Currency ],[Profit Currency],[Operationally Closed],[Accounting Closed],[Status],[Location],[MainCarriage From Port],[MainCarriage To Port],[Is Departed],[MainCarriage ATD],[Is Arrived],[Arrived Date],[Is Customs Cleared],[Total Shipments],[Create Date],[Last Update Date],[Operational Date],[Operational Close Date],[Accounting Close Date],[Open Receivables ( Local )],[Open Receivables ( Profit )],[Accounted Receivables ( Local )],[Accounted Receivables ( Profit )],[Open Payables ( Local )],[Open Payables ( Profit )],[Accounted Payables ( Local )],[Accounted Payables ( Profit )], [Agent Ref1],[Agent Ref2],[AMS BL],[Consignee Ref1],[Consignee Ref2],[Created By],[Custom Agent],[Customer Ref1],[Customer Ref2],[First Pickup Date],[Freight PC],[Carrier Date ],[Carrier],[Carrier Number],[Main Harmonize],[Other Charge PC],[Project#],[Shipper Ref1],[Shipper Ref2],[TEU],[Value of Goods],[Value of Goods Currency],[Warehouse Terminal],[Freight Forwarder] , [Booking Confirmation Number], [Main Carriage ATA],[Master Date] , [Transshipment 1 Master Date], [Status Date], [Customs Declaration Number],  [First Operational Close Date],   [Estimated Final Arrival Date], [Actual Final Arrival Date],  [Routing], [Description Of Goods], [Pre Carriage ETD], [Main Carriage ETA] , [Main Carriage ETD] , [Move Type]  , [Vessel] , [Special Services] , [First Pickup ETA] , [First Pickup ETD] , [Master Shipment Number] , [AR Invoices] ,[CustomFieldNamesVariable] , [Create Date Time], [Update Date Time],[Operational Date Time],[Cutoff Date],[Consolidator],[Consolidator Ref1] ,[Shipment Notes], [Notify 1],[Notify 1 Ref1],[Notify 2],[Notify 2 Ref1],[Coloader],[Coloader Ref1],[Shipper Not Exporter],[Shipper Not Exporter Ref1],[Releasing Agent],[Releasing Agent Ref1]
	  ,[Transshipment 1 Vessel] ,[Transshipment 1 Carrier],[Includes Customs],[Declaration Number], [Declaration Date],[Customs Clearance Date],[Terminal Available],[Warehouse Last free Date],[First Pickup ATD],[First Pickup ATA],[Final Delivery ETD],[Final Delivery ETA],[Final Delivery ATD],[Final Delivery ATA],[Transshipment 1 ETA],[Transshipment 1 ETD],[Transshipment 1 ATA],[Transshipment 1 ATD],[Transshipment 1 Master],[First Pickup Location],[ContainersNumbers Array],[Ratio],[Volumetric Weight],[Warehouse Entry Date],[Warehouse Release Date] , [Order Gross Weight],[Order Volume],[Order Number of Packages],[Order Chargeable Weight],[Estimated Profit (Profit)],[Estimated Profit (Local)]
	  ,[Consignee Not Importer],[Issuing Carrier Agent],[On Carriage Transport Mode],[First AR Invoice Approval Date],[Order Confirmation Notes],[Order Confirmed By]
	  ,[Number of Deliveries] , [Operational Closed By],[Last Pickup ATA],[Last Pickup ETA],[Delivery To Port],[Last Pickup ETD],[Last Pickup ATD],[Delivery From],[Delivery To],[Pickup From],[Pickup To],[Freight Release]
      ,[Payable Status],[Receivable Status],[Carrier Last Status Date],[AWB Print],[Exception Description],[Has Exception],[Exception Resolved Description],[Last Exception Description],[Registry Date],[Gross Weight Per Ton],[Next ETA],[Next ETD]
      ,[Commodity], [Trailer Number], [From Location], [To Location]
	  ,[First Pickup Trucker], [First Pickup Trucker Number], [First Pickup Driver],[First Pickup Trailer Number], [First Pickup Notes],[Final Delivery Trucker], [Final Delivery Trucker Number], [Final Delivery Driver],[Final Delivery Trailer Number], [Final Delivery Notes],[Document Closing Date],[Order Gross Weight in Ton],[Delivery Date],[On Hand Date],[POD Date],[In Warehouse Date],[Booking Confirmation Sent],[Pre Alert Sent],[Delivery Notice Sent],[Expected Arrival Notice Sent],[T1 Received],[Arrival Notice Sent],[Containers Numbers and Types Array]
	  ,[Un Number],
	   
	   [Pre Carriage Carrier Number], [On Carriage Carrier Number], [Pre Carriage Transport Mode], [Pre Carriage From Port],
	   [On Carriage From Port], [Pre Carriage To Port], [On Carriage To Port],[Transshipment 1 From Port], [Transshipment 2 From Port], 
	   [Transshipment 3 From Port],[Transshipment 1 To Port], [Transshipment 2 To Port], [Transshipment 3 To Port], [Pre Carriage Carrier],
       [On Carriage Carrier], [Pre Carriage ETA], [Pre Carriage ATD], [Pre Carriage ATA], [On Carriage ETD],  [On Carriage ETA], [On Carriage ATD],
       [On Carriage ATA], [Transshipment 2 ATA], [Transshipment 3 ATA],[Transshipment 2 ETA], [Transshipment 3 ETA], [Transshipment 2 ATD],
       [Transshipment 3 ATD], [Transshipment 2 ETD], [Transshipment 3 ETD],[Transshipment 2 Master], [Transshipment 3 Master],
	   [Transshipment 2 Carrier], [Transshipment 3 Carrier], [Connected Quote]
	  ) 

      values(@Id, @SourceTenant,@ParentTenant,@Direction,@TransportMode, @DirectHouse, @Type, @OBLType, @Department ,@Branch , @ShipmentNumber , @House ,@Master , @Shipper,  @Consignee , @Agent,@Customer,@Incoterm ,@TotalGrossWeightInKG,@TotalChargeableWeightInKG, @TotalVolumeInCBM,  @NumberOfPackages, @DangerousGoods, @NumberOfContainers, @Salesman , @AccountManager ,    @TotalProfitInLocalCurrency , @TotalProfitInProfitCurrency , @LocalCurrency,@ProfitCurrency ,@OperationallyClosed,@AccountingClosed, @ComputedStatus, @Location,  @MainCarriageFromPort , @FinalDestination , @IsDeparted , @MainCarriageATD  ,@IsArrived , @ArrivedDate   , @IsCustomsCleared , 1 ,dbo.GetDateFormateAsNumber(@CreateDate)    ,dbo.GetDateFormateAsNumber(@LastUpdateDate)   , dbo.GetDateFormateAsNumber(@OperationalDate),dbo.GetDateFormateAsNumber(@OperationalCloseDate),dbo.GetDateFormateAsNumber(@AccountingCloseDate) ,@OpenReceivablesInLocalCurrency , @OpenReceivablesInProfitCurrency ,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency, @OpenPayablesInLocalCurrency ,@OpenPayablesInProfitCurrency , @AccountedPayablesInLocalCurrency ,@AccountedPayablesInProfitCurrency , @AgentReference1, @AgentReference2,@AMSBL ,@ConsigneeReference1,@ConsigneeReference2,@CreatedBy,@CustomAgent,@CustomerReference1,@CustomerReference2,dbo.GetDateFormateAsNumber(@FirstPickupDate)   ,@FreightPC, dbo.GetDateFormateAsNumber(@CarrierDate)   ,@Carrier,@CarrierNumber,@MainHarmonize,@OtherChargePC,@ProjectNumber,@ShipperReference1,@ShipperReference2,@TEU,@ValueOfGoods,@ValueOfGoodsCurrency,@Warehouse,@FreightForwarder ,  @BookingConfirmationNumber,@MainCarriageATA , dbo.GetDateFormateAsNumber(@MAWBOBLDate) ,@MAWBOBLDate , dbo.GetDateFormateAsNumber(@ComputedStatusDate) , @CustomsDeclarationNumber ,dbo.GetDateFormateAsNumber(@FirstOperationalCloseDate) ,  @EstimatedFinalArrivalDate , @ActualFinalArrivalDate , REPLACE(@Routing,',','>'), @DescriptionOfGoods, @PreCarriageETD ,  @MainCarriageETA , @MainCarriageETD,@MoveType ,@Vessel,@SpecialServices ,@FirstPickupETA, @FirstPickupETD, @MasterShipmentNumber , @ARInvoices,[CustomFieldValuesVariable],@CreateDate,@LastUpdateDate,@OperationalDate,@CutoffDate ,@Consolidator,@ConsolidatorRef1, @ShipmentNotes, @Notify1, @Notify1Ref1, @Notify2, @Notify2Ref1, @Coloader, @ColoaderRef1, @ShipperNotExporter, @ShipperNotExporterRef1, @ReleasingAgent , @ReleasingAgentRef1 ,
	  @Transshipment1Vessel,@Transshipment1Carrier,@IncludesCustoms,@DeclarationNumber,@DeclarationDate,@CustomsClearanceDate,@TerminalAvailable,@WarehouseLegLastFreeDate,@FirstPickupATD,@FirstPickupATA,@FinalDeliveryETD,@FinalDeliveryETA,@FinalDeliveryATD,@FinalDeliveryATA,@Transshipment1ETA,@Transshipment1ETD,@Transshipment1ATA,@Transshipment1ATD, @Transshipment1AdditionalMAWBOBLBL,@FirstPickupLocation,@ContainersNumbers,@FinalRatio,@FinalVolumetricWeight,@WarehouseLegEntryDate,@WarehouseLegReleaseDate ,@OrderGrossWeightWithUnitCode ,@OrderVolumeWithUnitCode , @OrderNumberOfPackagesWithUnitCode ,@OrderChargeableWeight,@EstimateProfitInProfitCurrency , @EstimateProfitInLocalCurrency , 

	  @ConsigneeNotImporter,@IssuingCarrierAgent,@OnCarriageTransportMode,@FirstARInvoiceApprovalDate,@BookingConfirmationNotes,@BookingConfirmedBy,
	   @NumberOfDeliveries,@OperationallyClosedByUser,@LastPickupATA,@LastPickupETA,@DeliveryToPort,@LastPickupETD,@LastPickupATD,@DeliveryFrom,@DeliveryTo,@PickupFrom,@PickupTo,@FreightRelease,
       @PayableStatus,@ReceivableStatus,@CarrierLastStatusDate,@AWBPrint,@ExceptionDescription,@HasException,@ExceptionResolvedDescription,@LastExceptionDescription,dbo.GetDateFormateAsNumber(@RegistryDate),@GrossWeightPerTon,@NextETA,@NextETD,
       @Commodity,@TrailerNumber,@FromLocation,@ToLocation,@FirstPickupTruckerId, @FirstPickupTruckerNumber, @FirstPickupDriver, @FirstPickupTrailerNumber, @FirstPickupNotes,@FinalDeliveryTruckerId, @FinalDeliveryTruckerNumber, @FinalDeliveryDriver, @FinalDeliveryTrailerNumber, @FinalDeliveryNotes,@DocumentsClosingDate,@OrderGrossWeightinTon,@DeliveryDate,@OnHandDate,@PODDate,@InWarehouseDate,@BookingConfirmationSent, @PreAlertSent, @DeliveryNoticeSent, @ExpectedArrivalNoticeSent, @T1Received, @ArrivalNoticeSent, @ContainersNumbersAndTypesArray, @UnNumber,
	  
	  @PreCarriageCarrierNumber, @OnCarriageCarrierNumber, @PreCarriageTransportMode,  @PreCarriageFromPort, 
	   @OnCarriageFromPort, @PreCarriageToPort, @OnCarriageToPort,@Transshipment1FromPort, @Transshipment2FromPort,
	  @Transshipment3FromPort, @Transshipment1ToPort, @Transshipment2ToPort, @Transshipment3ToPort, @PreCarriageCarrier, 
	  @OnCarriageCarrier,@PreCarriageETA, @PreCarriageATD,  @PreCarriageATA, @OnCarriageETD,  @OnCarriageETA, @OnCarriageATD, 
	  @OnCarriageATA, @Transshipment2ATA, @Transshipment3ATA, @Transshipment2ETA , @Transshipment3ETA , @Transshipment2ATD,
      @Transshipment3ATD, @Transshipment2ETD, @Transshipment3ETD, @Transshipment2AdditionalMAWBOBLBL, @Transshipment3AdditionalMAWBOBLBL, 
	  @Transshipment2Carrier,@Transshipment3Carrier, @QuoteNumber
	  	  )
	END TRY 
BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (ShipmentId: ' + @Id +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

	
	FETCH NEXT FROM ShipmentsCursor    INTO   @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type, @OBLType, @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages , @DangerousGoods, @NumberOfContainers , @Salesman ,@AccountManager ,@TotalProfitInLocalCurrency , 
	 @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency  , @OperationallyClosed , @AccountingClosed  , @Location   ,
	 @MainCarriageATD ,  @ArrivedDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @MainCarriageFromPort ,@ToPortId,@DirectionId,@TransportModeId , @Tenant ,@CreateDate,@LastUpdateDate,@OperationalDate,@OperationalCloseDate,@AccountingCloseDate,
     @OpenReceivablesInLocalCurrency,@OpenReceivablesInProfitCurrency,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency,@OpenPayablesInLocalCurrency,@OpenPayablesInProfitCurrency,@AccountedPayablesInLocalCurrency,@AccountedPayablesInProfitCurrency,
    @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 , @ShipperReference1,@ShipperReference2,@ConsigneeReference1,@ConsigneeReference2,@MainHarmonize,@AMSBL,@FirstPickupETA,@FirstPickupETD,
	@FreightPC,@MainCarriageETD,@MainCarriageFinalDestinationATA ,@MainCarriageFinalDestinationETA,@CarrierNumber,@ProjectNumber,@OtherChargePC,@TEU,@ValueOfGoods,
	@Carrier,@ValueOfGoodsCurrency,@Warehouse,@FreightForwarder,@CreatedBy,@CustomAgentImportId , @CustomAgentExportId, @AirlinePrefix,
	@BookingConfirmationNumber, @MainCarriageATA, @MAWBOBLDate, @CustomsDeclarationNumber, @FirstOperationalCloseDate, @EstimatedFinalArrivalDate, @ActualFinalArrivalDate, @Routing, @DescriptionOfGoods, @PreCarriageETD, @MainCarriageETA,@MoveType , @Vessel , @SpecialServices,
	@MasterShipmentNumber,@ARInvoices,@CursorCustomFieldsVariable, @CutoffDate ,@Consolidator,@ConsolidatorRef1, @ShipmentNotes, @Notify1, @Notify1Ref1, @Notify2, @Notify2Ref1, @Coloader, @ColoaderRef1, @ShipperNotExporter, @ShipperNotExporterRef1, @ReleasingAgent , @ReleasingAgentRef1,
	@IncludesCustoms,@DeclarationNumber,@DeclarationDate,@CustomsClearanceDate,@TerminalAvailable,@WarehouseLegLastFreeDate,@WarehouseLegActualEntryDate,@WarehouseLegExpectedEntryDate,@WarehouseLegActualReleaseDate,@WarehouseLegExpectedReleaseDate,@ChargeableWeightUnitCode,@FirstPickupATD, 
    @FirstPickupATA,@FinalDeliveryETD,@FinalDeliveryETA,@FinalDeliveryATD,@FinalDeliveryATA, @Transshipment1ETA,@Transshipment1ETD,@Transshipment1ATA,@Transshipment1ATD,@Transshipment1Vessel,@Transshipment1Carrier,
    @Transshipment1AdditionalMAWBOBLBL,@FirstPickupLocation,@ContainersNumbers ,@Ratio,@VolumetricWeight,
    @OrderGrossWeight,@BookingVolume,@BookingNumberOfPackages,@OrderChargeableWeight,@EstimateProfitInProfitCurrency,@EstimateProfitInLocalCurrency ,   @GrossWeightUnitCode,@VolumeUnitCode,
    @ConsigneeNotImporter,@IssuingCarrierAgent,@OnCarriageTransportMode,@FirstARInvoiceApprovalDate,@BookingConfirmationNotes,@BookingConfirmedBy, @NumberOfDeliveries,@OperationallyClosedByUser,@LastPickupATA,@LastPickupATD,@LastPickupETA,@LastPickupETD,@DeliveryToPort,@DeliveryFrom,@DeliveryTo,@PickupFrom,@PickupTo,@FreightRelease,
    @PayableStatus,@ReceivableStatus,@CarrierLastStatusDate,@AWBPrint,@ExceptionDescription,@HasException,@ExceptionResolvedDescription,@LastExceptionDescription,@RegistryDate,@GrossWeightPerTon,@NextETA,@NextETD,
    @Commodity,@TrailerNumber,@FromLocation,@ToLocation,@FirstPickupTruckerId, @FirstPickupTruckerNumber, @FirstPickupDriver, @FirstPickupTrailerNumber, @FirstPickupNotes,@FinalDeliveryTruckerId, @FinalDeliveryTruckerNumber, @FinalDeliveryDriver, @FinalDeliveryTrailerNumber, @FinalDeliveryNotes,@DocumentsClosingDate,@DeliveryDate,@OnHandDate,@PODDate,@InWarehouseDate,@BookingConfirmationSent, @PreAlertSent, @DeliveryNoticeSent, @ExpectedArrivalNoticeSent, @T1Received, @ArrivalNoticeSent, @ContainersNumbersAndTypesArray
	,@ComputedStatus , @ComputedStatusDate, @UnNumber, 
	@PreCarriageCarrierNumber, @OnCarriageCarrierNumber, @PreCarriageTransportMode, @PreCarriageFromPort, @OnCarriageFromPort, @PreCarriageToPort,
    @OnCarriageToPort,@Transshipment1FromPort, @Transshipment2FromPort, @Transshipment3FromPort,  @Transshipment1ToPort, @Transshipment2ToPort, 
	@Transshipment3ToPort, @PreCarriageCarrier,  @OnCarriageCarrier,@PreCarriageETA, @PreCarriageATD,   @PreCarriageATA, @OnCarriageETD, @OnCarriageETA,
	@OnCarriageATD, @OnCarriageATA, @Transshipment2ATA, @Transshipment3ATA, @Transshipment2ETA , @Transshipment3ETA , @Transshipment2ATD,
    @Transshipment3ATD, @Transshipment2ETD, @Transshipment3ETD, @Transshipment2AdditionalMAWBOBLBL, @Transshipment3AdditionalMAWBOBLBL, 
	@Transshipment2Carrier,@Transshipment3Carrier, @QuoteNumber,   @ShipmentLevelCode, @PreForwardingETD,@PreForwardingETA,@PreForwardingATA,@PreForwardingATD,
	 @PreForwardingCarrierNumber, @PreForwardingCarrier, @PreForwardingFromPort, @PreForwardingToPort, @PreForwardingTransportMode, @MasterOnCarriageATA, @MasterOnCarriageATD,@MasterOnCarriageETD,@MasterOnCarriageETA


		End
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor


	