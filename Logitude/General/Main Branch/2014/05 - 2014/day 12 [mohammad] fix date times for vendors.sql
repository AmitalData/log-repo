declare @Tenant as int
declare @Id as varchar(15)

BEGIN 
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant
		FROM Vendors
		OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			declare @UpdateDate datetime
			declare @createdBy varchar(15)
			declare @updatedBy varchar(15)
			set @UpdateDate = (select top(1) EventDateTime from TraceEvents where EntityId=@Id and Tenant=@Tenant and EventTypeId =(select id from EventTypes where EnglishName like '%updated%' and ObjectTableId=(select id from objecttables where name='Vendor') and Tenant=@Tenant) order by EventDateTime)
			set @updatedBy = (select top(1) UserId from TraceEvents where EntityId=@Id and Tenant=@Tenant and EventTypeId =(select id from EventTypes where EnglishName like '%updated%' and ObjectTableId=(select id from objecttables where name='Vendor') and Tenant=@Tenant) order by EventDateTime)
			set @createdBy = (select UserId from TraceEvents where EntityId=@Id and Tenant=@Tenant and EventTypeId =(select id from EventTypes where EnglishName = 'Created' and ObjectTableId=(select id from objecttables where name='Vendor') and Tenant=@Tenant))

		    update Cards set UpdateDate=@UpdateDate,UpdatedByUserId=@updatedBy,CreatedByUserId=@createdBy where Id=@Id and Tenant=@Tenant



				FETCH NEXT FROM eventsCursor INTO @Id,@Tenant			
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END

select * from Cards where PartnerTypeId='VD'