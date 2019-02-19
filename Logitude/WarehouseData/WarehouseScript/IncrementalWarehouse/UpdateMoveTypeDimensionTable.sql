
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'MoveType' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_MoveTypes )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Code as varchar(3)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @TransportModeId as varchar(1)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE MoveTypesCursor CURSOR READ_ONLY
	FOR
    SELECT Id,Code, MoveTypeEnglishName , MoveTypeLocalName , TransportModeId, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant
	From dw_MoveTypes
	inner JOIN dw_DWHSettings ON dw_MoveTypes.Tenant = dw_DWHSettings.Tenant
	where dw_MoveTypes.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN MoveTypesCursor FETCH NEXT FROM MoveTypesCursor INTO  @Id ,@Code, @EnglishName, @LocalName,@TransportModeId , @SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_MoveTypes where Id = @Id)
	
	if(@Key is  null) begin     insert into #DIM_MoveTypes (Id, Code ,[English Name] ,[Local Name],[Transport Mode], [Source Tenant],[Parent Tenant]) values(@Id ,@Code, @EnglishName, @LocalName,@TransportModeId , @SourceTenant , @ParentTenant) end
	else begin update   DIM_MoveTypes set [English Name] =@EnglishName,[Code] =@Code, [Local Name] =@LocalName , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant Where Id = @Id end


	FETCH NEXT FROM MoveTypesCursor INTO  @Id ,@Code, @EnglishName, @LocalName,@TransportModeId , @SourceTenant , @ParentTenant
		End
	CLOSE MoveTypesCursor
	DEALLOCATE MoveTypesCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'MoveType'
End

