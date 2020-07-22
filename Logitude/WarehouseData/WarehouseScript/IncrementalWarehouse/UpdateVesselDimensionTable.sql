
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Vessel' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Vessels )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Code as varchar(5)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Notes as varchar(250)
   declare @IMOCode as varchar(10)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE VesselsCursor CURSOR READ_ONLY
	FOR
    SELECT Id,Code, EnglishName , LocalName , Notes, IMOCode, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_Vessels.AutomaticLastUpdateDate
	From dw_Vessels
	inner JOIN dw_DWHSettings ON dw_Vessels.Tenant = dw_DWHSettings.Tenant
	where dw_Vessels.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN VesselsCursor FETCH NEXT FROM VesselsCursor INTO  @Id ,@Code, @EnglishName, @LocalName, @Notes, @IMOCode , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_Vessels where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_Vessels (Id, Code ,[English Name] ,[Local Name],[Notes],[IMO Code], [Source Tenant],[Parent Tenant],[Automatic Last Update Date]) values(@Id ,@Code, @EnglishName, @LocalName, @Notes, @IMOCode , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate) end
	else begin update   DIM_Vessels set [Code] =@Code, [English Name] =@EnglishName, [Local Name] =@LocalName ,[Notes] = @Notes ,[IMO Code] = @IMOCode , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Id = @Id end


	FETCH NEXT FROM VesselsCursor INTO  @Id ,@Code, @EnglishName, @LocalName, @Notes, @IMOCode , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE VesselsCursor
	DEALLOCATE VesselsCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Vessel'
End

