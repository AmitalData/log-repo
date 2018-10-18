

-- Script To Update All AR Invoices Transfer fields --

DECLARE @Tenant AS INT
DECLARE @ARInvoiceId AS varchar(15)
DECLARE @DebitAccount AS varchar(15)
DECLARE @AccountingExternalCode AS varchar(3)
DECLARE @TransferError AS nvarchar(250)
DECLARE @TransferStatusCode as varchar(2)
DECLARE @IsReadyForTransfer AS BIT

declare @LinesErrorMessage as nvarchar(250)
declare @ExternalCodeError as nvarchar(250)
declare @VatCardError as nvarchar(250)
set @LinesErrorMessage = 'Credit Account is required'
set @ExternalCodeError = 'External Code is required'
set @VatCardError = 'External VAT Card is required'

BEGIN;
	DECLARE ARInvoicesCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant,Id,DebitAccount,AccountingExternalCode,TransferStatusCode
	FROM ARInvoices
	OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @Tenant,@ARInvoiceId,@DebitAccount,@AccountingExternalCode,@TransferStatusCode
	WHILE @@FETCH_STATUS = 0
	BEGIN

	SET @TransferError = NULL
	SET @IsReadyForTransfer = 1

	IF (@TransferStatusCode != 'TR')
	BEGIN;
			IF (@DebitAccount Is Null)
			BEGIN;
				SET @IsReadyForTransfer = 0
				SET @TransferError = 'Debit Account is required'
			END

			IF (@AccountingExternalCode Is Null)
			BEGIN;
				SET @IsReadyForTransfer = 0
				if (@TransferError is null)
				set @TransferError = @ExternalCodeError		
				else set @TransferError = @TransferError + ',' + @ExternalCodeError
			END

			IF EXISTS (Select Id from ARInvoiceLines where Tenant = @Tenant AND ARInvoiceId = @ARInvoiceId AND CreditAccount is null)				
			BEGIN;
				SET @IsReadyForTransfer = 0		
				if (@TransferError is null)
				set @TransferError = @LinesErrorMessage			
				else set @TransferError = @TransferError + ',' + @LinesErrorMessage
			END

			IF EXISTS (Select Id from ARInvoiceTotalVATs where Tenant = @Tenant AND ARInvoiceId = @ARInvoiceId AND VatPercent is not null AND VatPercent != 0 AND (ExternalVATCard is null OR ExternalVATCard = ''))
			BEGIN;
				SET @IsReadyForTransfer = 0
				if (@TransferError is null)
				set @TransferError = @VatCardError		
				else set @TransferError = @TransferError + ',' + @VatCardError			
			END

		IF (@TransferStatusCode != 'BL')
		BEGIN
			if (@IsReadyForTransfer = 1)
			set @TransferStatusCode = 'RD'
			else set @TransferStatusCode = 'NR'
		END
	END


				 
		 Update ARInvoices
		 set  
		 TransferError = @TransferError,
		 TransferStatusCode =@TransferStatusCode
		 Where Id = @ARInvoiceId AND Tenant = @Tenant

	FETCH NEXT FROM ARInvoicesCursor INTO @Tenant,@ARInvoiceId,@DebitAccount,@AccountingExternalCode,@TransferStatusCode
	END
	CLOSE ARInvoicesCursor
	DEALLOCATE ARInvoicesCursor	
END