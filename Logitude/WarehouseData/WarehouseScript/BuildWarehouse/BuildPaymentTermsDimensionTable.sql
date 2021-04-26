
   declare @Id as varchar(15)
   declare @Code as varchar(4)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)  
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime 

	DECLARE PaymentTermsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Code, EnglishName, LocalName, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_PaymentTerms.AutomaticLastUpdateDate
	From dw_PaymentTerms
	inner JOIN dw_DWHSettings ON dw_PaymentTerms.Tenant = dw_DWHSettings.Tenant
	OPEN PaymentTermsCursor FETCH NEXT FROM PaymentTermsCursor INTO  @Id ,@Code, @EnglishName, @LocalName, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_PaymentTermsTemp (Id, Code,[English Name],[Local Name], [Source Tenant],[Parent Tenant], [Automatic Last Update Date]) values(@Id ,@Code, @EnglishName, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM PaymentTermsCursor  INTO @Id ,@Code, @EnglishName, @LocalName, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE PaymentTermsCursor
	DEALLOCATE PaymentTermsCursor

