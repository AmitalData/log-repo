 
update ObjectFields set CanFilter = 1,ValidForQuerySection1 = 'Opportunity' where fieldname = 'MyOpenOpportunities'
update ObjectFields set CanFilter = 1,ValidForQuerySection1 = 'Opportunity' where fieldname = 'OpenByStageOpp'
update ObjectFields set CanFilter = 1,ValidForQuerySection1 = 'Opportunity' where fieldname = 'CancelledOpportunities'
update ObjectFields set CanFilter = 1,ValidForQuerySection1 = 'Opportunity' where fieldname = 'MyClosedOpportunities'
update ObjectFields set CanFilter = 1,ValidForQuerySection1 = 'Opportunity' where fieldname = 'AllOpenOpportunities'


-- run on global db
update SystemMetadataLastUpdates set ObjectFieldsUpdateDateGMT = SYSUTCDATETIME()
 