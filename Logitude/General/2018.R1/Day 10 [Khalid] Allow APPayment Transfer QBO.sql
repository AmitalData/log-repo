update AccountingSettings set IsAPPaymentsTransferEnabled=1 where AccountingSystemCode in ('QBO','QBOG')
update AccountingSystems set AllowAPPaymentsTransfer=1 where Code in ('QBO','QBOG')