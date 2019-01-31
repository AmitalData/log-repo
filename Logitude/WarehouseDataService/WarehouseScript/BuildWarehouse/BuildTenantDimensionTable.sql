
If(OBJECT_ID('tempdb..#DIM_TenantsTemp') Is Not Null)
Begin
    Drop Table #DIM_TenantsTemp
End



--Create temporal #DIM_PortsTable

 CREATE TABLE #DIM_TenantsTemp (
	[Tenant Number] int not null  primary key,
    [Tenant Name] varchar(100) not null,
	Country varchar(120),
);
insert into #DIM_TenantsTemp values ( -1 ,'Not Specified' ,'Not Specified')




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

	insert into #DIM_TenantsTemp values(@Id,@Company,@Country)

	FETCH NEXT FROM TenantsCursor  INTO @Id , @Company, @Country
		End
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
	


IF OBJECT_ID ('NewDIM_Tenants', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Tenants End
	SELECT * 
INTO NewDIM_Tenants
FROM #DIM_TenantsTemp

If(OBJECT_ID('tempdb..#DIM_TenantsTemp') Is Not Null)
Begin
    Drop Table #DIM_TenantsTemp
End

ALTER TABLE NewDIM_Tenants ADD CONSTRAINT PK_NewDIM_Tenants_TenantNumber PRIMARY KEY CLUSTERED ([Tenant Number]);    
CREATE NONCLUSTERED INDEX [IX_DIM_Tenants_TenantNumber] ON [dbo].[NewDIM_Tenants]([Tenant Number])




--IF OBJECT_ID ('DIM_Tenants', 'U')  IS NOT NULL
--begin
--EXEC sp_rename 'DIM_Tenants', 'OldDIM_Tenants'

--end

--EXEC sp_rename 'NewDIM_Tenants', 'DIM_Tenants'


--IF OBJECT_ID ('OldDIM_Tenants', 'U')  IS NOT NULL
--begin
--IF EXISTS (SELECT * 
--  FROM sys.foreign_keys 
--   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Tenants_SourceTenant')
--   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
--)
-- begin
--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_DIM_Tenants_SourceTenant;
--    ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_DIM_Tenants_ParentTenant;
--  end

--    drop table OldDIM_Tenants
--end







-- ALTER TABLE DIM_Tenants ADD CONSTRAINT PK_DIM_Tenants_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

-- IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
--begin



--  --declare @count  as varchar(15)
--  --set @count = (select count(*) from Fact_Shipments  where SourceTenant not in (select Id_Number from DIM_Tenants))  if(@count >0)  begin update  Fact_Shipments set SourceTenant = 1 where  SourceTenant not in (select Id_Number from DIM_Tenants)  end
--  --set @count = (select  count(*) from Fact_Shipments  where ParentTenant not in (select Id_Number from DIM_Tenants))  if(@count >0)  begin update  Fact_Shipments set ParentTenant = 1 where  ParentTenant not in (select Id_Number from DIM_Tenants)  end


-- ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Tenants_SourceTenant  FOREIGN KEY (SourceTenant) REFERENCES DIM_Tenants(Id_Number);
--  ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Tenants_ParentTenant  FOREIGN KEY (ParentTenant) REFERENCES DIM_Tenants(Id_Number);
-- end

--CREATE NONCLUSTERED INDEX [IX_DIM_Tenants_TenantNumber]
--ON [dbo].[DIM_Tenants]([TenantNumber])