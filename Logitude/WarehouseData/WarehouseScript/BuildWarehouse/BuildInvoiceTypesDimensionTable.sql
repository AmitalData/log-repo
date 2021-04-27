 
   declare @Code as varchar(2)
   declare @Name as varchar(20)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE InvoiceTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_InvoiceTypes.AutomaticLastUpdateDate
	From dw_InvoiceTypes
	inner JOIN dw_DWHSettings ON dw_InvoiceTypes.Tenant = dw_DWHSettings.Tenant
	OPEN InvoiceTypesCursor FETCH NEXT FROMInvoiceTypes INTO  @Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_InvoiceTypesTemp ( Code,[Name], [Source Tenant],[Parent Tenant], [Automatic Last Update Date]) values(@Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM InvoiceTypesCursor  INTO @Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE InvoiceTypesCursor
	DEALLOCATE InvoiceTypesCursor

