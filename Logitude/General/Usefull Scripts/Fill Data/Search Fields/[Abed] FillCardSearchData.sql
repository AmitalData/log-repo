





  --DROP FUNCTION SplitBySpaceFunction 



--CREATE FUNCTION dbo.SplitBySpaceFunction ( @stringToSplit VARCHAR(MAX) )
--RETURNS
-- @returnList TABLE ([Name] [nvarchar] (500))
--AS
--BEGIN
-- set @stringToSplit =  RTrim(@stringToSplit)

-- DECLARE @name NVARCHAR(255)
-- DECLARE @pos INT

-- WHILE CHARINDEX(' ', @stringToSplit) > 0
-- BEGIN
--  SELECT @pos  = CHARINDEX(' ', @stringToSplit)  
--  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

--  INSERT INTO @returnList 
--  SELECT @name

--  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
-- END

-- INSERT INTO @returnList
-- SELECT @stringToSplit

-- RETURN
--END






TRUNCATE table CardSearchs

 If(OBJECT_ID('tempdb..#temp_AllKeywords') Is Not Null)
Begin Drop Table #temp_AllKeywords End
CREATE TABLE #temp_AllKeywords([Value] [nvarchar](100) , [FieldName] [nvarchar](70))


If(OBJECT_ID('tempdb..#temp_CardSearchs') Is Not Null)
Begin
    Drop Table #temp_CardSearchs
End

 
CREATE TABLE #temp_CardSearchs
(
	[Id] [varchar](15) NOT NULL,
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


    DECLARE CardCursor CURSOR READ_ONLY
    FOR
    SELECT Id,Tenant, Code,EnglishName , LocalName , VatNumber ,CityName , CountryName , ReceivablesAccountingCard , PayablesAccountingCard , CreateDate ,UpdateDate , PartnerTypeId , InActive
    From Cards 
    OPEN CardCursor FETCH NEXT FROM CardCursor INTO  @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive
    WHILE @@FETCH_STATUS = 0
    BEGIN

 
        begin
		set @Weight = 0
	 DECLARE  @newId varchar(100) ;

	    TRUNCATE TABLE #temp_AllKeywords;
			 BEGIN TRY  
	  	 if (@Code is not null)	begin insert into #temp_AllKeywords (FieldName , Value) select 'Code',  t.Name from dbo.SplitBySpaceFunction(@Code) t end
		if (@EnglishName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'EnglishName',  t.Name from dbo.SplitBySpaceFunction(@EnglishName) t end
		if (@LocalName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'LocalName',  t.Name from dbo.SplitBySpaceFunction(@LocalName) t end
		if (@VatNumber is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'VatNumber',  t.Name from dbo.SplitBySpaceFunction(@VatNumber) t end
		if (@CountryName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'CountryName',  t.Name from dbo.SplitBySpaceFunction(@CountryName) t end
		if (@CityName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'CityName',  t.Name from dbo.SplitBySpaceFunction(@CityName) t end
		if (@ReceivablesAccountingCard is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'ReceivablesAccountingCard',  t.Name from dbo.SplitBySpaceFunction(@ReceivablesAccountingCard) t end
		if (@PayablesAccountingCard is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'PayablesAccountingCard',  t.Name from dbo.SplitBySpaceFunction(@PayablesAccountingCard) t end


		--	if (@Code is not null)	begin insert into #temp_AllKeywords (FieldName , Value) select 'Code',  t.value  FROM STRING_SPLIT(@Code, ' ') t end
		--if (@EnglishName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'EnglishName',   t.value  FROM STRING_SPLIT(@EnglishName, ' ') t end
		--if (@LocalName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'LocalName',   t.value  FROM STRING_SPLIT(@LocalName, ' ') t end
		--if (@VatNumber is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'VatNumber',   t.value  FROM STRING_SPLIT(@VatNumber, ' ') t end
		--if (@CountryName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'CountryName',  t.value  FROM STRING_SPLIT(@CountryName, ' ') t end
		--if (@CityName is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'CityName',   t.value  FROM STRING_SPLIT(@CityName, ' ') t end
		--if (@ReceivablesAccountingCard is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'ReceivablesAccountingCard',  t.value  FROM STRING_SPLIT(@ReceivablesAccountingCard, ' ') t end
		--if (@PayablesAccountingCard is not null)	begin  insert into #temp_AllKeywords (FieldName , Value) select 'PayablesAccountingCard', t.value  FROM STRING_SPLIT(@PayablesAccountingCard, ' ') t end




		  	END TRY 
			BEGIN CATCH  

  declare @Exception as varchar(4000)
  set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);  
  set @Exception = @Exception + ' (@CardId: ' + @CardId +') '+ ' (@Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
  RAISERROR(@Exception, 16, 3);

RETURN;
END CATCH  

    DECLARE @value nvarchar(100)
    DECLARE AllKeywordsCursor CURSOR READ_ONLY
    FOR
    SELECT Value from  #temp_AllKeywords where Value is not null and  Value !=''
    OPEN AllKeywordsCursor FETCH NEXT FROM AllKeywordsCursor INTO @Value
    WHILE @@FETCH_STATUS = 0
    BEGIN
        begin

	     EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'CardSearch'; 

		 declare  @RecordDate datetime
		 set @RecordDate = @UpdateDate;
		 if(@RecordDate is null) set @RecordDate = @CreateDate

		 insert into #temp_CardSearchs (Id, Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive ) values (@newId , @Tenant , @CardId , @Value , @RecordDate , @Weight , @PartnerTypeId,@InActive)

	    set @Count = @Count + 1;
        if(@Count = 4000)
        begin    

		
		    insert into CardSearchs  select  * from #temp_CardSearchs
            truncate table #temp_CardSearchs
            set @Count = 0
            WAITFOR DELAY '00:00:00'

        end



        end

 

    FETCH NEXT FROM AllKeywordsCursor INTO @Value
    END
    CLOSE AllKeywordsCursor
    DEALLOCATE AllKeywordsCursor


        end

 


    FETCH NEXT FROM CardCursor INTO @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive
    END
    CLOSE CardCursor
    DEALLOCATE CardCursor

 
         if (@Count > 0) begin  insert into CardSearchs  select  * from #temp_CardSearchs  end
 
	 
             drop table #temp_CardSearchs
			 drop table #temp_AllKeywords





 

    