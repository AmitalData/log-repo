
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

set @UserId = (select top 1 Id from Contacts where Email = @Email)
set @EndOfMonth = (SELECT DATEADD(month, ((@Year - 1900) * 12) + @Month, -1))
set @StartOfMonth = (SELECT DATEADD(month, DATEDIFF(month, 0, @EndOfMonth), 0))


set @FullTimeTotalMinutes = (select sum(TMEmployeeTimes.FullDuration)
								from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
								where TMEmployeeTimes.EmployeeUserId = @UserId
								and TMEmployeeTimes.DateOfWork >= @StartOfMonth
								and TMEmployeeTimes.DateOfWork <= @EndOfMonth
								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
								and TMProjects.IsProrated = 0
								)

set @DataEntryTotalMinutes = (select sum(TimeInMinutes)
								from TMEmployeeTimes 
								where LocationCode = 'O'
								and EmployeeUserId = @UserId
								and DateOfWork >= @StartOfMonth
								and DateOfWork <= @EndOfMonth)

set @ClockTimeTotalMinutes = (select sum(datediff(minute, EntryTime, ExitTime))
								from TMOfficeHours
								where Inactive = 0
								and UserId = @UserId
								and EntryTime is not null
								and ExitTime is not null
								and RecordedEntryTime >= @StartOfMonth
								and RecordedEntryTime <= @EndOfMonth
								and RecordedExitTime >= @StartOfMonth
								and RecordedExitTime <= @EndOfMonth
								)

set @FullTimeResult = convert(varchar,sum(@FullTimeTotalMinutes) / 60) + ':' + convert(varchar,sum(@FullTimeTotalMinutes) % 60)
set @DataEntryResult = convert(varchar,sum(@DataEntryTotalMinutes) / 60) + ':' + convert(varchar,sum(@DataEntryTotalMinutes) % 60)
set @ClockTimeResult = convert(varchar,sum(@ClockTimeTotalMinutes) / 60) + ':' + convert(varchar,sum(@ClockTimeTotalMinutes) % 60)
print 'FullTime: ' + convert(varchar,@FullTimeTotalMinutes) + ' minutes,	' + @FullTimeResult
print 'DataEntry: ' + convert(varchar,@DataEntryTotalMinutes) + ' minutes,	' + @DataEntryResult
print 'ClockTime: ' + convert(varchar,@ClockTimeTotalMinutes) + ' minutes,	' + @ClockTimeResult

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


