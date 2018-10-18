
alter procedure [dbo].[Queue_SetStatus]
(
   @MessageId bigint,
   @Statud int
)
 as begin

IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [Id] = @MessageId)

BEGIN
UPDATE [dbo].[QueueMessages]
SET  [Status] = @Statud,[CompleteDateTime] = getdate()
WHERE [Id] = @MessageId
END


end
