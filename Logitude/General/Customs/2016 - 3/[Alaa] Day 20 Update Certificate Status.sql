
declare @invoiceCertificate table (id varchar(15), counterKey integer, lineNumber integer)

 declare @id varchar(15)
 declare @counterKey integer
 declare @lineNumber integer
 declare @attachmentTypeCode varchar(3)
 declare @CertificateNumber varchar(35)
 declare @ReqConfirmationTypeCode varchar(4)
 declare @CertificateExemptionTypeCode varchar(3)
 declare @count integer
 declare @certificateStatusCode varchar(1)
 declare @CustomsAttachmentID varchar(35)
 declare @ResConfirmationTypeCode varchar(4)


Declare InvoiceItems cursor 
for 
select DeclarationId, CounterKey,LineNumber from customs.SupplierInvoiceItems 
	OPEN InvoiceItems FETCH NEXT FROM InvoiceItems INTO @id, @counterKey, @lineNumber 
		WHILE @@FETCH_STATUS = 0
	BEGIN

	 set @certificateStatusCode = null

	Declare ItemCertificate cursor
	for 
	select DeclarationId,InvoiceCounterKey,LineNumber, AttachmentTypeCode , CertificateNumber,ReqConfirmationTypeCode,CertificateExemptionTypeCode, CustomsAttachmentID, ResConfirmationTypeCode from customs.SupplierInvioceItemCertificats where  DeclarationId= @id and InvoiceCounterKey = @counterKey and LineNumber = @lineNumber
	open ItemCertificate FETCH NEXT FROM ItemCertificate INTO @id, @counterKey, @lineNumber, @attachmentTypeCode,@CertificateNumber,@ReqConfirmationTypeCode,@CertificateExemptionTypeCode, @CustomsAttachmentID, @ResConfirmationTypeCode
	while @@FETCH_STATUS = 0
	BEGIN

	--select * from customs.SupplierInvioceItemCertificats where DeclarationId= @id and InvoiceCounterKey = @counterKey and LineNumber = @lineNumber
	if (@attachmentTypeCode ='1' or @attachmentTypeCode = '2')
	begin
	if(@CertificateNumber is null or  @ResConfirmationTypeCode is null or  @ReqConfirmationTypeCode is null or @CertificateExemptionTypeCode is not null or @CustomsAttachmentID is not null )
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

	if(@CertificateExemptionTypeCode is null  or @ReqConfirmationTypeCode is null  or @CertificateNumber is not null or @ResConfirmationTypeCode is not null or @CustomsAttachmentID is not null)
	begin
	set @certificateStatusCode = '2'
		 BREAK;
	end

	else
	begin
	set @certificateStatusCode = '1'
	end

	end



   FETCH NEXT FROM ItemCertificate INTO @id, @counterKey, @lineNumber, @attachmentTypeCode,@CertificateNumber,@ReqConfirmationTypeCode,@CertificateExemptionTypeCode,@CustomsAttachmentID,@ResConfirmationTypeCode
 
	END
	CLOSE ItemCertificate
	DEALLOCATE ItemCertificate

	
     update customs.SupplierInvoiceItems set CertificatesStatusCode = @certificateStatusCode where DeclarationId = @id and CounterKey = @counterKey and LineNumber = @lineNumber

	FETCH NEXT FROM InvoiceItems INTO @id, @counterKey, @lineNumber
	END
	CLOSE InvoiceItems
	DEALLOCATE InvoiceItems
