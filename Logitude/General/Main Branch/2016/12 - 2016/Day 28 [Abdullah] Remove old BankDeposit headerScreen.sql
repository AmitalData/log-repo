update ObjectTables set HeaderScreenId = null where Name = 'bankdeposit'
delete from ScreenFields where ScreenId in (select id from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'bankdeposit'))
delete from Screens where ObjectTableId = ( select Id from ObjectTables where Name = 'bankdeposit')