
Select * FROM ObjectFields WHERE (FieldName = 'PhoneNumber') OR (FieldName = 'FaxNumber') AND (ObjectTableId = (SELECT Id  FROM ObjectTables WHERE (Name = 'Customer')))

Update ObjectFields set MaxLength = 40 WHERE (FieldName = 'PhoneNumber') OR (FieldName = 'FaxNumber') AND (ObjectTableId = (SELECT Id  FROM ObjectTables WHERE (Name = 'Customer')))
