delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'Master') and QueryId = (select Id from Queries where Code = 'CreatedBookings')
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'Master') and QueryId = (select Id from Queries where Code = 'WatingForResponse')
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'Master') and QueryId = (select Id from Queries where Code = 'ConfirmedBookings')
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'Master') and QueryId = (select Id from Queries where Code = 'RejectedBookings')
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'Master') and QueryId = (select Id from Queries where Code = 'InProgressBookings')
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'Master') and QueryId = (select Id from Queries where Code = 'AllBookings')
delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'Master') and QueryId = (select Id from Queries where Code = 'CancelledBookings')

