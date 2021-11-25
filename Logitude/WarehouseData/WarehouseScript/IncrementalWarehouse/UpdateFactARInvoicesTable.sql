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

	declare @OriginalInvoiceNumber as varchar(25)
	declare @InvoiceMasterNumber as nvarchar(30)
	declare @InvoiceHouseNumber as nvarchar(20)

	 --@[DeclareCustomFieldsVariable]
	declare @ConsolidationInvoiceNumber as varchar(25)
    declare @ConsolidationInvoiceDate as datetime	
	declare @ConsolidationStatusCode as varchar(2)
	declare @ConsolidationDraftNumber as varchar(20)
	declare @Partner as int
	declare @ConsolidationId as varchar(15)

	DECLARE ARInvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_ARInvoices.Id,dw_ARInvoices.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number], dw_ARInvoiceTypes.Name,dw_ARInvoices.InvoiceNumber,
	dw_ARInvoices.InvoiceDate, dw_ARInvoices.CreateDate, dw_ARInvoices.ApprovedDate, dw_ARInvoices.DueDate, dw_ARInvoices.PrintDate, dw_ARInvoices.PaidDate,
	ApprovedByUser.Id_Number, CreatedByUser.Id_Number, PrintedByUser.Id_Number, SalesmanUser.Id_Number, dw_ARInvoiceStatus.Name, dw_ARInvoices.PrintNotes,DIM_PaymentTerms.Id_Number,
	LocalCurrency.Id_Number, InvoiceCurrency.Id_Number, dw_ARInvoices.VatNumber, BillTo.Id_Number,dw_ARInvoices.SubTotalInLocalCurrency, dw_ARInvoices.SubTotalInInvoiceCurrency,
	dw_ARInvoices.AmountInLocalCurrency, dw_ARInvoices.AmountInProfitCurrency, dw_ARInvoices.AmountDueInLocalCurrency, dw_ARInvoices.AmountDueInProfitCurrency,
	dw_ARInvoiceLines.Description, dw_ARInvoiceLines.LocalDescription, dw_ARInvoiceLines.UnitPrice, dw_ARInvoiceLines.Quantity, DIM_VatTypes.Id_Number, 
	dw_ARInvoiceLines.VatPercentage, dw_ARInvoiceLines.LocalCurrencyAmount,dw_ARInvoiceLines.ForiegnCurrencyAmount,dw_ARInvoiceLines.InvoiceCurrencyAmount, ForiegnCurrency.Id_Number, dw_ARInvoiceLines.ForiegnExchangeRate,
	dw_ARInvoiceLines.ProfitCurrencyAmount,dw_ARInvoiceLines.Notes,dw_ARInvoices.InvoiceCurrencyExchangeRate,dw_ARInvoiceLines.IsExpense,dw_ARInvoiceLines.IsRegionalTax,
	DIM_Branches.Id_Number,dw_ARInvoices.StatusCode, dw_ARInvoices.DraftNumber,
	dw_ARInvoices.IsConsolidationInvoice,dw_ARInvoices.MainEntityId,dw_ARInvoiceLines.EntityId , dw_ARInvoices.MasterNumber, dw_ARInvoices.HouseNumber,@dw_ARInvoices.CustomFieldsVariable,
	ConsolidationInvoice.InvoiceNumber,ConsolidationInvoice.InvoiceDate, ConsolidationInvoice.StatusCode, ConsolidationInvoice.DraftNumber,  Partner.Id_Number, ConsolidationInvoice.Id

	 
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
    inner JOIN DIM_VatTypes ON dw_ARInvoiceLines.VatTypeId = DIM_VatTypes.Id 
	inner JOIN dw_CustomObjectFields  ON dw_ARInvoices.Tenant = dw_CustomObjectFields.Tenant and dw_CustomObjectFields.ObjectTableName = 'ARInvoice'
	inner JOIN dw_ARInvoices ConsolidationInvoice  ON ConsolidationInvoice.Id = dw_ARInvoices.ConsolidationInvoiceId
	inner JOIN DIM_Partners Partner ON dw_ARInvoices.PartnerId = Partner.Id 
 
    where dw_ARInvoices.AutomaticLastUpdateDate > @LastUpdateDate  and dw_ARInvoices.IsConsolidationInvoice = 0   and dw_ARInvoices.Id != '-1'

	OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor   into  @Id ,@Tenant , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber,
	 @InvoiceDate, @CreateDate, @ApprovedDate, @DueDate, @PrintDate, @PaidDate, @ApprovedBy, @CreatedBy, @PrintedBy,
	 @ARInvoicesSalesman, @Status, @PrintNotes, @PaymentTerm, @LocalCurrency, @InvoiceCurrency, @VATNumber, @BillTo, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
	 @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @Description, @LocalDescription, @UnitPrice, @Quantity, @VatType,
	 @VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax,
	 @Branch,@StatusCode, @DraftNumber, @IsConsolidationInvoice , @ARInvoiceEntityId,@ARInvoiceLineEntityId, @InvoiceMasterNumber , @InvoiceHouseNumber,@CursorCustomFieldsVariable,
	 @ConsolidationInvoiceNumber, @ConsolidationInvoiceDate, @ConsolidationStatusCode, @ConsolidationDraftNumber, @Partner, @ConsolidationId
	 
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

		--------------Original Invoice Number------------------
		 
				IF( @DraftNumber IS NOT NULL )
				  BEGIN
					  SET @OriginalInvoiceNumber = @DraftNumber;
				  END
				ELSE
				  BEGIN
					  SET @OriginalInvoiceNumber = @InvoiceNumber;
				  END 
		 
		----------------------------------------------
		 
		declare @IsCancelled as bit  
		set @IsCancelled =0;
		if(@StatusCode = 'AR'or @StatusCode = 'AC' or @StatusCode = 'LL' or @StatusCode = 'VD') 
		begin set @IsCancelled = 1 end  

		 
		 ---------------Consolidation Invoice Number -----------------
	 
	 
				IF( @ConsolidationStatusCode = 'DR' or  @ConsolidationStatusCode = 'LL')
					BEGIN
						IF(@ConsolidationDraftNumber is not null)
							BEGIN
								SET @ConsolidationInvoiceNumber = @ConsolidationDraftNumber;
							END
					END
		 
		----------------------------------------------
				----------------------------------------------

		 ---------------Consolidation Invoice Date -----------------
	 
	 		 
		if(@ConsolidationId = '-1') 
		begin set @ConsolidationInvoiceDate = Null end  

  

		 --------------Resolve Custom Field Data Type Code-------------------
            
			      --@[ResolveCustomFieldDataTypeCodeVariable]

	 ------------------------------------------------------

	   insert into Fact_ARInvoices ([Id],[Source Tenant],[Parent Tenant], [AR Invoice Type], [Invoice Number], [Invoice Date], [Create Date], [Approved Date], [Due Date], [Print Date],[Paid Date], 
	   [Approved By], [Created By], [Printed By], [Invoice Salesman],  [Invoice Status], [Print Note], [Payment Term], [Invoice Local Currency], [Invoice Currency], [VAT Number], [Bill To], 
	   
	   [Subtotal (Local)],[Subtotal (Profit)],[Invoice Amount (Local)],[Invoice Amount (Profit)],[Amount Due (Local)],[Amount Due (Profit)],
	  [Line Description],[Line Local Description], [Line Unit Price], [Line Quantity], [Line VAT Type], 
	   [Line VAT Percentage],[Line Amount (Local)], [Line Amount (Foreign)],[Line Amount (Invoice Currency)], [Foreign Currency], [Foreign Exchange Rate],[Line Amount (Profit)],
       [Line Notes], [Invoice Currency Exchange Rate], [Is Expense], [Is Regional Tax],[Invoice Branch],[Is Cancelled],[Main Entity Id],  [Original Invoice Number], [Invoice Master Number] , [Invoice House Number],[CustomFieldNamesVariable],
	   [Consolidation Invoice Number],[Consolidated Invoice Date],[Partner] )
	   
	   
      values(@Id  , @SourceTenant, @ParentTenant, @InvoiceType, @InvoiceNumber, dbo.GetDateFormateAsNumber(@InvoiceDate), dbo.GetDateFormateAsNumber(@CreateDate),dbo.GetDateFormateAsNumber(@ApprovedDate),  dbo.GetDateFormateAsNumber(@DueDate) ,dbo.GetDateFormateAsNumber(@PrintDate),dbo.GetDateFormateAsNumber(@PaidDate),
	  @ApprovedBy, @CreatedBy, @PrintedBy, @ARInvoicesSalesman, @Status, @PrintNotes,  @PaymentTerm, @LocalCurrency, @InvoiceCurrency,@VATNumber, @BillTo,
	 
	 @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency, @AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @Description, @LocalDescription, @UnitPrice, @Quantity,  @VatType,
	 @VatPercentage, @LocalCurrencyAmount,@ForiegnCurrencyAmount,@InvoiceCurrencyAmount,@ForiegnCurrencyId, @ForiegnExchangeRate, @ProfitCurrencyAmount,@Notes,@InvoiceCurrencyExchangeRate,@IsExpens,@IsRegionalTax,
	 @Branch,@IsCancelled,@MainEntityId, @OriginalInvoiceNumber, @InvoiceMasterNumber , @InvoiceHouseNumber, [CustomFieldValuesVariable],@ConsolidationInvoiceNumber,dbo.GetDateFormateAsNumber(@ConsolidationInvoiceDate), @Partner)



	   

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
	@Branch, @StatusCode, @DraftNumber, @IsConsolidationInvoice,  @ARInvoiceEntityId,@ARInvoiceLineEntityId , @InvoiceMasterNumber , @InvoiceHouseNumber,@CursorCustomFieldsVariable,
	@ConsolidationInvoiceNumber, @ConsolidationInvoiceDate, @ConsolidationStatusCode, @ConsolidationDraftNumber, @Partner, @ConsolidationId

		End
	CLOSE ARInvoicesCursor
	DEALLOCATE ARInvoicesCursor
	 

	end