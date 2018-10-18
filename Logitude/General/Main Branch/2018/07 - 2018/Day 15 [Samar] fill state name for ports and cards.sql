
declare @Tenant as int
declare @Id as varchar(15)
declare @StateId as varchar(15)
declare @StateName as varchar(40)

--Ports
BEGIN 
	DECLARE PortsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, StateId
	FROM [Ports]
	WHERE StateId is not null
	OPEN PortsCursor FETCH NEXT FROM PortsCursor INTO @Id, @Tenant, @StateId    
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @StateName = (select EnglishName from States where Id = @StateId and Tenant = @Tenant)
		
		update [Ports] set StateName = @StateName where Id = @Id and Tenant = @Tenant
		                  
		FETCH NEXT FROM PortsCursor INTO  @Id, @Tenant, @StateId        
    END
    CLOSE PortsCursor
    DEALLOCATE PortsCursor
END

--Cards
BEGIN 
	DECLARE CardsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Cards
	OPEN CardsCursor FETCH NEXT FROM CardsCursor INTO @Id, @Tenant    
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @StateId = (select StateId from Addresses where Tenant = @Tenant and CardId = @Id and AddressTypeId = 'M')

		if (@StateId is not null)
		begin
			set @StateName = (select EnglishName from States where Id = @StateId and Tenant = @Tenant)		
			update Cards set StateName = @StateName where Id = @Id and Tenant = @Tenant
		end
			              
		FETCH NEXT FROM CardsCursor INTO  @Id, @Tenant        
    END
    CLOSE CardsCursor
    DEALLOCATE CardsCursor
END