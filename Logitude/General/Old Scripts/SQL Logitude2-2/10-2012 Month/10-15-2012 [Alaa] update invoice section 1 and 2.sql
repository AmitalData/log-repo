DECLARE @NewLineChar AS CHAR(2) = CHAR(13) + CHAR(10)
DECLARE @Id int
declare @AddressId varchar(15)
declare @countryId varchar(15)
declare @StateId varchar(15)
declare @Address1 nvarchar(65)
declare @Address2 nvarchar(65)
declare @City nvarchar(25)
declare @CountryCode varchar(2)
declare @State varchar(2)
declare @ZipCode varchar(15)
declare @Phone varchar(40)
declare @Fax varchar(40)

declare @Company varchar(100)
DECLARE updatecursor CURSOR READ_ONLY
FOR
SELECT Id,AddressId,Company
FROM tenants

OPEN updatecursor

	FETCH NEXT FROM updatecursor
	INTO @Id,@AddressId,@Company


WHILE @@FETCH_STATUS = 0

BEGIN
	
	set @Address1= (select address1 from Addresses where id=@AddressId)
	set @Address2= (select address2 from Addresses where id=@AddressId)
	set @City= (select City from Addresses where id=@AddressId)
	set @ZipCode= (select ZipCode from Addresses where id=@AddressId)
	set @Phone= (select PhoneNumber from Addresses where id=@AddressId)
	set @Fax= (select FaxNumber from Addresses where id=@AddressId)
	set @countryId= (select CountryId from Addresses where id=@AddressId)
	set @StateId= (select StateId from Addresses where id=@AddressId)
	set @CountryCode= (select code from Countries where id=@countryId)	
	set @State=(select code from states where id=@StateId)

	declare @value nvarchar(500)
	set @value=@Company
	set @value=@value+@NewLineChar
	--print @Id
	--print @Address1


	if (@Address1 is not NULL and @Address2 is not null)
	BEGIN
	set @value=@value+@Address1+ ',' +@Address2+@NewLineChar 

	END

	if (@Address1 is not NULL and @Address2 is null)
	begin
	set @value=@value+@Address1+@NewLineChar
	end

	if (@Address1 is NULL and @Address2 is not null)
	begin

	set @value=@value+@Address2+@NewLineChar	
	end


	if (@City is not NULL OR @State is not null OR @CountryCode is not null OR @ZipCode is not null )
	begin

	IF (@City is not null and @CountryCode  is null)
	begin 
	set @value = @value + @City	+ SPACE(1)
	end

	IF ( @CountryCode is not null  and @City is null)
	begin
	set @value=@value +  @CountryCode + SPACE(1)
	end
	
	if (@City is not null and @CountryCode is not null)
	begin
	set @value = @value + @City + '-' +@CountryCode + SPACE(1) 
	end

	if ( @State is not null)
	begin
	set 
	 @value=@value+ '('+  @State + ')' 	+ SPACE(1)
	 end

	 if ( @ZipCode is not null)	
	begin
      set @value = @value + @ZipCode
	  end
	  set @value = @value + @NewLineChar
	end	


	if (@Phone is not NULL and @Fax is not null )
	begin
	set @value= @value+ 'Tel:' + '[' +  @Phone+ ']'+ Space(1) + 'Fax:'+ '[' +  @Fax + ']'+@NewLineChar
	end
	
	if (@Phone is not NULL and @Fax is  null )
	begin
	set @value=@value+'Tel:' + '[' + @Phone  + ']'+@NewLineChar
	end

	if (@Phone is NULL and @Fax is not null )
	begin
	set @value=@value+ 'Fax:'+ '[' + @Fax +']'+@NewLineChar
	end
	print @value

	update tenants set InvoiceSection1=@value
	where Id=@Id

	FETCH NEXT FROM updatecursor
	INTO  @Id,@AddressId,@Company
END

CLOSE updatecursor
DEALLOCATE updatecursor




