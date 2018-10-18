


declare @Tenant as int
set @Tenant = 570

declare @IsJournalMode as bit
declare @IsFromExternalTable as bit
declare @AccountingSystemCode as varchar(10)
set @AccountingSystemCode = (select AccountingSystemCode from AccountingSettings where Id = @Tenant)

if (@AccountingSystemCode is null)
BEGIN
	print 'Error'
END

else
BEGIN

	select
	@IsJournalMode = IsJournalMode,
	@IsFromExternalTable = IsExternalCodesFromTable
	from AccountingSystems
	where Code = @AccountingSystemCode

	-- Invoice Variables
	declare @IsUpdating as bit
	declare @InvoiceId as varchar(15)
	declare @DebitAccount as varchar(40)
	declare @AccountingExternalCode as nvarchar(25)	
	declare @PaymentTermExternalId as varchar(25)
	declare @BillToId as varchar(15)
	declare @InvoiceCurrencyId as varchar(15)
	declare @PaymentTermId as varchar(15)
	declare @PaymentTermExternalCode as varchar(25)

	-- Invoicelines Variables
	declare @InvoiceLineId as varchar(15)
	declare @InvoiceLineCreditAccount as varchar(40)
	declare @InvoiceLineVatTypeId as varchar(15)
	declare @InvoiceLineChargesTypeId as varchar(15)
	declare @ChargesTypeExternalCode as varchar(25)

	-- InvoiceVats Variables
	declare @VATLineId as varchar(15)
	declare @VATLineVatTypeId as varchar(15)
	declare @VATLineExternalVATCard as varchar(25)
	declare @VATLineExternalTAXItemId as varchar(25)
			

	DECLARE InvoicesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, DebitAccount, AccountingExternalCode, PaymentTermExternalId, BillToId, InvoiceCurrencyId, PaymentTermId
	FROM ARInvoices
	where Tenant = @Tenant
	OPEN InvoicesCursor FETCH NEXT FROM InvoicesCursor INTO @InvoiceId, @DebitAccount, @AccountingExternalCode, @PaymentTermExternalId, @BillToId, @InvoiceCurrencyId, @PaymentTermId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		set @IsUpdating = 0

		-- DebitAccount : BillTo
		if (@DebitAccount is null OR @DebitAccount = '')
		BEGIN
			set @IsUpdating = 1

			if (@IsFromExternalTable = 1)
			begin
				set @DebitAccount = (select ExternalTableId from CardExternalCodeByCurrencies where Tenant = @Tenant and CardId = @BillToId and CurrencyId = @InvoiceCurrencyId)				
			end

			else
			begin
				set @DebitAccount = (select AccountingCard from Cards where Tenant = @Tenant and Id = @BillToId)				
			end			
		END

		-- AccountingExternalCode : Invoice Currency
		if (@AccountingExternalCode is null OR @AccountingExternalCode = '')
		BEGIN
			set @IsUpdating = 1

			if (@IsFromExternalTable = 1)
			begin
				set @AccountingExternalCode = (select AccountingExternalCode from Currencies where Tenant = @Tenant and Id = @InvoiceCurrencyId)
			end

			else
			begin
				set @AccountingExternalCode = (select AccountingExternalCode from Currencies where Tenant = @Tenant and Id = @InvoiceCurrencyId)				
			end
		END

		-- PaymentTermExternalId
		if (@PaymentTermExternalId is null OR @PaymentTermExternalId = '')
		BEGIN		
			if (@PaymentTermId is not null)
			BEGIN
				set @IsUpdating = 1

				if (@IsFromExternalTable = 1)
				begin
					set @PaymentTermExternalCode = (select ExternalId from PaymentTerms where Tenant = @Tenant and Id = @PaymentTermId)
					if (@PaymentTermExternalCode is not null)
					begin
						set @PaymentTermExternalId  = (select Id from ExternalSystemsTablesCodes where Tenant = @Tenant and Code = @PaymentTermExternalCode)
					end
				end

				else
				begin
					set @PaymentTermExternalId = (select ExternalId from PaymentTerms where Tenant = @Tenant and Id = @PaymentTermId)
				end
			END
		END

		-- Loop Invoicelines
		BEGIN
		DECLARE LinesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, CreditAccount, VatTypeId, ChargesTypeId
		FROM ARInvoiceLines
		where Tenant = @Tenant and ARInvoiceId = @InvoiceId
		OPEN LinesCursor FETCH NEXT FROM LinesCursor INTO @InvoiceLineId, @InvoiceLineCreditAccount, @InvoiceLineVatTypeId, @InvoiceLineChargesTypeId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			if (@InvoiceLineCreditAccount is null Or @InvoiceLineCreditAccount = '')
			BEGIN
				if ((select AccountingVATSplit from ChargesTypes where Tenant = @Tenant and Id = @InvoiceLineChargesTypeId) = 1)
				begin
					set @InvoiceLineCreditAccount = (select top 1 ReceivableCreditAccount from ChargeTypeAccountings where Tenant = @Tenant and VatTypeId = @InvoiceLineVatTypeId and ChargeTypeId = @InvoiceLineChargesTypeId)
				end

				else if (@IsJournalMode = 1)
				begin
					set @InvoiceLineCreditAccount = (select ReceivableCreditAccount from ChargesTypes where Tenant = @Tenant and Id = @InvoiceLineChargesTypeId)
				end

				else
				begin
					set @InvoiceLineCreditAccount = (select ChargesTypeExternalCode from ChargesTypes where Tenant = @Tenant and Id = @InvoiceLineChargesTypeId)
				end

				update ARInvoiceLines
				set CreditAccount = @InvoiceLineCreditAccount
				where Tenant = @Tenant and ARInvoiceId = @InvoiceId and Id = @InvoiceLineId
			END

		FETCH NEXT FROM LinesCursor INTO @InvoiceLineId, @InvoiceLineCreditAccount, @InvoiceLineVatTypeId, @InvoiceLineChargesTypeId
		END
		CLOSE LinesCursor
		DEALLOCATE LinesCursor
		END

		-- Loop VATLines
		BEGIN
		DECLARE VATSCursor CURSOR READ_ONLY
		FOR
		SELECT Id, VatTypeId, ExternalVATCard, ExternalTAXItemId
		FROM ARInvoiceTotalVATs
		where Tenant = @Tenant and ARInvoiceId = @InvoiceId
		OPEN VATSCursor FETCH NEXT FROM VATSCursor INTO @VATLineId, @VATLineVatTypeId, @VATLineExternalVATCard, @VATLineExternalTAXItemId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			if (@VATLineExternalVATCard is null Or @VATLineExternalVATCard = '')
			begin
				set @VATLineExternalVATCard = (select ExternalVATCard from VatTypes where Tenant = @Tenant AND Id = @VATLineVatTypeId)
			end

			if (@VATLineExternalTAXItemId is null Or @VATLineExternalTAXItemId = '')
			begin
				set @VATLineExternalTAXItemId = (select ExternalTAXItemId from VatTypes where Tenant = @Tenant AND Id = @VATLineVatTypeId)
			end

			update ARInvoiceTotalVATs
			set
			ExternalVATCard = @VATLineExternalVATCard,
			ExternalTAXItemId = @VATLineExternalTAXItemId
			where Tenant = @Tenant AND Id = @VATLineId

		FETCH NEXT FROM VATSCursor INTO @VATLineId, @VATLineVatTypeId, @VATLineExternalVATCard, @VATLineExternalTAXItemId
		END
		CLOSE VATSCursor
		DEALLOCATE VATSCursor
		END


		if (@IsUpdating = 1)
		BEGIN
			update ARInvoices
			set
			DebitAccount = @DebitAccount,
			AccountingExternalCode = @AccountingExternalCode,
			PaymentTermExternalId = @PaymentTermExternalId
			where Tenant = @Tenant and Id = @InvoiceId
		END

	FETCH NEXT FROM InvoicesCursor INTO @InvoiceId, @DebitAccount, @AccountingExternalCode, @PaymentTermExternalId, @BillToId, @InvoiceCurrencyId, @PaymentTermId
	END
	CLOSE InvoicesCursor
	DEALLOCATE InvoicesCursor
END