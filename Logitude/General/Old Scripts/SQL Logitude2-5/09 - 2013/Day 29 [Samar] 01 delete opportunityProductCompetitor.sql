begin transaction
begin

drop table OpportunityProductCompetitors

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProductCompetitor')
delete from ObjectFields where FieldName = 'OpportunityProductCompetitors' and ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProduct')

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProductCompetitor')
delete from TextCodes where Code Like '%OpportunityProductCompetitors%' and ObjectTableId = (select Id from ObjectTables where name = 'OpportunityProduct')

delete from ObjectTables where Name = 'OpportunityProductCompetitor'

END
commit transaction