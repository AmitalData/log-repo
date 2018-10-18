
-- already exsist 

alter table ChargeTypeAccountings add PayableDebitAccount varchar(15) null
go

alter table ChargeTypeAccountings drop column PayableCreditAccount
go