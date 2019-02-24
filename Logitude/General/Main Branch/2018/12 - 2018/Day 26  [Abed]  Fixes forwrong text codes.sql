

delete ObjectFields WHERE FieldName = 'StateId' and ObjectTableId = (select id from ObjectTables where Name = 'CountryCity')

delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'StateId' and ObjectTableId = (select id from ObjectTables where Name = 'CountryCity'))


delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'Participant'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'Participant'))
delete ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'Participant')
delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'Participant'))

delete ScreenFields WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'VatType'))
delete QueryColumns WHERE ObjectFieldId = (select id from ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'VatType'))
--delete ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'VatType')
--delete TextCodes where Id =(select FullNameTextCodeId from ObjectFields WHERE FieldName = 'EnglishName' and ObjectTableId = (select id from ObjectTables where Name = 'VatType'))
delete TextCodes where Code = 'CountryCity.StateHelpText'
delete TextCodes where Code = 'Participant.English NameHelpText'
delete TextCodes where Code = 'Participant.CH.EnglishNameListLable'

--delete TextCodes where Code = 'VatType.NameHelpText'
--delete TextCodes where Code = 'VatType.CH.EnglishNameListLable'




