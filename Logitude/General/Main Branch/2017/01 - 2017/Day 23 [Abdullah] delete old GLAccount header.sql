

--USAGE: delete old GLAccount header
update ObjectTables set HeaderScreenId = null where Name = 'glaccount'
delete from ScreenFields where ScreenId in (select id from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'glaccount'))
delete from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'glaccount')

--
-- IMPORTANT!!
-- Ececute it before Update Accounting , then UPDATE + ZIP
--