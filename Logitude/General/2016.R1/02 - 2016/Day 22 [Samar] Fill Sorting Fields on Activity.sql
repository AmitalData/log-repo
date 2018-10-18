
declare @Tenant as int
declare @ActivityId as varchar(15)
declare @ActivityTypeCode as varchar(2)
declare @CompleteDate as datetime
declare @DueDate as datetime
declare @StartDate as datetime
declare @CreateDate as datetime
declare @SendReceiveDate as datetime

declare @SortingDate as datetime
declare @SortingBy as varchar(60)

BEGIN 
	DECLARE ActivitiesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, ActivityTypeCode, CompleteDate, DueDate, StartDateTime, CreateDate, SendReceiveDate
	FROM Activities	
	OPEN ActivitiesCursor FETCH NEXT FROM ActivitiesCursor INTO @ActivityId, @Tenant, @ActivityTypeCode, @CompleteDate, @DueDate, @StartDate, @CreateDate, @SendReceiveDate 
	WHILE @@FETCH_STATUS = 0
		BEGIN

			--Task
			if(@ActivityTypeCode = 'TS')
			begin
				
				if(@CompleteDate is not null)
				begin
					set @SortingDate = @CompleteDate
					set @SortingBy = 'Complete Date'
				end

				else if(@DueDate is not null)
				begin 
					set @SortingDate = @DueDate
					set @SortingBy = 'Due Date'
				end

				else if(@StartDate is not null)
				begin 
					set @SortingDate = @StartDate
					set @SortingBy = 'Start Date'
				end

				else if(@CreateDate is not null)
				begin 
					set @SortingDate = @CreateDate
					set @SortingBy = 'Create Date'
				end

			end

			--Appointment
			if(@ActivityTypeCode = 'AP')
			begin
				
				if(@CompleteDate is not null)
				begin
					set @SortingDate = @CompleteDate
					set @SortingBy = 'Complete Date'
				end

				else if(@StartDate is not null)
				begin 
					set @SortingDate = @StartDate
					set @SortingBy = 'Start Date'
				end
				
			end

			--Call
			if(@ActivityTypeCode = 'CL')
			begin
				
				if(@CompleteDate is not null)
				begin
					set @SortingDate = @CompleteDate
					set @SortingBy = 'Complete Date'
				end

				else if(@DueDate is not null)
				begin 
					set @SortingDate = @DueDate
					set @SortingBy = 'Due Date'
				end

				else if(@CreateDate is not null)
				begin 
					set @SortingDate = @CreateDate
					set @SortingBy = 'Create Date'
				end

			end

			--Email In
			if(@ActivityTypeCode = 'EI')
			begin
				
				if(@SendReceiveDate is not null)
				begin
					set @SortingDate = @SendReceiveDate
					set @SortingBy = 'Send/Receive Date'
				end
				
			end

			--Email Out
			if(@ActivityTypeCode = 'EO')
			begin
				
				if(@SendReceiveDate is not null)
				begin
					set @SortingDate = @SendReceiveDate
					set @SortingBy = 'Send/Receive Date'
				end
				
			end

			update Activities 
			set SortingDate = @SortingDate, SortingBy = @SortingBy
			where Id = @ActivityId and Tenant = @Tenant

			FETCH NEXT FROM ActivitiesCursor INTO @ActivityId, @Tenant, @ActivityTypeCode, @CompleteDate, @DueDate, @StartDate, @CreateDate, @SendReceiveDate 
		END
	CLOSE ActivitiesCursor
	DEALLOCATE ActivitiesCursor
END


