-- General Script From 202007061138_FillSettingsTMExpirationDate.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update settings
set TMPersonalAccessExpirationDate = '2021-01-01 00:00:00'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007061138_FillSettingsTMExpirationDate.sxml', GETDATE(), 'update settings
set TMPersonalAccessExpirationDate = ''2021-01-01 00:00:00''', DATEDIFF(MS,@StartTime,@EndTime), 'ed27b78d9166318f829e716b4143f1a2', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

