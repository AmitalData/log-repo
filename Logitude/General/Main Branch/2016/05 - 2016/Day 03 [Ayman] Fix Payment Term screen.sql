
-- Run this then update Tenant0
delete from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'PaymentTerm') and Code = 'PaymentTerm.GeneralTabScreen'
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'PaymentTerm') and Code = 'PaymentTerm.GeneralTabScreen')
go

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'PaymentTerm') and FieldName in ('Net', 'FromDate')
go

delete from TextCodes where Code in ('PaymentTerm.F.Net','PaymentTerm.NetHelpText','PaymentTerm.F.FromDate','PaymentTerm.FromDateHelpText')
go

