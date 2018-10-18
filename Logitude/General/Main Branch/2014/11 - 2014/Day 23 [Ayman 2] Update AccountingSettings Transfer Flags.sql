
declare @Code as varchar(4)
declare @AllowARInvoicesTransfer as bit
declare @AllowAPInvoicesTransfer as bit

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Code, AllowARInvoicesTransfer, AllowAPInvoicesTransfer
		FROM AccountingSystems
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Code, @AllowARInvoicesTransfer, @AllowAPInvoicesTransfer
		WHILE @@FETCH_STATUS = 0
			BEGIN
		
				update AccountingSettings set
				IsARInvoicesTransferEnabled = @AllowARInvoicesTransfer,
				IsAPInvoicesTransferEnabled = @AllowAPInvoicesTransfer
				where AccountingSystemCode = @Code

			FETCH NEXT FROM DataCursor INTO @Code, @AllowARInvoicesTransfer, @AllowAPInvoicesTransfer
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END