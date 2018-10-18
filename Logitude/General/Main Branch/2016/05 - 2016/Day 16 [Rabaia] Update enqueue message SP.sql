
/****** Object:  StoredProcedure [dbo].[Queue_Enqueue]    Script Date: 05/16/2016 09:02:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Queue_Enqueue]
(
    @QueueDefinitionCode varchar(255),
  
    @MessageBody varchar(1000),
	@DelaySeconds int,
	@CustomerId varchar(15),
	@BatchNumber varchar(15),
	@NextRunDTime datetime = null
	
)
 as begin
  declare @currentdate  datetime
  declare @nextRunDateTime  datetime
  set @currentdate =getdate()
  if @NextRunDTime is null
    set @nextRunDateTime = dateadd(second,@DelaySeconds,getdate())
  else	
    set @nextRunDateTime = @NextRunDTime
  --set @nextRunDateTime = dateadd(second,@DelaySeconds,getdate())

 insert into [dbo].[QueueMessages] ([CreateDateTime],[QueueDefinitionCode],[Status],[MessageBody],[NextRunDateTime],RetryNumber)
                             values(@currentdate,@QueueDefinitionCode,0,@MessageBody,@nextRunDateTime,0)
                             
                             declare @CId as varchar(15);
                             declare @BNo as varchar(15);
                             if @CustomerId = ''
                             set @CId = NULL
                             else	
                             set @CId = @CustomerId
                             if @BatchNumber = ''
                             set @BNo = NULL
                             else
                             set @BNo = @BatchNumber
                             
 insert into [dbo].[QueueMessageMoreDetails] ([Id],[CreateDateTime],[QueueDefinitionCode],[Status],[MessageBody],[NextRunDateTime],RetryNumber,Field1,Field2)
                             values((SELECT SCOPE_IDENTITY()),@currentdate,@QueueDefinitionCode,0,@MessageBody,@nextRunDateTime,0,@CId,@BNo)


end