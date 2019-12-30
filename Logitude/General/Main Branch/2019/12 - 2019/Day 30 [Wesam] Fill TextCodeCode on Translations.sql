

update Translations set TextCodeCode = (select TextCodes.Code from TextCodes where id = Translations.TextCodeId)
