
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'PackageType' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_PackageTypes )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(15)
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
	where dw_PackageTypes.Id !='-1' and dw_PackageTypes.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN PackageTypesCursor FETCH NEXT FROM PackageTypesCursor INTO @Id , @Name, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate,
	@TEU, @ContainerSize, @IsContainer, @Volume, @PrintAs, @Code
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from Dim_PackageTypes where Id = @Id)
	if(@Key is  null) begin
	insert into Dim_PackageTypes (Id,Name,[Local Name],[Source Tenant],[Parent Tenant],[Automatic Last Update Date],
	[TEU],[Container Size],[Is Container],[Volume],[Print As],[Code])
	
	values(@Id, @Name, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate,
	@TEU, @ContainerSize, @IsContainer, @Volume, @PrintAs, @Code); end
	
	else begin update Dim_PackageTypes set Name =@Name,  [Local Name] =@LocalName,  [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate ,
	[TEU] = @TEU, [Container Size] = @ContainerSize, [Is Container] = @IsContainer, [Volume] = @Volume, [Print As] = @PrintAs, [Code] = @Code Where Id = @Id; end



	FETCH NEXT FROM PackageTypesCursor  INTO @Id , @Name, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate,
	@TEU, @ContainerSize, @IsContainer, @Volume, @PrintAs, @Code
		End
	CLOSE PackageTypesCursor
	DEALLOCATE PackageTypesCursor
	
	update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'PackageType'
End