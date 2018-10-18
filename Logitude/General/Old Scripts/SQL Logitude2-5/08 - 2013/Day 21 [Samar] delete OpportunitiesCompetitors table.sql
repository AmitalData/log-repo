


alter table [OpportunitiesCompetitors] drop constraint [OpportunitiesCompetitors_Opportunity]
go

alter table [OpportunitiesCompetitors] drop constraint [OpportunitiesCompetitors_Competitor]
go

drop table [OpportunitiesCompetitors]
go


delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where name = 'OpportunitiesCompetitors')
delete from TextCodes where code like '%OpportunitiesCompetitors%'
delete from ObjectTables where name = 'OpportunitiesCompetitors'