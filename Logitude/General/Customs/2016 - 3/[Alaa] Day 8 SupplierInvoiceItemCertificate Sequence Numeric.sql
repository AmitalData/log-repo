
DECLARE @id varchar(15)
DECLARE @counterKey integer
DECLARE @lineNumber integer
DECLARE @sequenceNumeric integer 
DECLARE @duplicatedId varchar(15)
DECLARE @duplicatedCounterKey integer
DECLARE @itemCertificateCounterKey integer

Declare CertificateSequenceNumeric cursor 
for 
select DeclarationId, InvoiceCounterKey,LineNumber, ItemCertificateCounterKey from customs.SupplierInvioceItemCertificats order by  DeclarationId, InvoiceCounterKey,LineNumber ASC
	OPEN CertificateSequenceNumeric FETCH NEXT FROM CertificateSequenceNumeric INTO @id, @counterKey, @lineNumber ,@itemCertificateCounterKey
		WHILE @@FETCH_STATUS = 0
	BEGIN

	  if (@id = @duplicatedId and @counterKey = @duplicatedCounterKey)
	  begin
	  set  @sequenceNumeric = @sequenceNumeric +1
	  end
	
	 else 
	 begin
	 set @sequenceNumeric = 1
	 end


     update customs.SupplierInvioceItemCertificats set SequenceNumeric = @sequenceNumeric where DeclarationId = @id and InvoiceCounterKey = @counterKey and LineNumber = @lineNumber and ItemCertificateCounterKey = @itemCertificateCounterKey
	 	
     set  @duplicatedId =@id
	 set @duplicatedCounterKey = @counterKey
	

	FETCH NEXT FROM CertificateSequenceNumeric INTO @id, @counterKey, @lineNumber, @itemCertificateCounterKey
	
	
	END 
	CLOSE CertificateSequenceNumeric
	DEALLOCATE CertificateSequenceNumeric