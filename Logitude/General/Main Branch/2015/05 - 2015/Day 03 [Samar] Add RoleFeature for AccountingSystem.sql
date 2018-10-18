--execute after update

declare @ObjectTableId as varchar(15)
declare @FeatureId as varchar(15)
declare @RoleId as varchar(15)
declare @RoleFeatureId as varchar(15)

set @ObjectTableId = (select Id from ObjectTables where Name = 'AccountingSystem')
set @FeatureId = (select Id from Features where Code = 'UPDATE' and ObjectTableId = @ObjectTableId)
set @RoleId = (select Id from Roles where Code = 'ADMN')

EXECUTE usp_GetNextTableIdValue @RoleFeatureId OUTPUT,'RoleFeature'

insert into RoleFeatures(Id, RoleId, Tenant, FeatureId, FeatureAccessLevelCode)
values(@RoleFeatureId, @RoleId, 0, @FeatureId, 'OR')