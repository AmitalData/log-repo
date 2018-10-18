delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'SentBookings')
delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'PartiallyConfirmed')

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'SentBookings')
delete from QueryColumns where QueryId = (select Id from Queries where Code = 'PartiallyConfirmed')

delete from Queries where Code = 'SentBookings'
delete from Queries where Code = 'PartiallyConfirmed'

delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'Booking.Q.SentBookings')
delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'Booking.Q.PartiallyConfirmedBookings')

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'Booking.Q.SentBookings')
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'Booking.Q.PartiallyConfirmedBookings')

delete from Features where Code = 'Booking.Q.SentBookings'
delete from Features where Code = 'Booking.Q.PartiallyConfirmedBookings'

delete from ObjectFields where FieldName = 'PartiallyConfirmedBookings'
delete from ObjectFields where FieldName = 'SentBookings'

delete from TextCodes where Code like '%SentBooking%s'
delete from TextCodes where Code like '%PartiallyConfirmedBookings&'

