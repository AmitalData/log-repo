

alter table OpportunityProducts drop OpportunityProduct_ProductPeriod
go

begin transaction
begin

alter table OpportunityProducts drop column OpportunityProductPeriodCode

delete from ObjectFields where FieldName = 'OpportunityProductPeriodCode' and ObjectTableId  = (select Id from ObjectTables where Name = 'OpportunityProduct')

delete from ObjectFields where FieldName = 'OpportunityProductPeriodName' and ObjectTableId  = (select Id from ObjectTables where Name = 'OpportunityProduct')

delete from TextCodes where Code like '%OpportunityProduct.%OpportunityProductPeriod%'

END
commit transaction