 
declare @ForwarderDocumentId as varchar(40)
declare @Tenant as int
declare @Count as int
declare duplicatedRecordsCursor Cursor read_only
for

select top 1000 ForwarderDocumentId,tenant,count(*)
from documentsfilings
where forwarderdocumentid is not null and isdeleted = 0
group by ForwarderDocumentId,tenant
having count(*) > 1

open duplicatedRecordsCursor
fetch next from duplicatedRecordsCursor into @ForwarderDocumentId,@Tenant,@Count
while @@FETCH_STATUS = 0
begin
print '--------------------------------------------------------------------------------------------------------------------'
print  + '@Count:'  + cast(@Count as varchar(20)) +' @ForwarderDocumentId: '+@ForwarderDocumentId + ' @Tenant:'  + cast(@Tenant as varchar(20))

declare @EmptyDocsRecordsCount as int
set @EmptyDocsRecordsCount = (select count(*) from documentsfilings left outer join documents on documentsfilings.documentid = documents.id
where  ForwarderDocumentId = @ForwarderDocumentId and documentsfilings.Tenant = @Tenant and (documentId is null 
or documents.hasfile = 0) and isdeleted = 0)



if((@Count = @EmptyDocsRecordsCount) or (@EmptyDocsRecordsCount = 0))
begin
if(@Count = @EmptyDocsRecordsCount)
begin
print @ForwarderDocumentId + ': all documents filings have no documents! <====================================='
end

if(@EmptyDocsRecordsCount = 0)
begin
print 'all documents filings are connected to documents @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@'
end 

declare @TopEntityId as varchar(40)
set @TopEntityId = (select top 1 id from DocumentsFilings 
where tenant = @Tenant and ForwarderDocumentId = @ForwarderDocumentId and IsDeleted = 0
order by createdate desc)
print  @TopEntityId + ' will not be deleted'

update DocumentsFilings set IsDeleted = 1 where tenant = @Tenant and ForwarderDocumentId = @ForwarderDocumentId and id <> @TopEntityId and IsDeleted = 0
--select Id,ForwarderDocumentId,ComputedForwarderDocumentId,Tenant from  DocumentsFilings   
--where tenant = @Tenant and ForwarderDocumentId = @ForwarderDocumentId and id <> @TopEntityId and IsDeleted = 0
--order by createdate desc



end
else
begin
print cast(@EmptyDocsRecordsCount as varchar(20)) + ' of ' +  cast(@Count as varchar(20)) + ' ==============================================================> is deleted'

update docFiling set docFiling.IsDeleted = 1 
from DocumentsFilings as docFiling 
left outer join documents as doc on docFiling.documentid = doc.id 
where  ForwarderDocumentId = @ForwarderDocumentId and docFiling.Tenant = @Tenant and (documentId is null 
or doc.hasfile = 0) and IsDeleted = 0

--select docFiling.*  
--from DocumentsFilings as docFiling 
--left outer join documents as doc on docFiling.documentid = doc.id 
--where  ForwarderDocumentId = @ForwarderDocumentId and docFiling.Tenant = @Tenant and (documentId is null 
--or doc.hasfile = 0) and IsDeleted = 0


end
 
fetch next from duplicatedRecordsCursor into  @ForwarderDocumentId,@Tenant,@Count
end
close duplicatedRecordsCursor
deallocate duplicatedRecordsCursor


--update documentsfilings set computedforwarderdocumentid = id where isdeleted = 1 and computedforwarderdocumentid = forwarderdocumentid
--and forwarderdocumentid in (select forwarderdocumentid from documentsfilings where isdeleted = 0)

 

 
--ALTER TABLE documentsfilings ADD  CONSTRAINT [UQ_ComputedForwarderDocumentId_Tenant_DocumentsFilings] UNIQUE NONCLUSTERED 
--(
--	[ComputedForwarderDocumentId] ASC,
--	[Tenant] ASC
--)

 

 