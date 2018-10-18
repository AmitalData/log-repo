alter table contacts add UserType varchar(1) not null default 'R'
go
update contacts set usertype='S' where email like 'system@tenant%'
