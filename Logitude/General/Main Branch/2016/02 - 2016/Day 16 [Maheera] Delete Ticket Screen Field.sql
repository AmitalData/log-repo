delete ScreenFields 
where 
ScreenId = (select Id from Screens where ObjectTableId = (select id from ObjectTables where Name ='Ticket') and Name ='Header Screen')
and 
ObjectFieldId = (select Id from ObjectFields where FieldName = 'ClassificationName')