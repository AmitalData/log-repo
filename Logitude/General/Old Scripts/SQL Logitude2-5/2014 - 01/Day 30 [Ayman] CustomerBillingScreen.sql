
-- run this script and update tenants

delete from ScreenFields 
where
ScreenId = (select Id from Screens where Code = 'Customer.GeneralTabScreen' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
go

delete from ScreenFields 
where
ScreenId = (select Id from Screens where Code = 'Customer.BillingTabScreen' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
and ObjectFieldId = (select Id from ObjectFields where FieldName = 'VatNumber' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer') )
go