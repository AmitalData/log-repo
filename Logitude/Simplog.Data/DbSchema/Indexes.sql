/*
   Monday, June 07, 20107:20:59 PM
   User: sa
   Server: JALAL-PC
   Database: WebFreight
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE NONCLUSTERED INDEX IX_Shipments_Tenant_IsOperationalClosed ON dbo.Shipments
	(
	Tenant,
	IsOperationalClosed
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_Shipments_Tenant_OpenDate ON dbo.Shipments
	(
	Tenant,
	CreateDateTime DESC
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
--ALTER TABLE dbo.Shipments SET (LOCK_ESCALATION = TABLE)
--GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE NONCLUSTERED INDEX IX_Cards_Tenant_CreateDate ON dbo.Cards
	(
	Tenant,
	CreateDate DESC
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_Customers_Tenant_LastShipmentDate ON dbo.Customers
	(
	Tenant,
	LastShipmentDate DESC
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_Quotes_Tenant_OpenDate ON dbo.Quotes
	(
	Tenant,
	OpenDate DESC
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
--ALTER TABLE dbo.Shipments SET (LOCK_ESCALATION = TABLE)
--GO
COMMIT

--Delete from Production
CREATE NONCLUSTERED INDEX [IX_TraceEvents_Tenant_LogDateTime] ON [dbo].[TraceEvents] 
(
	[Tenant],
	[LogDateTime] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_TraceEvents_Tenant_EntityId] ON [dbo].[TraceEvents] 
(
	[Tenant],
	[EntityId] 
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Shipments_Tenant_CustomFileId ON dbo.Shipments
	(
	Tenant,
	CustomFileId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Shipments_Tenant_CustomFileNumber ON dbo.Shipments
	(
	Tenant,
	CustomFileNumber
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Contacts_Tenant_Email ON dbo.Contacts
	(
	Tenant,
	Email 
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Customers_Tenant_LastShipmentDate ON dbo.Customers
	(
	Tenant,
	LastShipmentDate desc 
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_GlobalContacts_GlobalTenantId_Email ON dbo.GlobalContacts
	(
	GlobalTenantId,
	Email  
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_TraceEVents_Tenant_ExternalId] ON [dbo].[TraceEvents]
(
	[Tenant] ASC,
	[ExternalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX IX_shipmentCarrierStatus_shipmentId ON dbo.shipmentcarrierstatuses
	(
	shipmentId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_APInvoices_MainEntityId ON dbo.APInvoices
	(
	MainEntityId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_ArInvoices_MainEntityId ON dbo.ArInvoices
	(
	MainEntityId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Eventtypes_Code_objecttableId] ON [dbo].[EventTypes]
(
	[Tenant] ASC,
	[Code] ASC,
	[objecttableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
CREATE NONCLUSTERED INDEX IX_ErrorLogs_LogDate ON dbo.ErrorLogs
	(
	LogDate
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_ErrorLogs_Tenant ON dbo.ErrorLogs
	(
	Tenant
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_ContactActivityLogs_Tenant_IsSharedLogisticsContact ON dbo.ContactActivityLogs
	(
	Tenant,
	IsSharedLogisticsContact
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_ContactActivityLogs_Tenant_GMTLogDateTime ON dbo.ContactActivityLogs
	(
	Tenant,
	GMTLogDateTime desc
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
CREATE INDEX IX_CommunicationLogs_CreateDate
ON dbo.CommunicationLogs
(
	CreateDate desc
)
GO
CREATE INDEX IX_ErrorLogs_LogDate
ON dbo.ErrorLogs
(
	LogDate desc
)
GO
CREATE INDEX IX_PerformanceLogs_LogDateTimeGMT
ON dbo.performancelogs
(
	LogDateTimeGMT desc
)
GO
CREATE INDEX IX_ContactsUnseenEntities_Tenant_ContactId_ObjectTableId
ON dbo.ContactsUnseenEntities
(
	Tenant,
	ContactId,
	ObjectTableId
)
GO
CREATE INDEX IX_ContactsUnseenEntities_Tenant_ContactId_ObjectTableId_EntityId 
ON dbo.ContactsUnseenEntities
(
	Tenant,
	ContactId,
	ObjectTableId,
	EntityId 
)
GO
CREATE INDEX IX_SharedFollowedShipments_Tenant_ContactId_ShipmentId
ON dbo.SharedFollowedShipments
(
	Tenant,
	ContactId,
	ShipmentId
)
GO

USE [Global]
GO

/****** Object:  Index [MobileNotificationLogs_EmailId_IsDeleted_CreateDate]    Script Date: 12/16/2015 16:26:22 ******/
CREATE NONCLUSTERED INDEX [MobileNotificationLogs_EmailId_IsDeleted_CreateDate] ON [dbo].[MobileNotificationLogs] 
(
	[Email] ASC,
	[IsDelete] ASC,
	[CreateDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

--Removed
CREATE NONCLUSTERED INDEX IX_APInvoices_Tenant_IsClosed_StatusCode ON dbo.APInvoices
	(
	Tenant,
	IsClosed,
	StatusCode
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_ARInvoices_Tenant_IsClosed_IsCancelled_StatusCode ON dbo.ARInvoices
	(
	Tenant,
	IsClosed,
	IsCancelled,
	StatusCode
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX IX_Ports_Tenant_InActive_IsAir_EnglishName ON dbo.Ports
	(
	Tenant,
	InActive,
	IsAir,
	EnglishName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Shipments_Tenant_TransportModeId_DirectionId_IsCancelled_ShipmentLevelCode]
ON [dbo].[Shipments] ([Tenant],[TransportModeId],[DirectionId],[IsCancelled],[ShipmentLevelCode])
INCLUDE ([Id],[StatusId],[MasterShipmentDataId])

CREATE NONCLUSTERED INDEX [IX_ShipmentMasterDatas_Tenant_Master_InterlineId]
ON [dbo].[ShipmentMasterDatas] ([Tenant],[Master],[InterlineId])

CREATE NONCLUSTERED INDEX IX_Cards_Tenant_InActive_PartnerTypeId_EnglishName ON dbo.Cards
	(
	Tenant,
	InActive,
	PartnerTypeId,
	EnglishName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--executed
CREATE NONCLUSTERED INDEX [IX_Shipments_Tenant_IsCancelled_CarrierLastStatusDate] ON [dbo].[Shipments] ([Tenant], [IsCancelled], [CarrierLastStatusDate])
WITH (ONLINE = ON)

--not yet
CREATE NONCLUSTERED INDEX [IX_EntityLastActivities_ObjectTableId_Tenant_UserId] ON [dbo].[EntityLastActivities] ([ObjectTableId], [Tenant], [UserId]) INCLUDE ([ActivityDate], [EntityId]) WITH (ONLINE = ON)

CREATE NONCLUSTERED INDEX [IX_ShipmentMasterDatas_Master_InterlineId]
ON [dbo].[ShipmentMasterDatas] ([Master],[InterlineId])

--removed
CREATE NONCLUSTERED INDEX [IX_ShipmentComputedFields_Tenant] ON [dbo].[ShipmentComputedFields] ([Tenant])
WITH (ONLINE = ON)

--executed
CREATE NONCLUSTERED INDEX [IX_CustomerActualDataHistory_Tenant] ON [dbo].[CustomerActualDataHistory] ([Tenant])
WITH (ONLINE = ON)

--executed
CREATE NONCLUSTERED INDEX [IX_ObjectTableLastUpdates_Tenant_LastUpdateDate]
ON [dbo].[ObjectTableLastUpdates] ([Tenant],[LastUpdateDate])

--executed
	CREATE NONCLUSTERED INDEX [IX_CommunicationLogs_CreateDateUTC_CommunicationStatusTypeCode]
ON [dbo].[CommunicationLogs] ([CreateDateUTC],[CommunicationStatusTypeCode])
WITH (ONLINE = ON)

CREATE NONCLUSTERED INDEX IX_Shipments_Tenant_TransportModeId_IsCancelled_LastFSRStatusRequestDate
ON [dbo].[Shipments] ([Tenant],[TransportModeId],[IsCancelled],[LastFSRStatusRequestDate])
INCLUDE ([Id],[ShipmentLevelCode]


------May 9 2016
CREATE NONCLUSTERED INDEX [IX_AirlineStatistics_Tenant_BookingId] ON [dbo].[AirlineStatistics] ([Tenant],[BookingId]) WITH (ONLINE = ON)

drop index IX_APInvoices_Tenant_IsClosed_StatusCode on [APInvoices]

CREATE NONCLUSTERED INDEX [APInvoices_Tenant_StatusCode] ON [dbo].[APInvoices] ([Tenant], [StatusCode]) 
CREATE NONCLUSTERED INDEX [IX_APInvoiceLines_Tenant_EntityPayableId] ON [dbo].[APInvoiceLines] ([Tenant], [EntityPayableId]) WITH (ONLINE = ON)


----------- May 17 2016
CREATE NONCLUSTERED INDEX [IX_DocumentsFilings_EntityId]
ON [dbo].[DocumentsFilings] ([EntityId])

-----14/6/2016
CREATE NONCLUSTERED INDEX [IX_LogitudeMessagesTransmissionLogs_AWBNumber_SentDate_Tenant] ON [dbo].[LogitudeMessagesTransmissionLogs] ([AWBNumber], [SentDate], [Tenant]) WITH (ONLINE = ON)
CREATE NONCLUSTERED INDEX [IX_CustomerActualDataHistory_Tenant_StartDateTime] ON [dbo].[CustomerActualDataHistory] ([Tenant], [StartDateTime])  WITH (ONLINE = ON)
CREATE NONCLUSTERED INDEX [IX_Cards_Tenant_InActive_PartnerTypeId_IsCustomer] ON [dbo].[Cards] ([Tenant], [InActive], [PartnerTypeId], [IsCustomer])  WITH (ONLINE = ON)

----- 21/6/2016
CREATE NONCLUSTERED INDEX [IX_AirlineStatistics_Tenant_ShipmentId] ON [dbo].[AirlineStatistics] ([Tenant],[ShipmentId]) WITH (ONLINE = ON)

--Another index added below with Tenant 
--CREATE NONCLUSTERED INDEX IX_APILogs_CorrelationId ON dbo.APILogs
--(
--CorrelationId
--) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--GO


CREATE NONCLUSTERED INDEX IX_Tenant_BatchNumber_CreateDate ON dbo.APILogs
(
Tenant,
BatchNumber,
CreateDate
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX IX_Contacts_ExternalId ON dbo.Contacts
(
ExternalId
) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_QueueMessages_QueueDefinitionCode_Status_NextRunDateTime
ON [dbo].[QueueMessages] ([QueueDefinitionCode],[Status],[NextRunDateTime])
INCLUDE ([Id])
GO

CREATE NONCLUSTERED INDEX IX_DocumentsFilings_Tenant_EntityId_IsRequested
ON [dbo].[DocumentsFilings] ([Tenant],[EntityId],[IsRequested])

CREATE NONCLUSTERED INDEX IX_Addresses_Tenant_ExternalId
ON [dbo].[Addresses] ([Tenant],[ExternalId])

--Added for Amital performance problem. 
CREATE NONCLUSTERED INDEX IX_Addresses_Tenant_AddressTypeId_CardId
ON [dbo].[Addresses] ([Tenant],[AddressTypeId],[CardId])

CREATE NONCLUSTERED INDEX IX_APILogs_Tenant_CorrelationId
ON [dbo].[APILogs] ([Tenant],[CorrelationId])

CREATE NONCLUSTERED INDEX IX_MobileNotificationLogs_IsRead_IsDelete
ON [dbo].[MobileNotificationLogs] ([IsRead],[IsDelete])
INCLUDE ([Email])


CREATE NONCLUSTERED INDEX IX_DocumentsFilings_Tenant_EntityId_IsDeleted
ON [dbo].[DocumentsFilings] ([Tenant],[EntityId],[IsDeleted])

CREATE NONCLUSTERED INDEX IX_CustomerSalesmanByProducts_Tenant_CustomerId
ON [dbo].[CustomerSalesmanByProducts] ([Tenant],[CustomerId])

CREATE NONCLUSTERED INDEX IX_CustomerAccountManagerByProducts_Tenant_CustomerId
ON [dbo].[CustomerAccountManagerByProducts] ([Tenant],[CustomerId])



	CREATE NONCLUSTERED INDEX IX_MobileNotificationLogs_AndroidStatus_IOSStatus
ON [dbo].[MobileNotificationLogs] (AndroidStatus,IOSStatus)

	CREATE NONCLUSTERED INDEX IX_Cards_Code
ON [dbo].[Cards] (Code)

CREATE NONCLUSTERED INDEX IX_MobileNotificationLogs_Email_IsRead_IsDelete
ON [dbo].[MobileNotificationLogs] ([Email],[IsRead],[IsDelete])

	CREATE NONCLUSTERED INDEX IX_Bookings_Tenant_BookingStatusCode
ON [dbo].[Bookings] (Tenant,BookingStatusCode)



CREATE NONCLUSTERED INDEX [IX_Eventtypes_Tenant_objecttableId] ON [dbo].[EventTypes]
(
	[Tenant] ASC,
	[objecttableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

--Abed 4/19/2017
CREATE NONCLUSTERED INDEX [IX_Shipments_Tenant_AgentSharedManifestRef] ON [dbo].[Shipments]
(
	[Tenant] ASC,
	[AgentSharedManifestRef] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

--25/7/2017
CREATE NONCLUSTERED INDEX [IX_ShipmentMasterDatas_Tenant ]
ON [dbo].[ShipmentMasterDatas] ([Tenant])
INCLUDE ([MainCarriageATD],[MainCarriageETD],[MainCarriageCarrierNumber],[MainCarriageCarrierId],[DepartureArrivalFromDate],[DepartureArrivalToDate])

CREATE NONCLUSTERED INDEX [IX_Shipments_Tenant_DirectId_IsOperationalClosed_IsCancelled]
ON [dbo].[Shipments] ([Tenant],[DirectionId],[IsOperationalClosed],[IsCancelled])
INCLUDE ([ShipmentLevelCode],[NoFreightFile])

--Not good
       CREATE NONCLUSTERED INDEX [IX_ShipmentMasterDatas_DepartureArrivalFromDate_DepartureArrivalToDate]
ON [dbo].[ShipmentMasterDatas] (DepartureArrivalFromDate,DepartureArrivalToDate)

-- 13-aug-2017

CREATE NONCLUSTERED INDEX [IX_Cards_UpdateDate]
ON [dbo].[Cards] ([UpdateDate])
INCLUDE ([Id],[EnglishName],[Code],[CreateDate],[CityName],[CountryName],[PrimaryContactId],[CountryId],[CountryCode])

--30/8/2017 : Jalal
CREATE NONCLUSTERED INDEX [IX_Shipments_Tenant_IsCancelled_IsNewARInvoiceBlocked_ProductCode]
ON [dbo].[Shipments] ([Tenant],[IsCancelled],[IsNewARInvoiceBlocked],[ProductCode])



CREATE NONCLUSTERED INDEX IX_EntityChange_EntityId_ObjectTableId_Tenant
ON [dbo].[EntityChanges] ([EntityId],[ObjectTableId],[Tenant])

--11/06/2018 : Rabaia

CREATE NONCLUSTERED INDEX [IX_Tenant_ForwarderShipmentNumber_IsCancelled]
ON [dbo].[Shipments] ([Tenant],[ForwarderShipmentNumber],[IsCancelled])

CREATE NONCLUSTERED INDEX [IX_Tenant_SignRequestByUserEmail]
ON [dbo].[DocumentsFilings] ([Tenant],[SignRequestByUserEmail])

CREATE NONCLUSTERED INDEX [IX_Tenant_Code]
ON [dbo].[DocumentsMetaDataTypes]([Tenant],[Code])


IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipments_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Shipments]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Shipments_AutomaticLastUpdateDate]
ON [dbo].[Shipments]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Cards_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Cards]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Cards_AutomaticLastUpdateDate]
ON [dbo].[Cards]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Directions_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Directions]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Directions_AutomaticLastUpdateDate]
ON [dbo].[Directions]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Ports_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Ports]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Ports_AutomaticLastUpdateDate]
ON [dbo].[Ports]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_TransportModes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[TransportModes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_TransportModes_AutomaticLastUpdateDate]
ON [dbo].[TransportModes]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentLevels_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ShipmentLevels]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ShipmentLevels_AutomaticLastUpdateDate]
ON [dbo].[ShipmentLevels]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentTypes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ShipmentTypes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ShipmentTypes_AutomaticLastUpdateDate]
ON [dbo].[ShipmentTypes]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Departments_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Departments]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Departments_AutomaticLastUpdateDate]
ON [dbo].[Departments]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Incoterms_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Incoterms]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Incoterms_AutomaticLastUpdateDate]
ON [dbo].[Incoterms]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Users_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Users]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Users_AutomaticLastUpdateDate]
ON [dbo].[Users]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Currencies_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Currencies]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Currencies_AutomaticLastUpdateDate]
ON [dbo].[Currencies]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EntityStatus_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[EntityStatus]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_EntityStatus_AutomaticLastUpdateDate]
ON [dbo].[EntityStatus]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Addresses_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Addresses]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Addresses_AutomaticLastUpdateDate]
ON [dbo].[Addresses]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Countries_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Countries]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Countries_AutomaticLastUpdateDate]
ON [dbo].[Countries]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Contacts_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Contacts]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Contacts_AutomaticLastUpdateDate]
ON [dbo].[Contacts]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_States_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[States]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_States_AutomaticLastUpdateDate]
ON [dbo].[States]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Customers_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Customers]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Customers_AutomaticLastUpdateDate]
ON [dbo].[Customers]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PartnerTypes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[PartnerTypes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_PartnerTypes_AutomaticLastUpdateDate]
ON [dbo].[PartnerTypes]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentMasterDatas_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ShipmentMasterDatas]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ShipmentMasterDatas_AutomaticLastUpdateDate]
ON [dbo].[ShipmentMasterDatas]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Tenants_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Tenants]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Tenants_AutomaticLastUpdateDate]
ON [dbo].[Tenants]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Branches_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Branches]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Branches_AutomaticLastUpdateDate]
ON [dbo].[Branches]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Ranks_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Ranks]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Ranks_AutomaticLastUpdateDate]
ON [dbo].[Ranks]([AutomaticLastUpdateDate])
  end   

-- 07-10-2018 - added by Ihab
CREATE NONCLUSTERED INDEX IX_DocumentsFilings_Tenant_SecurityId
ON [dbo].[DocumentsFilings] ([Tenant],[SecurityId])

CREATE NONCLUSTERED INDEX IX_ARInvoices_ARInvoices_IsCancelled_IsClosed_StatusCode
ON [dbo].[ARInvoices] ([Tenant],[IsCancelled],[IsClosed],[StatusCode])

CREATE NONCLUSTERED INDEX IX_EventTypes_Tenant
ON [dbo].[EventTypes] ([Tenant])

CREATE NONCLUSTERED INDEX IX_AWBOCIs_ShipmentId
ON [dbo].AWBOCIs ([ShipmentId])

CREATE NONCLUSTERED INDEX IX_Users_Tenant
ON [dbo].[Users] ([Tenant])


   --System Log DB
IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FailedLoginLogs_GMTDateTime'
    AND object_id = OBJECT_ID('[dbo].[FailedLoginLogs]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_FailedLoginLogs_GMTDateTime]
ON [dbo].[FailedLoginLogs]([GMTDateTime])
  end   


  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FailedTokenLogs_GMTDateTime'
    AND object_id = OBJECT_ID('[dbo].[FailedTokenLogs]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_FailedTokenLogs_GMTDateTime]
ON [dbo].[FailedTokenLogs]([GMTDateTime])
  end   

  CREATE NONCLUSTERED INDEX IX_ContactActivityLogs_Tenant_LogDateTime_PartnerTypeId ON [dbo].[ContactActivityLogs]
	(
	Tenant,
	LogDateTime,
	PartnerTypeId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


CREATE NONCLUSTERED INDEX IX_Activities_Tenant_ActivityTypeCode_ShipmentId
ON [dbo].[Activities] ([Tenant],[ActivityTypeCode],[ShipmentId])


CREATE NONCLUSTERED INDEX IX_ShipmentCarrierStatuses_RecordHash
ON [dbo].[ShipmentCarrierStatuses] ([RecordHash])

GO
CREATE NONCLUSTERED INDEX [ShipmentMasterDatas_Master_AirlinePrefix]
ON [dbo].[ShipmentMasterDatas] ([Master],[AirlinePrefix])

GO



GO
CREATE NONCLUSTERED INDEX [DocumentsFilings_Tenant_ForwarderDocumentId]
ON [dbo].[DocumentsFilings] ([Tenant],[ForwarderDocumentId])

