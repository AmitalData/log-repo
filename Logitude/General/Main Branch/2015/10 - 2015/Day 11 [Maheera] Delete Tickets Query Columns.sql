delete from QueryColumns where QueryId= (select Id from Queries where ObjectTableId=(select Id from ObjectTables where Name = 'Ticket') and Code ='Open Tickets')
delete from Queries where ObjectTableId=(select Id from ObjectTables where Name = 'Ticket') and Code ='Open Tickets'

delete from QueryColumns where QueryId= (select Id from Queries where ObjectTableId=(select Id from ObjectTables where Name = 'Ticket') and Code ='Cancelled Tickets')
delete from AdvancedQueryFilters where QueryId =(select Id from Queries where ObjectTableId=(select Id from ObjectTables where Name = 'Ticket') and Code ='Cancelled Tickets') 
delete from Queries where ObjectTableId=(select Id from ObjectTables where Name = 'Ticket') and Code ='Cancelled Tickets'

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where ObjectTableId=(select Id from ObjectTables where Name = 'Ticket') and FieldName = 'CompanyRankCode')

delete from ObjectFields where ObjectTableId=(select Id from ObjectTables where Name = 'Ticket')and FieldName = 'CompanyRankCode'
