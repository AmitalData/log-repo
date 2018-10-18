
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Currency' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Currencies )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @CurrencySign as nvarchar(3)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE CurrenciesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, Sign, Tenant , Tenant
	From dw_Currencies
	where AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN CurrenciesCursor FETCH NEXT FROM CurrenciesCursor INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from DIM_Currencies where Id = @Id)
	if(@Key is  null) begin  insert into DIM_Currencies values(@Id,@Code , @EnglishName,@LocalName, @CurrencySign , @SourceTenant , @ParentTenant); end
	else begin update   DIM_Currencies set Name =@EnglishName,  LocalName =@LocalName ,  Code = @Code , CurrencySign = @CurrencySign,   SourceTenant = @SourceTenant , ParentTenant = @ParentTenant  Where Id = @Id; end

    

	FETCH NEXT FROM CurrenciesCursor  INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant
		End
	CLOSE CurrenciesCursor
	DEALLOCATE CurrenciesCursor


	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Currency'
End

