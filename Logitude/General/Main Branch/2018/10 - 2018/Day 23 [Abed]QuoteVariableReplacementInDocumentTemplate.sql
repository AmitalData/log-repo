
----begin


----declare @ObjectTableId as varchar(15)

----set @ObjectTableId = (select id from ObjectTables where Name = 'Quote')

----declare @Tenant as int

----	 DECLARE TenantCursor CURSOR READ_ONLY
----	FOR
----	SELECT Id
----	From Tenants
----	 OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
----	 WHILE @@FETCH_STATUS = 0
----	BEGIN


----update DocumentTypeTemplates set  TemplateBodyHtml = convert(varbinary(max),REPLACE(convert(varchar(max),TemplateBodyHtml ,0) , '[OrderNumberOfPackages]','[NumberOfPackages]') ,0) where tenant = @Tenant and EditorTool = 'R'   and DocumentTypeId =(select id from DocumentTypes where ObjectTableId = @ObjectTableId and id =DocumentTypeId) and  TemplateBodyHtml is not null and convert(varchar(max),TemplateBodyHtml ,0) like '%[OrderNumberOfPackages]%'


----	FETCH NEXT FROM TenantCursor     INTO @Tenant
----	END
----	 CLOSE TenantCursor
----	 DEALLOCATE TenantCursor
----END

