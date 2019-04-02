


   declare @Id as varchar(15)
   declare @SourceTenant as int
   declare @ParentTenant as int
   declare @Direction as varchar(40)
   declare @TransportMode as varchar(13)
   declare @DirectHouse as varchar(40)
   declare @Type as varchar(40)
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
   declare @NumberOfContainers as int
   declare @Salesman as int
   declare @AccountManager as int
   declare @TotalProfitInLocalCurrency as float
   declare @TotalProfitInProfitCurrency as float
   declare @LocalCurrency as int
   declare @ProfitCurrency as int
   --declare @NumberOfInvoices as int
   declare @OperationallyClosed as bit
   declare @AccountingClosed as bit
   declare @Status as int
   declare @Location as nvarchar(40)
   declare @Origin as int
   declare @FinalDestination as int
   declare @IsDeparted as bit
   declare @MainCarriageATD as datetime
   declare @IsArrived as bit
   declare @ArrivedDate as datetime
   declare @IsCustomsCleared as bit
   declare @CustomsClearenceDate as date
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
   declare @Forwarder as int 

   declare @FirstPickupETA  as datetime
   declare @FirstPickupETD  as datetime
   declare @MainCarriageFinalDestinationATA  as datetime
   declare @MainCarriageFinalDestinationETA  as datetime
   declare @CustomAgentImportId as varchar(15)
   declare @CustomAgentExportId as varchar(15)
   declare @BookingConfirmationNumber as varchar(25)
   declare @MainCarriageETD  as datetime
   declare @MainCarriageATA as datetime
 --declare @MainCarriageATD as datetime
   declare @MAWBOBLDate as datetime
   declare @StatusDate as datetime
   declare @CustomsDeclarationNumber as varchar(35)
   declare @FirstOperationalCloseDate as datetime
   --declare @GrossWeightPerTon as varchar(15)

   declare @EstimatedFinalArrivalDate as datetime
   declare @ActualFinalArrivalDate as datetime
 --declare @MasterShipmentNumber as varchar(20)
   declare @Routing as varchar(100)
   declare @DescriptionOfGoods as varchar(2000)
   declare @PreCarriageETD as datetime
   declare @MainCarriageETA as datetime
   declare @MoveType as int
   declare @Vessel as int
   declare @SpecialServices as int

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Shipments.Id, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number] ,  NewDIM_Directions.Name, NewDIM_TransportModes.Name ,NewDIM_Levels.Name,  NewDIM_Types.Name , NewDIM_Departments.Id_Number ,NewDIM_Branches.Id_Number , dw_Shipments.ShipmentNumber, dw_Shipments.House ,dw_ShipmentMasterDatas.Master,shipperPartners.Id_Number, consigneePartners.Id_Number,
	agentPartners.Id_Number,customerPartners.Id_Number , NewDIM_Incoterms.Id_Number, dw_Shipments.GrossWeightInKG ,  dw_Shipments.ChargeableWeightInKG , dw_Shipments.VolumeInCBM ,  dw_Shipments.NumberOfPackages, dw_Shipments.NumberOfContainers,SalesmanUser.Id_Number, AccountManagerUser.Id_Number,
	dw_Shipments.ProfitInLocalCurrency,dw_Shipments.ProfitInProfitCurrency,  LocalCurrency.Id_Number  ,  ProfitCurrency.Id_Number,dw_Shipments.IsOperationalClosed,
	dw_Shipments.IsAccountingClosed , NewDIM_ShipmentStatuses.Id_Number , dw_Shipments.StatusLocation  ,  dw_ShipmentMasterDatas.MainCarriageATD ,dw_Shipments.FinalArrivalDate, dw_Shipments.CustomsClearanceDate , dw_ShipmentMasterDatas.Id ,mainCarriageToPort.Id_Number ,  transshipment1ToPort.Id_Number ,transshipment2ToPort.Id_Number,transshipment3ToPort.Id_Number,fromPort.Id_Number, toPort.Id_Number , dw_Shipments.DirectionId ,dw_Shipments.TransportModeId ,dw_Shipments.Tenant,
	dw_Shipments.CreateDateTime ,dw_Shipments.LastUpdateDate,  dw_Shipments.OperationalDate,dw_Shipments.OperationalCloseDate, dw_Shipments.AccountingCloseDate,
	dw_Shipments.OpenReceivablesInLocalCurrency,dw_Shipments.OpenReceivablesInProfitCurrency,dw_Shipments.AccountedReceivablesInLocalCurrency,dw_Shipments.AccountedReceivablesInProfitCurrency , dw_Shipments.OpenPayablesInLocalCurrency,dw_Shipments.OpenPayablesInProfitCurrency,dw_Shipments.AccountedPayablesInLocalCurrency,dw_Shipments.AccountedPayablesInProfitCurrency,
	dw_Shipments.AgentReference1, dw_Shipments.AgentReference2,dw_Shipments.CustomerReference1,dw_Shipments.CustomerReference2,dw_Shipments.ShipperReference1,dw_Shipments.ShipperReference2,dw_Shipments.ConsigneeReference1,dw_Shipments.ConsigneeReference2,dw_Shipments.MainHarmonize,dw_Shipments.AMSBL,
	dw_Shipments.FirstPickupETA,dw_Shipments.FirstPickupETD,dw_Shipments.FreightPrepaidCollectId,dw_ShipmentMasterDatas.MainCarriageETD,dw_ShipmentMasterDatas.MainCarriageFinalDestinationATA,dw_ShipmentMasterDatas.MainCarriageFinalDestinationETA,
	dw_ShipmentMasterDatas.MainCarriageCarrierNumber,dw_Shipments.ProjectNumber,dw_Shipments.OtherPrepaidCollectId,dw_Shipments.TEU,dw_Shipments.ValueOfGoods,
	mainCarriageCarrierPartners.Id_Number,valueOfGoodsCurrency.Id_Number,WarehouseLegWarehousePartners.Id_Number,forwarderPartners.Id_Number,createdByUser.Id_Number,customerAgentImportPartners.Id_Number , customerAgentExportPartners.Id_Number, dw_ShipmentMasterDatas.AirlinePrefix,
	dw_ShipmentMasterDatas.BookingConfirmationNumber , dw_ShipmentMasterDatas.MainCarriageATA , dw_ShipmentMasterDatas.MAWBOBLDate , dw_Shipments.StatusDate , dw_Shipments.CustomsDeclarationNumber ,  dw_Shipments.FirstOperationalCloseDate,
	 dw_Shipments.EstimatedFinalArrivalDate , dw_Shipments.ActualFinalArrivalDate,  dw_Shipments.Routing , dw_Shipments.DescriptionOfGoods , dw_Shipments.PreCarriageETD , dw_ShipmentMasterDatas.MainCarriageETA , NewDIM_MoveTypes.Id_Number , NewDIM_Vessels.Id_Number , NewDIM_SpecialServicesTypes.Id_Number

	From dw_Shipments
	inner JOIN NewDIM_Tenants SourceTenant ON dw_Shipments.Tenant = SourceTenant.[Tenant Number]
	inner JOIN dw_DWHSettings ON dw_Shipments.Tenant = dw_DWHSettings.Tenant
	inner JOIN NewDIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]


	inner JOIN NewDIM_Directions ON dw_Shipments.DirectionId = NewDIM_Directions.Code
    inner JOIN NewDIM_TransportModes ON dw_Shipments.TransportModeId = NewDIM_TransportModes.Code
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
	inner JOIN NewDIM_ShipmentStatuses  ON dw_Shipments.StatusId = NewDIM_ShipmentStatuses.Id
	inner JOIN dw_Tenants  ON dw_Shipments.Tenant = dw_Tenants.Id
	inner JOIN NewDIM_Currencies LocalCurrency ON dw_Tenants.CurrencyId = LocalCurrency.Id
    inner JOIN NewDIM_Ports fromPort  ON dw_Shipments.FromPortId = FromPort.Id
	inner JOIN NewDIM_Ports toPort  ON dw_Shipments.ToPortId = toPort.Id
	inner JOIN NewDIM_Ports mainCarriageToPort  ON dw_ShipmentMasterDatas.MainCarriageToPortId = mainCarriageToPort.Id
	inner JOIN NewDIM_Ports transshipment1ToPort  ON dw_ShipmentMasterDatas.Transshipment1ToPortId = transshipment1ToPort.Id
	inner JOIN NewDIM_Ports transshipment2ToPort  ON dw_ShipmentMasterDatas.Transshipment2ToPortId = transshipment2ToPort.Id
	inner JOIN NewDIM_Ports transshipment3ToPort  ON dw_ShipmentMasterDatas.Transshipment3ToPortId = transshipment3ToPort.Id


	inner JOIN NewDIM_Partners forwarderPartners ON dw_Shipments.ForwarderPartnerId = forwarderPartners.Id
    inner JOIN NewDIM_Partners customerAgentImportPartners ON dw_Shipments.CustomAgentImportId = customerAgentImportPartners.Id
	inner JOIN NewDIM_Partners customerAgentExportPartners ON dw_Shipments.CustomAgentExportId = customerAgentExportPartners.Id
	inner JOIN NewDIM_Partners mainCarriageCarrierPartners ON dw_ShipmentMasterDatas.MainCarriageCarrierId = mainCarriageCarrierPartners.Id
	inner JOIN NewDIM_Partners WarehouseLegWarehousePartners ON dw_Shipments.WarehouseLegWarehouseId = WarehouseLegWarehousePartners.Id
	inner JOIN NewDIM_Users createdByUser ON dw_Shipments.CreatedByUserId = createdByUser.Id
	inner JOIN NewDIM_Currencies valueOfGoodsCurrency ON dw_Shipments.ValueOfGoodsCurrencyId = valueOfGoodsCurrency.Id
	inner JOIN NewDIM_MoveTypes  ON dw_Shipments.MoveTypeId = NewDIM_MoveTypes.Id
    inner JOIN NewDIM_Vessels   ON dw_ShipmentMasterDatas.MainCarriageVesselId = NewDIM_Vessels.Id
	inner JOIN NewDIM_SpecialServicesTypes   ON dw_Shipments.SpecialServicesTypeId = NewDIM_SpecialServicesTypes.Id
	
	 where dw_Shipments.IsCancelled = 0 and dw_Shipments.ShipmentLevelCode in ('H','D') 

	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO    @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages , @NumberOfContainers , @Salesman ,@AccountManager,@TotalProfitInLocalCurrency ,
	 @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency  , @OperationallyClosed , @AccountingClosed , @Status , @Location   ,
    @MainCarriageATD ,  @ArrivedDate ,  @CustomsClearenceDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @Origin ,@ToPortId,@DirectionId,@TransportModeId , @Tenant ,@CreateDate, @LastUpdateDate,@OperationalDate,@OperationalCloseDate,@AccountingCloseDate,
    @OpenReceivablesInLocalCurrency,@OpenReceivablesInProfitCurrency,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency,@OpenPayablesInLocalCurrency,@OpenPayablesInProfitCurrency,@AccountedPayablesInLocalCurrency,@AccountedPayablesInProfitCurrency,
    @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 , @ShipperReference1,@ShipperReference2,@ConsigneeReference1,@ConsigneeReference2,@MainHarmonize,@AMSBL,@FirstPickupETA,@FirstPickupETD,
	@FreightPC,@MainCarriageETD,@MainCarriageFinalDestinationATA ,@MainCarriageFinalDestinationETA,@CarrierNumber,@ProjectNumber,@OtherChargePC,@TEU,@ValueOfGoods,
	@Carrier,@ValueOfGoodsCurrency,@Warehouse,@Forwarder,@CreatedBy,@CustomAgentImportId , @CustomAgentExportId , @AirlinePrefix , 
	@BookingConfirmationNumber, @MainCarriageATA, @MAWBOBLDate, @StatusDate, @CustomsDeclarationNumber, @FirstOperationalCloseDate, @EstimatedFinalArrivalDate, @ActualFinalArrivalDate, @Routing, @DescriptionOfGoods, @PreCarriageETD, @MainCarriageETA,@MoveType,@Vessel , @SpecialServices
	



	WHILE @@FETCH_STATUS = 0
	BEGIN



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
     if(@CustomsClearenceDate is null) begin set @IsCustomsCleared= 0 end
	 -------------------------------------------------


		
      insert into #Fact_ShipmentsTemp ([Id],[Source Tenant],[Parent Tenant],[Direction],[Transport Mode],[DirectHouse],[Type],[Department],[Branch],[Shipment Number],[House],[Master],[Shipper],[Consignee],[Agent],[Customer],[Incoterm],[Gross Weight (KG)],[Chargeable Weight (KG)],[Total Volume (CBM)],[Number of Packages],[Number of Containers],[Salesman],[Account Manager],[Profit ( Local )],[Profit],[Local Currency ],[Profit Currency],[Operationally Closed],[Accounting Closed],[Status],[Location],[Origin],[Final Destination],[Is Departed],[Departed Date],[Is Arrived],[Arrived Date],[Is Customs Cleared],[Customs Clearence Date],[Total Shipments],[Create Date],[Last Update Date],[Operational Date],[Operational Close Date],[Accounting Close Date],[Open Receivables ( Local )],[Open Receivables ( Profit )],[Accounted Receivables ( Local )],[Accounted Receivables ( Profit )],[Open Payables ( Local )],[Open Payables ( Profit )],[Accounted Payables ( Local )],[Accounted Payables ( Profit )], [Agent Ref1],[Agent Ref2],[AMS BL],[Consignee Ref1],[Consignee Ref2],[Created By],[Custom Agent],[Customer Ref1],[Customer Ref2],[First Pickup Date],[Freight PC],[Carrier Date ],[Carrier],[Carrier Number],[Main Harmonize],[Other Charge PC],[Project#],[Shipper Ref1],[Shipper Ref2],[TEU],[Value of Goods],[Value of Goods Currency],[Warehouse Terminal],[Forwarder] , [Booking Confirmation Number], [Main Carriage ATA], [Master Date], [Status Date], [Customs Declaration Number],  [First Operational Close Date],   [Estimated Final Arrival Date], [Actual Final Arrival Date],  [Routing], [Description Of Goods], [Pre Carriage ETD], [Main Carriage ETA] , [Main Carriage ETD] , [Move Type]  , [Vessel] , [Special Services] , [First Pickup ETA] , [First Pickup ETD]) 
	                            values(@Id, @SourceTenant,@ParentTenant,@Direction,@TransportMode, @DirectHouse, @Type , @Department ,@Branch , @ShipmentNumber , @House ,@Master , @Shipper,  @Consignee , @Agent,@Customer,@Incoterm ,@TotalGrossWeightInKG,@TotalChargeableWeightInKG, @TotalVolumeInCBM,  @NumberOfPackages, @NumberOfContainers, @Salesman , @AccountManager ,    @TotalProfitInLocalCurrency , @TotalProfitInProfitCurrency , @LocalCurrency,@ProfitCurrency ,@OperationallyClosed,@AccountingClosed, @Status, @Location,  @Origin , @FinalDestination , @IsDeparted , dbo.GetDateFormateAsNumber(@MainCarriageATD)  ,@IsArrived , dbo.GetDateFormateAsNumber(@ArrivedDate)   , @IsCustomsCleared , dbo.GetDateFormateAsNumber(@CustomsClearenceDate) , 1 ,dbo.GetDateFormateAsNumber(@CreateDate)    ,dbo.GetDateFormateAsNumber(@LastUpdateDate)   , dbo.GetDateFormateAsNumber(@OperationalDate),dbo.GetDateFormateAsNumber(@OperationalCloseDate),dbo.GetDateFormateAsNumber(@AccountingCloseDate) ,@OpenReceivablesInLocalCurrency , @OpenReceivablesInProfitCurrency ,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency, @OpenPayablesInLocalCurrency ,@OpenPayablesInProfitCurrency , @AccountedPayablesInLocalCurrency ,@AccountedPayablesInProfitCurrency , @AgentReference1, @AgentReference2,@AMSBL ,@ConsigneeReference1,@ConsigneeReference2,@CreatedBy,@CustomAgent,@CustomerReference1,@CustomerReference2,dbo.GetDateFormateAsNumber(@FirstPickupDate)   ,@FreightPC, dbo.GetDateFormateAsNumber(@CarrierDate)   ,@Carrier,@CarrierNumber,@MainHarmonize,@OtherChargePC,@ProjectNumber,@ShipperReference1,@ShipperReference2,@TEU,@ValueOfGoods,@ValueOfGoodsCurrency,@Warehouse,@Forwarder ,  @BookingConfirmationNumber, dbo.GetDateFormateAsNumber(@MainCarriageATA) ,dbo.GetDateFormateAsNumber(@MAWBOBLDate) , dbo.GetDateFormateAsNumber(@StatusDate) , @CustomsDeclarationNumber ,dbo.GetDateFormateAsNumber(@FirstOperationalCloseDate) ,  dbo.GetDateFormateAsNumber(@EstimatedFinalArrivalDate) , dbo.GetDateFormateAsNumber(@ActualFinalArrivalDate) , REPLACE(@Routing,',','>'), @DescriptionOfGoods, dbo.GetDateFormateAsNumber(@PreCarriageETD) ,   dbo.GetDateFormateAsNumber(@MainCarriageETA)  , dbo.GetDateFormateAsNumber(@MainCarriageETD),@MoveType ,@Vessel,@SpecialServices ,dbo.GetDateFormateAsNumber(@FirstPickupETA), dbo.GetDateFormateAsNumber(@FirstPickupETD)  )

	FETCH NEXT FROM ShipmentsCursor    INTO   @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages , @NumberOfContainers , @Salesman ,@AccountManager ,@TotalProfitInLocalCurrency , 
	 @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency  , @OperationallyClosed , @AccountingClosed , @Status , @Location   ,
	 @MainCarriageATD ,  @ArrivedDate ,  @CustomsClearenceDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @Origin ,@ToPortId,@DirectionId,@TransportModeId , @Tenant ,@CreateDate,@LastUpdateDate,@OperationalDate,@OperationalCloseDate,@AccountingCloseDate,
     @OpenReceivablesInLocalCurrency,@OpenReceivablesInProfitCurrency,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency,@OpenPayablesInLocalCurrency,@OpenPayablesInProfitCurrency,@AccountedPayablesInLocalCurrency,@AccountedPayablesInProfitCurrency,
    @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 , @ShipperReference1,@ShipperReference2,@ConsigneeReference1,@ConsigneeReference2,@MainHarmonize,@AMSBL,@FirstPickupETA,@FirstPickupETD,
	@FreightPC,@MainCarriageETD,@MainCarriageFinalDestinationATA ,@MainCarriageFinalDestinationETA,@CarrierNumber,@ProjectNumber,@OtherChargePC,@TEU,@ValueOfGoods,
	@Carrier,@ValueOfGoodsCurrency,@Warehouse,@Forwarder,@CreatedBy,@CustomAgentImportId , @CustomAgentExportId, @AirlinePrefix,
	@BookingConfirmationNumber, @MainCarriageATA, @MAWBOBLDate, @StatusDate, @CustomsDeclarationNumber, @FirstOperationalCloseDate, @EstimatedFinalArrivalDate, @ActualFinalArrivalDate, @Routing, @DescriptionOfGoods, @PreCarriageETD, @MainCarriageETA,@MoveType , @Vessel , @SpecialServices
		
		End
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
