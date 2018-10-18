
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'AIRLINETENANT')
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'CONNECTEDAIRLINES')

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'AIRLINETENANT')
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'CONNECTEDAIRLINES')

delete from Features where Code = 'AIRLINETENANT'
delete from Features where Code = 'CONNECTEDAIRLINES'

delete from TextCodes where Code = 'Airline.TH.AirlineTenant'
delete from TextCodes where Code = 'TenantManagement.TH.ConnectedAirlines'

delete from ObjectTableTabs where Code = 'ALTN'
delete from ObjectTableTabs where Code = 'TGCA'