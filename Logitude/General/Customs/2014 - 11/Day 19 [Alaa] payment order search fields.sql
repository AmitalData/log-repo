Update customs.PaymentOrders
set SearchFields = 
isnull(PaymentNumber,'') + ',' +
 isnull(FirstEntityID,'') + ',' +
 isnull(SecondEntityID,'') + ',' + 
  isnull(ThirdEntityID,'') + ',' 