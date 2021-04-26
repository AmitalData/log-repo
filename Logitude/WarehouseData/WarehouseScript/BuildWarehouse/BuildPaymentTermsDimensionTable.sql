
   declare @Id as varchar(15) 
   declare @Name as varchar(40)
   declare @LocalName as varchar(40)
   declare @Code as varchar(4) 
   declare @SourceTenant int
   declare @ParentTenant int
   --declare @AutomaticLastUpdateDate as datetime

	DECLARE PaymentTermsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name,LocalName, Code,dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant --, dw_QuoteClosingReasons.AutomaticLastUpdateDate
	From dw_PaymentTerms
	inner JOIN dw_DWHSettings ON dw_PaymentTerms.Tenant = dw_DWHSettings.Tenant
	OPEN PaymentTermsCursor FETCH NEXT FROM PaymentTermsCursor INTO  @Id ,@Name, @LocalName,@Code, @SourceTenant, @ParentTenant -- , @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_PaymentTermsTemp (Id, Name, [Local Name], Code,[Source Tenant],[Parent Tenant]) --, [Automatic Last Update Date])
	values (@Id ,@Name, @LocalName, @Code, @SourceTenant,@ParentTenant)--, @AutomaticLastUpdateDate)

	FETCH NEXT FROM PaymentTermsCursor  INTO @Id, @Name, @LocalName, @Code, @SourceTenant,@ParentTenant -- , @AutomaticLastUpdateDate
		End
	CLOSE PaymentTermsCursor
	DEALLOCATE PaymentTermsCursor
 