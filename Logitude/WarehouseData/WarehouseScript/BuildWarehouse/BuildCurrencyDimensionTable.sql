
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @CurrencySign as nvarchar(3)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE CurrenciesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Currencies.Id, dw_Currencies.EnglishName , dw_Currencies.LocalName ,dw_Currencies.Code, dw_Currencies.Sign, dw_Currencies.Tenant , dw_DWHSettings.ParentTenant, dw_Currencies.AutomaticLastUpdateDate, dw_Currencies.InActive
	From dw_Currencies
	inner JOIN dw_DWHSettings ON dw_Currencies.Tenant = dw_DWHSettings.Tenant
	OPEN CurrenciesCursor FETCH NEXT FROM CurrenciesCursor INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_CurrenciesTemp (Id,Code,Name,[Local Name],[Currency Sign],[Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id,@Code , @EnglishName,@LocalName, @CurrencySign , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive)

	FETCH NEXT FROM CurrenciesCursor  INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE CurrenciesCursor
	DEALLOCATE CurrenciesCursor

