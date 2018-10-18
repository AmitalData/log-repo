
alter procedure [dbo].[Queue_Enqueue]
(
    @QueueDefinitionCode varchar(255),
  
    @MessageBody varchar(1000),
	@DelaySeconds int
	
)
 as begin
  declare @currentdate  datetime
  declare @nextRunDateTime  datetime
  set @currentdate =getdate()
  set @nextRunDateTime = dateadd(second,@DelaySeconds,getdate())

 insert into [dbo].[QueueMessages] ([CreateDateTime],[QueueDefinitionCode],[Status],[MessageBody],[NextRunDateTime],RetryNumber)
                             values(@currentdate,@QueueDefinitionCode,0,@MessageBody,@nextRunDateTime,0)


end
