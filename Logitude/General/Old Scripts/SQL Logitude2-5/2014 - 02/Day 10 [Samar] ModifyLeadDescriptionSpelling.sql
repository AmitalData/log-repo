
delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'LeadDescreption' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'LeadDescreption' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'LeadDescreption' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))

delete from ObjectFields where FieldName = 'LeadDescreption'
delete from ObjectFields where ListTextCodeId = (select Id from TextCodes where Code = 'Customer.CH.LeadDescreptionListLable')

delete from TextCodes where Code like '%LeadDescreption%'
delete from TextCodes where Code = 'Customer.CH.LeadDescreptionListLable'