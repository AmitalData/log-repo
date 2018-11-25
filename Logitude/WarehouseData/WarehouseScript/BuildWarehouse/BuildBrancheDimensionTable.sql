 

--If(OBJECT_ID('tempdb..#DIM_BranchesTemp') Is Not Null)
--Begin
--    Drop Table #DIM_BranchesTemp
--End

--CREATE TABLE #DIM_BranchesTemp (
--	Id_Number int not null identity(1,1) primary key,
--    Id varchar(15) not null , 
--	Name varchar(40)  not null,
--	[Local Name]  nvarchar(40),
--    Code varchar(13),
--	[Source Tenant] int,
--	[Parent Tenant] int,
--);
 
 insert into #DIM_BranchesTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant]) values ('-1' , 'Not Specified' ,'Not Specified' ,'Not Specified', 0,0)

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
	
    insert into #DIM_BranchesTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant]) values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant)

	FETCH NEXT FROM BranchesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant
		End
	CLOSE BranchesCursor
	DEALLOCATE BranchesCursor


IF OBJECT_ID ('NewDIM_Branches', 'U')  IS NOT NULL Begin  Drop Table NewDIM_Branches End
SELECT *  INTO NewDIM_Branches FROM #DIM_BranchesTemp
If(OBJECT_ID('tempdb..#DIM_BranchesTemp') Is Not Null) Begin     Drop Table #DIM_BranchesTemp End

 ALTER TABLE NewDIM_Branches ADD CONSTRAINT PK_NewDIM_Branches_Id_Number PRIMARY KEY CLUSTERED (Id_Number);
 CREATE NONCLUSTERED INDEX [IX_DIM_Branches_Id] ON [dbo].[NewDIM_Branches]([Id])

