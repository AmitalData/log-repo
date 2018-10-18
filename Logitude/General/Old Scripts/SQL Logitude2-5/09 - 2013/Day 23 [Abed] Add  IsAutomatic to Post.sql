

begin transaction
begin



alter table [Posts] add [IsAutomatic] bit not null default 0
END
commit transaction




