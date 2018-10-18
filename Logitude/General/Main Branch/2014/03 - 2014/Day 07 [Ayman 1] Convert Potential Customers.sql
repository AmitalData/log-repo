

IF OBJECT_ID ('Ayman_GlobalContacts', 'U') is null
begin

	create table Ayman_GlobalContacts
	(
		Id varchar(15) not null,
		Email varchar(50) not null,
		GlobalTenantId int not null,
		InActive bit not null default 0,
		IsUser bit not null default 0,
		InternetAccess bit not null default 0
	);	

	alter table Ayman_GlobalContacts add constraint PK_Ayman_GlobalContacts primary key CLUSTERED (Id asc);
end

declare @Tenant as int
declare @PotentialId as varchar(15)
declare @PotentialCardId as varchar(15)
declare @PotentialEnglishName as varchar(100)
declare @PotentialLocalName as nvarchar(100)
declare @PotentialContactName as nvarchar(40)
declare @PotentialContactEmail as varchar(50)

declare @Address1 as nvarchar(65)
declare @Address2 as nvarchar(65)
declare @CountryId as varchar(15)
declare @StateId as varchar(15)
declare @City as nvarchar(25)
declare @ZipCode as varchar(15)
declare @FaxNumber as varchar(40)
declare @PhoneNumber as varchar(40)

declare @NewCardId as varchar(15)
declare @NewCardCode as varchar(15)
declare @NewCardRankId as varchar(15)
declare @NewAddressId as varchar(15)
declare @NewContactId as varchar(15)
declare @NewCardContactId as varchar(15)
declare @NewContactTenantId as varchar(15)

	DECLARE PotentialCustomersCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant, Id, CardId, EnglishName, LocalName, ContactName, ContactEmail, Address1, Address2, CountryId, StateId, City, ZipCode, FaxNumber, PhoneNumber
	FROM PotentialCustomers
	OPEN PotentialCustomersCursor FETCH NEXT FROM PotentialCustomersCursor INTO @Tenant, @PotentialId, @PotentialCardId, @PotentialEnglishName,@PotentialLocalName,@PotentialContactName,@PotentialContactEmail,@Address1,@Address2,@CountryId,@StateId,@City,@ZipCode,@FaxNumber,@PhoneNumber
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (len(@PotentialEnglishName) > 60)
		begin
			set @PotentialEnglishName = substring(@PotentialEnglishName,1,60)
		end

		set @PotentialContactName = RTRIM(LTRIM(@PotentialContactName))
		set @PotentialContactEmail = RTRIM(LTRIM(@PotentialContactEmail))

		if (@PotentialCardId is null)
		begin
			
			-- Cards
			EXECUTE usp_GetNextTableIdValue @NewCardId OUTPUT,'Card'
			EXECUTE usp_GetNextTableCodeValue @NewCardCode OUTPUT, 'Customer', @Tenant			
			insert into Cards(Id, Tenant, EnglishName, LocalName, Code, PartnerTypeId, CreateDate, SharedLogisticsInvitationStatusCode, InActive,SearchFields)
			values(@NewCardId,@Tenant,@PotentialEnglishName,@PotentialLocalName,@NewCardCode,'PO', GETDATE(), 1, 0, @NewCardCode + ',' + @PotentialEnglishName + ',' + @PotentialLocalName)

			-- Customers
			set @NewCardRankId = (select Id from Ranks where Tenant = @Tenant And Code = '1')
			insert into Customers(Id, Tenant, RankId, StartWorkingManuallySet, IsCustomer, CustomerStatusCode)
			values(@NewCardId, @Tenant, @NewCardRankId, 0, 0, 'POT')

			-- Address
			EXECUTE usp_GetNextTableIdValue @NewAddressId OUTPUT,'Address'
			insert into Addresses(Id, Tenant, CardId, AddressTypeId, Name, Description, Address1,	Address2, CountryId, StateId, City, ZipCode, FaxNumber, PhoneNumber, ATTN, InActive, IsLocalLanguage, SearchFields)		
			values(@NewAddressId, @Tenant, @NewCardId, 'M', @PotentialEnglishName, 'Main Address', @Address1, @Address2, @CountryId, @StateId, @City, @ZipCode, @FaxNumber, @PhoneNumber, null, 0, 0, @Address1 + ',' + @Address2 + ',' + @PotentialEnglishName + ',' + @City + ',' + @ZipCode)

			IF (@PotentialContactName is not null AND @PotentialContactName != '')
			begin

				-- Contacts
				EXECUTE usp_GetNextTableIdValue @NewContactId OUTPUT,'Contact'
				insert into Contacts(Id, Tenant, Email, EnglishName, LocalName ,UserType , InActive, SearchFields)
				values(@NewContactId, @Tenant, @PotentialContactEmail, @PotentialContactName, @PotentialContactName, 'R', 0, @PotentialContactName + ',' + @PotentialContactEmail)

				-- CardContacts
				EXECUTE usp_GetNextTableIdValue @NewCardContactId OUTPUT,'CardContact'
				insert into CardContacts(Id, CardId, ContactId,Tenant)
				values(@NewCardContactId,@NewCardId,@NewContactId,@Tenant)

				-- ContactTenants
				EXECUTE usp_GetNextTableIdValue @NewContactTenantId OUTPUT,'ContactTenant'
				insert into ContactTenants(Id, TenantId, ContactId)
				values(@NewContactTenantId, @Tenant, @NewContactId)

				-- ContactTenantRole 
				-- (Only if Contact.SignupRole == true)

				-- GlobalContact
				if (@PotentialContactEmail is not null AND @PotentialContactEmail != '')
				begin
					insert into Ayman_GlobalContacts(Id, Email, GlobalTenantId, InActive, IsUser, InternetAccess)
					values(@NewContactId, @PotentialContactEmail, @Tenant, 0, 0, 0)

					-- For our Local usage
					insert into [Logitude2-5_Global].[dbo].GlobalContacts(Id, Email, GlobalTenantId, InActive, IsUser, InternetAccess)
					values(@NewContactId, @PotentialContactEmail, @Tenant, 0, 0, 0)
				end							

			end

			update PotentialCustomers set CardId = @NewCardId, InActive = 1 where Tenant = @Tenant And Id = @PotentialId
		end

		else
		begin
						 
			if not exists (select * from Customers where Id = @PotentialCardId AND Tenant = @Tenant)
			begin
				set @NewCardRankId = (select Id from Ranks where Tenant = @Tenant And Code = '1')
				insert into Customers(Id, Tenant, RankId, StartWorkingManuallySet, IsCustomer, CustomerStatusCode)
				values(@PotentialCardId, @Tenant, @NewCardRankId, 0, 0, 'POT')
			end

			if not exists (select * from Addresses where CardId = @PotentialCardId AND AddressTypeId = 'M' AND Tenant = @Tenant)
			begin
				EXECUTE usp_GetNextTableIdValue @NewAddressId OUTPUT,'Address'
				insert into Addresses(Id, Tenant, CardId, AddressTypeId, Name, Description, Address1,	Address2, CountryId, StateId, City, ZipCode, FaxNumber, PhoneNumber, ATTN, InActive, IsLocalLanguage, SearchFields)		
				values(@NewAddressId, @Tenant, @PotentialCardId, 'M', @PotentialEnglishName, 'Main Address', @Address1, @Address2, @CountryId, @StateId, @City, @ZipCode, @FaxNumber, @PhoneNumber, null, 0, 0, @Address1 + ',' + @Address2 + ',' + @PotentialEnglishName + ',' + @City + ',' + @ZipCode)
			end

			If (@PotentialContactName is not null AND @PotentialContactName != '') AND (@PotentialContactEmail is not null AND @PotentialContactEmail != '')
				begin
				if not exists (select * from Contacts where EnglishName = @PotentialContactName and Email = @PotentialContactEmail and Tenant = @Tenant)
				begin
					
					-- Contacts
					EXECUTE usp_GetNextTableIdValue @NewContactId OUTPUT,'Contact'
					insert into Contacts(Id, Tenant, Email, EnglishName, LocalName ,UserType , InActive, SearchFields)
					values(@NewContactId, @Tenant, @PotentialContactEmail, @PotentialContactName, @PotentialContactName, 'R', 0, @PotentialContactName + ',' + @PotentialContactEmail)

					-- CardContacts
					EXECUTE usp_GetNextTableIdValue @NewCardContactId OUTPUT,'CardContact'
					insert into CardContacts(Id, CardId, ContactId,Tenant)
					values(@NewCardContactId,@PotentialCardId,@NewContactId,@Tenant)

					-- ContactTenants
					EXECUTE usp_GetNextTableIdValue @NewContactTenantId OUTPUT,'ContactTenant'
					insert into ContactTenants(Id, TenantId, ContactId)
					values(@NewContactTenantId, @Tenant, @NewContactId)

					-- GlobalContact
					insert into Ayman_GlobalContacts(Id, Email, GlobalTenantId, InActive, IsUser, InternetAccess)
					values(@NewContactId, @PotentialContactEmail, @Tenant, 0, 0, 0)

					-- For our Local usage
					insert into [Logitude2-5_Global].[dbo].GlobalContacts(Id, Email, GlobalTenantId, InActive, IsUser, InternetAccess)
					values(@NewContactId, @PotentialContactEmail, @Tenant, 0, 0, 0)

				end
			end

		end

	FETCH NEXT FROM PotentialCustomersCursor INTO @Tenant, @PotentialId, @PotentialCardId, @PotentialEnglishName,@PotentialLocalName,@PotentialContactName,@PotentialContactEmail,@Address1,@Address2,@CountryId,@StateId,@City,@ZipCode,@FaxNumber,@PhoneNumber
	END
	CLOSE PotentialCustomersCursor
	DEALLOCATE PotentialCustomersCursor

