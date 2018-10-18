
delete from ObjectTableTabs where Code = 'QTGC'

delete from TextCodes where Code = 'Quote.TH.General'

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'GENERAL' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from Features where Code = 'GENERAL' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')