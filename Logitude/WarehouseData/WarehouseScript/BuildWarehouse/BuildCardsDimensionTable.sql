

If(OBJECT_ID('tempdb..#DIM_PartnersTemp') Is Not Null)
Begin
    Drop Table #DIM_PartnersTemp
End



--Create temporal DIM_PartnersTemp

CREATE TABLE #DIM_PartnersTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15) not null,
    Name varchar(70) not null,
	[Local Name]  nvarchar(100),
	City nvarchar(25),
	Country varchar(120),
	[State Name]  varchar(40),
	[Zip Code]  varchar(15),
	[Primary Contact] varchar(60),
	[Account Manager]  varchar(60),
	Salesman  varchar(60),
	[Customer Rank]   varchar(40),
	[Partner Type]  varchar(20) not null,
	[Source Tenant]  int,
    [Parent Tenant]  int,
	[Country Code] varchar(2),
    [Primary Contact Email] varchar(70),

);
insert into #DIM_PartnersTemp values ('-1' , 'Not Specified' ,'Not Specified' ,'Not Specified','Not Specified','Not Specified','Not Specified','Not Specified','Not Specified','Not Specified','Not Specified' ,'Not Specified', 0,0,null,'Not Specified')

--Fill temporal DIM_PartnersTemp
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

   declare @CountryCode as varchar(2)
   declare @PrimaryContactEmail as varchar(70)
 
	DECLARE PartnersCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Partners.Id, dw_Partners.EnglishName,dw_Partners.LocalName , dw_Partners.CityName, dw_Partners.CountryName , dw_States.EnglishName , dw_Partners.ZipCode, dw_Contacts.EnglishName ,accountManagerUser.EnglishName, salesmanUser.EnglishName ,dw_Ranks.Name,dw_PartnerTypes.Name, dw_Partners.Tenant, dw_DWHSettings.ParentTenant ,dw_Countries.Code,dw_Contacts.Email
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

	OPEN PartnersCursor FETCH NEXT FROM PartnersCursor INTO @Id , @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant,  @CountryCode,@PrimaryContactEmail
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	insert into #DIM_PartnersTemp values(@Id,@Name,@LocalName ,@City,@Country, @State, @ZipCode , @PrimaryContact , @AccountManager , @Salesman ,@Rank , @PartnerType,  @SourceTenant , @ParentTenant,@CountryCode,@PrimaryContactEmail)

	FETCH NEXT FROM PartnersCursor INTO @Id , @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant,  @CountryCode,@PrimaryContactEmail
		End
	CLOSE PartnersCursor
	DEALLOCATE PartnersCursor
	

IF OBJECT_ID ('NewDIM_Partners', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Partners End
SELECT *  INTO  NewDIM_Partners FROM #DIM_PartnersTemp
If(OBJECT_ID('tempdb..#DIM_PartnersTemp') Is Not Null) Begin Drop Table #DIM_PartnersTemp End

ALTER TABLE NewDIM_Partners ADD CONSTRAINT PK_NewDIM_Partners_Id_Number PRIMARY KEY CLUSTERED (Id_Number);
CREATE NONCLUSTERED INDEX [IX_DIM_Partners_Id] ON [dbo].[NewDIM_Partners]([Id])
