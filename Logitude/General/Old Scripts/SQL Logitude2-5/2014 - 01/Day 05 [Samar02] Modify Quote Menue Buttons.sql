
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'SETASAPPROVED' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from MenuButtons where FeatureId = (select Id from Features where Code = 'SETASAPPROVED' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from Features where Code = 'SETASAPPROVED' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')

delete from TextCodes where Code = 'Quote.B.SetAsApproved'