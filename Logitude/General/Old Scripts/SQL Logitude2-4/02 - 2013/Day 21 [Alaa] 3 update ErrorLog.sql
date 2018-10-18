
update objecttables set name='ErrorLog'
where name='ErrorLogs'

update objecttables set DBTableName='ErrorLogs'
where name='ErrorLog'


UPDATE TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'ErrorLogs','ErrorLog') 
WHERE ObjectTableId= (select id from ObjectTables where Name='ErrorLog')

UPDATE TextCodes set Code=
REPLACE(SUBSTRING(Code,1,DATALENGTH(Code)),'SearchField','SearchFields') 
where ObjectTableId= (select id from ObjectTables where Name='ErrorLog')