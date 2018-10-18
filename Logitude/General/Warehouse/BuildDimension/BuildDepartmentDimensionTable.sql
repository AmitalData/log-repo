


If(OBJECT_ID('tempdb..#DIM_DepartmentsTemp') Is Not Null)
Begin
    Drop Table #DIM_DepartmentsTemp
End



CREATE TABLE #DIM_DepartmentsTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15) not null,
    Name varchar(40) not null,
	LocalName nvarchar(40),
   	SourceTenant  int,
    ParentTenant  int,
);

insert into #DIM_DepartmentsTemp values ('-1' , 'Not Specified' ,'Not Specified' , 0,0)

   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE DepartmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName , Tenant,Tenant
	From dw_Departments
	OPEN DepartmentsCursor FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_DepartmentsTemp values(@Id,@EnglishName,@LocalName,	@SourceTenant , @ParentTenant)

	FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant
		End
	CLOSE DepartmentsCursor
	DEALLOCATE DepartmentsCursor

	SELECT * 
INTO NewDIM_Departments
FROM #DIM_DepartmentsTemp

If(OBJECT_ID('tempdb..#DIM_DepartmentsTemp') Is Not Null)
Begin
    Drop Table #DIM_DepartmentsTemp
End

IF OBJECT_ID ('DIM_Departments', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Departments', 'OldDIM_Departments'

end

EXEC sp_rename 'NewDIM_Departments', 'DIM_Departments'


IF OBJECT_ID ('OldDIM_Departments', 'U')  IS NOT NULL
begin

 IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Departments_Department')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

    ALTER TABLE Fact_Shipments DROP  CONSTRAINT   FK_Fact_Shipment_DIM_Departments_Department;

  end

    drop table OldDIM_Departments
end



ALTER TABLE DIM_Departments ADD CONSTRAINT PK_DIM_Departments_Id_Number PRIMARY KEY CLUSTERED (Id_Number);



  IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Departments_Department  FOREIGN KEY (Department) REFERENCES DIM_Departments(Id_Number);

 end


 CREATE NONCLUSTERED INDEX [IX_DIM_Departments_Id]
ON [dbo].[DIM_Departments]([Id])