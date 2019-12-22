

update Queries set NameTextCodeCode = (select TextCodes.Code from TextCodes where id = Queries.NameTextCodeId)