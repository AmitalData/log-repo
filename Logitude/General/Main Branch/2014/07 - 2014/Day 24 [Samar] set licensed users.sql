
DECLARE @UserId varchar(15)
declare @Tenant as int

DECLARE TenantsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE UsersCursor CURSOR READ_ONLY
			FOR	
			SELECT Id
			FROM UserLastLogins
			OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @UserId
			WHILE @@FETCH_STATUS = 0
			BEGIN
	
				update Users
				set LicencedUser = 1
				where Id = @UserId and Tenant = @Tenant
	
				FETCH NEXT FROM UsersCursor INTO @UserId
			END	
		CLOSE UsersCursor
		DEALLOCATE UsersCursor	

	FETCH NEXT FROM TenantsCursor INTO @Tenant	
	END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
	