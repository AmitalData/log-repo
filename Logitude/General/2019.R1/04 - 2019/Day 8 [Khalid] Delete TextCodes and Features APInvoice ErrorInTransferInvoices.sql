delete from QueryColumns where QueryId In (Select Id from Queries where FeatureId in (Select Id From Features where Code like '%ErrorInTransfer%'))


delete from AdvancedQueryFilters where QueryId In (Select Id from Queries where FeatureId in (Select Id From Features where Code like '%ErrorInTransfer%' ))


delete from Queries where FeatureId in (Select Id From Features where Code like '%ErrorInTransfer%')


delete from PackageFeatures where FeatureId in (Select Id From Features where Code like '%ErrorInTransfer%')


delete from RoleFeatures where FeatureId in (Select Id From Features where Code like '%ErrorInTransfer%' )


delete From Features where Code like '%ErrorInTransfer%' 


delete from ObjectFields where FullNameTextCodeId In ( Select Id from TextCodes where  Code like '%ErrorInTransfer%')

delete from TextCodes where Code like '%ErrorInTransfer%'



delete from QueryColumns where QueryId In (Select Id from Queries where FeatureId=(Select Id From Features where Code like '%ERRORINTRANSFERINVOICES%'))


delete from AdvancedQueryFilters where QueryId In (Select Id from Queries where FeatureId=(Select Id From Features where Code like '%ERRORINTRANSFERINVOICES%' ))


delete from Queries where FeatureId=(Select Id From Features where Code like '%ERRORINTRANSFERINVOICES%' )


delete from PackageFeatures where FeatureId=(Select Id From Features where Code like '%ERRORINTRANSFERINVOICES%')


delete from RoleFeatures where FeatureId=(Select Id From Features where  Code like '%ERRORINTRANSFERINVOICES%' )

delete from QueryColumns where QueryId In ( Select Id from Queries where FeatureId In (Select Id from FEatures where  Code like '%ERRORINTRANSFERINVOICES%'))
delete from AdvancedQueryFilters where QueryId In ( Select Id from Queries where FeatureId In (Select Id from FEatures where Code like '%ERRORINTRANSFERINVOICES%'))
 
delete from Queries where FeatureId In ( Select Id from Features where  Code like '%ERRORINTRANSFERINVOICES%')
delete From Features where Code like '%ERRORINTRANSFERINVOICES%'


delete from ObjectFields where  Code like '%ERRORINTRANSFERINVOICES%'

delete from TextCodes where Code like '%ERRORINTRANSFERINVOICES%'

