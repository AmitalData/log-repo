
declare @Tenant as int
declare @ChargesTypeId as varchar(15)
declare @MeasurementId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @NewEntityId as varchar(15)
BEGIN 
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
	WHILE @@FETCH_STATUS = 0
		BEGIN
		
				if not exists (select * from ChargesGroups where   Tenant = @Tenant AND Code = 'NONE')
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'ChargesGroup'
					insert into ChargesGroups (Code, Name, SearchFields, Id, Tenant,LocalName,ViewOrder) values ('NONE','None','none,none',@NewEntityId, @Tenant, 'None',100)
				end

				set @ChargesGroupId = (select Id from ChargesGroups where   Tenant = @Tenant AND Code = 'NONE')
			    set @ChargesGroupCode = (select Code from ChargesGroups where   Tenant = @Tenant AND Code = 'NONE')

						
		      if not exists (select * from Measurements where   Tenant = @Tenant AND Code = 'FIXD')
				begin
					EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'Measurement'
					insert into Measurements (Code, Name, ShortName, Id, Tenant,IsContainerMeasurement,IsContainer,InActive,SearchFields,LocalName) values ('FIXD','Fixed','Fixed',@NewEntityId, @Tenant,0,0,0,'FIXD,Fixed,Fixed',NULL)
				end
			set @MeasurementId = (select Id from Measurements where Code = 'FIXD' and Tenant = @Tenant)

        if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = 'INT')
        begin 
              
			EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,'ChargesType'
            insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable, 
			IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
			IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
			ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
			ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
			IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
			ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax)

            values('INT', 'Interest', N'ריבית', @ChargesTypeId, @Tenant, 0, 0, @ChargesGroupCode , NULL, 1,
			1, 1, 1, 1, 0, 0, NULL, 1, NULL,
			0, @MeasurementId, NULL, 100, N'INT,Interest,ריבית', NULL, NULL, 0,
			NULL, NULL, NULL, NULL, NULL, NULL,
			@ChargesGroupId ,NULL,0,0,0,0,NULL,0,0,0,0,NULL,NULL,0)                            
        end                    

           FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END