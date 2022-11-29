 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Address' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Addresses )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin


   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit
   declare @Name as varchar(70)
   declare @Address1 as nvarchar(65)
   declare @Address2 as nvarchar(65)
   declare @ZipCode as varchar(15)
   declare @City as nvarchar(25)
   declare @Country as varchar(120)

	DECLARE AddressesCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Addresses.Id, dw_Addresses.Name, dw_Addresses.Address1, dw_Addresses.Address2, dw_Addresses.ZipCode, dw_Addresses.City, dw_Countries.EnglishName, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_Addresses.AutomaticLastUpdateDate, dw_Addresses.InActive
	From dw_Addresses
	INNER JOIN dw_DWHSettings ON dw_Addresses.Tenant = dw_DWHSettings.Tenant
    INNER JOIN dw_Countries ON dw_Addresses.CountryId = dw_Countries.Id
	where dw_Addresses.AutomaticLastUpdateDate > @LastUpdateDate

	OPEN AddressesCursor FETCH NEXT FROM AddressesCursor INTO @Id , @Name, @Address1, @Address2, @ZipCode, @City, @Country, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_Addresses where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_Addresses (Id,Name,[Address 1],[Address 2],[Zip Code],[City],[Country],[Source Tenant],[Parent Tenant], [Automatic Last Update Date],[InActive])
	values(@Id, @Name, @Address1, @Address2, @ZipCode, @City, @Country, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate,@InActive) end
	else begin update   DIM_Addresses set Name =@Name,  [Address 1] =@Address1 ,  [Address 2] = @Address2 ,  [Zip Code] = @ZipCode ,  [City] = @City ,  [Country] = @Country , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate,[InActive] = @InActive where Id = @Id; end

	FETCH NEXT FROM AddressesCursor  INTO @Id , @Name, @Address1, @Address2, @ZipCode,@City, @Country ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate,@InActive
		End
	CLOSE AddressesCursor
	DEALLOCATE AddressesCursor

		update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Address'
	End


