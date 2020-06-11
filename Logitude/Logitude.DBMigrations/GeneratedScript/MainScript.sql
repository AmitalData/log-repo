-- General Script From 202006092059_SetPermissionToAllBIFolders.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update BIReportFolders set PermissionForAll = 1
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006092059_SetPermissionToAllBIFolders.sxml', GETDATE(), 'update BIReportFolders set PermissionForAll = 1', DATEDIFF(MS,@StartTime,@EndTime), '1f42afaf1e581cb5298189859895c618', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

