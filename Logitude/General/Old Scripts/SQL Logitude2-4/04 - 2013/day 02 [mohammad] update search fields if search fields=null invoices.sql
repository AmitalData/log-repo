DECLARE @Id varchar(15)
-----------------------
DECLARE @InvoiceNumber varchar(250)
DECLARE @VatNumber varchar(250)
DECLARE @DraftNumber varchar(250)
DECLARE @BillToId varchar(250)
DECLARE @BillToName varchar(250)




DECLARE invoicecursor CURSOR READ_ONLY
FOR
SELECT Id,InvoiceNumber,VatNumber,DraftNumber,BillToId
FROM ARInvoices

OPEN invoicecursor

	FETCH NEXT FROM invoicecursor
	INTO @Id,@InvoiceNumber,@VatNumber,@DraftNumber,@BillToId


WHILE @@FETCH_STATUS = 0

BEGIN
print 'AR Invoice Id:'
PRINT @Id

set @BillToName = (select EnglishName from Cards
Where Id = @BillToId)
print @BillToId
--Invoice entites cursor----
-----------------------------------------------------------------------------
 DECLARE @InvoiceEntityId varchar(250)
 DECLARE @EntityReference varchar(250)
 DECLARE @EntityReference_local varchar(250)
 SET @EntityReference = ''

 DECLARE invoiceentitiescursor CURSOR READ_ONLY
FOR
SELECT Id
FROM ARInvoiceEntities
where ARInvoiceId=@Id

OPEN invoiceentitiescursor

	FETCH NEXT FROM invoiceentitiescursor
	INTO @InvoiceEntityId

WHILE @@FETCH_STATUS = 0

BEGIN

set @EntityReference_local = (select EntityReference from ARInvoiceEntities
Where Id = @InvoiceEntityId)

SET @EntityReference = @EntityReference + isnull(@EntityReference_local,'')  + ','

	
	FETCH NEXT FROM invoiceentitiescursor
	INTO @InvoiceEntityId
END

CLOSE invoiceentitiescursor
DEALLOCATE invoiceentitiescursor
-----------------------------------------------------------------------------
--------------------

--Invoice Payments cursor----
-----------------------------------------------------------------------------
 DECLARE @ARPaymentId varchar(250)
  DECLARE @PaymentNumber_local varchar(250)
 DECLARE @PaymentNumber varchar(250)
 SET @PaymentNumber = ''

 DECLARE invoicepaymentscursor CURSOR READ_ONLY
FOR
SELECT ARPaymentId
FROM ARInvoicePayments
where ARInvoiceId=@Id

OPEN invoicepaymentscursor

	FETCH NEXT FROM invoicepaymentscursor
	INTO @ARPaymentId

WHILE @@FETCH_STATUS = 0

BEGIN

set @PaymentNumber_local = (select PaymentNo from ARPayments
Where Id = @ARPaymentId)

SET @PaymentNumber = @PaymentNumber + isnull(@PaymentNumber_local,'')  + ','
	
	FETCH NEXT FROM invoicepaymentscursor
	INTO @ARPaymentId
END

CLOSE invoicepaymentscursor
DEALLOCATE invoicepaymentscursor
-----------------------------------------------------------------------------
--------------------
Print 'Entity Ref:'
PRINT @EntityReference
print 'Payment NO:'
print @PaymentNumber
print @BillToName
    Update ARInvoices
set SearchFields = 
isnull(@InvoiceNumber,'') + ',' + 
isnull(@VatNumber,'') + ',' + 
isnull(@DraftNumber,'') + ',' + 
isnull(@BillToName,'') + ',' + 
isnull(@EntityReference,'') + ',' + 
isnull(@PaymentNumber,'')+','+isnull(HouseNumber,'')+','+isnull(MasterNumber,'')+','+isnull(PartnerRefrenceNo,'')+','+isnull(Description,'')
	where Id = @Id 

	FETCH NEXT FROM invoicecursor
	INTO @Id,@InvoiceNumber,@VatNumber,@DraftNumber,@BillToId
END

CLOSE invoicecursor
DEALLOCATE invoicecursor
select id from cards where EnglishName='ayman'
select id,searchfields from arinvoices
where billtoid='1-712'