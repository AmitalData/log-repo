

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'Age' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from ObjectFields where FieldName = 'Age' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
delete from TextCodes where Code like 'Age' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Opportunity.HeaderScreen')