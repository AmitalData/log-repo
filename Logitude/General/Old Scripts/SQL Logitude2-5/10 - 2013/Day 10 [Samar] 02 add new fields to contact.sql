
alter table Contacts add [BirthDayOfYear] int null
go

begin transaction
begin

alter table Contacts add [DoneDate] DateTime null

update Contacts set BirthDayOfYear = DATEPART(dayofyear, Birthday) where Birthday is not null

END
commit transaction
