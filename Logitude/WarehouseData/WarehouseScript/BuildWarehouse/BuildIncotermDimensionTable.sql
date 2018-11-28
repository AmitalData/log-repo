
   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(3)
   declare @SourceTenant int
   declare @ParentTenant int
   

	DECLARE IncotermsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Incoterms.Id, dw_Incoterms.Name , dw_Incoterms.LocalName ,dw_Incoterms.Code, dw_Incoterms.Tenant,dw_DWHSettings.ParentTenant
	From dw_Incoterms
	inner JOIN dw_DWHSettings ON dw_Incoterms.Tenant = dw_DWHSettings.Tenant
	OPEN IncotermsCursor FETCH NEXT FROM IncotermsCursor INTO @Id , @Name, @LocalName, @Code, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_IncotermsTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant]) values(@Id,@Name,@LocalName,@Code ,@SourceTenant , @ParentTenant)

	FETCH NEXT FROM IncotermsCursor  INTO @Id , @Name, @LocalName, @Code,@SourceTenant , @ParentTenant
		End
	CLOSE IncotermsCursor
	DEALLOCATE IncotermsCursor
	

