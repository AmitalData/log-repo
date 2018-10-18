
-- do not run online - already exist

alter table Contacts alter column EnglishName varchar(60)
go

alter table Contacts alter column LocalName nvarchar(100)
go