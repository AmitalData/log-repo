

declare @ActivatedEventTypeId as varchar(15)
declare @ActivationRequestEventTypeId as varchar(15)
declare @SetAsInactiveEventTypeId as varchar(15)

declare @ActivationDate as datetime
declare @ActivationRequestDate as datetime
declare @InactiveDate as datetime

declare @ActivatedByUserId as varchar(15)
declare @ActivationRequestedByUserId as varchar(15)
declare @SetAsInactiveByUserId as varchar(15)

declare @CustomerId as varchar(15)
declare @CustomerStatusCode as varchar(10)
declare @Tenant as int
declare @TableId as varchar(15)
set @TableId = (select Id from ObjectTables where Name = 'Customer')

BEGIN 
	DECLARE CustomersCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, CustomerStatusCode
	FROM Customers
	OPEN CustomersCursor FETCH NEXT FROM CustomersCursor INTO @CustomerId, @Tenant, @CustomerStatusCode
	WHILE @@FETCH_STATUS = 0
		BEGIN

			set @ActivationDate = null
			set @ActivationRequestDate = null
			set @InactiveDate = null
			set @ActivatedByUserId = null
			set @ActivationRequestedByUserId = null
			set @SetAsInactiveByUserId = null
			
			set @ActivatedEventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'CSAV' and ObjectTableId = @TableId)	
			set @ActivationRequestEventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'CSWA' and ObjectTableId = @TableId)
			set @SetAsInactiveEventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'CSIN' and ObjectTableId = @TableId)
			
			set @ActivationDate = (select MAX(LogDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @CustomerId and ObjectTableId = @TableId and EventTypeId = @ActivatedEventTypeId)		
			set @ActivatedByUserId = (select UserId from TraceEvents where LogDateTime = @ActivationDate and Tenant = @Tenant and EntityId = @CustomerId and ObjectTableId = @TableId and EventTypeId = @ActivatedEventTypeId)			
			
			set @ActivationRequestDate = (select MAX(LogDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @CustomerId and ObjectTableId = @TableId and EventTypeId = @ActivationRequestEventTypeId)							
			set @ActivationRequestedByUserId = (select UserId from TraceEvents where LogDateTime = @ActivationRequestDate and Tenant = @Tenant and EntityId = @CustomerId and ObjectTableId = @TableId and EventTypeId = @ActivationRequestEventTypeId)			
			
			set @InactiveDate = (select MAX(LogDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @CustomerId and ObjectTableId = @TableId and EventTypeId = @SetAsInactiveEventTypeId)							
			set @SetAsInactiveByUserId = (select UserId from TraceEvents where LogDateTime = @InactiveDate and Tenant = @Tenant and EntityId = @CustomerId and ObjectTableId = @TableId and EventTypeId = @SetAsInactiveEventTypeId)		
					    
			update Customers
			set
			ActivationDate = @ActivationDate,
			ActivationRequestDate = @ActivationRequestDate,
			InactiveDate = @InactiveDate,
			ActivatedByUserId = @ActivatedByUserId,
			ActivationRequestedByUserId = @ActivationRequestedByUserId,
			SetAsInactiveByUserId = @SetAsInactiveByUserId
			where Id = @CustomerId					

			FETCH NEXT FROM CustomersCursor INTO @CustomerId, @Tenant, @CustomerStatusCode
		END
	CLOSE CustomersCursor
	DEALLOCATE CustomersCursor
END