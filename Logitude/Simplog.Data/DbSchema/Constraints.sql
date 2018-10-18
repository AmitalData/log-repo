BEGIN TRANSACTION ApplyConstraints
--USE [WebFreightBranch2]
--Go

/****** Object:  Index [UQ_Tenant_Airline_Number]    Script Date: 05/30/2011 12:43:09 ******/


ALTER TABLE [dbo].[MAWBStacks] ADD  CONSTRAINT [UQ_Tenant_Airline_Number_MAWBStacks] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[AirlineId] ASC,
	[Number] ASC
)

ALTER TABLE [dbo].[Ports] ADD  CONSTRAINT [UQ_Tenant_Code_Ports] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC,
	[CountryId] ASC
)

ALTER TABLE [dbo].[Contacts] ADD  CONSTRAINT [UQ_Tenant_Code_Contacts] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Email] ASC
)

ALTER TABLE [dbo].[States] ADD  CONSTRAINT [UQ_Tenant_Code_States] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC,
	[CountryId] ASC
)

ALTER TABLE [dbo].[DocumentTypes] ADD  CONSTRAINT [UQ_Tenant_Code_DocumentTypes] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

ALTER TABLE [dbo].[EventTypes] ADD  CONSTRAINT [UQ_Tenant_Code_ObjectTableId_EventTypes] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC,
	[ObjectTableId] ASC
)

ALTER TABLE [dbo].[PackageTypes] ADD  CONSTRAINT [UQ_Tenant_Code_PackageTypes] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

ALTER TABLE [dbo].[Vessels] ADD  CONSTRAINT [UQ_Tenant_Code_Vessels] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

ALTER TABLE [dbo].[Warehouses] ADD  CONSTRAINT [UQ_Tenant_Code_Warehouses] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

ALTER TABLE [dbo].[Incoterms] ADD  CONSTRAINT [UQ_Tenant_Code_Incoterms] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

ALTER TABLE [dbo].[Currencies] ADD  CONSTRAINT [UQ_Tenant_Code_Currencies] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

ALTER TABLE [dbo].[VatTypes] ADD  CONSTRAINT [UQ_Tenant_Code_VatTypes] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
)

--ALTER TABLE [dbo].[ChargesTypes] ADD  CONSTRAINT [UQ_Tenant_Code_ChargesTypes] UNIQUE NONCLUSTERED 
--(
	--[Tenant] ASC,
	--[Code] ASC
--)
--
ALTER TABLE [dbo].[Cards] ADD  CONSTRAINT [UQ_Tenant_Code_PartnerTypeId_Cards] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC
	--[PartnerTypeId] ASC
)

ALTER TABLE [dbo].[Shipments] ADD  CONSTRAINT [UQ_Tenant_ShipmentNumber_Shipments] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[ShipmentNumber] ASC
)

ALTER TABLE [dbo].[Quotes] ADD  CONSTRAINT [UQ_Tenant_QuoteNumber_Quotes] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[QuoteNumber] ASC
)

ALTER TABLE [dbo].[ARInvoices] ADD  CONSTRAINT [UQ_Tenant_InvoiceNumber_ARInvoices] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[InvoiceNumber] ASC
)

ALTER TABLE [dbo].[TextCodes] ADD  CONSTRAINT [UQ_Tenant_Code_ObjectTableId_TextCodes] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] Asc,
	[ObjectTableId] ASC
)

ALTER TABLE [dbo].[TranslationHeaders] ADD  CONSTRAINT [UQ_Tenant_Description] UNIQUE NONCLUSTERED 
(
	[Description] ASC,
	[Tenant] ASC
)

ALTER TABLE [dbo].[Measurements] ADD  CONSTRAINT [UQ_Tenant_Code_Measrements] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[Tenant] ASC

)

ALTER TABLE [dbo].[Queries] ADD  CONSTRAINT [UQ_Tenant_Code_ObjectTableId_UserId] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[ObjectTableId] Asc,
	[UserId] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[QueryColumns] ADD  CONSTRAINT [UQ_Tenant_QueryId_ObjectFieldId_UserId] UNIQUE NONCLUSTERED 
(
	[Tenant] Asc,
	[QueryId] Asc,
	[ObjectFieldId] Asc,
	[UserId] ASC
)

ALTER TABLE [dbo].[QueryColumns] ADD  CONSTRAINT [UQ_Tenant_QueryId_IndexOrder_UserId] UNIQUE NONCLUSTERED 
(
	[Tenant] Asc,
	[QueryId] Asc,
	[IndexOrder] Asc,
	[UserId] ASC
)

ALTER TABLE [dbo].[Screens] ADD  CONSTRAINT [UQ_Tenant_Code_ObjectTableId_Screens] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[ObjectTableId] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[ObjectTableRules] ADD  CONSTRAINT [UQ_Tenant_RuleCode] UNIQUE NONCLUSTERED 
(
	[RuleCode] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[ObjectTableRuleFields] ADD  CONSTRAINT [UQ_Tenant_ObjectFieldId_ObjectTableRuleId] UNIQUE NONCLUSTERED 
(
	[ObjectFieldId] Asc,
	[ObjectTableRuleId] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[ObjectTableTabs] ADD  CONSTRAINT [UQ_Tenant_Code_ObjectTableTabs] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[ObjectTableHelperControls] ADD  CONSTRAINT [UQ_Tenant_Code_ObjectTableHelperControls] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[EntityStatus] ADD  CONSTRAINT [UQ_Tenant_Code_EntityStatus] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[Tenant] ASC,
	[ObjectTableId] ASC
)

ALTER TABLE [dbo].[Ranks] ADD  CONSTRAINT [UQ_Tenant_Code_Ranks] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[MenusTables] ADD  CONSTRAINT [UQ_Tenant_Code_MenusTables] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [UQ_Tenant_Code_Roles] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[Features] ADD  CONSTRAINT [UQ_Tenant_Code_ObjectTableId_Features] UNIQUE NONCLUSTERED 
(
	[Code] Asc,
	[ObjectTableId] Asc,
	[Tenant] ASC
)


ALTER TABLE [dbo].[RoleFeatures] ADD  CONSTRAINT [UQ_Tenant_RoleId_FeatureId_Features] UNIQUE NONCLUSTERED 
(
	[RoleId] Asc,
	[FeatureId] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[ObjectFields] ADD  CONSTRAINT [UQ_Tenant_FieldName_ObjectTableId] UNIQUE NONCLUSTERED 
(
	[FieldName] Asc,
	[ObjectTableId] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[ObjectTables] ADD  CONSTRAINT [UQ_Tenant_Name_ObjectTables] UNIQUE NONCLUSTERED 
(
	[Name] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[MenuButtons] ADD  CONSTRAINT [UQ_Tenant_EventCode_MenuButtonGroupId] UNIQUE NONCLUSTERED 
(
	[EventCode] Asc,
	[MenuButtonGroupId] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[MenuButtonGroups] ADD  CONSTRAINT [UQ_Tenant_Name_MenuButtonGroups] UNIQUE NONCLUSTERED 
(
	[Name] Asc,
	[Tenant] ASC
)

ALTER TABLE [dbo].[DBIdCounters] ADD  CONSTRAINT [UQ_Code_DBIdCounters] UNIQUE NONCLUSTERED 
(
	[TableName] ASC
)

ALTER TABLE [dbo].[CounterDefinitions] ADD  CONSTRAINT [UQ_Tenant_Code] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] Asc
)

ALTER TABLE [dbo].[CountryCities] ADD  CONSTRAINT [UQ_Tenant_EnglishName_CountryId] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[EnglishName] Asc,
	[CountryId] Asc
)

ALTER TABLE [dbo].[GlobalContacts] ADD  CONSTRAINT [UQ_GlobalTenantId_Email] UNIQUE NONCLUSTERED 
(
	[GlobalTenantId] ASC,
	[Email] Asc
)


/****** Object:  Index [Shipments_CustomerId_ComputedStatusDate]    Script Date: 11/19/2015 13:43:51 ******/
CREATE NONCLUSTERED INDEX [Shipments_CustomerId_ComputedStatusDate] ON [dbo].[Shipments] 
(
	[CustomerId] ASC,
	[ComputedStatusDate] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



ALTER TABLE [dbo].[CardContacts] ADD  CONSTRAINT [UQ_CardId_ContactId_CardContacts] UNIQUE NONCLUSTERED 
(
	[CardId] ASC,
	[ContactId] ASC
)

USE [Global]
GO

/****** Object:  Index [MobileNotificationLogs_EmailId_IsDeleted_CreateDate]    Script Date: 12/08/2015 08:15:46 ******/
CREATE NONCLUSTERED INDEX [MobileNotificationLogs_Email_IsDeleted_CreateDate] ON [dbo].[MobileNotificationLogs] 
(
	[Email] ASC,
	[IsDelete] ASC,
	[CreateDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [IX_AnalyzeQueues_Subject] ON [dbo].[AnalyzeQueues] ([Subject])  INCLUDE ([CreateDate]) WITH (ONLINE = ON)


ALTER TABLE [dbo].[ObjectFields] ADD  CONSTRAINT [UQ_ObjectFields_Code_ObjectTableId_Tenant] UNIQUE NONCLUSTERED 
(
	[Tenant] ASC,
	[Code] ASC,
	[ObjectTableId] ASC
)