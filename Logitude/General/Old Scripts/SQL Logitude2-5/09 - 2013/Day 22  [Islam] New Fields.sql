begin transaction
begin

alter table shipments add NoFreightFile bit not null default(0)

alter table globalzones alter column code varchar(8) not null

alter table tenants add IsHybrid bit not null default(0)

END
commit transaction