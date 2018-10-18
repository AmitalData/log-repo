
-- Run this Script then update Tenants

delete from ObjectFields where FieldName = 'ARAccountName'
go

delete from ObjectFields where FieldName = 'Account' and ObjectTableId = (select Id from ObjectTables where Name = 'ARPayment')
go

delete from ObjectFields where FieldName = 'Account' and ObjectTableId = (select Id from ObjectTables where Name = 'APPayment')
go

delete from TextCodes where Code like '%CheckOrPaymentRef%'
go

delete from TextCodes where Code like '%ARAccountName%'
go

delete from TextCodes where Code = 'ARPayment.F.Account'
delete from TextCodes where Code = 'ARPayment.AccountHelpText'

delete from TextCodes where Code = 'APPayment.F.Account'
delete from TextCodes where Code = 'APPayment.AccountHelpText'
