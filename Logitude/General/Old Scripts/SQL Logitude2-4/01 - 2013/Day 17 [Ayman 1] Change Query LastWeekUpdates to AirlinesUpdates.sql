

delete from QueryColumns where QueryId = (Select Id From Queries where Code = 'Last Week Updates')
delete from AdvancedQueryFilters where QueryId = (Select Id From Queries where Code = 'Last Week Updates')
delete from Queries where Code = 'Last Week Updates'
delete from TextCodes where Code = 'Shipment.Q.LastWeekUpdates'
delete from TextCodes where Code like 'Shipment.F.LastWeek%'
delete from TextCodes where Code like 'Shipment.Features.LastWeek%'

delete from ObjectFields where FieldName = 'LastWeekUpdates'
delete from ObjectFields where FieldName = 'LastWeekUpdate'
delete from RoleFeatures where FeatureId = (Select Id from Features where Code = 'LASTWEEKUPDATE')
delete from PackageFeatures where FeatureId = (Select Id from Features where Code = 'LASTWEEKUPDATE')
delete from Features where Code = 'LASTWEEKUPDATE'
