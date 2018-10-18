Update customs.PaymentOrders
set SearchFields = 
isnull(PaymentNumber,'') + ',' +
 isnull(FirstEntityID,'') + ',' +
 isnull(SecondEntityID,'') + ',' + 
  isnull(ThirdEntityID,'') + ',' 

  Go



Update p
set SearchFields = 
isnull( p.SearchFields ,'') + ',' +
isnull (d.CustomFileNo , '') + ','

from Customs.PaymentOrders as p, Customs.Declarations as d , Customs.PaymentOrderConnectionTables as conn 
where conn.PaymentOrderId = p.Id and conn.ConnectedEntityId = d.Id and conn.ConnectedEntityCode ='D'