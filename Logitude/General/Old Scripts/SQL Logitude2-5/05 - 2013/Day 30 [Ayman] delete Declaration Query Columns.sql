
delete from QueryColumns where QueryId = (Select Id from Queries where Code = 'Declarations')