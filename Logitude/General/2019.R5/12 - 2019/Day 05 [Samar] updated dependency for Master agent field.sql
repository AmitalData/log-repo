

--update ObjectFields set DependencyFilter1Type = Null, DependencyFilter1Value = NULL
--where FieldName= 'AgentId' and ObjectTableId = (select Id from ObjectTables where Name = 'Master')


--update ObjectFields set DependencyFilter1Type = Null, DependencyFilter1Value = NULL
--where FieldName= 'AgentId' and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')
