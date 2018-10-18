


If(OBJECT_ID('tempdb..#Dim_UsersTemp') Is Not Null)
Begin
    Drop Table #Dim_UsersTemp
End




CREATE TABLE #Dim_UsersTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15),
    Name varchar(60) not null,
	LocalName nvarchar(100),
    Email varchar(70),
	Department varchar(40) not null,
	Branch varchar(40) not null,
	SourceTenant  int,
    ParentTenant  int,
);
insert into #Dim_UsersTemp values ('-1' , 'Not Specified' ,'Not Specified' ,'Not Specified','Not Specified','Not Specified', 0,0)


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
	SELECT dw_Users.Id, dw_Contacts.EnglishName,dw_Contacts.LocalName , dw_Contacts.Email, dw_Departments.EnglishName , dw_Branches.EnglishName ,  dw_Users.Tenant, dw_Users.Tenant
	From dw_Users
	INNER JOIN dw_Branches ON dw_Users.BranchId = dw_Branches.Id
	INNER JOIN dw_Departments ON dw_Users.DepartmentId = dw_Departments.Id
	INNER JOIN dw_Contacts ON dw_Users.Id = dw_Contacts.Id
	OPEN UsersCursor FETCH NEXT FROM UsersCursor INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #Dim_UsersTemp values(@Id,@Name,@LocalName ,@Email,@Department, @Branch, @SourceTenant , @ParentTenant )

	FETCH NEXT FROM UsersCursor  INTO @Id , @Name, @LocalName , @Email , @Department, @Branch , @SourceTenant , @ParentTenant  
		End
	CLOSE UsersCursor
	DEALLOCATE UsersCursor
	


 SELECT * 
INTO NewDim_Users
FROM #Dim_UsersTemp


IF OBJECT_ID ('DIM_Users', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Users', 'OldDIM_Users'

end

EXEC sp_rename 'NewDIM_Users', 'DIM_Users'


IF OBJECT_ID ('OldDIM_Users', 'U')  IS NOT NULL
begin
IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_Dim_Users_Salesman')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin
  ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_Dim_Users_Salesman;
    ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_Dim_Users_AccountManager;
  end

    drop table OldDIM_Users
end




 ALTER TABLE Dim_Users ADD CONSTRAINT PK_Dim_Users_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_Dim_Users_Salesman  FOREIGN KEY (Salesman) REFERENCES Dim_Users(Id_Number);
  ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_Dim_Users_AccountManager  FOREIGN KEY (AccountManager) REFERENCES Dim_Users(Id_Number);
 end

If(OBJECT_ID('tempdb..#Dim_UsersTemp') Is Not Null)
Begin
    Drop Table #Dim_UsersTemp
End

CREATE NONCLUSTERED INDEX [IX_Dim_Users_Id]
ON [dbo].[Dim_Users]([Id])