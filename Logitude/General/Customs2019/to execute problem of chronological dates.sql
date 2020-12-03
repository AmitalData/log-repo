select * from TextCodes where Code like '%TasksScheduler.ServiceClassNameHelpText%'
select * from ObjectFields where HelpTextCodeId='1-77109'
--delete from TextCodes where Code ='AccountingSetting.IsChronologicalDatesHelpText'

delete from TextCodes where Code = 'AccountingSetting.CH.IsChronologicalDates'

delete from TextCodes where Code = 'Participant.AccountingCardHelpText'
delete from ObjectFields where FieldName = 'IsChronologicalDates'
delete from TextCodes where Code like '%IsChronologicalDates%'
 delete from textcodes where code=  'Participant.CH.AccountingCardListLable'

delete TextCodes where Code = 'Tenant.VersionHelpText'
delete TextCodes where Code = 'Tenant.CH.VersionListLable'

delete from TextCodes where Code='Contact.B.AllowInternetAccess'

delete from ObjectFields where FieldName = 'LastRunTime'
delete from TextCodes where Code='TasksScheduler.LastRunTimeHelpText'
delete from TextCodes where Code like '%LastRunTime%'

delete from ObjectFields where FieldName = 'ServiceClassName'
delete from TextCodes where Code='TasksScheduler.ServiceClassNameHelpText'
delete from TextCodes where Code like '%ServiceClassName%'

select * from ObjectTables where Name='FeatureToggle'

update TenantManagements set BluesnapInttraStockContractQTY=1 where (BluesnapInttraStockContractQTY = 0 OR BluesnapInttraStockContractQTY is null)
