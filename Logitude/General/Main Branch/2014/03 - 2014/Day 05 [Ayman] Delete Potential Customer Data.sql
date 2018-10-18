
delete from ObjectTableTabs where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from MenusTables where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer'))
go

delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from QueryGroups where Code = 'POTC'
go

delete from RoleFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer'))
go

delete from PackageFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer'))
go

delete from Features where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer'))
go

update  ObjectTables 
set HeaderScreenId = null,
DescriptionTextCodeId = null
where Name = 'PotentialCustomer'
go

delete from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from TraceEvents where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from EventTypes where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from EntityLastActivities where ObjectTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

update ObjectFields set LookUpTableId = null where LookUpTableId = (select Id from ObjectTables where Name = 'PotentialCustomer')
go

delete from ObjectTables where Name = 'PotentialCustomer'
go


