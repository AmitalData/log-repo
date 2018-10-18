

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'TEU' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'Revenue' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ChargeableWeight' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CurrencyCode' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'TEU' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'Revenue' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ChargeableWeight' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CurrencyCode' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))
go

delete from ObjectFields where FieldName = 'TEU' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from ObjectFields where FieldName = 'Revenue' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from ObjectFields where FieldName = 'ChargeableWeight' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from ObjectFields where FieldName = 'CurrencyCode ' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code like '%TEU%' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code like '%Revenue%' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code like '%ChargeableWeight%' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code like '%CurrencyCode%' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go