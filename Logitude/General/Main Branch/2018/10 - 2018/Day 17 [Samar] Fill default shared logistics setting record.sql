
declare @Tenant as int
declare @RecordId as varchar(15)

BEGIN 
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
	WHILE @@FETCH_STATUS = 0
		BEGIN

        if not exists (select Id from SharedLogisticsSettings where Tenant = @Tenant)
        begin 
		    set @RecordId = @Tenant
            insert into SharedLogisticsSettings(Id, Tenant, IsAgentShared, IsShipperNotExporterShared, IsNotify1Shared, IsNotify2Shared, IsFreightForwarderShared, IsColoaderShared, IsConsigneeNotImporterShared, IsMainCarrierShared, IsPickDelivCarriesShared, IsInvoicesMenuEnabled, IsMoneyTabEnabled)
            values(@RecordId, @Tenant, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 )                            
        end 
		
           FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END