

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
   declare @Agent as int
   declare @Customer as int
   declare @Salesman as int
   declare @AccountManager as int
   declare @Status as int
   declare @MainCarriageFromPort as int
   declare @MainCarriageToPort as int
   declare @FinalDestination as int
   declare @Tenant as int
   declare @MasterDataId as varchar(15)
   declare @DirectionId as varchar(1)
   declare @TransportModeId as varchar(1)
   declare @ToPort as int
   declare @CreateDate as datetime

   declare @AgentReference1 as varchar(50)
   declare @AgentReference2 as varchar(50)
   declare @CustomerReference1 as varchar(50)
   declare @CustomerReference2 as varchar(50)
   declare @CreatedBy  as int
   declare @Carrier  as int
   declare @FirstOperationalCloseDate as datetime
   declare @SpecialServices as int
   declare @MasterShipmentNumber as varchar(20)

	DECLARE ShipmentsChargesCursor CURSOR READ_ONLY
	FOR

	with q1 as(select ShipmentId,invoiceNumber,openAmountInLocalCurrency,openAmountInProfitCurrency,accountedAmountInLocalCurrency,accountedAmountInProfitCurrency from(
		select dw_ShipmentPayables.ShipmentId ,dw_APInvoices.InvoiceNumber, dw_ShipmentPayables.OpenAmountInLocalCurrency,  dw_ShipmentPayables.OpenAmountInProfitCurrency,dw_ShipmentPayables.AccountedAmountInLocalCurrency,dw_ShipmentPayables.AccountedAmountInProfitCurrency from dw_shipments 
        inner JOIN dw_ShipmentPayables  ON dw_shipments.Id = dw_ShipmentPayables.ShipmentId
        inner JOIN dw_APInvoiceLines  ON dw_ShipmentPayables.Id = dw_APInvoiceLines.EntityPayableId
        inner JOIN dw_APInvoices  ON dw_APInvoiceLines.APInvoiceId = dw_APInvoices.Id
		
		union

      select dw_ShipmentReceivables.ShipmentId ,dw_ARInvoices.InvoiceNumber, 0,  0,0,0 from dw_shipments 
        inner JOIN dw_ShipmentReceivables  ON dw_shipments.Id = dw_ShipmentReceivables.ShipmentId
        inner JOIN dw_ARInvoiceLines  ON dw_ShipmentReceivables.Id = dw_ARInvoiceLines.ReceivableId
        inner JOIN dw_ARInvoices  ON dw_ARInvoiceLines.ARInvoiceId = dw_ARInvoices.Id


)tt

)
   
	SELECT  dw_Shipments.Id, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number] ,  NewDIM_Directions.Name, TransportModes.Name ,NewDIM_Levels.Name,  NewDIM_Types.Name , NewDIM_Departments.Id_Number ,NewDIM_Branches.Id_Number , dw_Shipments.ShipmentNumber,dw_Shipments.House,dw_ShipmentMasterDatas.Master , agentPartners.Id_Number,customerPartners.Id_Number 
	,SalesmanUser.Id_Number, AccountManagerUser.Id_Number,NewDIM_ShipmentStatuses.Id_Number ,mainCarriageToPort.Id_Number , fromPort.Id_Number, toPort.Id_Number , dw_Shipments.CreateDateTime
	,dw_Shipments.AgentReference1, dw_Shipments.AgentReference2,dw_Shipments.CustomerReference1,dw_Shipments.CustomerReference2, createdByUser.Id_Number,mainCarriageCarrierPartners.Id_Number,dw_Shipments.FirstOperationalCloseDate,NewDIM_SpecialServicesTypes.Id_Number,	dw_ShipmentMasterDatas.MasterShipmentNumber,
	 dw_Shipments.DirectionId ,dw_Shipments.TransportModeId ,dw_Shipments.Tenant,dw_Shipments.MasterShipmentDataId
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
	inner JOIN NewDIM_Partners agentPartners ON dw_Shipments.AgentComputed = agentPartners.Id
	inner JOIN NewDIM_Partners customerPartners ON dw_Shipments.CustomerId = customerPartners.Id
	inner JOIN NewDIM_Users SalesmanUser ON dw_Shipments.SalesmanUserId = SalesmanUser.Id
	inner JOIN NewDIM_Users AccountManagerUser ON dw_Shipments.AccountManagerUserId = AccountManagerUser.Id
	inner JOIN NewDIM_ShipmentStatuses  ON dw_Shipments.StatusId = NewDIM_ShipmentStatuses.Id
	inner JOIN dw_Tenants  ON dw_Shipments.Tenant = dw_Tenants.Id
    inner JOIN NewDIM_Ports fromPort  ON dw_Shipments.FromPortId = FromPort.Id
	inner JOIN NewDIM_Ports toPort  ON dw_Shipments.ToPortId = toPort.Id
	inner JOIN NewDIM_Ports mainCarriageToPort  ON dw_ShipmentMasterDatas.MainCarriageToPortId = mainCarriageToPort.Id
    inner JOIN NewDIM_Users createdByUser ON dw_Shipments.CreatedByUserId = createdByUser.Id
	inner JOIN NewDIM_Partners mainCarriageCarrierPartners ON dw_ShipmentMasterDatas.MainCarriageCarrierId = mainCarriageCarrierPartners.Id
    inner JOIN NewDIM_SpecialServicesTypes   ON dw_Shipments.SpecialServicesTypeId = NewDIM_SpecialServicesTypes.Id
    inner JOIN q1 ON dw_Shipments.Id = q1.ShipmentId
	
	where dw_Shipments.IsCancelled = 0 and dw_Shipments.ShipmentLevelCode in ('H','D') 

	OPEN ShipmentsChargesCursor FETCH NEXT FROM ShipmentsChargesCursor    INTO   @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Agent, @Customer 
	, @Salesman ,@AccountManager , @Status , @MainCarriageToPort, @MainCarriageFromPort ,@ToPort ,@CreateDate, @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 ,@CreatedBy , 
	@Carrier, @FirstOperationalCloseDate,@SpecialServices,@MasterShipmentNumber, @DirectionId,@TransportModeId , @Tenant,@MasterDataId


	WHILE @@FETCH_STATUS = 0
	BEGIN



	--------------Long Master Number------------------
	 if(@TransportModeId = 'A' and @Master is not null and @AirlinePrefix is not null)
	 BEGIN
	 SET @Master =  @AirlinePrefix + '-'+@Master  ;
	 end
    ----------------------------------------------

	--------------Shipment Type------------------
	if(@TransportModeId = 'A' and @type ='Not Specified')  BEGIN
	 set @Type = 'Air';
	 end
    ----------------------------------------------

	--------------ToPort-------------------------
    if(@DirectionId != 'D' or @TransportModeId != 'I')
		BEGIN

				--Master
				  if(@MasterDataId is not null)
				
				  BEGIN
					if(@MainCarriageToPort is not null and @MainCarriageToPort!=1 )BEGIN set @ToPort = @MainCarriageToPort;END
				  End
		 End


	 BEGIN TRY  

      insert into #Fact_ChargesTemp ([Id],[Source Tenant],[Parent Tenant],[Direction],[Transport Mode],[DirectHouse],[Type],[Department],[Branch],[Shipment Number],[House],[Master],[Agent],[Customer]
	  ,[Salesman],[Account Manager],[Status],[MainCarriage From Port],[MainCarriage To Port],[Create Date],  [Create Date Time] , [Agent Ref1],[Agent Ref2],[Customer Ref1],[Customer Ref2] , [Created By]
	  ,[Carrier] , [First Operational Close Date],     [Special Services] , [Master Shipment Number] 
	  ,[Charge Type],[Invoice Currency],[VAT amount in Invoice Currency]
	  ) 

      values(@Id, @SourceTenant,@ParentTenant,@Direction,@TransportMode, @DirectHouse, @Type , @Department ,@Branch , @ShipmentNumber , @House ,@Master ,  @Agent,@Customer,
	  @Salesman , @AccountManager ,    @Status, @MainCarriageFromPort ,@MainCarriageToPort , dbo.GetDateFormateAsNumber(@CreateDate) ,@CreateDate  ,@AgentReference1, @AgentReference2,@CustomerReference1, @CustomerReference2, @CreatedBy,
      @Carrier ,   dbo.GetDateFormateAsNumber(@FirstOperationalCloseDate), @SpecialServices , @MasterShipmentNumber,1,1,1)

	END TRY 
BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (ShipmentId: ' + @Id +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

	
	FETCH NEXT FROM ShipmentsChargesCursor    INTO   @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Agent, @Customer 
	, @Salesman ,@AccountManager , @Status , @MainCarriageToPort, @MainCarriageFromPort ,@ToPort ,@CreateDate, @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 ,@CreatedBy , 
	@Carrier, @FirstOperationalCloseDate,@SpecialServices,@MasterShipmentNumber, @DirectionId,@TransportModeId , @Tenant,@MasterDataId
	
		End
	CLOSE ShipmentsChargesCursor
	DEALLOCATE ShipmentsChargesCursor


	