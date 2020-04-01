
update ObjectTables set DescriptionTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectTables.DescriptionTextCodeId)

update ObjectTables set NewButtonTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectTables.NewButtonTextCodeId)
