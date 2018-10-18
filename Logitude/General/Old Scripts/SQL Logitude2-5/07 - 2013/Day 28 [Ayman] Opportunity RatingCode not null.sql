

update Opportunities set RatingCode = 'W' where RatingCode is null
go

alter table Opportunities alter column RatingCode varchar(1) not null
go