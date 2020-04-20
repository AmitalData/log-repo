

update Tips set ShortTextCodeCode = (select TextCodes.Code from TextCodes where id = Tips.ShortTextCode)
