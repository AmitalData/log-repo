declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @table_name = 'Opportunities'
set @col_name = 'OpportunityTypeCode'

select @Command = 'ALTER TABLE ' + @table_name + ' drop constraint ' + d.name
 from sys.tables t   
  join    sys.default_constraints d       
   on d.parent_object_id = t.object_id  
  join    sys.columns c      
   on c.object_id = t.object_id      
    and c.column_id = d.parent_column_id
 where t.name = @table_name
  and c.name = @col_name
  
execute (@Command)
go

alter table Opportunities drop column OpportunityTypeCode
go


delete from AdvancedQueryFilters where ObjectFieldId = (select Id from ObjectFields where FieldName = 'OpportunityTypeCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'OpportunityTypeCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from ObjectTableRuleFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'OpportunityTypeCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from ObjectFields where FullNameTextCodeId = (select Id from TextCodes where Code = 'Opportunity.F.OpportunityTypeCode')
delete from ObjectFields where FieldName = 'OpportunityTypeCode' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
delete from TextCodes where Code like '%OpportunityTypeCode%'