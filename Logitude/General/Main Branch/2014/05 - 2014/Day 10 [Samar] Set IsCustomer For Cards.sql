
declare @Tenant as int
declare @CustomerId as varchar(15)
declare @IsCustomer as bit

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN					

					BEGIN -- CustomersCursor
						DECLARE CustomersCursor CURSOR READ_ONLY
						FOR
						SELECT Id, IsCustomer
						FROM Customers
						OPEN CustomersCursor FETCH NEXT FROM CustomersCursor INTO @CustomerId, @IsCustomer		
						WHILE @@FETCH_STATUS = 0
							BEGIN

								update Cards
								set IsCustomer = @IsCustomer
								where Id = @CustomerId and Tenant = @Tenant

							FETCH NEXT FROM CustomersCursor INTO @CustomerId, @IsCustomer
						END
						CLOSE CustomersCursor
						DEALLOCATE CustomersCursor
					END

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END