-- General Script From 202006211023_SetTariffSettingDefaultCurrency.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update TariffSettings set DefaultCurrencyId = (select ProfitCurrencyId from Tenants where Id = TariffSettings.Tenant)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006211023_SetTariffSettingDefaultCurrency.sxml', GETDATE(), 'update TariffSettings set DefaultCurrencyId = (select ProfitCurrencyId from Tenants where Id = TariffSettings.Tenant)', DATEDIFF(MS,@StartTime,@EndTime), '43a37f27e3f9624fb0b112c28a0dd074', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

