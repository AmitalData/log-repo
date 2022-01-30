
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
   declare @CustomerTeam as varchar(40)
   declare @Salesman as varchar(60)
   declare @Rank as varchar(40)
   declare @PartnerType as varchar(20)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @ReceivablesAccountingCard as varchar(25)
   declare @CountryCode as varchar(2)
   declare @PrimaryContactEmail as varchar(70)
   declare @address1 as varchar(65)
   declare @address2 as varchar(65)
   declare @Phone as varchar(40)

   declare @Region as varchar(100)
	declare @CustomerSize as nvarchar(60)
    declare @Industry as nvarchar(60)

	declare @VatNumber as varchar(20)
	declare @CreditLimitAmount as float
    declare @CreditLimitOpenBalance as float 

	declare @LeadSource as varchar(60)
	declare @AutomaticLastUpdateDate as datetime

	declare @InActive as bit
	
    declare @FirstShipmentDate as datetime
    declare @LastShipmentDate as datetime


	   declare @BillTo as varchar(70)


	DECLARE PartnersCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Partners.Id,dw_Partners.Code ,dw_Partners.EnglishName,dw_Partners.LocalName , dw_Partners.CityName, dw_Partners.CountryName , dw_States.EnglishName , dw_Partners.ZipCode, dw_Contacts.EnglishName ,accountManagerUser.EnglishName, customerTeamUser.Name, salesmanUser.EnglishName ,dw_Ranks.Name,dw_PartnerTypes.Name, dw_Partners.Tenant, dw_DWHSettings.ParentTenant ,dw_Countries.Code,dw_Contacts.Email ,  dw_Partners.ReceivablesAccountingCard, dw_Partners.address1,dw_Partners.address2, dw_Partners.Phone
	,dw_Regions.Name , dw_CustomerSizes.Name ,dw_Industries.Name , dw_Partners.VatNumber , dw_Customers.CreditLimitAmount , dw_Customers.CreditLimitOpenBalance,dw_LeadSources.Name, dw_Partners.AutomaticLastUpdateDate, dw_Partners.InActive, dw_Customers.FirstShipmentDate, dw_Customers.LastShipmentDate ,null
	From dw_Partners
	--inner JOIN dw_Partners billTo ON dw_Partners.BillToId = billTo.Id
	left JOIN dw_Customers ON dw_Partners.Id = dw_Customers.Id

	inner JOIN dw_Contacts ON dw_Partners.PrimaryContactId = dw_Contacts.Id
	left join dw_Contacts accountManagerUser on dw_Customers.AccountManagerUserId=accountManagerUser.Id
	left join dw_CustomerTeams customerTeamUser on dw_Customers.TeamId=customerTeamUser.Id
	inner join dw_Contacts salesmanUser on dw_Partners.SalesmanUserId=salesmanUser.Id
	left join dw_Ranks  on dw_Customers.RankId=dw_Ranks.Id
    left join dw_LeadSources  on dw_Customers.LeadSourceId =dw_LeadSources.Id
	left join dw_Regions  on dw_Customers.RegionId=dw_Regions.Id
	left join dw_CustomerSizes  on dw_Customers.CustomerSizeId=dw_CustomerSizes.Id
	left join dw_Industries  on dw_Customers.IndustryId=dw_Industries.Id

	left join dw_Addresses  on dw_Partners.Id = dw_Addresses.CardId and dw_Addresses.AddressTypeId = 'M'
    left join dw_States  on dw_Addresses.StateId = dw_States.Id
	inner join dw_PartnerTypes  on dw_Partners.PartnerTypeId=dw_PartnerTypes.Id
	inner JOIN dw_DWHSettings ON dw_Partners.Tenant = dw_DWHSettings.Tenant
	inner JOIN dw_Countries ON dw_Partners.CountryId = dw_Countries.Id
	--where dw_Partners.id !='-1'
	OPEN PartnersCursor FETCH NEXT FROM PartnersCursor INTO @Id ,@Code, @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager, @CustomerTeam , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant,  @CountryCode,@PrimaryContactEmail,@ReceivablesAccountingCard,@address1, @address2, @Phone , @Region,@CustomerSize,@Industry ,@VatNumber , @CreditLimitAmount , @CreditLimitOpenBalance,@LeadSource, @AutomaticLastUpdateDate, @InActive, @FirstShipmentDate, @LastShipmentDate ,@BillTo
	WHILE @@FETCH_STATUS = 0																																																																								
	BEGIN																																																																													
	
	insert into #DIM_PartnersTemp (Id,Code,Name,[Local Name],City,Country,[State Name],[Zip Code],[Primary Contact],[Account Manager],[Customer Team],Salesman,[Customer Rank],[Partner Type],[Source Tenant],[Parent Tenant] ,[Country Code],[Primary Contact Email] , [Receivables Accounting Card]  ,[Address1],[Address2],[Phone] , [Region],[Customer Size],[Industry],[Vat Number] , [Credit Limit Amount (Local)] , [Open Balance (Local)] , [Lead Source], [Automatic Last Update Date], [InActive], [Customer First Shipment Date], [Customer Last Shipment Date] , [Bill To]) values(@Id,@Code, @Name,@LocalName ,@City,@Country, @State, @ZipCode , @PrimaryContact , @AccountManager, @CustomerTeam , @Salesman ,@Rank , @PartnerType,  @SourceTenant , @ParentTenant,@CountryCode,@PrimaryContactEmail, @ReceivablesAccountingCard,@address1,  @address2,  @Phone,@Region,@CustomerSize,@Industry ,@VatNumber , @CreditLimitAmount , @CreditLimitOpenBalance , @LeadSource, @AutomaticLastUpdateDate, @InActive, @FirstShipmentDate, @LastShipmentDate , @BillTo)

	FETCH NEXT FROM PartnersCursor INTO @Id ,@Code , @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager, @CustomerTeam , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant,  @CountryCode,@PrimaryContactEmail, @ReceivablesAccountingCard,@address1,  @address2,  @Phone ,@Region,@CustomerSize,@Industry ,@VatNumber , @CreditLimitAmount , @CreditLimitOpenBalance  , @LeadSource, @AutomaticLastUpdateDate, @InActive, @FirstShipmentDate, @LastShipmentDate , @BillTo
		End
	CLOSE PartnersCursor
	DEALLOCATE PartnersCursor
	
