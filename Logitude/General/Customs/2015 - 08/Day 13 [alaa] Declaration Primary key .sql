
declare @id varchar(15)

Declare DeclarationInvoicesCursor cursor 
    for
    select Id from customs.Declarations  
    OPEN DeclarationInvoicesCursor FETCH NEXT FROM DeclarationInvoicesCursor  into @id 
	WHILE @@FETCH_STATUS = 0
	BEGIN


    update customs.Declarations set PrimaryInvoiceCounterKey = (select InvoiceCounterKey from customs.SupplierInvoices where DeclarationId = @id  and SequenceNumeric =1) where Id = @id
   


    FETCH NEXT FROM DeclarationInvoicesCursor INTO  @id
	END
	CLOSE DeclarationInvoicesCursor
	DEALLOCATE DeclarationInvoicesCursor



	



