
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Tenant' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Tenants )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as  int
    declare @Id as int
    declare @Company as varchar(100)
    declare @Country as varchar(100)
	declare @AutomaticLastUpdateDate as datetime

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Tenants.Id, dw_Tenants.Company,dw_Countries.EnglishName, dw_Tenants.AutomaticLastUpdateDate
	From dw_Tenants
	INNER JOIN dw_Addresses ON dw_Tenants.AddressId = dw_Addresses.Id
	INNER JOIN dw_Countries ON dw_Addresses.CountryId = dw_Countries.Id
	where dw_Tenants.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Id , @Company, @Country, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select [Tenant Number] from Dim_Tenants where [Tenant Number] = @Id)
	if(@Key is  null) begin insert into Dim_Tenants ([Tenant Number] , [Tenant Name] , [Country],[Automatic Last Update Date]) values(@Id,@Company,@Country,@AutomaticLastUpdateDate); end
	else begin update   Dim_Tenants set [Tenant Name]  =@Company,  Country =@Country, [Automatic Last Update Date] = @AutomaticLastUpdateDate  Where [Tenant Number] = @Id end


	

	FETCH NEXT FROM TenantsCursor  INTO @Id , @Company, @Country, @AutomaticLastUpdateDate
		End
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
	

	
	  update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Tenant'
End

