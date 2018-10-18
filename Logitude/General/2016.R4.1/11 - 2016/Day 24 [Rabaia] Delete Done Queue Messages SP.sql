create procedure [dbo].[DeleteDoneQueueMessages]
as

begin

delete from [dbo].[QueueMessages] where [Status] = 1 or ([Status] = -1 and [ProcessingDateTime] < dateadd(second,-86400,getdate()))
 
end