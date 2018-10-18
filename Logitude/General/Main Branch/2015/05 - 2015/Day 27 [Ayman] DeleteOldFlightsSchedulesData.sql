

delete from ObjectFields where ObjectTableId in (select Id from ObjectTables where Name like '%Flight%Schedule%')
go

delete from TextCodes where ObjectTableId in (select Id from ObjectTables where Name like '%Flight%Schedule%')
go

delete from ObjectTables where Name like '%Flight%Schedule%'
go

