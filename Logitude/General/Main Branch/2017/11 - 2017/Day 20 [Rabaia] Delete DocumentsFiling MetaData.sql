

declare @ObjectTableId as varchar(15)
set @ObjectTableId = (select Id from ObjectTables where Name = 'DocumentsFiling')
delete from AdvancedQueryFilters where @ObjectTableId = @ObjectTableId
delete from ObjectFields where ObjectTableId = @ObjectTableId
delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = @ObjectTableId)
delete from QueryColumns where QueryId in (select Id  from Queries where ObjectTableId = @ObjectTableId)
delete from Queries where ObjectTableId = @ObjectTableId
delete From ObjectTableTabs where ObjectTableId = @ObjectTableId
delete from MenusTables where ObjectTableId = @ObjectTableId
delete from RoleFeatures where FeatureId in (select Id  from Features where ObjectTableId = @ObjectTableId)
delete from PackageFeatures where FeatureId in (select Id  from Features where ObjectTableId = @ObjectTableId)
delete from Features where ObjectTableId = @ObjectTableId
delete from TextCodes where ObjectTableId = @ObjectTableId

--delete from Screens where ObjectTableId = @ObjectTableId

-- Need to delete also counters if we want to delete the object table it self
--delete from ObjectTables where Id = @ObjectTableId