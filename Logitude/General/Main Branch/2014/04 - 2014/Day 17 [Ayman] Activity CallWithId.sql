
-- Step1: Run this Cursor
-- Step2: Drop the SenderId & RecipientId

-- Step1
BEGIN;
declare @Tenant as int
declare @ActivityId as varchar(15)
declare @CallTypeCode as varchar(1)
declare @SenderId as varchar(15)
declare @RecipientId as varchar(15)
declare @CallWithId as varchar(15)
declare @UpdatedByUserId as varchar(15)

	DECLARE ActivitiesCursor CURSOR READ_ONLY
	FOR
	SELECT Tenant, Id, CallTypeCode, SenderId, RecipientId, UpdatedByUserId, CallWithId
	From Activities
	where ActivityTypeCode = 'CL' --AND CallWithId is null 
	OPEN ActivitiesCursor FETCH NEXT FROM ActivitiesCursor INTO @Tenant, @ActivityId, @CallTypeCode, @SenderId, @RecipientId, @UpdatedByUserId, @CallWithId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if (@CallTypeCode is null)
		set @CallTypeCode = 'O'

		if (@CallTypeCode = 'O')
		begin
			if (@RecipientId is not null and @RecipientId != @UpdatedByUserId)		
			set @CallWithId = @RecipientId
		
			else
			set @CallWithId = @SenderId

			if (@CallWithId is null)
			begin
				if (@RecipientId is not null)
				set @CallWithId = @RecipientId

				else
				set @CallWithId = @SenderId
			end
		end

		else
		begin
			if (@SenderId is not null and @SenderId != @UpdatedByUserId)		
			set @CallWithId = @SenderId
		
			else
			set @CallWithId = @RecipientId

			if (@CallWithId is null)
			begin
				if (@SenderId is not null)
				set @CallWithId = @SenderId

				else
				set @CallWithId = @RecipientId
			end
		end

		
		update Activities
		set CallWithId = @CallWithId, CallTypeCode = @CallTypeCode
		where Tenant = @Tenant AND Id = @ActivityId
		
	FETCH NEXT FROM ActivitiesCursor INTO @Tenant, @ActivityId, @CallTypeCode, @SenderId, @RecipientId, @UpdatedByUserId, @CallWithId
	END
	CLOSE ActivitiesCursor
	DEALLOCATE ActivitiesCursor
END

-- Step2

	--alter table Activities drop Activity_SenderContact
	--go

	--alter table Activities drop Activity_RecipientContact
	--go

	--alter table Activities drop column SenderId
	--go

	--alter table Activities drop column RecipientId
	--go