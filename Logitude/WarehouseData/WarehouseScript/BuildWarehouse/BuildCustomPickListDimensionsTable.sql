
   declare @Id as varchar(15)
   declare @Code as nvarchar(100)
   declare @Value as nvarchar(1000)
   declare @IsMultipleChoice bit
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE CustomPickListsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_CustomPickLists.Id,dw_CustomPickLists.Code ,dw_CustomPickLists.Value,dw_CustomPickLists.IsMultipleChoice ,dw_CustomPickLists.Tenant , dw_DWHSettings.ParentTenant 
	From dw_CustomPickLists
	inner JOIN dw_DWHSettings ON dw_CustomPickLists.Tenant = dw_DWHSettings.Tenant

	OPEN CustomPickListsCursor FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0																																																																								
	BEGIN																																																																													
	
	insert into #DIM_CustomPickListsTemp (Id,Code,[Value],[Is Multiple Choice],[Source Tenant] ,[Parent Tenant]) values(@Id,@Code, @Value,@IsMultipleChoice ,  @SourceTenant , @ParentTenant)

	FETCH NEXT FROM CustomPickListsCursor INTO @Id ,@Code, @Value, @IsMultipleChoice ,@SourceTenant , @ParentTenant
		End
	CLOSE CustomPickListsCursor
	DEALLOCATE CustomPickListsCursor
	
