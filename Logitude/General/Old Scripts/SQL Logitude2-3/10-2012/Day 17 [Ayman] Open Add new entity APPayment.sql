
select * from Queries where ObjectTableId = (Select Id from ObjectTables where Name = 'APPayment')

update Queries set IsAddNewEntityEnabled = 1 where ObjectTableId = (Select Id from ObjectTables where Name = 'APPayment')