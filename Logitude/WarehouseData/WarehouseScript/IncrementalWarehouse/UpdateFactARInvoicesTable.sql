 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ARInvoice' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoices )

 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

 
    declare @Id as varchar(15)
	declare @Tenant as int
    declare @SourceTenant as int
    declare @ParentTenant as int
    declare @InvoiceType as varchar(20)
    declare @InvoiceNumber as varchar(25)
    declare @InvoiceDate as datetime
    declare @CreateDate as datetime
    declare @ApprovedDate as datetime
    declare @DueDate as datetime
    declare @PrintDate as datetime
    declare @PaidDate as datetime 
    declare @ApprovedBy as int
    declare @CreatedBy as int
    declare @PrintedBy as int
	declare @ARInvoicesSalesman as int
	declare @ShipmentSalesman as int

	declare @Status as varchar(20)
	declare @PrintNotes as varchar(250)
    declare @PaymentTerm as int
	declare @LocalCurrency as int
	declare @InvoiceCurrency as int
	declare @VATNumber as varchar(30)
	declare @BillTo as int
	declare @SubTotalInLocalCurrency as float
	declare @SubTotalInInvoiceCurrency as float
	declare @AmountInLocalCurrency as float
	declare @AmountInProfitCurrency as float
	declare @AmountDueInLocalCurrency as float
	declare @AmountDueInProfitCurrency as float
	declare @ShipmentsNumbers as varchar(1000) 

	declare @Description as nvarchar(250) 
	declare @LocalDescription as nvarchar(250) 
	declare @UnitPrice as float
	declare @Quantity as float 
	declare @VatType as int
	
	declare @VatPercentage  as float 
    declare @LocalCurrencyAmount  as float 
	declare @ForiegnCurrencyAmount  as float 
	declare @InvoiceCurrencyAmount  as float  
	declare @ForiegnCurrencyId as int
	declare @ForiegnExchangeRate  as float 
	declare @ProfitCurrencyAmount  as float  
	declare @Notes as nvarchar(500) 
	declare @InvoiceCurrencyExchangeRate  as float 
 	declare @IsExpens as bit
	declare @IsRegionalTax as bit
	declare @House as varchar(20)
	declare @MasterShipmentNumber as varchar(20)

    declare @Direction as varchar(40)
    declare @TransportMode as varchar(13)
	declare @Type as varchar(40)
	declare @ShipmentSubTypeId as varchar(15)
	declare @Department as int
	declare @Shipper as int
    declare @Consignee as int
	declare @Routing as varchar(100)
	declare @Branch as int 
	declare @StatusCode as varchar(2)
	declare @DraftNumber as varchar(20)

	DECLARE ARInvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_ARInvoices.Id,dw_ARInvoices.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number], dw_ARInvoiceTypes.Name,dw_ARInvoices.InvoiceNumber,
	dw_ARInvoices.InvoiceDate, dw_ARInvoices.CreateDate, dw_ARInvoices.ApprovedDate, dw_ARInvoices.DueDate, dw_ARInvoices.PrintDate, dw_ARInvoices.PaidDate,
	ApprovedByUser.Id_Number, CreatedByUser.Id_Number, PrintedByUser.Id_Number, SalesmanUser.Id_Number, ShipmentSalesmanUser.Id_Number , dw_ARInvoiceStatus.Name, dw_ARInvoices.PrintNotes,DIM_PaymentTerms.Id_Number,
	LocalCurrency.Id_Number, InvoiceCurrency.Id_Number, dw_ARInvoices.VatNumber, BillTo.Id_Number,dw_ARInvoices.SubTotalInLocalCurrency, dw_ARInvoices.SubTotalInInvoiceCurrency,
	dw_ARInvoices.AmountInLocalCurrency, dw_ARInvoices.AmountInProfitCurrency, dw_ARInvoices.AmountDueInLocalCurrency, dw_ARInvoices.AmountDueInProfitCurrency,dw_Shipments.ShipmentNumber,
	dw_ARInvoiceLines.Description, dw_ARInvoiceLines.LocalDescription, dw_ARInvoiceLines.UnitPrice, dw_ARInvoiceLines.Quantity, DIM_VatTypes.Id_Number, 
	dw_ARInvoiceLines.VatPercentage, dw_ARInvoiceLines.LocalCurrencyAmount,dw_ARInvoiceLines.ForiegnCurrencyAmount,dw_ARInvoiceLines.InvoiceCurrencyAmount, ForiegnCurrency.Id_Number, dw_ARInvoiceLines.ForiegnExchangeRate,
	dw_ARInvoiceLines.ProfitCurrencyAmount,dw_ARInvoiceLines.Notes,dw_ARInvoices.InvoiceCurrencyExchangeRate,dw_ARInvoiceLines.IsExpense,dw_ARInvoiceLines.IsRegionalTax, dw_Shipments.House, dw_ShipmentMasterDatas.MasterShipmentNumber,
	DIM_Directions.Name,TransportModes.Name, DIM_Types.Name, dw_Shipments.ShipmentSubTypeId, DIM_Departments.Id_Number, shipperPartners.Id_Number, consigneePartners.Id_Number, dw_Shipments.Routing, DIM_Branches.Id_Number,dw_ARInvoices.StatusCode, dw_ARInvoices.DraftNumber

	 
    From dw_ARInvoices
	inner JOIN DIM_Tenants SourceTenant ON dw_ARInvoices .Tenant = SourceTenant.[Tenant Number]
	inner JOIN dw_DWHSettings ON dw_ARInvoices.Tenant = dw_DWHSettings.Tenant
	inner JOIN DIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
	inner JOIN dw_ARInvoiceTypes ON dw_ARInvoices.ARInvoiceTypeCode = dw_ARInvoiceTypes.Code 
	inner JOIN DIM_Users ApprovedByUser ON dw_ARInvoices.ApprovedByUserId = ApprovedByUser.Id
	inner JOIN DIM_Users CreatedByUser ON dw_ARInvoices.CreatedByUserId = CreatedByUser.Id
	inner JOIN DIM_Users PrintedByUser ON dw_ARInvoices.PrintByUserId = PrintedByUser.Id
	inner JOIN DIM_Users SalesmanUser ON dw_ARInvoices.SalesmanUserId = SalesmanUser.Id
	inner JOIN dw_ARInvoiceStatus ON dw_ARInvoices.StatusCode = dw_ARInvoiceStatus.Code 
	inner JOIN DIM_PaymentTerms ON dw_ARInvoices.PaymentTermId = DIM_PaymentTerms.Id 
	inner JOIN DIM_Currencies LocalCurrency ON dw_ARInvoices.LocalCurrencyId = LocalCurrency.Id
	inner JOIN DIM_Currencies InvoiceCurrency ON dw_ARInvoices.InvoiceCurrencyId = InvoiceCurrency.Id
	inner JOIN DIM_Partners BillTo ON dw_ARInvoices.BillToId = BillTo.Id 
	inner JOIN DIM_Branches ON dw_ARInvoices.BranchId =DIM_Branches.Id
	inner JOIN dw_ARInvoiceLines  ON dw_ARInvoices.Id = dw_ARInvoiceLines.ARInvoiceId
	 
    inner JOIN DIM_Currencies ForiegnCurrency ON dw_ARInvoiceLines.ForiegnCurrencyId = ForiegnCurrency.Id
	-- join shipment
	
	inner JOIN dw_Shipments  ON dw_ARInvoices.MainEntityId = dw_Shipments.Id
	inner JOIN dw_ShipmentMasterDatas ON dw_Shipments.MasterShipmentDataId = dw_ShipmentMasterDatas.Id

	inner JOIN DIM_Directions ON dw_Shipments.DirectionId = DIM_Directions.Code
    inner JOIN DIM_TransportModes TransportModes ON dw_Shipments.TransportModeId = TransportModes.Code
	inner JOIN DIM_Types ON dw_Shipments.ShipmentTypeId = DIM_Types.Code
	inner JOIN DIM_Departments ON dw_Shipments.DepartmentId = DIM_Departments.Id
	inner JOIN DIM_Partners shipperPartners ON dw_Shipments.ShipperId = shipperPartners.Id
	inner JOIN DIM_Partners consigneePartners ON dw_Shipments.ConsigneeId = consigneePartners.Id 
	inner JOIN DIM_Users ShipmentSalesmanUser ON dw_Shipments.SalesmanUserId = ShipmentSalesmanUser.Id

   inner JOIN DIM_VatTypes ON dw_ARInvoiceLines.VatTypeId = DIM_VatTypes.Id
	 

	OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor   into  @Id ,@Tenant , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber,
	 @InvoiceDate, @CreateDate, @ApprovedDate, @DueDate, @PrintDate, @PaidDate, @ApprovedBy, @CreatedBy, @PrintedBy,
	 @ARInvoicesSalesman, @ShipmentSalesman, @Status, @PrintNotes, @PaymentTerm, @LocalCurrency, @InvoiceCurrency, @VATNumber, @BillTo, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
	 @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency,  @ShipmentsNumbers, @Description, @LocalDescription, @UnitPrice, @Quantity, @VatType,
	 @VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax, @House, @MasterShipmentNumber,
	 @Direction,@TransportMode, @Type, @ShipmentSubTypeId, @Department, @Shipper , @Consignee, @Routing, @Branch, @StatusCode, @DraftNumber
	 
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 
	 BEGIN TRY  
	  
	 ----------------------------------------------------
	  

		declare @Salesman as int
		set @Salesman = @ARInvoicesSalesman;
		if(@Salesman =1 )BEGIN set @Salesman = @ShipmentSalesman;END

   	--------------AR Invoice Number------------------
		 
				IF(@StatusCode = 'DR' or @StatusCode = 'LL')
					BEGIN
						IF(@DraftNumber is not null)
							BEGIN
								SET @InvoiceNumber = @DraftNumber;
							END
						ELSE
							BEGIN
								SET @InvoiceNumber = @Id;
							END
					END
		 
		----------------------------------------------



	   insert into Fact_ARInvoices ([Id],[Source Tenant],[Parent Tenant], [AR Invoice Type], [Invoice Number], [Invoice Date], [Create Date], [Approved Date], [Due Date], [Print Date],[Paid Date], 
	   [Approved By], [Created By], [Printed By], [Invoice Salesman],  [Invoice Status], [Print Note], [Payment Term], [Invoice Local Currency], [Invoice Currency], [VAT Number], [Bill To], 
	   
	   [Subtotal (Local)],[Subtotal (Profit)],[Invoice Amount (Local)],[Invoice Amount (Profit)],[Amount Due (Local)],[Amount Due (Profit)],
	   [Shipment Number], [Line Description],[Line Local Description], [Line Unit Price], [Line Quantity], [Line VAT Type], 
	   [Line VAT Percentage],[Line Amount (Local)], [Line Amount (Foreign)],[Line Amount (Invoice Currency)], [Foreign Currency], [Foreign Exchange Rate],[Line Amount (Profit)],
       [Line Notes], [Invoice Currency Exchange Rate], [Is Expense], [Is Regional Tax], [Shipment House Number], [Shipment Master Number], [Shipment Direction],[Shipment Transport Mode], [Shipment Type],[Shipment Sub Type],[Department],[Shipper], [Consignee],[Routing],[Invoice Branch] )
	   
	   
      values(@Id  , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber,dbo.GetDateFormateAsNumber(@InvoiceDate), dbo.GetDateFormateAsNumber(@CreateDate),dbo.GetDateFormateAsNumber(@ApprovedDate),  dbo.GetDateFormateAsNumber(@DueDate) ,dbo.GetDateFormateAsNumber(@PrintDate),dbo.GetDateFormateAsNumber(@PaidDate),
	  @ApprovedBy, @CreatedBy, @PrintedBy, @Salesman, @Status, @PrintNotes,  @PaymentTerm, @LocalCurrency, @InvoiceCurrency,@VATNumber, @BillTo,
	 
	 @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency, @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency,  @ShipmentsNumbers, @Description, @LocalDescription, @UnitPrice, @Quantity,  @VatType,
	  @VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax, @House, @MasterShipmentNumber, @Direction,@TransportMode,
	  @Type, @ShipmentSubTypeId, @Department, @Shipper , @Consignee, @Routing, @Branch)



	   

	END TRY 
BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (ARInvoiceId: ' + @Id +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

	
	FETCH NEXT FROM ARInvoicesCursor    INTO  @Id ,@Tenant , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber,
	@InvoiceDate, @CreateDate, @ApprovedDate, @DueDate, @PrintDate,@PaidDate, @ApprovedBy, @CreatedBy, @PrintedBy,
	@ARInvoicesSalesman, @ShipmentSalesman,  @Status, @PrintNotes, @PaymentTerm, @LocalCurrency, @InvoiceCurrency, @VATNumber, @BillTo,@SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
	@AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency,  @ShipmentsNumbers, @Description, @LocalDescription, @UnitPrice , @Quantity,  @VatType,
	@VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax, @House, @MasterShipmentNumber, @Direction,@TransportMode,
	@Type, @ShipmentSubTypeId, @Department, @Shipper , @Consignee, @Routing, @Branch, @StatusCode, @DraftNumber

		End
	CLOSE ARInvoicesCursor
	DEALLOCATE ARInvoicesCursor
	 

	end