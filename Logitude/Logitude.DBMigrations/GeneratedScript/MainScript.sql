-- General Script From 202006011050_UpdateAllowedInTicketFieldValueOfObjectTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update ObjectTables set AllowedInTicket=1 where Name in ('shipment','Quote')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011050_UpdateAllowedInTicketFieldValueOfObjectTable.sxml', GETDATE(), 'update ObjectTables set AllowedInTicket=1 where Name in (''shipment'',''Quote'')', DATEDIFF(MS,@StartTime,@EndTime), '23be70443f012ab36745546aaef594f9', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

