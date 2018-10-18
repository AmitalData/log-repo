alter table [Customs].[PaymentOrders] add ImporterId varchar(15) null

alter table [Customs].[PaymentOrders] add constraint [PaymentOrder_Client] foreign key ([ImporterId]) references [Customs].[Clients]([Id]);
