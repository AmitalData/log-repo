
delete from QueryColumns where QueryId = (Select Id From Queries where Code = 'Transfered Invoices')
delete from AdvancedQueryFilters where QueryId = (Select Id From Queries where Code = 'Transfered Invoices')
delete from Queries where Code = 'Transfered Invoices'

delete from ObjectFields where FieldName = 'TransferedInvoices' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')
go

delete from TextCodes where Code = 'ARInvoice.Q.TransferedInvoices'
delete from TextCodes where Code like 'ARInvoice.F.TransferedInvoices%'

delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'TRANSFEREDINVOICES')
delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'TRANSFEREDINVOICES')
delete from Features where Code = 'TRANSFEREDINVOICES'

delete from TextCodes where Code like 'ARInvoice.Features.TransferedInvoices%'

