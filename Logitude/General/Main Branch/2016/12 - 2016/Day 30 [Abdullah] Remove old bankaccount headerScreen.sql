-- USAGE: delete old bankaccount header
update ObjectTables set HeaderScreenId = null where Name = 'bankaccount'
delete from ScreenFields where ScreenId in (select id from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'bankaccount'))
delete from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'bankaccount')

-- (MUST) AFTER EXECUTE IT 
-- Update Accounting > Build ZIP