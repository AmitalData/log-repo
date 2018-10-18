

alter table ChargesTypes add AccountingVATSplit bit not null default 0
go

alter table ChargesTypes add ReceivableCreditAccount varchar(15) null
go

alter table ChargesTypes add PayableDebitAccount varchar(15) null
go

update ChargesTypes set ReceivableCreditAccount = CreditAccount
go

alter table ChargesTypes drop column CreditAccount
go