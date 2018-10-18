

declare @Tenant as int
declare @QuoteTableId as varchar(15)
set @QuoteTableId = (select Id from ObjectTables where Name = 'Quote')

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

				-- Old Statuses
				declare @StatusId_InProgress as varchar(15)
				declare @StatusId_Created as varchar(15)
				declare @StatusId_SentToCustomer as varchar(15)
				declare @StatusId_NoAnswer  as varchar(15)
				declare @StatusId_Used as varchar(15)
				declare @StatusId_Approved as varchar(15)
				declare @StatusId_Rejected as varchar(15)
				set @StatusId_Created = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTCR' and ObjectTableId = @QuoteTableId)
				set @StatusId_InProgress = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTIN' and ObjectTableId = @QuoteTableId)
				set @StatusId_SentToCustomer = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTSC' and ObjectTableId = @QuoteTableId)
				set @StatusId_NoAnswer = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTNA' and ObjectTableId = @QuoteTableId)
				set @StatusId_Used = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTUS' and ObjectTableId = @QuoteTableId)
				set @StatusId_Approved = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTAD' and ObjectTableId = @QuoteTableId)
				set @StatusId_Rejected = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTRJ' and ObjectTableId = @QuoteTableId)
				
				-- New Statuses
				declare @StatusId_Draft as varchar(15)
				declare @StatusId_Sent as varchar(15)
				declare @StatusId_Viewed as varchar(15)
				declare @StatusId_InDiscussion as varchar(15)
				declare @StatusId_Accepted as varchar(15)
				declare @StatusId_Declined as varchar(15)
				set @StatusId_Draft = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTDR' and ObjectTableId = @QuoteTableId)
				set @StatusId_Sent = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTST' and ObjectTableId = @QuoteTableId)
				set @StatusId_Viewed = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTVW' and ObjectTableId = @QuoteTableId)
				set @StatusId_InDiscussion = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTID' and ObjectTableId = @QuoteTableId)
				set @StatusId_Accepted = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTAC' and ObjectTableId = @QuoteTableId)
				set @StatusId_Declined = (select Id from EntityStatus where Tenant = @Tenant and Code = 'QTDC' and ObjectTableId = @QuoteTableId)

				-- if not exists Add Status: Draft
				if @StatusId_Draft is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId_Draft OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields)
					values(@StatusId_Draft, @Tenant, 0, 'QTDR', 'Draft', @QuoteTableId, 0,null)
				end

				-- if not exists Add Status: Sent
				if @StatusId_Sent is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId_Sent OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields)
					values(@StatusId_Sent, @Tenant, 1, 'QTST', 'Sent', @QuoteTableId, 0,null)
				end

				-- if not exists Add Status: Viewed
				if @StatusId_Viewed is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId_Viewed OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields)
					values(@StatusId_Viewed, @Tenant, 2, 'QTVW', 'Viewed', @QuoteTableId, 0,null)
				end

				-- if not exists Add Status: In Discussion
				if @StatusId_InDiscussion is null 
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId_InDiscussion OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields)
					values(@StatusId_InDiscussion, @Tenant, 3, 'QTID', 'In Discussion', @QuoteTableId, 0,null)
				end

				-- if not exists Add Status: Accepted
				if @StatusId_Accepted is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId_Accepted OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields)
					values(@StatusId_Accepted, @Tenant, 4, 'QTAC', 'Accepted', @QuoteTableId, 0,null)
				end

				-- if not exists Add Status: Declined
				if @StatusId_Declined is null
				begin
					EXECUTE usp_GetNextTableIdValue @StatusId_Declined OUTPUT,'EntityStatus'
					insert into EntityStatus(Id, Tenant, StatusWeight, Code, Name, ObjectTableId, InActive, SearchFields)
					values(@StatusId_Declined, @Tenant, 4, 'QTDC', 'Declined', @QuoteTableId, 0,null)
				end


				update Quotes set EntityStatusId = @StatusId_Draft where Tenant = @Tenant and EntityStatusId = @StatusId_InProgress
				update Quotes set EntityStatusId = @StatusId_Draft where Tenant = @Tenant and EntityStatusId = @StatusId_Created
				update Quotes set EntityStatusId = @StatusId_Sent where Tenant = @Tenant and EntityStatusId = @StatusId_SentToCustomer
				update Quotes set EntityStatusId = @StatusId_Accepted where Tenant = @Tenant and EntityStatusId = @StatusId_NoAnswer
				update Quotes set EntityStatusId = @StatusId_Accepted where Tenant = @Tenant and EntityStatusId = @StatusId_Used
				update Quotes set EntityStatusId = @StatusId_Accepted where Tenant = @Tenant and EntityStatusId = @StatusId_Approved
				update Quotes set EntityStatusId = @StatusId_Declined where Tenant = @Tenant and EntityStatusId = @StatusId_Rejected

				-- Old EventTypes
				declare @EventTypeId_Created as varchar(15)
				declare @EventTypeId_InProgress as varchar(15)
				declare @EventTypeId_Approved as varchar(15)
				declare @EventTypeId_Rejected as varchar(15)
				declare @EventTypeId_NoAnswer as varchar(15)
				declare @EventTypeId_SentToCustomer as varchar(15)
				declare @EventTypeId_ReturnInProgress as varchar(15)
				declare @EventTypeId_Used as varchar(15)
				set @EventTypeId_Created = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'CRQT')
				set @EventTypeId_InProgress = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'QTIN')
				set @EventTypeId_Approved = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'CLAD')
				set @EventTypeId_Rejected = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'SARC')
				set @EventTypeId_NoAnswer = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'SANA')
				set @EventTypeId_SentToCustomer = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'SASC')
				set @EventTypeId_ReturnInProgress = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'RPRG')
				set @EventTypeId_Used = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'SHPU')
				
				-- New EventTypes
				declare @EventTypeId_Viewed as varchar(15)
				declare @EventTypeId_Accepted as varchar(15)
				declare @EventTypeId_Declined as varchar(15)
				declare @EventTypeId_InDiscussion as varchar(15)
				declare @EventTypeId_ReturnToDraft as varchar(15)
				set @EventTypeId_Viewed = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'QTVI')
				set @EventTypeId_Accepted = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'QTCP')
				set @EventTypeId_Declined = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'QTDL')
				set @EventTypeId_InDiscussion = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'QTDS')
				set @EventTypeId_ReturnToDraft = (select Id from EventTypes where Tenant = @Tenant and ObjectTableId = @QuoteTableId and Code = 'RQTD')

				-- if not exists Add EventType: Viewed
				if @EventTypeId_Viewed is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId_Viewed OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId_Viewed, @Tenant, 'QTVI', 'Quote Viewed', 0, 0,'Quote Viewed', @StatusId_Viewed, @QuoteTableId, 0,null, null,1,0,0,null,null,'OPE',0,0,0,'QTVI,Quote Viewed,Quote Viewed')
				end

				-- if not exists Add EventType: In Discussion
				if @EventTypeId_InDiscussion is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId_InDiscussion OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId_InDiscussion, @Tenant, 'QTDS', 'Quote In Discussion', 0, 0,'Quote In Discussion', @StatusId_Viewed, @QuoteTableId, 0,null, null,1,0,0,null,null,'OPE',0,0,0,'QTDS,Quote In Discussion,Quote In Discussion')
				end

				-- if not exists Add EventType: ReturnToDraft
				if @EventTypeId_ReturnToDraft is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId_ReturnToDraft OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId_ReturnToDraft, @Tenant, 'RQTD', 'Return To Draft', 0, 0,'Return To Draft', @StatusId_Viewed, @QuoteTableId, 0,null, null,1,0,0,null,null,'OPE',0,0,0,'RQTD,Return To Draft,Return To Draft')
				end

				-- if not exists Add EventType: Declined
				if @EventTypeId_Declined is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId_Declined OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId_Declined, @Tenant, 'QTDL', 'Quote Declined', 0, 0,'Quote Declined', @StatusId_Viewed, @QuoteTableId, 0,null, null,1,0,0,null,null,'OPE',0,0,0,'QTDL,Quote Declined,Quote Declined')
				end

				-- if not exists Add EventType: Accepted
				if @EventTypeId_Accepted is null
				begin
					EXECUTE usp_GetNextTableIdValue @EventTypeId_Accepted OUTPUT,'EventType'
					insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp, FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, SearchFields)
					values(@EventTypeId_Accepted, @Tenant, 'QTCP', 'Quote Accepted', 0, 1,'Quote Accepted', @StatusId_Viewed, @QuoteTableId, 1,'Customer Accepted', 'Customer Accepted',1,1,0,null,null,'OPE',0,0,0,'QTCP,Quote Accepted,Quote Accepted')
				end

				update EventTypes set EntityStatusId = @StatusId_Sent where EntityStatusId = @StatusId_SentToCustomer and Tenant = @Tenant and ObjectTableId = @QuoteTableId
				update EventTypes set EntityStatusId = @StatusId_Draft where EntityStatusId = @StatusId_Created and Tenant = @Tenant and ObjectTableId = @QuoteTableId

				update TraceEvents set EventTypeId = @EventTypeId_Created where EventTypeId = @EventTypeId_InProgress and Tenant = @Tenant
				update TraceEvents set EventTypeId = @EventTypeId_Accepted where EventTypeId = @EventTypeId_Approved and Tenant = @Tenant
				update TraceEvents set EventTypeId = @EventTypeId_Declined where EventTypeId = @EventTypeId_Rejected and Tenant = @Tenant
				update TraceEvents set EventTypeId = @EventTypeId_ReturnToDraft where EventTypeId = @EventTypeId_ReturnInProgress and Tenant = @Tenant

				update FollowUps set EventTypeId = @EventTypeId_Accepted where EventTypeId = @EventTypeId_Approved and Tenant = @Tenant

				delete from TraceEvents where Tenant = @Tenant and ObjectTableId = @QuoteTableId and EventTypeId = @EventTypeId_Used
				delete from TraceEvents where Tenant = @Tenant and ObjectTableId = @QuoteTableId and EventTypeId = @EventTypeId_NoAnswer

			FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END


--select * from EventTypes where EntityStatusId = '1-25'

delete from EventTypes where Code = 'QTIN' --InProgress
delete from EventTypes where Code = 'CLAD' --Approved
delete from EventTypes where Code = 'SARC' --Rejected
delete from EventTypes where Code = 'RPRG' --ReturnInProgress
delete from EventTypes where Code = 'SANA' --No Answer
delete from EventTypes where Code = 'SHPU' --Used


--delete from EntityStatus where Code = 'QTCR' --Created (Connected to so many tables)
delete from EntityStatus where Code = 'QTIN' --InProgress
delete from EntityStatus where Code = 'QTAD' --Approved
delete from EntityStatus where Code = 'QTRJ' --Rejected
delete from EntityStatus where Code = 'QTNA' --No Answer
delete from EntityStatus where Code = 'QTUS' --Used	
delete from EntityStatus where Code = 'QTSC' --Sent To Customer		

update Quotes
set IsClosed = 1
where EntityStatusId in (select Id from EntityStatus where Code = 'QTAC' or Code = 'QTDC')

--select * from TraceEvents where Tenant = 1 and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
--go

--select * from EventTypes where Tenant = 1 and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
--go

--select * from EntityStatus where Tenant = 1 and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
--go

select * from EntityStatus where Code = 'QTSC' --Sent To Customer	