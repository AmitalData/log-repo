-------------------------------No need in prodcution ------------------------------------
-----------------------------------------------------------------------------------------


update ARInvoices set TransferStatusCode = 'TR' where IsTransferred = 1
go

update ARInvoices set TransferStatusCode = 'BL' where TransferStatusCode != 'TR' AND DontTransfer = 1
go

update ARInvoices set TransferStatusCode = 'RD' where TransferStatusCode != 'TR' AND TransferStatusCode != 'BL' AND ReadyForTransfer = 1
go

update ARInvoices set TransferError = NULL where TransferStatusCode = 'TR' OR TransferStatusCode = 'RD'
go

alter table ARInvoices drop DF__ARInvoice__IsTra__009508B4
alter table ARInvoices drop DF__ARInvoice__DontT__01892CED
alter table ARInvoices drop DF__ARInvoice__Ready__027D5126

alter table ARInvoices drop column IsTransferred
go

alter table ARInvoices drop column DontTransfer
go

alter table ARInvoices drop column ReadyForTransfer
go


delete from ObjectFields where FieldName = 'DontTransfer' AND ObjectTableId = (Select Id From ObjectTables where Name = 'ARInvoice')
go

delete from ObjectFields where FieldName = 'IsTransferred' AND ObjectTableId = (Select Id From ObjectTables where Name = 'ARInvoice')
go

delete from TextCodes where Code like 'ARInvoice.DontTransfer%'
go

delete from TextCodes where Code like 'ARInvoice.F.DontTransfer%'
go

delete from TextCodes where Code like '%ARInvoice%IsTransferred%'
go



delete from AdvancedQueryFilters where QueryId = (Select Id from Queries where Code = 'Marked As Dont Transfer' AND ObjectTableId = (Select Id From ObjectTables where Name = 'ARInvoice'))
go

delete from QueryColumns where QueryId = (Select Id from Queries where Code = 'Marked As Dont Transfer' AND ObjectTableId = (Select Id From ObjectTables where Name = 'ARInvoice'))
go

delete from Queries where Code = 'Marked As Dont Transfer' AND ObjectTableId = (Select Id From ObjectTables where Name = 'ARInvoice')
go

delete from ObjectFields where FieldName = 'MarkedAsDontTransferInvoices' AND ObjectTableId = (Select Id From ObjectTables where Name = 'ARInvoice')
go

delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'MARKEDASDONTTRANSFER')
go

delete from Features where Code = 'MARKEDASDONTTRANSFER'
go

delete from TextCodes where Code like '%ARInvoice%MarkedAsDont%'
go


-------------------------------------------------------------------
-------------------------------------------------------------------