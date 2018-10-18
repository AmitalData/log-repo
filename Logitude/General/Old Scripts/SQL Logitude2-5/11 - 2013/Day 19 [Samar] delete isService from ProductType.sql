
delete from CustomerAdditionalServices
delete from ProductTypes where IsService = 1

delete from ObjectFields where FieldName = 'IsService' and ObjectTableId = (select Id from ObjectTables where Name = 'ProductType')
delete from TextCodes where Code like '%IsService%' and ObjectTableId = (select Id from ObjectTables where Name = 'ProductType')