declare @Tenant as int
declare @EntityId as varchar(15)
declare @CustomerId as varchar(15)
declare @Subject as nvarchar(250)
declare @StageName as nvarchar(250)
declare @StageId as varchar(15)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, CustomerId, Subject, StageId 
	FROM Opportunities where CustomerId is not null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @CustomerId, @Subject, @StageId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		BEGIN
			set @StageName = (SELECT Name FROM Stages where Tenant = @Tenant and Id = @StageId)
			update customers set LastOpportunityStatus = @StageName, LastOpportunitySubject = @Subject where Tenant=@Tenant and Id=@CustomerId
		END
	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @CustomerId, @Subject, @StageId
	END
	CLOSE DataCursor   
	DEALLOCATE DataCursor
END

