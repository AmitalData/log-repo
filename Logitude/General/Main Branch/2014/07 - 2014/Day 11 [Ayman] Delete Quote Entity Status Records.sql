

update EventTypes set EntityStatusId = null where ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

update EventTypes set EntityStatusId = null where EntityStatusId in (select Id from  EntityStatus where ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from EntityStatus where ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

--select EntityStatusId , Tenant, EnglishName
--from EventTypes 
--where EntityStatusId in (select Id from EntityStatus where ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
--group by EntityStatusId, Tenant, EnglishName
--go