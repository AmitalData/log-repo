
begin transaction
begin

delete from AdvancedQueryfilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'CurrencyId' AND ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))

delete from ObjectFields where FieldName = 'CurrencyId' AND ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')

delete from TextCodes where Code like '%Opportunity.%CurrencyId%'

END
commit transaction