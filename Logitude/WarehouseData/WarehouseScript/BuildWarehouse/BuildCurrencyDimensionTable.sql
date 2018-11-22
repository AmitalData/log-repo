

--If(OBJECT_ID('tempdb..#DIM_CurrenciesTemp') Is Not Null)
--Begin
--    Drop Table #DIM_CurrenciesTemp
--End



--CREATE TABLE #DIM_CurrenciesTemp (
--	Id_Number int not null identity(1,1) primary key,
--    Id varchar(15)  not null, 
--	Code varchar(3) not null,
--	Name varchar(40) not null,
--	[Local Name] nvarchar(40),
--	[Currency Sign] nvarchar(3),
--   	[Source Tenant]  int,
--    [Parent Tenant]  int,
--);
insert into #DIM_CurrenciesTemp (Id,Code,Name,[Local Name],[Currency Sign],[Source Tenant],[Parent Tenant]) values ('-1' , 'NOS' ,'Not Specified' ,'Not Specified','NOS', 0,0)
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @CurrencySign as nvarchar(3)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE CurrenciesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Currencies.Id, dw_Currencies.EnglishName , dw_Currencies.LocalName ,dw_Currencies.Code, dw_Currencies.Sign, dw_Currencies.Tenant , dw_DWHSettings.ParentTenant
	From dw_Currencies
	inner JOIN dw_DWHSettings ON dw_Currencies.Tenant = dw_DWHSettings.Tenant
	OPEN CurrenciesCursor FETCH NEXT FROM CurrenciesCursor INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_CurrenciesTemp (Id,Code,Name,[Local Name],[Currency Sign],[Source Tenant],[Parent Tenant]) values(@Id,@Code , @EnglishName,@LocalName, @CurrencySign , @SourceTenant , @ParentTenant)

	FETCH NEXT FROM CurrenciesCursor  INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant
		End
	CLOSE CurrenciesCursor
	DEALLOCATE CurrenciesCursor


IF OBJECT_ID ('NewDIM_Currencies', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Currencies End
		SELECT * 
INTO NewDIM_Currencies
FROM #DIM_CurrenciesTemp

If(OBJECT_ID('tempdb..#DIM_CurrenciesTemp') Is Not Null)
Begin
    Drop Table #DIM_CurrenciesTemp

End

ALTER TABLE NewDIM_Currencies ADD CONSTRAINT PK_NewDIM_Currencies_Id_Number PRIMARY KEY CLUSTERED (Id_Number);
CREATE NONCLUSTERED INDEX [IX_DIM_Currencies_Id] ON [dbo].NewDIM_Currencies([Id])




