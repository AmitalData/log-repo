

If(OBJECT_ID('tempdb..#DIM_PartnersTemp') Is Not Null)
Begin
    Drop Table #DIM_PartnersTemp
End



--Create temporal DIM_PartnersTemp

CREATE TABLE #DIM_PartnersTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15) not null,
    Name varchar(70) not null,
	LocalName nvarchar(100),
	City nvarchar(25),
	Country varchar(120),
	StateName  varchar(40),
    ZipCode varchar(15),
	PrimaryContact varchar(60),
	AccountManager  varchar(60),
	Salesman  varchar(60),
	CustomerRank   varchar(40),
	PartnerType  varchar(20) not null,
	SourceTenant  int,
    ParentTenant  int,

);
insert into #DIM_PartnersTemp values ('-1' , 'Not Specified' ,'Not Specified' ,'Not Specified','Not Specified','Not Specified','Not Specified','Not Specified','Not Specified','Not Specified','Not Specified' ,'Not Specified', 0,0)

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


	OPEN PartnersCursor FETCH NEXT FROM PartnersCursor INTO @Id , @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	insert into #DIM_PartnersTemp values(@Id,@Name,@LocalName ,@City,@Country, @State, @ZipCode , @PrimaryContact , @AccountManager , @Salesman ,@Rank , @PartnerType,  @SourceTenant , @ParentTenant)

	FETCH NEXT FROM PartnersCursor INTO @Id , @Name, @LocalName ,@City , @Country, @State , @ZipCode , @PrimaryContact , @AccountManager , @Salesman , @Rank , @PartnerType  , @SourceTenant, @ParentTenant
		End
	CLOSE PartnersCursor
	DEALLOCATE PartnersCursor
	

--IF OBJECT_ID ('DIM_Partners', 'U')  IS NOT NULL 
--begin
--IF EXISTS (SELECT * 
--  FROM sys.foreign_keys 
--   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Partners_Shipper')
--   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
--)
-- begin

--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Shipper;
--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Consignee;
--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Agent;
--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Customer;
--  end

--   drop table DIM_Partners

--end

SELECT * 
INTO  NewDIM_Partners
FROM #DIM_PartnersTemp

If(OBJECT_ID('tempdb..#DIM_PartnersTemp') Is Not Null)
Begin
    Drop Table #DIM_PartnersTemp


End


IF OBJECT_ID ('DIM_Partners', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Partners', 'OldDIM_Partners'

end

EXEC sp_rename 'NewDIM_Partners', 'DIM_Partners'

IF OBJECT_ID ('OldDIM_Partners', 'U')  IS NOT NULL
begin

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Partners_Shipper')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Shipper;
  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Consignee;
  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Agent;
  ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Partners_Customer;
  end

   drop table OldDIM_Partners

end


 ALTER TABLE DIM_Partners ADD CONSTRAINT PK_DIM_Partners_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

  IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Shipper  FOREIGN KEY (Shipper) REFERENCES DIM_Partners(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Consignee  FOREIGN KEY (Consignee) REFERENCES DIM_Partners(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Agent  FOREIGN KEY (Agent) REFERENCES DIM_Partners(Id_Number);
  ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Customer  FOREIGN KEY (Customer) REFERENCES DIM_Partners(Id_Number);

 end



CREATE NONCLUSTERED INDEX [IX_DIM_Partners_Id]
ON [dbo].[DIM_Partners]([Id])