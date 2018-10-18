
declare @Tenant as int
declare @BranchId as varchar(15)
set @Tenant = 1
--set @BranchId = '1-19'	-- Office 2

declare @ARInvoice_NotReady as int
declare @ARInvoice_MarkedBlocked as int
declare @ARInvoice_ErrorInTransfer as int

declare @APInvoice_NotReady as int
declare @APInvoice_MarkedBlocked as int
declare @APInvoice_ErrorInTransfer as int

declare @ARPayment_NotReady as int
declare @ARPayment_MarkedBlocked as int


if (@BranchId is null)
BEGIN
set @ARInvoice_NotReady = (select count(*)
from ARInvoices
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
and IsConstituentInvoice = 0
and TransferStatusCode = 'NR')
-------------------------------------------------
set @ARInvoice_MarkedBlocked = (select count(*)
from ARInvoices
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
and IsConstituentInvoice = 0
and IsCancelled = 0
and TransferStatusCode = 'BL')
-------------------------------------------------
set @ARInvoice_ErrorInTransfer = (select count(*)
from ARInvoices
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
and IsConstituentInvoice = 0
and IsCancelled = 0
and TransferStatusCode = 'ET')
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @APInvoice_NotReady = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode != 'WA'
and StatusCode != 'VD'
and TransferStatusCode = 'NR')
-------------------------------------------------
set @APInvoice_MarkedBlocked = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode != 'WA'
and StatusCode != 'VD'
and TransferStatusCode = 'BL')
-------------------------------------------------
set @APInvoice_ErrorInTransfer = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode != 'WA'
and StatusCode != 'VD'
and TransferStatusCode = 'ET')
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @ARPayment_NotReady = (select count(*)
from ARPayments
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and TransferStatusCode = 'NR')
-------------------------------------------------
set @ARPayment_MarkedBlocked = (select count(*)
from ARPayments
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and TransferStatusCode = 'BL')
END

else
BEGIN
set @ARInvoice_NotReady = (select count(*)
from ARInvoices
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
and IsConstituentInvoice = 0
and TransferStatusCode = 'NR'
and BranchId = @BranchId)
-------------------------------------------------
set @ARInvoice_MarkedBlocked = (select count(*)
from ARInvoices
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
and IsConstituentInvoice = 0
and IsCancelled = 0
and TransferStatusCode = 'BL'
and BranchId = @BranchId)
-------------------------------------------------
set @ARInvoice_ErrorInTransfer = (select count(*)
from ARInvoices
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
and IsConstituentInvoice = 0
and IsCancelled = 0
and TransferStatusCode = 'ET'
and BranchId = @BranchId)
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @APInvoice_NotReady = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode != 'WA'
and StatusCode != 'VD'
and TransferStatusCode = 'NR'
and BranchId = @BranchId)
-------------------------------------------------
set @APInvoice_MarkedBlocked = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode != 'WA'
and StatusCode != 'VD'
and TransferStatusCode = 'BL'
and BranchId = @BranchId)
-------------------------------------------------
set @APInvoice_ErrorInTransfer = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode != 'WA'
and StatusCode != 'VD'
and TransferStatusCode = 'ET'
and BranchId = @BranchId)
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @ARPayment_NotReady = (select count(*)
from ARPayments
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and TransferStatusCode = 'NR')
-------------------------------------------------
set @ARPayment_MarkedBlocked = (select count(*)
from ARPayments
where Tenant = @Tenant
and StatusCode != 'DR'
and StatusCode != 'VD'
and TransferStatusCode = 'BL'
and BranchId = @BranchId)
END

print '[AR/ Invoices]'
print 'Not Ready: ' + convert(varchar,@ARInvoice_NotReady)
print 'Marked Blocked: ' + convert(varchar,@ARInvoice_MarkedBlocked)
print 'Error In Transfer: ' + convert(varchar,@ARInvoice_ErrorInTransfer)
print ''

print '[AP/ Invoices]'
print 'Not Ready: ' + convert(varchar,@APInvoice_NotReady)
print 'Marked Blocked: ' + convert(varchar,@APInvoice_MarkedBlocked)
print 'Error In Transfer: ' + convert(varchar,@APInvoice_ErrorInTransfer)
print ''

print '[AR/ Payments]'
print 'Not Ready: ' + convert(varchar,@ARPayment_NotReady)
print 'Marked Blocked: ' + convert(varchar,@ARPayment_MarkedBlocked)
print ''