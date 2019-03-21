
 

 
IF OBJECT_ID('[dbo].[DeleteOldAPILogsTask]', 'P') IS NOT NULL
drop procedure [dbo].[DeleteOldAPILogsTask]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[DeleteOldAPILogsTask]
as 

begin


delete from [dbo].[APILogData] where id in (select id from [dbo].[APILogs] where [CreateDate] < GETDATE() - 90 )
delete from [dbo].[APILogs] where [CreateDate] < GETDATE() - 90

end