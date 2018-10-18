declare @Tenant integer
declare @BusinessHourId as varchAR(15)
declare @DefaultId as varchAR(15)

BEGIN 

	DECLARE BusinessHoursCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	From BusinessHours
	OPEN BusinessHoursCursor FETCH NEXT FROM BusinessHoursCursor INTO @BusinessHourId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		set @DefaultId = (SELECT Id FROM BusinessHours where Tenant = @Tenant and Code = 'CLA')

		update SLALines set BusinessHoursId = @DefaultId where Id = @BusinessHourId

	FETCH NEXT FROM BusinessHoursCursor INTO @BusinessHourId, @Tenant	
	END

	CLOSE BusinessHoursCursor
	DEALLOCATE BusinessHoursCursor
END


delete from BusinessHours where Code = 'CAL'

declare @NewId as varchar(15)
declare @ContactId as varchar(15)

BEGIN
	DECLARE BusinessHoursTablesCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN BusinessHoursTablesCursor FETCH NEXT FROM BusinessHoursTablesCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	-- BusinessHours
	BEGIN

		if not exists (select * from BusinessHours where Code = 'CAL' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'BusinessHour'
			set @ContactId = (select top 1 Id from Contacts where Tenant = @Tenant AND  UserType = 'S' And Email like '%system%')
			insert into BusinessHours(Id, Tenant, Name,Is247, CreatedByUserId, UpdatedByUserId,CreateDate,UpdateDate ,
			SundayFromHour,SundayToHour,FridayFromHour,FridayToHour,ThursdayFromHour,ThursdayToHour,WednesdayFromHour,WednesdayToHour,
			TuesdayFromHour,TuesdayToHour,MondayFromHour,MondayToHour,SaturdayFromHour,SaturdayToHour,
			IsSundayEnabeled,IsFridayEnabeled,IsThursdayEnabeled,IsWednesdayEnabeled,IsTuesdayEnabeled,IsMondayEnabeled,IsSaturdayEnabeled,Code)
			values
			(
			@NewId,
			@Tenant,
			'Calender',
			1,
			@ContactId,
			@ContactId,
			GETDATE(),
			GETDATE(),
			'08:00:00.0000000',
			'17:00:00.0000000',
			'08:00:00.0000000',
			'17:00:00.0000000',
			'08:00:00.0000000',
			'17:00:00.0000000',
			'08:00:00.0000000',
			'17:00:00.0000000',
			'08:00:00.0000000',
			'17:00:00.0000000',
			'08:00:00.0000000',
			'17:00:00.0000000',
			'08:00:00.0000000',
			'17:00:00.0000000',
			'1',
			'1',
			'1',
			'1',
			'1',
			'1',
			'1',
			'CAL'
			)
		END
	END

	FETCH NEXT FROM BusinessHoursTablesCursor INTO  @Tenant
	END
	CLOSE BusinessHoursTablesCursor
	DEALLOCATE BusinessHoursTablesCursor

END