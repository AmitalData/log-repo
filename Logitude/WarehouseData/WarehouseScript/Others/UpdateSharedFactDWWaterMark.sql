	
IF (OBJECT_ID ('dw_ARInvoices', 'U')  IS NOT NULL)
BEGIN
	update dw_WaterMarks set LastUpdateDate =  (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoices ) where TableName = 'ARInvoice'

end	


IF (OBJECT_ID ('dw_Shipments', 'U')  IS NOT NULL)
BEGIN
update dw_WaterMarks set LastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Shipments ) where TableName = 'Shipment'

end
	
	
	
	

