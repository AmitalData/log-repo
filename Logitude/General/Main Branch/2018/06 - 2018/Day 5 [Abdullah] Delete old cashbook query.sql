


-- WARNING !!  you must update accounting after excute this script
--
-- USAGE: Delete old query columns
--
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'cashbook'))
delete from QueryColumns where QueryId  in (select Id from Queries where ObjectTableId = ( select Id from ObjectTables where Name = 'cashbook'))
