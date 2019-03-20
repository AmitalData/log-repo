
 
 IF OBJECT_ID('[dbo].[DeleteOldQueueMessageMoreDetailsTask]', 'P') IS NOT NULL
drop procedure [dbo].[DeleteOldQueueMessageMoreDetailsTask]
GO




GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[DeleteOldQueueMessageMoreDetailsTask]
as 

begin

delete from [dbo].[QueueMessageMoreDetails] where [CreateDateTime] < GETDATE() - 90

end