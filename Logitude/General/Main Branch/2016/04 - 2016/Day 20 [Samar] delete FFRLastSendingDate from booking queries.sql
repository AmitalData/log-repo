
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'FFRLastSendingDate')

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'AllBookings')