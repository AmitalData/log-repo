
declare @Tenant as int
declare @BranchId as varchar(15)
set @Tenant = 1
--set @BranchId = '1-19'	-- Office 2

declare @Invoice_Draft as int
declare @Invoice_OpenConstituent as int
declare @Invoice_Unpaid as int
declare @Invoice_All as int
declare @Payment_Draft as int
declare @Payment_Open as int
declare @Payment_All as int

if (@BranchId is null)
BEGIN
set @Invoice_Draft = (select count(*)
from ARInvoices
where Tenant = @Tenant
and IsClosed = 0
and IsCancelled = 0
and StatusCode != 'LL'
and StatusCode = 'DR')
-------------------------------------------------
set @Invoice_OpenConstituent = (select count(*)
from ARInvoices
where Tenant = @Tenant
and IsClosed = 0
and IsCancelled = 0
and StatusCode != 'LL'
and StatusCode != 'VD'
and IsConstituentInvoice = 1
and ConsolidationInvoiceId is null)
-------------------------------------------------
set @Invoice_Unpaid = (select count(*)
from ARInvoices
where Tenant = @Tenant
and IsClosed = 0
and IsCancelled = 0
and StatusCode != 'LL'
and ((StatusCode != 'DR' and StatusCode != 'VD' and IsAutoCredit = 0) OR (IsConstituentInvoice = 1 and ConsolidationInvoiceId is not null))
)
-------------------------------------------------
set @Invoice_All = (select count(*)
from ARInvoices
where Tenant = @Tenant)
-------------------------------------------------
set @Payment_Draft = (select count(*)
from ARPayments
where Tenant = @Tenant
and StatusCode = 'DR')
-------------------------------------------------
set @Payment_Open = (select count(*)
from ARPayments
where Tenant = @Tenant
and IsClosed = 0
and StatusCode != 'DR'
and StatusCode != 'VD')
-------------------------------------------------
set @Payment_All = (select count(*)
from ARPayments
where Tenant = @Tenant)
END

else
BEGIN
set @Invoice_Draft = (select count(*)
from ARInvoices
where Tenant = @Tenant
and IsClosed = 0
and IsCancelled = 0
and StatusCode != 'LL'
and StatusCode = 'DR'
and BranchId = @BranchId)
-------------------------------------------------
set @Invoice_OpenConstituent = (select count(*)
from ARInvoices
where Tenant = @Tenant
and IsClosed = 0
and IsCancelled = 0
and StatusCode != 'LL'
and StatusCode != 'VD'
and IsConstituentInvoice = 1
and ConsolidationInvoiceId is null
and BranchId = @BranchId)
-------------------------------------------------
set @Invoice_Unpaid = (select count(*)
from ARInvoices
where Tenant = @Tenant
and IsClosed = 0
and IsCancelled = 0
and StatusCode != 'LL'
and ((StatusCode != 'DR' and StatusCode != 'VD' and IsAutoCredit = 0) OR (IsConstituentInvoice = 1 and ConsolidationInvoiceId is not null))
and BranchId = @BranchId)
-------------------------------------------------
set @Invoice_All = (select count(*)
from ARInvoices
where Tenant = @Tenant
and BranchId = @BranchId)
-------------------------------------------------
set @Payment_Draft = (select count(*)
from ARPayments
where Tenant = @Tenant
and StatusCode = 'DR'
and BranchId = @BranchId)
-------------------------------------------------
set @Payment_Open = (select count(*)
from ARPayments
where Tenant = @Tenant
and IsClosed = 0
and StatusCode != 'DR'
and StatusCode != 'VD'
and BranchId = @BranchId)
-------------------------------------------------
set @Payment_All = (select count(*)
from ARPayments
where Tenant = @Tenant
and BranchId = @BranchId)
END

print '[AR Invoices]'
print 'Draft: ' + convert(varchar,@Invoice_Draft)
print 'Open Constituent: ' + convert(varchar,@Invoice_OpenConstituent)
print 'Unpaid: ' + convert(varchar,@Invoice_Unpaid)
print 'All: ' + convert(varchar,@Invoice_All)
print ''

print '[AR Payments]'
print 'Draft: ' + convert(varchar,@Payment_Draft)
print 'Open: ' + convert(varchar,@Payment_Open)
print 'All: ' + convert(varchar,@Payment_All)
print ''
