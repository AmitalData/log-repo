

If(OBJECT_ID('tempdb..#DIM_CurrenciesTemp') Is Not Null)
Begin
    Drop Table #DIM_CurrenciesTemp
End



CREATE TABLE #DIM_CurrenciesTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15)  not null, 
	Code varchar(3) not null,
	Name varchar(40) not null,
	LocalName nvarchar(40),
	CurrencySign nvarchar(3),
   	SourceTenant  int,
    ParentTenant  int,
);
insert into #DIM_CurrenciesTemp values ('-1' , 'NOS' ,'Not Specified' ,'Not Specified','NOS', 0,0)
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
	OPEN CurrenciesCursor FETCH NEXT FROM CurrenciesCursor INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_CurrenciesTemp values(@Id,@Code , @EnglishName,@LocalName, @CurrencySign , @SourceTenant , @ParentTenant)

	FETCH NEXT FROM CurrenciesCursor  INTO @Id , @EnglishName, @LocalName, @Code,  @CurrencySign ,	@SourceTenant , @ParentTenant
		End
	CLOSE CurrenciesCursor
	DEALLOCATE CurrenciesCursor

		SELECT * 
INTO NewDIM_Currencies
FROM #DIM_CurrenciesTemp

If(OBJECT_ID('tempdb..#DIM_CurrenciesTemp') Is Not Null)
Begin
    Drop Table #DIM_CurrenciesTemp

End



IF OBJECT_ID ('DIM_Currencies', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Currencies', 'OldDIM_Currencies'

end

EXEC sp_rename 'NewDIM_Currencies', 'DIM_Currencies'


IF OBJECT_ID ('OldDIM_Currencies', 'U')  IS NOT NULL
begin
 IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Currencies_LocalCurrency')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Currencies_LocalCurrency;
    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Currencies_ProfitCurrency;
  end

    drop table OldDIM_Currencies
end



ALTER TABLE DIM_Currencies ADD CONSTRAINT PK_DIM_Currencies_Id_Number PRIMARY KEY CLUSTERED (Id_Number);
  IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Currencies_LocalCurrency  FOREIGN KEY (LocalCurrency) REFERENCES DIM_Currencies(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Currencies_ProfitCurrency  FOREIGN KEY (ProfitCurrency) REFERENCES DIM_Currencies(Id_Number);
 

 end


CREATE NONCLUSTERED INDEX [IX_DIM_Currencies_Id]
ON [dbo].[DIM_Currencies]([Id])

