

   declare @ShipmentId as varchar(15)
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
   declare @MainCarriageFromPort as int
   declare @MainCarriageToPort as int
   declare @FinalDestination as int
   declare @Tenant as int
   declare @MasterDataId as varchar(15)
   declare @DirectionId as varchar(1)
   declare @TransportModeId as varchar(1)
   declare @ToPort as int
   declare @ShipmentCreateDate as datetime

   declare @AgentReference1 as varchar(50)
   declare @AgentReference2 as varchar(50)
   declare @CustomerReference1 as varchar(50)
   declare @CustomerReference2 as varchar(50)
   declare @ShipmentCreatedBy  as int
   declare @Carrier  as int
   declare @FirstOperationalCloseDate as datetime
   declare @SpecialServices as int
   declare @MasterShipmentNumber as varchar(20)
   declare @MainCarriageATA as datetime
   declare @MainCarriageATD as datetime
   declare @ShipmentOperationalDate as datetime
   declare @ShipmentOperationallyClosed as bit
   declare @ShipmentAccountingClosed as bit

   declare @PayableId as varchar(15)
   declare @ReceivableId as varchar(15)

   declare @ChargesType as int

   declare @OpenPayablesinLocal as float
   declare @AccountedPayablesinLocal as float
  
   declare @OpenPayablesinProfit as float
   declare @AccountedPayablesinProfit as float
   declare @InvoiceNumber as varchar(20)
   declare @InvoiceCurrencyExchangeRate as float

   declare @InvoiceCurrency as int
   declare @VATamountinInvoiceCurrency as float
   declare @ShipmentPayablesReceivablesType as varchar(20)
   declare @ReceivablesTotalAmount as float
   declare @ReceivablesTotalAmountLocal as float
   declare @ReceivablesInvoiceLineId as varchar(20)
   declare @InvoiceId as  varchar(15)

   declare @Vendor as int
   declare @BillTo as int

   declare @OperationalCloseDate as datetime
   declare @AccountingCloseDate as datetime
   declare @RegistryDate as datetime
   declare @ProjectNumber as  varchar(100)
   declare @Shipper as int
   declare @Consignee as int
   declare @Routing as varchar(100)
   declare @Incoterm as int
	
   declare @ComputedStatus as int
   
   declare @InvoiceDate as datetime

   declare @HousesOpenPayablesInLocal as float
   declare @HousesOpenPayablesInProfit as float
   declare @HousesACCTPayablesInLocal as float
   declare @HousesACCTPayablesInProfit as float
   declare @HousesOpenReceivablesInLocal as float
   declare @HousesOpenReceivablesInProfit as float
   declare @HousesACCTReceivablesInLocal as float
   declare @HousesACCTReceivablesInProfit as float  

   declare @InvoiceLineAmountForeign as float
   declare @ForiegnCurrencyId as int 
   declare @InvoiceChargeLineDescription as nvarchar(500)
   declare @ChargeTypeNote as nvarchar(250)
   declare @ShipmentPayablesReceivablesInvoiceStatusCode as varchar(2)
   declare @ShipmentPayablesReceivablesInvoiceDraftNumber as varchar(20)
   declare @InvoiceLineDescription as nvarchar(250)
   declare @InvoiceLineLocalDescription as nvarchar(250)

	DECLARE ShipmentsChargesCursor CURSOR READ_ONLY
	FOR
	 
	with ShipmentPayablesReceivables as(select ShipmentId,ChargesTypeId,EntityType,InvoiceNumber,InvoiceCurrencyId,InvoiceCurrencyExchangeRate  ,AmountInInvoiceCurrency,  OpenPayablesinLocal , OpenPayablesinProfit , AccountedPayablesinLocal , AccountedPayablesinProfit , InvoiceLineId , ReceivablesTotalAmount , ReceivablesTotalAmountLocal ,PayableId,ReceivableId , BillTo ,Vendor , InvoiceId, InvoiceDate,InvoiceLineAmountForeign, ForiegnCurrencyId, InvoiceChargeLineDescription, ChargeTypeNote, InvoiceStatusCode, InvoiceDraftNumber,InvoiceLineDescription, InvoiceLineLocalDescription from(
		
		select dw_ShipmentPayables.ShipmentId ,dw_ShipmentPayables.ChargesTypeId, 'Payables' as EntityType , dw_APInvoices.InvoiceNumber,dw_APInvoices.InvoiceCurrencyId,dw_APInvoices.InvoiceCurrencyExchangeRate ,  dw_APInvoices.AmountInInvoiceCurrency, 0 as OpenPayablesinLocal ,0 as OpenPayablesinProfit , dw_APInvoiceLines.LocalCurrencyAmount as AccountedPayablesinLocal ,dw_APInvoiceLines.ProfitCurrencyAmount   as AccountedPayablesinProfit , '' as  InvoiceLineId ,
		0 as ReceivablesTotalAmount ,0 as ReceivablesTotalAmountLocal , dw_ShipmentPayables.Id as PayableId, null as ReceivableId , 1 as BillTo  , vendorPartners.Id_Number as  Vendor ,dw_APInvoices.Id as InvoiceId, dw_APInvoices.InvoiceDate as InvoiceDate, dw_APInvoiceLines.ForiegnCurrencyAmount   as InvoiceLineAmountForeign, dw_APInvoiceLines.ForiegnCurrencyId   as ForiegnCurrencyId, dw_APInvoiceLines.Notes  as InvoiceChargeLineDescription, dw_ShipmentPayables.Notes as  ChargeTypeNote, null as InvoiceStatusCode, null as InvoiceDraftNumber,dw_APInvoiceLines.Description  as InvoiceLineDescription,dw_APInvoiceLines.LocalDescription  as InvoiceLineLocalDescription  from dw_shipments 
 
        inner JOIN dw_ShipmentPayables  ON dw_shipments.Id = dw_ShipmentPayables.ShipmentId
      	inner JOIN NewDIM_Partners vendorPartners ON dw_ShipmentPayables.VendorId = vendorPartners.Id
	    inner JOIN dw_APInvoiceLines  ON dw_ShipmentPayables.Id = dw_APInvoiceLines.EntityPayableId
        inner JOIN dw_APInvoices  ON dw_APInvoiceLines.APInvoiceId = dw_APInvoices.Id
		where dw_Shipments.IsCancelled = 0 and dw_ShipmentPayables.ShipmentPayableParentId is null  and dw_Shipments.ShipmentLevelCode in ('H','D','C')  and ( (dw_APInvoiceLines.LocalCurrencyAmount is not null and dw_APInvoiceLines.LocalCurrencyAmount !=0) or (dw_APInvoiceLines.ProfitCurrencyAmount is not null and dw_APInvoiceLines.ProfitCurrencyAmount !=0) )
		union

 
		select dw_ShipmentPayables.ShipmentId ,dw_ShipmentPayables.ChargesTypeId, 'Payables' as EntityType , dw_APInvoices.InvoiceNumber,dw_APInvoices.InvoiceCurrencyId,dw_APInvoices.InvoiceCurrencyExchangeRate ,  dw_APInvoices.AmountInInvoiceCurrency, 0 as OpenPayablesinLocal ,0 as OpenPayablesinProfit , dw_ShipmentPayables.AccountedAmountInLocalCurrency as AccountedPayablesinLocal ,
		dw_ShipmentPayables.AccountedAmountInProfitCurrency   as AccountedPayablesinProfit , '' as  InvoiceLineId , 0 as ReceivablesTotalAmount ,0 as ReceivablesTotalAmountLocal , dw_ShipmentPayables.Id as PayableId, null as ReceivableId , 1 as BillTo  , vendorPartners.Id_Number as  Vendor ,dw_APInvoices.Id as InvoiceId, dw_APInvoices.InvoiceDate as InvoiceDate, dw_APInvoiceLines.ForiegnCurrencyAmount   as InvoiceLineAmountForeign, dw_APInvoiceLines.ForiegnCurrencyId   as ForiegnCurrencyId , dw_APInvoiceLines.Notes  as InvoiceChargeLineDescription, dw_ShipmentPayables.Notes as  ChargeTypeNote, null as InvoiceStatusCode, null as InvoiceDraftNumber,dw_APInvoiceLines.Description  as InvoiceLineDescription,dw_APInvoiceLines.LocalDescription  as InvoiceLineLocalDescription  from dw_shipments 
 
        inner JOIN dw_ShipmentPayables  ON dw_shipments.Id = dw_ShipmentPayables.ShipmentId
		inner JOIN dw_ShipmentPayables masterPayables  ON dw_ShipmentPayables.ShipmentPayableParentId = masterPayables.Id
      	inner JOIN NewDIM_Partners vendorPartners ON masterPayables.VendorId = vendorPartners.Id
	    inner JOIN dw_APInvoiceLines  ON masterPayables.Id = dw_APInvoiceLines.EntityPayableId
        inner JOIN dw_APInvoices  ON dw_APInvoiceLines.APInvoiceId = dw_APInvoices.Id
		where dw_Shipments.IsCancelled = 0 and dw_ShipmentPayables.ShipmentPayableParentId is not null  and dw_Shipments.ShipmentLevelCode in ('H','D','C')  and ( (dw_APInvoiceLines.LocalCurrencyAmount is not null and dw_APInvoiceLines.LocalCurrencyAmount !=0) or (dw_APInvoiceLines.ProfitCurrencyAmount is not null and dw_APInvoiceLines.ProfitCurrencyAmount !=0) )
		union

		 
      select dw_ShipmentReceivables.ShipmentId ,dw_ShipmentReceivables.ChargesTypeId ,'Receivables' as EntityType , dw_ARInvoices.InvoiceNumber,dw_ARInvoices.InvoiceCurrencyId,dw_ARInvoices.InvoiceCurrencyExchangeRate ,
	  dw_ARInvoices.AmountInInvoiceCurrency ,0 as OpenPayablesinLocal  , 0 as OpenPayablesinProfit,  0 as AccountedPayablesinLocal ,0 as AccountedPayablesinProfit, dw_ShipmentReceivables.ARInvoiceLineId as InvoiceLineId ,
	  dw_ShipmentReceivables.AmountInProfitCurrency as ReceivablesTotalAmount ,dw_ShipmentReceivables.TotalAmountLocal as ReceivablesTotalAmountLocal ,null as PayableId  , dw_ShipmentReceivables.Id as ReceivableId , billToPartners.Id_Number as  BillTo , 
	  1 as Vendor ,dw_ARInvoices.Id as InvoiceId, dw_ARInvoices.InvoiceDate as InvoiceDate, dw_ARInvoiceLines.ForiegnCurrencyAmount   as InvoiceLineAmountForeign, dw_ARInvoiceLines.ForiegnCurrencyId   as ForiegnCurrencyId,   dw_ARInvoiceLines.Notes  as InvoiceChargeLineDescription, dw_ShipmentReceivables.Notes as  ChargeTypeNote, dw_ARInvoices.StatusCode as InvoiceStatusCode, dw_ARInvoices.DraftNumber  as InvoiceDraftNumber, dw_ARInvoiceLines.Description  as InvoiceLineDescription,dw_ARInvoiceLines.LocalDescription  as InvoiceLineLocalDescription       from dw_shipments 
 
        inner JOIN dw_ShipmentReceivables  ON dw_shipments.Id = dw_ShipmentReceivables.ShipmentId
        left JOIN dw_ARInvoiceLines  ON dw_ShipmentReceivables.Id = dw_ARInvoiceLines.ReceivableId
        left JOIN dw_ARInvoices  ON dw_ARInvoiceLines.ARInvoiceId = dw_ARInvoices.Id
        left JOIN NewDIM_Partners billToPartners ON dw_ARInvoices.BillToId = billToPartners.Id
		where dw_Shipments.IsCancelled = 0 and  dw_ShipmentReceivables.ShipmentReceivableParentId is null and  dw_Shipments.ShipmentLevelCode in ('H','D','C')  and ( (dw_ShipmentReceivables.TotalAmountLocal is not null and dw_ShipmentReceivables.TotalAmountLocal !=0) or (dw_ShipmentReceivables.AmountInProfitCurrency is not null and dw_ShipmentReceivables.AmountInProfitCurrency !=0) )
		union 
		
		 
	    select dw_ShipmentReceivables.ShipmentId ,dw_ShipmentReceivables.ChargesTypeId ,'Receivables' as EntityType , dw_ARInvoices.InvoiceNumber,dw_ARInvoices.InvoiceCurrencyId,dw_ARInvoices.InvoiceCurrencyExchangeRate ,dw_ARInvoices.AmountInInvoiceCurrency ,0 as OpenPayablesinLocal  ,
		0 as OpenPayablesinProfit,  0 as AccountedPayablesinLocal ,0 as AccountedPayablesinProfit, dw_ShipmentReceivables.ARInvoiceLineId as InvoiceLineId , dw_ShipmentReceivables.AmountInProfitCurrency as ReceivablesTotalAmount ,dw_ShipmentReceivables.TotalAmountLocal as ReceivablesTotalAmountLocal ,
		null as PayableId  , dw_ShipmentReceivables.Id as ReceivableId , billToPartners.Id_Number as  BillTo , 1 as Vendor ,dw_ARInvoices.Id as InvoiceId, dw_ARInvoices.InvoiceDate as InvoiceDate,  dw_ARInvoiceLines.ForiegnCurrencyAmount   as InvoiceLineAmountForeign, dw_ARInvoiceLines.ForiegnCurrencyId   as ForiegnCurrencyId, dw_ARInvoiceLines.Notes  as InvoiceChargeLineDescription, dw_ShipmentReceivables.Notes as  ChargeTypeNote, dw_ARInvoices.StatusCode as InvoiceStatusCode, dw_ARInvoices.DraftNumber  as InvoiceDraftNumber, dw_ARInvoiceLines.Description  as InvoiceLineDescription,dw_ARInvoiceLines.LocalDescription  as InvoiceLineLocalDescription    from dw_shipments 
 
        inner JOIN dw_ShipmentReceivables  ON dw_shipments.Id = dw_ShipmentReceivables.ShipmentId
		inner JOIN dw_ShipmentReceivables masterReceivables  ON dw_ShipmentReceivables.ShipmentReceivableParentId = masterReceivables.Id
        left JOIN dw_ARInvoiceLines  ON masterReceivables.Id = dw_ARInvoiceLines.ReceivableId
        left JOIN dw_ARInvoices  ON dw_ARInvoiceLines.ARInvoiceId = dw_ARInvoices.Id
        left JOIN NewDIM_Partners billToPartners ON dw_ARInvoices.BillToId = billToPartners.Id
		where dw_Shipments.IsCancelled = 0 and  dw_ShipmentReceivables.ShipmentReceivableParentId is not null and  dw_Shipments.ShipmentLevelCode in ('H','D','C')  and ( (dw_ShipmentReceivables.TotalAmountLocal is not null and dw_ShipmentReceivables.TotalAmountLocal !=0) or (dw_ShipmentReceivables.AmountInProfitCurrency is not null and dw_ShipmentReceivables.AmountInProfitCurrency !=0) )
		union 


 
		select dw_ShipmentPayables.ShipmentId ,dw_ShipmentPayables.ChargesTypeId, 'Payables' as EntityType , null , null , null  ,  null, dw_ShipmentPayables.OpenAmountInLocalCurrency as OpenPayablesinLocal ,dw_ShipmentPayables.OpenAmountInProfitCurrency as OpenPayablesinProfit , 0 as AccountedPayablesinLocal ,0 as AccountedPayablesinProfit , 
		'' as  InvoiceLineId , 0 as ReceivablesTotalAmount ,0 as ReceivablesTotalAmountLocal , dw_ShipmentPayables.Id as PayableId, null as ReceivableId,  1 as BillTo  , vendorPartners.Id_Number as  Vendor , null as InvoiceId, null as InvoiceDate,  0 as InvoiceLineAmountForeign, null as ForiegnCurrencyId,  null as InvoiceChargeLineDescription,  dw_ShipmentPayables.Notes as  ChargeTypeNote, null as InvoiceStatusCode, null as InvoiceDraftNumber, null as InvoiceLineDescription,  null as InvoiceLineLocalDescription from dw_shipments 
 
        inner JOIN dw_ShipmentPayables  ON dw_shipments.Id = dw_ShipmentPayables.ShipmentId
		 inner JOIN NewDIM_Partners vendorPartners ON dw_ShipmentPayables.VendorId = vendorPartners.Id
		where dw_Shipments.IsCancelled = 0 and dw_Shipments.ShipmentLevelCode in ('H','D','C') and ( (dw_ShipmentPayables.OpenAmountInLocalCurrency is not null and dw_ShipmentPayables.OpenAmountInLocalCurrency !=0) or (dw_ShipmentPayables.OpenAmountInProfitCurrency is not null and dw_ShipmentPayables.OpenAmountInProfitCurrency !=0) )
)tt
)

	SELECT  dw_Shipments.Id, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number] ,  NewDIM_Directions.Name, TransportModes.Name ,NewDIM_Levels.Name,  NewDIM_Types.Name , NewDIM_Departments.Id_Number ,NewDIM_Branches.Id_Number, dw_Shipments.ShipmentNumber,dw_Shipments.House,dw_ShipmentMasterDatas.Master , agentPartners.Id_Number,customerPartners.Id_Number 
	,SalesmanUser.Id_Number, AccountManagerUser.Id_Number ,mainCarriageToPort.Id_Number , fromPort.Id_Number, toPort.Id_Number , dw_Shipments.CreateDateTime
	,dw_Shipments.AgentReference1, dw_Shipments.AgentReference2,dw_Shipments.CustomerReference1,dw_Shipments.CustomerReference2, createdByUser.Id_Number,mainCarriageCarrierPartners.Id_Number,dw_Shipments.FirstOperationalCloseDate,NewDIM_SpecialServicesTypes.Id_Number,	dw_ShipmentMasterDatas.MasterShipmentNumber,dw_ShipmentMasterDatas.MainCarriageATA,dw_ShipmentMasterDatas.MainCarriageATD,dw_Shipments.OperationalDate,dw_Shipments.IsOperationalClosed,dw_Shipments.IsAccountingClosed,dw_ShipmentMasterDatas.AirlinePrefix,
	 dw_Shipments.DirectionId ,dw_Shipments.TransportModeId ,dw_Shipments.Tenant,dw_Shipments.MasterShipmentDataId
	 ,NewDIM_ChargesTypes.Id_Number, ShipmentPayablesReceivables.EntityType, ShipmentPayablesReceivables.InvoiceNumber,InvoiceCurrency.Id_Number , ShipmentPayablesReceivables.InvoiceCurrencyExchangeRate
	 ,ShipmentPayablesReceivables.OpenPayablesinLocal , ShipmentPayablesReceivables.OpenPayablesinProfit ,ShipmentPayablesReceivables.AccountedPayablesinLocal,ShipmentPayablesReceivables.AccountedPayablesinProfit
	 ,ShipmentPayablesReceivables.ReceivablesTotalAmount,  ShipmentPayablesReceivables.ReceivablesTotalAmountLocal,  ShipmentPayablesReceivables.InvoiceLineId , ShipmentPayablesReceivables.AmountInInvoiceCurrency , ShipmentPayablesReceivables.PayableId,ShipmentPayablesReceivables.ReceivableId
 
	,ShipmentPayablesReceivables.BillTo ,ShipmentPayablesReceivables.Vendor , ShipmentPayablesReceivables.InvoiceId, dw_Shipments.OperationalCloseDate, dw_Shipments.AccountingCloseDate, dw_Shipments.RegistryDate, dw_Shipments.ProjectNumber,shipperPartners.Id_Number, consigneePartners.Id_Number, dw_Shipments.Routing, NewDIM_Incoterms.Id_Number, ShipmentPayablesReceivables.InvoiceDate, ShipmentPayablesReceivables.InvoiceLineAmountForeign, ForiegnCurrencyId.Id_Number,  ShipmentPayablesReceivables.InvoiceChargeLineDescription, ShipmentPayablesReceivables.ChargeTypeNote,dw_Shipments.HousesOpenPayablesInLocal, dw_Shipments.HousesOpenPayablesInProfit, dw_Shipments.HousesACCTPayablesInLocal, dw_Shipments.HousesACCTPayablesInProfit, dw_Shipments.HousesOpenReceivablesInLocal, dw_Shipments.HousesOpenReceivablesInProfit, dw_Shipments.HousesACCTReceivablesInLocal, dw_Shipments.HousesACCTReceivablesInProfit
 
	,NewDIM_ShipmentStatuses.Id_Number, ShipmentPayablesReceivables.InvoiceStatusCode, ShipmentPayablesReceivables.InvoiceDraftNumber, ShipmentPayablesReceivables.InvoiceLineDescription, ShipmentPayablesReceivables.InvoiceLineLocalDescription

	 
	
	
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
	inner JOIN NewDIM_Partners shipperPartners ON dw_Shipments.ShipperId = shipperPartners.Id
	inner JOIN NewDIM_Partners consigneePartners ON dw_Shipments.ConsigneeId = consigneePartners.Id
	inner JOIN NewDIM_Incoterms  ON dw_Shipments.IncotermId = NewDIM_Incoterms.Id
	inner JOIN NewDIM_Users SalesmanUser ON dw_Shipments.SalesmanUserId = SalesmanUser.Id
	inner JOIN NewDIM_Users AccountManagerUser ON dw_Shipments.AccountManagerUserId = AccountManagerUser.Id
	inner JOIN NewDIM_ShipmentStatuses  ON dw_Shipments.ComputedStatusId = NewDIM_ShipmentStatuses.Id
	inner JOIN dw_Tenants  ON dw_Shipments.Tenant = dw_Tenants.Id
    inner JOIN NewDIM_Ports fromPort  ON dw_Shipments.FromPortId = FromPort.Id
	inner JOIN NewDIM_Ports toPort  ON dw_Shipments.ToPortId = toPort.Id
	inner JOIN NewDIM_Ports mainCarriageToPort  ON dw_ShipmentMasterDatas.MainCarriageToPortId = mainCarriageToPort.Id
    inner JOIN NewDIM_Users createdByUser ON dw_Shipments.CreatedByUserId = createdByUser.Id
	inner JOIN NewDIM_Partners mainCarriageCarrierPartners ON dw_ShipmentMasterDatas.MainCarriageCarrierId = mainCarriageCarrierPartners.Id
    inner JOIN NewDIM_SpecialServicesTypes   ON dw_Shipments.SpecialServicesTypeId = NewDIM_SpecialServicesTypes.Id
    inner JOIN ShipmentPayablesReceivables ON dw_Shipments.Id = ShipmentPayablesReceivables.ShipmentId
    inner JOIN NewDIM_ChargesTypes  ON ShipmentPayablesReceivables.ChargesTypeId = NewDIM_ChargesTypes.Id
    left JOIN NewDIM_Currencies InvoiceCurrency ON ShipmentPayablesReceivables.InvoiceCurrencyId = InvoiceCurrency.Id
 --   inner JOIN NewDIM_Partners billToPartners ON ShipmentPayablesReceivables.BillTo = billToPartners.Id
	--inner JOIN NewDIM_Partners VendoroPartners ON ShipmentPayablesReceivables.Vendor = VendoroPartners.Id

	 left JOIN NewDIM_Currencies ForiegnCurrencyId ON ShipmentPayablesReceivables.ForiegnCurrencyId = ForiegnCurrencyId.Id

	where dw_Shipments.IsCancelled = 0 and dw_Shipments.ShipmentLevelCode in ('H','D','C') 

	OPEN ShipmentsChargesCursor FETCH NEXT FROM ShipmentsChargesCursor    INTO   @ShipmentId ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Agent, @Customer 
	, @Salesman ,@AccountManager  , @MainCarriageToPort, @MainCarriageFromPort ,@ToPort ,@ShipmentCreateDate, @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 ,@ShipmentCreatedBy , 
	@Carrier, @FirstOperationalCloseDate,@SpecialServices,@MasterShipmentNumber,@MainCarriageATA, @MainCarriageATD, @ShipmentOperationalDate, @ShipmentOperationallyClosed, @ShipmentAccountingClosed, @AirlinePrefix , @DirectionId,@TransportModeId , @Tenant,@MasterDataId
    ,@ChargesType,@ShipmentPayablesReceivablesType, @InvoiceNumber,@InvoiceCurrency,@InvoiceCurrencyExchangeRate
	,@OpenPayablesinLocal,@OpenPayablesinProfit,@AccountedPayablesinLocal,@AccountedPayablesinProfit
	,@ReceivablesTotalAmount , @ReceivablesTotalAmountLocal, @ReceivablesInvoiceLineId,@VATamountinInvoiceCurrency,	@PayableId ,@ReceivableId ,@BillTo ,@Vendor , @InvoiceId, @OperationalCloseDate, @AccountingCloseDate, @RegistryDate, @ProjectNumber, @Shipper, @Consignee, @Routing, @Incoterm, @InvoiceDate, @InvoiceLineAmountForeign,@InvoiceChargeLineDescription, @ChargeTypeNote, @ForiegnCurrencyId, @HousesOpenPayablesInLocal, @HousesOpenPayablesInProfit, @HousesACCTPayablesInLocal, @HousesACCTPayablesInProfit, @HousesOpenReceivablesInLocal, @HousesOpenReceivablesInProfit, @HousesACCTReceivablesInLocal, @HousesACCTReceivablesInProfit

	,@ComputedStatus, @ShipmentPayablesReceivablesInvoiceStatusCode, @ShipmentPayablesReceivablesInvoiceDraftNumber, @InvoiceLineDescription, @InvoiceLineLocalDescription

	



	WHILE @@FETCH_STATUS = 0
	BEGIN



	



    declare @OpenReceivablesinLocal as float =0
   declare @AccountedReceivablesinLocal as float =0
    declare @OpenReceivablesinProfit as float =0
   declare @AccountedReceivablesinProfit as float =0
      declare @IsOpenReceivable as bit = 0
   declare @IsOpenPayable as bit = 0



   
	 --   declare @RecordType as varchar(100)
		--set @RecordType = 'Master';
		--if(@DirectHouse = 'House' or @DirectHouse = 'Direct') begin  set @RecordType = 'Shipment'; end



	--------------Long Master Number------------------
	 if(@TransportModeId = 'A' and @Master is not null and @AirlinePrefix is not null)
	 BEGIN
	 SET @Master =  @AirlinePrefix + '-'+@Master  ;
	 end
    ----------------------------------------------

	--------------Shipment Type------------------
	if(@TransportModeId = 'A' and @Type ='Not Specified')  BEGIN
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


		  --Receivables
		   if(@ShipmentPayablesReceivablesType = 'Receivables')
		BEGIN

				--Receivables
				  if(@ReceivablesInvoiceLineId is not null)
				
				  BEGIN
					set @AccountedReceivablesinProfit  =@ReceivablesTotalAmount;
					set @AccountedReceivablesinLocal  = @ReceivablesTotalAmountLocal;
				  End
				  else 
				  
				  BEGIN
				    set @OpenReceivablesinProfit  = @ReceivablesTotalAmount;
					set @OpenReceivablesinLocal  =@ReceivablesTotalAmountLocal;

		
				   if((@OpenReceivablesinLocal !=0  and @OpenReceivablesinLocal is not null) or (@OpenReceivablesinProfit !=0  and @OpenReceivablesinProfit is not null))
				   begin   set @IsOpenReceivable = 1; end
				  End



	--------------AR Invoice Number------------------

				IF(@ShipmentPayablesReceivablesInvoiceStatusCode = 'DR' or @ShipmentPayablesReceivablesInvoiceStatusCode = 'LL')
					BEGIN
						IF(@ShipmentPayablesReceivablesInvoiceDraftNumber is not null)
							BEGIN
								SET @InvoiceNumber = @ShipmentPayablesReceivablesInvoiceDraftNumber;
							END
						ELSE
							BEGIN
								SET @InvoiceNumber = @InvoiceId;
							END
					END
		----------------------------------------------

		 End
		 

		 --Payable
		    else 
			 begin

		 		  if((@OpenPayablesinLocal !=0  and @OpenPayablesinLocal is not null) or (@OpenPayablesinProfit !=0  and @OpenPayablesinProfit is not null))
				  begin   set @IsOpenPayable = 1; end

			end 



		 if(@BillTo is null) begin SET @BillTo = 1;end
		  if(@InvoiceCurrency is null) begin SET @InvoiceCurrency = 1;end
		  if(@ForiegnCurrencyId is null) begin SET @ForiegnCurrencyId = 1;end
		   


			
	 BEGIN TRY  

      insert into #Fact_ChargesTemp ([Shipment Id],[Source Tenant],[Parent Tenant],[Direction],[Transport Mode],[DirectHouse],[Type],[Department],[Branch],[Shipment Number],[House],[Master],[Agent],[Customer]
	  ,[Salesman],[Account Manager],[Status],[MainCarriage From Port],[MainCarriage To Port],[Shipment Create Date],  [Shipment Create Date Time] , [Agent Ref1],[Agent Ref2],[Customer Ref1],[Customer Ref2] , [Shipment Created By]
	  ,[Carrier] , [First Operational Close Date],     [Special Services] , [Master Shipment Number] , [Main Carriage ATA] , [Main Carriage ATD] , [Shipment Operational Date] , [Shipment Operationally Closed] , [Shipment Accounting Closed]
	  ,[Charges Type],[Invoice Number] ,[Invoice Currency] , [Invoice Exchange Rate] ,[Open Payables in Local],[Open Payables in Profit] , [Accounted Payables in Local],[Accounted Payables in Profit],[Open Receivables in Local] ,[Open Receivables in Profit],[Accounted Receivables in Local],[Accounted Receivables in Profit], [Is Open Receivable],[Is Open Payable],[VAT amount in Invoice Currency],[Payable Id] ,[Receivable Id] ,[Bill To],[Vendor] , [Invoice Id]
    ,[Operational Close Date],[Accounting Close Date],[Registry Date],[Project#],[Shipper],[Consignee],[Routing],[Incoterm],[Invoice Date],[Invoice Line Amount (Foreign)], [Invoice Line Foreign Currency], [Invoice Charge Line Description], [Charge Type Note], [Houses Open Payables In Local],[Houses Open Payables In Profit],[Houses ACCT Payables In Local],[Houses ACCT Payables In Profit],[Houses Open Receivables In Local],[Houses Open Receivables In Profit],[Houses ACCT Receivables In Local],[Houses ACCT Receivables In Profit],
	 [Invoice Line Description],[Invoice Line Local Description]) 
 
	
      values(@ShipmentId, @SourceTenant,@ParentTenant,@Direction,@TransportMode, @DirectHouse, @Type , @Department ,@Branch , @ShipmentNumber , @House ,@Master ,  @Agent,@Customer,
	  @Salesman , @AccountManager ,  @ComputedStatus, @MainCarriageFromPort ,@MainCarriageToPort , dbo.GetDateFormateAsNumber(@ShipmentCreateDate) ,@ShipmentCreateDate  ,@AgentReference1, @AgentReference2,@CustomerReference1, @CustomerReference2, @ShipmentCreatedBy,
      @Carrier ,   dbo.GetDateFormateAsNumber(@FirstOperationalCloseDate), @SpecialServices , @MasterShipmentNumber, @MainCarriageATA, @MainCarriageATD, dbo.GetDateFormateAsNumber(@ShipmentOperationalDate), @ShipmentOperationallyClosed, @ShipmentAccountingClosed,
     @ChargesType ,  @InvoiceNumber ,@InvoiceCurrency ,@InvoiceCurrencyExchangeRate ,   @OpenPayablesinLocal,@OpenPayablesinProfit,@AccountedPayablesinLocal , @AccountedPayablesinProfit, @OpenReceivablesinLocal,@OpenReceivablesinProfit,@AccountedReceivablesinLocal,@AccountedReceivablesinProfit, @IsOpenReceivable,@IsOpenPayable, @VATamountinInvoiceCurrency ,@PayableId,@ReceivableId,@BillTo ,@Vendor,@InvoiceId, 
 
	 dbo.GetDateFormateAsNumber(@OperationalCloseDate), dbo.GetDateFormateAsNumber(@AccountingCloseDate), dbo.GetDateFormateAsNumber(@RegistryDate), @ProjectNumber, @Shipper, @Consignee, @Routing, @Incoterm, dbo.GetDateFormateAsNumber(@InvoiceDate), @InvoiceLineAmountForeign, @ForiegnCurrencyId, @InvoiceChargeLineDescription, @ChargeTypeNote, @HousesOpenPayablesInLocal, @HousesOpenPayablesInProfit, @HousesACCTPayablesInLocal, @HousesACCTPayablesInProfit, @HousesOpenReceivablesInLocal, @HousesOpenReceivablesInProfit, @HousesACCTReceivablesInLocal, @HousesACCTReceivablesInProfit,
	 @InvoiceLineDescription, @InvoiceLineLocalDescription)
 

	END TRY 
BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (ShipmentId: ' + @ShipmentId +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

	
	FETCH NEXT FROM ShipmentsChargesCursor    INTO   @ShipmentId ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @DirectHouse , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Agent, @Customer 
	, @Salesman ,@AccountManager  , @MainCarriageToPort, @MainCarriageFromPort ,@ToPort ,@ShipmentCreateDate, @AgentReference1,@AgentReference2,@CustomerReference1,@CustomerReference2 ,@ShipmentCreatedBy , 
	@Carrier, @FirstOperationalCloseDate,@SpecialServices,@MasterShipmentNumber,@MainCarriageATA,@MainCarriageATD,@ShipmentOperationalDate,@ShipmentOperationallyClosed,@ShipmentAccountingClosed,@AirlinePrefix,  @DirectionId,@TransportModeId , @Tenant,@MasterDataId
    ,@ChargesType,@ShipmentPayablesReceivablesType, @InvoiceNumber,@InvoiceCurrency,@InvoiceCurrencyExchangeRate
	,@OpenPayablesinLocal,@OpenPayablesinProfit,@AccountedPayablesinLocal,@AccountedPayablesinProfit
	,@ReceivablesTotalAmount , @ReceivablesTotalAmountLocal, @ReceivablesInvoiceLineId,@VATamountinInvoiceCurrency,	@PayableId ,@ReceivableId,@BillTo ,@Vendor , @InvoiceId 
	,@OperationalCloseDate, @AccountingCloseDate, @RegistryDate, @ProjectNumber, @Shipper, @Consignee, @Routing, @Incoterm, @InvoiceDate, @InvoiceLineAmountForeign, @ForiegnCurrencyId, @InvoiceChargeLineDescription, @ChargeTypeNote, @HousesOpenPayablesInLocal, @HousesOpenPayablesInProfit, @HousesACCTPayablesInLocal, @HousesACCTPayablesInProfit, @HousesOpenReceivablesInLocal, @HousesOpenReceivablesInProfit, @HousesACCTReceivablesInLocal, @HousesACCTReceivablesInProfit
 
     ,@ComputedStatus, @ShipmentPayablesReceivablesInvoiceStatusCode, @ShipmentPayablesReceivablesInvoiceDraftNumber,@InvoiceLineDescription, @InvoiceLineLocalDescription

		End
	CLOSE ShipmentsChargesCursor
	DEALLOCATE ShipmentsChargesCursor


	