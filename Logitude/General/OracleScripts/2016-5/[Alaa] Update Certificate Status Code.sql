

DECLARE 

  c_id  SupplierInvoiceItems.DeclarationId%type;
  c_counterKey SupplierInvoiceItems.CounterKey%type;
  c_lineNumber SupplierInvoiceItems.LineNumber%type;
  c_attachmentTypeCode SupplierInvioceItemCertificats.AttachmentTypeCode%type;
  c_certificateNumber SupplierInvioceItemCertificats.CertificateNumber%type;
  c_reqConfirmationTypeCode SupplierInvioceItemCertificats.ReqConfirmationTypeCode%type;
  c_certificateExemptionTypeCode SupplierInvioceItemCertificats.CertificateExemptionTypeCode%type;
  c_count number;
  c_certificateStatusCode varchar2(1);
  c_customsAttachmentId SupplierInvioceItemCertificats.CustomsAttachmentID%type;
c_resconfirmationTypeCode SupplierInvioceItemCertificats.ResConfirmationTypeCode%type;


  CURSOR InvoiceItems  is
     select DeclarationId, CounterKey,LineNumber from SupplierInvoiceItems;
     
     
     CURSOR ItemCertificate is
	select DeclarationId,InvoiceCounterKey,LineNumber, AttachmentTypeCode , CertificateNumber,ReqConfirmationTypeCode,CertificateExemptionTypeCode, CustomsAttachmentID, ResConfirmationTypeCode from SupplierInvioceItemCertificats
  where  DeclarationId= c_id and InvoiceCounterKey =c_counterKey and LineNumber = c_lineNumber;
  
  BEGIN
  
     OPEN InvoiceItems;
     LOOP 
      FETCH InvoiceItems  into c_id, c_counterKey, c_lineNumber;
      EXIT WHEN InvoiceItems%notfound;
    
   
   BEGIN
  
      c_certificateStatusCode := NULL;
	 open ItemCertificate;
   loop
    FETCH ItemCertificate  into c_id, c_counterKey, c_lineNumber,c_attachmentTypeCode,c_CertificateNumber,c_ReqConfirmationTypeCode,c_CertificateExemptionTypeCode, c_CustomsAttachmentID, c_ResConfirmationTypeCode;
      EXIT WHEN ItemCertificate%notfound;
      
      begin
   
   if(c_attachmentTypeCode ='1' or c_attachmentTypeCode = '2') then
   
   begin
   
   if(c_CertificateNumber is null or c_ResConfirmationTypeCode is null or  c_ReqConfirmationTypeCode is null or c_CertificateExemptionTypeCode is not null or c_CustomsAttachmentID is not null ) then
	begin
	 c_certificateStatusCode := '2';
	 EXIT;
	end;
   
   
   
   else
   

   begin

	c_certificateStatusCode :=  '1';
	end;
  
   
   end if;
   end;
   
   end if;
   
   
   If (c_attachmentTypeCode = '4') then
	begin 

	if(c_CertificateExemptionTypeCode is null  or c_ReqConfirmationTypeCode is null  or c_CertificateNumber is not null or c_ResConfirmationTypeCode is not null or c_CustomsAttachmentID is not null) then
	begin
  
  c_certificateStatusCode := '2';
	
  
		 EXIT;
	end;

	else
	begin
	c_certificateStatusCode := '1';
	end;

	end if;
  end;
  end if;
   
   
   
      end;
     
      
      end loop;
      close ItemCertificate;
     
           update SupplierInvoiceItems set CertificatesStatusCode = c_certificateStatusCode where DeclarationId = c_id and CounterKey = c_counterKey and LineNumber = c_lineNumber;

 



      END;
      END LOOP;
      CLOSE InvoiceItems;
      
      END;




