

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





TRUNCATE table CardSearches


If(OBJECT_ID('tempdb..#temp_CardSearches') Is Not Null)
Begin
    Drop Table #temp_CardSearches
End

 
CREATE TABLE #temp_CardSearches
(
[Id] INT IDENTITY(1,1) NOT NULL,
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



    Declare @SearchField nvarchar(max)


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

	  declare  @RecordDate datetime
		 set @RecordDate = @UpdateDate;
		 if(@RecordDate is null) set @RecordDate = @CreateDate

			 BEGIN TRY  

			 set @SearchField = @Code;
			 if(@EnglishName is not null) set @SearchField += (' ' + @EnglishName);
			 if(@LocalName is not null) set @SearchField += (' ' + @LocalName);
			 if(@VatNumber is not null) set @SearchField += (' ' + @VatNumber);
			 if(@CountryName is not null) set @SearchField += (' ' + @CountryName);
			 if(@CityName is not null) set @SearchField += (' ' + @CityName);
			 if(@ReceivablesAccountingCard is not null) set @SearchField += (' ' + @ReceivablesAccountingCard);
			 if(@PayablesAccountingCard is not null) set @SearchField += (' ' + @PayablesAccountingCard);


			
			 insert into #temp_CardSearches (Tenant, CardId  , RecordDate , Weight , PartnerTypeId,InActive , Keyword) select  @Tenant, @CardId , @RecordDate , @Weight , @PartnerTypeId,@InActive ,  Name from dbo.SplitBySpaceFunction(@SearchField) where Name !=' '


	  set @Count = @Count + 1;
        if(@Count = 500000)
        begin    

		    insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive from #temp_CardSearches
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

 


    FETCH NEXT FROM CardCursor INTO @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive
    END
    CLOSE CardCursor
    DEALLOCATE CardCursor

 
        if (@Count > 0) begin  insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive from #temp_CardSearches end
 
	 
             drop table #temp_CardSearches

			 
			

     

 

    