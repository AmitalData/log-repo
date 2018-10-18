-- update search fields for old created data in categories tables 
update Category1 set SearchFields = LocalName + ',' + EnglishName
update Category2 set SearchFields = LocalName + ',' + EnglishName
update Category3 set SearchFields = LocalName + ',' + EnglishName
update Category4 set SearchFields = LocalName + ',' + EnglishName
update Category5 set SearchFields = LocalName + ',' + EnglishName
