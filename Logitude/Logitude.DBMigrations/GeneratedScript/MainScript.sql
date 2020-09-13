-- Trigger Script From Trigger_AutomaticLastUpdateDateAddresses.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateAddresses]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateAddresses] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateAddresses ON Addresses AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Addresses SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateBranches.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateBranches]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateBranches] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateBranches ON Branches AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Branches SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCards.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCards]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCards] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCards ON Cards AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Cards SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateChargesTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateChargesTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateChargesTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateChargesTypes ON ChargesTypes AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ChargesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateContacts.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateContacts]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateContacts] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateContacts ON Contacts AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Contacts SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCountries.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCountries]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCountries] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCountries ON Countries AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Countries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCurrencies.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCurrencies]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCurrencies] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCurrencies ON Currencies AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Currencies SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCustomers.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCustomers]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCustomers] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomers ON Customers AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Customers SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCustomerSizes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCustomerSizes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCustomerSizes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomerSizes ON CustomerSizes AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE CustomerSizes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateDepartments.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateDepartments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateDepartments] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateDepartments ON Departments AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Departments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateDWHSettings.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateDWHSettings]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateDWHSettings] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateDWHSettings ON DWHSettings AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE DWHSettings SET AutomaticLastUpdateDate = GETDATE() WHERE Tenant IN (SELECT DISTINCT Tenant FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateIncoterms.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateIncoterms]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateIncoterms] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateIncoterms ON Incoterms AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Incoterms SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateIndustries.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateIndustries]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateIndustries] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateIndustries ON Industries AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Industries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateLoadSoucess.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateLoadSoucess]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateLoadSoucess] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateLoadSoucess ON LeadSources AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE LeadSources SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDatePartnerTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDatePartnerTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDatePartnerTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDatePartnerTypes ON PartnerTypes AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE PartnerTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDatePorts.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDatePorts]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDatePorts] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDatePorts ON Ports AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Ports SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateRanks.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateRanks]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateRanks] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateRanks ON Ranks AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Ranks SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateRegions.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateRegions]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateRegions] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateRegions ON Regions AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Regions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateStates.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateStates]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateStates] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateStates ON States AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE States SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateTenants.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateTenants]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateTenants] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateTenants ON Tenants AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Tenants SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateUsers.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateUsers]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateUsers] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateUsers ON Users AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Users SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateVessels.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateVessels]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateVessels] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateVessels ON Vessels AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Vessels SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateCustomPickLists.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateCustomPickLists]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateCustomPickLists] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomPickLists ON CustomPickLists AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE CustomPickLists SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateDirections.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateDirections]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateDirections] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateDirections ON Directions AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Directions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateEntityStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateEntityStatus]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateEntityStatus] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateEntityStatus ON EntityStatus AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE EntityStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateMoveTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateMoveTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateMoveTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateMoveTypes ON MoveTypes AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE MoveTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateObjectFields.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateObjectFields]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateObjectFields] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateObjectFields ON ObjectFields AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ObjectFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateTransportModes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateTransportModes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateTransportModes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateTransportModes ON TransportModes AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE TransportModes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateAPInvoiceLines.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateAPInvoiceLines]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateAPInvoiceLines] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateAPInvoiceLines ON APInvoiceLines AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE APInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE APInvoiceId IN (SELECT DISTINCT APInvoiceId FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateAPInvoices.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateAPInvoices]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateAPInvoices] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateAPInvoices ON APInvoices AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE APInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateARInvoiceLines.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateARInvoiceLines]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateARInvoiceLines] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateARInvoiceLines ON ARInvoiceLines AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ARInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateARInvoices.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateARInvoices]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateARInvoices] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateARInvoices ON ARInvoices AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ARInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentComputedFields.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentComputedFields]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentComputedFields] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentComputedFields ON ShipmentComputedFields AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentComputedFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentLevels.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentLevels]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentLevels] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentLevels ON ShipmentLevels AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentLevels SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentMasterDatas.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentMasterDatas]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentMasterDatas] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentMasterDatas ON ShipmentMasterDatas AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentMasterDatas SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentPayables.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentPayables]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentPayables] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentPayables ON ShipmentPayables AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentPayables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentPayableStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentPayableStatus]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentPayableStatus] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentPayableStatus ON ShipmentPayableStatus AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentPayableStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentReceivables.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivables]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivables] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentReceivables ON ShipmentReceivables AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentReceivables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentReceivableStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivableStatus]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentReceivableStatus] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentReceivableStatus ON ShipmentReceivableStatus AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentReceivableStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipments.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipments]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipments] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipments ON Shipments AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateShipmentTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateShipmentTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateShipmentTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentTypes ON ShipmentTypes AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE ShipmentTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


-- Trigger Script From Trigger_AutomaticLastUpdateDateSpecialServicesTypes.dxml
EXEC('IF (OBJECT_ID(''[dbo].[Trigger_AutomaticLastUpdateDateSpecialServicesTypes]'', ''TR'') IS NOT NULL) BEGIN DROP TRIGGER [dbo].[Trigger_AutomaticLastUpdateDateSpecialServicesTypes] END');
EXEC('CREATE TRIGGER Trigger_AutomaticLastUpdateDateSpecialServicesTypes ON SpecialServicesTypes AFTER UPDATE  AS  BEGIN
SET NOCOUNT ON;
IF TRIGGER_NESTLEVEL() > 1 RETURN;
IF (SELECT program_name FROM sys.dm_exec_sessions WHERE session_id = (SELECT @@SPID)) <> ''ZeroDownTimeDBMigrationsTool''
BEGIN
UPDATE SpecialServicesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted);
END;
END;');


