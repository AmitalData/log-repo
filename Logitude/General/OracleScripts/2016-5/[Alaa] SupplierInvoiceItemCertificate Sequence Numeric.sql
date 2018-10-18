DECLARE

   c_id SupplierInvioceItemCertificats.DECLARATIONID%type;
   c_invoiceKey SupplierInvioceItemCertificats.INVOICECOUNTERKEY%TYPE;
   c_lineNumber SupplierInvioceItemCertificats.LineNumber%TYPE;
  c_duplicatedId VARCHAR2(15);
   c_duplicatedCounterKey integer;
   c_itemCertificateCounterKey SupplierInvioceItemCertificats.ItemCertificateCounterKey%Type;
   c_sequenceNumeric SUPPLIERINVIOCEITEMCERTIFICATS.SEQUENCENUMERIC%type;
 
   
   CURSOR CertificateSequenceNumeric  is
      SELECT DeclarationId, InvoiceCounterKey,LineNumber, ItemCertificateCounterKey from SupplierInvioceItemCertificats order by  DeclarationId, InvoiceCounterKey,LineNumber ASC;
BEGIN
   OPEN CertificateSequenceNumeric;
   LOOP
      FETCH CertificateSequenceNumeric into c_id, c_invoiceKey, c_lineNumber,c_itemCertificateCounterKey;
      EXIT WHEN CertificateSequenceNumeric%notfound;
      BEGIN
        
         if (c_id = c_duplicatedId and c_invoiceKey = c_duplicatedCounterKey) then
	  begin
	     c_sequenceNumeric := c_sequenceNumeric +1;
	  end;
    
    else
    begin
    c_sequenceNumeric :=1;
    end;
    end if;
    
    update SupplierInvioceItemCertificats set SequenceNumeric = c_sequenceNumeric  where DeclarationId = c_id and InvoiceCounterKey =  c_invoiceKey  and LineNumber =c_lineNumber and ItemCertificateCounterKey = c_itemCertificateCounterKey;  
	 
   c_duplicatedId  := c_id;
	 c_duplicatedCounterKey  := c_invoiceKey;
        
        
    END;
   END LOOP;
   CLOSE CertificateSequenceNumeric;
END;
