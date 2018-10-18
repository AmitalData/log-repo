

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Quote.Approved' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Quote.WaitingForCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'Quote.Approved' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'Quote.WaitingForCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from Queries where Code = 'Quote.Approved' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
delete from Queries where Code = 'Quote.WaitingForCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'Quote.Feature.Approved'and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'Quote.Feature.WaitingForCustomer'and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'Quote.Feature.Approved'and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'Quote.Feature.WaitingForCustomer'and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from Features where Code = 'Quote.Feature.Approved'and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
delete from Features where Code = 'Quote.Feature.WaitingForCustomer'and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

delete from TextCodes where Code = 'Quote.Q.Approved' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
delete from TextCodes where Code = 'Quote.Q.WaitingForCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

delete from ObjectFields where FieldName = 'ApprovedWithourAnyShipment' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
delete from ObjectFields where FieldName = 'WatingForCustomerResponse' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

delete from TextCodes where Code like '%ApprovedWithourAnyShipment%' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
delete from TextCodes where Code like '%WatingForCustomerResponse%' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')