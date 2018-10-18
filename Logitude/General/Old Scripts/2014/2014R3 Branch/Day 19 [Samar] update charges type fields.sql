update ObjectFields
set MaxLength = 4
where FieldName = 'MeasurementCode' and ObjectTableId = (select Id from ObjectTables where Name = 'ChargesType')

update ObjectFields
set MaxLength = 40
where FieldName = 'MeasurementShortName' and ObjectTableId = (select Id from ObjectTables where Name = 'ChargesType')