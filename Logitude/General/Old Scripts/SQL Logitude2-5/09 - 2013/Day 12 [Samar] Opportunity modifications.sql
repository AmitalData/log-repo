-- line by line 

begin transaction
begin

--select * from [Opportunities]

--alter table [Opportunities] add [Value] decimal null 

--alter table [Opportunities] add [Revenue] decimal null 

update [Opportunities]
set Value = EstimatedRevenue

update [Opportunities]
set Revenue = BudgetAmount

alter table [Opportunities] drop column [EstimatedRevenue]

alter table [Opportunities] drop column [BudgetAmount]

delete from QueryColumns where ObjectFieldId = ( select Id from ObjectFields where FieldName = 'EstimatedRevenue' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from QueryColumns where ObjectFieldId = ( select Id from ObjectFields where FieldName = 'BudgetAmount' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))

delete from ObjectFields where FieldName = 'EstimatedRevenue' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
delete from ObjectFields where FieldName = 'BudgetAmount' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')

delete from TextCodes where  Code = 'Opportunity.F.EstimatedRevenue' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
delete from TextCodes where  Code = 'Opportunity.EstimatedRevenueHelpText' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
delete from TextCodes where  Code = 'Opportunity.CH.EstimatedRevenueListLable' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
delete from TextCodes where  Code like '%BudgetAmount%' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')

END
commit transaction

