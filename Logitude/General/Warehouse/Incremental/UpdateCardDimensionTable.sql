 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Card' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Partners )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin


   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(70)
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


	DECLARE PartnersCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Partners.Id, dw_Partners.EnglishName,dw_Partners.LocalName , dw_Partners.CityName, dw_Partners.CountryName , dw_States.EnglishName , dw_Partners.ZipCode, dw_Contacts.EnglishName ,accountManagerUser.EnglishName, salesmanUser.EnglishName ,dw_Ranks.Name,dw_PartnerTypes.Name, dw_Partners.Tenant, dw_Partners.Tenant
	From dw_Partners

	left JOIN dw_Customers ON dw_Partners.Id = dw_Customers.Id
	inner JOIN dw_Contacts ON dw_Partners.PrimaryContactId = dw_Contacts.Id
	left join dw_Contacts accountManagerUser on dw_Customers.AccountManagerUserId=accountManagerUser.Id
	inner join dw_Contacts salesmanUser on dw_Partners.SalesmanUserId=salesmanUser.Id
	left join dw_Ranks  on dw_Customers.RankId=dw_Ranks.Id
	left join dw_Addresses  on dw_Partners.Id = dw_Addresses.Id
    left join dw_States  on dw_Addresses.StateId = dw_States.Id
	inner join dw_PartnerTypes  on dw_Partners.PartnerTypeId=dw_PartnerTypes.Id
	where dw_Partners.AutomaticLastUpdateDate > @LastUpdateDate	

	OPEN PartnersCursor FETCH NEXT FROM PartnersCursor INTO @Id , @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from DIM_Partners where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_Partners values(@Id,@Name,@LocalName ,@City,@Country, @State, @ZipCode , @PrimaryContact , @AccountManager , @Salesman ,@Rank , @PartnerType,  @SourceTenant , @ParentTenant) end
	else begin update   DIM_Partners set Name =@Name,  LocalName =@LocalName ,  City = @City , Country = @Country,  StateName = @State, ZipCode = @ZipCode ,  PrimaryContact = @PrimaryContact,AccountManager = @AccountManager,Salesman = @Salesman,CustomerRank = @Rank,PartnerType = @PartnerType , SourceTenant = @SourceTenant , ParentTenant = @ParentTenant where Id = @Id; end

	

	FETCH NEXT FROM PartnersCursor INTO @Id , @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant
		End
	CLOSE PartnersCursor
	DEALLOCATE PartnersCursor
	
End

	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Card'