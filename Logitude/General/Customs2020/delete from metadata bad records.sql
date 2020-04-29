delete from objecttabletabs where code in ('ACGC','ACEV');
 delete from TextCodes where code in ('AccountingPartner.TH.General','AccountingPartner.TH.Events');
 
 insert into FieldDataTypes (Code, Name, SearchFields) values ('Time', 'Time', 'Time,Time');
insert into FieldDataTypes (Code, Name, SearchFields) values ('BigInteger', 'BigInteger', 'BigInteger,BigInteger');