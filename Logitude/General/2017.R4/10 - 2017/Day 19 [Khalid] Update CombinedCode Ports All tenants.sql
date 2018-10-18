
  declare @Code varchar(15)
   declare @CountryCode varchar(15)
	declare @CountryId varchar(15)
 declare @CombinedCode varchar(15)
DECLARE @Ports AS VARCHAR(15)
DECLARE PortsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Ports 
	OPEN PortsCursor FETCH NEXT FROM PortsCursor INTO @Ports
	WHILE @@FETCH_STATUS = 0
	BEGIN
	set @CountryId=(select CountryId from Ports where Id=@Ports)
	set @CountryCode=(select Code from Countries where Id= @CountryId)
		set @Code=(select Code from Ports where Id=@Ports) 	
		set @CombinedCode=@CountryCode+@Code
		print @CombinedCode
		update Ports set CombinedCode=@CombinedCode where Id=@Ports
		FETCH NEXT FROM PortsCursor INTO @Ports
		END
CLOSE PortsCursor	
DEALLOCATE PortsCursor		



