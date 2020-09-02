
   declare @Id as varchar(15)
   declare @Code as nvarchar(100)
   declare @Value as nvarchar(1000)
   declare @IsMultipleChoice bit
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE CustomPickListsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_CustomPickLists.Id,dw_CustomPickLists.Code ,dw_CustomPickLists.Value,dw_CustomPickLists.IsMultipleChoice ,dw_CustomPickLists.Tenant , dw_DWHSettings.ParentTenant, dw_CustomPickLists.AutomaticLastUpdateDate
	From dw_CustomPickLists
	inner JOIN dw_DWHSettings ON dw_CustomPickLists.Tenant = dw_DWHSettings.Tenant

	OPEN CustomPickListsCursor FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0																																																																								
	BEGIN																																																																													
	
	insert into #DIM_CustomPickListsTemp (Id,Code,[Value],[Is Multiple Choice],[Source Tenant] ,[Parent Tenant], [Automatic Last Update Date]) values(@Id,@Code, @Value,@IsMultipleChoice ,  @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE CustomPickListsCursor
	DEALLOCATE CustomPickListsCursor
	
