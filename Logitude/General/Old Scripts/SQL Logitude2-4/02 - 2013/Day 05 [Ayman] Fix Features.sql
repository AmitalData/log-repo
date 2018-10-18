

delete from RoleFeatures where FeatureId = (select Id from Features where code = 'ACCOUNTING' AND ObjectTableId = (select Id from ObjectTables where name = 'ChargesType'))
go

delete from ObjectTableTabs where code = 'CHAC' AND ObjectTableId = (select Id from ObjectTables where name = 'ChargesType')
go

delete from Features where code = 'ACCOUNTING' AND ObjectTableId = (select Id from ObjectTables where name = 'ChargesType')
go


delete from RoleFeatures where FeatureId = (select Id from Features where code = 'TRANSFER' AND ObjectTableId = (select Id from ObjectTables where name = 'ARInvoice'))
go

delete from ObjectTableTabs where code = 'INTR' AND ObjectTableId = (select Id from ObjectTables where name = 'ARInvoice')
go

delete from Features where code = 'TRANSFER' AND ObjectTableId = (select Id from ObjectTables where name = 'ARInvoice')
go

delete from TextCodes where code = 'ARInvoice.Features.Transfer'
delete from TextCodes where code = 'ARInvoice.TH.ARTransfer'
