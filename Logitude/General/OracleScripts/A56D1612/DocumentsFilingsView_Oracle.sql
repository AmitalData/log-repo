CREATE VIEW DocumentsFilingsView
AS
select DocumentsFilings.Id as Id ,DocumentsFilings.Description as Description
,DocumentsFilings.Code as Code, DocumentsFilings.Tenant as Tenant,
 DocumentsFilings.DirectionCode as DirectionCode, DocumentsFilings.CreateDate as CreateDate,
 DocumentsFilings.CreatedByUserId as CreatedByUserId, DocumentsFilings.ExternalEntityReference as ExternalEntityReference,Documents.Extension  AS Extension,
 Documents.HasFile AS HasFile, Documents.FileName AS FileName, DocumentTypes.Name AS DocumentTypeName,
 DocumentTypes.Code AS DocumentTypeCode, Contacts.EnglishName AS OwnerName,CustomsDocuments.CustomsDocId AS CustomsDocId,
 DocumentsFilings.HasCopies as HasCopies,DocumentsFilings.DocumentId as DocumentId,DocumentsFilings.ObjectTableId as ObjectTableId ,DocumentsFilings.ChildEntityId as ChildEntityId,
 DocumentsFilings.DocumentTypeId as DocumentTypeId,DocumentsFilings.EntityId as EntityId,DocumentsFilings.Notes as Notes,DocumentsFilings.OwnerId as OwnerId,DocumentsFilings.SearchFields AS SearchFields
 ,Documents.FileSize AS FileSize,DocumentsFilings.Received,DocumentsFilings.ReceivedDate,DocumentsFilings.ReceivedByUserId as ReceivedByUserId
 ,DocumentsFilings.StatusCode as StatusCode,DocumentsFilings.UpdateDate as UpdateDate,DocumentsFilings.UpdatedByUserId as UpdatedByUserId,DocumentsFilings.EntityReference as EntityReference
 ,DocumentsFilings.ExternalEntityName as ExternalEntityName,DocumentsFilings.FolderId as FolderId,DocumentsFilings.IsDeleted as IsDeleted
,DocumentsFilings.DeleteDateTime as DeleteDateTime,DocumentsFilings.DeletedByUserId as DeletedByUserId,DocumentsFilings.IsDigitallySigned as IsDigitallySigned,DocumentsFilings.SignersList as SignersList
,DocumentsFilings.IsSharedWithCustomer as IsSharedWithCustomer,DocumentsFilings.IsSharedWithForwarder as IsSharedWithForwarder,DocumentsFilings.CustomerDocumentId as CustomerDocumentId,
DocumentsFilings.ForwarderDocumentId as ForwarderDocumentId ,DocumentsFilings.SecurityId as SecurityId,
DocumentsFilings.CustomerTenantNumber as CustomerTenantNumber,DocumentsFilings.ChildObjectTableId as ChildObjectTableId,
DocumentsFilings.ChildEntityReference as ChildEntityReference ,DocumentsFilings.ExternalCode as ExternalCode , DocumentsFilings.Folder as Folder ,DocumentsFilings.CustomsDocumentTypeName as CustomsDocumentTypeName,
DocumentsFilings.CustomsDocumentTypeCode as CustomsDocumentTypeCode
FROM DocumentsFilings 
 LEFT OUTER JOIN DocumentTypes ON DocumentsFilings.DocumentTypeId = DocumentTypes.Id  
 LEFT OUTER JOIN Documents ON DocumentsFilings.DocumentId = Documents.Id 
 LEFT OUTER JOIN Contacts ON DocumentsFilings.OwnerId = Contacts.Id 
 LEFT OUTER JOIN CustomsDocuments ON DocumentsFilings.Id = CustomsDocuments.DocumentsFilingId
 LEFT OUTER JOIN Contacts as CreatedByUsers ON DocumentsFilings.CreatedByUserId = Contacts.Id 
 
 
 