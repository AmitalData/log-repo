
alter table Customs.countries alter column EnglishName varchar(60) null
go

alter table Customs.countries alter column LocalName nvarchar(60) null
go

alter table Customs.subcountries alter column EnglishName varchar(60) null
go

alter table Customs.subcountries alter column LocalName nvarchar(60) null
go

alter table Customs.subcountries alter column searchfields nvarchar(1000) null
go

alter table Customs.CountryGroups alter column EnglishName varchar(60) null
go

alter table Customs.CountryGroups alter column LocalName nvarchar(60) null
go

alter table Customs.CountryGroups alter column searchfields nvarchar(1000) null
go