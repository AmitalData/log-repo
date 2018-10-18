
declare @Tenant as int
declare @QuoteId as varchar(15)
declare @DirectionId as varchar(1)
declare @TransportModeId as varchar(1)
declare @IncludePickUp bit
declare @IncludeDelivery bit
declare @PotentialShipperId as varchar(15)
declare @PotentialConsigneeId as varchar(15)
declare @PotentialCustomerId as varchar(15)

declare @ShipperId as varchar(15)
declare @ShipperName as varchar(60)
declare @ShipperAddressId as varchar(15)
declare @ShipperContactId as varchar(15)
declare @FromAddressId as varchar(15)
declare @FromPartnerId as varchar(15)
declare @FromPartnerAddressId as varchar(15)

declare @ConsigneeId as varchar(15)
declare @ConsigneeName as varchar(60)
declare @ConsigneeAddressId as varchar(15)
declare @ConsigneeContactId as varchar(15)
declare @ToAddressId as varchar(15)
declare @ToPartnerId as varchar(15)
declare @ToPartnerAddressId as varchar(15)

declare @CustomerId as varchar(15)
declare @CustomerName as varchar(60)
declare @CustomerContactId as varchar(15)

	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant, Id, DirectionId, TransportModeId, IncludePickUp, IncludeDelivery, PotentialShipperId, PotentialConsigneeId, PotentialCustomerId
	FROM Quotes
	where PotentialShipperId is not null OR PotentialConsigneeId is not null OR PotentialCustomerId is not null
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @Tenant, @QuoteId, @DirectionId, @TransportModeId, @IncludePickUp, @IncludeDelivery, @PotentialShipperId, @PotentialConsigneeId, @PotentialCustomerId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (@PotentialShipperId is not null)
		begin

			set @FromAddressId = null;
			set @FromPartnerId = null;
			set @FromPartnerAddressId = null;
					
			set @ShipperId = (select CardId from PotentialCustomers where Id = @PotentialShipperId AND Tenant = @Tenant)
			set @ShipperName = (select EnglishName from Cards where Id = @ShipperId AND Tenant = @Tenant)
			set @ShipperAddressId = (select Max(Id) from Addresses where Tenant = @Tenant and AddressTypeId = 'M' and CardId = @ShipperId)
			set @ShipperContactId = (select Max(ContactId) from CardContacts where Tenant = @Tenant and CardId = @ShipperId)
									
			if (@IncludePickUp = 1)
			begin
			set @FromAddressId = @ShipperAddressId
			end

			if (@DirectionId = 'D' AND @TransportModeId = 'I')
			begin
			set @FromPartnerId = @ShipperId
			set @FromPartnerAddressId = @ShipperAddressId
			end

			update Quotes
			set
			PotentialShipperId = null,
			ShipperId = @ShipperId,
			ShipperName = @ShipperName,
			ShipperContactId = @ShipperContactId,
			FromAddressId = @FromAddressId,
			FromPartnerId = @FromPartnerId,
			FromPartnerAddressId =@FromPartnerAddressId
			where Tenant = @Tenant AND Id = @QuoteId

		end

		if (@PotentialConsigneeId is not null)
		begin
			
			set @ToAddressId = null;
			set @ToPartnerId = null;
			set @ToPartnerAddressId = null;

			set @ConsigneeId = (select CardId from PotentialCustomers where Id = @PotentialConsigneeId AND Tenant = @Tenant)
			set @ConsigneeName = (select EnglishName from Cards where Id = @ConsigneeId AND Tenant = @Tenant)
			set @ConsigneeAddressId = (select Max(Id) from Addresses where Tenant = @Tenant and AddressTypeId = 'M' and CardId = @ConsigneeId)
			set @ConsigneeContactId = (select Max(ContactId) from CardContacts where Tenant = @Tenant and CardId = @ConsigneeId)
			
			if (@IncludeDelivery = 1)
			begin
			set @ToAddressId = @ConsigneeAddressId
			end

			if (@DirectionId = 'D' AND @TransportModeId = 'I')
			begin
			set @ToPartnerId = @ConsigneeId
			set @ToPartnerAddressId = @ConsigneeAddressId
			end

			update Quotes
			set
			PotentialConsigneeId = null,
			ConsigneeId = @ConsigneeId,
			ConsigneeName = @ConsigneeName,
			ConsigneeContactId = @ConsigneeContactId,
			ToAddressId = @ToAddressId,
			ToPartnerId = @ToPartnerId,
			ToPartnerAddressId =@ToPartnerAddressId
			where Tenant = @Tenant AND Id = @QuoteId

		end

		if (@PotentialCustomerId is not null)
		begin
			
			set @CustomerId = (select CardId from PotentialCustomers where Id = @PotentialCustomerId AND Tenant = @Tenant)
			set @CustomerName = (select EnglishName from Cards where Id = @CustomerId AND Tenant = @Tenant)
			set @CustomerContactId = (select Max(ContactId) from CardContacts where Tenant = @Tenant and CardId = @CustomerId)			

			update Quotes
			set
			PotentialCustomerId = null,
			CustomerId = @CustomerId,					
			CustomerName = @CustomerName,
			CustomerContactId = @CustomerContactId
			where Tenant = @Tenant AND Id = @QuoteId

		end

	FETCH NEXT FROM QuotesCursor INTO @Tenant, @QuoteId, @DirectionId, @TransportModeId, @IncludePickUp, @IncludeDelivery, @PotentialShipperId, @PotentialConsigneeId, @PotentialCustomerId	
	END
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor

		
	-- Step(2):
	--	if the Cursor executed without errors
	--	then uncomment this and execute it
	--	after done please comment this back
	------------------------------------------------


	--alter table Quotes drop FK_PotentialShipperQuote
	--go

	--DROP INDEX Quotes.IX_FK_PotentialShipperQuote
	--go

	--alter table Quotes drop column PotentialShipperId
	--go

	--alter table Quotes drop FK_PotentialCustomerQuote
	--go

	--DROP INDEX Quotes.IX_FK_PotentialCustomerQuote
	--go

	--alter table Quotes drop column PotentialConsigneeId
	--go

	--alter table Quotes drop FK_QuotePotentialCutomer
	--go

	--DROP INDEX Quotes.IX_FK_QuotePotentialCutomer
	--go

	--alter table Quotes drop column PotentialCustomerId
	--go
 
	-- drop table PotentialCustomers
		