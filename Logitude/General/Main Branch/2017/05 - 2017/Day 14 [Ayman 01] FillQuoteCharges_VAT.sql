

declare @Tenant as int
declare @EntityId as varchar(15)
declare @VatTypeId as varchar(15)
declare @VatPercentage as float
declare @ChargeTypeId as varchar(15)
declare @UpdateDate as Datetime

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, ChargesTypeId, UpdateDate
	FROM QuoteCharges
	WHERE VatTypeId is null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ChargeTypeId, @UpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @VatTypeId = (select VatTypeId from ChargesTypes where Tenant = @Tenant AND Id = @ChargeTypeId)
	if (@VatTypeId is not null)
	begin
		set @VatPercentage = (select top 1 Percentage from VatTypePercentages where VatTypeId = @VatTypeId AND Tenant = @Tenant and FromDate <= @UpdateDate order by FromDate desc)
		
		update QuoteCharges
		set VatTypeId = @VatTypeId, VatPercentage = @VatPercentage
		where Id = @EntityId and Tenant = @Tenant
	end

	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ChargeTypeId, @UpdateDate
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END
