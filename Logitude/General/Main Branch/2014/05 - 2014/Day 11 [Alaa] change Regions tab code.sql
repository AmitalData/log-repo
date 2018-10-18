update ObjectTableTabs set Code ='SSGN' where Code = 'STGN' and ObjectTableId = ( select id from ObjectTables where name = 'specialservicestype')

