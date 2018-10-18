
declare @Tenant as int
declare @EventTypeId as varchar(15)
declare @ObjectTableId as varchar(15)
set @ObjectTableId = (select Id from ObjectTables where Name = 'Customer')

BEGIN 
              DECLARE TenantsCursor CURSOR READ_ONLY
              FOR
              SELECT Id
              FROM Tenants
              OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
              WHILE @@FETCH_STATUS = 0
                     BEGIN

                     if not exists (select Id from EventTypes where Tenant = @Tenant and Code = 'SPOT')
                     begin                
                                  EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
                                  insert into EventTypes(Id, Tenant, Code, EnglishName, AddedManually, IsManualEntry, LocalName, EntityStatusId, ObjectTableId, IsFollowUp,FollowUpEnglishName, FollowUpLocalName, ShortView, ManualActivatedFollowUp, InActive, SearchFields, CustomerRoleId, AgentRoleId, EventTypeCategoryCode, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled, AllowedInAutomation)
                                  values(@EventTypeId, @Tenant, 'SPOT', 'Set as Potential', 0, 0, 'Set as Potential', NULL, @ObjectTableId, 0, NULL, NULL, 1, 0, 0, 'SPOT,Set as Potential', NULL, NULL, 'OPE', 0, 0, 0, 0)                            
                  end                    

                           FETCH NEXT FROM TenantsCursor INTO @Tenant      
                     END
              CLOSE TenantsCursor
              DEALLOCATE TenantsCursor
END