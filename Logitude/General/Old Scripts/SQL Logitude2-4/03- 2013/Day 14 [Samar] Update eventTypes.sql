-- excute on main DB before update

DECLARE @Tenant AS INT
BEGIN;

	DECLARE TenantCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants	 
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN

		delete from TraceEvents where EventTypeId = (select Id from EventTypes where code = 'UMas' and Tenant = @Tenant) 
		delete from TraceEvents where EventTypeId = (select Id from EventTypes where code = 'SCHM' and Tenant = @Tenant)
		delete from TraceEvents where EventTypeId = (select Id from EventTypes where code = 'RMCL' and Tenant = @Tenant)

	FETCH NEXT FROM TenantCursor INTO @Tenant
	END
	CLOSE TenantCursor
	DEALLOCATE TenantCursor	
END
go

update EventTypes
		set Code = 'USHI'
		where code COLLATE Latin1_General_CS_AS = 'UShi' COLLATE Latin1_General_CS_AS
--

--select * from EventTypes where code COLLATE Latin1_General_CS_AS = 'USHI' COLLATE Latin1_General_CS_AS

delete from EventTypes where code COLLATE Latin1_General_CS_AS = 'UShi' COLLATE Latin1_General_CS_AS
go

delete from EventTypes where code like 'UMas' 
go

delete from EventTypes where code = 'SCHM'
go

delete from EventTypes where code = 'RMCL'
go


