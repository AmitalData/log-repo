
   declare @Id as varchar(15) 
   declare @Name as nvarchar(120)
   declare @Code as varchar(2) 
   declare @Tenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE CountriesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,  dw_Countries.EnglishName, Code, dw_DWHSettings.Tenant, dw_Countries.AutomaticLastUpdateDate
	From dw_Countries
	inner JOIN dw_DWHSettings ON dw_Countries.Tenant = dw_DWHSettings.Tenant
	OPEN CountriesCursor FETCH NEXT FROM CountriesCursor INTO  @Id, @Name, @Code, @Tenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_CountriesTemp (Id, Name, Code,[Tenant], [Automatic Last Update Date]) values (@Id ,@Name, @Code, @Tenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM CountriesCursor  INTO @Id, @Name, @Code, @Tenant, @AutomaticLastUpdateDate
		End
	CLOSE CountriesCursor
	DEALLOCATE CountriesCursor
 