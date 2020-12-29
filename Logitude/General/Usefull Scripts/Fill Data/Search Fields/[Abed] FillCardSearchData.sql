
If(OBJECT_ID('tempdb..#temp_CardSearches') Is Not Null)
Begin
    Drop Table #temp_CardSearches
End

 
CREATE TABLE #temp_CardSearches
(
	[Tenant] [int] NOT NULL,
	[RecordDate] [datetime] NOT NULL,
	[Keyword] [nvarchar](100) NULL,
	[Weight] [int] NOT NULL,
	[CardId] [varchar](15) NULL,
	[PartnerTypeId] [varchar](2) not NULL,
    [InActive] bit,
)




declare  @Tenant int
declare @Count as int
set @Count = 0;

declare  @CardId varchar(15)
declare  @EnglishName varchar(70)
declare  @LocalName nvarchar(100)
declare  @VatNumber varchar(20)
declare  @CityName nvarchar(25)
declare  @CountryName varchar(120)
declare  @Code varchar(15)
declare  @ReceivablesAccountingCard varchar(25)
declare  @PayablesAccountingCard varchar(25)
declare  @CreateDate datetime
declare  @UpdateDate datetime

declare  @Weight int

declare  @PartnerTypeId varchar(2)
declare  @InActive bit
declare  @IsCustomer bit



    Declare @SearchField nvarchar(max)


    DECLARE CardCursor CURSOR READ_ONLY
    FOR
    SELECT Id,Tenant, Code,EnglishName , LocalName , VatNumber ,CityName , CountryName , ReceivablesAccountingCard , PayablesAccountingCard , CreateDate ,UpdateDate , PartnerTypeId , InActive , IsCustomer
    From Cards  where Id not in (select CardId from CardSearches )
    OPEN CardCursor FETCH NEXT FROM CardCursor INTO  @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive , @IsCustomer
    WHILE @@FETCH_STATUS = 0
    BEGIN

 
        begin
		set @Weight = 0
	 DECLARE  @newId varchar(100) ;

	  declare  @RecordDate datetime
		 set @RecordDate = @UpdateDate;
		 if(@RecordDate is null) set @RecordDate = @CreateDate

			 BEGIN TRY  

		if (@Code is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,@IsCustomer,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@Code , 90 , 90) t where KeyWord !=' ' end
		if (@EnglishName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,@IsCustomer,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@EnglishName , 100 , 90) t where KeyWord !=' ' end
		if (@LocalName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,@IsCustomer,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@LocalName , 100 , 90) t where KeyWord !=' ' end
		if (@VatNumber is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@VatNumber , 100 , 100) t where KeyWord !=' ' end
		if (@CountryName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,@IsCustomer ,   t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CountryName , 50 , 50) t where KeyWord !=' ' end
		if (@CityName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CityName , 40 , 40) t where KeyWord !=' ' end
		if (@ReceivablesAccountingCard is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,@IsCustomer,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ReceivablesAccountingCard , 80 , 80) t where KeyWord !=' ' end
		if (@PayablesAccountingCard is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive ,IsCustomer, Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive , @IsCustomer ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@PayablesAccountingCard , 80 , 80) t where KeyWord !=' ' end

	  set @Count = @Count + 1;
        if(@Count = 500000)
        begin    

		    insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive , IsCustomer) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive , IsCustomer from #temp_CardSearches
            truncate table #temp_CardSearches
            set @Count = 0
        end




		  	END TRY 
			BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (@CardId: ' + @CardId +') '+ ' (@Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

    

        end

 


    FETCH NEXT FROM CardCursor INTO @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive , @IsCustomer
    END
    CLOSE CardCursor
    DEALLOCATE CardCursor

 
        if (@Count > 0) begin  insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive ,IsCustomer) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive,IsCustomer from #temp_CardSearches end
 
	 
             drop table #temp_CardSearches

			 