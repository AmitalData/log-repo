
delete from MenuButtons where EventCode = 'SetAsRejectedByCustomer' and FeatureId = 
(select Id from Features where Code = 'SETASREJECTED' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from MenuButtons where EventCode = 'SetAsNoAnswer' and FeatureId = 
(select Id from Features where Code = 'SETASNOANSWER' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from MenuButtons where EventCode = 'ReturnInProgress' and FeatureId = 
(select Id from Features where Code = 'RETURNINPROGRESS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'SETASREJECTED' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'SETASNOANSWER' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'RETURNINPROGRESS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from Features where Code = 'SETASREJECTED' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
delete from Features where Code = 'SETASNOANSWER' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
delete from Features where Code = 'RETURNINPROGRESS' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')