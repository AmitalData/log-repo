
-- [Ayman]

declare @Month as int
declare @Year as int
declare @Email as varchar(70)
declare @UserId as varchar(15)

set @Month = 1
set @Year = 2019
set @Email = 'ayman@fnarsoft.com'

BEGIN
DECLARE @StartOfMonth DATETIME
DECLARE @EndOfMonth DATETIME
-------
DECLARE @DataEntryTotalMinutes as int
DECLARE @HomeAndClientMinutes as int
DECLARE @ClockTimeTotalMinutes as int
DECLARE @TotalMinutes as int
DECLARE @OverTimeMinutes as int
DECLARE @PerProjectMinutes as int
DECLARE @PerProjectMinutes_NoProject as int
DECLARE @PerProjectMinutes_HasProject as int
-------
declare @DataEntryTime as varchar(10)
declare @HomeAndClientEntryTime as varchar(10)
declare @ClockTimeTime as varchar(10)
declare @TotalTime as varchar(10)
declare @OverTime as varchar(10)
declare @PerProjectTime as varchar(10)
declare @PerProjectTime_NoProject as varchar(10)
declare @PerProjectTime_HasProject as varchar(10)
-------

declare @TotalsTable table
(
  DataEntryMinutes int,
  ClockTimeMinutes int,
  HomeAndClientMinutes int,
  TotalMinutes int,
  OverTimeMinutes int,
  PerProjectMinutes int,
  PerProjectMinutes_NoProject int,
  PerProjectMinutes_HasProject int,
  -------
  DataEntry varchar(10),
  ClockTime varchar(10),
  HomeAndClient varchar(10),
  Total varchar(10),
  OverTime varchar(10),
  PerProjectTime varchar(10),
  PerProjectTime_NoProject varchar(10),
  PerProjectTime_HasProject varchar(10)
)

set @UserId = (select top 1 Id from Contacts where Email = @Email)
set @EndOfMonth = (SELECT DATEADD(month, ((@Year - 1900) * 12) + @Month, -1))
set @StartOfMonth = (SELECT DATEADD(month, DATEDIFF(month, 0, @EndOfMonth), 0))
set @EndOfMonth = (SELECT DATEADD(HOUR, 23, @EndOfMonth))
set @EndOfMonth = (SELECT DATEADD(MINUTE, 59, @EndOfMonth))
set @EndOfMonth = (SELECT DATEADD(SECOND, 59, @EndOfMonth))

set @DataEntryTotalMinutes = (select sum(TimeInMinutes) from TMEmployeeTimes where LocationCode = 'O' and EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth)
set @HomeAndClientMinutes = (select sum(TimeInMinutes) from TMEmployeeTimes where LocationCode != 'O' and EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth)
set @ClockTimeTotalMinutes = (select sum(datediff(minute, EntryTime, ExitTime)) from TMOfficeHours where Inactive = 0 and UserId = @UserId and EntryTime is not null and ExitTime is not null and WorkDate >= @StartOfMonth	and WorkDate <= @EndOfMonth)
set @TotalMinutes = isnull(@HomeAndClientMinutes,0) + isnull(@ClockTimeTotalMinutes,0)
set @OverTimeMinutes = @TotalMinutes - (207 * 60)
set @PerProjectMinutes_NoProject = (select sum(FullDuration)
								from TMEmployeeTimes
								where EmployeeUserId = @UserId
								and DateOfWork >= @StartOfMonth
								and DateOfWork <= @EndOfMonth
								and (ProjectId is null OR ProjectId = '')
								)
set @PerProjectMinutes_HasProject = (select sum(TMEmployeeTimes.FullDuration)
								from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
								where TMEmployeeTimes.EmployeeUserId = @UserId
								and TMEmployeeTimes.DateOfWork >= @StartOfMonth
								and TMEmployeeTimes.DateOfWork <= @EndOfMonth
								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
								and TMProjects.IsProrated = 0
								)
set @PerProjectMinutes = isnull(@PerProjectMinutes_NoProject,0) + isnull(@PerProjectMinutes_HasProject,0)

set @DataEntryTime = convert(varchar,@DataEntryTotalMinutes / 60) + ':' + convert(varchar,@DataEntryTotalMinutes % 60)
set @HomeAndClientEntryTime = convert(varchar,@HomeAndClientMinutes / 60) + ':' + convert(varchar,@HomeAndClientMinutes % 60)
set @ClockTimeTime = convert(varchar,@ClockTimeTotalMinutes / 60) + ':' + convert(varchar,@ClockTimeTotalMinutes % 60)
set @TotalTime = convert(varchar,@TotalMinutes / 60) + ':' + convert(varchar,@TotalMinutes % 60)
set @OverTime = convert(varchar,@OverTimeMinutes / 60) + ':' + convert(varchar,@OverTimeMinutes % 60)
set @PerProjectTime = convert(varchar,@PerProjectMinutes / 60) + ':' + convert(varchar,@PerProjectMinutes % 60)
set @PerProjectTime_NoProject = convert(varchar,@PerProjectMinutes_NoProject / 60) + ':' + convert(varchar,@PerProjectMinutes_NoProject % 60)
set @PerProjectTime_HasProject = convert(varchar,@PerProjectMinutes_HasProject / 60) + ':' + convert(varchar,@PerProjectMinutes_HasProject % 60)

insert into @TotalsTable
(
DataEntryMinutes,
HomeAndClientMinutes,
ClockTimeMinutes,
TotalMinutes,
OverTimeMinutes,
PerProjectMinutes,
PerProjectMinutes_NoProject,
PerProjectMinutes_HasProject,
DataEntry,
HomeAndClient,
ClockTime,
Total,
OverTime,
PerProjectTime,
PerProjectTime_NoProject,
PerProjectTime_HasProject
)
values
(
@DataEntryTotalMinutes,
@HomeAndClientMinutes,
@ClockTimeTotalMinutes,
@TotalMinutes,
@OverTimeMinutes,
@PerProjectMinutes,
@PerProjectMinutes_NoProject,
@PerProjectMinutes_HasProject,

@DataEntryTime,
@HomeAndClientEntryTime,
@ClockTimeTime,
@TotalTime,
@OverTime,
@PerProjectTime,
@PerProjectTime_NoProject,
@PerProjectTime_HasProject
)


select DataEntry, ClockTime, HomeAndClient, Total, OverTime, PerProjectTime from @TotalsTable


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

	   -- select 
	   --TMProjects.Name,
	   --TMProjects.ProjectNumber,
	   --TMEmployeeTimes.DateOfWork, 
	   --TMEmployeeTimes.TimeInMinutes,
	   --TMEmployeeTimes.ProratedDuration,
	   --TMEmployeeTimes.FullDuration,
	   --TMEmployeeTimes.FullDuration / 60,
	   ----TMEmployeeTimes.FullDuration % 60
	   --TMEmployeeTimes.WINumber,
	   --TMEmployeeTimes.Description
	   
				--				from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
				--				where TMEmployeeTimes.EmployeeUserId = @UserId
				--				and TMEmployeeTimes.DateOfWork >= @StartOfMonth
				--				and TMEmployeeTimes.DateOfWork <= '2019-03-05'
				--				--and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
				--				--and TMProjects.IsProrated = 0
				--				order by TMEmployeeTimes.DateOfWork, TMProjects.Name

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

								

--select sum(TimeInMinutes), sum(FullDuration)
--								from TMEmployeeTimes
--								where EmployeeUserId = @UserId
--								and DateOfWork >= @StartOfMonth
--								and DateOfWork <= @EndOfMonth
--								and (ProjectId is null OR ProjectId = '')
								
--select sum(TMEmployeeTimes.TimeInMinutes), sum(TMEmployeeTimes.FullDuration)
--								from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where TMEmployeeTimes.EmployeeUserId = @UserId
--								and TMEmployeeTimes.DateOfWork >= @StartOfMonth
--								and TMEmployeeTimes.DateOfWork <= @EndOfMonth
--								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 0
	
--update TMEmployeeTimes set NeedsProrating = 1 where EmployeeUserId = '1-117301'

--select count(*) from TMEmployeeTimes where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth
select TimeInMinutes, ProratedDuration, FullDuration from TMEmployeeTimes where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth and (ProjectId is null OR ProjectId = '')
--select count(*) from TMEmployeeTimes where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth and ProjectId is not null and ProjectId <> ''
select TimeInMinutes, ProratedDuration, FullDuration from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId =  TMProjects.Id where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth and TMProjects.IsProrated = 1
select TimeInMinutes, ProratedDuration, FullDuration from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId =  TMProjects.Id where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth and TMProjects.IsProrated = 0

-- 13143.4227272727
-- 219.057045454545

--select Id, TimeInMinutes, ProratedDuration, FullDuration, SprintId, ProjectId from TMEmployeeTimes where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth
--select Id, TimeInMinutes, ProratedDuration, FullDuration, SprintId, ProjectId from TMEmployeeTimes where EmployeeUserId = @UserId and DateOfWork >= @StartOfMonth and DateOfWork <= @EndOfMonth



--select sum(TimeInMinutes), sum(FullDuration)
--								from TMEmployeeTimes
--								where EmployeeUserId = @UserId
--								and DateOfWork >= @StartOfMonth
--								and DateOfWork <= @EndOfMonth
--								and (ProjectId is null OR ProjectId = '')							
--select sum(TimeInMinutes), sum(FullDuration)
--								from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where TMEmployeeTimes.EmployeeUserId = @UserId
--								and TMEmployeeTimes.DateOfWork >= @StartOfMonth
--								and TMEmployeeTimes.DateOfWork <= @EndOfMonth
--								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 0	
--select sum(TimeInMinutes), sum(FullDuration)
--								from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where TMEmployeeTimes.EmployeeUserId = @UserId
--								and TMEmployeeTimes.DateOfWork >= @StartOfMonth
--								and TMEmployeeTimes.DateOfWork <= @EndOfMonth
--								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 1	


-- 13769					229.4833333333333
-- 15324.70319526816		255.411719921136



--13143.4227272727
--select Id, TimeInMinutes, ProratedDuration, FullDuration, NeedsProrating, ProjectId
--								from TMEmployeeTimes
--								where EmployeeUserId = @UserId
--								and DateOfWork >= @StartOfMonth
--								and DateOfWork <= @EndOfMonth
--								and (ProjectId is null OR ProjectId = '')
								
--select TMEmployeeTimes.Id, TMEmployeeTimes.TimeInMinutes, TMEmployeeTimes.ProratedDuration, TMEmployeeTimes.FullDuration, TMEmployeeTimes.NeedsProrating, TMEmployeeTimes.ProjectId
--								from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where TMEmployeeTimes.EmployeeUserId = @UserId
--								and TMEmployeeTimes.DateOfWork >= @StartOfMonth
--								and TMEmployeeTimes.DateOfWork <= @EndOfMonth
--								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 0	

END

--select * from TMEmployeeTimes where WINumber = '49353'
--select * from TMProjects where Id = '1-415'

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

-- ayman@fnarsoft.com
-- 1-117301

--select * from TMEmployeeTimes where EmployeeUserId = '1-117301' and DateOfWork >= '2019-03-01'
--select count(*) from TMEmployeeTimes where EmployeeUserId = '1-117301' and ProjectId is not null and ProjectId <> '' and DateOfWork >= '2019-03-01'
--select count(*) from TMEmployeeTimes where EmployeeUserId = '1-117301' and (ProjectId is null or ProjectId = '') and DateOfWork >= '2019-03-01'

--select * from TMEmployeeTimes where EmployeeUserId = '1-117301' and DateOfWork >= '2019-03-01' and DateOfWork <= '2019-03-03 11:59:59.000'
--select * from TMProjects where Id in ('1-40', '1-226')

--select * from TMProjectCategories where Tenant = 1489




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

	--select * from TMEmployeeTimes where SprintId is null or SprintId = ''

	--select SprintId, count(*) from TMEmployeeTimes where EmployeeUserId = '1-117301' group by SprintId

	--select * from TMEmployeeTimes where SprintId = '1-1926' and EmployeeUserId = '1-117301'
	--select * from TMProjects where Id = '1-76'





--	select sum(x.TimeInMinutes) from
--(
--								(select Id, TimeInMinutes, EmployeeUserId, SprintId  from TMEmployeeTimes
--								where 								
--								(ProjectId is null OR ProjectId = '')
--								and SprintId is null
--								and EmployeeUserId = '1-117301'
--								)

--								union 

--								(select TMEmployeeTimes.Id, TMEmployeeTimes.TimeInMinutes, TMEmployeeTimes.EmployeeUserId, TMEmployeeTimes.SprintId from TMEmployeeTimes
--								join TMProjects
--								on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where 								
--								TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 0
--								)
								
--) x where x.SprintId is null and x.EmployeeUserId = '1-117301'





--select sum(TimeInMinutes) from TMEmployeeTimes
--								where 
--								EmployeeUserId = '1-117301'
--								and SprintId is null
--								and (ProjectId is null OR ProjectId = '')

--select sum(TimeInMinutes) from TMEmployeeTimes
--								join TMProjects
--								on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where 
--								TMEmployeeTimes.EmployeeUserId = '1-117301'
--								and TMEmployeeTimes.SprintId is null
--								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 0





--declare @SprintId as varchar(15)
--set @SprintId = '1-1896'

--select count(TimeInMinutes)
--								from TMEmployeeTimes join TMProjects on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where 
--								TMEmployeeTimes.EmployeeUserId = '1-117301'
--								and TMEmployeeTimes.SprintId = @SprintId
--								and TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 1	


--select count(x.TimeInMinutes) from
--(
--								(select Id, TimeInMinutes, EmployeeUserId, SprintId  from TMEmployeeTimes
--								where ProjectId is null OR ProjectId = ''
--								)

--								union 

--								(select TMEmployeeTimes.Id, TMEmployeeTimes.TimeInMinutes, TMEmployeeTimes.EmployeeUserId, TMEmployeeTimes.SprintId from TMEmployeeTimes
--								join TMProjects
--								on TMEmployeeTimes.ProjectId = TMProjects.Id
--								where 								
--								TMEmployeeTimes.ProjectId is not null and TMEmployeeTimes.ProjectId <> ''
--								and TMProjects.IsProrated = 0
--								)
								
--) x where x.SprintId = @SprintId and x.EmployeeUserId = '1-117301'
