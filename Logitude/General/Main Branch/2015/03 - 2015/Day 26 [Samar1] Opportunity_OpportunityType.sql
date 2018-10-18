alter table Opportunities add [OpportunityTypeId] [varchar] (15)
go

alter table Opportunities drop constraint Opportunity_OpportunityType
go


declare @keyString as varchar(200) 
set @keyString = (SELECT A.CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS A, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B
WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND A.CONSTRAINT_NAME = B.CONSTRAINT_NAME AND A.TABLE_NAME = 'OpportunityTypes')

print @keyString

alter table OpportunityTypes drop constraint [@keyString]
go

declare @keyName as varchar(100)

set @keyName= (SELECT top (1) KU.CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
INNER JOIN
INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
and ku.table_name='OpportunityTypes'
ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION);

print @keyName