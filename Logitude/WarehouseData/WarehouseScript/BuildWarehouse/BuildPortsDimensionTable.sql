
   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @CombinedCode as varchar(30)
   declare @Country varchar(120) 
   declare @State varchar(40) 
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE PortsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Ports.Id, dw_Ports.EnglishName, dw_Ports.Code , dw_Ports.LocalName , dw_Ports.CombinedCode ,dw_Countries.EnglishName, dw_States.EnglishName, dw_Ports.Tenant, dw_DWHSettings.ParentTenant, dw_Ports.AutomaticLastUpdateDate, dw_Ports.InActive
	From dw_Ports
	INNER JOIN dw_States ON dw_Ports.StateId = dw_States.Id
	INNER JOIN dw_Countries ON dw_Ports.CountryId = dw_Countries.Id
	INNER JOIN dw_DWHSettings ON dw_Ports.Tenant = dw_DWHSettings.Tenant
	where dw_Ports.Id !='-1'

	OPEN PortsCursor FETCH NEXT FROM PortsCursor INTO @Id , @Name, @Code , @LocalName  , @CombinedCode, @Country , @State , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #DIM_PortsTemp  (Id,Name,Code,[Local Name],[UN Loc Code] ,Country,[State Name],  [Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive])  values(@Id,@Name,@Code,@LocalName ,@CombinedCode, @Country, @State , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive)

	FETCH NEXT FROM PortsCursor   INTO @Id , @Name, @Code , @LocalName  , @CombinedCode, @Country , @State , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE PortsCursor
	DEALLOCATE PortsCursor
