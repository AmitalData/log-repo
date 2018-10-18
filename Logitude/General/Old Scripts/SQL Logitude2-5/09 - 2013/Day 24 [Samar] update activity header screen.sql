
begin transaction
begin

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'ActivityTypeName' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity'))

END
commit transaction