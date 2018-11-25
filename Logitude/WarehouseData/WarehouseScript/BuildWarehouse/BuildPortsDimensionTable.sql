
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
