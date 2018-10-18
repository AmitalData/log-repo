

alter table Activities add IsOpen bit not null default 0
go 

begin transaction
begin

update Activities set IsOpen = 1 where ActivityStatusCode = 'N' OR ActivityStatusCode = 'W' OR ActivityStatusCode = 'I' OR ActivityStatusCode = 'D'

END
commit transaction