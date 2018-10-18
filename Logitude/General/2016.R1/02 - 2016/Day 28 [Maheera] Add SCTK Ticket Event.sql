declare @NewId as varchar(15)
declare @Tenant as int
declare @ObjectTableId as varchar (15)

BEGIN
		set @Tenant = 7
		set @ObjectTableId = (select Id from ObjectTables where Name = 'Ticket')

		if not exists (select * from EventTypes where Code = 'SCTK' and Tenant = @Tenant)
		BEGIN
			EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
			INSERT INTO EventTypes(Id,Tenant,Code,EnglishName,AddedManually,IsManualEntry,LocalName,ObjectTableId,IsFollowUp,ShortView,ManualActivatedFollowUp,InActive,SearchFields,EventTypeCategoryCode,IsCustomerView,IsAgentView,IsSharedLogisticsEnabled)
			Values 
			(
			@NewId,
			@Tenant,
			'SCTK',
			'Ticket Stage Changed',
			0,
			0,
			'Ticket Stage Changed',
			@ObjectTableId,
			0,
			1,
			0,
			0,
			'SCTK,Ticket Stage Changed,Ticket Stage Changed',
			'OPE',
			0,
			0,
			0
			) 
		end
end 