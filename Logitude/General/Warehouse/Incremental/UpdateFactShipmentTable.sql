 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Shipment' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Shipments )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Id as varchar(15)
   declare @SourceTenant as int
   declare @ParentTenant as int
   declare @Direction as varchar(1)
   declare @TransportMode as varchar(1)
   declare @Level as varchar(1)
   declare @Type as varchar(4)
   declare @Department as int
   declare @Branch as int
   declare @ShipmentNumber as varchar(15)
   declare @House as varchar(20)
   declare @Master as varchar(30)
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
   declare @TotalReceivablesInLocalCurrency as float
   declare @TotalPayablesInLocalCurrency as float
   declare @TotalProfitInLocalCurrency as float
   declare @TotalReceivablesInProfitCurrency as float
   declare @TotalPayablesInProfitCurrency as float
  declare @TotalProfitInProfitCurrency as float
 

   declare @LocalCurrency as int
   declare @ProfitCurrency as int
   declare @NumberOfInvoices as int
   declare @OperationallyClosed as bit
   declare @AccountingClosed as bit
   declare @Status as int
   declare @Location as nvarchar(40)
   declare @Origin as int
   declare @FinalDestination as int
   declare @IsDeparted as bit
   declare @DepartedDate as date
   declare @IsArrived as bit
   declare @ArrivedDate as date
   declare @IsCustomsCleared as bit
   declare @CustomsClearenceDate as date


   declare @Tenant as int
   declare @MasterDataId as varchar(15)
   declare @Transshipment3ToPortId  as int
   declare @Transshipment2ToPortId  as int
   declare @Transshipment1ToPortId  as int
   declare @MainCarriageToPortId  as int
   declare @DirectionId as varchar(15)
   declare @TransportModeId as varchar(1)
   declare @ToPortId as int







	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Shipments.Id, Dim_Tenants.Id_Number, Dim_Tenants.Id_Number , DIM_Directions.Code, DIM_TransportModes.Code ,DIM_Levels.Code,  DIM_Types.Code , DIM_Departments.Id_Number ,DIM_Branches.Id_Number , dw_Shipments.ShipmentNumber, dw_Shipments.House ,dw_ShipmentMasterDatas.Master,shipperPartners.Id_Number, consigneePartners.Id_Number,
	agentPartners.Id_Number,customerPartners.Id_Number , DIM_Incoterms.Id_Number, dw_Shipments.GrossWeightInKG ,  dw_Shipments.ChargeableWeightInKG , dw_Shipments.VolumeInCBM ,  dw_Shipments.NumberOfPackages, dw_Shipments.NumberOfContainers,SalesmanUser.Id_Number, AccountManagerUser.Id_Number,dw_Shipments.AccountedReceivablesInLocalCurrency,
	dw_Shipments.AccountedPayablesInLocalCurrency,dw_Shipments.ProfitInLocalCurrency,dw_Shipments.AccountedReceivablesInProfitCurrency, dw_Shipments.AccountedPayablesInProfitCurrency,dw_Shipments.ProfitInProfitCurrency,  LocalCurrency.Id_Number  ,  ProfitCurrency.Id_Number,0,dw_Shipments.IsOperationalClosed,
	dw_Shipments.IsAccountingClosed , DIM_ShipmentStatuses.Id_Number , dw_Shipments.StatusLocation  ,  dw_ShipmentMasterDatas.MainCarriageATD ,dw_Shipments.FinalArrivalDate, dw_Shipments.CustomsClearanceDate , dw_ShipmentMasterDatas.Id ,mainCarriageToPort.Id_Number ,  transshipment1ToPort.Id_Number ,transshipment2ToPort.Id_Number,transshipment3ToPort.Id_Number,fromPort.Id_Number, toPort.Id_Number , dw_Shipments.DirectionId ,dw_Shipments.TransportModeId ,dw_Shipments.Tenant
	From dw_Shipments
	inner JOIN Dim_Tenants ON dw_Shipments.Tenant = Dim_Tenants.TenantNumber
	inner JOIN DIM_Directions ON dw_Shipments.DirectionId = DIM_Directions.Code
    inner JOIN DIM_TransportModes ON dw_Shipments.TransportModeId = DIM_TransportModes.Code
	inner JOIN DIM_Levels ON dw_Shipments.ShipmentLevelCode = DIM_Levels.Code
	inner JOIN DIM_Types ON dw_Shipments.ShipmentTypeId = DIM_Types.Code
	inner JOIN DIM_Departments ON dw_Shipments.DepartmentId = DIM_Departments.Id
	inner JOIN DIM_Branches ON dw_Shipments.BranchId = DIM_Branches.Id
    inner JOIN dw_ShipmentMasterDatas ON dw_Shipments.MasterShipmentDataId = dw_ShipmentMasterDatas.Id
	inner JOIN DIM_Partners shipperPartners ON dw_Shipments.ShipperId = shipperPartners.Id
	inner JOIN DIM_Partners consigneePartners ON dw_Shipments.ConsigneeId = consigneePartners.Id
	inner JOIN DIM_Partners agentPartners ON dw_Shipments.AgentId = agentPartners.Id
	inner JOIN DIM_Partners customerPartners ON dw_Shipments.CustomerId = customerPartners.Id
	inner JOIN DIM_Incoterms  ON dw_Shipments.IncotermId = DIM_Incoterms.Id
	inner JOIN DIM_Users SalesmanUser ON dw_Shipments.SalesmanUserId = SalesmanUser.Id
	inner JOIN DIM_Users AccountManagerUser ON dw_Shipments.AccountManagerUserId = AccountManagerUser.Id
	
	inner JOIN DIM_Currencies ProfitCurrency ON dw_Shipments.ProfitCurrencyId = ProfitCurrency.Id
	inner JOIN DIM_ShipmentStatuses  ON dw_Shipments.StatusId = DIM_ShipmentStatuses.Id
	inner JOIN dw_Tenants  ON dw_Shipments.Tenant = dw_Tenants.Id
	inner JOIN DIM_Currencies LocalCurrency ON dw_Tenants.CurrencyId = LocalCurrency.Id
    inner JOIN Dim_Ports fromPort  ON dw_Shipments.FromPortId = FromPort.Id
	inner JOIN Dim_Ports toPort  ON dw_Shipments.ToPortId = toPort.Id
	inner JOIN Dim_Ports mainCarriageToPort  ON dw_ShipmentMasterDatas.MainCarriageToPortId = mainCarriageToPort.Id
	inner JOIN Dim_Ports transshipment1ToPort  ON dw_ShipmentMasterDatas.Transshipment1ToPortId = transshipment1ToPort.Id
	inner JOIN Dim_Ports transshipment2ToPort  ON dw_ShipmentMasterDatas.Transshipment2ToPortId = transshipment2ToPort.Id
	inner JOIN Dim_Ports transshipment3ToPort  ON dw_ShipmentMasterDatas.Transshipment3ToPortId = transshipment3ToPort.Id
	where dw_Shipments.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO    @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @Level , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages , @NumberOfContainers , @Salesman ,@AccountManager, @TotalReceivablesInLocalCurrency , @TotalPayablesInLocalCurrency ,@TotalProfitInLocalCurrency , @TotalReceivablesInProfitCurrency, 
	@TotalPayablesInProfitCurrency , @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency , @NumberOfInvoices , @OperationallyClosed , @AccountingClosed , @Status , @Location   ,
	  @DepartedDate ,  @ArrivedDate ,  @CustomsClearenceDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @Origin ,@ToPortId,@DirectionId,@TransportModeId , @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
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
	
		 set @FinalDestination = @ToPortId;
		 if(@FinalDestination is null) begin set @FinalDestination= 1; end

		 set @IsArrived = 1;set @IsDeparted = 1;set @IsCustomsCleared = 1;
		 if(@ArrivedDate is null) begin set @IsArrived= 0; end
	     if(@DepartedDate is null) begin set @IsDeparted=0; end
		 if(@CustomsClearenceDate is null) begin set @IsCustomsCleared= 0 end


		 
		insert into Fact_Shipments values(@Id, @SourceTenant,@ParentTenant,@Direction,@TransportMode, @Level, @Type , @Department ,@Branch , @ShipmentNumber , @House ,@Master , @Shipper,  @Consignee , @Agent,@Customer,@Incoterm ,@TotalGrossWeightInKG,@TotalChargeableWeightInKG, @TotalVolumeInCBM,  @NumberOfPackages, @NumberOfContainers, @Salesman , @AccountManager ,   @TotalReceivablesInLocalCurrency , @TotalPayablesInLocalCurrency ,@TotalProfitInLocalCurrency , @TotalReceivablesInProfitCurrency, @TotalPayablesInProfitCurrency , @TotalProfitInProfitCurrency , @LocalCurrency,@ProfitCurrency,@NumberOfInvoices ,@OperationallyClosed,@AccountingClosed, @Status, @Location,  @Origin , @FinalDestination , @IsDeparted , @DepartedDate ,@IsArrived , @ArrivedDate , @IsCustomsCleared ,@CustomsClearenceDate )

	FETCH NEXT FROM ShipmentsCursor    INTO   @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @Level , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages , @NumberOfContainers , @Salesman ,@AccountManager, @TotalReceivablesInLocalCurrency , @TotalPayablesInLocalCurrency ,@TotalProfitInLocalCurrency , @TotalReceivablesInProfitCurrency, 
	@TotalPayablesInProfitCurrency , @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency , @NumberOfInvoices , @OperationallyClosed , @AccountingClosed , @Status , @Location   ,
	  @DepartedDate ,  @ArrivedDate ,  @CustomsClearenceDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @Origin ,@ToPortId,@DirectionId,@TransportModeId , @Tenant
		End
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor


	
	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Shipment'

End

