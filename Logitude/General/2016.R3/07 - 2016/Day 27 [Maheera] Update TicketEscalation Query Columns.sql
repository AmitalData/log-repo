delete FROM QueryColumns where QueryId = (SELECT Id FROM Queries WHERE ObjectTableId = (SELECT Id FROM ObjectTables where name= 'ticketescalation'))
delete FROM Queries WHERE ObjectTableId = (SELECT Id FROM ObjectTables where name= 'ticketescalation')
