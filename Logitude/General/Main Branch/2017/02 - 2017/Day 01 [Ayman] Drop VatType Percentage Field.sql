
delete from QueryColumns
where
ObjectFieldId in (select Id from ObjectFields where FieldName = 'Percentage' and ObjectTableId = (select Id from ObjectTables where Name = 'VatType'))
and
QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'VatType'))


delete from ObjectFields where FieldName = 'Percentage' and ObjectTableId = (select Id from ObjectTables where Name = 'VatType')
delete from TextCodes where Code in ('VatType.F.Percentage', 'VatType.CH.PercentageListLable', 'VatType.PercentageHelpText')