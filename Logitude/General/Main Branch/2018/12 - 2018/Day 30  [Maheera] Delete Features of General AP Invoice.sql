delete from QueryColumns  where QueryId in (select id from Queries where Code in ('All General APInvoices','Approval General APInvoices','Draft General APInvoices'))
delete from AdvancedQueryFilters where QueryId in (select id from Queries where Code in ('All General APInvoices','Approval General APInvoices','Draft General APInvoices'))
delete from Queries where Code in ('All General APInvoices','Approval General APInvoices','Draft General APInvoices')
delete from PackageFeatures where FeatureId in (select id  from features where code in ('ALLGENERALAPINVOICES','DRAFTGENERALAPINVOICES','APPROVALGENERALAPINVOICES'))
delete from RoleFeatures where FeatureId in (select id  from features where code in ('ALLGENERALAPINVOICES','DRAFTGENERALAPINVOICES','APPROVALGENERALAPINVOICES'))
delete  from features where code in ('ALLGENERALAPINVOICES','DRAFTGENERALAPINVOICES','APPROVALGENERALAPINVOICES')

