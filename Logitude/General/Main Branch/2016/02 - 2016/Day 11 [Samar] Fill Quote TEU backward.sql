
declare @Tenant as int
declare @QuoteId as varchar(15)

declare @PackageType1Id as varchar(15)
declare @PackageType2Id as varchar(15)
declare @PackageType3Id as varchar(15)
declare @PackageType4Id as varchar(15)
declare @PackageType5Id as varchar(15)

declare @PackageType1Quantity as int
declare @PackageType2Quantity as int
declare @PackageType3Quantity as int
declare @PackageType4Quantity as int
declare @PackageType5Quantity as int

declare @PackageTypeTEU as float
declare @QuoteTEU as float

BEGIN 
	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, PackageType1Id, PackageType2Id, PackageType3Id, PackageType4Id, PackageType5Id, PackageType1Quantity, PackageType2Quantity, PackageType3Quantity, PackageType4Quantity, PackageType5Quantity
	FROM Quotes	
	WHERE ShipmentTypeId = 'FCLD' or ShipmentTypeId ='FTL'
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @PackageType1Id, @PackageType2Id, @PackageType3Id, @PackageType4Id, @PackageType5Id, @PackageType1Quantity, @PackageType2Quantity, @PackageType3Quantity, @PackageType4Quantity, @PackageType5Quantity
	WHILE @@FETCH_STATUS = 0
		BEGIN

		set @QuoteTEU = 0
		set @PackageTypeTEU = 0

		if (@PackageType1Id is not null)
		begin 

			if (@PackageType1Quantity is not null and @PackageType1Quantity <> 0)
			begin

				set @PackageTypeTEU = (select TEU from PackageTypes where Id = @PackageType1Id and Tenant = @Tenant)
				set @QuoteTEU = @QuoteTEU + (@PackageTypeTEU * @PackageType1Quantity)

			end
		end

		if (@PackageType2Id is not null)
		begin 

			if (@PackageType2Quantity is not null and @PackageType2Quantity <> 0)
			begin

				set @PackageTypeTEU = (select TEU from PackageTypes where Id = @PackageType2Id and Tenant = @Tenant)
				set @QuoteTEU = @QuoteTEU + (@PackageTypeTEU * @PackageType2Quantity)

			end
		end

		if (@PackageType3Id is not null)
		begin 

			if (@PackageType3Quantity is not null and @PackageType3Quantity <> 0)
			begin

				set @PackageTypeTEU = (select TEU from PackageTypes where Id = @PackageType3Id and Tenant = @Tenant)
				set @QuoteTEU = @QuoteTEU + (@PackageTypeTEU * @PackageType3Quantity)

			end
		end

		if (@PackageType4Id is not null)
		begin 

			if (@PackageType4Quantity is not null and @PackageType4Quantity <> 0)
			begin

				set @PackageTypeTEU = (select TEU from PackageTypes where Id = @PackageType4Id and Tenant = @Tenant)
				set @QuoteTEU = @QuoteTEU + (@PackageTypeTEU * @PackageType4Quantity)

			end
		end

		if (@PackageType5Id is not null)
		begin 

			if (@PackageType5Quantity is not null and @PackageType5Quantity <> 0)
			begin

				set @PackageTypeTEU = (select TEU from PackageTypes where Id = @PackageType5Id and Tenant = @Tenant)
				set @QuoteTEU = @QuoteTEU + (@PackageTypeTEU * @PackageType5Quantity)

			end
		end

		if(@QuoteTEU <> 0)
		begin
			
			update Quotes set TEU = @QuoteTEU where Id = @QuoteId and Tenant = @Tenant

		end

			FETCH NEXT FROM QuotesCursor INTO @QuoteId, @Tenant, @PackageType1Id, @PackageType2Id, @PackageType3Id, @PackageType4Id, @PackageType5Id, @PackageType1Quantity, @PackageType2Quantity, @PackageType3Quantity, @PackageType4Quantity, @PackageType5Quantity
		END
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor
END


