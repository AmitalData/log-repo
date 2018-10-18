
declare @Tenant as int
declare @ActivityId as varchar(15)
declare @ActivityTypeCode as varchar(2)
declare @OwnerId as varchar(15)
declare @CallWithId as varchar(15)
declare @ActivityWith as varchar(500)
declare @AppointmentContactId as varchar(15)
declare @AppointmentContactName as varchar(100)
declare @RecipientContactId as varchar(15)
declare @RecipientContactName as varchar(100)

BEGIN
	DECLARE ActivitiesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, ActivityTypeCode, OwnerId, CallWithId
	FROM Activities	
	OPEN ActivitiesCursor FETCH NEXT FROM ActivitiesCursor INTO @ActivityId, @Tenant, @ActivityTypeCode, @OwnerId, @CallWithId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @ActivityWith = null

		--Task
	    if(@ActivityTypeCode = 'TS')
			begin
				
			if(@OwnerId is not null)
			begin
				set @ActivityWith = (select EnglishName from Contacts where Id = @OwnerId and Tenant = @Tenant)					
			end

		end

		--Call
		if(@ActivityTypeCode = 'CL')
		begin
				
			if(@CallWithId is not null)
			begin
				set @ActivityWith = (select EnglishName from Contacts where Id = @CallWithId and Tenant = @Tenant)					
			end
				
		end

		--Appointment
		if(@ActivityTypeCode = 'AP')
		begin
			BEGIN
			DECLARE InviteesCursor CURSOR READ_ONLY
			FOR
			SELECT ContactId
			FROM ActivityInvitees
			where IsRequired = 1 and ActivityId = @ActivityId and Tenant = @Tenant
			OPEN InviteesCursor FETCH NEXT FROM InviteesCursor INTO @AppointmentContactId
			WHILE @@FETCH_STATUS = 0		
			BEGIN
			
				if(@AppointmentContactId is not null)
				begin
					set @AppointmentContactName = (select EnglishName from Contacts where Id = @AppointmentContactId)
					if(@AppointmentContactName is not null)
					begin
						if(@ActivityWith is null) set @ActivityWith = @AppointmentContactName
						else set @ActivityWith = @ActivityWith + ', '+ @AppointmentContactName
					end
				end

			FETCH NEXT FROM InviteesCursor INTO @AppointmentContactId
			END
			CLOSE InviteesCursor
			DEALLOCATE InviteesCursor
			END
		end

		--Email In / Out
		if(@ActivityTypeCode = 'EI' or @ActivityTypeCode = 'EO')
		begin
			BEGIN
			DECLARE RecipientCursor CURSOR READ_ONLY
			FOR
			SELECT ContactId
			FROM ActivityEmailRecipients
			where ActivityId = @ActivityId and Tenant = @Tenant
			OPEN RecipientCursor FETCH NEXT FROM RecipientCursor INTO @RecipientContactId
			WHILE @@FETCH_STATUS = 0		
			BEGIN
			
				if(@RecipientContactId is not null)
				begin
					set @RecipientContactName = (select EnglishName from Contacts where Id = @RecipientContactId)
					if(@RecipientContactName is not null)
					begin
						if(@ActivityWith is null or @ActivityWith = '') set @ActivityWith = @RecipientContactName
						else set @ActivityWith = @ActivityWith + ', '+ @RecipientContactName
					end
				end

			FETCH NEXT FROM RecipientCursor INTO @RecipientContactId
			END
			CLOSE RecipientCursor
			DEALLOCATE RecipientCursor
			END
		end

		update Activities set ActivityWith = @ActivityWith where Id = @ActivityId and Tenant = @Tenant

		FETCH NEXT FROM ActivitiesCursor INTO @ActivityId, @Tenant, @ActivityTypeCode, @OwnerId, @CallWithId
	END
	CLOSE ActivitiesCursor
	DEALLOCATE ActivitiesCursor
END