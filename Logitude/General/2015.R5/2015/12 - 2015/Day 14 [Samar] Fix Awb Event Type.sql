delete from EventTypes where Code = 'RCF'
go

declare @Tenant as int
declare @EventTypeId as varchar(15)
declare @ObjectTableId as varchar(15)
set @ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')

BEGIN 
              DECLARE TenantsCursor CURSOR READ_ONLY
              FOR
              SELECT Id
              FROM Tenants
              OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
              WHILE @@FETCH_STATUS = 0
                     BEGIN

                     if not exists (select Id from EventTypes where Tenant = @Tenant and Code = 'RCFE')
                     begin                
                                  EXECUTE usp_GetNextTableIdValue @EventTypeId OUTPUT,'EventType'
                                  insert into EventTypes(Id, Tenant, Code, EnglishName, LocalName, EventTypeCategoryCode, SearchFields, ObjectTableId, AddedManually, IsManualEntry, ShortView, IsFollowUp, ManualActivatedFollowUp, InActive, IsCustomerView, IsAgentView, IsSharedLogisticsEnabled)
                                  values(@EventTypeId, @Tenant, 'RCFE', 'Received from Flight', 'Received from Flight', 'LOG', 'LOG,Received from Flight,Received from Flight', @ObjectTableId, 0, 0, 0, 0, 0, 0, 0, 0, 0)                            
                  end                    

                           FETCH NEXT FROM TenantsCursor INTO @Tenant      
                     END
              CLOSE TenantsCursor
              DEALLOCATE TenantsCursor
END
