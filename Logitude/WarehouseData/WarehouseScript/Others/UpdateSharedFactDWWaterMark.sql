	update dw_WaterMarks set LastUpdateDate =  (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ARInvoices ) where TableName = 'ARInvoice'

update dw_WaterMarks set LastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Shipments ) where TableName = 'Shipment'