--*****************************************
--****************STEPS********************
-- 1) update tenant 0 and other tenants
-- 2) update CRM
-- 3) execute this script
-- 4) execute "Day 02 [Samar] set foreign key relations for CRM" script
--*****************************************

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProductType')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProductType')

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProductPeriod')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProductPeriod')

delete from ObjectTables where name = 'OpportunityProductType'
delete from ObjectTables where name = 'OpportunityProductPeriod'


alter table [OpportunityProducts] drop constraint [OpportunityProduct_OpportunityProductType]
go

alter table [OpportunityProducts] drop constraint [OpportunityProduct_OpportunityProductPeriod]
go

alter table [OpportunityProductCompetitors] drop constraint [FK_OpportunityProductCompetitor_OpportunityProductType]
go

alter table [OpportunityProductLocations] drop constraint [OpportunityProductLocation_OpportunityProductType]
go

alter table [OpportunityProductCompetitors] drop constraint [FK_OpportunityProductCompetitor_OpportunityProductPeriod]
go