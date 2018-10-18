delete from QueryColumns where ObjectFieldId in (select id from ObjectFields where FullNameTextCodeId in ( select id from TextCodes where ObjectTableId = (select id from objecttables where name = 'activity') and 
(code like '%.to%' or code like '%.cc%' or code like '%.from%')))

delete from AdvancedQueryFilters where ObjectFieldId in (select id  from ObjectFields where FullNameTextCodeId in ( select id from TextCodes where ObjectTableId = (select id from objecttables where name = 'activity') and 
(code like '%.to%' or code like '%.cc%' or code like '%.from%')))

delete  from ObjectFields where FullNameTextCodeId in ( select id from TextCodes where ObjectTableId = (select id from objecttables where name = 'activity') and 
(code like '%.to%' or code like '%.cc%' or code like '%.from%'))

delete from TextCodes where ObjectTableId = (select id from objecttables where name = 'activity') and 
(code like '%.to%' or code like '%.cc%' or code like '%.from%')