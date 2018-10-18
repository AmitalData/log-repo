
declare @CardId varchar(15)
declare @Tenant as int
declare @PartnerTypeId as varchar(5)
declare @ContactId as varchar(15)

declare @ContactName varchar(60)
declare @ContactEmail varchar(70)
declare @ContactPhone varchar(25)

BEGIN 
	DECLARE CardsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, PartnerTypeId, PrimaryContactId
	FROM Cards
	WHERE PrimaryContactId is not null
	OPEN CardsCursor FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant, @PartnerTypeId, @ContactId       
	WHILE @@FETCH_STATUS = 0
		BEGIN
			
			set @ContactName = (select EnglishName from Contacts where id = @ContactId and Tenant = @Tenant)
			set @ContactEmail = (select Email from Contacts where id = @ContactId and Tenant = @Tenant)
			set @ContactPhone = (select BusinessPhone from Contacts where id = @ContactId and Tenant = @Tenant)

			--Agent
			if(@PartnerTypeId = 'AG')
			begin 
				update Agents set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end
			
			--Airline
			if(@PartnerTypeId = 'AL')
			begin 
				update Airlines set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Custom Agent
			if(@PartnerTypeId = 'CG')
			begin 
				update CustomAgents set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Customer
			if(@PartnerTypeId = 'CS' or @PartnerTypeId = 'PO')
			begin 
				update Customers set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Participant
			if(@PartnerTypeId = 'PT')
			begin 
				update Participants set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Shipping Agent
			if(@PartnerTypeId = 'SG')
			begin 
				update ShippingAgents set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Shipping Line
			if(@PartnerTypeId = 'SL')
			begin 
				update ShippingLines set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Trucker
			if(@PartnerTypeId = 'TR')
			begin 
				update Truckers set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Vendor
			if(@PartnerTypeId = 'VD')
			begin 
				update Vendors set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

			--Warehouse
			if(@PartnerTypeId = 'WH')
			begin 
				update Warehouses set PrimaryContactName = @ContactName, PrimaryContactEmail = @ContactEmail, PrimaryContactPhone = @ContactPhone
				where Id = @CardId and Tenant = @Tenant
			end

           FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant, @PartnerTypeId, @ContactId      
        END
	CLOSE CardsCursor
	DEALLOCATE CardsCursor
END