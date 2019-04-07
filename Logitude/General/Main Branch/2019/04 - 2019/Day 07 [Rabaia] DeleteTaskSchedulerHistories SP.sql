/****** Object:  StoredProcedure [dbo].[DeleteOldFilingInboxes]    Script Date: 2019-04-07 10:27:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[DeleteTaskSchedulerHistories]
( 
  @TaskId  as  varchar(15)
)
as
begin 
	delete from [dbo].[TaskSchedulerHistory] where [TaskId] = @TaskId and [StartDateTime] < DATEADD(month,-3,GETDATE()) 
end

	
