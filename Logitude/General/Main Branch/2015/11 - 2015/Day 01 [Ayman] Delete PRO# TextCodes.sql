delete from Translations where TextCodeId in (select Id from TextCodes where Code like '%.PRO')
go
delete from TextCodes where Code like '%.PRO'
go