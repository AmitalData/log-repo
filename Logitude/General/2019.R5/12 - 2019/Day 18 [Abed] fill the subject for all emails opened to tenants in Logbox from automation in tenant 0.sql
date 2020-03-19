update DocumentTypeTemplates 
set  [Subject] = (select top(1) Subject from DocumentTypeTemplates t2  where t2.Id = DocumentTypeTemplates.OriginalTemplateId ) 
where id in (select TemplateId from Automations where   ResultCode = 'EMAIL' and tenant !=0  ) 
and tenant !=0 and Subject is null and OriginalTemplateId is not null
