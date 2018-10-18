update Customers set ActivityWatch = 0
go

declare @Tenant as int
declare @CustomerId as varchar(15)
declare @IsExists as bit
declare @CommitmentChargeableWeight as decimal
declare @CommitmentNumberOfShipments as decimal
declare @CommitmentRevenue as decimal
declare @CommitmentTEU as decimal
declare @PotentialChargeableWeight as decimal
declare @PotentialNumberOfShipments as decimal
declare @PotentialRevenue as decimal
declare @PotentialTEU as decimal

-- customer products
BEGIN
	DECLARE CustomerProductsCursor CURSOR READ_ONLY
	FOR
	SELECT CustomerId, Tenant, CommitmentChargeableWeight, CommitmentNumberOfShipments, CommitmentRevenue, CommitmentTEU, PotentialChargeableWeight, PotentialNumberOfShipments, PotentialRevenue, PotentialTEU
	FROM CustomerProducts
	OPEN CustomerProductsCursor FETCH NEXT FROM CustomerProductsCursor INTO @CustomerId, @Tenant, @CommitmentChargeableWeight, @CommitmentNumberOfShipments, @CommitmentRevenue, @CommitmentTEU, @PotentialChargeableWeight, @PotentialNumberOfShipments,	@PotentialRevenue, @PotentialTEU 
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		set @IsExists = 0

		if (@CommitmentChargeableWeight is not null or @CommitmentChargeableWeight != 0)
		begin
			set @IsExists = 1
		end

		else if (@CommitmentNumberOfShipments is not null or @CommitmentNumberOfShipments != 0)
		begin
			set @IsExists = 1
		end

		else if(@CommitmentRevenue is not null or @CommitmentRevenue != 0)
		begin
			set @IsExists = 1
		end

		else if (@CommitmentTEU is not null or @CommitmentTEU != 0)
		begin
			set @IsExists = 1
		end

		else if (@PotentialChargeableWeight is not null or @PotentialChargeableWeight != 0)
		begin
			set @IsExists = 1
		end

		else if (@PotentialNumberOfShipments is not null or @PotentialNumberOfShipments != 0)
		begin
			set @IsExists = 1
		end

		else if (@PotentialRevenue is not null or @PotentialRevenue != 0)
		begin
			set @IsExists = 1
		end

		else if (@PotentialTEU is not null or @PotentialTEU != 0)
		begin
			set @IsExists = 1
		end

		if (@IsExists = 1)
		begin
			
			update Customers
			set ActivityWatch = 1
			where Id = @CustomerId and Tenant = @Tenant

		end
	
	FETCH NEXT FROM CustomerProductsCursor INTO @CustomerId, @Tenant, @CommitmentChargeableWeight, @CommitmentNumberOfShipments, @CommitmentRevenue, @CommitmentTEU, @PotentialChargeableWeight, @PotentialNumberOfShipments,	@PotentialRevenue, @PotentialTEU
	END
	CLOSE CustomerProductsCursor
	DEALLOCATE CustomerProductsCursor
END



-- customer additional services
BEGIN
	DECLARE AdditionalServicesCursor CURSOR READ_ONLY
	FOR
	SELECT CustomerId, Tenant
	FROM CustomerAdditionalServices
	OPEN AdditionalServicesCursor FETCH NEXT FROM AdditionalServicesCursor INTO @CustomerId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		update Customers
		set ActivityWatch = 1
		where Id = @CustomerId and Tenant = @Tenant
		
	FETCH NEXT FROM AdditionalServicesCursor INTO @CustomerId, @Tenant	
	END
	CLOSE AdditionalServicesCursor
	DEALLOCATE AdditionalServicesCursor
END

