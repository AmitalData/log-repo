
declare @Tenant as int
declare @LineId as varchar(15)
declare @ForeginId as varchar(15)
declare @PCId as varchar(1)
declare @APInvoiceId as varchar(15)
declare @APLineNumber as int

--AR
BEGIN 
	DECLARE ARInvoiceLinesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, ReceivableId, Tenant
	FROM ARInvoiceLines
	OPEN ARInvoiceLinesCursor FETCH NEXT FROM ARInvoiceLinesCursor INTO @LineId, @ForeginId, @Tenant    
	WHILE @@FETCH_STATUS = 0
		BEGIN

        if exists (select Id from ShipmentReceivables where Tenant = @Tenant and Id = @ForeginId)
        begin 
		    set @PCId = (select PrepaidCollectId from ShipmentReceivables where Tenant = @Tenant and Id = @ForeginId)
           update ARInvoiceLines set PrepaidCollectId = @PCId where Id = @LineId                           
        end 
		
           FETCH NEXT FROM ARInvoiceLinesCursor INTO @LineId, @ForeginId, @Tenant       
        END
	CLOSE ARInvoiceLinesCursor
	DEALLOCATE ARInvoiceLinesCursor
END

--AP
BEGIN 
	DECLARE APInvoiceLinesCursor CURSOR READ_ONLY
	FOR
	SELECT APInvoiceId, LineNumber, EntityPayableId, Tenant
	FROM APInvoiceLines
	OPEN APInvoiceLinesCursor FETCH NEXT FROM APInvoiceLinesCursor INTO @APInvoiceId, @APLineNumber, @ForeginId, @Tenant    
	WHILE @@FETCH_STATUS = 0
		BEGIN

        if exists (select Id from ShipmentPayables where Tenant = @Tenant and Id = @ForeginId)
        begin 
		    set @PCId = (select PrepaidCollectId from ShipmentPayables where Tenant = @Tenant and Id = @ForeginId)
           update APInvoiceLines set PrepaidCollectId = @PCId where APInvoiceId = @APInvoiceId and LineNumber = @APLineNumber                           
        end 
		
           FETCH NEXT FROM APInvoiceLinesCursor INTO @APInvoiceId, @APLineNumber, @ForeginId, @Tenant         
        END
	CLOSE APInvoiceLinesCursor
	DEALLOCATE APInvoiceLinesCursor
END