select * from EntityStatus

update EntityStatus set SearchFields = Code + ','+Name + ',' + (select name from ObjectTables where id = ObjectTableId) + ',' where SearchFields is null

 