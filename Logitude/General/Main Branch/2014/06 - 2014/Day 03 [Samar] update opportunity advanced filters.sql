--Run this script then update CRM

delete from AdvancedQueryFilters 
where QueryId = (select Id from Queries where Code = 'My Open Opportunities')