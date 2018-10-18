alter table shipments add [AsAgreed] bit NULL
go

update shipments set AsAgreed = 0
update shipments set AsAgreed = 1 where AWBPrintSpecificationCode = 'AAD'

