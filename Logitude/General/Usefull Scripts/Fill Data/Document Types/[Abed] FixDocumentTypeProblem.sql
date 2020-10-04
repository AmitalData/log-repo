
update DocumentTypes 
set DocumentTypeDefaultHTMLTemplateId = (select top(1) id from DocumentTypeTemplates where DocumentTypeId =DocumentTypes.Id and TemplateType  = 'M' and Tenant = DocumentTypes.Tenant ) WHERE (DocumentTypeDefaultHTMLTemplateId is null or (DocumentTypeDefaultHTMLTemplateId is not null and  DocumentTypeDefaultHTMLTemplateId not in (select id from DocumentTypeTemplates where iD =DocumentTypes.DocumentTypeDefaultHTMLTemplateId and TemplateType  = 'M' and Tenant = DocumentTypes.Tenant AND DocumentTypeId = DocumentTypes.Id )))

update DocumentTypes 
set DocumentTypeDefaultReportTemplateId = (select top(1) id from DocumentTypeTemplates where DocumentTypeId =DocumentTypes.Id and TemplateType  = 'P' and Tenant = DocumentTypes.Tenant ) WHERE (DocumentTypeDefaultReportTemplateId is null or (DocumentTypeDefaultReportTemplateId is not null and  DocumentTypeDefaultReportTemplateId not in (select id from DocumentTypeTemplates where iD =DocumentTypes.DocumentTypeDefaultReportTemplateId and TemplateType  = 'P' and Tenant = DocumentTypes.Tenant AND DocumentTypeId = DocumentTypes.Id )))

update doc set doc.ObjectTableId = (select ObjectTableId from DocumentTypes where Tenant = 0 and Code = doc.Code) from DocumentTypes doc  where Tenant  =doc.tenant and Code = doc.Code



UPDATE DocumentOuts set DocumentTemplateId = (select DocumentTypeDefaultReportTemplateId from DocumentTypes where id = DocumentsFilings.DocumentTypeId and Tenant = DocumentsFilings.Tenant)
from DocumentOuts
inner join DocumentsFilings on DocumentOuts.Id = DocumentsFilings.Id
WHERE DocumentOuts.DocumentTemplateId not in (select DocumentTypeDefaultReportTemplateId from Documenttypes where Id =DocumentsFilings.DocumentTypeId) and DocumentOuts.Tenant = DocumentsFilings.Tenant   AND DocumentsFilings.DirectionCode = 'O' AND DocumentsFilings.DocumentTypeId IS NOT NULL



UPDATE DocumentOuts set EmailTemplateId = (select DocumentTypeDefaultHTMLTemplateId from DocumentTypes where id = DocumentsFilings.DocumentTypeId and Tenant = DocumentsFilings.Tenant)
from DocumentOuts
inner join DocumentsFilings on DocumentOuts.Id = DocumentsFilings.Id
WHERE DocumentOuts.EmailTemplateId not in (select DocumentTypeDefaultHTMLTemplateId from Documenttypes where Id =DocumentsFilings.DocumentTypeId) and DocumentOuts.Tenant = DocumentsFilings.Tenant   AND DocumentsFilings.DirectionCode = 'O' AND DocumentsFilings.DocumentTypeId IS NOT NULL



