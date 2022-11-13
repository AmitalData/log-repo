

 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'CustomPickList' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_CustomPickLists )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin


   declare @Id as varchar(15)
   declare @Code as nvarchar(100)
   declare @Value as nvarchar(1000)
   declare @IsMultipleChoice bit
   declare @SourceTenant int
   declare @ParentTenant int
   declare @Key as varchar(15)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE CustomPickListsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_CustomPickLists.Id,dw_CustomPickLists.Code ,dw_CustomPickLists.Value,dw_CustomPickLists.IsMultipleChoice ,dw_CustomPickLists.Tenant , dw_DWHSettings.ParentTenant, dw_CustomPickLists.AutomaticLastUpdateDate
    From dw_CustomPickLists
	inner JOIN dw_DWHSettings ON dw_CustomPickLists.Tenant = dw_DWHSettings.Tenant
	where dw_CustomPickLists.AutomaticLastUpdateDate > @LastUpdateDate	

	OPEN CustomPickListsCursor FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from DIM_CustomPickLists where Id = @Id)

	if(@Key is  null) begin insert into DIM_CustomPickLists (Id,Code,[Value],[Is Multiple Choice],[Source Tenant] ,[Parent Tenant], [Automatic Last Update Date]) values(@Id,@Code, @Value,@IsMultipleChoice ,  @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate)end

	else begin update   DIM_CustomPickLists set Value =@Value,Code= @Code, [Is Multiple Choice] =@IsMultipleChoice , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate where Id = @Id; end

	FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE CustomPickListsCursor
	DEALLOCATE CustomPickListsCursor

		update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'CustomPickList'
	
End

