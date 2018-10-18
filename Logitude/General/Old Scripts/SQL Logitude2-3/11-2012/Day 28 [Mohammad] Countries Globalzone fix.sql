
--PLEASE TEST THE CODE VERY CAREFULLY BEFOR EXECUTE 

DECLARE @Tenant AS INT
DECLARE @GlobalZoneId AS varchar(15)
DECLARE @Id AS varchar(15)
 DECLARE @ZeroCode AS varchar(2)
BEGIN; -- update countries for global zones
	DECLARE CountriesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,GlobalZoneId
	FROM Countries 
	where 0=(select tenant from globalzones where id=Countries.GlobalZoneId) and tenant <>0
	OPEN CountriesCursor FETCH NEXT FROM CountriesCursor INTO @Id,@Tenant,@GlobalZoneId
	WHILE @@FETCH_STATUS = 0
	BEGIN

    print @GlobalZoneId
	print @Tenant
	set @ZeroCode = (select code from globalzones where id=@GlobalZoneId)
	print @ZeroCode
	declare @newId varchar(15)
	set @newId=(select Id from globalzones where code=@ZeroCode and tenant=@Tenant)
	print @newId
	update countries set globalzoneid=@newId where id=@Id
	
	  
	FETCH NEXT FROM CountriesCursor INTO  @Id,@Tenant,@GlobalZoneId
	END
	CLOSE CountriesCursor
	DEALLOCATE CountriesCursor
END