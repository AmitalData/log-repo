
delete from QueryColumns where QueryId = (Select Id from Queries where Code = 'Expected Departures')
go

delete from QueryColumns where QueryId = (Select Id from Queries where Code = 'Airlines Updates')
go
