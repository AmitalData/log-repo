delete from Features where Code = 'ACCOUNTS'

--Account.Features.New
delete from RoleFeatures where FeatureId = (select Id from Features where NameTextCodeId = (select id from TextCodes where Code = 'Account.Features.New'))
delete from Features where NameTextCodeId = (select id from TextCodes where Code = 'Account.Features.New')
delete from TextCodes where Code = 'Account.Features.New'

--Account.Features.Edit
delete from RoleFeatures where FeatureId = (select Id from Features where NameTextCodeId = (select id from TextCodes where Code = 'Account.Features.Edit'))
delete from Features where NameTextCodeId = (select id from TextCodes where Code = 'Account.Features.Edit')
delete from TextCodes where Code = 'Account.Features.Edit'

delete from Features where Code = 'JOURNALS'
--Journal.Features.New
delete from RoleFeatures where FeatureId = (select Id from Features where NameTextCodeId = (select id from TextCodes where Code = 'Journal.Features.New'))
delete from Features where NameTextCodeId = (select id from TextCodes where Code = 'Journal.Features.New')
delete from TextCodes where Code = 'Journal.Features.New'

--Journal.Features.Edit
delete from RoleFeatures where FeatureId = (select Id from Features where NameTextCodeId = (select id from TextCodes where Code = 'Journal.Features.Edit'))
delete from Features where NameTextCodeId = (select id from TextCodes where Code = 'Journal.Features.Edit')
delete from TextCodes where Code = 'Journal.Features.Edit'
