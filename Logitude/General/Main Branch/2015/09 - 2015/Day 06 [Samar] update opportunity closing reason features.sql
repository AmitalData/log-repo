delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'ClosingReason.Tab.General')
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'ClosingReason.Tab.Events')

delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'ClosingReason.Tab.General')
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'ClosingReason.Tab.Events')

delete from ObjectTableTabs where FeatureId = (select Id from Features where Code = 'ClosingReason.Tab.General')
delete from ObjectTableTabs where FeatureId = (select Id from Features where Code = 'ClosingReason.Tab.Events')

delete from Features where Code = 'ClosingReason.Tab.General'
delete from Features where Code = 'ClosingReason.Tab.Events'