

    declare @Id as varchar(15)
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
    declare @FirstApproveDate as datetime
    declare @ApprovedBy as int
    declare @CreatedBy as int
    declare @PrintedBy as int
	declare @Salesman as int
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
	declare @VatType as varchar(15) 
	
	declare @VatPercentage  as float 
    declare @LocalCurrencyAmount  as float 
	declare @ForiegnCurrencyAmount  as float 
	declare @InvoiceCurrencyAmount  as float  
	declare @ForiegnCurrencyId as int
	declare @ForiegnExchangeRate  as float 
	declare @ProfitCurrencyAmount  as float  
	declare @Notes as nvarchar(500) 
	declare @InvoiceCurrencyExchangeRate  as float 
 	declare @IsExpense as bit
	declare @IsRegionalTax as bit


	DECLARE ARInvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_ARInvoices.Id,dw_ARInvoices.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number], dw_ARInvoiceTypes.Name,dw_ARInvoices.InvoiceNumber,
	dw_ARInvoices.InvoiceDate, dw_ARInvoices.CreateDate, dw_ARInvoices.ApprovedDate, dw_ARInvoices.DueDate, dw_ARInvoices.PrintDate, dw_ARInvoices.PaidDate,dw_ARInvoices.FirstApproveDate,
	ApprovedByUser.Id_Number, CreatedByUser.Id_Number, PrintedByUser.Id_Number, SalesmanUser.Id_Number, dw_ARInvoiceStatus.Name, dw_ARInvoices.PrintNotes,NewDIM_PaymentTerms.Id_Number,
	LocalCurrency.Id_Number, InvoiceCurrency.Id_Number, dw_ARInvoices.VatNumber, BillTo.Id_Number,dw_ARInvoices.SubTotalInLocalCurrency, dw_ARInvoices.SubTotalInInvoiceCurrency,
	dw_ARInvoices.AmountInLocalCurrency, dw_ARInvoices.AmountInProfitCurrency, dw_ARInvoices.AmountDueInLocalCurrency, dw_ARInvoices.AmountDueInProfitCurrency,dw_ARInvoices.ShipmentsNumbers,
	dw_ARInvoiceLines.Description, dw_ARInvoiceLines.LocalDescription, dw_ARInvoiceLines.UnitPrice, dw_ARInvoiceLines.Quantity, dw_VatTypes.Name

	 
    From dw_ARInvoices
	inner JOIN NewDIM_Tenants SourceTenant ON dw_ARInvoices .Tenant = SourceTenant.[Tenant Number]
	inner JOIN dw_DWHSettings ON dw_ARInvoices.Tenant = dw_DWHSettings.Tenant
	inner JOIN NewDIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
	inner JOIN dw_ARInvoiceTypes ON dw_ARInvoices.ARInvoiceTypeCode = dw_ARInvoiceTypes.Code 
	inner JOIN NewDIM_Users ApprovedByUser ON dw_ARInvoices.ApprovedByUserId = ApprovedByUser.Id
	inner JOIN NewDIM_Users CreatedByUser ON dw_ARInvoices.CreatedByUserId = CreatedByUser.Id
	inner JOIN NewDIM_Users PrintedByUser ON dw_ARInvoices.PrintByUserId = PrintedByUser.Id
	inner JOIN NewDIM_Users SalesmanUser ON dw_ARInvoices.SalesmanUserId = SalesmanUser.Id
	inner JOIN dw_ARInvoiceStatus ON dw_ARInvoices.StatusCode = dw_ARInvoiceStatus.Code 
	inner JOIN NewDIM_PaymentTerms ON dw_ARInvoices.PaymentTermId = NewDIM_PaymentTerms.Id 
	inner JOIN NewDIM_Currencies LocalCurrency ON dw_ARInvoices.LocalCurrencyId = LocalCurrency.Id
	inner JOIN NewDIM_Currencies InvoiceCurrency ON dw_ARInvoices.InvoiceCurrencyId = InvoiceCurrency.Id
	inner JOIN NewDIM_Partners BillTo ON dw_ARInvoices.BillToId = BillTo.Id 
	inner JOIN dw_APInvoices  ON dw_APInvoiceLines.APInvoiceId = dw_APInvoices.Id
	inner JOIN dw_VatTypes ON  dw_APInvoiceLines.VatTypeId = dw_APInvoiceLines.Id 

	OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor   into  @Id ,@Tenant , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber,
	 @InvoiceDate, @CreateDate, @ApprovedDate, @DueDate, @PrintDate, @PaidDate, @FirstApproveDate,@ApprovedBy, @CreatedBy, @PrintedBy,
	 @Salesman, @Status, @PrintNotes, @PaymentTerm, @LocalCurrency, @InvoiceCurrency, @VATNumber, @BillTo, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
	 @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency,  @ShipmentsNumbers, @Description, @LocalDescription, @UnitPrice, @Quantity,@VatType
	 
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 
	 BEGIN TRY  
	  
	 ----------------------------------------------------
	  
	   insert into #Fact_ARInvoicesTemp ([Id],[Source Tenant],[Parent Tenant], [Invoice Type], [Invoice Number], [Invoice Date], [Create Date], [Approved Date], [Due Date], [Print Date],[Paid Date], [First Approve Date],
	   [Approved By], [Created By], [Printed By], [Invoice Salesman],  [Invoice Status], [Print Note], [Payment Term], [Invoice Local Currency], [Invoice Currency], [VAT Number], [Bill To],  [Shipment Number],
	   [Line Description],[Line Local Description], [Unit Price], [Quantity], [Line VAT Type], [Line VAT Percentage])
	   
	   
      values(@Id  , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber, @InvoiceDate, @CreateDate, @ApprovedDate, @DueDate, @PrintDate,@PaidDate, @FirstApproveDate,
	  @ApprovedBy, @CreatedBy, @PrintedBy, @Salesman, @Status, @PrintNotes,  @PaymentTerm, @LocalCurrency, @InvoiceCurrency,@VATNumber, @BillTo,
	  @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency, @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency,  @ShipmentsNumbers, @Description, @LocalDescription, @UnitPrice, Quantity, @VatType )



	   

	END TRY 
BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (ARInvoiceId: ' + @Id +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

	
	FETCH NEXT FROM ARInvoicesCursor    INTO  @Id ,@Tenant , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber,
	@InvoiceDate, @CreateDate, @ApprovedDate, @DueDate, @PrintDate,@PaidDate, @FirstApproveDate, @ApprovedBy, @CreatedBy, @PrintedBy,
	@Salesman,  @Status, @PrintNotes, @PaymentTerm, @LocalCurrency, @InvoiceCurrency, @@VATNumber, @BillTo,@SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
	@AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency,  @ShipmentsNumbers, @Description, @LocalDescription, @UnitPrice , @Quantity, @VatType

		End
	CLOSE ARInvoicesCursor
	DEALLOCATE ARInvoicesCursor


	