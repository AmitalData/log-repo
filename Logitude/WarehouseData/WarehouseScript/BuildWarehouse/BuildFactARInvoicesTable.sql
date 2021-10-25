

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
	 
	declare @Branch as int 
	declare @StatusCode as varchar(2)
	declare @DraftNumber as varchar(20)  
    declare @IsConsolidationInvoice  as bit 

	declare @ARInvoiceEntityId as varchar(15)
	declare @ARInvoiceLineEntityId as varchar(15)




	DECLARE ARInvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_ARInvoices.Id,dw_ARInvoices.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number], dw_ARInvoiceTypes.Name,dw_ARInvoices.InvoiceNumber,
	dw_ARInvoices.InvoiceDate, dw_ARInvoices.CreateDate, dw_ARInvoices.ApprovedDate, dw_ARInvoices.DueDate, dw_ARInvoices.PrintDate, dw_ARInvoices.PaidDate,
	ApprovedByUser.Id_Number, CreatedByUser.Id_Number, PrintedByUser.Id_Number, SalesmanUser.Id_Number, dw_ARInvoiceStatus.Name, dw_ARInvoices.PrintNotes,NewDIM_PaymentTerms.Id_Number,
	LocalCurrency.Id_Number, InvoiceCurrency.Id_Number, dw_ARInvoices.VatNumber, BillTo.Id_Number,dw_ARInvoices.SubTotalInLocalCurrency, dw_ARInvoices.SubTotalInInvoiceCurrency,
	dw_ARInvoices.AmountInLocalCurrency, dw_ARInvoices.AmountInProfitCurrency, dw_ARInvoices.AmountDueInLocalCurrency, dw_ARInvoices.AmountDueInProfitCurrency,
	dw_ARInvoiceLines.Description, dw_ARInvoiceLines.LocalDescription, dw_ARInvoiceLines.UnitPrice, dw_ARInvoiceLines.Quantity, NewDIM_VatTypes.Id_Number, 
	dw_ARInvoiceLines.VatPercentage, dw_ARInvoiceLines.LocalCurrencyAmount,dw_ARInvoiceLines.ForiegnCurrencyAmount,dw_ARInvoiceLines.InvoiceCurrencyAmount, ForiegnCurrency.Id_Number, dw_ARInvoiceLines.ForiegnExchangeRate,
	dw_ARInvoiceLines.ProfitCurrencyAmount,dw_ARInvoiceLines.Notes,dw_ARInvoices.InvoiceCurrencyExchangeRate,dw_ARInvoiceLines.IsExpense,dw_ARInvoiceLines.IsRegionalTax,
	NewDIM_Branches.Id_Number,dw_ARInvoices.StatusCode, dw_ARInvoices.DraftNumber,
	dw_ARInvoices.IsConsolidationInvoice, dw_ARInvoices.MainEntityId,dw_ARInvoiceLines.EntityId

	 
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
	inner JOIN NewDIM_Branches ON dw_ARInvoices.BranchId =NewDIM_Branches.Id
	inner JOIN dw_ARInvoiceLines  ON dw_ARInvoices.Id = dw_ARInvoiceLines.ARInvoiceId
	 
    inner JOIN NewDIM_Currencies ForiegnCurrency ON dw_ARInvoiceLines.ForiegnCurrencyId = ForiegnCurrency.Id
	-- join shipment


   inner JOIN NewDIM_VatTypes ON dw_ARInvoiceLines.VatTypeId = NewDIM_VatTypes.Id
	 

	OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor   into  @Id ,@Tenant , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber,
	 @InvoiceDate, @CreateDate, @ApprovedDate, @DueDate, @PrintDate, @PaidDate, @ApprovedBy, @CreatedBy, @PrintedBy,
	 @ARInvoicesSalesman, @Status, @PrintNotes, @PaymentTerm, @LocalCurrency, @InvoiceCurrency, @VATNumber, @BillTo, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
	 @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @Description, @LocalDescription, @UnitPrice, @Quantity, @VatType,
	 @VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax,
	 @Branch,@StatusCode, @DraftNumber, @IsConsolidationInvoice, @ARInvoiceEntityId,@ARInvoiceLineEntityId
	 
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 
	 BEGIN TRY  
	  
	 ----------------------------------------------------
 	declare @MainEntityId as varchar(15) = @ARInvoiceEntityId;
	if(@ARInvoiceLineEntityId is not null) set @MainEntityId = @ARInvoiceLineEntityId;


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
		 
		declare @IsCancelled as bit  
		set @IsCancelled =0;
		if(@StatusCode = 'AR'or @StatusCode = 'AC' or @StatusCode = 'LL' or @StatusCode = 'VD') 
		begin set @IsCancelled = 1 end  

		 ----------------------------------------------
	 

	   insert into #Fact_ARInvoicesTemp ([Id],[Source Tenant],[Parent Tenant], [AR Invoice Type], [Invoice Number], [Invoice Date], [Create Date], [Approved Date], [Due Date], [Print Date],[Paid Date], 
	   [Approved By], [Created By], [Printed By], [Invoice Salesman],  [Invoice Status], [Print Note], [Payment Term], [Invoice Local Currency], [Invoice Currency], [VAT Number], [Bill To], 
	   
	   [Subtotal (Local)],[Subtotal (Profit)],[Invoice Amount (Local)],[Invoice Amount (Profit)],[Amount Due (Local)],[Amount Due (Profit)],
	  [Line Description],[Line Local Description], [Line Unit Price], [Line Quantity], [Line VAT Type], 
	   [Line VAT Percentage],[Line Amount (Local)], [Line Amount (Foreign)],[Line Amount (Invoice Currency)], [Foreign Currency], [Foreign Exchange Rate],[Line Amount (Profit)],
       [Line Notes], [Invoice Currency Exchange Rate], [Is Expense], [Is Regional Tax],[Invoice Branch],[Is Cancelled],[Main Entity Id] )
	   
	   
      values(@Id  , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber, dbo.GetDateFormateAsNumber(@InvoiceDate), dbo.GetDateFormateAsNumber(@CreateDate),dbo.GetDateFormateAsNumber(@ApprovedDate),  dbo.GetDateFormateAsNumber(@DueDate) ,dbo.GetDateFormateAsNumber(@PrintDate),dbo.GetDateFormateAsNumber(@PaidDate),
	  @ApprovedBy, @CreatedBy, @PrintedBy, @ARInvoicesSalesman, @Status, @PrintNotes,  @PaymentTerm, @LocalCurrency, @InvoiceCurrency,@VATNumber, @BillTo,
	 
	 @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency, @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @Description, @LocalDescription, @UnitPrice, @Quantity,  @VatType,
	 @VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax,
	 @Branch,@IsCancelled,@MainEntityId)



	   

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
	@ARInvoicesSalesman,  @Status, @PrintNotes, @PaymentTerm, @LocalCurrency, @InvoiceCurrency, @VATNumber, @BillTo,@SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
	@AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @Description, @LocalDescription, @UnitPrice , @Quantity,  @VatType,
	@VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax,
	@Branch, @StatusCode, @DraftNumber, @IsConsolidationInvoice, @ARInvoiceEntityId,@ARInvoiceLineEntityId

		End
	CLOSE ARInvoicesCursor
	DEALLOCATE ARInvoicesCursor


	