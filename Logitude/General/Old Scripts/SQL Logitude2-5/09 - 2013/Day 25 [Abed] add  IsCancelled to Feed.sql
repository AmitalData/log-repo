

begin transaction
begin



alter table [Feeds] add [IsCancelled] bit not null default 0
END
commit transaction




