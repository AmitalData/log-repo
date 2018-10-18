declare @NewId as varchar(15)
declare @Tenant as int
declare @ObjectTableId as varchar (15)

BEGIN

		set @Tenant = 0
		set @ObjectTableId = (select Id from ObjectTables where Name = 'Ticket' and Tenant = @Tenant)

		if not exists (select * from DocumentTypes where Code = 'CUA' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'DocumentType'
			INSERT INTO DocumentTypes(Id,Tenant,Name,Code,IsDocIn,ObjectTableId,SearchFields,IsEnabledForCustomers,IsCopiedAtSignup,DocumentTypeCategoryCode,IsAir,IsOcean,IsInland,IsDocOut,InActive,IsMaster,IsHouse,IsDirect,IsDocumentOneTimePrintLimited)
			Values 
			(
			@NewId,
			@Tenant,
			'Customer Attachment',
			'CUA',
			1, 
			@ObjectTableId,
			'CUA,Customer Attachment',
			1,
			1,
		    'O',
			0,
			0,
			0,
			0,
			0,
			0, 
			0,
			0,
			0
			) 
		end

		if not exists (select * from DocumentTypes where Code = 'USA' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'DocumentType'
			INSERT INTO DocumentTypes(Id,Tenant,Name,Code,IsDocIn,ObjectTableId,SearchFields,IsEnabledForCustomers,IsCopiedAtSignup,DocumentTypeCategoryCode,IsAir,IsOcean,IsInland,IsDocOut,InActive,IsMaster,IsHouse,IsDirect,IsDocumentOneTimePrintLimited)
			Values 
			(
			@NewId,
			@Tenant,
			'User attachment',
			'USA',
			1, 
			@ObjectTableId,
			'USA,User attachment',
			1,
			1,
		    'O',
			0,
			0, 
			0,
			0,
			0,
			0, 
			0,
			0,
			0
			) 
		end
end 