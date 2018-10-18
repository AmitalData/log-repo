
IF OBJECT_ID('[dbo].[usp_GetShipmentsCounts]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_GetShipmentsCounts]
GO

Create PROCEDURE [dbo].[usp_GetShipmentsCounts]
(
	@Tenant int,
	@DirectionId varchar(2),
	@TransportModeId varchar(2),
	@LoggedEmail varchar(100),
	@HasETDFeature bit,
	@HasFollowupsFeature bit,

	@OperationalOpen_Shipments int OUTPUT,
	@OperationalOpen_ImportShipments int OUTPUT,
	@OperationalOpen_Masters int OUTPUT,
	@AccountingOpen_OpenReceivablesShipments int OUTPUT,
	@AccountingOpen_OpenPayablesMasters int OUTPUT,
	@EAWB_ExpectedDeparture int OUTPUT,
	@EAWB_AirlineUpdates int OUTPUT,
	@Others_AllFollowups int OUTPUT,
	@Others_MyFollowups int OUTPUT,
	@Others_CreditLimitBlocked int OUTPUT,
	@FSR_FSRRequestLast7Days int OUTPUT
)
AS

declare @todayDate as date
declare @lastWeekDate as date
declare @LoggedUserId as varchar(15)
declare @HasBranchesFilter as bit
declare @HasProductsFilter as bit
set @todayDate = (SELECT DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0))
set @lastWeekDate = (SELECT DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), -7))
set @LoggedUserId = (select top 1 Id from Contacts where Tenant = @Tenant and Email = @LoggedEmail)
set @HasBranchesFilter = 0
set @HasProductsFilter = 0

declare @MemoryTable table
(
  Id varchar(15) not null,
  DirectionId varchar(2) not null,
  TransportModeId varchar(2) not null,
  ShipmentLevelCode varchar(1) not null,
  IsOperationalClosed bit,
  IsAccountingClosed bit,
  CarrierLastStatusDate datetime,
  LastFSRStatusRequestDate datetime,  
  ShipmentPayableStatusCode varchar(5),
  ShipmentReceivableStatusCode varchar(5),  
  IsNewARInvoiceBlocked bit,  
  NoFreightFile bit,
  StatusId varchar(15),
  MasterShipmentDataId varchar(15)
)

if exists (select * from UserPermittedBranches where Tenant = @Tenant AND UserId = @LoggedUserId)
BEGIN
	set @HasBranchesFilter = 1
END

if exists (select * from UserPermittedProducts where Tenant = @Tenant AND UserId = @LoggedUserId)
BEGIN
	set @HasProductsFilter = 1
END

-- Select Shipments
BEGIN
	if (@HasBranchesFilter = 1 AND @HasProductsFilter = 1)
	BEGIN
		insert into @MemoryTable
		SELECT
		Id,
		DirectionId,
		TransportModeId,	
		ShipmentLevelCode,
		IsOperationalClosed,
		IsAccountingClosed,
		CarrierLastStatusDate,
		LastFSRStatusRequestDate,	
		ShipmentPayableStatusCode,
		ShipmentReceivableStatusCode,
		IsNewARInvoiceBlocked,
		NoFreightFile,
		StatusId,
		MasterShipmentDataId
		From Shipments
		Where IsCancelled = 0 AND Tenant = @Tenant
		AND BranchId in (select BranchId from UserPermittedBranches where Tenant = @Tenant AND UserId = @LoggedUserId)
		AND ProductCode in (select ProductTypeCode from UserPermittedProducts where Tenant = @Tenant AND UserId = @LoggedUserId)
	END

	else if (@HasBranchesFilter = 1)
	BEGIN
		insert into @MemoryTable
		SELECT
		Id,
		DirectionId,
		TransportModeId,	
		ShipmentLevelCode,
		IsOperationalClosed,
		IsAccountingClosed,
		CarrierLastStatusDate,
		LastFSRStatusRequestDate,	
		ShipmentPayableStatusCode,
		ShipmentReceivableStatusCode,
		IsNewARInvoiceBlocked,
		NoFreightFile,
		StatusId,
		MasterShipmentDataId
		From Shipments
		Where IsCancelled = 0 AND Tenant = @Tenant
		AND BranchId in (select BranchId from UserPermittedBranches where Tenant = @Tenant AND UserId = @LoggedUserId)
	END

	else if (@HasProductsFilter = 1)
	BEGIN
		insert into @MemoryTable
		SELECT
		Id,
		DirectionId,
		TransportModeId,	
		ShipmentLevelCode,
		IsOperationalClosed,
		IsAccountingClosed,
		CarrierLastStatusDate,
		LastFSRStatusRequestDate,	
		ShipmentPayableStatusCode,
		ShipmentReceivableStatusCode,
		IsNewARInvoiceBlocked,
		NoFreightFile,
		StatusId,
		MasterShipmentDataId
		From Shipments
		Where IsCancelled = 0 AND Tenant = @Tenant
		AND ProductCode in (select ProductTypeCode from UserPermittedProducts where Tenant = @Tenant AND UserId = @LoggedUserId)
	END

	else
	BEGIN
		insert into @MemoryTable
		SELECT
		Id,
		DirectionId,
		TransportModeId,	
		ShipmentLevelCode,
		IsOperationalClosed,
		IsAccountingClosed,
		CarrierLastStatusDate,
		LastFSRStatusRequestDate,	
		ShipmentPayableStatusCode,
		ShipmentReceivableStatusCode,
		IsNewARInvoiceBlocked,
		NoFreightFile,
		StatusId,
		MasterShipmentDataId
		From Shipments
		Where IsCancelled = 0 AND Tenant = @Tenant
	END
END

-- Operational Open
set @OperationalOpen_Shipments = (select count(*) from @MemoryTable where IsOperationalClosed = 0 and ShipmentLevelCode in ('D', 'H', 'A'))
set @OperationalOpen_Masters = (select count(*) from @MemoryTable where IsOperationalClosed = 0 and ShipmentLevelCode in ('D', 'C'))
set @OperationalOpen_ImportShipments = (select count(*) from @MemoryTable where IsOperationalClosed = 0 and ShipmentLevelCode != 'C' and (DirectionId = 'I' OR (DirectionId = 'C' AND NoFreightFile = 1)))

-- Accounting Open
set @AccountingOpen_OpenReceivablesShipments = (select count(*) from @MemoryTable where IsAccountingClosed = 0 and ShipmentReceivableStatusCode = 'OPEN' and ShipmentLevelCode in ('D', 'H'))
set @AccountingOpen_OpenPayablesMasters = (select count(*) from @MemoryTable where IsAccountingClosed = 0 and ShipmentPayableStatusCode = 'OPEN' and ShipmentLevelCode in ('D', 'C'))

-- Others
set @EAWB_AirlineUpdates = (select count(*) from @MemoryTable where ShipmentLevelCode != 'H' and CarrierLastStatusDate >= @lastWeekDate)
set @Others_CreditLimitBlocked = (select count(*) from @MemoryTable where IsNewARInvoiceBlocked = 1)
set @FSR_FSRRequestLast7Days = (select count(*) from @MemoryTable where ShipmentLevelCode != 'H' and TransportModeId = 'A' and LastFSRStatusRequestDate >= @lastWeekDate)
-------------------------------------------------

if (@HasETDFeature = 1)
BEGIN
	declare @StatusId_Arrived as varchar(15)
	declare @StatusId_Delivered as varchar(15)
	set @StatusId_Arrived = (select Id from EntityStatus where Tenant = @Tenant and Code = 'SARR')
	set @StatusId_Delivered = (select Id from EntityStatus where Tenant = @Tenant and Code = 'SDLD')

	set @EAWB_ExpectedDeparture = (select count(*)
	from ShipmentMasterDatas
	where
	Tenant = @Tenant
	AND MainCarriageATD is null
	AND MainCarriageATA is null
	AND MainCarriageETD is not null
	AND MainCarriageETD > @lastWeekDate
	AND Id in
		(
		select MasterShipmentDataId from @MemoryTable
		where MasterShipmentDataId is not null
		AND ShipmentLevelCode != 'H'
		AND DirectionId = 'E'
		AND TransportModeId = 'A'
		AND StatusId != @StatusId_Arrived
		AND StatusId != @StatusId_Delivered
		)
	)
END

if (@HasFollowupsFeature = 1)
BEGIN	
	set @Others_AllFollowups = (select count(*)
	from FollowUps
	where
	Tenant = @Tenant
	AND ShipmentId is not null
	AND ShipmentId in (select Id from @MemoryTable))
	
	set @Others_MyFollowups = (select count(*)
	from FollowUps
	where
	Tenant = @Tenant
	AND ShipmentId is not null
	AND OwnerUserId = @LoggedUserId
	AND ShipmentId in (select Id from @MemoryTable))	
END





