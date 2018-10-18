
begin transaction
begin

delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Upcoming Events')
delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'Upcoming Events')
delete from Queries where Code = 'Upcoming Events'
delete from TextCodes where code = 'Contact.Q.UpcomingEvents'

delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'UPCOMINGEVENTSCONTACTS')
delete from Features where Code = 'UPCOMINGEVENTSCONTACTS'
delete from TextCodes where code = 'Contact.Features.UpcomingEvents'
END
commit transaction

