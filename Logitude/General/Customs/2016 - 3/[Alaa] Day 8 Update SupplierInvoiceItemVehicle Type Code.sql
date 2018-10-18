DECLARE @VehicleId varchar(15)
DECLARE @RichbitFileNumber varchar(10)
DECLARE @id varchar(15)
DECLARE @InvoiceCounterKey integer
DECLARE @InvoiceItemLineNumber integer
DECLARE @LineNumber integer

    DECLARE SetVehicleTypeCode CURSOR
    FOR
    SELECT  DeclarationId, InvoiceCounterKey, InvoiceItemLineNumber,LineNumber, VehicleId, RichbitFileNumber from Customs.SupplierInvoiceItemVehicles 
    OPEN SetVehicleTypeCode FETCH NEXT FROM SetVehicleTypeCode INTO @id,@InvoiceCounterKey,@InvoiceItemLineNumber,@LineNumber, @VehicleId,@RichbitFileNumber
    WHILE @@FETCH_STATUS =0

    BEGIN

	if((@VehicleId is not null or @VehicleId != '') or (@RichbitFileNumber is not null or @RichbitFileNumber != ''))
	
	begin
	 update customs.SupplierInvoiceItemVehicles set VehicleTypeCode ='ZZZ'  where DeclarationId = @id and InvoiceCounterKey = @InvoiceCounterKey and InvoiceItemLineNumber = @InvoiceItemLineNumber and LineNumber = @LineNumber

	end


	else
	begin
	 update customs.SupplierInvoiceItemVehicles set VehicleTypeCode ='CN' where DeclarationId = @id and InvoiceCounterKey = @InvoiceCounterKey and InvoiceItemLineNumber = @InvoiceItemLineNumber and LineNumber = @LineNumber
	end

	FETCH NEXT FROM SetVehicleTypeCode INTO @id,@InvoiceCounterKey,@InvoiceItemLineNumber,@LineNumber, @VehicleId,@RichbitFileNumber
 
	END
	CLOSE SetVehicleTypeCode
	DEALLOCATE SetVehicleTypeCode

