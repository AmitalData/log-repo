
declare @Tenant as int
declare @IndustryId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'WHS')
			begin
			
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'WHS', 'Wholesale', 0, 'WHS,Wholesale')
		    end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'VCR')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'VCR', 'Vehicle Retail', 0, 'VCR,Vehicle Retail')
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'UCD')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'UCD', 'Utility Creation and Distribution', 0, 'UCD,Utility Creation and Distribution')
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'TRN')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'TRN', 'Transportation', 0, 'TRN,Transportation')
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'SRY')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'SRY', 'Specialty Realty', 0, 'SRY,Specialty Realty')
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'SOT')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'SOT', 'Special Outbound Trade Contractors', 0, 'SOT,Special Outbound Trade Contractors')
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'SSV')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'SSV', 'Social Services', 0, 'SSV,Social Services')
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'SIG')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'SIG', 'SIG Affiliations', 0, 'SIG,SIG Affiliations')
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'SVR')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'SVR', 'Service Retail', 0, 'SVR,Service Retail')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'PED')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'PED', 'Petrochemical Extraction and Distributionr', 0, 'PED,Petrochemical Extraction and Distribution')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'OCS')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'OCS', 'Outbound Consumer Service', 0, 'OCS,Outbound Consumer Service')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'NMR')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'NMR', 'Non-Durable Merchandies Retail', 0, 'NMR,Non-Durable Merchandies Retail')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'LSV')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'LSV', 'Legal Services', 0, 'LSV,Legal Services')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'ISR')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'ISR', 'Insurance', 0, 'ISR,Insurance')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'IRS')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'IRS', 'Inbound Repair and Services', 0, 'IRS,Inbound Repair and Services')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'ICP')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'ICP', 'Inbound Capital Intensive Processing', 0, 'ICP,Inbound Capital Intensive Processing')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'FTP')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'FTP', 'Food and Tobacco Processing', 0, 'FTP,Food and Tobacco Processing')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'FIN')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'FIN', 'Financial', 0, 'FIN,Financial')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'ERL')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'ERL', 'Equipment Rental and Leasing', 0, 'ERL,Equipment Rental and Leasing')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'ETR')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'ETR', 'Entertainment Retail', 0, 'ETR,Entertainment Retail')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'EDP')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'EDP', 'Eating and Drinking Places', 0, 'EDP,Eating and Drinking Places')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'DMF')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'DMF', 'Durable Manufacturing', 0, 'DMF,Durable Manufacturing')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'DOC')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'DOC', 'Doctors''s Offices and Clinics', 0, 'Doctors''s Offices and Clinics')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'DDP')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'DDP', 'Distributors, Dispatchers and Processors', 0, 'DDP,Distributors, Dispatchers and Processors')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'DDM')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'DDM', 'Design, Direction and Creative Management', 0, 'DDM,Design, Direction and Creative Management')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'CSS')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'CSS', 'Consumer Service', 0, 'CSS,Consumer Service')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'CLT')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'CLT', 'Consulting', 0, 'CLT,Consulting')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'BSS')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'BSS', 'Business Service', 0, 'BSS,Business Service')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'BSR')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'BSR', 'Building Supply Retail', 0, 'BSR,Building Supply Retail')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'BRK')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'BRK', 'Brokers', 0, 'BRK,Brokers')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'BPP')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'BPP', 'Broadcasting Printing and Publishing', 0, 'BPP,Broadcasting Printing and Publishing')		
			end
			
			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'APN')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'APN', 'Agriculture and Non-petrol Natural Resource Ex"', 0, 'APN,Agriculture and Non-petrol Natural Resource Ex"')		
			end

			if not exists (select Id from Industries where Tenant = @Tenant and Code = 'ACT')
			begin
					EXECUTE usp_GetNextTableIdValue @IndustryId OUTPUT,'Industry'
					insert into Industries(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@IndustryId, @Tenant, 'ACT', 'Accounting', 0, 'ACT,Accounting')		
			end

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END
