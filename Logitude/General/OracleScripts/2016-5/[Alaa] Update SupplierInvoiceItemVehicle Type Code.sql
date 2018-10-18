DECLARE
   c_VehicleId SUPPLIERINVOICEITEMVEHICLES.VEHICLEID%type;
   c_FileNumber SUPPLIERINVOICEITEMVEHICLES.RICHBITFILENUMBER%type;
   c_id SUPPLIERINVOICEITEMVEHICLES.DECLARATIONID%type;
   c_invoiceKey SUPPLIERINVOICEITEMVEHICLES.INVOICECOUNTERKEY%TYPE;
   c_invoiceItemLine SUPPLIERINVOICEITEMVEHICLES.INVOICEITEMLINENUMBER%TYPE;
   c_lineNumber SUPPLIERINVOICEITEMVEHICLES.LINENUMBER%TYPE;
   
   
   
   CURSOR SetVehicleTypeCode is
      SELECT DeclarationId, InvoiceCounterKey, InvoiceItemLineNumber,LineNumber, VehicleId, RichbitFileNumber from SupplierInvoiceItemVehicles;
BEGIN
   OPEN SetVehicleTypeCode;
   LOOP
      FETCH SetVehicleTypeCode into c_id, c_invoiceKey, c_invoiceItemLine,c_lineNumber,c_VehicleId,c_FileNumber;
      EXIT WHEN SetVehicleTypeCode%notfound;

   BEGIN
   if( c_VehicleId is not null OR c_VehicleId <> ''  ) OR  (c_FileNumber  is not null or c_FileNumber <> '')  THEN
   begin
	 update SupplierInvoiceItemVehicles set VehicleTypeCode ='ZZZ'  where DeclarationId = c_id and InvoiceCounterKey = c_invoiceKey and InvoiceItemLineNumber = c_invoiceItemLine and LineNumber = c_lineNumber;

	end;
else

begin
	 update SupplierInvoiceItemVehicles set VehicleTypeCode ='CN' where DeclarationId = c_id  and InvoiceCounterKey = c_invoiceKey and InvoiceItemLineNumber = c_invoiceItemLine  and LineNumber = c_lineNumber;
end;
  END IF;
   END;

   END LOOP;
   CLOSE SetVehicleTypeCode;
END;

