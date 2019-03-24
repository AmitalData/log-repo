
BEGIN

declare @Id as varchar(15)
declare @SearchFields as nvarchar(1000)
declare @CompanyVat as nvarchar(100)
declare @CustomerTenant as int


       DECLARE CustomerTenantAccessCursor CURSOR READ_ONLY
       FOR
       SELECT Id,CompanyVat , CustomerTenant , SearchFields
       From CustomerTenantAccesses 
       OPEN CustomerTenantAccessCursor FETCH NEXT FROM CustomerTenantAccessCursor INTO @Id,@CompanyVat,@CustomerTenant,@SearchFields
       WHILE @@FETCH_STATUS = 0
       BEGIN

	   declare @AdditionalSearchFields as varchar(1000)



 

	  set	@AdditionalSearchFields =  @CompanyVat + ',' + CAST(@CustomerTenant as varchar(100))   ;



		IF CHARINDEX(@AdditionalSearchFields,@SearchFields) !> 0
	    BEGIN
		
        set @SearchFields = @SearchFields + ','+ @AdditionalSearchFields 

        update  CustomerTenantAccesses  set SearchFields= @SearchFields where Id =@Id 

     END
	ELSE


       FETCH NEXT FROM CustomerTenantAccessCursor INTO @Id,@CompanyVat,@CustomerTenant,@SearchFields
       END 
       CLOSE CustomerTenantAccessCursor
       DEALLOCATE CustomerTenantAccessCursor
END



