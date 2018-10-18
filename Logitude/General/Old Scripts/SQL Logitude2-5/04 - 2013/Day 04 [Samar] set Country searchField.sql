
Update Countries
set SearchFields = 
isnull(Code,'') + ',' +
isnull(EnglishName,'') + ',' +
isnull(LocalName,'')