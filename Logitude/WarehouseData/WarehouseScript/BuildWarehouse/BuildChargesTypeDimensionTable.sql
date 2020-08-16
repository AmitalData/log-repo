
   declare @Id as varchar(15)
   declare @Code as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @ChargeGroupCode as varchar(5)
   declare @IsExpense as bit

   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE ChargesTypeCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Code, EnglishName , LocalName ,ChargesGroupCode, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant,IsExpense, dw_ChargesTypes.AutomaticLastUpdateDate, dw_ChargesTypes.InActive
	From dw_ChargesTypes
	inner JOIN dw_DWHSettings ON dw_ChargesTypes.Tenant = dw_DWHSettings.Tenant
	OPEN ChargesTypeCursor FETCH NEXT FROM ChargesTypeCursor INTO  @Id ,@Code, @EnglishName, @LocalName, @ChargeGroupCode , @SourceTenant , @ParentTenant,@IsExpense, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ChargesTypesTemp (Id, Code ,[English Name] ,[Local Name],[Charge Group Code], [Source Tenant],[Parent Tenant],[Is Expense], [Automatic Last Update Date],[InActive]) values(@Id ,@Code, @EnglishName, @LocalName, @ChargeGroupCode , @SourceTenant , @ParentTenant,@IsExpense, @AutomaticLastUpdateDate, @InActive)

	FETCH NEXT FROM ChargesTypeCursor  INTO @Id ,@Code, @EnglishName, @LocalName, @ChargeGroupCode , @SourceTenant , @ParentTenant,@IsExpense, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE ChargesTypeCursor
	DEALLOCATE ChargesTypeCursor

