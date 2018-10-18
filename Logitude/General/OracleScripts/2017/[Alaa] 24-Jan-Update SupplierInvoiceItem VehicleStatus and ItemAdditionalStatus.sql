DECLARE

 
  c_id SupplierInvoiceItems.DeclarationId%type;
  c_counterKey SupplierInvoiceItems.CounterKey%type;
c_lineNumber SupplierInvoiceItems.LineNumber%type;
c_tenant SupplierInvoiceItems.Tenant%type;
c_statisticQuantity  SupplierInvoiceItems.StatisticQuantity%type;
 c_statisticQuantityType SupplierInvoiceItems.StatisticQuantityType%type;
c_additionalQuantity SupplierInvoiceItems.AdditionalQuantity%type;
c_additionalQuantityType SupplierInvoiceItems.AdditionalQuantityType%type;
c_customsBookType SupplierInvoiceItems.CustomsBookTypeCode%type;
c_pereferenceDocument SupplierInvoiceItems.PreferenceDocumentNumber%type;
c_actualInvoiceLine  SupplierInvoiceItems.ActualInvoiceLines%type;
c_deferredCustomsTax SupplierInvoiceItems.DeferredCustomsTax%type;
c_deferredPurchaseTax SupplierInvoiceItems.DeferredPurchaseTax%type;
c_salesTaxExemption SupplierInvoiceItems.SalesTaxExemptionTypeCode%type;
c_TaxExemptCode SupplierInvoiceItems.TaxExemptCode%type;
 c_optionalTamaPercentage SupplierInvoiceItems.OptionalTamaPercentage%type;
  c_nonCustomsItemPrice SupplierInvoiceItems.NonCustomsItemPrice%type;
c_nonCustomsItemPriceCode SupplierInvoiceItems.NonCustomsItemPriceCurCode%type;
c_WholeSalesItemPrice SupplierInvoiceItems.WholeSaleItemPrice%type;
c_WholeSalesItemPriceCode SupplierInvoiceItems.WholeSaleItemPriceCurrencyCode%type;
  c_manufactureIdentifire SupplierInvoiceItems.ManufactureIdentifier%type;
c_dangerousClassificationCode SupplierInvoiceItems.DangerousClassificationCode%type;
 c_dangerousPakingGroup  SupplierInvoiceItems.DangerousPackingGroupTypeCode%type;
  c_used  SUPPLIERINVOICEITEMS.ISUSED%type;
c_AdditionalItemStatus number(1,0);
c_modificationsCount integer;
c_processCount integer;
c_connectedDeclarationsCount integer;
c_serialNumbersCount integer;
c_descriptionsCount integer;
c_productIdentificationsCount integer;
c_leviesCount integer;
c_vehiclesCount integer;
   
   CURSOR InvoiceItemsStatus  is
      SELECT  DeclarationId, CounterKey,LineNumber,Tenant,StatisticQuantity, StatisticQuantityType,AdditionalQuantity,AdditionalQuantityType,CustomsBookTypeCode,PreferenceDocumentNumber,
ActualInvoiceLines,DeferredCustomsTax,DeferredPurchaseTax,SalesTaxExemptionTypeCode,TaxExemptCode,OptionalTamaPercentage,NonCustomsItemPrice,NonCustomsItemPriceCurCode,WholeSaleItemPrice,WholeSaleItemPriceCurrencyCode,
ManufactureIdentifier,DangerousClassificationCode,DangerousPackingGroupTypeCode, IsUsed from SupplierInvoiceItems;
BEGIN
   OPEN InvoiceItemsStatus;
   LOOP
      FETCH InvoiceItemsStatus into c_id, c_counterKey, c_lineNumber,c_tenant,c_statisticQuantity, c_statisticQuantityType,c_additionalQuantity,c_additionalQuantityType,c_customsBookType,
	c_pereferenceDocument,c_actualInvoiceLine,c_deferredCustomsTax,c_deferredPurchaseTax,c_salesTaxExemption,c_TaxExemptCode,c_optionalTamaPercentage,c_nonCustomsItemPrice,c_nonCustomsItemPriceCode,c_WholeSalesItemPrice,
	c_nonCustomsItemPriceCode,c_manufactureIdentifire,c_dangerousClassificationCode,c_dangerousPakingGroup,c_used;
      EXIT WHEN InvoiceItemsStatus%notfound;
      BEGIN
      c_AdditionalItemStatus := 0;
  
       select count(*) into c_modificationsCount  from SupplierInvoiceItemsMods where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and LineNumber = c_lineNumber and Tenant = c_tenant;
       select count(*) into c_processCount  from SupplierInvoiceItemProcesTypes where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;
       select count(*) into c_connectedDeclarationsCount  from SupplierInvoiceItemsConDeclars where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;
       select count(*) into c_serialNumbersCount  from SupplierInvoiceItemsSerialNums where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;
       select count(*) into c_descriptionsCount  from SupplierInvoiceItemsDescripts where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;
       select count(*) into c_productIdentificationsCount  from SupplierInvoiceItemsProdIdents  where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;
      select count(*) into c_productIdentificationsCount  from SupplierInvoiceItemsProdIdents  where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;
       select count(*) into c_leviesCount  from SupplierInvoiceItemsLevies  where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;
       select count(*) into c_vehiclesCount  from SupplierInvoiceItemVehicles  where DeclarationId = c_Id and InvoiceCounterKey = c_counterKey and InvoiceItemLineNumber = c_lineNumber and Tenant = c_tenant;


         if (c_statisticQuantity is not null or  c_statisticQuantityType  is not null or c_additionalQuantity  is not null or c_additionalQuantityType  is not null or c_customsBookType  is not null or
	c_pereferenceDocument   is not null or c_actualInvoiceLine  is not null or c_deferredCustomsTax  is not null or c_deferredPurchaseTax  is not null or  c_salesTaxExemption  is not null or  c_TaxExemptCode  is not null or   c_optionalTamaPercentage  is not null or c_nonCustomsItemPrice  is not null or c_nonCustomsItemPriceCode  is not null or  c_WholeSalesItemPrice  is not null or
	c_nonCustomsItemPriceCode   is not null or  c_manufactureIdentifire  is not null or  c_dangerousClassificationCode   is not null or c_dangerousPakingGroup  is not null or c_used =1 ) then
	begin
	  c_AdditionalItemStatus := 1;

	end;
  end if;

	 if (c_modificationsCount >0 or c_processCount >0 or c_connectedDeclarationsCount >0 or c_serialNumbersCount >0 or c_descriptionsCount >0 or c_productIdentificationsCount >0 or c_leviesCount >0) then
	begin
	  c_AdditionalItemStatus :=1;
	end;

  end if;
		
           update SUPPLIERINVOICEITEMS  set ItemAdditionalStatus =c_AdditionalItemStatus where DeclarationId = c_id and CounterKey =c_counterKey and LineNumber = c_lineNumber and Tenant = c_tenant;

if(c_vehiclesCount >0) then
	begin

			update SUPPLIERINVOICEITEMS  set VehicleStatus = 1 where DeclarationId = c_id and CounterKey =c_counterKey and LineNumber = c_lineNumber and Tenant = c_tenant;
	end;

  
  else  
	begin 
	
	update SUPPLIERINVOICEITEMS set VehicleStatus = 0 where DeclarationId = c_id and CounterKey = c_counterKey and LineNumber = c_lineNumber and Tenant = c_tenant;

	end ;
    end if;
    END;
   END LOOP;
   CLOSE InvoiceItemsStatus;
END;
