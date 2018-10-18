

IF OBJECT_ID('[dbo].[usp_UpdateMobileNotificationLogsMarkReadOrDelete]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateMobileNotificationLogsMarkReadOrDelete]
GO


Create PROCEDURE [dbo].[usp_UpdateMobileNotificationLogsMarkReadOrDelete] 
(
  @pType   varchar(10),
  @pEmail   varchar(70),
  @pNotificationId  varchar(50),
  @pIsAll    bit
)
as 

IF @pType = 'Delete'
BEGIN; 


IF @pIsAll = 1
BEGIN;
Update   MobileNotificationLogs Set IsDelete = 1 where [Email] =  @pEmail ;
End;

ELSE
BEGIN;
Update   MobileNotificationLogs Set IsDelete = 1 where [Id] =  @pNotificationId ;
End;

End;

IF @pType = 'Read'
BEGIN; 

IF @pIsAll = 1
BEGIN;
Update   MobileNotificationLogs Set IsRead = 1 where [Email] =  @pEmail ;
End;

ELSE
BEGIN;
Update   MobileNotificationLogs Set IsRead = 1 where [Id] =  @pNotificationId ;
End;

End;