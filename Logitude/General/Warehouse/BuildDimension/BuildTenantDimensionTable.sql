
If(OBJECT_ID('tempdb..#Dim_TenantsTemp') Is Not Null)
Begin
    Drop Table #Dim_TenantsTemp
End



--Create temporal #Dim_PortsTable

 CREATE TABLE #Dim_TenantsTemp (
	Id_Number int not null identity(1,1) primary key,
    TenantNumber int,
    TenantName varchar(100) not null,
	Country varchar(120),
);
insert into #Dim_TenantsTemp values ( -1 ,'Not Specified' ,'Not Specified')




    declare @Id as int
    declare @Company as varchar(100)
    declare @Country as varchar(100)

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Tenants.Id, dw_Tenants.Company,dw_Countries.EnglishName
	From dw_Tenants
	INNER JOIN dw_Addresses ON dw_Tenants.AddressId = dw_Addresses.Id
	INNER JOIN dw_Countries ON dw_Addresses.CountryId = dw_Countries.Id

	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Id , @Company, @Country
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #Dim_TenantsTemp values(@Id,@Company,@Country)

	FETCH NEXT FROM TenantsCursor  INTO @Id , @Company, @Country
		End
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
	



	SELECT * 
INTO NewDim_Tenants
FROM #Dim_TenantsTemp

If(OBJECT_ID('tempdb..#Dim_TenantsTemp') Is Not Null)
Begin
    Drop Table #Dim_TenantsTemp
End



IF OBJECT_ID ('DIM_Tenants', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Tenants', 'OldDIM_Tenants'

end

EXEC sp_rename 'NewDIM_Tenants', 'DIM_Tenants'


IF OBJECT_ID ('OldDIM_Tenants', 'U')  IS NOT NULL
begin
IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_Dim_Tenants_SourceTenant')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin
  ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_Dim_Tenants_SourceTenant;
    ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_Dim_Tenants_ParentTenant;
  end

    drop table OldDIM_Tenants
end







 ALTER TABLE Dim_Tenants ADD CONSTRAINT PK_Dim_Tenants_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_Dim_Tenants_SourceTenant  FOREIGN KEY (SourceTenant) REFERENCES Dim_Tenants(Id_Number);
  ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_Dim_Tenants_ParentTenant  FOREIGN KEY (ParentTenant) REFERENCES Dim_Tenants(Id_Number);
 end

CREATE NONCLUSTERED INDEX [IX_Dim_Tenants_TenantNumber]
ON [dbo].[Dim_Tenants]([TenantNumber])