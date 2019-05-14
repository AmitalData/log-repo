
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Tenant' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Tenants )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as  int
    declare @Id as int
    declare @Company as varchar(100)
    declare @Country as varchar(100)

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Tenants.Id, dw_Tenants.Company,dw_Countries.EnglishName
	From dw_Tenants
	INNER JOIN dw_Addresses ON dw_Tenants.AddressId = dw_Addresses.Id
	INNER JOIN dw_Countries ON dw_Addresses.CountryId = dw_Countries.Id
	where dw_Tenants.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Id , @Company, @Country
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select [Tenant Number] from Dim_Tenants where [Tenant Number] = @Id)
	if(@Key is  null) begin insert into Dim_Tenants ([Tenant Number] , [Tenant Name] , [Country]) values(@Id,@Company,@Country); end
	else begin update   Dim_Tenants set [Tenant Name]  =@Company,  Country =@Country  Where [Tenant Number] = @Id end


	

	FETCH NEXT FROM TenantsCursor  INTO @Id , @Company, @Country
		End
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
	

	
	  update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Tenant'
End

