
alter procedure [dbo].[Queue_DelayMessage]
(
   @MessageId bigint,
   @DelaySeconds int
)
 as begin

IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [ID] = @MessageId)

BEGIN
UPDATE [dbo].[QueueMessages]
SET  [NextRunDateTime] = dateadd(second,@DelaySeconds,getdate())
WHERE [Id] = @MessageId
END


end
