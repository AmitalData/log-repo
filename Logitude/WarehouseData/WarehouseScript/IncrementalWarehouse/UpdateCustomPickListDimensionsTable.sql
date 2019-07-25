

	 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'CustomPickList' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_CustomPickLists )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin


   declare @Id as varchar(15)
   declare @Code as nvarchar(100)
   declare @Value as nvarchar(1000)
   declare @IsMultipleChoice bit
   declare @SourceTenant int
   declare @ParentTenant int
   declare @Key as varchar(15)


	DECLARE CustomPickListsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_CustomPickLists.Id,dw_CustomPickLists.Code ,dw_CustomPickLists.Value,dw_CustomPickLists.IsMultipleChoice ,dw_CustomPickLists.Tenant , dw_DWHSettings.ParentTenant 
    From dw_CustomPickLists
	inner JOIN dw_DWHSettings ON dw_CustomPickLists.Tenant = dw_DWHSettings.Tenant
	where dw_CustomPickLists.AutomaticLastUpdateDate > @LastUpdateDate	

	OPEN CustomPickListsCursor FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Id from DIM_CustomPickLists where Id = @Id)

	if(@Key is  null) begin insert into DIM_CustomPickLists (Id,Code,[Value],[Is Multiple Choice],[Source Tenant] ,[Parent Tenant]) values(@Id,@Code, @Value,@IsMultipleChoice ,  @SourceTenant , @ParentTenant)end

	else begin update   DIM_CustomPickLists set Value =@Value,Code= @Code, [Is Multiple Choice] =@IsMultipleChoice , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant where Id = @Id; end

	FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant
		End
	CLOSE CustomPickListsCursor
	DEALLOCATE CustomPickListsCursor
	
End

	update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'CustomPickList'