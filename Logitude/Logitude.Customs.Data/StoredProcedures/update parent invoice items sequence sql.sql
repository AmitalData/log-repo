
IF OBJECT_ID('[Customs].[usp_UpdateParentInvoiceItemsSequence]', 'P') IS NOT NULL
drop PROCEDURE [Customs].usp_UpdateParentInvoiceItemsSequence
GO

Create PROCEDURE [Customs].usp_UpdateParentInvoiceItemsSequence
(
@DeclarationId varchar(15),
@Tenant int,
@CounterKey int
)
AS

declare @SequenceNumeric integer
set @SequenceNumeric=0
declare @LineNumber integer
BEGIN 

	DECLARE InvoiceItemsCursor CURSOR READ_ONLY
	FOR
	SELECT LineNumber
	From Customs.SupplierInvoiceItems where DeclarationId=@DeclarationId and CounterKey=@CounterKey and Tenant=@Tenant and IsParent=1 order by OrderByLineNo
	OPEN InvoiceItemsCursor FETCH NEXT FROM InvoiceItemsCursor INTO @LineNumber
	WHILE @@FETCH_STATUS = 0

	BEGIN
	
		set @SequenceNumeric=@SequenceNumeric+1
	    update Customs.SupplierInvoiceItems set SequenceNumeric=@SequenceNumeric
	    where DeclarationId=@DeclarationId 	and LineNumber=@LineNumber and CounterKey=@CounterKey

			


	FETCH NEXT FROM InvoiceItemsCursor INTO @LineNumber	
	END

	CLOSE InvoiceItemsCursor
	DEALLOCATE InvoiceItemsCursor

END