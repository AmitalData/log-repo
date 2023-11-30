
   declare @Code as varchar(2)
   declare @Name as varchar(20)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ARInvoiceTransferStatusCursor CURSOR READ_ONLY
	FOR
	SELECT Code, [Name], dw_ARInvoiceTransferStatus.AutomaticLastUpdateDate
	From dw_ARInvoiceTransferStatus
	OPEN ARInvoiceTransferStatusCursor FETCH NEXT FROM ARInvoiceTransferStatusCursor INTO @Code, @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ARInvoiceTransferStatusTemp ([Code], [Name], [Automatic Last Update Date]) values(@Code, @Name,@AutomaticLastUpdateDate)

	FETCH NEXT FROM ARInvoiceTransferStatusCursor  INTO @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE ARInvoiceTransferStatusCursor
	DEALLOCATE ARInvoiceTransferStatusCursor

