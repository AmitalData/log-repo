
-- Execute this Script on SystemLogs DataBase Not Main

GO
/****** Object:  StoredProcedure [dbo].[DeleteDoneQueueMessages]    Script Date: 2018-08-28 3:40:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[DeleteOldErrorLog]
as 

begin

delete from [dbo].[ErrorLogs] where [LogDate] < GETDATE() - 30
 
end