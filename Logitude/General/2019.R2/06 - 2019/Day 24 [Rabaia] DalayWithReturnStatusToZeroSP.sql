
/****** Object:  StoredProcedure [dbo].[Queue_DelayMessageandChangeStatusToNew]    Script Date: 6/24/2019 8:23:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[Queue_DelayMessageandChangeStatusTozero]
(
   @MessageId bigint,
   @DelaySeconds int
)
 as begin

IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [ID] = @MessageId)

BEGIN
UPDATE [dbo].[QueueMessages]
SET  [NextRunDateTime] = dateadd(second,@DelaySeconds,getdate()),[Status] = 0
WHERE [Id] = @MessageId and [Status] <> 0

UPDATE [dbo].[QueueMessageMoreDetails]
SET  [NextRunDateTime] = dateadd(second,@DelaySeconds,getdate()),[Status] = 0
WHERE [Id] = @MessageId and [Status] <> 0
END


end