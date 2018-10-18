
begin transaction
begin

alter table ActivityStatus alter column Name varchar(40) not null

if not exists (select * from ActivityStatus where Code = 'N')
insert into ActivityStatus(Code, Name,SearchFields) values ('N','Not Started','N,Not Started')
else update ActivityStatus set Name = 'Not Started', SearchFields = 'N,Not Started' where Code = 'N'

if not exists (select * from ActivityStatus where Code = 'I')
insert into ActivityStatus(Code, Name,SearchFields) values ('I','In Progress','I,In Progress')
else update ActivityStatus set Name = 'In Progress', SearchFields = 'I,In Progress' where Code = 'I'

if not exists (select * from ActivityStatus where Code = 'C')
insert into ActivityStatus(Code, Name,SearchFields) values ('C','Completed','C,Completed')
else update ActivityStatus set Name = 'Completed', SearchFields = 'C,Completed' where Code = 'C'

if not exists (select * from ActivityStatus where Code = 'D')
insert into ActivityStatus(Code, Name,SearchFields) values ('D','Deferred','D,Deferred')
else update ActivityStatus set Name = 'Deferred', SearchFields = 'D,Deferred' where Code = 'D'

if not exists (select * from ActivityStatus where Code = 'X')
insert into ActivityStatus(Code, Name,SearchFields) values ('X','Canceled','X,Canceled')
else update ActivityStatus set Name = 'Canceled', SearchFields = 'X,Canceled' where Code = 'X'

if not exists (select * from ActivityStatus where Code = 'W')
insert into ActivityStatus(Code, Name,SearchFields) values ('W','Waiting on Someone else','W,Waiting on Someone else')
else update ActivityStatus set Name = 'Waiting on Someone else', SearchFields = 'W,Waiting on Someone else' where Code = 'W'

update Activities set ActivityStatusCode = 'N' where ActivityStatusCode = 'O'

delete from ActivityStatus where Code = 'O'

END
commit transaction