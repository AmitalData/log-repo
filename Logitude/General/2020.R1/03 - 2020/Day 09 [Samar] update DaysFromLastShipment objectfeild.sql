

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'DaysFromLastShipment')

update ObjectFields set DisplayInList = 0 where FieldName = 'DaysFromLastShipment'