-- Procedure Script From DeleteOldErrorLog.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldErrorLog]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldErrorLog] END');
EXEC('Create procedure [dbo].[DeleteOldErrorLog]
as
begin
delete from [dbo].[ErrorLogs] where [LogDate] < GETDATE() - 30
end');


