
BEGIN;
declare @Id as varchar(15)
declare @Tenant as int
declare @CountryId as varchar(15)


	DECLARE CardsCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant,CountryId
	From Cards where (CountryId in (select id from Countries  where Tenant = 0)) and Tenant <> 0
	 
	OPEN CardsCursor FETCH NEXT FROM CardsCursor INTO @Id,@Tenant,@CountryId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	declare @CountryCode as varchar(15)
	set @CountryCode = (select Code from Countries where id = @CountryId)
	update Cards set CountryId = (select id from Countries where Code =@CountryCode and Tenant = @Tenant) where id= @Id
	
	FETCH NEXT FROM CardsCursor INTO  @Id,@Tenant,@CountryId
	END
	CLOSE CardsCursor
	DEALLOCATE CardsCursor
END



