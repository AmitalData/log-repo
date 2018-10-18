
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Participant.HeaderScreen')
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Participant.GeneralTabScreen')
delete from Screens where Code = 'Participant.GeneralTabScreen'

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'AirlineTenant' and ObjectTableId = (select Id from ObjectTables where Name = 'Participant'))

delete from ObjectFields where FieldName = 'AirlineTenant' and ObjectTableId = (select Id from ObjectTables where Name = 'Participant')

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Participant') and Code like '%AirlineTenant%'

