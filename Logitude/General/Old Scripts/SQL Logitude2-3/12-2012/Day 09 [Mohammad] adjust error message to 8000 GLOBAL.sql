--EXECUTE THIS SCRIPT ON GLOBAL DATABASE PLEASE

alter table analyzequeues alter column ErrorMessage varchar(8000) null
go