
declare @Tenant as int
declare @BranchId as varchar(15)
set @Tenant = 1
--set @BranchId = '1-19'	-- Office 2

declare @Invoice_Draft as int
declare @Invoice_Unpaid as int
declare @Invoice_All as int
declare @Payment_Draft as int
declare @Payment_Open as int
declare @Payment_All as int

if (@BranchId is null)
BEGIN
set @Invoice_Draft = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode = 'WA')
-------------------------------------------------
set @Invoice_Unpaid = (select count(*)
from APInvoices
where Tenant = @Tenant
and IsClosed = 0
and StatusCode != 'WA'
and StatusCode != 'VD'
and StatusCode != 'LL')
-------------------------------------------------
set @Invoice_All = (select count(*)
from APInvoices
where Tenant = @Tenant)
-------------------------------------------------
set @Payment_Draft = (select count(*)
from APPayments
where Tenant = @Tenant
and StatusCode = 'DR')
-------------------------------------------------
set @Payment_Open = (select count(*)
from APPayments
where Tenant = @Tenant
and IsClosed = 0
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
)
-------------------------------------------------
set @Payment_All = (select count(*)
from APPayments
where Tenant = @Tenant)
END

else
BEGIN
set @Invoice_Draft = (select count(*)
from APInvoices
where Tenant = @Tenant
and StatusCode = 'WA'
and BranchId = @BranchId)
-------------------------------------------------
set @Invoice_Unpaid = (select count(*)
from APInvoices
where Tenant = @Tenant
and IsClosed = 0
and StatusCode != 'WA'
and StatusCode != 'VD'
and StatusCode != 'LL'
and BranchId = @BranchId)
-------------------------------------------------
set @Invoice_All = (select count(*)
from APInvoices
where Tenant = @Tenant
and BranchId = @BranchId)
-------------------------------------------------
set @Payment_Draft = (select count(*)
from APPayments
where Tenant = @Tenant
and StatusCode = 'DR'
and BranchId = @BranchId)
-------------------------------------------------
set @Payment_Open = (select count(*)
from APPayments
where Tenant = @Tenant
and IsClosed = 0
and StatusCode != 'DR'
and StatusCode != 'VD'
and StatusCode != 'LL'
and BranchId = @BranchId)
-------------------------------------------------
set @Payment_All = (select count(*)
from APPayments
where Tenant = @Tenant
and BranchId = @BranchId)
END

print '[AP Invoices]'
print 'Waiting for approval: ' + convert(varchar,@Invoice_Draft)
print 'Unpaid: ' + convert(varchar,@Invoice_Unpaid)
print 'All: ' + convert(varchar,@Invoice_All)
print ''

print '[AP Payments]'
print 'Draft: ' + convert(varchar,@Payment_Draft)
print 'Open: ' + convert(varchar,@Payment_Open)
print 'All: ' + convert(varchar,@Payment_All)
print ''