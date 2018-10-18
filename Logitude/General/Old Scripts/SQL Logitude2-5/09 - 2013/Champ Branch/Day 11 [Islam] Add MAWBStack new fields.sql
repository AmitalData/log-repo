

begin transaction
begin

alter table mawbstacks add AssignedToId varchar(15) null
alter table mawbstacks add constraint [MAWBStack_AssignedToCard] foreign key ([AssignedToId]) references [dbo].[Cards]([Id]);

alter table mawbstacks add IsUsed bit not null default '0'


END
commit transaction
