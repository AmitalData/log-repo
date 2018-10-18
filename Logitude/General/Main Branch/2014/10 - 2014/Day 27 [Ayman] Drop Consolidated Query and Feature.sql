
delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'Consolidated')
go

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Consolidated')
go

delete from Queries where Code = 'Consolidated'
go

delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'ConsolidatedInvoices')
go

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'ConsolidatedInvoices')
go

delete from Features where Code = 'ConsolidatedInvoices'
go

delete from ObjectFields where FieldName = 'ConsolidatedInvoices'
go

delete from TextCodes where Code = 'ARInvoice.Q.Consolidated'
delete from TextCodes where Code = 'ARInvoice.F.ConsolidatedInvoices'
delete from TextCodes where Code = 'ARInvoice.ConsolidatedInvoicesHelpText'
delete from TextCodes where Code = 'ARInvoice.Features.ConsolidatedInvoices'
