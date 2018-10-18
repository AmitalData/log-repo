
delete from QueryColumns where QueryId = (select Id from Queries where Code = 'Customer.MyCustomers')