-- General Script From 202006171015_TruncateErrorLogs.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
truncate table ErrorLogs
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006171015_TruncateErrorLogs.sxml', GETDATE(), 'truncate table ErrorLogs', DATEDIFF(MS,@StartTime,@EndTime), '4b0ef6ced1a3cd4c85a6be7db124f40c', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

