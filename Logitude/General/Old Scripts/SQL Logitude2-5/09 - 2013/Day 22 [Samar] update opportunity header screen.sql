
begin transaction
begin

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'RatingName' and ObjectTableId = (select id from ObjectTables where name = 'Opportunity'))

END
commit transaction