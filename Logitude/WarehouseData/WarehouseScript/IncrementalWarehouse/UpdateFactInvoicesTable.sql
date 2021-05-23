
declare @AutomaticLastUpdateDate as datetime
declare @ARInvoicesAutomaticLastUpdateDate as datetime
declare @APInvoicesAutomaticLastUpdateDate as datetime
declare @LastUpdateDate as datetime

set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ARInvoice' or TableName = 'APInvoice')
set @ARInvoicesAutomaticLastUpdateDate = (select MAX(AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoices)
set @APInvoicesAutomaticLastUpdateDate = (select MAX(AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_APInvoices)
if(@ARInvoicesAutomaticLastUpdateDate > @APInvoicesAutomaticLastUpdateDate)
	set @AutomaticLastUpdateDate = @ARInvoicesAutomaticLastUpdateDate
else
	set @AutomaticLastUpdateDate = @APInvoicesAutomaticLastUpdateDate

 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Id as varchar(15)
	declare @SourceTenant as int
	declare @ParentTenant as int
	declare @Tenant as int
	declare @InvoiceNumber as varchar(25)
	declare @VATNumber as varchar(25)
	declare @ShipmentsNumbers as varchar(1000)
	declare @SubTotalInLocalCurrency as float
	declare @SubTotalInInvoiceCurrency as float
	declare @AmountInLocalCurrency as float
	declare @AmountInProfitCurrency as float
	declare @AmountDueInLocalCurrency as float
	declare @AmountDueInProfitCurrency as float
	declare @PrintNotes as varchar(250)
	declare @InvoiceDate as datetime
	declare @CreateDate as datetime
	declare @ApprovedDate as datetime
	declare @DueDate as datetime
	declare @PrintDate as datetime
	declare @FirstApproveDate as datetime
	declare @Branch as int
	declare @PaymentTerm as int
	declare @LocalCurrency as int
	declare @InvoiceCurrency as int
	declare @Salesman as int
	declare @ApprovedBy as int
	declare @CreatedBy as int
	declare @PrintedBy as int
	declare @Status as varchar(20)
	declare @AR_APInvoice as varchar(15)
	declare @InvoiceType as varchar(20)
	declare @Partner as int
	declare @PartnerExternalID as nvarchar(25)
	declare @MainEntityId as varchar(15)

	DECLARE InvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT	dw_ARInvoices.Id, dw_ARInvoices.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number], dw_ARInvoices.InvoiceNumber,
			dw_ARInvoices.VatNumber, dw_ARInvoices.ShipmentsNumbers, dw_ARInvoices.SubTotalInLocalCurrency, dw_ARInvoices.SubTotalInInvoiceCurrency,
			dw_ARInvoices.AmountInLocalCurrency, dw_ARInvoices.AmountInProfitCurrency, dw_ARInvoices.AmountDueInLocalCurrency, dw_ARInvoices.AmountDueInProfitCurrency,
			dw_ARInvoices.PrintNotes, dw_ARInvoices.InvoiceDate, dw_ARInvoices.CreateDate, dw_ARInvoices.ApprovedDate, dw_ARInvoices.DueDate, dw_ARInvoices.PrintDate,
			null, DIM_Branches.Id_Number, DIM_PaymentTerms.Id_Number, LocalCurrency.Id_Number, InvoiceCurrency.Id_Number,
			SalesmanUser.Id_Number, ApprovedByUser.Id_Number, CreatedByUser.Id_Number, PrintedByUser.Id_Number, dw_ARInvoiceStatus.Name, 'AR Invoice',
			dw_ARInvoiceTypes.Name, ARPartner.Id_Number, dw_ARInvoices.AccountingExternalCode, dw_ARInvoices.MainEntityId
		--, @dw_ARInvoices.CustomFieldsVariable

    From dw_ARInvoices
	inner JOIN dw_Tenants  ON dw_ARInvoices.Tenant = dw_Tenants.Id
	inner JOIN dw_DWHSettings ON dw_ARInvoices.Tenant = dw_DWHSettings.Tenant
	
	inner JOIN DIM_Tenants SourceTenant ON dw_ARInvoices.Tenant = SourceTenant.[Tenant Number]
	inner JOIN DIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]

	inner JOIN DIM_Branches ON dw_ARInvoices.BranchId = DIM_Branches.Id
	
	inner JOIN DIM_PaymentTerms ON dw_ARInvoices.PaymentTermId = DIM_PaymentTerms.Id

	inner JOIN DIM_Currencies LocalCurrency ON dw_ARInvoices.LocalCurrencyId = LocalCurrency.Id
	inner JOIN DIM_Currencies InvoiceCurrency ON dw_ARInvoices.InvoiceCurrencyId = InvoiceCurrency.Id
	
	inner JOIN DIM_Users SalesmanUser ON dw_ARInvoices.SalesmanUserId = SalesmanUser.Id
	inner JOIN DIM_Users ApprovedByUser ON dw_ARInvoices.ApprovedByUserId = ApprovedByUser.Id
	inner JOIN DIM_Users CreatedByUser ON dw_ARInvoices.CreatedByUserId = CreatedByUser.Id
	inner JOIN DIM_Users PrintedByUser ON dw_ARInvoices.PrintByUserId = PrintedByUser.Id

	inner JOIN dw_ARInvoiceStatus ON dw_ARInvoices.StatusCode = dw_ARInvoiceStatus.Code
	
	inner JOIN dw_ARInvoiceTypes ON dw_ARInvoices.ARInvoiceTypeCode = dw_ARInvoiceTypes.Code
	
	inner JOIN DIM_Partners ARPartner ON dw_ARInvoices.PartnerId = ARPartner.Id

	--inner JOIN dw_CustomObjectFields  ON dw_ARInvoices.Tenant = dw_CustomObjectFields.Tenant and dw_CustomObjectFields.ObjectTableName = 'ARInvoice'

	where dw_ARInvoices.AutomaticLastUpdateDate > @LastUpdateDate 

	UNION ALL

	SELECT	dw_APInvoices.Id, dw_APInvoices.Tenant, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number], dw_APInvoices.InvoiceNumber,
			dw_APInvoices.VatNumber, dw_APInvoices.ShipmentsNumbers, dw_APInvoices.SubTotalInLocalCurrency, dw_APInvoices.SubTotalInInvoiceCurrency,
			dw_APInvoices.AmountInLocalCurrency, dw_APInvoices.AmountInProfitCurrency, dw_APInvoices.AmountDueInLocalCurrency, dw_APInvoices.AmountDueInProfitCurrency,
			null, dw_APInvoices.InvoiceDate, dw_APInvoices.CreateDate, dw_APInvoices.ApprovedDate, dw_APInvoices.DueDate, null,
			dw_APInvoices.FirstApproveDate, DIM_Branches.Id_Number, DIM_PaymentTerms.Id_Number, LocalCurrency.Id_Number, InvoiceCurrency.Id_Number,
			1, ApprovedByUser.Id_Number, CreatedByUser.Id_Number, 1, dw_APInvoiceStatus.Name, 'AP Invoice',
			'Invoice', 1, dw_APInvoices.AccountingExternalCode, dw_APInvoices.MainEntityId
			--, @dw_ARInvoices.CustomFieldsVariable

    From dw_APInvoices
	inner JOIN dw_Tenants  ON dw_APInvoices.Tenant = dw_Tenants.Id
	inner JOIN dw_DWHSettings ON dw_APInvoices.Tenant = dw_DWHSettings.Tenant
	
	inner JOIN DIM_Tenants SourceTenant ON dw_APInvoices.Tenant = SourceTenant.[Tenant Number]
	inner JOIN DIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
	
	inner JOIN DIM_Branches ON dw_APInvoices.BranchId = DIM_Branches.Id
	
	inner JOIN DIM_PaymentTerms ON dw_APInvoices.PaymentTermId = DIM_PaymentTerms.Id
	
	inner JOIN DIM_Currencies LocalCurrency ON dw_APInvoices.LocalCurrencyId = LocalCurrency.Id
	inner JOIN DIM_Currencies InvoiceCurrency ON dw_APInvoices.InvoiceCurrencyId = InvoiceCurrency.Id

	inner JOIN DIM_Users ApprovedByUser ON dw_APInvoices.ApprovedByUserId = ApprovedByUser.Id
	inner JOIN DIM_Users CreatedByUser ON dw_APInvoices.CreatedByUserId = CreatedByUser.Id
	--inner JOIN DIM_Users PrintedByUser ON dw_APInvoices.PrintByUserId = PrintedByUser.Id

	inner JOIN dw_APInvoiceStatus ON dw_APInvoices.StatusCode = dw_APInvoiceStatus.Code
	
	--inner JOIN dw_APInvoiceTypes ON dw_APInvoices.APInvoiceTypeCode = dw_APInvoiceTypes.Code

	--inner JOIN dw_CustomObjectFields  ON dw_APInvoices.Tenant = dw_CustomObjectFields.Tenant and dw_CustomObjectFields.ObjectTableName = 'APInvoice'

	where dw_APInvoices.AutomaticLastUpdateDate > @LastUpdateDate 

	OPEN InvoicesCursor FETCH NEXT FROM InvoicesCursor into
		@Id, @Tenant, @SourceTenant, @ParentTenant, @InvoiceNumber, @VATNumber, @ShipmentsNumbers, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
		@AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @PrintNotes, @InvoiceDate, @CreateDate,
		@ApprovedDate, @DueDate, @PrintDate, @FirstApproveDate, @Branch, @PaymentTerm, @LocalCurrency, @InvoiceCurrency,
		@Salesman, @ApprovedBy, @CreatedBy, @PrintedBy, @Status, @AR_APInvoice,
		@InvoiceType, @Partner, @PartnerExternalID, @MainEntityId


	WHILE @@FETCH_STATUS = 0
	BEGIN
		BEGIN TRY
		insert into Fact_Invoices 
			([Id], [Source Tenant], [Parent Tenant], [Invoice Number], [VAT Number], [Shipment Number], [Subtotal (Local)], [Subtotal (Profit)],
			[Invoice Amount (Local)], [Invoice Amount (Profit)], [Amount Due (Local)], [Amount Due (Profit)], [Print Note], [Invoice Date], [Create Date],
			[Approved Date], [Due Date], [Print Date], [First Approve Date], [Branch], [Payment Term], [Local Currency], [Invoice Currency],
			[Salesman], [Approved By], [Created By], [Printed By], [Status], [AR_AP Invoice],
			[Invoice Type], [Partner], [Partner External ID], [Main Entity Id])
	   
		values (
			@Id, @SourceTenant, @ParentTenant, @InvoiceNumber, @VATNumber, @ShipmentsNumbers, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
			@AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @PrintNotes, dbo.GetDateFormateAsNumber(@InvoiceDate), dbo.GetDateFormateAsNumber(@CreateDate),
			dbo.GetDateFormateAsNumber(@ApprovedDate), dbo.GetDateFormateAsNumber(@DueDate), dbo.GetDateFormateAsNumber(@PrintDate), dbo.GetDateFormateAsNumber(@FirstApproveDate), @Branch, @PaymentTerm, @LocalCurrency, @InvoiceCurrency,
			@Salesman, @ApprovedBy, @CreatedBy, @PrintedBy, @Status, @AR_APInvoice,
			@InvoiceType, @Partner, @PartnerExternalID, @MainEntityId
			)

		END TRY 

		BEGIN CATCH  
			declare @Exception as varchar(4000)
			set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
			set @Exception = @Exception + ' (InvoiceId: ' + @Id +') '+ ' (Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
			RAISERROR(@Exception, 16, 3);
			RETURN;
		END CATCH  

		FETCH NEXT FROM InvoicesCursor  INTO
			@Id, @Tenant, @SourceTenant, @ParentTenant, @InvoiceNumber, @VATNumber, @ShipmentsNumbers, @SubTotalInLocalCurrency, @SubTotalInInvoiceCurrency,
			@AmountInLocalCurrency, @AmountInProfitCurrency, @AmountDueInLocalCurrency, @AmountDueInProfitCurrency, @PrintNotes, @InvoiceDate, @CreateDate,
			@ApprovedDate, @DueDate, @PrintDate, @FirstApproveDate, @Branch, @PaymentTerm, @LocalCurrency, @InvoiceCurrency,
			@Salesman, @ApprovedBy, @CreatedBy, @PrintedBy, @Status, @AR_APInvoice,
			@InvoiceType, @Partner, @PartnerExternalID, @MainEntityId

	END
	CLOSE InvoicesCursor
	DEALLOCATE InvoicesCursor


	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'ARInvoice' or TableName = 'APInvoice'

End

