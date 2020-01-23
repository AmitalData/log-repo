declare @Tenant integer
declare @TariffSettingId as varchAR(15)
declare @LCLDefaultStepsId as varchAR(15)
declare @AirDefaultStepsId as varchAR(15)
declare @DefaultId as varchAR(15)
declare @NewId as varchar(15)
declare @ContactId as varchar(15)

BEGIN 

	DECLARE TariffSettingsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, LCLDefaultStepsId, AirDefaultStepsId, Tenant
	From TariffSettings
	OPEN TariffSettingsCursor FETCH NEXT FROM TariffSettingsCursor INTO @TariffSettingId, @LCLDefaultStepsId, @AirDefaultStepsId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		BEGIN

		if (@LCLDefaultStepsId) is null and (@AirDefaultStepsId) is null
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'PriceStep'
			set @ContactId = (select top 1 Id from Contacts where Tenant = @Tenant AND  UserType = 'S' And Email like '%system%')
			insert into PriceSteps(Id, Tenant,CreateDate, UpdateDate, CreatedByUserId,UpdatedByUserId,Name,Inactive,Steps,SearchFields)
			values
			(
			@NewId,
			@Tenant,
			GETDATE(),
			GETDATE(),
			@ContactId,
			@ContactId,
			'Default Price Step',
			0,
			'0,45,100,250,500,1000',
			'Default Price Step'
			)
			update TariffSettings set LCLDefaultStepsId = @NewId, AirDefaultStepsId = @NewId where Id =  @TariffSettingId and Tenant = @Tenant
		END
	END

	FETCH NEXT FROM TariffSettingsCursor INTO @TariffSettingId, @LCLDefaultStepsId, @AirDefaultStepsId, @Tenant
	END

	CLOSE TariffSettingsCursor
	DEALLOCATE TariffSettingsCursor
END