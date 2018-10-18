
create procedure [dbo].[Queue_ReturnMessage]
(
   @MessageId bigint
)
 as begin

IF exists (SELECT [Id]
FROM [dbo].[QueueMessages] WITH (UPDLOCK) WHERE  [ID] = @MessageId)

BEGIN
UPDATE [dbo].[QueueMessages]
SET  [NextRunDateTime] = getdate()
WHERE [Id] = @MessageId
END


end
