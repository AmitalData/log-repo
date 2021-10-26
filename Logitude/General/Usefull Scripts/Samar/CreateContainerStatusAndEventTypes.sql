

declare @Tenant as int
declare @StatusId as varchar(15)
declare @EventTypeId as varchar(15)
declare @TableId as varchar(15)
set @TableId = (select Id from ObjectTables where Name = 'Container')

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				set @StatusId = NULL
				set @EventTypeId = NULL

				--------Empty to Shipper
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'EMPS' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'EMPS')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 10, 'EMPS', 'Empty to Shipper', @TableId, 0, 'EMPS,Empty to Shipper', 'Empty to Shipper', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'EMPS', 'Empty to Shipper', 0, 0,'Empty to Shipper', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'EMPS,Empty to Shipperd')
				end

				--------Picked up at Shipper
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'PICS' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'PICS')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 20, 'PICS', 'Picked up at Shipper', @TableId, 0, 'PICS,Picked up at Shipper', 'Picked up at Shipper', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'PICS', 'Picked up at Shipper', 0, 0,'Picked up at Shipper', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'PICS,Picked up at Shipper')
				end

				--------Gate In
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'GTIN' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'GTIN')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 30, 'GTIN', 'Gate In', @TableId, 0, 'GTIN,Gate In', 'Gate In', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'GTIN', 'Gate In', 0, 0,'Gate In', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'GTIN,Gate In')
				end

				--------Pre Carriage Departed
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'PCDP' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'PCDP')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 40, 'PCDP', 'Pre Carriage Departed', @TableId, 0, 'PCDP,Pre Carriage Departed', 'Pre Carriage Departed', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'PCDP', 'Pre Carriage Departed', 0, 0,'Pre Carriage Departed', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'PCDP,Pre Carriage Departed')
				end

				--------Pre Carriage Arrived
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'PCAV' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'PCAV')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 50, 'PCAV', 'Pre Carriage Arrived', @TableId, 0, 'PCAV,Pre Carriage Arrived', 'Pre Carriage Arrived', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'PCAV', 'Pre Carriage Arrived', 0, 0,'Pre Carriage Arrived', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'PCAV,Pre Carriage Arrived')
				end

				--------POL Departed
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'POLD' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'POLD')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 60, 'POLD', 'POL Departed', @TableId, 0, 'POLD,POL Departed', 'POL Departed', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'POLD', 'POL Departed', 0, 0,'POL Departed', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'POLD,POL Departed')
				end

				--------Transshipment  1 Arrived
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'T1AV' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'T1AV')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 70, 'T1AV', 'Transshipment  1 Arrived', @TableId, 0, 'T1AV,Transshipment  1 Arrived', 'Transshipment  1 Arrived', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'T1AV', 'Transshipment  1 Arrived', 0, 0,'Transshipment  1 Arrived', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'T1AV,Transshipment  1 Arrived')
				end

				--------Transshipment1 Departed
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'T1DT' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'T1DT')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 80, 'T1DT', 'Transshipment1 Departed', @TableId, 0, 'T1DT,Transshipment1 Departed', 'Transshipment1 Departed', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'T1DT', 'Transshipment1 Departed', 0, 0,'Transshipment1 Departed', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'T1DT,Transshipment1 Departed')
				end

				--------Transshipment 2 Arrived
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'T2AV' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'T2AV')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 90, 'T2AV', 'Transshipment 2 Arrived', @TableId, 0, 'T2AV,Transshipment 2 Arrived', 'Transshipment 2 Arrived', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'T2AV', 'Transshipment 2 Arrived', 0, 0,'Transshipment 2 Arrived', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'T2AV,Transshipment 2 Arrived')
				end

				--------Transshipment 2 Departed
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'T2DT' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'T2DT')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 100, 'T2DT', 'Transshipment 2 Departed', @TableId, 0, 'T2DT,Transshipment 2 Departed', 'Transshipment 2 Departed', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'T2DT', 'Transshipment 2 Departed', 0, 0,'Transshipment 2 Departed', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'T2DT,Transshipment 2 Departed')
				end

				--------Transshipment 3 Arrived
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'T3AV' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'T3AV')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 110, 'T3AV', 'Transshipment 3 Arrived', @TableId, 0, 'T3AV,Transshipment 3 Arrived', 'Transshipment 3 Arrived', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'T3AV', 'Transshipment 3 Arrived', 0, 0,'Transshipment 3 Arrived', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'T3AV,Transshipment 3 Arrived')
				end

				--------Transshipment 3 Departed
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'T3DT' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'T3DT')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 120, 'T3DT', 'Transshipment 3 Departed', @TableId, 0, 'T3DT,Transshipment 3 Departed', 'Transshipment 3 Departed', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'T3DT', 'Transshipment 3 Departed', 0, 0,'Transshipment 3 Departed', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'T3DT,Transshipment 3 Departed')
				end

				--------Arrived at POD
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'ARPD' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'ARPD')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 130, 'ARPD', 'Arrived at POD', @TableId, 0, 'ARPD,Arrived at POD', 'Arrived at POD', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'ARPD', 'Arrived at POD', 0, 0,'Arrived at POD', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'ARPD,Arrived at POD')
				end

				--------Discharged
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'DSCH' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'DSCH')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 140, 'DSCH', 'Discharged', @TableId, 0, 'DSCH,Discharged', 'Discharged', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'DSCH', 'Discharged', 0, 0,'Discharged', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'DSCH,Discharged')
				end

				--------Appointment Arranged
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'APAR' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'APAR')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 150, 'APAR', 'Appointment Arranged', @TableId, 0, 'APAR,Appointment Arranged', 'Appointment Arranged', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'APAR', 'Appointment Arranged', 0, 0,'Appointment Arranged', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'APAR,Appointment Arranged')
				end

				--------Gate Out
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'GTOT' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'GTOT')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 160, 'GTOT', 'Gate Out', @TableId, 0, 'GTOT,Gate Out', 'Gate Out', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'GTOT', 'Gate Out', 0, 0,'Gate Out', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'GTOT,Gate Out')
				end

				--------Arrived to Warehouse
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'ARWH' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'ARWH')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 170, 'ARWH', 'Arrived to Warehouse', @TableId, 0, 'ARWH,Arrived to Warehouse', 'Arrived to Warehouse', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'ARWH', 'Arrived to Warehouse', 0, 0,'Arrived to Warehouse', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'ARWH,Arrived to Warehouse')
				end

				--------Available for Delivery
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'AVDL' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'AVDL')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 180, 'AVDL', 'Available for Delivery', @TableId, 0, 'AVDL,Available for Delivery', 'Available for Delivery', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'AVDL', 'Available for Delivery', 0, 0,'Available for Delivery', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'AVDL,Available for Delivery')
				end

				--------Departed from Warehouse
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'DPWH' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'DPWH')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 190, 'DPWH', 'Departed from Warehouse', @TableId, 0, 'DPWH,Departed from Warehouse', 'Departed from Warehouse', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'DPWH', 'Departed from Warehouse', 0, 0,'Departed from Warehouse', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'DPWH,Departed from Warehouse')
				end

				--------Delivered to Consignee
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'DLCO' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'DLCO')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 200, 'DLCO', 'Delivered to Consignee', @TableId, 0, 'DLCO,Delivered to Consignee', 'Delivered to Consignee', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'DLCO', 'Delivered to Consignee', 0, 0,'Delivered to Consignee', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'DLCO,Delivered to Consignee')
				end

				--------Unloaded at Destination
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'UNDS' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'UNDS')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 210, 'UNDS', 'Unloaded at Destination', @TableId, 0, 'UNDS,Unloaded at Destination', 'Unloaded at Destination', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'UNDS', 'Unloaded at Destination', 0, 0,'Unloaded at Destination', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'UNDS,Unloaded at Destination')
				end

				--------POD
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'PODC' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'PODC')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 220, 'PODC', 'POD', @TableId, 0, 'PODC,POD', 'POD', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'PODC', 'POD', 0, 0,'POD', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'PODC,POD')
				end

				--------Empty Return
				set @StatusId = (select Id from EntityStatus where Tenant = @Tenant and Code = 'EMRT' and ObjectTableId = @TableId)
				set @EventTypeId = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @TableId and Code = 'EMRT')

				if @StatusId is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields, DisplayName, AutomaticLastUpdateDate)
					values(@StatusId, @Tenant, 230, 'EMRT', 'Empty Return', @TableId, 0, 'EMRT,Empty Return', 'Empty Return', GETDATE())
				end

				if @EventTypeId is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId, @Tenant, 'EMRT', 'Empty Return', 0, 0,'Empty Return', @StatusId, @TableId, 0,null, null,1,0,0,null,null,NULL,0,0,0,'EMRT,Empty Return')
				end

			FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END