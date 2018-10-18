
-- Run this after update database migration

if not exists (select * from APInvoiceTransferStatus where Code = 'RD')
begin
	Insert	into  APInvoiceTransferStatus(Code, Name, SearchFields) values ('RD', 'Ready', 'RD,Ready')
end

if not exists (select * from APInvoiceTransferStatus where Code = 'NR')
begin
	Insert	into  APInvoiceTransferStatus(Code, Name,SearchFields) values ('NR', 'Not Ready', 'NR,Not Ready')
end

if not exists (select * from APInvoiceTransferStatus where Code = 'BL')
begin
	Insert	into  APInvoiceTransferStatus(Code, Name,SearchFields) values ('BL', 'Blocked', 'BL,Blocked')
end

if not exists (select * from APInvoiceTransferStatus where Code = 'TR')
begin
Insert	into  APInvoiceTransferStatus(Code, Name,SearchFields) values ('TR', 'Transferred', 'TR,Transferred')
end

IF COLUMNPROPERTY( OBJECT_ID(N'APInvoices'), 'TransferStatusCode', 'ColumnId') IS NULL
begin

	ALTER TABLE APInvoices ADD TransferStatusCode varchar(2) not null CONSTRAINT DF_APInvoices_Fixed_Name default 'NR';

	ALTER TABLE APInvoices ADD CONSTRAINT FK_APInvoiceAPInvoiceTransferStatus
		FOREIGN KEY (TransferStatusCode)
		REFERENCES APInvoiceTransferStatus(Code)
		ON DELETE NO ACTION ON UPDATE NO ACTION;

	CREATE INDEX [IX_FK_APInvoiceAPInvoiceTransferStatus] ON APInvoices (TransferStatusCode);

	ALTER TABLE APInvoices DROP DF_APInvoices_Fixed_Name
end
GO