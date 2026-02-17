 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Card' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Partners )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin


   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(70)
   declare @Code as varchar(15)
   declare @LocalName as nvarchar(100)
   declare @City as nvarchar(25)
   declare @Country as varchar(120)
   declare @State as varchar(40)
   declare @ZipCode as varchar(15)
   declare @PrimaryContact as varchar(60)
   declare @AccountManager as varchar(60)
   declare @Salesman as varchar(60)
   declare @Rank as varchar(40)
   declare @PartnerType as varchar(20)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @CountryCode as varchar(2)
   declare @PrimaryContactEmail as varchar(70)
   declare @ReceivablesAccountingCard as varchar(25)
   declare @PayablesAccountingCard as varchar(25)


	DECLARE PartnersCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Partners.Id,dw_Partners.Code, dw_Partners.EnglishName,dw_Partners.LocalName , dw_Partners.CityName, dw_Partners.CountryName , dw_States.EnglishName , dw_Partners.ZipCode, dw_Contacts.EnglishName ,accountManagerUser.EnglishName, salesmanUser.EnglishName ,dw_Ranks.Name,dw_PartnerTypes.Name, dw_Partners.Tenant, dw_DWHSettings.ParentTenant,dw_Countries.Code,dw_Contacts.Email , dw_Partners.ReceivablesAccountingCard,dw_Partners.PayablesAccountingCard
	From dw_Partners

	left JOIN dw_Customers ON dw_Partners.Id = dw_Customers.Id
	inner JOIN dw_Contacts ON dw_Partners.PrimaryContactId = dw_Contacts.Id
	left join dw_Contacts accountManagerUser on dw_Customers.AccountManagerUserId=accountManagerUser.Id
	inner join dw_Contacts salesmanUser on dw_Partners.SalesmanUserId=salesmanUser.Id
	left join dw_Ranks  on dw_Customers.RankId=dw_Ranks.Id
	left join dw_Addresses  on dw_Partners.Id = dw_Addresses.Id
    left join dw_States  on dw_Addresses.StateId = dw_States.Id
	inner join dw_PartnerTypes  on dw_Partners.PartnerTypeId=dw_PartnerTypes.Id
	inner JOIN dw_DWHSettings ON dw_Partners.Tenant = dw_DWHSettings.Tenant
	inner JOIN dw_Countries ON dw_Partners.CountryId = dw_Countries.Id
	where dw_Partners.AutomaticLastUpdateDate > @LastUpdateDate	

	OPEN PartnersCursor FETCH NEXT FROM PartnersCursor INTO @Id ,@Code, @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant,@CountryCode,@PrimaryContactEmail , @ReceivablesAccountingCard,@PayablesAccountingCard
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from DIM_Partners where Id = @Id)

	if(@Key is  null) begin insert into DIM_Partners (Id,Code,Name,[Local Name],City,Country,[State Name],[Zip Code],[Primary Contact],[Account Manager],Salesman,[Customer Rank],[Partner Type],[Source Tenant],[Parent Tenant] ,[Country Code],[Primary Contact Email] , [Receivables Accounting Card] ,[Payables Accounting Card] ) values(@Id,@Code, @Name,@LocalName ,@City,@Country, @State, @ZipCode , @PrimaryContact , @AccountManager , @Salesman ,@Rank , @PartnerType,  @SourceTenant , @ParentTenant,@CountryCode,@PrimaryContactEmail, @ReceivablesAccountingCard,@PayablesAccountingCard)end
	else begin update   DIM_Partners set Name =@Name,Code= @Code, [Local Name] =@LocalName ,  City = @City , Country = @Country,  [State Name] = @State, [Zip Code] = @ZipCode ,  [Primary Contact] = @PrimaryContact,[Account Manager] = @AccountManager,Salesman = @Salesman, [Customer Rank] = @Rank,[Partner Type] = @PartnerType , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant ,[Country Code] = @CountryCode,[Primary Contact Email] = @PrimaryContactEmail , [Receivables Accounting Card] = @ReceivablesAccountingCard , [Payables Accounting Card] = @PayablesAccountingCard  where Id = @Id; end

	FETCH NEXT FROM PartnersCursor INTO @Id ,@Code, @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant ,@CountryCode,@PrimaryContactEmail , @ReceivablesAccountingCard,@PayablesAccountingCard
		End
	CLOSE PartnersCursor
	DEALLOCATE PartnersCursor
	
End

	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Card'