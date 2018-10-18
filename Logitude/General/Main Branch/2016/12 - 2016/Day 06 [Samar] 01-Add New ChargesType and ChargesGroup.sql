insert into ChargesGroups(Code, Name, SearchFields) values ('COMM', 'Commission', 'comm,commission')
go

declare @Tenant as int
declare @ChargesTypeId as varchar(15)
declare @MeasurementId as varchar(15)

BEGIN 
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
	WHILE @@FETCH_STATUS = 0
		BEGIN

        if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = 'AGCM')
        begin 
		
			set @MeasurementId = (select Id from Measurements where Code = 'PRFR' and Tenant = @Tenant)
			               
			EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,'ChargesType'
            insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable, 
			IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
			IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
			ReceivableCreditAccount, PayableDebitAccount, ChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId)

            values('AGCM', 'Agent Commission', 'Agent Commission', @ChargesTypeId, @Tenant, 0, 0, 'COMM', NULL, 0,
			0, 1, 0, 0, 0, 0, NULL, 0, 'AG',
			0, @MeasurementId, NULL, 80, 'AGCM,Agent Commission,Agent Commission', NULL, NULL, 0,
			NULL, NULL, NULL, NULL, NULL, NULL)                            
        end                    

           FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END