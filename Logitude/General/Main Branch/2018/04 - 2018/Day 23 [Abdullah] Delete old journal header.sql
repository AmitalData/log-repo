
--USAGE: delete old header
update ObjectTables set HeaderScreenId = null where Name = 'journal'
delete from ScreenFields where ScreenId in (select id from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'journal'))
delete from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'journal')