
declare @Tenant as int
declare @Email as varchar(100)
declare @todayDate as date
declare @lastWeekDate as date
declare @LoggedUserId as varchar(15)
declare @StatusId_Arrived as varchar(15)
declare @StatusId_Delivered as varchar(15)

set @Tenant = 1
set @Email = 'angular@fnarsoft.com'
set @todayDate = (SELECT DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0))
set @lastWeekDate = (SELECT DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), -7))
set @LoggedUserId = (select top 1 Id from Contacts where Tenant = @Tenant and Email = @Email)
set @StatusId_Arrived = (select Id from EntityStatus where Tenant = @Tenant and Code = 'SARR')
set @StatusId_Delivered = (select Id from EntityStatus where Tenant = @Tenant and Code = 'SDLD')

if (@LoggedUserId is null)
begin
	print 'This Email does not exists'
end

else
BEGIN
declare @OperationalOpen_Shipments as int
declare @OperationalOpen_ImportShipments as int
declare @OperationalOpen_Masters as int
declare @AccountingOpen_OpenReceivablesShipments as int
declare @AccountingOpen_OpenPayablesMasters as int
declare @EAWB_ExpectedDeparture as int 
declare @EAWB_AirlineUpdates as int
declare @Others_AllFollowups as int
declare @Others_MyFollowups as int
declare @Others_AllShipments as int
declare @Others_AllMasters as int
declare @Others_CreditLimitBlocked as int
declare @Others_Cancelled as int
declare @FSR_FSRRequestLast7Days as int 

-------------------------------------------------
set @OperationalOpen_Shipments = (select count(*)
from Shipments
where Tenant = @Tenant
and IsCancelled = 0
and IsOperationalClosed = 0
and ShipmentLevelCode in ('D', 'H', 'A'))
-------------------------------------------------
set @OperationalOpen_ImportShipments = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and IsOperationalClosed = 0
and ShipmentLevelCode != 'C'
and (DirectionId = 'I' OR (DirectionId = 'C' AND NoFreightFile = 1)))
-------------------------------------------------
set @OperationalOpen_Masters = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and IsOperationalClosed = 0
and ShipmentLevelCode in ('D', 'C'))
-------------------------------------------------
set @AccountingOpen_OpenReceivablesShipments = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and IsAccountingClosed = 0
and ShipmentReceivableStatusCode = 'OPEN'
and ShipmentLevelCode in ('D', 'H'))
-------------------------------------------------
set @AccountingOpen_OpenPayablesMasters = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and IsAccountingClosed = 0
and ShipmentPayableStatusCode = 'OPEN'
and ShipmentLevelCode in ('D', 'C'))
-------------------------------------------------
set @EAWB_ExpectedDeparture = (select count(*)
from Shipments join ShipmentMasterDatas on Shipments.MasterShipmentDataId = ShipmentMasterDatas.Id
where
Shipments.Tenant = @Tenant
and Shipments.IsCancelled = 0
and Shipments.ShipmentLevelCode != 'H'
and Shipments.DirectionId = 'E'
and Shipments.TransportModeId = 'A'
and Shipments.StatusId != @StatusId_Arrived
and Shipments.StatusId != @StatusId_Delivered
and ShipmentMasterDatas.MainCarriageATD is null
and ShipmentMasterDatas.MainCarriageATA is null
and ShipmentMasterDatas.MainCarriageETD is not null
and ShipmentMasterDatas.MainCarriageETD > @lastWeekDate)
-------------------------------------------------
set @EAWB_AirlineUpdates = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and ShipmentLevelCode != 'H'
and Shipments.CarrierLastStatusDate >= @lastWeekDate)
-------------------------------------------------
set @Others_AllFollowups = (select count(*)
from FollowUps join Shipments on FollowUps.ShipmentId = Shipments.Id
where
FollowUps.Tenant = 1
and FollowUps.ShipmentId is not null
and Shipments.IsCancelled = 0)
-------------------------------------------------
set @Others_MyFollowups = (select count(*)
from FollowUps join Shipments on FollowUps.ShipmentId = Shipments.Id
where
FollowUps.Tenant = 1
and FollowUps.ShipmentId is not null
and Shipments.IsCancelled = 0
and FollowUps.OwnerUserId = @LoggedUserId)
-------------------------------------------------
set @Others_AllShipments = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and ShipmentLevelCode in ('D', 'H', 'A'))
-------------------------------------------------
set @Others_AllMasters = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and ShipmentLevelCode in ('D', 'C'))
-------------------------------------------------
set @Others_CreditLimitBlocked = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 0
and IsNewARInvoiceBlocked = 1)
-------------------------------------------------
set @Others_Cancelled = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 1)
-------------------------------------------------
set @FSR_FSRRequestLast7Days = (select count(*)
from Shipments
where
Tenant = @Tenant
and IsCancelled = 1
and ShipmentLevelCode != 'H'
and TransportModeId = 'A'
and LastFSRStatusRequestDate >= @lastWeekDate)
-------------------------------------------------

print '[Operational Open]'
print 'Shipments: ' + convert(varchar,@OperationalOpen_Shipments)
print 'Import Shipments: ' + convert(varchar,@OperationalOpen_ImportShipments)
print 'Masters: ' + convert(varchar,@OperationalOpen_Masters)
print ''
print '[Accounting Open]'
print 'Open Receivables Shipments: ' + convert(varchar,@AccountingOpen_OpenReceivablesShipments)
print 'Open Payables Masters: ' + convert(varchar,@AccountingOpen_OpenPayablesMasters)
print ''
print '[EAWB]'
print 'Expected Departure: ' + convert(varchar,@EAWB_ExpectedDeparture)
print 'Airline Updates: ' + convert(varchar,@EAWB_AirlineUpdates)
print ''
print '[Others]'
print 'All Followups: ' + convert(varchar,@Others_AllFollowups)
print 'My Followups: ' + convert(varchar,@Others_MyFollowups)
print 'All Shipments: ' + convert(varchar,@Others_AllShipments)
print 'All Masters: ' + convert(varchar,@Others_AllMasters)
print 'Credit Limit Blocked: ' + convert(varchar,@Others_CreditLimitBlocked)
print 'Cancelled: ' + convert(varchar,@Others_Cancelled)
print ''
print '[FSR]'
print 'FSR Request Last 7 Days: ' + convert(varchar,@FSR_FSRRequestLast7Days)
END