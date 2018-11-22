

--If(OBJECT_ID('tempdb..#DIM_IncotermsTemp') Is Not Null)
--Begin
--    Drop Table #DIM_IncotermsTemp
--End

--CREATE TABLE #DIM_IncotermsTemp (
--	Id_Number int not null identity(1,1) primary key,
--    Id varchar(15) not null,
--    Name varchar(40) not null,
--	[Local Name] nvarchar(40),
--    Code varchar(3) not null,
--   	[Source Tenant]  int,
--    [Parent Tenant]  int,
--);
insert into #DIM_IncotermsTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant]) values ('-1' , 'Not Specified' ,'Not Specified' ,'NOS', 0,0)


   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @SourceTenant int
   declare @ParentTenant int
   

	DECLARE IncotermsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Incoterms.Id, dw_Incoterms.Name , dw_Incoterms.LocalName ,dw_Incoterms.Code, dw_Incoterms.Tenant,dw_DWHSettings.ParentTenant
	From dw_Incoterms
	inner JOIN dw_DWHSettings ON dw_Incoterms.Tenant = dw_DWHSettings.Tenant
	OPEN IncotermsCursor FETCH NEXT FROM IncotermsCursor INTO @Id , @Name, @LocalName, @Code, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_IncotermsTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant]) values(@Id,@Name,@LocalName,@Code ,@SourceTenant , @ParentTenant)

	FETCH NEXT FROM IncotermsCursor  INTO @Id , @Name, @LocalName, @Code,@SourceTenant , @ParentTenant
		End
	CLOSE IncotermsCursor
	DEALLOCATE IncotermsCursor
	

IF OBJECT_ID ('NewDIM_Incoterms', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Incoterms End
	SELECT * 
INTO NewDIM_Incoterms
FROM #DIM_IncotermsTemp

If(OBJECT_ID('tempdb..#DIM_IncotermsTemp') Is Not Null)
Begin
    Drop Table #DIM_IncotermsTemp
End

ALTER TABLE NewDIM_Incoterms ADD CONSTRAINT PK_NewDIM_Incoterms_Id_Number PRIMARY KEY CLUSTERED (Id_Number);    
CREATE NONCLUSTERED INDEX [IX_DIM_Incoterms_Id] ON [dbo].[NewDIM_Incoterms]([Id])

--IF OBJECT_ID ('DIM_Incoterms', 'U')  IS NOT NULL
--begin
--EXEC sp_rename 'DIM_Incoterms', 'OldDIM_Incoterms'

--end

--EXEC sp_rename 'NewDIM_Incoterms', 'DIM_Incoterms'


--IF OBJECT_ID ('OldDIM_Incoterms', 'U')  IS NOT NULL
--begin
-- IF EXISTS (SELECT * 
--  FROM sys.foreign_keys 
--   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Incoterms_Incoterm')
--   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
--)
-- begin

--    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Incoterms_Incoterm;

--  end

--    drop table OldDIM_Incoterms
--end







--ALTER TABLE DIM_Incoterms ADD CONSTRAINT PK_DIM_Incoterms_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

--  IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
--begin



-- -- declare @count  as int

-- --set @count = (select  count(*)   from Fact_Shipments  where Incoterm not in (select Id_Number from DIM_Incoterms))  if(@count >0)  begin update  Fact_Shipments set Incoterm = 1 where  Incoterm not in (select Id_Number from DIM_Incoterms)  end


-- ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Incoterms_Incoterm  FOREIGN KEY (Incoterm) REFERENCES DIM_Incoterms(Id_Number);

-- end




--CREATE NONCLUSTERED INDEX [IX_DIM_Incoterms_Id]
--ON [dbo].[DIM_Incoterms]([Id])