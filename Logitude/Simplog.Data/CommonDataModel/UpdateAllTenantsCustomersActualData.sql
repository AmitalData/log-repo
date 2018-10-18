

if OBJECT_ID('[dbo].[CustomerActualDataHistory]', 'U') IS NULL
	CREATE TABLE CustomerActualDataHistory
	 (
	 Id int not null identity,
	 Tenant int not null,
	 StartDateTime datetime,
	 EndDateTime datetime, 
	 PRIMARY KEY (Id)
	 )
GO

IF OBJECT_ID('[dbo].[usp_UpdateAllTenantsCustomersActualData]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateAllTenantsCustomersActualData]
GO

Create PROCEDURE [dbo].[usp_UpdateAllTenantsCustomersActualData]
AS

declare @Tenant integer

BEGIN 

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0

	BEGIN
		

			EXECUTE usp_UpdateTenantCustomersActualData @Tenant


	FETCH NEXT FROM TenantsCursor INTO @Tenant	
	END

	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor

END