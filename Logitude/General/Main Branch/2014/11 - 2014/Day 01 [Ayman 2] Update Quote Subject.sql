


declare @Tenant as int
declare @QuoteId as varchar(15)
declare @IncotermId as varchar(15)
declare @IsSubjectEdited as bit
declare @MySubjectField as nvarchar(60)

declare @IncludePickUp as bit
declare @FromAddressId as varchar(15)
declare @FromAddressZipCode as varchar(15)
declare @FromAddressCity as nvarchar(25)
declare @FromPortId as varchar(15)
declare @FromPortCode as varchar(3)

declare @IncludeDelivery as bit
declare @ToAddressId as varchar(15)
declare @ToAddressZipCode as varchar(15)
declare @ToAddressCity as nvarchar(25)
declare @ToPortId as varchar(15)
declare @ToPortCode as varchar(3)

declare @DirectionId as varchar(1)
declare @TransportModeId as varchar(1)
declare @FromPartnerAddressId as varchar(15)
declare @ToPartnerAddressId as varchar(15)

BEGIN
		DECLARE QuotesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, IncotermId, IncludePickUp, FromAddressId, FromAddressZipCode, FromAddressCity, FromPortId, IncludeDelivery, ToAddressId, ToAddressZipCode, ToAddressCity, ToPortId, IsSubjectEdited, DirectionId, TransportModeId, FromPartnerAddressId, ToPartnerAddressId
		FROM Quotes
		OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @IncotermId, @IncludePickUp, @FromAddressId, @FromAddressZipCode, @FromAddressCity, @FromPortId, @IncludeDelivery, @ToAddressId, @ToAddressZipCode, @ToAddressCity, @ToPortId, @IsSubjectEdited, @DirectionId, @TransportModeId, @FromPartnerAddressId, @ToPartnerAddressId
		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			if (@IsSubjectEdited = 0)
			begin

				set @MySubjectField = ''

				if (@IncotermId is not null)
				begin
					set @MySubjectField = (select Code from Incoterms where Id = @IncotermId AND Tenant = @Tenant)
				end

				if (@DirectionId = 'D' AND @TransportModeId = 'I')
				begin

					if (@FromPartnerAddressId is not null)
					begin

						select @FromAddressZipCode = ZipCode, @FromAddressCity = City
						from Addresses
						where Id = @FromPartnerAddressId AND Tenant = @Tenant

						if (@FromAddressCity is not null)
						begin
							if (@MySubjectField = '') set @MySubjectField = @FromAddressCity
							else set @MySubjectField = @MySubjectField + ' ' + @FromAddressCity
						end

						else if (@FromAddressZipCode is not null)
						begin
							if (@MySubjectField = '') set @MySubjectField = @FromAddressZipCode
							else set @MySubjectField = @MySubjectField + ' ' + @FromAddressZipCode
						end
					end

					if (@ToPartnerAddressId is not null)
					begin

						select @ToAddressZipCode = ZipCode, @ToAddressCity = City
						from Addresses
						where Id = @ToPartnerAddressId AND Tenant = @Tenant
																
						if (@ToAddressCity is not null)
						begin
							if (@MySubjectField = '') set @MySubjectField = @ToAddressCity
							else set @MySubjectField = @MySubjectField + ' > ' + @ToAddressCity
						end								
												
						else if (@ToAddressZipCode is not null)
						begin
							if (@MySubjectField = '') set @MySubjectField = @ToAddressZipCode
							else set @MySubjectField = @MySubjectField + ' > ' + @ToAddressZipCode
						end

					end

				end

				else
				begin
					if (@IncludePickUp = 1)
					begin
					
						if (@FromAddressId is not null)
						begin

							select @FromAddressZipCode = ZipCode, @FromAddressCity = City
							from Addresses
							where Id = @FromAddressId AND Tenant = @Tenant

							if (@FromAddressCity is not null)
							begin
								if (@MySubjectField = '') set @MySubjectField = @FromAddressCity
								else set @MySubjectField = @MySubjectField + ' ' + @FromAddressCity
							end

							else if (@FromAddressZipCode is not null)
							begin
								if (@MySubjectField = '') set @MySubjectField = @FromAddressZipCode
								else set @MySubjectField = @MySubjectField + ' ' + @FromAddressZipCode
							end

						end

						else if (@FromAddressCity is not null)
						begin
								if (@MySubjectField = '') set @MySubjectField = @FromAddressCity
								else set @MySubjectField = @MySubjectField + ' ' + @FromAddressCity	
						end

						else if (@FromAddressZipCode is not null)
						begin
								if (@MySubjectField = '') set @MySubjectField = @FromAddressZipCode
								else set @MySubjectField = @MySubjectField + ' ' + @FromAddressZipCode
						end

					end

					else if (@FromPortId is not null)
					begin					
						set @FromPortCode = (select Code from Ports where Id = @FromPortId)
						if (@FromPortCode is not null)
						begin
								if (@MySubjectField = '') set @MySubjectField = @FromPortCode
								else set @MySubjectField = @MySubjectField + ' ' + @FromPortCode	
						end
					end

					if (@IncludeDelivery = 1)
					begin
					
						if (@ToAddressId is not null)
						begin

							select @ToAddressZipCode = ZipCode, @ToAddressCity = City
							from Addresses
							where Id = @ToPartnerAddressId AND Tenant = @Tenant

							if (@ToAddressCity is not null)
							begin
								if (@MySubjectField = '') set @MySubjectField = @ToAddressCity
								else set @MySubjectField = @MySubjectField + ' > ' + @ToAddressCity							
							end

							else if (@ToAddressZipCode is not null)
							begin
								if (@MySubjectField = '') set @MySubjectField = @ToAddressZipCode
								else set @MySubjectField = @MySubjectField + ' > ' + @ToAddressZipCode
							end

						end

						else if (@ToAddressCity is not null)
						begin
								if (@MySubjectField = '') set @MySubjectField = @ToAddressCity
								else set @MySubjectField = @MySubjectField + ' > ' + @ToAddressCity	
						end

						else if (@ToAddressZipCode is not null)
						begin
								if (@MySubjectField = '') set @MySubjectField = @ToAddressZipCode
								else set @MySubjectField = @MySubjectField + ' > ' + @ToAddressZipCode
						end


					end

					else if (@ToPortId is not null)
					begin					
					set @ToPortCode = (select Code from Ports where Id = @ToPortId)
					if (@ToPortCode is not null)
					begin
							if (@MySubjectField = '') set @MySubjectField = @ToPortCode
							else set @MySubjectField = @MySubjectField + ' > ' + @ToPortCode	
					end
				end
				end
				
				update Quotes set Subject  = @MySubjectField where Id = @QuoteId AND Tenant = @Tenant
			end
		
		FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @IncotermId, @IncludePickUp, @FromAddressId, @FromAddressZipCode, @FromAddressCity, @FromPortId, @IncludeDelivery, @ToAddressId, @ToAddressZipCode, @ToAddressCity, @ToPortId, @IsSubjectEdited, @DirectionId, @TransportModeId, @FromPartnerAddressId, @ToPartnerAddressId
		END				
		CLOSE QuotesCursor
		DEALLOCATE QuotesCursor
END


