
DECLARE @Tenant AS INT
DECLARE @LineNumber AS INT
DECLARE @LineId AS varchar(15)
DECLARE @InvoiceId AS varchar(15)
DECLARE @VatTypeId AS varchar(15)

BEGIN; -- Update APInvoiceLine	
	DECLARE APInvoiceLineCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant,LineNumber,APInvoiceId,VatTypeId
	FROM APInvoiceLines 
	OPEN APInvoiceLineCursor FETCH NEXT FROM APInvoiceLineCursor INTO @Tenant,@LineNumber,@InvoiceId,@VatTypeId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		IF EXISTS (SELECT * from APInvoiceTotalVATs where APInvoiceId = @InvoiceId AND VatTypeId = @VatTypeId)
			BEGIN;
				Update APInvoiceLines
				set VatPercentage = (Select top 1 VatPercent from APInvoiceTotalVATs where APInvoiceId = @InvoiceId AND VatTypeId = @VatTypeId AND Tenant = @Tenant)
				where APInvoiceId = @InvoiceId AND LineNumber = @LineNumber AND Tenant = @Tenant AND VatTypeId = @VatTypeId
			END

	FETCH NEXT FROM APInvoiceLineCursor INTO @Tenant,@LineNumber,@InvoiceId,@VatTypeId
	END
	CLOSE APInvoiceLineCursor
	DEALLOCATE APInvoiceLineCursor
END

BEGIN; -- Update ARInvoiceLine	
	DECLARE ARInvoiceLineCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant,Id,ARInvoiceId,VatTypeId
	FROM ARInvoiceLines 
	OPEN ARInvoiceLineCursor FETCH NEXT FROM ARInvoiceLineCursor INTO @Tenant,@LineId,@InvoiceId,@VatTypeId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	    IF EXISTS (SELECT * from ARInvoiceTotalVATs where ARInvoiceId = @InvoiceId AND VatTypeId = @VatTypeId)
			BEGIN;
				Update ARInvoiceLines
				set VatPercentage = (Select top 1 VatPercent from ARInvoiceTotalVATs where ARInvoiceId = @InvoiceId AND VatTypeId = @VatTypeId)
				where ARInvoiceId = @InvoiceId AND Id = @LineId AND Tenant = @Tenant AND VatTypeId = @VatTypeId
			END

	FETCH NEXT FROM ARInvoiceLineCursor INTO @Tenant,@LineId,@InvoiceId,@VatTypeId
	END
	CLOSE ARInvoiceLineCursor
	DEALLOCATE ARInvoiceLineCursor
END