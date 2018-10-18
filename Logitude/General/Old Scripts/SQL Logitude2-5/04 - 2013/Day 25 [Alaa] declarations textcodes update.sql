----***************************
-- DO NOT RUN ONLINE ----

UPDATE TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'Declaration.','Customs.Declaration.') 
WHERE ObjectTableId= (select id from ObjectTables where Name='Customs.Declaration')
go


