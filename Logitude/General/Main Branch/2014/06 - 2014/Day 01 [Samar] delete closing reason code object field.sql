
delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ClosingReasonCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from ObjectFields where FieldName = 'ClosingReasonCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')and Code like '%ClosingReasonCode%'