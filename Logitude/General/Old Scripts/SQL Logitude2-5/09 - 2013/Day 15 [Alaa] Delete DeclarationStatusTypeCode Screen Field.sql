begin transaction
begin

delete from ScreenFields where ObjectFieldId=( select id from ObjectFields where FieldName='DeclarationStatusTypeCode' and ObjectTableId = ( select id from ObjectTables where name='customs.Declaration'))


END
commit transaction