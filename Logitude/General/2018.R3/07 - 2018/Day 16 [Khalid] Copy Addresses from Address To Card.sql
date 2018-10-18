	
declare @Tenant as int
declare @Id as varchar(15)
declare @AddressId as varchar(15)

declare @Address1 as varchar(65)
declare @Address2 as varchar(65)

	
	DECLARE CardsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Cards
	OPEN CardsCursor FETCH NEXT FROM CardsCursor INTO @Id, @Tenant    
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @AddressId = (select Id from Addresses where Tenant = @Tenant and CardId = @Id and AddressTypeId = 'M')
		
		if (@AddressId is not null)
		begin
			set @Address1 = (select Address1 from Addresses where Id = @AddressId and Tenant = @Tenant)	
			set @Address2 = (select Address2 from Addresses where Id = @AddressId and Tenant = @Tenant)		
			update Cards set Address1 = @Address1,Address2=@Address2 where Id = @Id and Tenant = @Tenant
		end
		
		
				              
		FETCH NEXT FROM CardsCursor INTO  @Id, @Tenant        
    END
    CLOSE CardsCursor
    DEALLOCATE CardsCursor