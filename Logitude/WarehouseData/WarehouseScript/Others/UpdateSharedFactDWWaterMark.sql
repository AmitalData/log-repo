	
IF (OBJECT_ID ('dw_ARInvoices', 'U')  IS NOT NULL)
BEGIN

 declare @MaxARInvoiceAutomaticLastUpdateDate as datetime
 declare @LastARInvoiceUpdateDate as datetime

 set @LastARInvoiceUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ARInvoice' )
 set @MaxARInvoiceAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoices )
  if(@MaxARInvoiceAutomaticLastUpdateDate > @LastARInvoiceUpdateDate)

 begin
 	update dw_WaterMarks set LastUpdateDate = @MaxARInvoiceAutomaticLastUpdateDate where TableName = 'ARInvoice'

 END

end	


IF (OBJECT_ID ('dw_Shipments', 'U')  IS NOT NULL)
BEGIN


 declare @MaxShipmentAutomaticLastUpdateDate as datetime
 declare @LastShipmentUpdateDate as datetime

 set @LastShipmentUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Shipment' )
 set @MaxShipmentAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Shipments )
  if(@MaxShipmentAutomaticLastUpdateDate > @LastShipmentUpdateDate)

 begin
 	update dw_WaterMarks set LastUpdateDate = @MaxShipmentAutomaticLastUpdateDate where TableName = 'Shipment'

 END



end
	
	
	
	

