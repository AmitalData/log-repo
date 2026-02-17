select Id, DateOfWork,* from TMEmployeeTimes
select * from Sprints where tenant = 1

select * from Sprints
select * from Sprints
DECLARE @Tenant AS INT
DECLARE @DateOfWork AS datetime
declare @Id as varchar(15)
declare @SprintId as varchar(60)

	DECLARE TMEmployeeTimesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id, Tenant, DateOfWork
	FROM TMEmployeeTimes where SprintId is null	
	OPEN TMEmployeeTimesCursor FETCH NEXT FROM TMEmployeeTimesCursor INTO @Id , @Tenant, @DateOfWork
	WHILE @@FETCH_STATUS = 0
	BEGIN
		set @SprintId = (select Id from Sprints where FromDate <= @DateOfWork and ToDate >=  @DateOfWork and Tenant = @Tenant)
		--print @SprintId + ': '+ @Id
		if (@SprintId is not null)
	    	update TMEmployeeTimes set SprintId = @SprintId where  Tenant = @Tenant and Id = @Id

	FETCH NEXT FROM TMEmployeeTimesCursor INTO @Id , @Tenant, @DateOfWork
	END
	CLOSE TMEmployeeTimesCursor
	DEALLOCATE TMEmployeeTimesCursor