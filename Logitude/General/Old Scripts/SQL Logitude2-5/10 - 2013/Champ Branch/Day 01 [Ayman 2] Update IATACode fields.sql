

declare @IATACode varchar(5)
declare @MeasurementCode varchar(4)
declare @MeasurementId varchar(15)
declare @Tenant int

BEGIN
	DECLARE IATACursor CURSOR READ_ONLY
	FOR
	SELECT Code
	From IATACodes
	OPEN IATACursor FETCH NEXT FROM IATACursor INTO @IATACode
	WHILE @@FETCH_STATUS = 0
	BEGIN

	if exists (select Top 1 * from ChargesTypes where IATACodeCode = @IATACode)
		begin
			
			set @Tenant = (select Top 1 Tenant from ChargesTypes where IATACodeCode = @IATACode)
			set @MeasurementId = (select Top 1 MeasurementId from ChargesTypes where IATACodeCode = @IATACode)
			set @MeasurementCode = (select Top 1 Code from Measurements where Tenant = @Tenant AND Id = @MeasurementId)

			update IATACodes
			set
			MeasurementCode = @MeasurementCode,
			DueTypeCode = (select Top 1 DueTypeCode from ChargesTypes where IATACodeCode = @IATACode)
			where Code = @IATACode
		end

	FETCH NEXT FROM IATACursor INTO @IATACode
	END
	CLOSE IATACursor
	DEALLOCATE IATACursor
END