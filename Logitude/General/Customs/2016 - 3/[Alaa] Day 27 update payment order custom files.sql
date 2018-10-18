
declare @Id varchar(15)
declare @Tenant integer
declare @connections table (paymentOrderId varchar(15))
declare @paymentOrderId varchar(15)
declare @ConnectedEntityId varchar(15)
declare @count integer
declare @AccountingCustomFile varchar(30)


    DECLARE PaymentOrderConnections CURSOR
    FOR
    SELECT Id, Tenant   from customs.PaymentOrders 
    OPEN PaymentOrderConnections FETCH NEXT FROM PaymentOrderConnections INTO @Id, @Tenant
    WHILE @@FETCH_STATUS = 0
	BEGIN
	


	    set @count = (select count(*) from customs.PaymentOrderConnectionTables where PaymentOrderId = @Id and Tenant = @Tenant and ConnectedEntityCode = 'D')--(select COUNT(paymentOrderId) from @connections)
		set @AccountingCustomFile = ( select AccountingCustomFile from customs.PaymentOrders where Id = @Id and Tenant = @Tenant)
		set @ConnectedEntityId=(select top(1) ConnectedEntityId from customs.PaymentOrderConnectionTables where PaymentOrderId=@Id ORDER BY ConnectedEntityId ASC)
	
		if(@count > 1)
		begin
		 update Customs.PaymentOrders set CustomFiles = ((select customfileNo from Customs.Declarations where Id = @ConnectedEntityId and Tenant = @Tenant) + '*') where id = @Id
		end
		else if(@count = 1)
		begin
		 update Customs.PaymentOrders set CustomFiles = (select customfileNo from Customs.Declarations where Id = @ConnectedEntityId and Tenant = @Tenant)where id = @Id

		end
	
	    delete  from @connections
	
	FETCH NEXT FROM PaymentOrderConnections INTO @Id, @Tenant
	

	END 
	CLOSE PaymentOrderConnections
	DEALLOCATE PaymentOrderConnections

	

