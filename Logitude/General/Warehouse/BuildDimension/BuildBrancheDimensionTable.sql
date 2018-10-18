 

If(OBJECT_ID('tempdb..#DIM_BranchesTemp') Is Not Null)
Begin
    Drop Table #DIM_BranchesTemp
End

CREATE TABLE #DIM_BranchesTemp (
	Id_Number int not null identity(1,1) primary key,
    Id varchar(15) not null , 
	Name varchar(40)  not null,
	LocalName nvarchar(40),
    Code varchar(10),
	SourceTenant  int,
    ParentTenant  int,
);
 
 insert into #DIM_BranchesTemp values ('-1' , 'Not Specified' ,'Not Specified' ,'NotSpecifi', 0,0)

   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(10)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE BranchesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant
	From dw_Branches
	inner JOIN dw_DWHSettings ON dw_Branches.Tenant = dw_DWHSettings.Tenant
	OPEN BranchesCursor FETCH NEXT FROM BranchesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_BranchesTemp values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant)

	FETCH NEXT FROM BranchesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant
		End
	CLOSE BranchesCursor
	DEALLOCATE BranchesCursor


	
SELECT *  INTO NewDIM_Branches FROM #DIM_BranchesTemp
If(OBJECT_ID('tempdb..#DIM_BranchesTemp') Is Not Null)
Begin
    Drop Table #DIM_BranchesTemp

End

IF OBJECT_ID ('DIM_Branches', 'U')  IS NOT NULL
begin
EXEC sp_rename 'DIM_Branches', 'OldDIM_Branches'

end

EXEC sp_rename 'NewDIM_Branches', 'DIM_Branches'


IF OBJECT_ID ('OldDIM_Branches', 'U')  IS NOT NULL
begin
IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_Fact_Shipment_DIM_Branches_Branch')
   AND parent_object_id = OBJECT_ID(N'dbo.Fact_Shipments')
)
 begin

  ALTER TABLE Fact_Shipments DROP  CONSTRAINT FK_Fact_Shipment_DIM_Branches_Branch;
  end

    drop table OldDIM_Branches
end

 ALTER TABLE DIM_Branches ADD CONSTRAINT PK_DIM_Branches_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL
begin
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Branches_Branch  FOREIGN KEY (Branch) REFERENCES DIM_Branches(Id_Number);
 end
 CREATE NONCLUSTERED INDEX [IX_DIM_Branches_Id]
ON [dbo].[DIM_Branches]([Id])

