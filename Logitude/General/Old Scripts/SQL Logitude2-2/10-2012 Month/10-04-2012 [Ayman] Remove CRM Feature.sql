
select * from Features where Code = 'CRM'
select * from TextCodes where Code = 'General.Features.CRM'
select * from RoleFeatures where FeatureId = (Select Id from Features where Code = 'CRM')
select * from PackageFeatures where FeatureId = (Select Id from Features where Code = 'CRM')
select * from MenusTables where FeatureId  = (Select Id from Features where Code = 'CRM')


delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'CRM')
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'CRM')
update MenusTables set FeatureId = (Select Id from Features where Code = 'CUSTOMERS') where MenuTypeCode = 'Main' and Code = 'MCRM' and TextCode = 'General.MH.Customers'
delete from Features where Code = 'CRM'
delete from TextCodes where Code = 'General.Features.CRM'