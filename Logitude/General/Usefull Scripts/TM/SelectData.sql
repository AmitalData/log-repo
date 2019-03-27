
-- [Ayman]

declare @Month as int
declare @Year as int
declare @Email as varchar(70)
declare @UserId as varchar(15)

set @Month = 3
set @Year = 2019
set @Email = 'ayman@fnarsoft.com'

BEGIN
DECLARE @StartOfMonth DATETIME
DECLARE @EndOfMonth DATETIME
DECLARE @FullTimeTotalMinutes as int
DECLARE @DataEntryTotalMinutes as int
DECLARE @ClockTimeTotalMinutes as int
declare @FullTimeResult as varchar(10)
declare @DataEntryResult as varchar(10)
declare @ClockTimeResult as varchar(10)


declare @TotalsTable table
(
  DataEntryMinutes int,
  ClockTimeMinutes int,
  PerProjectMinutes int,
  PerProjectMinutes_NoProject int,
  DataEntryTime varchar(10),
  ClockTimeTime varchar(10),
  PerProjectTime varchar(10),
  PerProjectTime_NoProject varchar(10)
)

set @UserId = (select top 1 Id from Contacts where Email = @Email)
set @EndOfMonth = (SELECT DATEADD(month, ((@Year - 1900) * 12) + @Month, -1))
set @StartOfMonth = (SELECT DATEADD(month, DATEDIFF(month, 0, @EndOfMonth), 0))
set @EndOfMonth = (SELECT DATEADD(HOUR, 23, @EndOfMonth))
set @EndOfMonth = (SELECT DATEADD(MINUTE, 59, @EndOfMonth))
set @EndOfMonth = (SELECT DATEADD(SECOND, 59, @EndOfMonth))


set @DataEntryTotalMinutes = (select sum(TimeInMinutes)
								from TMEmployeeTimes 
								where LocationCode = 'O'
								and EmployeeUserId = @UserId
								and DateOfWork >= @StartOfMonth
								and DateOfWork <= @EndOfMonth
								)
set @ClockTimeTotalMinutes = (select sum(datediff(minute, EntryTime, ExitTime))
								from TMOfficeHours
								where Inactive = 0
								and UserId = @UserId
								and EntryTime is not null
								and ExitTime is not null
								and WorkDate >= @StartOfMonth
								and WorkDate <= @EndOfMonth
								)

set @DataEntryTime = convert(varchar,sum(@DataEntryTotalMinutes) / 60) + ':' + convert(varchar,sum(@DataEntryTotalMinutes) % 60)
set @ClockTimeTime = convert(varchar,sum(@ClockTimeTotalMinutes) / 60) + ':' + convert(varchar,sum(@ClockTimeTotalMinutes) % 60)
set @PerProjectTime = convert(varchar,sum(@PerProjectMinutes) / 60) + ':' + convert(varchar,sum(@PerProjectMinutes) % 60)
set @PerProjectTime_NoProject = convert(varchar,sum(@PerProjectMinutes_NoProject) / 60) + ':' + convert(varchar,sum(@PerProjectMinutes_NoProject) % 60)

insert into @TotalsTable(DataEntryMinutes, ClockTimeMinutes, PerProjectMinutes, DataEntryTime, ClockTimeTime, PerProjectTime, PerProjectMinutes_NoProject, PerProjectTime_NoProject)
values
(
@DataEntryTotalMinutes,
@ClockTimeTotalMinutes,
@PerProjectMinutes,
@DataEntryTime,
@ClockTimeTime,
@PerProjectTime,
@PerProjectMinutes_NoProject,
@PerProjectTime_NoProject
)


select DataEntryTime, ClockTimeTime, PerProjectTime, PerProjectTime_NoProject  from @TotalsTable


--select sum(FullDuration), convert(varchar,sum(FullDuration) / 60)
--from TMEmployeeTimes where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth and (ProjectId is null OR ProjectId = '')

--select sum(FullDuration), convert(varchar,sum(FullDuration) / 60)
--from TMEmployeeTimes where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth and (ProjectId is not null and ProjectId <> '')

--set @FullTimeResult = convert(varchar,sum(@FullTimeTotalMinutes) / 60) + ':' + convert(varchar,sum(@FullTimeTotalMinutes) % 60)
--set @DataEntryResult = convert(varchar,sum(@DataEntryTotalMinutes) / 60) + ':' + convert(varchar,sum(@DataEntryTotalMinutes) % 60)
--set @ClockTimeResult = convert(varchar,sum(@ClockTimeTotalMinutes) / 60) + ':' + convert(varchar,sum(@ClockTimeTotalMinutes) % 60)
--print 'FullTime: ' + convert(varchar,@FullTimeTotalMinutes) + ' minutes,	' + @FullTimeResult
--print 'DataEntry: ' + convert(varchar,@DataEntryTotalMinutes) + ' minutes,	' + @DataEntryResult
--print 'ClockTime: ' + convert(varchar,@ClockTimeTotalMinutes) + ' minutes,	' + @ClockTimeResult

--if exists (select * from TMEmployeeTimes where FullDuration = 0 and EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth)
--select * from TMEmployeeTimes where FullDuration = 0 and EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth

--select WorkDate,
--sum(datediff(minute, EntryTime, ExitTime)) as TotalMinutes,
--sum(datediff(minute, EntryTime, ExitTime)) / 60 as Hours,
--sum(datediff(minute, EntryTime, ExitTime)) % 60 as Minutes
--from TMOfficeHours
--where Inactive = 0
--and UserId = @UserId
--and EntryTime is not null
--and ExitTime is not null
--and RecordedEntryTime >= @StartOfMonth
--and RecordedEntryTime <= @EndOfMonth
--and RecordedExitTime >= @StartOfMonth
--and RecordedExitTime <= @EndOfMonth
--group by WorkDate
--order by WorkDate

--select WorkDate,
--datediff(minute, EntryTime, ExitTime) as TotalMinutes,
--((convert(varchar,datediff(minute, EntryTime, ExitTime) / 60)) + ':' + right('00'+(convert(varchar,datediff(minute, EntryTime, ExitTime) % 60)),2)) as Duration
--from TMOfficeHours
--where Inactive = 0
--and UserId = @UserId
--and EntryTime is not null
--and ExitTime is not null
--and RecordedEntryTime >= @StartOfMonth
--and RecordedEntryTime <= @EndOfMonth
--and RecordedExitTime >= @StartOfMonth
--and RecordedExitTime <= @EndOfMonth
----group by WorkDate
--order by WorkDate

								

END

--select WorkDate,
--sum(datediff(minute, EntryTime, ExitTime)) as TotalMinutes,
--sum(datediff(minute, EntryTime, ExitTime)) / 60 as Hours,
--sum(datediff(minute, EntryTime, ExitTime)) % 60 as Minutes
--from TMOfficeHours
--where Inactive = 0
--and UserId = '1-117301'
--and EntryTime is not null
--and ExitTime is not null
--and RecordedEntryTime >= '2019-03-01'
--and RecordedEntryTime <= '2019-03-31'
--group by WorkDate
						
--select 
--sum(datediff(minute, EntryTime, ExitTime)) as TotalMinutes,
--sum(datediff(minute, EntryTime, ExitTime)) / 60 as Hours,
--sum(datediff(minute, EntryTime, ExitTime)) % 60 as Minutes
--from TMOfficeHours
--where Inactive = 0
--and UserId = '1-117301'
--and EntryTime is not null
--and ExitTime is not null
--and RecordedEntryTime >= '2019-03-01'
--and RecordedEntryTime <= '2019-03-31'

-- [Get the last day of the month in SQL]
-- [https://stackoverflow.com/questions/1051488/get-the-last-day-of-the-month-in-sql]
-- DECLARE @test DATETIME
-- SET @test = GETDATE()  -- or any other date
-- SELECT DATEADD(month, ((YEAR(@test) - 1900) * 12) + MONTH(@test), -1)

-- [How can I select the first day of a month in SQL?]
-- [https://stackoverflow.com/questions/1520789/how-can-i-select-the-first-day-of-a-month-in-sql]
-- SELECT DATEADD(month, DATEDIFF(month, 0, @mydate), 0) AS StartOfMonth

-- [How do I calculate total minutes between start and end times?]
-- [https://stackoverflow.com/questions/25446484/how-do-i-calculate-total-minutes-between-start-and-end-times]
-- select (datediff(minute, starttime, endtime) -lunch -recess) * 5 AS TotalInstruct from YourTable
-- select sum((datediff(minute, starttime, endtime) -lunch -recess) * 5) AS TotalInstruct from YourTable

--select * from Contacts where Email = 'ayman@fnarsoft.com'
-- 2019-03-03 06:59:00.000
-- 2019-03-03 06:59:00.000

-- update TMEmployeeTimes set FullDuration = TimeInMinutes where FullDuration = 0 and ProratedDuration = 0 and TimeInMinutes <> 0
-- select count(*) from TMEmployeeTimes where FullDuration = 0 and ProratedDuration = 0 and TimeInMinutes <> 0

-- 19/3/2019
-- select count (*) from TMEmployeeTimes where ProjectId is null	637
-- select count (*) from TMEmployeeTimes where ProjectId = ''		2276





	--DECLARE TMOfficeHoursCursor CURSOR READ_ONLY
	--FOR
	--SELECT CAST(WorkDate AS DATE), EntryTime, ExitTime, RecordedEntryTime, RecordedExitTime
	--FROM TMOfficeHours
	--WHERE
	--Inactive = 0 
	--AND UserId = @UserId
	--AND WorkDate >= @StartOfMonth
	--AND WorkDate <= @EndOfMonth
	--AND EntryTime is not null
	--AND ExitTime is not null
	--OPEN TMOfficeHoursCursor FETCH NEXT FROM TMOfficeHoursCursor INTO @WorkDate, @EntryTime, @ExitTime, @RecordedEntryTime, @RecordedExitTime
	--WHILE @@FETCH_STATUS = 0
	--BEGIN
		
	--	set @Date1 = CAST(@EntryTime AS DATE)
	--	insert into @TMEmployeeTimes(DateOfWork)
	--	values
	--	(
	--	@WorkDate
	--	)



	--FETCH NEXT FROM TMOfficeHoursCursor INTO @WorkDate
	--END
	--CLOSE TMOfficeHoursCursor
	--DEALLOCATE TMOfficeHoursCursor