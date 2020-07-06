-- General Script From 202007051545_SetVATUniquePartnerDefaultValue.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Tenants set VatUniquePartnerTypeCode = 'CUS'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051545_SetVATUniquePartnerDefaultValue.sxml', GETDATE(), 'update Tenants set VatUniquePartnerTypeCode = ''CUS''', DATEDIFF(MS,@StartTime,@EndTime), '1b727a2b425151f4208bb16daa668fe4', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

