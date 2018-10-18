
-- Logitude_Main_db

-- Creating table 'ARInvoiceTransferStatus'
CREATE TABLE [dbo].[ARInvoiceTransferStatus] (
    [Code]varchar(2)   NOT NULL,
    [Name]varchar(20)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);
GO

-- Creating primary key on [Code] in table 'ARInvoiceTransferStatus'
ALTER TABLE [dbo].[ARInvoiceTransferStatus]
ADD CONSTRAINT [PK_ARInvoiceTransferStatus]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

Insert	into  ARInvoiceTransferStatus(Code, Name,SearchFields) values ('RD', 'Ready', 'RD,Ready')
go
Insert	into  ARInvoiceTransferStatus(Code, Name,SearchFields) values ('NR', 'Not Ready', 'NR,Not Ready')
go
Insert	into  ARInvoiceTransferStatus(Code, Name,SearchFields) values ('BL', 'Blocked', 'BL,Blocked')
go
Insert	into  ARInvoiceTransferStatus(Code, Name,SearchFields) values ('TR', 'Transferred', 'TR,Transferred')
go

ALTER TABLE ARInvoices ADD TransferStatusCode varchar(2) Not NULL default 'NR'
go	

-- Creating foreign key on [TransferStatusCode] in table 'ARInvoices'
ALTER TABLE [dbo].[ARInvoices]
ADD CONSTRAINT [FK_InvoiceInvoiceTransferStatus]
    FOREIGN KEY ([TransferStatusCode])
    REFERENCES [dbo].[ARInvoiceTransferStatus]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO