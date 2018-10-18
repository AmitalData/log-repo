sp_rename 'ARInvoices.Reference1', 'PartnerRefrenceNo', 'COLUMN';
go
sp_rename 'ARInvoices.Reference2', 'HouseNumber', 'COLUMN';
go
sp_rename 'ARInvoices.Reference3', 'MasterNumber', 'COLUMN';
go
alter table ARInvoices add Description nvarchar(250) null
go