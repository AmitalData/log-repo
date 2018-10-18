

declare @Tenant as int
declare @offset as float

BEGIN 
		DECLARE TenantCursor CURSOR READ_ONLY
		FOR
		SELECT Id,TimeZoneOffset
		FROM Tenants
		OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant, @offset		
		WHILE @@FETCH_STATUS = 0
			BEGIN
			declare @hours as int
			declare @minutes as int
			declare @reminderMinutes as float

			set @hours = FLOOR(@offset)
			set @reminderMinutes=@offset - @hours
			set @minutes=@reminderMinutes*60
			set @hours=@hours*-1
			set @minutes=@minutes*-1
		
		    update CommunicationLogs set CreateDateUTC= DATEADD(HOUR,@hours,DATEADD(MINUTE,@minutes,CreateDate)),LastStatusDateUTC= DATEADD(HOUR,@hours,DATEADD(MINUTE,@minutes,LastStatusDate))
			,DoneDateUTC= DATEADD(HOUR,@hours,DATEADD(MINUTE,@minutes,DoneDate)),NextTryDateTimeUTC=DATEADD(HOUR,@hours,DATEADD(MINUTE,@minutes,NextTryDateTime)) where Tenant=@Tenant

			--update CommunicationLogs set LastStatusDateUTC= DATEADD(HOUR,@hours,DATEADD(MINUTE,@minutes,LastStatusDate)) where Tenant=@Tenant
			--update CommunicationLogs set DoneDateUTC= DATEADD(HOUR,@hours,DATEADD(MINUTE,@minutes,DoneDate)) where Tenant=@Tenant
			--update CommunicationLogs set NextTryDateTimeUTC=DATEADD(HOUR,@hours,DATEADD(MINUTE,@minutes,NextTryDateTime)) where Tenant=@Tenant

			--update CommunicationLogs set CreateDateUTC= DATEADD(HOUR,@hours,CreateDate) where Tenant=@Tenant
			--update CommunicationLogs set CreateDateUTC= DATEADD(MINUTE,@minutes,CreateDateUTC) where Tenant=@Tenant

			--update CommunicationLogs set LastStatusDateUTC= DATEADD(HOUR,@hours,LastStatusDate) where Tenant=@Tenant
			--update CommunicationLogs set LastStatusDateUTC= DATEADD(MINUTE,@minutes,LastStatusDateUTC) where Tenant=@Tenant

			--update CommunicationLogs set DoneDateUTC= DATEADD(HOUR,@hours,DoneDate) where Tenant=@Tenant
			--update CommunicationLogs set DoneDateUTC= DATEADD(MINUTE,@minutes,DoneDateUTC) where Tenant=@Tenant

			--update CommunicationLogs set NextTryDateTimeUTC= DATEADD(HOUR,@hours,NextTryDateTime) where Tenant=@Tenant
			--update CommunicationLogs set NextTryDateTimeUTC= DATEADD(MINUTE,@minutes,NextTryDateTimeUTC) where Tenant=@Tenant

			FETCH NEXT FROM TenantCursor INTO @Tenant, @offset		
			END
		CLOSE TenantCursor
		DEALLOCATE TenantCursor
END


--declare @date1 datetime
--declare @date2 datetime

--set @date1='01-02-2014'

--set @date2=DATEADD(HOUR,5,DATEADD(MINUTE,25,@date1))
--select @date2

--set @date2=DATEADD(HOUR,5,@date1)
--set @date2=DATEADD(MINUTE,25,@date2)
--select @date2 

--2014-03-04 14:02:42.233

select CreateDate,CreateDateUTC,DoneDate,DoneDateUTC,LastStatusDate,LastStatusDateUTC,NextTryDateTime,NextTryDateTimeUTC from CommunicationLogs where  Tenant=1

--            declare @hours as int
--			declare @minutes as int
--			declare @reminderMinutes as float
--			declare @offset as float
--			declare @floorHours as float
--			set @offset=5.5
--			set @floorHours = FLOOR(@offset)
--			print @floorHours
--			set @reminderMinutes=@offset-@floorHours
--			print @reminderMinutes

--			set @hours=@floorHours
--			set @minutes=@reminderMinutes*60
--			set @hours=@hours*-1
--			set @minutes=@minutes*-1
--			update CommunicationLogs set DoneDateUTC= DATEADD(HOUR,@hours,DoneDate) where Id='1-664' and Tenant=1
--			update CommunicationLogs set DoneDateUTC= DATEADD(MINUTE,@minutes,DoneDateUTC) where Id='1-664' and Tenant=1
