
update QueryColumns
set ColumnWidth = 40
where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ActivityTypeName' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity'))