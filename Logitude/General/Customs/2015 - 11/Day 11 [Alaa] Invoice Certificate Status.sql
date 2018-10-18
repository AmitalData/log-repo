
declare @invoiceCertificate table (id varchar(15), counterKey integer, lineNumber integer)




 declare @id varchar(15)
 declare @counterKey integer
 declare @lineNumber integer
 declare @attachmentTypeCode varchar(3)
 declare @CertificateNumber varchar(35)
 declare @ConfirmationTypeCode varchar(4)
 declare @CertificateExemptionTypeCode varchar(3)
 declare @count integer
 declare @certificateStatusCode varchar(1)


Declare InvoiceItems cursor 
for 
select DeclarationId, CounterKey,LineNumber from customs.SupplierInvoiceItems 
	OPEN InvoiceItems FETCH NEXT FROM InvoiceItems INTO @id, @counterKey, @lineNumber 
		WHILE @@FETCH_STATUS = 0
	BEGIN

	 set @certificateStatusCode = null

	Declare ItemCertificate cursor
	for 
	select DeclarationId,InvoiceCounterKey,LineNumber, AttachmentTypeCode , CertificateNumber,ReqConfirmationTypeCode,CertificateExemptionTypeCode from customs.SupplierInvioceItemCertificats where  DeclarationId= @id and InvoiceCounterKey = @counterKey and LineNumber = @lineNumber
	open ItemCertificate FETCH NEXT FROM ItemCertificate INTO @id, @counterKey, @lineNumber, @attachmentTypeCode,@CertificateNumber,@ConfirmationTypeCode,@CertificateExemptionTypeCode
	while @@FETCH_STATUS = 0
	BEGIN

	--select * from customs.SupplierInvioceItemCertificats where DeclarationId= @id and InvoiceCounterKey = @counterKey and LineNumber = @lineNumber
	if (@attachmentTypeCode ='1' or @attachmentTypeCode = '2')
	begin
	if(@CertificateNumber is null or @CertificateExemptionTypeCode = '' or @ConfirmationTypeCode is null )
	begin
	set @certificateStatusCode = '2'
	 BREAK;
	end

	else 

	begin

	set @certificateStatusCode = '1'
	end
	end

	If (@attachmentTypeCode = '4')
	begin 

	if(@CertificateExemptionTypeCode is null)
	begin
	set @certificateStatusCode = '2'
		 BREAK;
	end

	else
	begin
	set @certificateStatusCode = '1'
	end

	end



   FETCH NEXT FROM ItemCertificate INTO @id, @counterKey, @lineNumber, @attachmentTypeCode,@CertificateNumber,@ConfirmationTypeCode,@CertificateExemptionTypeCode
 
	END
	CLOSE ItemCertificate
	DEALLOCATE ItemCertificate

	
     update customs.SupplierInvoiceItems set CertificatesStatusCode = @certificateStatusCode where DeclarationId = @id and CounterKey = @counterKey and LineNumber = @lineNumber

	FETCH NEXT FROM InvoiceItems INTO @id, @counterKey, @lineNumber
	END
	CLOSE InvoiceItems
	DEALLOCATE InvoiceItems
