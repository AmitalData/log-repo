create procedure [dbo].[DeleteOldFilingInboxes]
as
begin
	delete from FilingInboxAttachmentLogs where FilingInboxAttachmentId in (select id from FilingInboxAttachments where FilingInboxId in (select id from [dbo].[FilingInboxes] where [CreateDate] < DATEADD(month,-2,GETDATE())))
	delete from FilingInboxAttachments where FilingInboxId in (select id from [dbo].[FilingInboxes] where [CreateDate] < DATEADD(month,-2,GETDATE()))
	delete from [dbo].[FilingInboxes] where [CreateDate] < DATEADD(month,-2,GETDATE()) 
end

	
