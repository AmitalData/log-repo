
delete from QueryColumns where QueryId = (select Id from Queries where Code = 'All Additional Services')
go

delete from Queries where Code = 'All Additional Services'
go