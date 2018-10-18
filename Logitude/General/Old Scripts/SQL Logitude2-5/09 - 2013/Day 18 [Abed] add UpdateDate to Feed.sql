

begin transaction
begin

alter table [Feeds] add [UpdateDate]  datetime NULL

END
commit transaction