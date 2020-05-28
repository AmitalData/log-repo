
update ObjectFields set FullNameTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.FullNameTextCodeId)

update ObjectFields set HelpTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.HelpTextCodeId)

update ObjectFields set ListTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.ListTextCodeId)

update ObjectFields set ShortNameTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectFields.ShortNameTextCodeId)
