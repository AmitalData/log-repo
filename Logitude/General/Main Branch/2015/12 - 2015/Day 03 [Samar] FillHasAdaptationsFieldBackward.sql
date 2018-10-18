
declare @Tenant as int
declare @AirlineId as varchar(15)
declare @AirlineCode as varchar(2)
declare @HasAdaptations as bit

BEGIN 
		DECLARE AirlinesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Code
		FROM Cards
		where Tenant = 0 ANd PartnerTypeId = 'AL'
		OPEN AirlinesCursor FETCH NEXT FROM AirlinesCursor INTO @AirlineId, @Tenant, @AirlineCode
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @HasAdaptations = 0

			if exists (select * from Commodities where Tenant = @Tenant and AirlineId = @AirlineId)
				set @HasAdaptations = 1

			else if exists (select * from IATACodes where AirlineId = @AirlineId)
				set @HasAdaptations = 1

			else if exists (select * from BookingProducts where AirlineId = @AirlineId)
				set @HasAdaptations = 1

			else if exists (select * from AWBSpecialHandlingCodes where AirlineId = @AirlineId)
				set @HasAdaptations = 1

			update Airlines set HasAdaptations = @HasAdaptations where Id = @AirlineId

			FETCH NEXT FROM AirlinesCursor INTO @AirlineId, @Tenant, @AirlineCode
			END
		CLOSE AirlinesCursor
		DEALLOCATE AirlinesCursor
END

