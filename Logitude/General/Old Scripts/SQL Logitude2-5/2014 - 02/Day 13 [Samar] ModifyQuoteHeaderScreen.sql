
delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'EntityStatusId' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')) 
and ScreenId = (select Id from Screens where Code = 'Quote.HeaderScreen')