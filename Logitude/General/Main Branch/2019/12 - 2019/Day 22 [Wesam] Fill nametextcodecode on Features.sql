

update Features set NameTextCodeCode = (select TextCodes.Code from TextCodes where id = Features.NameTextCodeId)