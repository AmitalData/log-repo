

-- Execute this Script then Update CRM

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Opportunity.HeaderScreen')
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))
go

delete from ObjectFields where FieldName = 'Value' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')
go

delete from TextCodes where Code = 'Opportunity.F.Value'
delete from TextCodes where Code = 'Opportunity.ValueHelpText'
delete from TextCodes where Code = 'Opportunity.CH.ValueListLable'

