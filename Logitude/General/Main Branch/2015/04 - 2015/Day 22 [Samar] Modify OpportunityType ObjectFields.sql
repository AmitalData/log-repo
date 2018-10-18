update ObjectFields
set CanFilter = 0, DisplayInList = 0
where FieldName = 'Code' and ObjectTableId = (select Id from ObjectTables where Name = 'OpportunityType')

update ObjectFields
set DisplayInList = 0
where FieldName = 'SearchFields' and ObjectTableId = (select Id from ObjectTables where Name = 'OpportunityType')