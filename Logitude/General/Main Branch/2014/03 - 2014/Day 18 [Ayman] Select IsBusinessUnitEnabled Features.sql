
update RoleFeatures set FeatureAccessLevelCode = 'OR'
go

update Features set IsBusinessUnitEnabled = 0
go

update Features set IsBusinessUnitEnabled = 1 where (Code = 'READ' OR Code = 'NEW' OR Code = 'UPDATE') AND ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

update Features set IsBusinessUnitEnabled = 1 where (Code = 'READ' OR Code = 'NEW' OR Code = 'UPDATE') AND ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
go

update Features set IsBusinessUnitEnabled = 1 where (Code = 'READ' OR Code = 'NEW' OR Code = 'UPDATE') AND ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go


