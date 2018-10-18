
select * from Queries where Code = 'Masters'
select * from QueryColumns where QueryId = (select Id from Queries where Code = 'Masters')

update QueryColumns 
set ObjectFieldId = (select Id from ObjectFields where FieldName = 'AgentName' AND ObjectTableId = (select Id from ObjectTables where Name = 'Shipment'))
where IndexOrder = '5' and QueryId = (select Id from Queries where Code = 'Masters') 
