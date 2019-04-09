delete from QueryColumns where QueryId In (Select Id from Queries where FeatureId=(Select Id From Features where Code='ERRORINTRANSFER' and ObjectTableId=(Select Id from ObjectTables where Name= 'APInvoice')))


delete from AdvancedQueryFilters where QueryId In (Select Id from Queries where FeatureId=(Select Id From Features where Code='ERRORINTRANSFER' and ObjectTableId=(Select Id from ObjectTables where Name= 'APInvoice')))


delete from Queries where FeatureId=(Select Id From Features where Code='ERRORINTRANSFER' and ObjectTableId=(Select Id from ObjectTables where Name= 'APInvoice'))


delete from PackageFeatures where FeatureId=(Select Id From Features where Code='ERRORINTRANSFER' and ObjectTableId=(Select Id from ObjectTables where Name= 'APInvoice'))


delete from RoleFeatures where FeatureId=(Select Id From Features where Code='ERRORINTRANSFER' and ObjectTableId=(Select Id from ObjectTables where Name= 'APInvoice'))


delete From Features where Code='ERRORINTRANSFER' and ObjectTableId=(Select Id from ObjectTables where Name= 'APInvoice')


delete from ObjectFields where FullNameTextCodeId In ( Select Id from TextCodes where ObjectTableId=(Select Id from ObjectTables where Name ='APInvoice') and Code = 'ERRORINTRANSFER')

delete from TextCodes where ObjectTableId=(Select Id from ObjectTables where Name ='APInvoice') and Code = 'ERRORINTRANSFER'