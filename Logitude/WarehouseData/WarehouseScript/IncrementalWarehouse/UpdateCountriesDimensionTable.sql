
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Country' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Countries )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15) 
   declare @Name as nvarchar(120)
   declare @Code as varchar(2)   
   declare @Tenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE CountriesCursor CURSOR READ_ONLY
	FOR
    SELECT Id, dw_Countries.EnglishName, Code, dw_DWHSettings.Tenant, dw_Countries.AutomaticLastUpdateDate
	From dw_Countries
	inner JOIN dw_DWHSettings ON dw_Countries.Tenant = dw_DWHSettings.Tenant
	where dw_Countries.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN CountriesCursor FETCH NEXT FROM CountriesCursor INTO   @Id , @Name, @Code, @Tenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_Countries where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_Countries (Id,[Name], Code, [Tenant],[Automatic Last Update Date])
	values(@Id , @Name, @Code, @Tenant, @AutomaticLastUpdateDate) end
	else begin update  
	DIM_Countries set [Code] =@Code, [Name] =@Name, [Tenant] = @Tenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate 
	Where Id = @Id end


	FETCH NEXT FROM CountriesCursor INTO   @Id ,@Name, @Code,@Tenant, @AutomaticLastUpdateDate
		End
	CLOSE CountriesCursor
	DEALLOCATE CountriesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Country'
End

