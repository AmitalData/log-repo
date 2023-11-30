
   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @TEU as float
   declare @ContainerSize as int
   declare @IsContainer as bit
   declare @Volume as float
   declare @PrintAs as varchar(20)
   declare @Code as varchar(4)

	DECLARE PackageTypesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_PackageTypes.Id, dw_PackageTypes.EnglishName, dw_PackageTypes.LocalName,dw_PackageTypes.Tenant, dw_DWHSettings.ParentTenant, dw_PackageTypes.AutomaticLastUpdateDate,
	dw_PackageTypes.TEU, dw_PackageTypes.ContainerSize, dw_PackageTypes.IsContainer, dw_PackageTypes.Volume, dw_PackageTypes.PrintAs, dw_PackageTypes.Code 
	From dw_PackageTypes
	INNER JOIN dw_DWHSettings ON dw_PackageTypes.Tenant = dw_DWHSettings.Tenant
	where dw_PackageTypes.Id !='-1'
	OPEN PackageTypesCursor FETCH NEXT FROM PackageTypesCursor INTO @Id , @Name, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate,
	@TEU, @ContainerSize, @IsContainer, @Volume, @PrintAs, @Code
	WHILE @@FETCH_STATUS = 0
	BEGIN

	insert into #DIM_PackageTypesTemp (Id,Name,[Local Name],[Source Tenant],[Parent Tenant],[Automatic Last Update Date],
	[TEU],[Container Size],[Is Container],[Volume],[Print As],[Code])
	
	values(@Id, @Name, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate,
	@TEU, @ContainerSize, @IsContainer, @Volume, @PrintAs, @Code)

	FETCH NEXT FROM PackageTypesCursor  INTO  @Id , @Name, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate,
	@TEU, @ContainerSize, @IsContainer, @Volume, @PrintAs, @Code

	End
	CLOSE PackageTypesCursor
	DEALLOCATE PackageTypesCursor
