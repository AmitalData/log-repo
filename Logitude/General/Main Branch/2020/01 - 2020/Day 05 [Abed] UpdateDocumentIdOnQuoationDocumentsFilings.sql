update DocumentsFilings
set SecurityId = LTRIM(Id + str(RAND() * POWER(CAST(10 as BIGINT), 10)))
where SecurityId is null

update DocumentsFilings 
set DocumentId =(select top(1) DocumentId from DocumentOutCopies where DocumentOutId = DocumentsFilings.Id and Tenant = DocumentsFilings.Tenant order by CreateDate desc ) 
where ObjectTableId = (select id from ObjectTables where Name = 'Quote') and DocumentTypeId = (select id from DocumentTypes where Code = 'QUOTE' and Tenant =DocumentsFilings.Tenant )

