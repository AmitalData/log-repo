

--If(OBJECT_ID('tempdb..#DIM_PortsTemp') Is Not Null)
--Begin
--    Drop Table #DIM_PortsTemp
--End



----Create temporal #DIM_PortsTemp

--CREATE TABLE #DIM_PortsTemp (
--	Id_Number int not null identity(1,1) primary key,
--    Id varchar(15) not null,
--    Name varchar(40) not null,
--	Code varchar(3) not null,
--	[Local Name] nvarchar(40),
--	[UN Loc Code] varchar(30),
--	Country varchar(120)  not null,
--	[State Name]  varchar(40),
--   	[Source Tenant]  int,
--    [Parent Tenant]  int,
--);
insert into #DIM_PortsTemp (Id,Name,Code,[Local Name],[UN Loc Code] ,Country,[State Name],  [Source Tenant],[Parent Tenant]) values ('-1' , 'Not Specified' ,'NOS' ,'Not Specified','Not Specified','Not Specified','Not Specified',0,0)

   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @CombinedCode as varchar(30)
   declare @Country varchar(120) 
   declare @State varchar(40) 
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE PortsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Ports.Id, dw_Ports.EnglishName, dw_Ports.Code , dw_Ports.LocalName , dw_Ports.CombinedCode ,dw_Countries.EnglishName, dw_States.EnglishName, dw_Ports.Tenant, dw_DWHSettings.ParentTenant 
	From dw_Ports
	INNER JOIN dw_States ON dw_Ports.StateId = dw_States.Id
	INNER JOIN dw_Countries ON dw_Ports.CountryId = dw_Countries.Id
	INNER JOIN dw_DWHSettings ON dw_Ports.Tenant = dw_DWHSettings.Tenant
	OPEN PortsCursor FETCH NEXT FROM PortsCursor INTO @Id , @Name, @Code , @LocalName  , @CombinedCode, @Country , @State , @SourceTenant , @ParentTenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #DIM_PortsTemp  (Id,Name,Code,[Local Name],[UN Loc Code] ,Country,[State Name],  [Source Tenant],[Parent Tenant])  values(@Id,@Name,@Code,@LocalName ,@CombinedCode, @Country, @State , @SourceTenant , @ParentTenant)

	FETCH NEXT FROM PortsCursor   INTO @Id , @Name, @Code , @LocalName  , @CombinedCode, @Country , @State , @SourceTenant , @ParentTenant 
		End
	CLOSE PortsCursor
	DEALLOCATE PortsCursor
	
IF OBJECT_ID ('NewDIM_Ports', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Ports End
	SELECT * 
INTO NewDIM_Ports
FROM #DIM_PortsTemp

If(OBJECT_ID('tempdb..#DIM_PortsTemp') Is Not Null)
Begin
    Drop Table #DIM_PortsTemp
End

ALTER TABLE NewDIM_Ports ADD CONSTRAINT PK_NewDIM_Ports_Id_Number PRIMARY KEY CLUSTERED (Id_Number);    
CREATE NONCLUSTERED INDEX [IX_DIM_Ports_Id] ON [dbo].[NewDIM_Ports]([Id])


--IF OBJECT_ID ('DIM_Ports', 'U')  IS NOT NULL
--begin
--EXEC sp_rename 'DIM_Ports', 'OldDIM_Ports'

--end

--EXEC sp_rename 'NewDIM_Ports', 'DIM_Ports'


--IF OBJECT_ID ('OldDIM_Ports', 'U')  IS NOT NULL
--begin
-- IF EXISTS (SELECT * 
--  FROM sys.foreign_keys 
--   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Ports_Origin')
--   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
--)
-- begin

--    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Ports_Origin;
--    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Ports_FinalDestination;
--  end


--    drop table OldDIM_Ports
--end



--ALTER TABLE DIM_Ports ADD CONSTRAINT PK_DIM_Ports_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

--  IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL

--begin

-- -- declare @count  as varchar(15)
-- --set @count = (select  count(*)  from Fact_Shipments  where Origin not in (select Id_Number from DIM_Ports))   if(@count >0)  begin update  Fact_Shipments set Origin = 1 where  Origin not in (select Id_Number from DIM_Ports)  end
-- --set @count = (select  count(*)  from Fact_Shipments  where FinalDestination not in (select Id_Number from DIM_Ports))  if(@count >0)  begin update  Fact_Shipments set FinalDestination = 1 where  FinalDestination not in (select Id_Number from DIM_Ports)  end


-- ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Ports_Origin  FOREIGN KEY (Origin) REFERENCES DIM_Ports(Id_Number);
-- ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Ports_FinalDestination  FOREIGN KEY (FinalDestination) REFERENCES DIM_Ports(Id_Number);
 

-- end


--CREATE NONCLUSTERED INDEX [IX_DIM_Ports_Id]
--ON [dbo].[DIM_Ports]([Id])