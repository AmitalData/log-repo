select * from objectfields where objecttableid = (select id from ObjectTables where name = 'user') and fieldname = 'EnglishName'
update ObjectFields set DisplayInEntityVariables = 1 where objecttableid = (select id from ObjectTables where name = 'user') and fieldname = 'EnglishName'

--global
update SystemMetadataLastUpdates set ObjectFieldsUpdateDateGMT = SYSUTCDATETIME ()