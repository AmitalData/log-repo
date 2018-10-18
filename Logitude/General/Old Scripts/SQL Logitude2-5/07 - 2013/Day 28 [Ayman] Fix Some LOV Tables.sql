

update ObjectFields set DisplayOnLookUp = 0 where FieldName = 'Code' And ObjectTableId  = (Select Id from ObjectTables where Name = 'Rating')
go

update ObjectFields set DisplayOnLookUp = 0 where FieldName = 'Code' And ObjectTableId  = (Select Id from ObjectTables where Name = 'Priority')
go


select DisplayOnLookUp from ObjectFields where FieldName = 'Code' And ObjectTableId  = (Select Id from ObjectTables where Name = 'Rating')
go