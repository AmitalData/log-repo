--Run this script and update CRM

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'opportunity') and ObjectFieldId=(select id from ObjectFields where FieldName='StageAge'))
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Opportunity.HeaderScreen')

delete from ObjectFields where FieldName = 'StageAge'
delete from TextCodes where Code like '%StageAge%'
