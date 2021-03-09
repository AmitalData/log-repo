
   declare @Id as varchar(15) 
   declare @Name as nvarchar(120)
   declare @Code as varchar(2)  
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

   DECLARE CountriesCursor CURSOR READ_ONLY
	FOR
		SELECT Id,  dw_Countries.EnglishName, Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_Countries.AutomaticLastUpdateDate
	From dw_Countries
	inner JOIN dw_DWHSettings ON dw_Countries.Tenant = dw_DWHSettings.Tenant
	where dw_Countries.Id !='-1'

	OPEN CountriesCursor FETCH NEXT FROM CountriesCursor INTO  @Id, @Name, @Code, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_CountriesTemp (Id, Name, Code, [Source Tenant],[Parent Tenant], [Automatic Last Update Date]) values (@Id ,@Name, @Code, @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM CountriesCursor  INTO @Id, @Name, @Code, @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE CountriesCursor
	DEALLOCATE CountriesCursor
