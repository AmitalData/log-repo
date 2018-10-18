

SET NOCOUNT ON

declare @MySearchFields as nvarchar(1500)

declare @PortsTable table
(
  Id varchar(15) not null
)

declare @PartnersTable table
(
  Id varchar(15) not null
)

declare @ReferencesTable table
(
  Reference varchar(50) not null
)

-- Quote fields
BEGIN
declare @Id as varchar(15)
declare @Tenant as int
declare @QuoteNumber as varchar(15)
declare @Subject as varchar(60)
declare @Notes as nvarchar(500)
END

-- Ports Firlds
BEGIN
declare @PortId as varchar(15)
declare @PortCode as varchar(3)
declare @PortName as varchar(40)

declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
END

-- Partners Fields
BEGIN
declare @PartnerId as varchar(15)
declare @PartnerName as varchar(60)
declare @PartnerReference1 as varchar(50)
declare @PartnerReference2 as varchar(50)

declare @ShipperId as varchar(15)
declare @ShipperReference1 as varchar(50)
declare @ShipperReference2 as varchar(50)

declare @ConsigneeId as varchar(15)
declare @ConsigneeReference1 as varchar(50)
declare @ConsigneeReference2 as varchar(50)

declare @CustomerId as varchar(15)
declare @CustomerContactId as varchar(15)
declare @ContactEmail as varchar(70)
declare @CustomerReference1 as varchar(50)
declare @CustomerReference2 as varchar(50)

declare @NotifyId as varchar(15)
END

-- Carrier Fields
BEGIN
declare @MainCarriageCarrierId as varchar(15)
declare @MainCarriageCarrierCode as varchar(15)
declare @MainCarriageCarrierName as varchar(60)
END

BEGIN
		DECLARE QuotesCursor CURSOR READ_ONLY
		FOR
		SELECT
		Id, Tenant, QuoteNumber, [Subject], Notes,		
		FromPortId, ToPortId, NotifyId, MainCarriageCarrierId,
		ShipperId, ShipperReference1, ShipperReference2,
		ConsigneeId, ConsigneeReference1, ConsigneeReference2,
		CustomerId, CustomerContactId, CustomerReference1, CustomerReference2	
		
		FROM Quotes
		OPEN QuotesCursor FETCH NEXT FROM QuotesCursor

		INTO 
		@Id, @Tenant, @QuoteNumber, @Subject, @Notes,		
		@FromPortId, @ToPortId, @NotifyId, @MainCarriageCarrierId,
		@ShipperId, @ShipperReference1, @ShipperReference2,
		@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
		@CustomerId, @CustomerContactId, @CustomerReference1, @CustomerReference2
		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			set @MySearchFields = ''

			delete from @PortsTable
			delete from @PartnersTable
			delete from @ReferencesTable

			-- Fields
			BEGIN

			if (@QuoteNumber is not null AND @QuoteNumber <> '')
			begin
				if (@MySearchFields = '') set @MySearchFields = @QuoteNumber
				else set @MySearchFields = @MySearchFields + ',' + @QuoteNumber
			end

			if (@Subject is not null AND @Subject <> '')
			begin
				if (@MySearchFields = '') set @MySearchFields = @Subject
				else set @MySearchFields = @MySearchFields + ',' + @Subject
			end
			
			if (@Notes is not null AND @Notes <> '')
			begin
				if (@MySearchFields = '') set @MySearchFields = @Notes
				else set @MySearchFields = @MySearchFields + ',' + @Notes
			end
			END

			-- Carrier
			if (@MainCarriageCarrierId is not null)
			begin
				select
				@MainCarriageCarrierCode = Cards.Code,
				@MainCarriageCarrierName = Cards.EnglishName				
				from Airlines join Cards on Airlines.Id = Cards.Id
				where Airlines.Tenant = @Tenant AND Airlines.Id = @MainCarriageCarrierId

				if (@MainCarriageCarrierCode is not null AND @MainCarriageCarrierCode <> '')
				begin
					if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierCode
					else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierCode	
				end

				if (@MainCarriageCarrierName is not null AND @MainCarriageCarrierName <> '')
				begin
					if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierName
					else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierName	
				end				
			end

			-- Ports
			BEGIN
			set @PortId = @FromPortId
			if (@PortId is not null)
			if not exists (select * from @PortsTable where Id = @PortId)
			begin
				insert into @PortsTable(Id) values (@PortId)

				select
				@PortCode = Code,
				@PortName = EnglishName
				from Ports
				where Id = @PortId AND Tenant = @Tenant

				if (@PortCode is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @PortCode
					else set @MySearchFields = @MySearchFields + ',' + @PortCode	
				end

				if (@PortName is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @PortName
					else set @MySearchFields = @MySearchFields + ',' + @PortName	
				end
			end

			set @PortId = @ToPortId
			if (@PortId is not null)
			if not exists (select * from @PortsTable where Id = @PortId)
			begin
				insert into @PortsTable(Id) values (@PortId)

				select
				@PortCode = Code,
				@PortName = EnglishName				
				from Ports
				where Id = @PortId AND Tenant = @Tenant

				if (@PortCode is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @PortCode
					else set @MySearchFields = @MySearchFields + ',' + @PortCode	
				end

				if (@PortName is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @PortName
					else set @MySearchFields = @MySearchFields + ',' + @PortName	
				end
			end
			END			

			-- Partners
			BEGIN
			set @PartnerId = @ShipperId
			set @PartnerReference1 = @ShipperReference1
			set @PartnerReference2 = @ShipperReference2
			if (@PartnerId is not null)
			begin

				if not exists (select * from @PartnersTable where Id = @PartnerId)
				begin
					insert into @PartnersTable(Id) values (@PartnerId)
					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
					if (@PartnerName is not null)
					begin
						if (@MySearchFields = '') set @MySearchFields = @PartnerName
						else set @MySearchFields = @MySearchFields + ',' + @PartnerName	
					end
				end

				if (@PartnerReference1 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference1)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1	
				end

				if (@PartnerReference2 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference2)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2	
				end
			end

			set @PartnerId = @ConsigneeId
			set @PartnerReference1 = @ConsigneeReference1
			set @PartnerReference2 = @ConsigneeReference2
			if (@PartnerId is not null)
			begin

				if not exists (select * from @PartnersTable where Id = @PartnerId)
				begin
					insert into @PartnersTable(Id) values (@PartnerId)
					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
					if (@PartnerName is not null)
					begin
						if (@MySearchFields = '') set @MySearchFields = @PartnerName
						else set @MySearchFields = @MySearchFields + ',' + @PartnerName	
					end
				end

				if (@PartnerReference1 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference1)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1	
				end

				if (@PartnerReference2 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference2)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2	
				end
			end

			set @PartnerId = @CustomerId
			set @PartnerReference1 = @CustomerReference1
			set @PartnerReference2 = @CustomerReference2
			if (@PartnerId is not null)
			begin

				if not exists (select * from @PartnersTable where Id = @PartnerId)
				begin
					insert into @PartnersTable(Id) values (@PartnerId)
					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
					if (@PartnerName is not null)
					begin
						if (@MySearchFields = '') set @MySearchFields = @PartnerName
						else set @MySearchFields = @MySearchFields + ',' + @PartnerName	
					end
				end

				if (@PartnerReference1 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference1)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1	
				end

				if (@PartnerReference2 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference2)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2	
				end

				if(@CustomerContactId is not null)
				begin
					select @ContactEmail = Email from Contacts where Id = @CustomerContactId AND Tenant = @Tenant
					if (@MySearchFields = '') set @MySearchFields = @ContactEmail
					else set @MySearchFields = @MySearchFields + ',' + @ContactEmail

				end
			end

			set @PartnerId = @NotifyId
			set @PartnerReference1 = null
			set @PartnerReference2 = null
			if (@PartnerId is not null)
			begin

				if not exists (select * from @PartnersTable where Id = @PartnerId)
				begin
					insert into @PartnersTable(Id) values (@PartnerId)
					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
					if (@PartnerName is not null)
					begin
						if (@MySearchFields = '') set @MySearchFields = @PartnerName
						else set @MySearchFields = @MySearchFields + ',' + @PartnerName	
					end
				end

				if (@PartnerReference1 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference1)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1	
				end

				if (@PartnerReference2 is not null)
				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
				begin
					insert into @ReferencesTable(Reference) values (@PartnerReference2)
					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2	
				end
			end
			END

			--print  @MySearchFields
			update Quotes set SearchFields = @MySearchFields where Id = @Id AND Tenant = @Tenant

		FETCH NEXT FROM QuotesCursor
		INTO 
		@Id, @Tenant, @QuoteNumber, @Subject, @Notes,		
		@FromPortId, @ToPortId, @NotifyId, @MainCarriageCarrierId,
		@ShipperId, @ShipperReference1, @ShipperReference2,
		@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
		@CustomerId, @CustomerContactId, @CustomerReference1, @CustomerReference2
		END				
		CLOSE QuotesCursor
		DEALLOCATE QuotesCursor
END

SET NOCOUNT OFF
