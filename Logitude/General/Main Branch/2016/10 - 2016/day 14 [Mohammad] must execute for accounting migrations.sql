
--declare @keyName varchar(max)

--set @keyName= (SELECT top (1) KU.CONSTRAINT_NAME
--FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
--INNER JOIN
--INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
--ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
--TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
--and ku.table_name='BankAccounts'
--ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION);
--select @keyName
--alter table bankaccounts drop constraint dbo.BankAccounts.PK_dbo.BankAccounts

ALTER TABLE bankaccounts
DROP CONSTRAINT [PK_dbo.BankAccounts]
alter table bankaccounts alter column Id varchar(15) not null
ALTER TABLE bankaccounts
ADD Constraint [PK_dbo.BankAccounts] PRIMARY KEY (Id)




--EXEC sp_fkeys 'Logitude2-5_Main.dbo.BankAccounts'

--Select
--S.[name] as 'Dependent_Tables'
--From
--sys.objects S inner join sys.sysreferences R
--on S.object_id = R.rkeyid
--Where
--S.[type] = 'U' AND
--R.fkeyid = OBJECT_ID('dbo.BankAccounts')