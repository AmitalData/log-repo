delete from objecttabletabs where code in ('ACGC','ACEV');
 delete from TextCodes where code in ('AccountingPartner.TH.General','AccountingPartner.TH.Events');
 
 insert into FieldDataTypes (Code, Name, SearchFields) values ('Time', 'Time', 'Time,Time');
insert into FieldDataTypes (Code, Name, SearchFields) values ('BigInteger', 'BigInteger', 'BigInteger,BigInteger');



----delete from querycolumns where querycode in (select UniqueCode from queries group by UniqueCode having count(*) > 1 )
----delete from advancedqueryfilters where querycode in (select UniqueCode from queries group by UniqueCode having count(*) > 1 )
----delete from queries where UniqueCode in (select UniqueCode from queries group by UniqueCode having count(*) > 1 )