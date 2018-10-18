

alter table ARInvoices add IsTransferred bit not null default 0
alter table ARInvoices add DontTransfer bit not null default 0
alter table ARInvoices add ReadyForTransfer bit not null default 0
alter table ARInvoices add DebitAccount varchar(15) null