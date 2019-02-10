
declare @UserId as varchar(15)
declare @ContactTenantId as varchar(15)
declare @Tenant as int
declare @RoleId as varchar(15)
declare @UserRoles as varchar(300)

BEGIN
		DECLARE UsersCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM Users			
		OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @UserId, @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
			set @UserRoles = null

			set @ContactTenantId = (select Id from ContactTenants where ContactId = @UserId and TenantId = @Tenant)
			
			if(@ContactTenantId is not null)
			begin
			
				BEGIN
				DECLARE RolesCursor CURSOR READ_ONLY
				FOR
				SELECT RoleId
				FROM ContactTenantRoleSet	
				WHERE Tenant = @Tenant and ContactTenantId = @ContactTenantId
				OPEN RolesCursor FETCH NEXT FROM RolesCursor INTO @RoleId
				WHILE @@FETCH_STATUS = 0
				BEGIN
		
					if(@UserRoles is null)
					begin
						set @UserRoles = (select Name from Roles where Id = @RoleId)
					end

					else
					begin 
						set @UserRoles = @UserRoles + ', ' + (select Name from Roles where Id = @RoleId)
					end

				FETCH NEXT FROM RolesCursor INTO @RoleId
				END				
				CLOSE RolesCursor
				DEALLOCATE RolesCursor
				END
				--------------------------------------------

				update Users set UserRoles = @UserRoles where Id = @UserId and Tenant = @Tenant
			end

		FETCH NEXT FROM UsersCursor INTO @UserId, @Tenant

		END				
		CLOSE UsersCursor
		DEALLOCATE UsersCursor
END