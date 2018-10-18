Update Customs.PaymentOrders 
set SearchFields = 
isnull( PaymentNumber ,'') + ',' 



Update p
set SearchFields = 
isnull( p.SearchFields ,'') + ',' +
isnull (d.DeclarationNumber , '') + ','+
isnull (d.CustomFileNo , '') + ','

from Customs.PaymentOrders as p, Customs.Declarations as d , Customs.PaymentOrderConnectionTables as conn 
where conn.PaymentOrderId = p.Id and conn.ConnectedEntityId = d.Id and conn.ConnectedEntityCode ='D'


