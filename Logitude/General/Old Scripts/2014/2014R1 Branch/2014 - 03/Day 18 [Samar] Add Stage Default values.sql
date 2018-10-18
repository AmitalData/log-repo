
declare @Tenant as int
declare @StageId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if not exists (select Id from Stages where Tenant = @Tenant and Code = 'QUA')
			begin
			
					EXECUTE usp_GetNextTableIdValue @StageId OUTPUT,'Stage'
					insert into Stages(Id, Tenant, Code, Name, Probability, SearchFields, IsSelectable, MaxDays, InActive)
					values(@StageId, @Tenant, 'QUA', 'Qualification',20, 'QUA,Qualification,20', 1, null,  0)
		    end

			if not exists (select Id from Stages where Tenant = @Tenant and Code = 'DEV')
			begin
					EXECUTE usp_GetNextTableIdValue @StageId OUTPUT,'Stage'
					insert into Stages(Id, Tenant, Code, Name, Probability, SearchFields, IsSelectable, MaxDays, InActive)
					values(@StageId, @Tenant, 'DEV', 'Development',50, 'DEV,Development,50', 1, null,  0)
			end

			if not exists (select Id from Stages where Tenant = @Tenant and Code = 'QOT')
			begin
					EXECUTE usp_GetNextTableIdValue @StageId OUTPUT,'Stage'
					insert into Stages(Id, Tenant, Code, Name, Probability, SearchFields, IsSelectable, MaxDays, InActive)
					values(@StageId, @Tenant, 'QOT', 'Quote',70, 'QOT,Quote,70', 1, null,  0)
			end

			if not exists (select Id from Stages where Tenant = @Tenant and Code = 'CWN')
			begin
					EXECUTE usp_GetNextTableIdValue @StageId OUTPUT,'Stage'
					insert into Stages(Id, Tenant, Code, Name, Probability, SearchFields, IsSelectable, MaxDays, InActive)
					values(@StageId, @Tenant, 'CWN', 'Closed Won',100, 'CWN,Closed Won,100', 0, null,  0)
			end

			if not exists (select Id from Stages where Tenant = @Tenant and Code = 'CLS')
			begin
					EXECUTE usp_GetNextTableIdValue @StageId OUTPUT,'Stage'
					insert into Stages(Id, Tenant, Code, Name, Probability, SearchFields, IsSelectable, MaxDays, InActive)
					values(@StageId, @Tenant, 'CLS', 'Closed Lost',0, 'CLS,Closed Lost,0', 0, null,  0)
			end

			if not exists (select Id from Stages where Tenant = @Tenant and Code = 'CLC')
			begin
					EXECUTE usp_GetNextTableIdValue @StageId OUTPUT,'Stage'
					insert into Stages(Id, Tenant, Code, Name, Probability, SearchFields, IsSelectable, MaxDays, InActive)
					values(@StageId, @Tenant, 'CLC', 'Lost To competition',0, 'CLC,Lost To competition,0', 0, null,  0)
			end

			if not exists (select Id from Stages where Tenant = @Tenant and Code = 'APV')
			begin
					EXECUTE usp_GetNextTableIdValue @StageId OUTPUT,'Stage'
					insert into Stages(Id, Tenant, Code, Name, Probability, SearchFields, IsSelectable, MaxDays, InActive)
				    values(@StageId, @Tenant, 'APV', 'Approved',90, 'APV,Approved,90', 1, null,  0)
			end			

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END