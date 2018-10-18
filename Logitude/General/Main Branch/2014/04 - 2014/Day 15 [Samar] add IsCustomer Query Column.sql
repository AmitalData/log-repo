delete from QueryColumns where QueryId = (select Id from Queries where code = 'By Last Shipment')
delete from QueryColumns where QueryId = (select Id from Queries where code = 'Customers')