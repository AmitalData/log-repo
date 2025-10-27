

   declare @Id as varchar(15)
   declare @SourceTenant as int
   declare @ParentTenant as int
   declare @Direction as varchar(40)
   declare @TransportMode as varchar(13)
   declare @Type as varchar(40)
   declare @Department as int
   declare @Branch as int
   declare @QuoteNumber as varchar(15)
   
   declare @Shipper as int
   declare @Consignee as int
   declare @Agent as int
   declare @Customer as int
   declare @Incoterm as int

      declare @CreatedByUser as int
	    --@[DeclareCustomFieldsVariable]


	     declare @Tenant as int

   declare @TotalGrossWeightInKG as float
   declare @TotalChargeableWeightInKG as float
   declare @TotalVolumeInCBM as float 
   declare @NumberOfPackages as int
   declare @NumberOfContainers as int
   declare @Salesman as int
   declare @SaleCurrency as int


      declare @OpenDate as datetime
	        declare @SentDate as datetime
      declare @AcceptedDate as datetime
      declare @DeclinedDate as datetime
	     declare @StartDate as datetime
      declare @LastActivityDate as datetime
	      declare @ExpirationDate as datetime
      declare @StageDueDate as datetime
	  declare @AutomaticallyCloseDate as datetime
	  declare @QuoteStages as int
	  declare @QuoteClosingReasons as int


   declare @Notes as nvarchar(500)
   declare @EstimatedProfitInLocal as float
   declare @Subject as nvarchar(200)
   declare @GrossWeightInKG as float
   declare @ChargeableWeightInKG as float


   declare @VolumeInCBM as float
   declare @IsAutomaticallyClosed as bit
   declare @IncludePickUp as bit
   declare @IncludeDelivery as bit
   declare @IsQuoteDataExternal as bit
   declare @IsQuoteDocumentExternal as bit
    
      
   declare @FromCountry as int
   declare @ToCountry as int
	   declare @ShipperPartnerType  as varchar(20)
		   declare @ConsigneePartnerType  as varchar(20)
		   	   declare @CustomerPartnerType  as varchar(20)

   declare @ConnectedToShipment as bit
   declare @ConnectedToTicket as bit
   declare @ToLocation as varchar(500)
   declare @FromLocation as varchar(500)
   declare @DeliveryTo as varchar(500)
   declare @PickupFrom as varchar(500)
   declare @EstimatedPayablesInSales as float
   declare @EstimatedPayablesInLocal as float
   declare @EstimatedReceivablesInLocal as float
   declare @EstimatedReceivablesInSales as float
   declare @EstimateProfit as float
   declare @LocalCurrency as int
  declare @MarkupPercentage as float
    declare @CostChargeGroupVal as float


   declare @BusinessUnitId varchar(15)
   declare @SalesmanUserId as varchar(15)
   --declare @ShipmentSubType as int
   declare @ShipperNotExporter as int
   declare @ConsigneeNotImporter as int
   declare @IsCancelled as bit
   declare @ToPortCode as varchar(3)
   declare @ToPortName as varchar(40)
   declare @FromPortCode as varchar(3)
   declare @FromPortName as varchar(40)
   declare @ShipmentNumber as varchar(20)
   declare @CostChargeGroupVal as varchar(30)

	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Quotes.Id,dw_Quotes.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number] ,  DIM_Directions.Name, TransportModes.Name , 
	DIM_Types.Name, DIM_Departments.Id_Number ,DIM_Branches.Id_Number , dw_Quotes.QuoteNumber,
	shipperPartners.Id_Number, consigneePartners.Id_Number,agentPartners.Id_Number,customerPartners.Id_Number , DIM_Incoterms.Id_Number ,
	createdByUser.Id_Number , dw_Quotes.OpenDate , dw_Quotes.SentDate, dw_Quotes.AcceptedDate, dw_Quotes.DeclinedDate , dw_Quotes.StartDate, dw_Quotes.LastActivityDate,
	SalesmanUser.Id_Number, DIM_QuoteStages.Id_Number, dw_Quotes.Notes , DIM_QuoteClosingReasons.Id_Number,dw_Quotes.EstimatedProfitInLocal,
	SaleCurrency.Id_Number,dw_Quotes.Subject, dw_Quotes.GrossWeightInKG ,dw_Quotes.ChargeableWeightInKG,dw_Quotes.VolumeInCBM,dw_Quotes.NumberOfPackages , 
	dw_Quotes.NumberOfContainers,dw_Quotes.ExpirationDate,dw_Quotes.IsAutomaticallyClosed, dw_Quotes.IncludePickUp , 
	dw_Quotes.IncludeDelivery , dw_Quotes.StageDueDate , dw_Quotes.IsQuoteDataExternal, dw_Quotes.IsQuoteDocumentExternal,
	dw_Quotes.AutomaticallyCloseDate   , FromCountry.Id_Number , ToCountry.Id_Number , shipperPartners.[Partner Type]  , consigneePartners.[Partner Type] , customerPartners.[Partner Type],
	dw_QuoteComputedFields.ConnectedToShipment, dw_QuoteComputedFields.ConnectedToTicket, dw_QuoteComputedFields.ToLocation, dw_QuoteComputedFields.FromLocation, 
	dw_QuoteComputedFields.DeliveryTo, dw_QuoteComputedFields.PickupFrom, dw_QuoteComputedFields.EstimatedPayablesInSales, dw_QuoteComputedFields.EstimatedPayablesInLocal, 
	dw_QuoteComputedFields.EstimatedReceivablesInLocal, dw_QuoteComputedFields.EstimatedReceivablesInSales, dw_Quotes.EstimateProfit, LocalCurrency.Id_Number, dw_QuoteComputedFields.MarkupPercentage
	,toPort.Code, toPort.EnglishName,fromPort.Code,fromPort.EnglishName,dw_Quotes.ShipmentNumber, dw_QuoteComputedFields.CostChargeGroupVal






    From dw_Quotes
	inner JOIN DIM_Tenants SourceTenant ON dw_Quotes.Tenant = SourceTenant.[Tenant Number]
	inner JOIN dw_DWHSettings ON dw_Quotes.Tenant = dw_DWHSettings.Tenant
	inner JOIN DIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
	inner JOIN DIM_Directions ON dw_Quotes.DirectionId = DIM_Directions.Code
    inner JOIN DIM_TransportModes TransportModes ON dw_Quotes.TransportModeId = TransportModes.Code
	inner JOIN DIM_Types ON dw_Quotes.ShipmentTypeId = DIM_Types.Code
	inner JOIN DIM_Departments ON dw_Quotes.DepartmentId = DIM_Departments.Id
	inner JOIN DIM_Branches ON dw_Quotes.BranchId = DIM_Branches.Id
	inner JOIN DIM_Partners shipperPartners ON dw_Quotes.ShipperId = shipperPartners.Id
	inner JOIN DIM_Partners consigneePartners ON dw_Quotes.ConsigneeId = consigneePartners.Id
	inner JOIN DIM_Partners agentPartners ON dw_Quotes.AgentId = agentPartners.Id
	inner JOIN DIM_Partners customerPartners ON dw_Quotes.CustomerId = customerPartners.Id
	inner JOIN DIM_Incoterms  ON dw_Quotes.IncotermId = DIM_Incoterms.Id
	inner JOIN DIM_Users SalesmanUser ON dw_Quotes.SalesmanUserId = SalesmanUser.Id
	inner JOIN DIM_QuoteStages  ON dw_Quotes.StageId = DIM_QuoteStages.Id
	inner JOIN DIM_QuoteClosingReasons  ON dw_Quotes.QuoteClosingReasonId = DIM_QuoteClosingReasons.Id
	inner JOIN DIM_Currencies SaleCurrency ON dw_Quotes.SaleCurrencyId = SaleCurrency.Id
	inner JOIN DIM_Users createdByUser ON dw_Quotes.CreatedByUserId = createdByUser.Id
	
	inner JOIN dw_Ports fromPort ON dw_Quotes.FromPortId = fromPort.Id
	inner JOIN DIM_Countries FromCountry ON fromPort.CountryId = FromCountry.Id


		inner JOIN dw_CustomObjectFields  ON dw_Quotes.Tenant = dw_CustomObjectFields.Tenant and dw_CustomObjectFields.ObjectTableName = 'Quote'


	inner JOIN dw_Ports toPort ON dw_Quotes.ToPortId = toPort.Id
	inner JOIN DIM_Countries ToCountry ON toPort.CountryId = ToCountry.Id
	inner JOIN dw_QuoteComputedFields ON dw_Quotes.Id = dw_QuoteComputedFields.Id
	inner JOIN dw_Tenants  ON dw_Quotes.Tenant = dw_Tenants.Id
	inner JOIN DIM_Currencies LocalCurrency ON dw_Tenants.CurrencyId = LocalCurrency.Id
	inner JOIN DIM_Partners shipperNotExporterPartners ON dw_Quotes.ShipperNotExporterId = ShipperNotExporterPartners.Id
	inner JOIN DIM_Partners consigneeNotImporterPartners ON dw_Quotes.ConsigneeNotImporterId = consigneeNotImporterPartners.Id
	--inner JOIN DIM_ShipmentSubTypes ON dw_Quotes.ShipmentSubTypeId= DIM_ShipmentSubTypes.Id

	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor   into  @Id ,@Tenant , @SourceTenant, @ParentTenant ,@Direction , @TransportMode , @Type,@Department ,@Branch,@QuoteNumber
	 ,@Shipper ,@Consignee, @Agent ,@Customer , @Incoterm ,@CreatedByUser ,@OpenDate , @SentDate ,@AcceptedDate ,@DeclinedDate ,@StartDate ,@LastActivityDate
	, @Salesman ,@QuoteStages, @Notes ,@QuoteClosingReasons , @EstimatedProfitInLocal 
	, @SaleCurrency ,@Subject , @GrossWeightInKG ,@ChargeableWeightInKG ,@VolumeInCBM ,@NumberOfPackages ,@NumberOfContainers
	 , @ExpirationDate ,@IsAutomaticallyClosed ,@IncludePickUp ,@IncludeDelivery ,@StageDueDate ,@IsQuoteDataExternal ,@IsQuoteDocumentExternal,@AutomaticallyCloseDate , @FromCountry, @ToCountry  , @ShipperPartnerType ,@ConsigneePartnerType  ,  @CustomerPartnerType
	 , @ConnectedToShipment, @ConnectedToTicket, @ToLocation, @FromLocation,@DeliveryTo, @PickupFrom, @EstimatedPayablesInSales
	 , @EstimatedPayablesInLocal, @EstimatedReceivablesInLocal, @EstimatedReceivablesInSales, @EstimateProfit,  @LocalCurrency,  @MarkupPercentage,@ToPortCode, @ToPortName,@FromPortCode,@FromPortName,@ShipmentNumber,@CostChargeGroupVal


	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	 
	 BEGIN TRY  

	 	 ------------Resolve Custom Field Data Type Code-------------------
            
			    --@[ResolveCustomFieldDataTypeCodeVariable]

	 ----------------------------------------------------

	       declare @IsPotentialShipper as bit  set @IsPotentialShipper =0;if(@ShipperPartnerType = 'Potential Customer') begin set @IsPotentialShipper = 1 end 
		   declare @IsPotentialConsignee as bit  set @IsPotentialConsignee =0;if(@ConsigneePartnerType = 'Potential Customer') begin set @IsPotentialConsignee = 1 end 
		   declare @IsPotentialCustomer as bit  set @IsPotentialCustomer =0;if(@CustomerPartnerType = 'Potential Customer') begin set @IsPotentialCustomer = 1 end 
       
	   insert into #Fact_QuotesTemp ([Id],[Source Tenant],[Parent Tenant],[Direction],[Transport Mode],[Type] , [Department],[Branch],[Quote Number],
	   [Shipper],[Consignee],[Agent],[Customer],[Incoterms],[Opened by] ,[Open Date] , [Sent Date] ,[Accepted Date] ,[Declined Date] ,[Start Date] ,[Last Activity Date]
	   ,[Salesman] , [Stage] , [Notes] ,[Closing Reason] , [Estimated Profit in Local Currency] ,
	   [Sales Currency] , [Subject] , [Gross Weight In Kg] , [Chargeable Weight in Kg], [Volume in CBM] , [Number of Packages] , [Number of Containers] , 
	   [Expiration Date] , [Close Auto by System] , [Include Pickup] , [Include Delivery],
	   [Stage Due Date] , [Is Quote Data External] , [Is Quote Document External], [Close Date], 
	   [From Port Country] ,[To Port Country] ,[Is Potential Shipper], [Is Potential Consignee],[Is Potential Customer],

	    [Connected To Shipment], [Connected To Ticket],[To Location], [From Location],
	   [Delivery To], [Pickup From], [Estimated Payables in Sales Currency], [Estimated Payables in Local Currency],
	   [Estimated Receivables in Local Currency], [Estimated Receivables in Sales Currency], [Estimated Profit in Sales Currency], [Local Currency],[Spot Rates Markup] ,[To Port Code], [To Port Name],[From Port Code],[From Port Name],[Shipment No])
	   
	   
      values(@Id  , @SourceTenant, @ParentTenant ,@Direction , @TransportMode , @Type,@Department ,@Branch,@QuoteNumber
	 ,@Shipper ,@Consignee, @Agent ,@Customer , @Incoterm ,@CreatedByUser ,dbo.GetDateFormateAsNumber(@OpenDate)    , dbo.GetDateFormateAsNumber(@SentDate)  , dbo.GetDateFormateAsNumber(@AcceptedDate)  ,dbo.GetDateFormateAsNumber(@DeclinedDate)    ,dbo.GetDateFormateAsNumber(@StartDate)   ,dbo.GetDateFormateAsNumber(@LastActivityDate) 
	, @Salesman ,@QuoteStages, @Notes ,@QuoteClosingReasons , @EstimatedProfitInLocal 
	, @SaleCurrency ,@Subject , @GrossWeightInKG ,@ChargeableWeightInKG ,@VolumeInCBM ,@NumberOfPackages ,@NumberOfContainers
	 , dbo.GetDateFormateAsNumber(@ExpirationDate)  ,@IsAutomaticallyClosed ,@IncludePickUp ,@IncludeDelivery ,dbo.GetDateFormateAsNumber(@StageDueDate)   ,
	 @IsQuoteDataExternal ,@IsQuoteDocumentExternal,dbo.GetDateFormateAsNumber(@AutomaticallyCloseDate) 
	  , @FromCountry , @ToCountry ,@IsPotentialShipper ,  @IsPotentialConsignee , @IsPotentialCustomer,
	   @ConnectedToShipment, @ConnectedToTicket, @ToLocation, @FromLocation,@DeliveryTo, @PickupFrom, @EstimatedPayablesInSales
	 , @EstimatedPayablesInLocal, @EstimatedReceivablesInLocal, @EstimatedReceivablesInSales, @EstimateProfit,  @LocalCurrency, @MarkupPercentage ,@ToPortCode, @ToPortName,@FromPortCode,@FromPortName,@ShipmentNumber,@CostChargeGroupVal
	 )









	END TRY 
BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (QuoteId: ' + @Id +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

	
	FETCH NEXT FROM QuotesCursor    INTO      @Id ,@Tenant , @SourceTenant, @ParentTenant ,@Direction , @TransportMode , @Type,@Department ,@Branch,@QuoteNumber
	 ,@Shipper ,@Consignee, @Agent ,@Customer , @Incoterm ,@CreatedByUser ,@OpenDate , @SentDate ,@AcceptedDate ,@DeclinedDate ,@StartDate ,@LastActivityDate
	, @Salesman ,@QuoteStages, @Notes ,@QuoteClosingReasons , @EstimatedProfitInLocal 
	, @SaleCurrency ,@Subject , @GrossWeightInKG ,@ChargeableWeightInKG ,@VolumeInCBM ,@NumberOfPackages ,@NumberOfContainers
	 , @ExpirationDate ,@IsAutomaticallyClosed ,@IncludePickUp ,@IncludeDelivery ,@StageDueDate ,@IsQuoteDataExternal ,@IsQuoteDocumentExternal,@AutomaticallyCloseDate,@FromCountry , @ToCountry  , @ShipperPartnerType,@ConsigneePartnerType  , @CustomerPartnerType
	 ,@ConnectedToShipment, @ConnectedToTicket, @ToLocation, @FromLocation,@DeliveryTo, @PickupFrom, @EstimatedPayablesInSales
	 , @EstimatedPayablesInLocal, @EstimatedReceivablesInLocal, @EstimatedReceivablesInSales, @EstimateProfit, @LocalCurrency , @MarkupPercentage,@ToPortCode, @ToPortName,@FromPortCode,@FromPortName,@ShipmentNumber,@CostChargeGroupVal
		End
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor


	