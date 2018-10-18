
declare @Tenant as int
declare @LeadSourceId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'AD')
			begin
			
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'AD', 'Advertisement', 0, 'AD,Advertisement')
		    end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'EM')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'EM', 'Employee Referral', 0, 'EM,Employee Referral')
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'EX')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'EX', 'External Referral', 0, 'EX,External Referral')
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'PA')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'PA', 'Partner', 0, 'PA,Partner')
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'PU')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'PU', 'Public Relations', 0, 'PU,Public Relations')
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'SE')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'SE', 'Seminar – Internal', 0, 'SE,Seminar – Internal')
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'SP')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'SP', 'Seminar – Partner', 0, 'SP,Seminar – Partner')
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'TS')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'TS', 'Trade Show"', 0, 'TS,Trade Show"')
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'WB')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'WB', 'Web', 0, 'WB,Web')		
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'WM')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'WM', 'Word Of Mouth', 0, 'WM,Word Of Mouth')		
			end

			if not exists (select Id from LeadSources where Tenant = @Tenant and Code = 'OT')
			begin
					EXECUTE usp_GetNextTableIdValue @LeadSourceId OUTPUT,'LeadSource'
					insert into LeadSources(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@LeadSourceId, @Tenant, 'OT', 'Other', 0, 'OT,Other')		
			end

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END