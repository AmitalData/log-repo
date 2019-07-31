declare @ProjectId varchar(15)
declare @TMEmployeeTimeId varchar(15)
declare @ProjectDayOffType varchar(15)
DECLARE @Tenant AS INT

	DECLARE TMEmployeeCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant,Id, ProjectId
	FROM TMEmployeeTimes	
	OPEN TMEmployeeCursor FETCH NEXT FROM TMEmployeeCursor INTO @Tenant,@TMEmployeeTimeId,@ProjectId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @ProjectDayOffType = (select DayOffTypeCode from TMProjects where Id = @ProjectId and Tenant = @Tenant)
		if(@ProjectDayOffType is not null)
		begin
			update TMEmployeeTimes set LocationCode ='D' where Id = @TMEmployeeTimeId and Tenant = @Tenant	
		end		
		FETCH NEXT FROM TMEmployeeCursor INTO @Tenant,@TMEmployeeTimeId,@ProjectId
	END
CLOSE TMEmployeeCursor
DEALLOCATE TMEmployeeCursor

