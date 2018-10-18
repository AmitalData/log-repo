

-- Script To Update All (AP) Invoices Transfer fields --

DECLARE @Tenant AS INT
DECLARE @InvoiceId AS varchar(15)
DECLARE @CreditAccount AS varchar(15)
DECLARE @AccountingExternalCode AS varchar(3)
DECLARE @TransferError AS nvarchar(250)
DECLARE @TransferStatusCode as varchar(2)
DECLARE @IsReadyForTransfer AS BIT

declare @LinesErrorMessage as nvarchar(250)
declare @ExternalCodeError as nvarchar(250)
declare @VatCardError as nvarchar(250)
set @LinesErrorMessage = 'Debit Account is required'
set @ExternalCodeError = 'External Code is required'
set @VatCardError = 'External VAT Card is required'

BEGIN;
	DECLARE APInvoicesCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant,Id, CreditAccount,AccountingExternalCode,TransferStatusCode
	FROM APInvoices
	OPEN APInvoicesCursor FETCH NEXT FROM APInvoicesCursor INTO @Tenant,@InvoiceId,@CreditAccount,@AccountingExternalCode,@TransferStatusCode
	WHILE @@FETCH_STATUS = 0
	BEGIN

	SET @TransferError = NULL
	SET @IsReadyForTransfer = 1

	IF (@TransferStatusCode != 'TR')
	BEGIN;
			IF (@CreditAccount Is Null)
			BEGIN;
				SET @IsReadyForTransfer = 0
				SET @TransferError = 'Credit Account is required'
			END

			IF (@AccountingExternalCode Is Null)
			BEGIN;
				SET @IsReadyForTransfer = 0
				if (@TransferError is null)
				set @TransferError = @ExternalCodeError		
				else set @TransferError = @TransferError + ',' + @ExternalCodeError
			END

			IF EXISTS (Select * from APInvoiceLines where Tenant = @Tenant AND APInvoiceId = @InvoiceId AND DebitAccount is null)				
			BEGIN;
				SET @IsReadyForTransfer = 0		
				if (@TransferError is null)
				set @TransferError = @LinesErrorMessage			
				else set @TransferError = @TransferError + ',' + @LinesErrorMessage
			END

			IF EXISTS (Select Id from APInvoiceTotalVATs where Tenant = @Tenant AND APInvoiceId = @InvoiceId AND VatPercent is not null AND VatPercent != 0 AND (ExternalVATCard is null OR ExternalVATCard = ''))
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
				 
		 Update APInvoices
		 set  
		 TransferError = @TransferError,
		 TransferStatusCode =@TransferStatusCode
		 Where Id = @InvoiceId AND Tenant = @Tenant

	FETCH NEXT FROM APInvoicesCursor INTO @Tenant,@InvoiceId,@CreditAccount,@AccountingExternalCode,@TransferStatusCode
	END
	CLOSE APInvoicesCursor
	DEALLOCATE APInvoicesCursor	
END