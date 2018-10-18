Update Customs.PaymentOrders 
set SearchFields = 
isnull( PaymentNumber ,'') + ',' +
isnull( FirstEntityID ,'') + ',' +
isnull( SecondEntityID ,'') + ',' +
isnull( ThirdEntityID ,'') + ',' 