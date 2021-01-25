

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

   declare @DeliveryAddress as nvarchar(250)
   declare @PickUpAddress as nvarchar(250)
      
   declare @FromCountry as int
   declare @ToCountry as int
	   declare @ShipperPartnerType  as varchar(20)
		   declare @ConsigneePartnerType  as varchar(20)
		   	   declare @CustomerPartnerType  as varchar(20)

  

	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Quotes.Id,dw_Quotes.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number] ,  NewDIM_Directions.Name, TransportModes.Name , 
	NewDIM_Types.Name, NewDIM_Departments.Id_Number ,NewDIM_Branches.Id_Number , dw_Quotes.QuoteNumber,
	shipperPartners.Id_Number, consigneePartners.Id_Number,agentPartners.Id_Number,customerPartners.Id_Number , NewDIM_Incoterms.Id_Number ,
	createdByUser.Id_Number , dw_Quotes.OpenDate , dw_Quotes.SentDate, dw_Quotes.AcceptedDate, dw_Quotes.DeclinedDate , dw_Quotes.StartDate, dw_Quotes.LastActivityDate,
	SalesmanUser.Id_Number, NewDIM_QuoteStages.Id_Number, dw_Quotes.Notes , NewDIM_QuoteClosingReasons.Id_Number,dw_Quotes.EstimatedProfitInLocal,
	SaleCurrency.Id_Number,dw_Quotes.Subject, dw_Quotes.GrossWeightInKG ,dw_Quotes.ChargeableWeightInKG,dw_Quotes.VolumeInCBM,dw_Quotes.NumberOfPackages , 
	dw_Quotes.NumberOfContainers,dw_Quotes.ExpirationDate,dw_Quotes.IsAutomaticallyClosed, dw_Quotes.IncludePickUp , 
	dw_Quotes.IncludeDelivery , dw_Quotes.StageDueDate , dw_Quotes.IsQuoteDataExternal, dw_Quotes.IsQuoteDocumentExternal,
	dw_Quotes.DeliveryAddress,dw_Quotes.PickUpAddress,dw_Quotes.AutomaticallyCloseDate   , FromCountry.Id_Number , ToCountry.Id_Number , shipperPartners.[Partner Type]  , consigneePartners.[Partner Type],customerPartners.[Partner Type]









    From dw_Quotes
	inner JOIN NewDIM_Tenants SourceTenant ON dw_Quotes.Tenant = SourceTenant.[Tenant Number]
	inner JOIN dw_DWHSettings ON dw_Quotes.Tenant = dw_DWHSettings.Tenant
	inner JOIN NewDIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
	inner JOIN NewDIM_Directions ON dw_Quotes.DirectionId = NewDIM_Directions.Code
    inner JOIN NewDIM_TransportModes TransportModes ON dw_Quotes.TransportModeId = TransportModes.Code
	inner JOIN NewDIM_Types ON dw_Quotes.ShipmentTypeId = NewDIM_Types.Code
	inner JOIN NewDIM_Departments ON dw_Quotes.DepartmentId = NewDIM_Departments.Id
	inner JOIN NewDIM_Branches ON dw_Quotes.BranchId =NewDIM_Branches.Id
	inner JOIN NewDIM_Partners shipperPartners ON dw_Quotes.ShipperId = shipperPartners.Id
	inner JOIN NewDIM_Partners consigneePartners ON dw_Quotes.ConsigneeId = consigneePartners.Id
	inner JOIN NewDIM_Partners agentPartners ON dw_Quotes.AgentId = agentPartners.Id
	inner JOIN NewDIM_Partners customerPartners ON dw_Quotes.CustomerId = customerPartners.Id
	inner JOIN NewDIM_Incoterms  ON dw_Quotes.IncotermId = NewDIM_Incoterms.Id
	inner JOIN NewDIM_Users SalesmanUser ON dw_Quotes.SalesmanUserId = SalesmanUser.Id
	inner JOIN NewDIM_QuoteStages  ON dw_Quotes.StageId = NewDIM_QuoteStages.Id
	inner JOIN NewDIM_QuoteClosingReasons  ON dw_Quotes.QuoteClosingReasonId = NewDIM_QuoteClosingReasons.Id
	inner JOIN NewDIM_Currencies SaleCurrency ON dw_Quotes.SaleCurrencyId = SaleCurrency.Id
	inner JOIN NewDIM_Users createdByUser ON dw_Quotes.CreatedByUserId = createdByUser.Id
	inner JOIN dw_Ports fromPort ON dw_Quotes.FromPortId = fromPort.Id
	inner JOIN NewDIM_Countries FromCountry ON fromPort.CountryId = FromCountry.Id
	inner JOIN dw_Ports toPort ON dw_Quotes.ToPortId = toPort.Id
	inner JOIN NewDIM_Countries ToCountry ON toPort.CountryId = ToCountry.Id



	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor   into  @Id ,@Tenant , @SourceTenant, @ParentTenant ,@Direction , @TransportMode , @Type,@Department ,@Branch,@QuoteNumber
	 ,@Shipper ,@Consignee, @Agent ,@Customer , @Incoterm ,@CreatedByUser ,@OpenDate , @SentDate ,@AcceptedDate ,@DeclinedDate ,@StartDate ,@LastActivityDate
	, @Salesman ,@QuoteStages, @Notes ,@QuoteClosingReasons , @EstimatedProfitInLocal 
	, @SaleCurrency ,@Subject , @GrossWeightInKG ,@ChargeableWeightInKG ,@VolumeInCBM ,@NumberOfPackages ,@NumberOfContainers
	 , @ExpirationDate ,@IsAutomaticallyClosed ,@IncludePickUp ,@IncludeDelivery ,@StageDueDate ,@IsQuoteDataExternal ,@IsQuoteDocumentExternal,@DeliveryAddress ,@PickUpAddress,@AutomaticallyCloseDate , @FromCountry, @ToCountry  , @ShipperPartnerType ,@ConsigneePartnerType ,  @CustomerPartnerType
		 


	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	 
	 BEGIN TRY  

	 

	       declare @IsPotentialShipper as bit  set @IsPotentialShipper =0;if(@ShipperPartnerType = 'Potential Customer') begin set @IsPotentialShipper = 1 end 
		   declare @IsPotentialConsignee as bit  set @IsPotentialConsignee =0;if(@ConsigneePartnerType = 'Potential Customer') begin set @IsPotentialConsignee = 1 end 
		   declare @IsPotentialCustomer as bit  set @IsPotentialCustomer =0;if(@CustomerPartnerType = 'Potential Customer') begin set @IsPotentialCustomer = 1 end 
       
	   insert into #Fact_QuotesTemp ([Id],[Source Tenant],[Parent Tenant],[Direction],[Transport Mode],[Type] , [Department],[Branch],[Quote Number],
	   [Shipper],[Consignee],[Agent],[Customer],[Incoterms],[Opened by] ,[Open Date] , [Sent Date] ,[Accepted Date] ,[Declined Date] ,[Start Date] ,[Last Activity Date]
	   ,[Salesman] , [Stage] , [Notes] ,[Closing Reason] , [Estimated Profit in Local Currency] ,
	   [Sales Currency] , [Subject] , [Gross Weight In Kg] , [Chargeable Weight in Kg], [Volume in CBM] , [Number of Packages] , [Number of Containers] , 
	   [Expiration Date] , [Close Auto by System] , [Include Pickup] , [Include Delivery],
	   [Stage Due Date] , [Is Quote Data External] , [Is Quote Document External] , [Delivery To] ,[Pickup From] , [Close Date], [From Port Country] ,[To Port Country] ,[Is Potential Shipper], [Is Potential Consignee],[Is Potential Customer],

	   [Local Currency] )
	   
	   
      values(@Id  , @SourceTenant, @ParentTenant ,@Direction , @TransportMode , @Type,@Department ,@Branch,@QuoteNumber
	 ,@Shipper ,@Consignee, @Agent ,@Customer , @Incoterm ,@CreatedByUser ,dbo.GetDateFormateAsNumber(@OpenDate)    , dbo.GetDateFormateAsNumber(@SentDate)  , dbo.GetDateFormateAsNumber(@AcceptedDate)  ,dbo.GetDateFormateAsNumber(@DeclinedDate)    ,dbo.GetDateFormateAsNumber(@StartDate)   ,dbo.GetDateFormateAsNumber(@LastActivityDate) 
	, @Salesman ,@QuoteStages, @Notes ,@QuoteClosingReasons , @EstimatedProfitInLocal 
	, @SaleCurrency ,@Subject , @GrossWeightInKG ,@ChargeableWeightInKG ,@VolumeInCBM ,@NumberOfPackages ,@NumberOfContainers
	 , dbo.GetDateFormateAsNumber(@ExpirationDate)  ,@IsAutomaticallyClosed ,@IncludePickUp ,@IncludeDelivery ,dbo.GetDateFormateAsNumber(@StageDueDate)   ,@IsQuoteDataExternal ,@IsQuoteDocumentExternal,@DeliveryAddress ,@PickUpAddress,dbo.GetDateFormateAsNumber(@AutomaticallyCloseDate) 
	  , @FromCountry , @ToCountry ,@IsPotentialShipper ,  @IsPotentialConsignee,@IsPotentialCustomer,
	  1
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
	 , @ExpirationDate ,@IsAutomaticallyClosed ,@IncludePickUp ,@IncludeDelivery ,@StageDueDate ,@IsQuoteDataExternal ,@IsQuoteDocumentExternal,@DeliveryAddress ,@PickUpAddress,@AutomaticallyCloseDate,@ToCountry ,@FromCountry , @ShipperPartnerType,@ConsigneePartnerType ,  @CustomerPartnerType
		 

		End
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor


	