-- This fields are removed from AccountingUpdate class and did not removed from DB (main)
-- >>>>> Execute it and Rebuild ZIP Files

delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalCurrencyId')
delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'ProfitCurrencyId')
delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'SourceEntityReference')
delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalTypeCode')
delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'TotalInForeignCurrency')
delete from ScreenFields where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalCurrencyCode')

delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'SourceEntityReference')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'TotalInForeignCurrency')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalTypeCode')
delete from QueryColumns where ObjectFieldId = (select id from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalCurrencyCode')

delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalCurrencyId'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'ProfitCurrencyId'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'SourceEntityReference'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'SourceEntityId'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'Reference1'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'Reference2'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'Reference3'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'Reference4'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalTypeCode'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'SourceEntityDate'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'TotalInLocalCurrency'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'TotalInForeignCurrency'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'LastUpdateDate'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'StornoDate'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'StornoJournalId'

delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'JournalCurrencyCode'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'TotalInProfitCurrency'
delete from ObjectFields where ObjectTableId = ( select Id from ObjectTables where Name = 'Journal') and FieldName  = 'Description'





