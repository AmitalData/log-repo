


--If(OBJECT_ID('tempdb..#DIM_UsersTemp') Is Not Null)
--Begin
--    Drop Table #DIM_UsersTemp
--End




--CREATE TABLE #DIM_UsersTemp (
--	Id_Number int not null identity(1,1) primary key,
--    Id varchar(15),
--    Name varchar(60) not null,
--	[Local Name] nvarchar(100),
--    Email varchar(70),
--	Department varchar(40) not null,
--	Branch varchar(40) not null,
--	[Source Tenant]  int,
--    [Parent Tenant]  int,
--);
insert into #DIM_UsersTemp (Id,Name,[Local Name],Email, Department ,Branch,  [Source Tenant],[Parent Tenant]) values ('-1' , 'Not Specified' ,'Not Specified' ,'Not Specified','Not Specified','Not Specified', 0,0)


   declare @Id as varchar(15)
   declare @Name as varchar(60)
   declare @LocalName as nvarchar(100)
   declare @Email as varchar(70)
   declare @Department varchar(40) 
   declare @Branch varchar(40) 
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE UsersCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Users.Id, dw_Contacts.EnglishName,dw_Contacts.LocalName , dw_Contacts.Email, dw_Departments.EnglishName , dw_Branches.EnglishName ,  dw_Users.Tenant, dw_DWHSettings.ParentTenant
	From dw_Users
	INNER JOIN dw_Branches ON dw_Users.BranchId = dw_Branches.Id
	INNER JOIN dw_Departments ON dw_Users.DepartmentId = dw_Departments.Id
	INNER JOIN dw_Contacts ON dw_Users.Id = dw_Contacts.Id
	INNER JOIN dw_DWHSettings ON dw_Users.Tenant = dw_DWHSettings.Tenant
	OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #DIM_UsersTemp (Id,Name,[Local Name],Email, Department ,Branch,  [Source Tenant],[Parent Tenant]) values(@Id,@Name,@LocalName ,@Email,@Department, @Branch, @SourceTenant , @ParentTenant )

	FETCH NEXT FROM UsersCursor  INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant  
		End
	CLOSE UsersCursor
	DEALLOCATE UsersCursor
	

IF OBJECT_ID ('NewDIM_Users', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Users End
 SELECT * 
INTO NewDIM_Users
FROM #DIM_UsersTemp

If(OBJECT_ID('tempdb..#DIM_UsersTemp') Is Not Null) Begin Drop Table #DIM_UsersTemp End
ALTER TABLE NewDIM_Users ADD CONSTRAINT PK_NewDIM_Users_Id_Number PRIMARY KEY CLUSTERED (Id_Number);    
CREATE NONCLUSTERED INDEX [IX_DIM_Users_Id] ON [dbo].[NewDIM_Users]([Id])

  

--IF OBJECT_ID ('DIM_Users', 'U')  IS NOT NULL
--begin
--EXEC sp_rename 'DIM_Users', 'OldDIM_Users'

--end

--EXEC sp_rename 'NewDIM_Users', 'DIM_Users'


--IF OBJECT_ID ('OldDIM_Users', 'U')  IS NOT NULL
--begin
--IF EXISTS (SELECT * 
--  FROM sys.foreign_keys 
--   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Users_Salesman')
--   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
--)
-- begin
--  ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_DIM_Users_Salesman;
--    ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_DIM_Users_AccountManager;
--  end

--    drop table OldDIM_Users
--end




-- ALTER TABLE DIM_Users ADD CONSTRAINT PK_DIM_Users_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

-- IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
--begin



-- --declare @count  as varchar(15)
-- --set @count = (select count(*) from Fact_Shipments  where Salesman not in (select Id_Number from DIM_Users))  if(@count >0)  begin update  Fact_Shipments set Salesman = 1 where  Salesman not in (select Id_Number from DIM_Users)  end
-- --set @count= (select count(*) from Fact_Shipments  where AccountManager not in (select Id_Number from DIM_Users))  if(@count >0)  begin update  Fact_Shipments set AccountManager = 1 where  AccountManager not in (select Id_Number from DIM_Users)  end

-- ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Users_Salesman  FOREIGN KEY (Salesman) REFERENCES DIM_Users(Id_Number);
--  ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Users_AccountManager  FOREIGN KEY (AccountManager) REFERENCES DIM_Users(Id_Number);
-- end

--If(OBJECT_ID('tempdb..#DIM_UsersTemp') Is Not Null)
--Begin
--    Drop Table #DIM_UsersTemp
--End

--CREATE NONCLUSTERED INDEX [IX_DIM_Users_Id]
--ON [dbo].[DIM_Users]([Id])