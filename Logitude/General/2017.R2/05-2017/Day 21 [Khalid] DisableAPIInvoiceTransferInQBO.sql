--> Mark the QBO interface parameter : AllowAPInvoicesTransfer As Disabled
update AccountingSystems set AllowAPInvoicesTransfer=0 where Code = 'QBO'
