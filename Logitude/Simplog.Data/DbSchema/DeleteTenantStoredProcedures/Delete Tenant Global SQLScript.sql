--delete global records execute this on global database
Create  PROCEDURE DeleteTenantFromGlobalDB
(

@tenant int                     --Input parameter ,  tenant to delete

)
AS
Begin
delete from PerformanceLogs where Tenant=@tenant
delete from AnalyzeQueues where Tenant=@tenant
delete from ContactPasswords where Email in (select Email from GlobalContacts where globaltenantid=@tenant)
--delete from __MigrationHistor where globaltenantid=@tenanty


--delete from HelpResources where globaltenantid=@tenant
delete from PasswordResetRequests where Email in (select Email from GlobalContacts where globaltenantid=@tenant)
delete from LogitudeLeads where TenantNumber=@tenant
delete from MobileNotificationLogs where Tenant=@tenant
--delete from PaymentMethods where t=@tenant
--delete from AnalyzeQueueStatus where globaltenantid=@tenant
--delete from PaymentChannels where globaltenantid=@tenant
--delete from PaymentCurrencies where globaltenantid=@tenant
--delete from RecurringPeriods where globaltenantid=@tenant
--delete from AWBMessagesCCSTypes where globaltenantid=@tenant
--delete from GlobalDBs where globaltenantid=@tenant
--delete from GlobalTenantCounters where globaltenantid=@tenant
--delete from Settings where globaltenantid=@tenant
--delete from SystemMetadataLastUpdates where globaltenantid=@tenant
--delete from ApiCredintials where globaltenantid=@tenant
--delete from AutoSignupEmails where globaltenantid=@tenant
--delete from BatchServicesDefinitions where globaltenantid=@tenant
--delete from BluesnapContracts where globaltenantid=@tenant
delete from ChangePasswordLogs where Email in (select Email from GlobalContacts where globaltenantid=@tenant)
delete from ContactMobileDevices where Email in (select Email from GlobalContacts where globaltenantid=@tenant)
delete from GlobalContacts where globaltenantid = @tenant
delete from TenantManagements where Id=@tenant
delete from GlobalTenants where Id=@tenant
--delete from ConvertProgramInfoes where globaltenantid=@tenant
--delete from MobileNotificationLogs where globaltenantid=@tenant
--delete from MonitorServiceLastUpdates where globaltenantid=@tenant
--delete from TenantTypes where globaltenantid=@tenant

End



