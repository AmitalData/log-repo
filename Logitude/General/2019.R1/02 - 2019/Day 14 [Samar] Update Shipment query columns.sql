
declare @Tenant as int
declare @ObjectTableId as varchar(15)
set @ObjectTableId = (select Id from objecttables where name = 'Shipment')

update QueryColumns set ObjectFieldId = (select Id from ObjectFields where FieldName = 'From' and ObjectTableId = @ObjectTableId)
where ObjectFieldId  = (select Id from ObjectFields where fieldname = 'FromPort' and objecttableid = @ObjectTableId)

update QueryColumns set ObjectFieldId = (select Id from ObjectFields where FieldName = 'To' and ObjectTableId = @ObjectTableId)
where ObjectFieldId  = (select Id from ObjectFields where fieldname = 'ToPort' and objecttableid = @ObjectTableId)

update QueryColumns set ObjectFieldId = (select Id from ObjectFields where FieldName = 'Origin' and ObjectTableId = @ObjectTableId)
where ObjectFieldId  = (select Id from ObjectFields where fieldname = 'MainCarriageFromPortName' and objecttableid = @ObjectTableId)

begin
	DECLARE TenantCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		update DocumentTypeTemplates set  TemplateBodyHtml = convert(varbinary(max),REPLACE(convert(varchar(max),TemplateBodyHtml ,0) , '[FromPort]','[From]') ,0) where tenant = @Tenant and EditorTool = 'R'   and DocumentTypeId in (select id from DocumentTypes where ObjectTableId = @ObjectTableId) and  TemplateBodyHtml is not null and convert(varchar(max),TemplateBodyHtml ,0) like '%[FromPort]%'
		update DocumentTypeTemplates set  TemplateBodyHtml = convert(varbinary(max),REPLACE(convert(varchar(max),TemplateBodyHtml ,0) , '[ToPort]','[To]') ,0) where tenant = @Tenant and EditorTool = 'R'   and DocumentTypeId in (select id from DocumentTypes where ObjectTableId = @ObjectTableId) and  TemplateBodyHtml is not null and convert(varchar(max),TemplateBodyHtml ,0) like '%[FromPort]%'
		update DocumentTypeTemplates set  TemplateBodyHtml = convert(varbinary(max),REPLACE(convert(varchar(max),TemplateBodyHtml ,0) , '[MainCarriageFromPortName]','[Origin]') ,0) where tenant = @Tenant and EditorTool = 'R'   and DocumentTypeId in (select id from DocumentTypes where ObjectTableId = @ObjectTableId) and  TemplateBodyHtml is not null and convert(varchar(max),TemplateBodyHtml ,0) like '%[MainCarriageFromPortName]%'
		
	FETCH NEXT FROM TenantCursor     INTO @Tenant
	END
	CLOSE TenantCursor
	DEALLOCATE TenantCursor
END

--update DocumentTypeTemplates set  TemplateBodyHtml = convert(varbinary(max),REPLACE(convert(varchar(max),TemplateBodyHtml ,0) , '[FromPort]','[From]') ,0) where EditorTool = 'R'   and DocumentTypeId in (select id from DocumentTypes where ObjectTableId = @ObjectTableId) and  TemplateBodyHtml is not null and convert(varchar(max),TemplateBodyHtml ,0) like '%[FromPort]%'
--update DocumentTypeTemplates set  TemplateBodyHtml = convert(varbinary(max),REPLACE(convert(varchar(max),TemplateBodyHtml ,0) , '[ToPort]','[To]') ,0) where EditorTool = 'R'   and DocumentTypeId in (select id from DocumentTypes where ObjectTableId = @ObjectTableId) and  TemplateBodyHtml is not null and convert(varchar(max),TemplateBodyHtml ,0) like '%[FromPort]%'
--update DocumentTypeTemplates set  TemplateBodyHtml = convert(varbinary(max),REPLACE(convert(varchar(max),TemplateBodyHtml ,0) , '[MainCarriageFromPortName]','[Origin]') ,0) where EditorTool = 'R'   and DocumentTypeId in (select id from DocumentTypes where ObjectTableId = @ObjectTableId) and  TemplateBodyHtml is not null and convert(varchar(max),TemplateBodyHtml ,0) like '%[MainCarriageFromPortName]%'
