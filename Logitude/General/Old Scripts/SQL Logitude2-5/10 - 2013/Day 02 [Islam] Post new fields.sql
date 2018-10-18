
-- Run it line by line.
alter table [Posts] add [UpdateDate]  datetime NULL
go

update Posts set UpdateDate = CreateDate


alter table [Posts] add [NumberOfComments] int not null default 0

alter table posts add EntityDescription nvarchar(100) null

alter table feeds add IsCancelled bit not null default 0

