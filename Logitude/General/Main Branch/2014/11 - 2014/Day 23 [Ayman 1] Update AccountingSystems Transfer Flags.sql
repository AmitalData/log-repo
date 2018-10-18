
-- None
update AccountingSystems set
AllowARInvoicesTransfer = 0,
AllowAPInvoicesTransfer = 0
where Code = 'NO'
GO

-- Rivheet
update AccountingSystems set
AllowARInvoicesTransfer = 1,
AllowAPInvoicesTransfer = 0
where Code = 'RH'
GO

-- Hashavshevet
update AccountingSystems set
AllowARInvoicesTransfer = 1,
AllowAPInvoicesTransfer = 0
where Code = 'HV'
GO

-- Quick Books
update AccountingSystems set
AllowARInvoicesTransfer = 1,
AllowAPInvoicesTransfer = 0
where Code = 'QB'
GO

-- Logitude Generic Interface
update AccountingSystems set
AllowARInvoicesTransfer = 1,
AllowAPInvoicesTransfer = 1
where Code = 'GI'





