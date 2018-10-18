alter table eventtypes alter column englishname varchar (45)
alter table eventtypes alter column localname nvarchar (45)

update ObjectFields
set MaxLength = 45 
where FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'EventType')

update ObjectFields
set MaxLength = 45 
where FieldName = 'LocalName' and ObjectTableId = (select id from ObjectTables where Name = 'EventType')


select * from objectfields
where FieldName = 'LocalName' and ObjectTableId = (select id from ObjectTables where Name = 'EventType')