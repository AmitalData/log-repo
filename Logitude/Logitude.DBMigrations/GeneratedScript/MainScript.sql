-- DataView Script From CustomersDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[CustomersDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[CustomersDataView] END');
EXEC('CREATE VIEW [dbo].[CustomersDataView]
AS
SELECT
dbo.Customers.Id, dbo.Customers.Tenant,dbo.cards.EnglishName,dbo.cards.ZipCode,dbo.cards.Address1,dbo.cards.Address2,dbo.cards.Phone,dbo.Cards.LocalName,dbo.Cards.ReceivablesAccountingCard, dbo.Cards.PayablesAccountingCard, dbo.Cards.ExternalId2,dbo.Cards.InActive,dbo.Cards.EnableConsolidationInvoices, dbo.Cards.ExternalAccountingBusinessArea, dbo.Cards.SATPaymentMethodCode,dbo.Cards.SATForeignRFC,dbo.Cards.MetodoPagoCode,dbo.Cards.UsoCFDICode
,dbo.Cards.Notes,dbo.Customers.BillToId,dbo.Cards.Website,dbo.Customers.SalesmanUserId,dbo.Cards.PaymentTermId,dbo.Cards.CreateDate,dbo.Cards.UpdateDate,dbo.Cards.CreatedByUserId,dbo.Cards.UpdatedByUserId
,dbo.Cards.VatNumber,dbo.Cards.SearchFields,dbo.PaymentTerms.EnglishName as PaymentTermEnglishName,dbo.Cards.InvoiceCurrencyId,dbo.Customers.LastShipmentDate,dbo.Customers.StartWorkingDate,dbo.Customers.StartWorkingManuallySet
,AccountManagerUserContacts.EnglishName as AccountManagerUserEnglishName,SalesmanUserContacts.EnglishName as SalesmanUserEnglishName,CollectorContacts.EnglishName as CollectorName,ClassifierContacts.EnglishName as ClassifierName
,dbo.Cards.CityName ,dbo.Cards.VatTypeId,BillToCards.EnglishName as BillToName,dbo.Customers.Field1,dbo.Customers.Field2,dbo.Customers.Field3,dbo.Customers.Field4,dbo.Customers.Field5,dbo.Customers.Field6,dbo.Customers.Field7,dbo.Customers.Field8
,dbo.Customers.Field9,dbo.Customers.Field10,dbo.Ranks.Code as RankCode,dbo.Ranks.Name as RankName
,dbo.Cards.SharedLogisticsInvitationStatusCode, dbo.Customers.ActivityWatch
,dbo.SharedLogisticsInvitationStatus.Name as SharedLogisticsInvitationStatusName
,dbo.cards.LastLoginDate,dbo.cards.InvitationDate,dbo.Industries.Name as IndustryName
,dbo.customers.LeadDescription,dbo.customers.ClassifierId ,dbo.Cards.IsCustomer,dbo.Customers.CollectorId,dbo.customers.FreelancerId,FreelancerContacts.EnglishName as FreelancerName,dbo.customers.ForwarderId,ForwarderCards.EnglishName as ForwarderName
,dbo.Customers.CustomsAgentId,CustomsAgentCards.EnglishName as CustomsAgentName,dbo.customers.MediatorId,MediatorCards.EnglishName as MediatorName,dbo.customers.BeforeDeactiveStatusCode,dbo.Customers.ReadyForActivationDate,CreatedByUserContacts.EnglishName as UpdatedByUserName
,PrimaryContacts.EnglishName as PrimaryContactName
,PrimaryContacts.Email as PrimaryContactEmail
,dbo.cards.PrimaryContactId,dbo.customers.RegionId,dbo.regions.Name as RegionName, dbo.customers.CustomerStatusCode,dbo.CustomerStatus.Name as CustomerStatusName
,dbo.Customers.RankId, dbo.Customers.LeadSourceId, LeadSources.Name as LeadSourceName, dbo.Customers.IndustryId
,dbo.cards.CountryId ,dbo.cards.CountryCode, dbo.cards.CountryName
,dbo.customers.AccountManagerUserId,CreatedByUserContacts.EnglishName as CreatedByUserName,dbo.cards.code,customers.FirstShipmentDate,dbo.cards.IsActiveForMobile as  IsActiveForMobile
,SalesmanUsers.BusinessUnitId as SalesmanBusinessUnitId,dbo.customers.FirstInvoiceDate,customers.LastOpportunityDate,customers.LastOpportunitySubject,customers.LastOpportunityStatus, customers.LastMeetingDate,customers.LastCallDate,customers.LastQuoteDate,customers.LastInteractionDate
,InvoiceCurrency.Code as InvoiceCurrencyCode
,dbo.Customers.KnownConsignor, dbo.Customers.KCExpirationDate,
dbo.Cards.PartnerTypeId, dbo.customers.CustomerSizeId, CustomerSizes.Name as CustomerSizeName, dbo.Cards.SupportNotes,
dbo.Customers.IsCreditLimitEnabled, dbo.Customers.CreditLimitAmount, dbo.Customers.CreditLimitOpenBalance, dbo.Customers.CreditLimitWarningPercentage,
dbo.Customers.BlockNewInvoiceCreation, dbo.Customers.BlockNewShipmentCreation,dbo.Customers.CompetitorFields,
dbo.Customers.ActivatedByUserId, dbo.Customers.ActivationRequestedByUserId, dbo.Customers.SetAsInactiveByUserId,
ActivatedByUserContacts.EnglishName as ActivatedByUserName, SetAsInactiveByUserContacts.EnglishName as SetAsInactiveByName,
ActivationRequestedByUserContacts.EnglishName as ActivationRequestedByUserName, dbo.Customers.ActivationDate, dbo.Customers.InactiveDate, dbo.Customers.ActivationRequestDate,dbo.cards.CreatedByPartner, dbo.cards.StateName, dbo.cards.GLAccountId , dbo.cards.GLAccountDisplayNumber
FROM            dbo.Customers Inner join
dbo.Cards ON  dbo.Customers.Id = dbo.Cards.Id LEFT OUTER JOIN
dbo.PaymentTerms ON dbo.Cards.PaymentTermId = dbo.PaymentTerms.Id LEFT OUTER JOIN
dbo.Contacts AS AccountManagerUserContacts ON dbo.Customers.AccountManagerUserId = AccountManagerUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SalesmanUserContacts ON dbo.Customers.SalesmanUserId = SalesmanUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS CollectorContacts ON dbo.Customers.CollectorId = CollectorContacts.Id LEFT OUTER JOIN
dbo.Contacts AS ClassifierContacts ON dbo.Customers.ClassifierId = ClassifierContacts.Id LEFT OUTER JOIN
dbo.Cards AS BillToCards ON dbo.Customers.BillToId = BillToCards.Id LEFT OUTER JOIN
dbo.Ranks  ON dbo.Customers.RankId = dbo.Ranks.Id LEFT OUTER JOIN
dbo.SharedLogisticsInvitationStatus ON dbo.Cards.SharedLogisticsInvitationStatusCode = dbo.SharedLogisticsInvitationStatus.Code LEFT OUTER JOIN
dbo.Industries ON dbo.Customers.IndustryId = dbo.Industries.Id LEFT OUTER JOIN
dbo.Contacts AS FreelancerContacts ON dbo.Customers.FreelancerId = FreelancerContacts.Id LEFT OUTER JOIN
dbo.Cards AS ForwarderCards ON dbo.Customers.ForwarderId = ForwarderCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomsAgentCards ON dbo.Customers.CustomsAgentId = CustomsAgentCards.Id LEFT OUTER JOIN
dbo.Cards AS MediatorCards ON dbo.Customers.MediatorId = MediatorCards.Id LEFT OUTER JOIN
dbo.Contacts AS CreatedByUserContacts ON dbo.Cards.CreatedByUserId = CreatedByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS UpdatedByUserContacts ON dbo.Cards.UpdatedByUserId = UpdatedByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS PrimaryContacts ON dbo.Cards.PrimaryContactId = PrimaryContacts.Id LEFT OUTER JOIN
dbo.Regions ON dbo.Customers.RegionId = dbo.Regions.Id LEFT OUTER JOIN
dbo.CustomerStatus ON dbo.Customers.CustomerStatusCode = dbo.CustomerStatus.Code LEFT OUTER JOIN
dbo.Users as SalesmanUsers on dbo.customers.SalesmanUserId = SalesmanUsers.Id LEFT OUTER JOIN
dbo.Countries as MainAddressCountries on dbo.Cards.CountryId = MainAddressCountries.Id LEFT OUTER JOIN
dbo.CustomerSizes ON dbo.Customers.CustomerSizeId = CustomerSizes.Id LEFT OUTER JOIN
dbo.LeadSources ON dbo.Customers.LeadSourceId = LeadSources.Id LEFT OUTER JOIN
dbo.Currencies as InvoiceCurrency on dbo.Cards.InvoiceCurrencyId = InvoiceCurrency.Id LEFT OUTER JOIN
dbo.Contacts AS ActivatedByUserContacts ON dbo.Customers.ActivatedByUserId = ActivatedByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SetAsInactiveByUserContacts ON dbo.Customers.SetAsInactiveByUserId = SetAsInactiveByUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS ActivationRequestedByUserContacts ON dbo.Customers.ActivationRequestedByUserId = ActivationRequestedByUserContacts.Id
where dbo.Cards.PartnerTypeId <> ''AC''');


-- DataView Script From ShipmentDataView.dxml
EXEC('IF (OBJECT_ID(''[dbo].[ShipmentDataView]'', ''V'') IS NOT NULL) BEGIN DROP VIEW [dbo].[ShipmentDataView] END');
EXEC('CREATE VIEW [dbo].[ShipmentDataView]
AS
SELECT        dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber,dbo.Shipments.DeclarationNumber,dbo.Shipments.IncludesCustoms,dbo.Shipments.CustomsClearanceDate,dbo.Shipments.DeclarationDate, dbo.Shipments.ShipperReference1, dbo.Shipments.ARInvoiceIssued, dbo.Shipments.CreditNoteIssued,dbo.Shipments.ValueOfGoods,
dbo.ShipmentMasterDatas.Tenant AS ShipmentMasterDataTenant, dbo.ShipmentMasterDatas.Id AS ShipmentMasterDataId, dbo.shipments.SLAC,
dbo.ShipmentMasterDatas.MainCarriageFromPortId, dbo.ShipmentMasterDatas.MainCarriageToPortId, dbo.Shipments.FirstARInvoiceApprovalDate,
dbo.Shipments.LastFinalDestination, [dbo].[Shipments].[From], [dbo].[Shipments].[To], dbo.Shipments.Origin, dbo.Shipments.FirstPickupETA, dbo.Shipments.FirstPickupETD,
dbo.Shipments.ExceptionDescription,dbo.Shipments.ValueOfGoodsCurrencyId,dbo.Shipments.ExceptionDate, dbo.Shipments.HasException, dbo.Shipments.IsManifestSentToAgent, dbo.Shipments.ManifestLastSharingDate,
dbo.Shipments.AgentSharedManifestRef , dbo.Shipments.ExceptionResolvedDescription,dbo.Shipments.LastExceptionDescription, dbo.Shipments.OperationalDate, dbo.ShipmentMasterDatas.CutoffDate,
dbo.Shipments.ComputedStatusId, dbo.Shipments.ComputedStatusDate, dbo.Shipments.OperationalCloseDate, dbo.Shipments.AccountingCloseDate,
dbo.Shipments.CustomConnectToShipment, dbo.Shipments.ForeignPartnerCountryCode, dbo.Shipments.IsNewARInvoiceBlocked,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId, dbo.ShipmentMasterDatas.Transshipment3CarrierId,
dbo.ShipmentMasterDatas.Transshipment2CarrierId, dbo.ShipmentMasterDatas.Transshipment1CarrierId, dbo.ShipmentMasterDatas.MainCarriageCarrierId,
dbo.ShipmentMasterDatas.MainCarriageIsFromStack, dbo.ShipmentMasterDatas.Transshipment3AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment2AdditionalMAWBOBLBL, dbo.ShipmentMasterDatas.Transshipment1AdditionalMAWBOBLBL,
dbo.ShipmentMasterDatas.Transshipment3VesselId, dbo.ShipmentMasterDatas.Transshipment2VesselId, dbo.ShipmentMasterDatas.Transshipment1VesselId,
dbo.ShipmentMasterDatas.MainCarriageVesselId, dbo.ShipmentMasterDatas.BookingConfirmedBy, dbo.ShipmentMasterDatas.BookingConfirmationNotes,
dbo.ShipmentMasterDatas.BookingConfirmationNumber, dbo.ShipmentMasterDatas.MAWBOBLDate, dbo.ShipmentMasterDatas.Transshipment3CarrierNumber,
dbo.ShipmentMasterDatas.Transshipment3ETA, dbo.ShipmentMasterDatas.Transshipment3ETD, dbo.ShipmentMasterDatas.Transshipment3ATA,
dbo.ShipmentMasterDatas.Transshipment3ATD, dbo.ShipmentMasterDatas.Transshipment3ToPortId, dbo.ShipmentMasterDatas.Transshipment3FromPortId,
dbo.ShipmentMasterDatas.Transshipment2CarrierNumber, dbo.ShipmentMasterDatas.Transshipment2ETA, dbo.ShipmentMasterDatas.Transshipment2ETD,
dbo.ShipmentMasterDatas.Transshipment2ATA, dbo.ShipmentMasterDatas.Transshipment2ATD, dbo.ShipmentMasterDatas.Transshipment2ToPortId,
dbo.ShipmentMasterDatas.Transshipment2FromPortId, dbo.ShipmentMasterDatas.Transshipment1CarrierNumber, dbo.ShipmentMasterDatas.Transshipment1ETA,
dbo.ShipmentMasterDatas.Transshipment1ETD, dbo.ShipmentMasterDatas.Transshipment1ATA, dbo.ShipmentMasterDatas.Transshipment1ATD,
dbo.ShipmentMasterDatas.Transshipment1ToPortId, dbo.ShipmentMasterDatas.Transshipment1FromPortId, dbo.ShipmentMasterDatas.Master,
dbo.ShipmentMasterDatas.MainCarriageCarrierNumber, dbo.ShipmentMasterDatas.MainCarriageETD, dbo.ShipmentMasterDatas.MainCarriageETA, dbo.ShipmentMasterDatas.TrailerNumber,
dbo.ShipmentMasterDatas.MainCarriageFromAddressId, dbo.ShipmentMasterDatas.MainCarriageToAddressId,
dbo.ShipmentMasterDatas.MainCarriageATA, dbo.ShipmentMasterDatas.MainCarriageATD, dbo.Shipments.ShipperReference2,
dbo.ShipmentMasterDatas.InterlineId, dbo.ShipmentMasterDatas.ManifestReason, dbo.ShipmentMasterDatas.ManifestStatusCode, dbo.ShipmentMasterDatas.AirlinePrefix,
dbo.ShipmentMasterDatas.ImportManifest,
dbo.Shipments.MasterShipmentDataId, dbo.Shipments.ShipmentLevelCode, dbo.Shipments.NextETA, dbo.Shipments.NextETD,
dbo.Shipments.NumberOfInsidePackages, dbo.Shipments.NumberOfInsidePackagesDetails, dbo.Shipments.RegistryDate, dbo.Shipments.IsAssembly, dbo.Shipments.FirstOperationalCloseDate,
dbo.Shipments.NextLegCode, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Shipments.OpenReceivablesInProfitCurrency, dbo.Shipments.FirstAccountingCloseDate,
dbo.Shipments.ProfitInProfitCurrency, dbo.Shipments.EstimateProfitInProfitCurrency, dbo.Shipments.ProfitCurrencyId, dbo.Shipments.SCI,
dbo.Shipments.AWBHandlingInformation, dbo.Shipments.AWBInsurrenceValue, dbo.Shipments.AWBAccountingInformation, dbo.Shipments.ProductCode,
dbo.Shipments.AWBDeclaredValueForCustoms, dbo.Shipments.AWBDeclaredValueForCarriage, dbo.Shipments.AWBCarrierTarrifReference,
dbo.Shipments.FreightForwarderContactId, dbo.Shipments.FreightForwarderAddressId, dbo.Shipments.CustomAgentExportContactId,
dbo.Shipments.CustomAgentExportAddressId, dbo.Shipments.ShipmentCustomerTypeCode, dbo.Shipments.CustomerReference1, dbo.Shipments.CustomerReference2, dbo.Shipments.CustomerContactId,
dbo.Shipments.CustomerAddressId, dbo.Shipments.CustomerId, dbo.Shipments.FreightForwarderReference, dbo.Shipments.FreightForwarderId,
dbo.Shipments.CustomAgentExportReference, dbo.Shipments.CustomAgentExportId, dbo.Shipments.CustomAgentImportReference,
dbo.Shipments.ConsigneeAddressOneTime, dbo.Shipments.ShipperAddressOneTime, dbo.Shipments.EstimateProfitInLocalCurrency, dbo.Shipments.AWBCurrencyId,
dbo.Shipments.OrderChargeableWeight, dbo.Shipments.OnCarriageCarrierId, dbo.Shipments.PreCarriageCarrierId, dbo.Shipments.ProfitInLocalCurrency,
dbo.Shipments.AccountedReceivablesInLocalCurrency, dbo.Shipments.OpenReceivablesInLocalCurrency, dbo.Shipments.LastUpdateDate,
dbo.Shipments.UpdatedByUserId, dbo.Shipments.IsCancelled, dbo.Shipments.CancelledDate, dbo.Shipments.IsAccountingClosed, dbo.Shipments.ShipmentPayableStatusCode,
dbo.Shipments.ShipmentReceivableStatusCode, dbo.Shipments.QuoteId, dbo.Shipments.ShipmentDeliveryIndex, dbo.Shipments.ShipmentPickUpIndex,
dbo.Shipments.LTCWEdited, dbo.Shipments.OnCarriageVesselId, dbo.Shipments.PreCarriageVesselId, dbo.Shipments.DangerousMaterialDescription,
dbo.Shipments.DangerousPackagingGroup, dbo.Shipments.DangerousClassNumber, dbo.Shipments.DangerousUnNumber, dbo.Shipments.DangerousIMDGCode,
dbo.Shipments.DangerousFlashPoint, dbo.Shipments.AWBFreightAmountPrepaid, dbo.Shipments.AWBFreightAmountCollect, dbo.Shipments.IsDangerous,
dbo.Shipments.MainHarmonize, dbo.Shipments.VolumeUnitCode, dbo.Shipments.ChargeableWeightEdited,
dbo.Shipments.GrossWeightEdited, dbo.Shipments.Ratio, dbo.Shipments.PackagesQuantity, dbo.Shipments.ColoaderId,
dbo.Shipments.NumberOfPackages, dbo.Shipments.NumberOfContainers, dbo.Shipments.VolumeInCBM, dbo.Shipments.NumberOfFollowUps,
dbo.Shipments.DimensionsUnitCode, dbo.Shipments.AgentReference2, dbo.Shipments.AgentReference1, dbo.Shipments.ShipperNotExporterContactId,
dbo.Shipments.ConsigneeNotImporterContactId, dbo.Shipments.ConsigneeNotImporterAddressId, dbo.Shipments.ShipperNotExporterAddressId,
dbo.Shipments.ConsigneeNotImporterId, dbo.Shipments.ShipperNotExporterId, dbo.Shipments.OtherPrepaidCollectId, dbo.Shipments.FreightPrepaidCollectId,
dbo.Shipments.OrderIsDangerouseGoods, dbo.Shipments.BookingNumberOfPackages, dbo.Shipments.BookingVolume, dbo.Shipments.OrderGrossWeight,
dbo.Shipments.Field10, dbo.Shipments.Field9, dbo.Shipments.Field8, dbo.Shipments.Field7, dbo.Shipments.Field6, dbo.Shipments.Field5, dbo.Shipments.Field4,
dbo.Shipments.Field3, dbo.Shipments.Field2, dbo.Shipments.Field1, dbo.Shipments.GrossWeight, dbo.Shipments.ChargeableWeight,
dbo.Shipments.Field11, dbo.Shipments.Field12, dbo.Shipments.Field13, dbo.Shipments.Field14, dbo.Shipments.Field15,
dbo.Shipments.Field16, dbo.Shipments.Field17, dbo.Shipments.Field18, dbo.Shipments.Field19, dbo.Shipments.Field20,
dbo.Shipments.Field21, dbo.Shipments.Field22, dbo.Shipments.Field23, dbo.Shipments.Field24, dbo.Shipments.Field25,
dbo.Shipments.Field26, dbo.Shipments.Field27, dbo.Shipments.Field28, dbo.Shipments.Field29, dbo.Shipments.Field30,
dbo.Shipments.Field31, dbo.Shipments.Field32, dbo.Shipments.Field33, dbo.Shipments.Field34, dbo.Shipments.Field35,
dbo.Shipments.Field36, dbo.Shipments.Field37, dbo.Shipments.Field38, dbo.Shipments.Field39, dbo.Shipments.Field40,
dbo.Shipments.IsOperationalClosed, dbo.Shipments.ConsigneeContactId, dbo.Shipments.AgentContactId, dbo.Shipments.AgentAddressId,
dbo.Shipments.CustomAgentImportAddressId, dbo.Shipments.CustomAgentImportContactId, dbo.Shipments.ShipperContactId, dbo.Shipments.Notify2ContactId,
dbo.Shipments.Notify1ContactId, dbo.Shipments.Notify2AddressId, dbo.Shipments.Notify1AddressId, dbo.Shipments.PreCarriageETD,
dbo.Shipments.PreCarriageETA, dbo.Shipments.OnCarriageETA, dbo.Shipments.OnCarriageETD, dbo.Shipments.AgentId,dbo.Shipments.AgentComputed, dbo.Shipments.OnCarriageCarrierNumber,
dbo.Shipments.OnCarriageATA, dbo.Shipments.OnCarriageATD, dbo.Shipments.OnCarriageToPortId, dbo.Shipments.OnCarriageFromPortId,
dbo.Shipments.OnCarriageTransportModeId, dbo.Shipments.PreCarriageCarrierNumber, dbo.Shipments.PreCarriageATA, dbo.Shipments.PreCarriageATD,
dbo.Shipments.PreCarriageToPortId, dbo.Shipments.PreCarriageFromPortId, dbo.Shipments.PreCarriageTransportModeId, dbo.Shipments.HAWBDate,
dbo.Shipments.DescriptionOfGoods, dbo.Shipments.Notes, dbo.Shipments.DirectionId, dbo.Shipments.TransportModeId, dbo.Shipments.ConsigneeAddressId,
dbo.Shipments.ShipperAddressId, dbo.Shipments.Notify2Id, dbo.Shipments.Notify1Id, dbo.Shipments.ConsigneeId, dbo.Shipments.CustomAgentImportId,
dbo.Shipments.ShipperId, dbo.Shipments.ShipmentTypeId, dbo.Shipments.DepartmentId, dbo.Shipments.CreateDateTime, dbo.Shipments.SalesmanUserId,
dbo.Shipments.IncotermId, dbo.Shipments.BranchId, dbo.Shipments.House, dbo.Shipments.ConsigneeReference2, dbo.Shipments.ConsigneeReference1,
dbo.Shipments.ConsolidatorId,dbo.Shipments.ConsolidatorAddressId,dbo.Shipments.ConsolidatorContactId,dbo.Shipments.ConsolidatorReference,
dbo.Shipments.ShipmentSubTypeId,
dbo.Shipments.ARInvoices,
dbo.Shipments.NotInvoicedReceivablesAmount,
dbo.Shipments.ReleasingAgentId,
dbo.Shipments.ContainerLastStatusDate,
dbo.Shipments.Notify1Reference,
dbo.Shipments.Notify2Reference,
dbo.Shipments.ShipperNotExporterReference,
dbo.Shipments.ConsigneeNotImporterReference,
dbo.Shipments.ProjectNumber,
dbo.Shipments.INTTRALastStatusDate,
dbo.Shipments.INTTRASIError,
dbo.Shipments.INTTRASIStatusCode,
dbo.Shipments.INTTRASIStatusDate,
dbo.Shipments.INTTRABookingError,
dbo.Shipments.INTTRALastBookingResponse,
dbo.INTTRASIStatus.Name AS INTTRASIStatusName,
dbo.Shipments.CreatedByPartner AS CreatedByPartner,
dbo.Shipments.INTTRABookingStatusCode,
dbo.INTTRABookingStatuses.Name AS INTTRABookingStatusName,
dbo.Shipments.INTTRABookingTransStatusCode,
dbo.INTTRABookingTransStatuses.Name AS INTTRABookingTransStatusName,
dbo.Shipments.AccountManagerUserId,
dbo.Shipments.CustomsDeclarationNumber,
dbo.Shipments.ShipperName,dbo.Shipments.FBLIsFromStock,
dbo.Shipments.FreightRelease, dbo.Shipments.TerminalAvailable, dbo.Shipments.ISFDate, dbo.Shipments.ISFNumber, dbo.Shipments.ITDate, dbo.Shipments.ITNumber,
dbo.ShipmentMasterDatas.OBLTypeCode, dbo.ShipmentMasterDatas.DocumentsClosingDate,
dbo.Shipments.ShipperName as Shipper, dbo.Shipments.ENSNumber, dbo.Shipments.ENSDate, dbo.Shipments.WarehouseLegWarehouseId, dbo.Shipments.WarehouseLegAddressId, dbo.Shipments.WarehouseLegTerminalCode, dbo.Shipments.WarehouseLegExpectedEntryDate,
dbo.Shipments.WarehouseLegLastFreeDate,dbo.Shipments.WarehouseLegExpectedReleaseDate,dbo.Shipments.WarehouseLegActualReleaseDate, dbo.Shipments.WarehouseLegRemarks, dbo.Shipments.WarehouseLegActualEntryDate,dbo.Shipments.WarehouseLegReference,dbo.Shipments.WarehouseStorageFreeDays,
WarehouseLegCard.EnglishName AS WarehouseLegTerminalName,
dbo.Shipments.LastSharedEventId, dbo.Shipments.LastSharedEventLocation, dbo.Shipments.LastSharedEventNotes, dbo.Shipments.LastSharedEventDate,
dbo.EventTypes.EnglishName as LastSharedEventName,
WarehouseLegCard.CountryCode AS WarehouseLegAddressCountryCode,
WarehouseLegCard.CountryName AS WarehouseLegAddressCountryName,
--ShipperCards.EnglishName AS ShipperName,
MainCarriageFromPorts.Code AS MainCarriageFromPortCode, MainCarriageToPorts.Code AS MainCarriageToPortCode,
Transshipment1FromPorts.Code AS Transshipment1FromPortCode, Transshipment1ToPorts.Code AS Transshipment1ToPortCode,
Transshipment2FromPorts.Code AS Transshipment2FromPortCode, Transshipment2ToPorts.Code AS Transshipment2ToPortCode,
Transshipment3FromPorts.Code AS Transshipment3FromPortCode, Transshipment3ToPorts.Code AS Transshipment3ToPortCode,
PreCarriageFromPorts.Code AS PreCarriageFromPortCode, PreCarriageToPorts.Code AS PreCarriageToPortCode,
OnCarriageFromPorts.Code AS OnCarriageFromPortCode, OnCarriageToPorts.Code AS OnCarriageToPortCode,
MainCarriageToPorts.EnglishName AS MainCarriageToPortName, MainCarriageFromPorts.EnglishName AS MainCarriageFromPortName,
Transshipment1FromPorts.EnglishName AS Transshipment1FromPortName, Transshipment1ToPorts.EnglishName AS Transshipment1ToPortName,
Transshipment2ToPorts.EnglishName AS Transshipment2ToPortName, Transshipment2FromPorts.EnglishName AS Transshipment2FromPortName,
PreCarriageFromPorts.EnglishName AS PreCarriageFromPortName, OnCarriageFromPorts.EnglishName AS OnCarriageFromPortName,
PreCarriageToPorts.EnglishName AS PreCarriageToPortName, Transshipment3FromPorts.EnglishName AS Transshipment3FromPortName,
Transshipment3ToPorts.EnglishName AS Transshipment3ToPortName, OnCarriageToPorts.EnglishName AS OnCarriageToPortName,
CustomerCards.EnglishName AS CustomerName, CustomerCards.Notes AS CustomerNote,
ConsolidatorCards.EnglishName AS ConsolidatorName, ConsolidatorCards.Notes AS ConsolidatorNote,
FreightForwarderCards.EnglishName AS FreightForwarderName,
FreightForwarderCards.Notes AS FreightForwarderNote, ShipperCards.Notes AS ShipperNote, ReleasingAgentCards.EnglishName AS ReleasingAgentName,
ConsigneeCards.EnglishName AS ConsigneeName,ConsigneeCards.EnglishName AS Consignee, ConsigneeCards.Notes AS ConsigneeNote, AgentComputedCards.EnglishName AS AgentName,
AgentCards.Notes AS AgentNote, CustomAgentExportCards.EnglishName AS CustomAgentExportName, CustomAgentExportCards.Notes AS CustomAgentExportNote,
CustomAgentImportCards.EnglishName AS CustomAgentImportName, CustomAgentImportCards.Notes AS CustomAgentImportNote,
Notify1Cards.EnglishName AS Notify1Name, Notify1Cards.Notes AS Notify1Note, Notify2Cards.EnglishName AS Notify2Name, Notify2Cards.Notes AS Notify2Note,
ShipperNotExporterCards.EnglishName AS ShipperNotExporterName, ShipperNotExporterCards.Notes AS ShipperNotExporterNote,
ConsigneeNotImporterCards.EnglishName AS ConsigneeNotImporterName, ConsigneeNotImporterCards.Notes AS ConsigneeNotImporterNote,
ToPorts.Code AS ToPortCode, FromPorts.Code AS FromPortCode,
MainCarriageFromCountries.Code AS MainCarriageFromPortCountryCode, MainCarriageFromCountries.EnglishName AS MainCarriageFromPortCountryName,
MainCarriageToCountries.Code AS MainCarriageToPortCountryCode, MainCarriageToCountries.EnglishName AS MainCarriageToPortCountryName,
Transshipment1FromCountries.Code AS Transshipment1FromPortCountryCode,
Transshipment1FromCountries.EnglishName AS Transshipment1FromPortCountryName,
Transshipment2FromCountries.Code AS Transshipment2FromPortCountryCode,
Transshipment2FromCountries.EnglishName AS Transshipment2FromPortCountryName,
Transshipment3FromCountries.Code AS Transshipment3FromPortCountryCode,
Transshipment3FromCountries.EnglishName AS Transshipment3FromPortCountryName, PreCarriageFromCountries.Code AS PreCarriageFromPortCountryCode,
PreCarriageFromCountries.EnglishName AS PreCarriageFromPortCountryName, OnCarriageFromCountries.Code AS OnCarriageFromPortCountryCode,
OnCarriageFromCountries.EnglishName AS OnCarriageFromPortCountryName, Transshipment1ToCountries.Code AS Transshipment1ToPortCountryCode,
Transshipment1ToCountries.EnglishName AS Transshipment1ToPortCountryName, OnCarriageToCountries.Code AS OnCarriageToPortCountryCode,
OnCarriageToCountries.EnglishName AS OnCarriageToPortCountryName, Transshipment2ToCountries.Code AS Transshipment2ToPortCountryCode,
Transshipment2ToCountries.EnglishName AS Transshipment2ToPortCountryName, PreCarriageToCountries.Code AS PreCarriageToPortCountryCode,
PreCarriageToCountries.EnglishName AS PreCarriageToPortCountryName, Transshipment3ToCountries.Code AS Transshipment3ToPortCountryCode,
Transshipment3ToCountries.EnglishName AS Transshipment3ToPortCountryName, MainCarriageCarrierCards.EnglishName AS MainCarriageCarrierName,
MainCarriageCarrierCards.Code AS MainCarriageCarrierCode, Transshipment1CarrierCards.EnglishName AS Transshipment1CarrierName,
Transshipment1CarrierCards.Code AS Transshipment1CarrierCode, Transshipment2CarrierCards.EnglishName AS Transshipment2CarrierName,
Transshipment2CarrierCards.Code AS Transshipment2CarrierCode, Transshipment3CarrierCards.EnglishName AS Transshipment3CarrierName,
Transshipment3CarrierCards.Code AS Transshipment3CarrierCode, PreCarriageCarrierCards.EnglishName AS PreCarriageCarrierName,
PreCarriageCarrierCards.Code AS PreCarriageCarrierCode, OnCarriageCarrierCards.EnglishName AS OnCarriageCarrierName,
OnCarriageCarrierCards.Code AS OnCarriageCarrierCode, dbo.NextLegs.Name AS NextLegName, dbo.Directions.Name AS DirectionName,
dbo.TransportModes.Name AS TransportModeName, dbo.ShipmentTypes.Name AS ShipmentTypeName,
dbo.ShipmentReceivableStatus.Name AS ShipmentReceivableStatusName, dbo.ShipmentPayableStatus.Name AS ShipmentPayableStatusName,
AWBCurrencies.Code AS AWBCurrencyCode, dbo.Branches.EnglishName AS BranchName,dbo.MoveTypes.MoveTypeEnglishName AS MoveTypeName ,
dbo.ShipmentLevels.Name AS ShipmentLevelName,
dbo.ShipmentSubTypes.Name AS ShipmentSubTypeName,
dbo.EntityStatus.Id AS ShipmentStatusId,
dbo.EntityStatus.Name AS ShipmentStatusName,
dbo.EntityStatus.StatusWeight AS ShipmentStatusWeight,
dbo.Shipments.StatusDate as ShipmentStatusDate,
dbo.Shipments.StatusLocation as ShipmentStatusLocation,
ShipmentMasterDataEntityStatus.Id AS ShipmentMasterDataStatusId,
ShipmentMasterDataEntityStatus.Name AS ShipmentMasterDataStatusName,
ShipmentMasterDataEntityStatus.StatusWeight AS ShipmentMasterDataStatusWeight,
dbo.ShipmentMasterDatas.StatusDate As ShipmentMasterDataStatusDate,
dbo.ShipmentMasterDatas.StatusLocation as ShipmentMasterDataStatusLocation,
MainCarriageFinalDestinationPorts.Code AS MainCarriageFinalDestinationPortCode,
MainCarriageFinalDestinationPorts.EnglishName AS MainCarriageFinalDestinationPortName,
FinalDestinationCountries.Code AS MainCarriageFinalDestinationCountryCode,
FinalDestinationCountries.EnglishName AS MainCarriageFinalDestinationCountryName,
dbo.ShipmentMasterDatas.MasterShipmentNumber,
dbo.ShipmentMasterDatas.CarrierTransportDocumentNumber,
dbo.Shipments.SearchFields, MainCarriageAirline.Prefix AS MainCarriageAirlinePrefix, dbo.Shipments.CreatedByUserId,dbo.Shipments.OperationalClosedByUserId,
dbo.Shipments.OpenPayablesInLocalCurrency, dbo.Shipments.AccountedPayablesInLocalCurrency, dbo.Shipments.OpenPayablesInProfitCurrency,
dbo.Shipments.AccountedPayablesInProfitCurrency, dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, dbo.Shipments.GrossWeightPerStorageDays,dbo.Shipments.GrossWeightPerTon,
dbo.Shipments.GrossWeightUnitCode, dbo.Shipments.ChargeableWeightUnitCode, dbo.Shipments.OrderVolumetricWeight, dbo.Shipments.VolumetricWeight,
dbo.Shipments.Volume, dbo.Shipments.IssuingCarrierAgentId, dbo.Incoterms.Code AS IncotermCode,
dbo.FHLStatus.Code AS FHLStatusCode,
dbo.FHLStatus.Name AS FHLStatusName,
dbo.Shipments.FHLStatusDate,
dbo.FWBStatus.Code AS FWBStatusCode,
dbo.FWBStatus.Name AS FWBStatusName,
dbo.ShipmentMasterDatas.FWBStatusDate,
dbo.Shipments.LastSentByUserId,
dbo.CustomsTransmissionsStatus.Code AS LocalCustomsTransmissionsStatusCode,
dbo.CustomsTransmissionsStatus.Name AS LocalCustomsTransmissionsStatusName,
dbo.shipments.LocalCustomsTransmissionsStatusError,
dbo.Shipments.LocalCustomsTransmissionsStatusDate,
dbo.Shipments.LocalCustomsSentByUserId,
LocalCustomsSentByUser.EnglishName as LocalCustomsSentByUserName,
CargonautFHLStatus.Code AS CargonautFHLStatusCode,
CargonautFHLStatus.Name AS CargonautFHLStatusName,
dbo.Shipments.CargonautFHLStatusDate,
CargonautFWBStatus.Code AS CargonautFWBStatusCode,
CargonautFWBStatus.Name AS CargonautFWBStatusName,
dbo.ShipmentMasterDatas.CargonautFWBStatusDate,
dbo.Shipments.AWBPrint, CarrierLastStatuses.Name AS CarrierLastStatusName, dbo.Shipments.FNAReason, dbo.Shipments.Routing, dbo.ShipmentMasterDatas.TruckNumber,
dbo.Shipments.AsAgreedFreight, dbo.Shipments.AsAgreedOtherCharges, dbo.Shipments.AccountNumber,
dbo.Shipments.CarrierLastStatusCode, dbo.Shipments.CarrierLastStatusDate, dbo.Shipments.LastFSRStatusRequestDate,
dbo.Shipments.FinalArrivalDate, dbo.Shipments.EstimatedFinalArrivalDate, dbo.Shipments.ActualFinalArrivalDate, FromPortCountries.Code AS FromPortCountryCode,
ToPortCountries.Code AS ToPortCountryCode, dbo.Shipments.TEU, MainCarriageFromAddresses.City AS MainCarriageFromCity,
MainCarriageToAddresses.City AS MainCarriageToCity, MainCarriageFromAddressCountries.Code AS MainCarriageFromCountryCode,
MainCarriageToAddressCountries.Code AS MainCarriageToCountryCode, dbo.Shipments.ProfitExchangeRate, dbo.Shipments.CASSCode, dbo.Shipments.AMSBL,
dbo.Shipments.SpecialServicesTypeId, dbo.SpecialServicesTypes.Code AS SpecialServicesTypeCode,
dbo.SpecialServicesTypes.EnglishName AS SpecialServicesTypeName, dbo.Shipments.CustomFileId, dbo.Shipments.CustomFileNumber,
dbo.Shipments.NoFreightFile, ToPortCountries.EnglishName AS ToPortCountryName, FromPortCountries.EnglishName AS FromPortCountryName,
dbo.Vessels.EnglishName AS MainCarriageVesselName,dbo.Shipments.LastStatusLogDate As LastStatusLogDate,
AccountManagerUserContacts.EnglishName as AccountManagerUserName,
SalesmanUserContact.EnglishName as SalesmanUserName,
CreatedByUserContact.EnglishName as CreatedByUserName,
LastSentByUserContact.EnglishName as LastSentByUserName,
ShipmentComputedFields.IsMissingDocuments as IsMissingDocument,
ShipmentComputedFields.DocumentsSearchFields as DocumentsSearchFields,
dbo.Shipments.ForwarderShipmentNumber as ForwarderShipmentNumber,
dbo.Shipments.CustomerShipmentNumber as CustomerShipmentNumber,
ShipmentComputedFields.LastDocumentDateTime as LastDocumentDateTime,
HybridPartner.SmallLogoId as PartnerLogoId,
ShipmentComputedFields.MissingDocumentsCount as MissingDocumentsCount,
ShipmentComputedFields.MissingDocumentsNames as MissingDocsNames,
ShipmentComputedFields.RequestedDocumentsCount as RequestedDocumentsCount,
ShipmentComputedFields.IsRequestedDocuments as IsRequestedDocuments,
ShipmentComputedFields.IsDigitalSignRequired as IsDigitalSignRequired,
ShipmentComputedFields.IsDepositionRequired as IsDepositionRequired,
ShipmentComputedFields.CreatedFromDigital as CreatedFromDigital,
ShipmentComputedFields.ImporterDepositionRequestDetails as ImporterDepositionRequestDetails,
ShipmentComputedFields.NumberOfHouses as NumberOfHouses,
ShipmentAdditionalCloudDatas.DeclarationXmlData as DeclarationXmlData,
ShipmentAdditionalCloudDatas.IsImporterApprovalRequried as IsImporterApprovalRequried,
ShipmentAdditionalCloudDatas.ApprovedByUserName as ApprovedByUserName,
ShipmentAdditionalCloudDatas.ApproveDateTime as ApproveDateTime,
ShipmentAdditionalCloudDatas.VersionApproved as VersionApproved,
HybridPartner.Name as PartnerName,
HybridPartner.Id as ForwarderPartnerId,
dbo.ShipmentMasterDatas.DepartureArrivalFromDate as DepartureArrivalFromDate,
dbo.ShipmentMasterDatas.DepartureArrivalToDate as DepartureArrivalToDate,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationETA as MainCarriageFinalDestinationETA,
dbo.ShipmentMasterDatas.MainCarriageFinalDestinationATA as MainCarriageFinalDestinationATA,
CASE WHEN ( NOT ((dbo.ShipmentTypes.Name IS NULL) OR ((LEN(dbo.ShipmentTypes.Name)) = 0))) THEN CASE WHEN (dbo.ShipmentTypes.Name IS NULL) THEN N'''' ELSE dbo.ShipmentTypes.Name END + N'' '' + CASE WHEN (dbo.ShipmentLevels.Name IS NULL) THEN N'''' ELSE dbo.ShipmentLevels.Name END ELSE dbo.ShipmentLevels.Name END AS ShipmentType,
CASE WHEN ( NOT ((MainCarriageFromPortId IS NULL) OR ((LEN(MainCarriageFromPortId)) = 0))) THEN MainCarriageFromPortId ELSE dbo.Shipments.FromPortId END AS FromPortId,
CASE WHEN ( NOT ((MainCarriageToPortId IS NULL) OR ((LEN(MainCarriageToPortId)) = 0))) THEN MainCarriageToPortId ELSE dbo.Shipments.ToPortId END AS ToPortId,
CASE WHEN ( NOT ((MainCarriageFromPorts.Code IS NULL) OR ((LEN(MainCarriageFromPorts.Code)) = 0))) THEN MainCarriageFromPorts.Code ELSE FromPorts.Code END AS FromPort,
CASE WHEN ( NOT ((MainCarriageFromPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFromPorts.EnglishName)) = 0))) THEN MainCarriageFromPorts.EnglishName ELSE FromPorts.EnglishName END AS FromPortName,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.Code IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.Code)) = 0))) THEN MainCarriageFinalDestinationPorts.Code ELSE ToPorts.Code END AS ToPort,
CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.EnglishName)) = 0))) THEN MainCarriageFinalDestinationPorts.EnglishName ELSE ToPorts.EnglishName END AS ToPortName,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (MainCarriageCarrierCards.Code IS NULL) THEN N'''' ELSE MainCarriageCarrierCards.Code END + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''O'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (dbo.Vessels.EnglishName IS NULL) THEN N'''' ELSE dbo.Vessels.EnglishName END + N''/'' + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'''' ELSE MainCarriageCarrierNumber END WHEN (''I'' = dbo.Shipments.TransportModeId) THEN MainCarriageCarrierNumber END AS CarrierNumber,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN MainCarriageATA ELSE MainCarriageETA END AS MainCarriageExpectedOrActual,
CASE WHEN (MainCarriageATA IS NOT NULL) THEN N''ATA'' ELSE N''ETA'' END AS MainCarriageETAOrATA,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Id ELSE dbo.EntityStatus.Id END ELSE dbo.EntityStatus.Id END AS StatusId,
dbo.Shipments.ComputedStatusDate AS StatusDate,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Name ELSE dbo.EntityStatus.Name END ELSE dbo.EntityStatus.Name END AS StatusName,
CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusLocation ELSE dbo.Shipments.StatusLocation END ELSE dbo.Shipments.StatusLocation END AS StatusLocation,
CASE WHEN (''A'' = dbo.Shipments.TransportModeId) THEN CASE WHEN (( NOT ((AirlinePrefix IS NULL) OR ((LEN(AirlinePrefix)) = 0))) AND ( NOT ((Master IS NULL) OR ((LEN(Master)) = 0)))) THEN CASE WHEN (AirlinePrefix IS NULL) THEN N'''' ELSE AirlinePrefix END + N''-'' + CASE WHEN (Master IS NULL) THEN N'''' ELSE Master END ELSE N'''' END ELSE Master END AS LongMaster,
CAST( MissingDocumentsCount AS nvarchar(max)) + N'' Missing'' AS MissingDocumentsCountWords,
CASE WHEN (IsOperationalClosed = 1) THEN N''Archived'' ELSE N'''' END AS ArchivedText
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageFromAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN FromPortsStates.EnglishName
--ELSE MainCarriageFromPortsStates.EnglishName END) END AS MainCarriageFromState,
--CASE WHEN (dbo.Shipments.TransportModeId = ''I'' AND dbo.Shipments.DirectionId = ''D'') THEN MainCarriageToAddressesStates.EnglishName
--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = ''H'' AND dbo.Shipments.MasterShipmentDataId is null) THEN ToPortsStates.EnglishName
--ELSE MainCarriageFinalDestinationPortsStates.EnglishName END) END AS MainCarriageToState
FROM            dbo.Shipments LEFT OUTER JOIN
dbo.ShipmentMasterDatas ON dbo.ShipmentMasterDatas.Id = dbo.Shipments.MasterShipmentDataId LEFT OUTER JOIN
dbo.Ports AS MainCarriageFromPorts ON dbo.ShipmentMasterDatas.MainCarriageFromPortId = MainCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageToPorts ON dbo.ShipmentMasterDatas.MainCarriageToPortId = MainCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1FromPorts ON dbo.ShipmentMasterDatas.Transshipment1FromPortId = Transshipment1FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment1ToPorts ON dbo.ShipmentMasterDatas.Transshipment1ToPortId = Transshipment1ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2FromPorts ON dbo.ShipmentMasterDatas.Transshipment2FromPortId = Transshipment2FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment2ToPorts ON dbo.ShipmentMasterDatas.Transshipment2ToPortId = Transshipment2ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3FromPorts ON dbo.ShipmentMasterDatas.Transshipment3FromPortId = Transshipment3FromPorts.Id LEFT OUTER JOIN
dbo.Ports AS Transshipment3ToPorts ON dbo.ShipmentMasterDatas.Transshipment3ToPortId = Transshipment3ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS MainCarriageFinalDestinationPorts ON dbo.ShipmentMasterDatas.MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageFromPorts ON dbo.Shipments.PreCarriageFromPortId = PreCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS PreCarriageToPorts ON dbo.Shipments.PreCarriageToPortId = PreCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageFromPorts ON dbo.Shipments.OnCarriageFromPortId = OnCarriageFromPorts.Id LEFT OUTER JOIN
dbo.Ports AS OnCarriageToPorts ON dbo.Shipments.OnCarriageToPortId = OnCarriageToPorts.Id LEFT OUTER JOIN
dbo.Ports AS ToPorts ON dbo.Shipments.ToPortId = ToPorts.Id LEFT OUTER JOIN
dbo.Ports AS FromPorts ON dbo.Shipments.FromPortId = FromPorts.Id LEFT OUTER JOIN
dbo.Cards AS CustomerCards ON dbo.Shipments.CustomerId = CustomerCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsolidatorCards ON dbo.Shipments.ConsolidatorId = ConsolidatorCards.Id LEFT OUTER JOIN
dbo.Cards AS FreightForwarderCards ON dbo.Shipments.FreightForwarderId = FreightForwarderCards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperCards ON dbo.Shipments.ShipperId = ShipperCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeCards ON dbo.Shipments.ConsigneeId = ConsigneeCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentCards ON dbo.Shipments.AgentId = AgentCards.Id LEFT OUTER JOIN
dbo.Cards AS ReleasingAgentCards ON dbo.Shipments.ReleasingAgentId = ReleasingAgentCards.Id LEFT OUTER JOIN
dbo.Cards AS AgentComputedCards ON dbo.Shipments.AgentComputed = AgentComputedCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentExportCards ON dbo.Shipments.CustomAgentExportId = CustomAgentExportCards.Id LEFT OUTER JOIN
dbo.Cards AS CustomAgentImportCards ON dbo.Shipments.CustomAgentImportId = CustomAgentImportCards.Id LEFT OUTER JOIN
dbo.Cards AS Notify1Cards ON dbo.Shipments.Notify1Id = Notify1Cards.Id LEFT OUTER JOIN
dbo.Cards AS Notify2Cards ON dbo.Shipments.Notify2Id = Notify2Cards.Id LEFT OUTER JOIN
dbo.Cards AS ShipperNotExporterCards ON dbo.Shipments.ShipperNotExporterId = ShipperNotExporterCards.Id LEFT OUTER JOIN
dbo.Cards AS ConsigneeNotImporterCards ON dbo.Shipments.ConsigneeNotImporterId = ConsigneeNotImporterCards.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromCountries ON MainCarriageFromPorts.CountryId = MainCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToCountries ON MainCarriageToPorts.CountryId = MainCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1FromCountries ON Transshipment1FromPorts.CountryId = Transshipment1FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2FromCountries ON Transshipment2FromPorts.CountryId = Transshipment2FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3FromCountries ON Transshipment3FromPorts.CountryId = Transshipment3FromCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageFromCountries ON PreCarriageFromPorts.CountryId = PreCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageFromCountries ON OnCarriageFromPorts.CountryId = OnCarriageFromCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment1ToCountries ON Transshipment1ToPorts.CountryId = Transshipment1ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment2ToCountries ON Transshipment2ToPorts.CountryId = Transshipment2ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS Transshipment3ToCountries ON Transshipment3ToPorts.CountryId = Transshipment3ToCountries.Id LEFT OUTER JOIN
dbo.Countries AS PreCarriageToCountries ON PreCarriageToPorts.CountryId = PreCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS OnCarriageToCountries ON OnCarriageToPorts.CountryId = OnCarriageToCountries.Id LEFT OUTER JOIN
dbo.Countries AS FromPortCountries ON FromPorts.CountryId = FromPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS ToPortCountries ON ToPorts.CountryId = ToPortCountries.Id LEFT OUTER JOIN
dbo.Countries AS FinalDestinationCountries ON MainCarriageFinalDestinationPorts.CountryId = FinalDestinationCountries.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageCarrierCards ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment1CarrierCards ON dbo.ShipmentMasterDatas.Transshipment1CarrierId = Transshipment1CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment2CarrierCards ON dbo.ShipmentMasterDatas.Transshipment2CarrierId = Transshipment2CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS Transshipment3CarrierCards ON dbo.ShipmentMasterDatas.Transshipment3CarrierId = Transshipment3CarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS PreCarriageCarrierCards ON dbo.Shipments.PreCarriageCarrierId = PreCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS OnCarriageCarrierCards ON dbo.Shipments.OnCarriageCarrierId = OnCarriageCarrierCards.Id LEFT OUTER JOIN
dbo.Cards AS WarehouseLegCard ON dbo.Shipments.WarehouseLegWarehouseId = WarehouseLegCard.Id LEFT OUTER JOIN
dbo.NextLegs ON dbo.Shipments.NextLegCode = dbo.NextLegs.Code LEFT OUTER JOIN
dbo.Directions ON dbo.Shipments.DirectionId = dbo.Directions.Id LEFT OUTER JOIN
dbo.TransportModes ON dbo.Shipments.TransportModeId = dbo.TransportModes.Id LEFT OUTER JOIN
dbo.ShipmentTypes ON dbo.Shipments.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
dbo.ShipmentReceivableStatus ON dbo.Shipments.ShipmentReceivableStatusCode = dbo.ShipmentReceivableStatus.Code LEFT OUTER JOIN
dbo.ShipmentPayableStatus ON dbo.Shipments.ShipmentPayableStatusCode = dbo.ShipmentPayableStatus.Code LEFT OUTER JOIN
dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id LEFT OUTER JOIN
dbo.Currencies AS AWBCurrencies ON dbo.Shipments.AWBCurrencyId = AWBCurrencies.Id LEFT OUTER JOIN
dbo.Branches ON dbo.Shipments.BranchId = dbo.Branches.Id LEFT OUTER JOIN
dbo.ShipmentSubTypes ON dbo.Shipments.ShipmentSubTypeId = dbo.ShipmentSubTypes.Id LEFT OUTER JOIN
dbo.MoveTypes ON dbo.Shipments.MoveTypeId = dbo.MoveTypes.Id LEFT OUTER JOIN
dbo.ShipmentLevels ON dbo.Shipments.ShipmentLevelCode = dbo.ShipmentLevels.Code LEFT OUTER JOIN
dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id LEFT OUTER JOIN
dbo.Airlines AS MainCarriageAirline ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageAirline.Id LEFT OUTER JOIN
dbo.Incoterms ON dbo.Shipments.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
dbo.CustomsTransmissionsStatus ON dbo.Shipments.LocalCustomsTransmissionsStatusCode = dbo.CustomsTransmissionsStatus.Code LEFT OUTER JOIN
dbo.FHLStatus ON dbo.Shipments.FHLStatusCode = dbo.FHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus ON dbo.ShipmentMasterDatas.FWBStatusCode = dbo.FWBStatus.Code LEFT OUTER JOIN
dbo.FHLStatus AS CargonautFHLStatus ON dbo.Shipments.CargonautFHLStatusCode = CargonautFHLStatus.Code LEFT OUTER JOIN
dbo.FWBStatus AS CargonautFWBStatus ON dbo.ShipmentMasterDatas.CargonautFWBStatusCode = CargonautFWBStatus.Code LEFT OUTER JOIN
dbo.AWBStatus AS CarrierLastStatuses ON dbo.Shipments.CarrierLastStatusCode = CarrierLastStatuses.Code LEFT OUTER JOIN
dbo.INTTRASIStatus ON dbo.Shipments.INTTRASIStatusCode = dbo.INTTRASIStatus.Code LEFT OUTER JOIN
dbo.INTTRABookingTransStatuses ON dbo.Shipments.INTTRABookingTransStatusCode = dbo.INTTRABookingTransStatuses.Code LEFT OUTER JOIN
dbo.INTTRABookingStatuses ON dbo.Shipments.INTTRABookingStatusCode = dbo.INTTRABookingStatuses.Code LEFT OUTER JOIN
dbo.Addresses AS MainCarriageFromAddresses ON dbo.ShipmentMasterDatas.MainCarriageFromAddressId = MainCarriageFromAddresses.Id LEFT OUTER JOIN
dbo.Addresses AS MainCarriageToAddresses ON dbo.ShipmentMasterDatas.MainCarriageToAddressId = MainCarriageToAddresses.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageToPartners ON dbo.ShipmentMasterDatas.MainCarriageToPartnerId = MainCarriageToPartners.Id LEFT OUTER JOIN
dbo.Cards AS MainCarriageFromPartners ON dbo.ShipmentMasterDatas.MainCarriageFromPartnerId = MainCarriageFromPartners.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageFromAddressCountries ON MainCarriageFromAddresses.CountryId = MainCarriageFromAddressCountries.Id LEFT OUTER JOIN
dbo.Countries AS MainCarriageToAddressCountries ON MainCarriageToAddresses.CountryId = MainCarriageToAddressCountries.Id LEFT OUTER JOIN
dbo.SpecialServicesTypes ON dbo.Shipments.SpecialServicesTypeId = dbo.SpecialServicesTypes.Id LEFT OUTER JOIN
dbo.Vessels ON dbo.ShipmentMasterDatas.MainCarriageVesselId = dbo.Vessels.Id LEFT OUTER JOIN
dbo.Contacts AS AccountManagerUserContacts ON dbo.Shipments.AccountManagerUserId = AccountManagerUserContacts.Id LEFT OUTER JOIN
dbo.Contacts AS SalesmanUserContact ON dbo.Shipments.SalesmanUserId = SalesmanUserContact.Id LEFT OUTER JOIN
dbo.HybridPartners AS HybridPartner ON dbo.Shipments.ForwarderPartnerId = HybridPartner.Id LEFT OUTER JOIN
dbo.EventTypes ON dbo.Shipments.LastSharedEventId = dbo.EventTypes.Id LEFT OUTER JOIN
dbo.Contacts AS LastSentByUserContact ON dbo.Shipments.LastSentByUserId = LastSentByUserContact.Id LEFT OUTER JOIN
dbo.Contacts AS LocalCustomsSentByUser ON dbo.Shipments.LocalCustomsSentByUserId = LocalCustomsSentByUser.Id LEFT OUTER JOIN
dbo.Contacts AS CreatedByUserContact ON dbo.Shipments.CreatedByUserId = CreatedByUserContact.Id INNER JOIN
dbo.ShipmentComputedFields AS ShipmentComputedFields ON dbo.Shipments.Id = ShipmentComputedFields.Id INNER JOIN
dbo.ShipmentAdditionalCloudDatas AS ShipmentAdditionalCloudDatas ON dbo.Shipments.Id = ShipmentAdditionalCloudDatas.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromAddressesStates ON MainCarriageFromAddresses.StateId = MainCarriageFromAddressesStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageToAddressesStates ON MainCarriageToAddresses.StateId = MainCarriageToAddressesStates.Id LEFT OUTER JOIN
dbo.States AS FromPortsStates ON FromPorts.StateId = FromPortsStates.Id  LEFT OUTER JOIN
dbo.States AS ToPortsStates ON ToPorts.StateId = ToPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFromPortsStates ON MainCarriageFromPorts.StateId = MainCarriageFromPortsStates.Id LEFT OUTER JOIN
dbo.States AS MainCarriageFinalDestinationPortsStates ON MainCarriageFinalDestinationPorts.StateId = MainCarriageFinalDestinationPortsStates.Id');


-- Procedure Script From usp_UpdateCardSearchFunction.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateCardSearchFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateCardSearchFunction] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateCardSearchFunction]
(
@CardId varchar(15)
)
AS
declare  @Tenant int
declare  @EnglishName varchar(70)
declare  @LocalName nvarchar(100)
declare  @VatNumber varchar(20)
declare  @CityName nvarchar(25)
declare  @CountryName varchar(120)
declare  @Code varchar(15)
declare  @ReceivablesAccountingCard varchar(25)
declare  @PayablesAccountingCard varchar(25)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
if (@CardId is not null)
begin
delete CardSearches where CardId = @CardId
select
@Tenant = Tenant,
@Code = Code,
@EnglishName = EnglishName,
@LocalName = LocalName,
@VatNumber = VatNumber,
@CityName = CityName,
@CountryName =CountryName,
@ReceivablesAccountingCard = ReceivablesAccountingCard,
@PayablesAccountingCard = PayablesAccountingCard,
@CreateDate = CreateDate,
@UpdateDate = UpdateDate,
@PartnerTypeId = PartnerTypeId,
@InActive = InActive
from Cards
where Id = @CardId
set @Weight = 0
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
if (@Code is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@Code , 90 , 90) t where KeyWord !='' '' end
if (@EnglishName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@EnglishName , 100 , 90) t where KeyWord !='' '' end
if (@LocalName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@LocalName , 100 , 90) t where KeyWord !='' '' end
if (@VatNumber is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@VatNumber , 100 , 100) t where KeyWord !='' '' end
if (@CountryName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CountryName , 50 , 50) t where KeyWord !='' '' end
if (@CityName is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CityName , 40 , 40) t where KeyWord !='' '' end
if (@ReceivablesAccountingCard is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ReceivablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
if (@PayablesAccountingCard is not null)	begin 		 insert into CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@PayablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
end');


-- Procedure Script From DeleteOldAPILogs.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldAPILogs]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldAPILogs] END');
EXEC('create procedure [dbo].[DeleteOldAPILogs]
as
begin
IF OBJECT_ID(''dbo.TempDeletedAPILogs'') IS NOT NULL
DROP TABLE TempDeletedAPILogs
SELECT * INTO TempDeletedAPILogs
FROM (SELECT top(1000) Id
FROM APILogs
WHERE CreateDate < GETDATE() - 90) AS t
DELETE FROM APILogsData WHERE Id IN (SELECT Id FROM TempDeletedAPILogs)
DELETE FROM APILogs WHERE Id IN (SELECT Id FROM TempDeletedAPILogs)
end');


-- Procedure Script From usp_ComputeHouseShipmentStatusFunction.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ComputeHouseShipmentStatusFunction]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ComputeHouseShipmentStatusFunction] END');
EXEC('Create PROCEDURE [dbo].[usp_ComputeHouseShipmentStatusFunction]
(
@ShipmentId varchar(15)
)
AS
declare @MasterDataId as varchar(15)
declare @CustomFileId as varchar(15)
declare @ComputedStatusId as varchar(15)
declare @ShipmentStatusId as varchar(35)
declare @ShipmentDeclarationNumber as varchar(35)
declare @ShipmentStatusWeight as int
declare @CustomsDeclarationNumber as varchar(35)
declare @ComputedStatusDate as datetime
declare @MasterStatusWeight as int
declare @MasterStatusId as varchar(15)
declare @Tenant as int
declare @MasterStatusDate as datetime
declare @CustomFileStatusDate as datetime
declare @CustomStatusWeight as int
declare @CustomStatusId as varchar(15)
declare @SearchFields as varchar(1000)
if (@ShipmentId is not null)
begin
select
@Tenant = Tenant,
@MasterDataId = MasterShipmentDataId,
@ShipmentStatusId = StatusId,
@CustomFileId = CustomFileId,
@ComputedStatusDate = StatusDate,
@ShipmentDeclarationNumber =CustomsDeclarationNumber,
@ComputedStatusId = StatusId,
@SearchFields = SearchFields
from Shipments
where Id = @ShipmentId
set @ShipmentStatusWeight = (select StatusWeight from EntityStatus where Id = @ShipmentStatusId AND Tenant = @Tenant)
if(@MasterDataId is not null)
Begin
select @MasterStatusId = StatusId, @MasterStatusDate = StatusDate  from ShipmentMasterDatas  where  Id = @MasterDataId AND Tenant = @Tenant
set @MasterStatusWeight = (select StatusWeight from EntityStatus where Id = @MasterStatusId AND Tenant = @Tenant)
if(@MasterStatusWeight > @ShipmentStatusWeight)
begin
set @ShipmentStatusWeight = @MasterStatusWeight
set @ComputedStatusId = @MasterStatusId
set @ComputedStatusDate = @MasterStatusDate
end
End
if (@CustomFileId is not null)
Begin
select @CustomsDeclarationNumber = CustomsDeclarationNumber ,@CustomStatusId = StatusId , @CustomFileStatusDate = StatusDate from Shipments where Id = @CustomFileId AND Tenant = @Tenant
set @CustomStatusWeight = (select StatusWeight from EntityStatus where Id = @CustomStatusId AND Tenant = @Tenant)
if(@CustomStatusWeight > @ShipmentStatusWeight)
begin
set @ShipmentStatusWeight = @CustomStatusWeight
set @ComputedStatusId = @CustomStatusId
set @ComputedStatusDate = @CustomFileStatusDate
end
END
if(@CustomsDeclarationNumber is not null and (@ShipmentDeclarationNumber is null or   @ShipmentDeclarationNumber !=@CustomsDeclarationNumber   ))
begin
set @SearchFields = left((@SearchFields + '','' + @CustomsDeclarationNumber ),1000);
update Shipments set
ComputedStatusDate =@ComputedStatusDate ,
ComputedStatusId=  @ComputedStatusId ,
CustomsDeclarationNumber =@CustomsDeclarationNumber ,
SearchFields = @SearchFields
where Id = @ShipmentId and Tenant = @Tenant
end
else
begin
update Shipments set ComputedStatusDate =@ComputedStatusDate ,
ComputedStatusId=  @ComputedStatusId
where Id = @ShipmentId and Tenant = @Tenant
end
end');


-- Procedure Script From usp_ComputeShipmentStatus.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_ComputeShipmentStatus]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_ComputeShipmentStatus] END');
EXEC('Create PROCEDURE [dbo].[usp_ComputeShipmentStatus]
(
@ShipmentId varchar(15)
)
AS
declare @Tenant as int
declare @MasterDataId as varchar(15)
declare @CustomFileId as varchar(15)
declare @ShipmentLevelCode as varchar(1)
declare @ShipmentStatusId as varchar(15)
declare @ShipmentStatusWeight as int
declare @ShipmentDeclarationNumber as varchar(35)
declare @CustomDeclarationNumber as varchar(35)
declare @HouseId   as varchar(15)
declare @CustomId as varchar(15)
declare @IsConnect as bit
declare @ComputedStatusDate as datetime
declare @ComputedStatusId as varchar(15)
select
@Tenant = Shipments.Tenant,
@MasterDataId = Shipments.MasterShipmentDataId,
@ShipmentLevelCode= Shipments.ShipmentLevelCode,
@CustomFileId = Shipments.CustomFileId,
@ComputedStatusDate = CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusDate ELSE dbo.Shipments.StatusDate END ELSE dbo.Shipments.StatusDate END ,
@ShipmentDeclarationNumber =CustomsDeclarationNumber,
@ComputedStatusId = CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Id ELSE dbo.EntityStatus.Id END ELSE dbo.EntityStatus.Id END
from Shipments
LEFT OUTER JOIN   dbo.ShipmentMasterDatas ON dbo.ShipmentMasterDatas.Id = Shipments.MasterShipmentDataId
LEFT OUTER JOIN dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id
LEFT OUTER JOIN dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id
where Shipments.Id = @ShipmentId
set  @IsConnect = 0
if(@ShipmentLevelCode = ''C'')
BEGIN
DECLARE HousesCursor CURSOR READ_ONLY
FOR
SELECT Id
From Shipments
where ShipmentLevelCode = ''H'' AND MasterShipmentDataId = @MasterDataId
OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
WHILE @@FETCH_STATUS = 0
BEGIN
begin
EXECUTE usp_ComputeHouseShipmentStatusFunction  @HouseId
end
FETCH NEXT FROM HousesCursor INTO  @HouseId
END
CLOSE HousesCursor
DEALLOCATE HousesCursor
END
if(@ShipmentLevelCode = ''A'')
BEGIN
DECLARE HousesCursor CURSOR READ_ONLY
FOR
SELECT Id
From Shipments
where CustomFileId = @ShipmentId
OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @CustomId
WHILE @@FETCH_STATUS = 0
BEGIN
begin
set @IsConnect = 1
EXECUTE usp_ComputeHouseShipmentStatusFunction  @CustomId
end
FETCH NEXT FROM HousesCursor INTO @CustomId
END
CLOSE HousesCursor
DEALLOCATE HousesCursor
END
if(@ShipmentLevelCode =''A'' or @ShipmentLevelCode =''C'')
begin
update Shipments set CustomConnectToShipment = @IsConnect , ComputedStatusDate =@ComputedStatusDate , ComputedStatusId=  @ComputedStatusId where Id = @ShipmentId and Tenant = @Tenant
set @IsConnect = 0
end
if(@ShipmentLevelCode = ''H''  OR @ShipmentLevelCode = ''D'')
begin
EXECUTE usp_ComputeHouseShipmentStatusFunction  @ShipmentId
end');


-- General Script From 202009041551_UpdateDisplayNumberFieldOnCards.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
Update  Cards set GLAccountDisplayNumber = (select GLAccounts.DisplayNumber from GLAccounts WHERE GLAccounts.Id = Cards.GLAccountId) where Cards.GLAccountId is not  null and Cards.GLAccountDisplayNumber is null
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009041551_UpdateDisplayNumberFieldOnCards.sxml', GETDATE(), 'Update  Cards set GLAccountDisplayNumber = (select GLAccounts.DisplayNumber from GLAccounts WHERE GLAccounts.Id = Cards.GLAccountId) where Cards.GLAccountId is not  null and Cards.GLAccountDisplayNumber is null', DATEDIFF(MS,@StartTime,@EndTime), '0dd14e3affb7cc353eefe3be4d48debb', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008161345_RemoveCHWFeatureToggle.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
DELETE FROM FeatureToggles WHERE ToggleCode = 'CWH'
DELETE FROM Toggles WHERE Code = 'CWH'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008161345_RemoveCHWFeatureToggle.sxml', GETDATE(), 'DELETE FROM FeatureToggles WHERE ToggleCode = ''CWH''
DELETE FROM Toggles WHERE Code = ''CWH''', DATEDIFF(MS,@StartTime,@EndTime), 'eb5d39db9c06a115d397bc8c48402cad', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From FillShipmentSearchFields.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
--If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
--Begin
--    Drop Table #tempTable
--End
--If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
--Begin
--    Drop Table #temp_Shipments
--End
--CREATE TABLE #temp_Shipments (
--	Id varchar(15) not null ,
--    SearchFields nvarchar(4000)  null,
--    )
--select Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
--		StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
--		FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
--		OnCarriageFromPortId, OnCarriageToPortId,
--		House, CustomFileNumber, AWBCarrierTarrifReference,
--		AgentId, AgentReference1, AgentReference2,
--		ShipperId, ShipperReference1, ShipperReference2,
--		ConsigneeId, ConsigneeReference1, ConsigneeReference2,
--		CustomerId, CustomerReference1, CustomerReference2,
--		Notify1Id, Notify2Id, IssuingCarrierAgentId,
--		CustomAgentImportId, CustomAgentImportReference,
--		CustomAgentExportId, CustomAgentExportReference,
--		ShipperNotExporterId, ConsigneeNotImporterId,
--		FreightForwarderId, FreightForwarderReference,
--		ConsolidatorId, ConsolidatorReference,
--		Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
--		CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
--		ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
--		AMSBL, WarehouseLegReference
--	into #tempTable
--	FROM Shipments
--SET NOCOUNT ON
--declare @MySearchFields as nvarchar(4000)
--declare @PortsTable table
--(
--  Id varchar(15) not null
--)
--declare @PartnersTable table
--(
--  Id varchar(15) not null
--)
--declare @ReferencesTable table
--(
--  Reference varchar(50) not null
--)
---- Shipment fields
--BEGIN
--declare @Id as varchar(15)
--declare @Tenant as int
--declare @ShipmentNumber as varchar(15)
--declare @ShipmentLevelCode as varchar(1)
--declare @TransportModeId as varchar(1)
--declare @DirectionId as varchar(1)
--declare @StatusId as varchar(15)
--declare @StatusCode as varchar(4)
--declare @StatusName as varchar(40)
--declare @QuoteId as varchar(15)
--declare @QuoteNumber as varchar(15)
--declare @SalesmanUserId as varchar(15)
--declare @SalesmanUserName as varchar(60)
--declare @MasterShipmentDataId as varchar(15)
--declare @House as varchar(20)
--declare @CustomFileNumber as varchar(15)
--declare @AWBCarrierTarrifReference as varchar(25)
--declare @CustomsDeclarationNumber as varchar(35)
--declare @ForwarderShipmentNumber as varchar(15)
--declare @TransportDocumentNumber as varchar(50)
--declare @ImportManifest as varchar(50)
--declare @BookingConfirmationNumber as varchar(25)
--declare @CarrierTransportDocumentNumber as varchar(50)
--declare @ProjectNumber as varchar(100)
--declare @AMSBL as nvarchar(17)
--declare @WarehouseLegReference as nvarchar(50)
--END
---- Ports Firlds
--BEGIN
--declare @PortId as varchar(15)
--declare @PortCode as varchar(3)
--declare @PortName as varchar(40)
--declare @PortCountryCode as varchar(2)
--declare @PortCountryName as varchar(120)
--declare @FromPortId as varchar(15)
--declare @ToPortId as varchar(15)
--declare @PreCarriageFromPortId as varchar(15)
--declare @PreCarriageToPortId as varchar(15)
--declare @OnCarriageFromPortId as varchar(15)
--declare @OnCarriageToPortId as varchar(15)
--declare @MainCarriageFromPortId as varchar(15)
--declare @MainCarriageToPortId as varchar(15)
--declare @Transshipment1FromPortId as varchar(15)
--declare @Transshipment1ToPortId as varchar(15)
--declare @Transshipment2FromPortId as varchar(15)
--declare @Transshipment2ToPortId as varchar(15)
--declare @Transshipment3FromPortId as varchar(15)
--declare @Transshipment3ToPortId as varchar(15)
--declare @MainCarriageFinalDestinationPortId as varchar(15)
--END
---- Partners Fields
--BEGIN
--declare @PartnerId as varchar(15)
--declare @PartnerName as varchar(60)
--declare @CityName as varchar(120)
--declare @PartnerReference1 as varchar(50)
--declare @PartnerReference2 as varchar(50)
--declare @AgentId as varchar(15)
--declare @AgentReference1 as varchar(50)
--declare @AgentReference2 as varchar(50)
--declare @ShipperId as varchar(15)
--declare @ShipperReference1 as varchar(50)
--declare @ShipperReference2 as varchar(50)
--declare @ConsigneeId as varchar(15)
--declare @ConsigneeReference1 as varchar(50)
--declare @ConsigneeReference2 as varchar(50)
--declare @CustomerId as varchar(15)
--declare @CustomerReference1 as varchar(50)
--declare @CustomerReference2 as varchar(50)
--declare @Notify1Id as varchar(15)
--declare @Notify2Id as varchar(15)
--declare @IssuingCarrierAgentId as varchar(15)
--declare @CustomAgentImportId as varchar(15)
--declare @CustomAgentImportReference as varchar(50)
--declare @CustomAgentExportId as varchar(15)
--declare @CustomAgentExportReference as varchar(50)
--declare @ShipperNotExporterId as varchar(15)
--declare @ConsigneeNotImporterId as varchar(15)
--declare @FreightForwarderId as varchar(15)
--declare @FreightForwarderReference as varchar(50)
--declare @ConsolidatorId as varchar(15)
--declare @ConsolidatorReference as varchar(50)
--declare @ReleasingAgentId as varchar(15)
--declare @ReleasingAgentReference1 as varchar(50)
--declare @ReleasingAgentReference2 as varchar(50)
--declare @MainCarriageFromAddressId as varchar(50)
--declare @MainCarriageToAddressId as varchar(50)
--END
---- MasterData Fields
--BEGIN
--declare @Master as varchar(20)
--declare @LongMaster as varchar(30)
--declare @MasterShipmentNumber as varchar(15)
--declare @MainCarriageVesselId as varchar(15)
--declare @MainCarriageVesselCode as varchar(5)
--declare @MainCarriageVesselName as varchar(40)
--declare @MainCarriageCarrierId as varchar(15)
--declare @MainCarriageCarrierCode as varchar(15)
--declare @MainCarriageCarrierName as varchar(60)
--declare @MainCarriageCarrierPrefix as varchar(3)
--declare @MainCarriageCarrierNumber as varchar(15)
--declare @Transshipment1AdditionalMAWBOBLBL as varchar(20)
--declare @Transshipment2AdditionalMAWBOBLBL as varchar(20)
--declare @Transshipment3AdditionalMAWBOBLBL as varchar(20)
--END
---- Custom Fields
--BEGIN
--declare @Field nvarchar(250)
--declare @FieldName varchar(10)
--declare @FieldDataTypeCode as varchar(10)
--declare @Field1 nvarchar(250)
--declare @Field2 nvarchar(250)
--declare @Field3 nvarchar(250)
--declare @Field4 nvarchar(250)
--declare @Field5 nvarchar(250)
--declare @Field6 nvarchar(250)
--declare @Field7 nvarchar(250)
--declare @Field8 nvarchar(250)
--declare @Field9 nvarchar(250)
--declare @Field10 nvarchar(250)
--END
--declare @ARInvoiceId as varchar(20)
--declare @ARInvoiceNumber as varchar(20)
--declare @ARInvoiceDraftNumber as varchar(20)
--declare @ContainerNumber as varchar(20)
--declare @Count as int
--set @Count = 0;
--BEGIN
--       DECLARE DataCursor CURSOR READ_ONLY
--       FOR
--       SELECT Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
--		StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
--		FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
--		OnCarriageFromPortId, OnCarriageToPortId,
--		House, CustomFileNumber, AWBCarrierTarrifReference,
--		AgentId, AgentReference1, AgentReference2,
--		ShipperId, ShipperReference1, ShipperReference2,
--		ConsigneeId, ConsigneeReference1, ConsigneeReference2,
--		CustomerId, CustomerReference1, CustomerReference2,
--		Notify1Id, Notify2Id, IssuingCarrierAgentId,
--		CustomAgentImportId, CustomAgentImportReference,
--		CustomAgentExportId, CustomAgentExportReference,
--		ShipperNotExporterId, ConsigneeNotImporterId,
--		FreightForwarderId, FreightForwarderReference,
--		ConsolidatorId, ConsolidatorReference,
--		Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
--		CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
--		ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
--		AMSBL, WarehouseLegReference
--       FROM #tempTable
--       OPEN DataCursor FETCH NEXT FROM DataCursor INTO
--	   @Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
--		@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
--		@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
--		@OnCarriageFromPortId, @OnCarriageToPortId,
--		@House, @CustomFileNumber, @AWBCarrierTarrifReference,
--		@AgentId, @AgentReference1, @AgentReference2,
--		@ShipperId, @ShipperReference1, @ShipperReference2,
--		@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
--		@CustomerId, @CustomerReference1, @CustomerReference2,
--		@Notify1Id,
--		@Notify2Id,
--		@IssuingCarrierAgentId,
--		@CustomAgentImportId, @CustomAgentImportReference,
--		@CustomAgentExportId, @CustomAgentExportReference,
--		@ShipperNotExporterId,
--		@ConsigneeNotImporterId,
--		@FreightForwarderId, @FreightForwarderReference,
--		@ConsolidatorId, @ConsolidatorReference,
--		@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
--		@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
--		@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
--		@AMSBL, @WarehouseLegReference
--       WHILE @@FETCH_STATUS = 0
--       BEGIN
--			set @MySearchFields = ''
--			delete from @PortsTable
--			delete from @PartnersTable
--			delete from @ReferencesTable
--			-- Master Data
--			BEGIN
--				if (@MasterShipmentDataId is not null)
--				BEGIN
--					select
--					@Master = Master,
--					@MasterShipmentNumber = MasterShipmentNumber,
--					@MainCarriageVesselId = MainCarriageVesselId,
--					@MainCarriageCarrierId  = MainCarriageCarrierId,
--					@MainCarriageCarrierNumber = MainCarriageCarrierNumber,
--					@MainCarriageFromPortId = MainCarriageFromPortId,
--					@MainCarriageToPortId = MainCarriageToPortId,
--					@Transshipment1FromPortId = Transshipment1FromPortId,
--					@Transshipment2FromPortId = Transshipment2FromPortId,
--					@Transshipment3FromPortId = Transshipment3FromPortId,
--					@Transshipment1ToPortId = Transshipment1ToPortId,
--					@Transshipment2ToPortId = Transshipment2ToPortId,
--					@Transshipment3ToPortId = Transshipment3ToPortId,
--					@MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPortId,
--					@ImportManifest = ImportManifest,
--					@BookingConfirmationNumber = BookingConfirmationNumber,
--					@CarrierTransportDocumentNumber = CarrierTransportDocumentNumber,
--					@Transshipment1AdditionalMAWBOBLBL = Transshipment1AdditionalMAWBOBLBL,
--					@Transshipment2AdditionalMAWBOBLBL = Transshipment2AdditionalMAWBOBLBL,
--					@Transshipment3AdditionalMAWBOBLBL = Transshipment3AdditionalMAWBOBLBL,
--					@MainCarriageFromAddressId = MainCarriageFromAddressId,
--					@MainCarriageToAddressId = MainCarriageToAddressId
--					from ShipmentMasterDatas
--					where Id = @MasterShipmentDataId AND Tenant = @Tenant
--				END
--			END
--			-- Fields
--			BEGIN
--			if (@ProjectNumber is not null AND @ProjectNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @ProjectNumber
--				else set @MySearchFields = @MySearchFields + ',' + @ProjectNumber
--			end
--			if (@House is not null AND @House <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @House
--				else set @MySearchFields = @MySearchFields + ',' + @House
--			end
--			if (@Master is not null AND @Master <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @Master
--				else set @MySearchFields = @MySearchFields + ',' + @Master
--			end
--			if (@ShipmentNumber is not null AND @ShipmentNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @ShipmentNumber
--				else set @MySearchFields = @MySearchFields + ',' + @ShipmentNumber
--			end
--			if (@CustomFileNumber is not null AND @CustomFileNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @CustomFileNumber
--				else set @MySearchFields = @MySearchFields + ',' + @CustomFileNumber
--			end
--			if (@AWBCarrierTarrifReference is not null AND @AWBCarrierTarrifReference <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @AWBCarrierTarrifReference
--				else set @MySearchFields = @MySearchFields + ',' + @AWBCarrierTarrifReference
--			end
--			if (@CustomsDeclarationNumber is not null AND @CustomsDeclarationNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @CustomsDeclarationNumber
--				else set @MySearchFields = @MySearchFields + ',' + @CustomsDeclarationNumber
--			end
--			if (@ForwarderShipmentNumber is not null AND @ForwarderShipmentNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @ForwarderShipmentNumber
--				else set @MySearchFields = @MySearchFields + ',' + @ForwarderShipmentNumber
--			end
--			if (@TransportDocumentNumber is not null AND @TransportDocumentNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @TransportDocumentNumber
--				else set @MySearchFields = @MySearchFields + ',' + @TransportDocumentNumber
--			end
--			if (@ImportManifest is not null AND @ImportManifest <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @ImportManifest
--				else set @MySearchFields = @MySearchFields + ',' + @ImportManifest
--			end
--			if (@BookingConfirmationNumber is not null AND @BookingConfirmationNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @BookingConfirmationNumber
--				else set @MySearchFields = @MySearchFields + ',' + @BookingConfirmationNumber
--			end
--			if (@CarrierTransportDocumentNumber is not null AND @CarrierTransportDocumentNumber <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @CarrierTransportDocumentNumber
--				else set @MySearchFields = @MySearchFields + ',' + @CarrierTransportDocumentNumber
--			end
--			if (@Transshipment1AdditionalMAWBOBLBL is not null AND @Transshipment1AdditionalMAWBOBLBL <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @Transshipment1AdditionalMAWBOBLBL
--				else set @MySearchFields = @MySearchFields + ',' + @Transshipment1AdditionalMAWBOBLBL
--			end
--			if (@Transshipment2AdditionalMAWBOBLBL is not null AND @Transshipment2AdditionalMAWBOBLBL <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @Transshipment2AdditionalMAWBOBLBL
--				else set @MySearchFields = @MySearchFields + ',' + @Transshipment2AdditionalMAWBOBLBL
--			end
--			if (@Transshipment3AdditionalMAWBOBLBL is not null AND @Transshipment3AdditionalMAWBOBLBL <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @Transshipment3AdditionalMAWBOBLBL
--				else set @MySearchFields = @MySearchFields + ',' + @Transshipment3AdditionalMAWBOBLBL
--			end
--			if (@QuoteId is not null)
--			begin
--				set @QuoteNumber = (select QuoteNumber from Quotes where Id = @QuoteId AND Tenant = @Tenant)
--				if (@QuoteNumber is not null AND @QuoteNumber <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @QuoteNumber
--					else set @MySearchFields = @MySearchFields + ',' + @QuoteNumber
--				end
--			end
--			if (@StatusId is not null)
--			begin
--				select
--				@StatusCode = Code,
--				@StatusName = Name
--				from EntityStatus
--				where Id = @StatusId AND Tenant = @Tenant
--				if (@StatusCode is not null AND @StatusCode <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @StatusCode
--					else set @MySearchFields = @MySearchFields + ',' + @StatusCode
--				end
--				if (@StatusName is not null AND @StatusName <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @StatusName
--					else set @MySearchFields = @MySearchFields + ',' + @StatusName
--				end
--			end
--			if (@MainCarriageVesselId is not null)
--			begin
--				select
--				@MainCarriageVesselCode = Code,
--				@MainCarriageVesselName = EnglishName
--				from Vessels
--				where Id = @MainCarriageVesselId AND Tenant = @Tenant
--				if (@MainCarriageVesselCode is not null AND @MainCarriageVesselCode <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @MainCarriageVesselCode
--					else set @MySearchFields = @MySearchFields + ',' + @MainCarriageVesselCode
--				end
--				if (@MainCarriageVesselName is not null AND @MainCarriageVesselName <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @MainCarriageVesselName
--					else set @MySearchFields = @MySearchFields + ',' + @MainCarriageVesselName
--				end
--			end
--			if (@MainCarriageCarrierId is not null)
--			begin
--				select
--				@MainCarriageCarrierCode = Cards.Code,
--				@MainCarriageCarrierName = Cards.EnglishName,
--				@MainCarriageCarrierPrefix = Airlines.Prefix
--				from Airlines join Cards on Airlines.Id = Cards.Id
--				where Airlines.Tenant = @Tenant AND Airlines.Id = @MainCarriageCarrierId
--				if (@TransportModeId = 'A' AND @Master is not null AND @Master <> '')
--				begin
--					set @LongMaster = @MainCarriageCarrierPrefix + '-' + @Master
--					if (@LongMaster is not null AND @LongMaster <> '')
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @LongMaster
--						else set @MySearchFields = @MySearchFields + ',' + @LongMaster
--					end
--				end
--				if (@MainCarriageCarrierCode is not null AND @MainCarriageCarrierCode <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierCode
--					else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierCode
--				end
--				if (@MainCarriageCarrierName is not null AND @MainCarriageCarrierName <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierName
--					else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierName
--				end
--				if (@MainCarriageCarrierNumber is not null AND @MainCarriageCarrierNumber <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @MainCarriageCarrierNumber
--					else set @MySearchFields = @MySearchFields + ',' + @MainCarriageCarrierNumber
--				end
--			end
--			if (@SalesmanUserId is not null)
--			begin
--				set @SalesmanUserName = (select EnglishName from Contacts where Id = @SalesmanUserId AND Tenant = @Tenant)
--				if (@SalesmanUserName is not null AND @SalesmanUserName <> '')
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @SalesmanUserName
--					else set @MySearchFields = @MySearchFields + ',' + @SalesmanUserName
--				end
--			end
--			if (@AMSBL is not null AND @AMSBL <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @AMSBL
--				else set @MySearchFields = @MySearchFields + ',' + @AMSBL
--			end
--			if (@WarehouseLegReference is not null AND @WarehouseLegReference <> '')
--			begin
--				if (@MySearchFields = '') set @MySearchFields = @WarehouseLegReference
--				else set @MySearchFields = @MySearchFields + ',' + @WarehouseLegReference
--			end
--			END
--			-- Ports
--			BEGIN
--			set @PortId = @FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @PreCarriageFromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @PreCarriageToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @OnCarriageFromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @OnCarriageToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @MainCarriageFromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @MainCarriageToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment1FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment1ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment2FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment2ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment3FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment3ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			set @PortId = @MainCarriageFinalDestinationPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + ',' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + ',' + @PortCountryName
--				end
--			end
--			END
--			-- Partners
--			BEGIN
--			set @PartnerId = @AgentId
--			set @PartnerReference1 = @AgentReference1
--			set @PartnerReference2 = @AgentReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ShipperId
--			set @PartnerReference1 = @ShipperReference1
--			set @PartnerReference2 = @ShipperReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ConsigneeId
--			set @PartnerReference1 = @ConsigneeReference1
--			set @PartnerReference2 = @ConsigneeReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @CustomerId
--			set @PartnerReference1 = @CustomerReference1
--			set @PartnerReference2 = @CustomerReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @Notify1Id
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @Notify2Id
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @IssuingCarrierAgentId
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @CustomAgentImportId
--			set @PartnerReference1 = @CustomAgentImportReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @CustomAgentExportId
--			set @PartnerReference1 = @CustomAgentExportReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ShipperNotExporterId
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ConsigneeNotImporterId
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @FreightForwarderId
--			set @PartnerReference1 = @FreightForwarderReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ConsolidatorId
--			set @PartnerReference1 = @ConsolidatorReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ReleasingAgentId
--			set @PartnerReference1 = @ReleasingAgentReference1
--			set @PartnerReference2 = @ReleasingAgentReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + ',' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + ',' + @PartnerReference2
--				end
--			end
--			if(@TransportModeId = 'I' and @DirectionId ='D' and @MasterShipmentDataId is not null)
--			begin
--			set @CityName = (select City from Addresses where Id = @MainCarriageFromAddressId AND Tenant = @Tenant)
--			if(@CityName is not null or @CityName != '')
--			begin
--			if (@MySearchFields = '') set @MySearchFields = @CityName
--					else set @MySearchFields = @MySearchFields + ',' + @CityName
--					end
--					set @CityName = (select City from Addresses where Id = @MainCarriageToAddressId AND Tenant = @Tenant)
--			if(@CityName is not null or @CityName != '')
--			begin
--			if (@MySearchFields = '') set @MySearchFields = @CityName
--					else set @MySearchFields = @MySearchFields + ',' + @CityName
--					end
--			end
--			END
--			-- Invoices
--			if exists (select * from ARInvoiceEntities where EntityId = @Id AND Tenant = @Tenant)
--			BEGIN
--				DECLARE ARInvoicesCursor CURSOR READ_ONLY
--				FOR
--				SELECT ARInvoices.Id, ARInvoices.InvoiceNumber, ARInvoices.DraftNumber
--				FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id AND ARInvoiceEntities.Tenant = ARInvoices.Tenant
--				WHERE ARInvoiceEntities.EntityId = @Id AND ARInvoiceEntities.Tenant = @Tenant
--				OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
--				WHILE @@FETCH_STATUS = 0
--				BEGIN
--					if (@ARInvoiceNumber is not null AND @ARInvoiceNumber <> @ARInvoiceId)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @ARInvoiceNumber
--						else set @MySearchFields = @MySearchFields + ',' + @ARInvoiceNumber
--					end
--					else if (@ARInvoiceDraftNumber is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @ARInvoiceDraftNumber
--						else set @MySearchFields = @MySearchFields + ',' + @ARInvoiceDraftNumber
--					end
--				FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
--				END
--				CLOSE ARInvoicesCursor
--				DEALLOCATE ARInvoicesCursor
--			END
--			-- Packages Containers
--			if exists (select * from ShipmentPackages where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null)
--			BEGIN
--				DECLARE PackagesCursor CURSOR READ_ONLY
--				FOR
--				SELECT ContainerNumber
--				FROM ShipmentPackages
--				Where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null AND ContainerNumber <> ''
--				group by ContainerNumber
--				OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
--				WHILE @@FETCH_STATUS = 0
--				BEGIN
--					if (@ContainerNumber is not null)
--					begin
--						if (@MySearchFields = '') set @MySearchFields = @ContainerNumber
--						else set @MySearchFields = @MySearchFields + ',' + @ContainerNumber
--					end
--				FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
--				END
--				CLOSE PackagesCursor
--				DEALLOCATE PackagesCursor
--			END
--			-- Custom Fields
--			BEGIN
--			set @Field = @Field1
--			set @FieldName = 'Field1'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field2
--			set @FieldName = 'Field2'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field3
--			set @FieldName = 'Field3'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field4
--			set @FieldName = 'Field4'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field5
--			set @FieldName = 'Field5'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field6
--			set @FieldName = 'Field6'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field7
--			set @FieldName = 'Field7'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field8
--			set @FieldName = 'Field8'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field9
--			set @FieldName = 'Field9'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			set @Field = @Field10
--			set @FieldName = 'Field10'
--			if (@Field is not null AND @Field <> '')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = 'Shipment'
--				if (@FieldDataTypeCode = 'Text' OR @FieldDataTypeCode = 'nText')
--				begin
--				if (@MySearchFields = '') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + ',' + @Field
--				end
--			end
--			END
--       		insert into #temp_Shipments(Id, SearchFields) values (@Id, @MySearchFields)
--			set @Count = @Count + 1;
--			if(@Count = 4000)
--			begin
--				update Shipments
--				set
--				SearchFields = #temp_Shipments.SearchFields
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--				truncate table #temp_Shipments
--				set @Count = 0
--			end
--       FETCH NEXT FROM DataCursor INTO
--	   @Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
--		@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
--		@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
--		@OnCarriageFromPortId, @OnCarriageToPortId,
--		@House, @CustomFileNumber, @AWBCarrierTarrifReference,
--		@AgentId, @AgentReference1, @AgentReference2,
--		@ShipperId, @ShipperReference1, @ShipperReference2,
--		@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
--		@CustomerId, @CustomerReference1, @CustomerReference2,
--		@Notify1Id,
--		@Notify2Id,
--		@IssuingCarrierAgentId,
--		@CustomAgentImportId, @CustomAgentImportReference,
--		@CustomAgentExportId, @CustomAgentExportReference,
--		@ShipperNotExporterId,
--		@ConsigneeNotImporterId,
--		@FreightForwarderId, @FreightForwarderReference,
--		@ConsolidatorId, @ConsolidatorReference,
--		@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
--		@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
--		@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
--		@AMSBL, @WarehouseLegReference
--       END
--       CLOSE DataCursor
--       DEALLOCATE DataCursor
--    END
--	if (@Count > 0)
--	begin
--				update Shipments
--				set
--				SearchFields = #temp_Shipments.SearchFields
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--	end
--SET NOCOUNT OFF
--drop table #tempTable
--drop table #temp_Shipments
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('FillShipmentSearchFields.sxml', GETDATE(), '--If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
--Begin
--    Drop Table #tempTable
--End
--If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
--Begin
--    Drop Table #temp_Shipments
--End
--CREATE TABLE #temp_Shipments (
--	Id varchar(15) not null ,
--    SearchFields nvarchar(4000)  null,
--    )
--select Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
--		StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
--		FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
--		OnCarriageFromPortId, OnCarriageToPortId,
--		House, CustomFileNumber, AWBCarrierTarrifReference,
--		AgentId, AgentReference1, AgentReference2,
--		ShipperId, ShipperReference1, ShipperReference2,
--		ConsigneeId, ConsigneeReference1, ConsigneeReference2,
--		CustomerId, CustomerReference1, CustomerReference2,
--		Notify1Id, Notify2Id, IssuingCarrierAgentId,
--		CustomAgentImportId, CustomAgentImportReference,
--		CustomAgentExportId, CustomAgentExportReference,
--		ShipperNotExporterId, ConsigneeNotImporterId,
--		FreightForwarderId, FreightForwarderReference,
--		ConsolidatorId, ConsolidatorReference,
--		Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
--		CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
--		ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
--		AMSBL, WarehouseLegReference
--	into #tempTable
--	FROM Shipments
--SET NOCOUNT ON
--declare @MySearchFields as nvarchar(4000)
--declare @PortsTable table
--(
--  Id varchar(15) not null
--)
--declare @PartnersTable table
--(
--  Id varchar(15) not null
--)
--declare @ReferencesTable table
--(
--  Reference varchar(50) not null
--)
---- Shipment fields
--BEGIN
--declare @Id as varchar(15)
--declare @Tenant as int
--declare @ShipmentNumber as varchar(15)
--declare @ShipmentLevelCode as varchar(1)
--declare @TransportModeId as varchar(1)
--declare @DirectionId as varchar(1)
--declare @StatusId as varchar(15)
--declare @StatusCode as varchar(4)
--declare @StatusName as varchar(40)
--declare @QuoteId as varchar(15)
--declare @QuoteNumber as varchar(15)
--declare @SalesmanUserId as varchar(15)
--declare @SalesmanUserName as varchar(60)
--declare @MasterShipmentDataId as varchar(15)
--declare @House as varchar(20)
--declare @CustomFileNumber as varchar(15)
--declare @AWBCarrierTarrifReference as varchar(25)
--declare @CustomsDeclarationNumber as varchar(35)
--declare @ForwarderShipmentNumber as varchar(15)
--declare @TransportDocumentNumber as varchar(50)
--declare @ImportManifest as varchar(50)
--declare @BookingConfirmationNumber as varchar(25)
--declare @CarrierTransportDocumentNumber as varchar(50)
--declare @ProjectNumber as varchar(100)
--declare @AMSBL as nvarchar(17)
--declare @WarehouseLegReference as nvarchar(50)
--END
---- Ports Firlds
--BEGIN
--declare @PortId as varchar(15)
--declare @PortCode as varchar(3)
--declare @PortName as varchar(40)
--declare @PortCountryCode as varchar(2)
--declare @PortCountryName as varchar(120)
--declare @FromPortId as varchar(15)
--declare @ToPortId as varchar(15)
--declare @PreCarriageFromPortId as varchar(15)
--declare @PreCarriageToPortId as varchar(15)
--declare @OnCarriageFromPortId as varchar(15)
--declare @OnCarriageToPortId as varchar(15)
--declare @MainCarriageFromPortId as varchar(15)
--declare @MainCarriageToPortId as varchar(15)
--declare @Transshipment1FromPortId as varchar(15)
--declare @Transshipment1ToPortId as varchar(15)
--declare @Transshipment2FromPortId as varchar(15)
--declare @Transshipment2ToPortId as varchar(15)
--declare @Transshipment3FromPortId as varchar(15)
--declare @Transshipment3ToPortId as varchar(15)
--declare @MainCarriageFinalDestinationPortId as varchar(15)
--END
---- Partners Fields
--BEGIN
--declare @PartnerId as varchar(15)
--declare @PartnerName as varchar(60)
--declare @CityName as varchar(120)
--declare @PartnerReference1 as varchar(50)
--declare @PartnerReference2 as varchar(50)
--declare @AgentId as varchar(15)
--declare @AgentReference1 as varchar(50)
--declare @AgentReference2 as varchar(50)
--declare @ShipperId as varchar(15)
--declare @ShipperReference1 as varchar(50)
--declare @ShipperReference2 as varchar(50)
--declare @ConsigneeId as varchar(15)
--declare @ConsigneeReference1 as varchar(50)
--declare @ConsigneeReference2 as varchar(50)
--declare @CustomerId as varchar(15)
--declare @CustomerReference1 as varchar(50)
--declare @CustomerReference2 as varchar(50)
--declare @Notify1Id as varchar(15)
--declare @Notify2Id as varchar(15)
--declare @IssuingCarrierAgentId as varchar(15)
--declare @CustomAgentImportId as varchar(15)
--declare @CustomAgentImportReference as varchar(50)
--declare @CustomAgentExportId as varchar(15)
--declare @CustomAgentExportReference as varchar(50)
--declare @ShipperNotExporterId as varchar(15)
--declare @ConsigneeNotImporterId as varchar(15)
--declare @FreightForwarderId as varchar(15)
--declare @FreightForwarderReference as varchar(50)
--declare @ConsolidatorId as varchar(15)
--declare @ConsolidatorReference as varchar(50)
--declare @ReleasingAgentId as varchar(15)
--declare @ReleasingAgentReference1 as varchar(50)
--declare @ReleasingAgentReference2 as varchar(50)
--declare @MainCarriageFromAddressId as varchar(50)
--declare @MainCarriageToAddressId as varchar(50)
--END
---- MasterData Fields
--BEGIN
--declare @Master as varchar(20)
--declare @LongMaster as varchar(30)
--declare @MasterShipmentNumber as varchar(15)
--declare @MainCarriageVesselId as varchar(15)
--declare @MainCarriageVesselCode as varchar(5)
--declare @MainCarriageVesselName as varchar(40)
--declare @MainCarriageCarrierId as varchar(15)
--declare @MainCarriageCarrierCode as varchar(15)
--declare @MainCarriageCarrierName as varchar(60)
--declare @MainCarriageCarrierPrefix as varchar(3)
--declare @MainCarriageCarrierNumber as varchar(15)
--declare @Transshipment1AdditionalMAWBOBLBL as varchar(20)
--declare @Transshipment2AdditionalMAWBOBLBL as varchar(20)
--declare @Transshipment3AdditionalMAWBOBLBL as varchar(20)
--END
---- Custom Fields
--BEGIN
--declare @Field nvarchar(250)
--declare @FieldName varchar(10)
--declare @FieldDataTypeCode as varchar(10)
--declare @Field1 nvarchar(250)
--declare @Field2 nvarchar(250)
--declare @Field3 nvarchar(250)
--declare @Field4 nvarchar(250)
--declare @Field5 nvarchar(250)
--declare @Field6 nvarchar(250)
--declare @Field7 nvarchar(250)
--declare @Field8 nvarchar(250)
--declare @Field9 nvarchar(250)
--declare @Field10 nvarchar(250)
--END
--declare @ARInvoiceId as varchar(20)
--declare @ARInvoiceNumber as varchar(20)
--declare @ARInvoiceDraftNumber as varchar(20)
--declare @ContainerNumber as varchar(20)
--declare @Count as int
--set @Count = 0;
--BEGIN
--       DECLARE DataCursor CURSOR READ_ONLY
--       FOR
--       SELECT Id, Tenant, ShipmentNumber, ShipmentLevelCode, TransportModeId,DirectionId,
--		StatusId, QuoteId, SalesmanUserId, MasterShipmentDataId,
--		FromPortId, ToPortId, PreCarriageFromPortId, PreCarriageToPortId,
--		OnCarriageFromPortId, OnCarriageToPortId,
--		House, CustomFileNumber, AWBCarrierTarrifReference,
--		AgentId, AgentReference1, AgentReference2,
--		ShipperId, ShipperReference1, ShipperReference2,
--		ConsigneeId, ConsigneeReference1, ConsigneeReference2,
--		CustomerId, CustomerReference1, CustomerReference2,
--		Notify1Id, Notify2Id, IssuingCarrierAgentId,
--		CustomAgentImportId, CustomAgentImportReference,
--		CustomAgentExportId, CustomAgentExportReference,
--		ShipperNotExporterId, ConsigneeNotImporterId,
--		FreightForwarderId, FreightForwarderReference,
--		ConsolidatorId, ConsolidatorReference,
--		Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10,
--		CustomsDeclarationNumber, ForwarderShipmentNumber, TransportDocumentNumber,
--		ReleasingAgentId, ReleasingAgentReference1 , ReleasingAgentReference2,ProjectNumber,
--		AMSBL, WarehouseLegReference
--       FROM #tempTable
--       OPEN DataCursor FETCH NEXT FROM DataCursor INTO
--	   @Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
--		@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
--		@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
--		@OnCarriageFromPortId, @OnCarriageToPortId,
--		@House, @CustomFileNumber, @AWBCarrierTarrifReference,
--		@AgentId, @AgentReference1, @AgentReference2,
--		@ShipperId, @ShipperReference1, @ShipperReference2,
--		@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
--		@CustomerId, @CustomerReference1, @CustomerReference2,
--		@Notify1Id,
--		@Notify2Id,
--		@IssuingCarrierAgentId,
--		@CustomAgentImportId, @CustomAgentImportReference,
--		@CustomAgentExportId, @CustomAgentExportReference,
--		@ShipperNotExporterId,
--		@ConsigneeNotImporterId,
--		@FreightForwarderId, @FreightForwarderReference,
--		@ConsolidatorId, @ConsolidatorReference,
--		@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
--		@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
--		@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
--		@AMSBL, @WarehouseLegReference
--       WHILE @@FETCH_STATUS = 0
--       BEGIN
--			set @MySearchFields = ''''
--			delete from @PortsTable
--			delete from @PartnersTable
--			delete from @ReferencesTable
--			-- Master Data
--			BEGIN
--				if (@MasterShipmentDataId is not null)
--				BEGIN
--					select
--					@Master = Master,
--					@MasterShipmentNumber = MasterShipmentNumber,
--					@MainCarriageVesselId = MainCarriageVesselId,
--					@MainCarriageCarrierId  = MainCarriageCarrierId,
--					@MainCarriageCarrierNumber = MainCarriageCarrierNumber,
--					@MainCarriageFromPortId = MainCarriageFromPortId,
--					@MainCarriageToPortId = MainCarriageToPortId,
--					@Transshipment1FromPortId = Transshipment1FromPortId,
--					@Transshipment2FromPortId = Transshipment2FromPortId,
--					@Transshipment3FromPortId = Transshipment3FromPortId,
--					@Transshipment1ToPortId = Transshipment1ToPortId,
--					@Transshipment2ToPortId = Transshipment2ToPortId,
--					@Transshipment3ToPortId = Transshipment3ToPortId,
--					@MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPortId,
--					@ImportManifest = ImportManifest,
--					@BookingConfirmationNumber = BookingConfirmationNumber,
--					@CarrierTransportDocumentNumber = CarrierTransportDocumentNumber,
--					@Transshipment1AdditionalMAWBOBLBL = Transshipment1AdditionalMAWBOBLBL,
--					@Transshipment2AdditionalMAWBOBLBL = Transshipment2AdditionalMAWBOBLBL,
--					@Transshipment3AdditionalMAWBOBLBL = Transshipment3AdditionalMAWBOBLBL,
--					@MainCarriageFromAddressId = MainCarriageFromAddressId,
--					@MainCarriageToAddressId = MainCarriageToAddressId
--					from ShipmentMasterDatas
--					where Id = @MasterShipmentDataId AND Tenant = @Tenant
--				END
--			END
--			-- Fields
--			BEGIN
--			if (@ProjectNumber is not null AND @ProjectNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @ProjectNumber
--				else set @MySearchFields = @MySearchFields + '','' + @ProjectNumber
--			end
--			if (@House is not null AND @House <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @House
--				else set @MySearchFields = @MySearchFields + '','' + @House
--			end
--			if (@Master is not null AND @Master <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @Master
--				else set @MySearchFields = @MySearchFields + '','' + @Master
--			end
--			if (@ShipmentNumber is not null AND @ShipmentNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @ShipmentNumber
--				else set @MySearchFields = @MySearchFields + '','' + @ShipmentNumber
--			end
--			if (@CustomFileNumber is not null AND @CustomFileNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @CustomFileNumber
--				else set @MySearchFields = @MySearchFields + '','' + @CustomFileNumber
--			end
--			if (@AWBCarrierTarrifReference is not null AND @AWBCarrierTarrifReference <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @AWBCarrierTarrifReference
--				else set @MySearchFields = @MySearchFields + '','' + @AWBCarrierTarrifReference
--			end
--			if (@CustomsDeclarationNumber is not null AND @CustomsDeclarationNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @CustomsDeclarationNumber
--				else set @MySearchFields = @MySearchFields + '','' + @CustomsDeclarationNumber
--			end
--			if (@ForwarderShipmentNumber is not null AND @ForwarderShipmentNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @ForwarderShipmentNumber
--				else set @MySearchFields = @MySearchFields + '','' + @ForwarderShipmentNumber
--			end
--			if (@TransportDocumentNumber is not null AND @TransportDocumentNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @TransportDocumentNumber
--				else set @MySearchFields = @MySearchFields + '','' + @TransportDocumentNumber
--			end
--			if (@ImportManifest is not null AND @ImportManifest <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @ImportManifest
--				else set @MySearchFields = @MySearchFields + '','' + @ImportManifest
--			end
--			if (@BookingConfirmationNumber is not null AND @BookingConfirmationNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @BookingConfirmationNumber
--				else set @MySearchFields = @MySearchFields + '','' + @BookingConfirmationNumber
--			end
--			if (@CarrierTransportDocumentNumber is not null AND @CarrierTransportDocumentNumber <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @CarrierTransportDocumentNumber
--				else set @MySearchFields = @MySearchFields + '','' + @CarrierTransportDocumentNumber
--			end
--			if (@Transshipment1AdditionalMAWBOBLBL is not null AND @Transshipment1AdditionalMAWBOBLBL <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @Transshipment1AdditionalMAWBOBLBL
--				else set @MySearchFields = @MySearchFields + '','' + @Transshipment1AdditionalMAWBOBLBL
--			end
--			if (@Transshipment2AdditionalMAWBOBLBL is not null AND @Transshipment2AdditionalMAWBOBLBL <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @Transshipment2AdditionalMAWBOBLBL
--				else set @MySearchFields = @MySearchFields + '','' + @Transshipment2AdditionalMAWBOBLBL
--			end
--			if (@Transshipment3AdditionalMAWBOBLBL is not null AND @Transshipment3AdditionalMAWBOBLBL <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @Transshipment3AdditionalMAWBOBLBL
--				else set @MySearchFields = @MySearchFields + '','' + @Transshipment3AdditionalMAWBOBLBL
--			end
--			if (@QuoteId is not null)
--			begin
--				set @QuoteNumber = (select QuoteNumber from Quotes where Id = @QuoteId AND Tenant = @Tenant)
--				if (@QuoteNumber is not null AND @QuoteNumber <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @QuoteNumber
--					else set @MySearchFields = @MySearchFields + '','' + @QuoteNumber
--				end
--			end
--			if (@StatusId is not null)
--			begin
--				select
--				@StatusCode = Code,
--				@StatusName = Name
--				from EntityStatus
--				where Id = @StatusId AND Tenant = @Tenant
--				if (@StatusCode is not null AND @StatusCode <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @StatusCode
--					else set @MySearchFields = @MySearchFields + '','' + @StatusCode
--				end
--				if (@StatusName is not null AND @StatusName <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @StatusName
--					else set @MySearchFields = @MySearchFields + '','' + @StatusName
--				end
--			end
--			if (@MainCarriageVesselId is not null)
--			begin
--				select
--				@MainCarriageVesselCode = Code,
--				@MainCarriageVesselName = EnglishName
--				from Vessels
--				where Id = @MainCarriageVesselId AND Tenant = @Tenant
--				if (@MainCarriageVesselCode is not null AND @MainCarriageVesselCode <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @MainCarriageVesselCode
--					else set @MySearchFields = @MySearchFields + '','' + @MainCarriageVesselCode
--				end
--				if (@MainCarriageVesselName is not null AND @MainCarriageVesselName <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @MainCarriageVesselName
--					else set @MySearchFields = @MySearchFields + '','' + @MainCarriageVesselName
--				end
--			end
--			if (@MainCarriageCarrierId is not null)
--			begin
--				select
--				@MainCarriageCarrierCode = Cards.Code,
--				@MainCarriageCarrierName = Cards.EnglishName,
--				@MainCarriageCarrierPrefix = Airlines.Prefix
--				from Airlines join Cards on Airlines.Id = Cards.Id
--				where Airlines.Tenant = @Tenant AND Airlines.Id = @MainCarriageCarrierId
--				if (@TransportModeId = ''A'' AND @Master is not null AND @Master <> '''')
--				begin
--					set @LongMaster = @MainCarriageCarrierPrefix + ''-'' + @Master
--					if (@LongMaster is not null AND @LongMaster <> '''')
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @LongMaster
--						else set @MySearchFields = @MySearchFields + '','' + @LongMaster
--					end
--				end
--				if (@MainCarriageCarrierCode is not null AND @MainCarriageCarrierCode <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierCode
--					else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierCode
--				end
--				if (@MainCarriageCarrierName is not null AND @MainCarriageCarrierName <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierName
--					else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierName
--				end
--				if (@MainCarriageCarrierNumber is not null AND @MainCarriageCarrierNumber <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @MainCarriageCarrierNumber
--					else set @MySearchFields = @MySearchFields + '','' + @MainCarriageCarrierNumber
--				end
--			end
--			if (@SalesmanUserId is not null)
--			begin
--				set @SalesmanUserName = (select EnglishName from Contacts where Id = @SalesmanUserId AND Tenant = @Tenant)
--				if (@SalesmanUserName is not null AND @SalesmanUserName <> '''')
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @SalesmanUserName
--					else set @MySearchFields = @MySearchFields + '','' + @SalesmanUserName
--				end
--			end
--			if (@AMSBL is not null AND @AMSBL <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @AMSBL
--				else set @MySearchFields = @MySearchFields + '','' + @AMSBL
--			end
--			if (@WarehouseLegReference is not null AND @WarehouseLegReference <> '''')
--			begin
--				if (@MySearchFields = '''') set @MySearchFields = @WarehouseLegReference
--				else set @MySearchFields = @MySearchFields + '','' + @WarehouseLegReference
--			end
--			END
--			-- Ports
--			BEGIN
--			set @PortId = @FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @PreCarriageFromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @PreCarriageToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @OnCarriageFromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @OnCarriageToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @MainCarriageFromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @MainCarriageToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment1FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment1ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment2FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment2ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment3FromPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @Transshipment3ToPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			set @PortId = @MainCarriageFinalDestinationPortId
--			if (@PortId is not null)
--			if not exists (select * from @PortsTable where Id = @PortId)
--			begin
--				insert into @PortsTable(Id) values (@PortId)
--				select
--				@PortCode = Ports.Code,
--				@PortName = Ports.EnglishName,
--				@PortCountryCode = Countries.Code,
--				@PortCountryName  = Countries.EnglishName
--				from Ports join Countries on Ports.CountryId = Countries.Id AND Ports.Tenant = Countries.Tenant
--				where Ports.Id = @PortId AND Ports.Tenant = @Tenant
--				if (@PortCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCode
--				end
--				if (@PortName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortName
--					else set @MySearchFields = @MySearchFields + '','' + @PortName
--				end
--				if (@PortCountryCode is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryCode
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryCode
--				end
--				if (@PortCountryName is not null)
--				begin
--					if (@MySearchFields = '''') set @MySearchFields = @PortCountryName
--					else set @MySearchFields = @MySearchFields + '','' + @PortCountryName
--				end
--			end
--			END
--			-- Partners
--			BEGIN
--			set @PartnerId = @AgentId
--			set @PartnerReference1 = @AgentReference1
--			set @PartnerReference2 = @AgentReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ShipperId
--			set @PartnerReference1 = @ShipperReference1
--			set @PartnerReference2 = @ShipperReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ConsigneeId
--			set @PartnerReference1 = @ConsigneeReference1
--			set @PartnerReference2 = @ConsigneeReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @CustomerId
--			set @PartnerReference1 = @CustomerReference1
--			set @PartnerReference2 = @CustomerReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @Notify1Id
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @Notify2Id
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @IssuingCarrierAgentId
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @CustomAgentImportId
--			set @PartnerReference1 = @CustomAgentImportReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @CustomAgentExportId
--			set @PartnerReference1 = @CustomAgentExportReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ShipperNotExporterId
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ConsigneeNotImporterId
--			set @PartnerReference1 = null
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @FreightForwarderId
--			set @PartnerReference1 = @FreightForwarderReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ConsolidatorId
--			set @PartnerReference1 = @ConsolidatorReference
--			set @PartnerReference2 = null
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			set @PartnerId = @ReleasingAgentId
--			set @PartnerReference1 = @ReleasingAgentReference1
--			set @PartnerReference2 = @ReleasingAgentReference2
--			if (@PartnerId is not null)
--			begin
--				if not exists (select * from @PartnersTable where Id = @PartnerId)
--				begin
--					insert into @PartnersTable(Id) values (@PartnerId)
--					set @PartnerName = (select EnglishName from Cards where Id = @PartnerId AND Tenant = @Tenant)
--					if (@PartnerName is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @PartnerName
--						else set @MySearchFields = @MySearchFields + '','' + @PartnerName
--					end
--				end
--				if (@PartnerReference1 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference1)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference1)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference1
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference1
--				end
--				if (@PartnerReference2 is not null)
--				if not exists (select * from @ReferencesTable where Reference = @PartnerReference2)
--				begin
--					insert into @ReferencesTable(Reference) values (@PartnerReference2)
--					if (@MySearchFields = '''') set @MySearchFields = @PartnerReference2
--					else set @MySearchFields = @MySearchFields + '','' + @PartnerReference2
--				end
--			end
--			if(@TransportModeId = ''I'' and @DirectionId =''D'' and @MasterShipmentDataId is not null)
--			begin
--			set @CityName = (select City from Addresses where Id = @MainCarriageFromAddressId AND Tenant = @Tenant)
--			if(@CityName is not null or @CityName != '''')
--			begin
--			if (@MySearchFields = '''') set @MySearchFields = @CityName
--					else set @MySearchFields = @MySearchFields + '','' + @CityName
--					end
--					set @CityName = (select City from Addresses where Id = @MainCarriageToAddressId AND Tenant = @Tenant)
--			if(@CityName is not null or @CityName != '''')
--			begin
--			if (@MySearchFields = '''') set @MySearchFields = @CityName
--					else set @MySearchFields = @MySearchFields + '','' + @CityName
--					end
--			end
--			END
--			-- Invoices
--			if exists (select * from ARInvoiceEntities where EntityId = @Id AND Tenant = @Tenant)
--			BEGIN
--				DECLARE ARInvoicesCursor CURSOR READ_ONLY
--				FOR
--				SELECT ARInvoices.Id, ARInvoices.InvoiceNumber, ARInvoices.DraftNumber
--				FROM ARInvoiceEntities join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id AND ARInvoiceEntities.Tenant = ARInvoices.Tenant
--				WHERE ARInvoiceEntities.EntityId = @Id AND ARInvoiceEntities.Tenant = @Tenant
--				OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
--				WHILE @@FETCH_STATUS = 0
--				BEGIN
--					if (@ARInvoiceNumber is not null AND @ARInvoiceNumber <> @ARInvoiceId)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @ARInvoiceNumber
--						else set @MySearchFields = @MySearchFields + '','' + @ARInvoiceNumber
--					end
--					else if (@ARInvoiceDraftNumber is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @ARInvoiceDraftNumber
--						else set @MySearchFields = @MySearchFields + '','' + @ARInvoiceDraftNumber
--					end
--				FETCH NEXT FROM ARInvoicesCursor INTO @ARInvoiceId, @ARInvoiceNumber, @ARInvoiceDraftNumber
--				END
--				CLOSE ARInvoicesCursor
--				DEALLOCATE ARInvoicesCursor
--			END
--			-- Packages Containers
--			if exists (select * from ShipmentPackages where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null)
--			BEGIN
--				DECLARE PackagesCursor CURSOR READ_ONLY
--				FOR
--				SELECT ContainerNumber
--				FROM ShipmentPackages
--				Where ShipmentId = @Id AND Tenant = @Tenant AND ContainerNumber is not null AND ContainerNumber <> ''''
--				group by ContainerNumber
--				OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
--				WHILE @@FETCH_STATUS = 0
--				BEGIN
--					if (@ContainerNumber is not null)
--					begin
--						if (@MySearchFields = '''') set @MySearchFields = @ContainerNumber
--						else set @MySearchFields = @MySearchFields + '','' + @ContainerNumber
--					end
--				FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
--				END
--				CLOSE PackagesCursor
--				DEALLOCATE PackagesCursor
--			END
--			-- Custom Fields
--			BEGIN
--			set @Field = @Field1
--			set @FieldName = ''Field1''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field2
--			set @FieldName = ''Field2''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field3
--			set @FieldName = ''Field3''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field4
--			set @FieldName = ''Field4''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field5
--			set @FieldName = ''Field5''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field6
--			set @FieldName = ''Field6''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field7
--			set @FieldName = ''Field7''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field8
--			set @FieldName = ''Field8''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field9
--			set @FieldName = ''Field9''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			set @Field = @Field10
--			set @FieldName = ''Field10''
--			if (@Field is not null AND @Field <> '''')
--			begin
--				select
--				@FieldDataTypeCode = ObjectFields.DataTypeCode
--				from ObjectFields join ObjectTables on ObjectFields.ObjectTableId = ObjectTables.Id
--				where ObjectFields.FieldName = @FieldName AND ObjectTables.Name = ''Shipment''
--				if (@FieldDataTypeCode = ''Text'' OR @FieldDataTypeCode = ''nText'')
--				begin
--				if (@MySearchFields = '''') set @MySearchFields = @Field
--				else set @MySearchFields = @MySearchFields + '','' + @Field
--				end
--			end
--			END
--       		insert into #temp_Shipments(Id, SearchFields) values (@Id, @MySearchFields)
--			set @Count = @Count + 1;
--			if(@Count = 4000)
--			begin
--				update Shipments
--				set
--				SearchFields = #temp_Shipments.SearchFields
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--				truncate table #temp_Shipments
--				set @Count = 0
--			end
--       FETCH NEXT FROM DataCursor INTO
--	   @Id, @Tenant, @ShipmentNumber, @ShipmentLevelCode, @TransportModeId,@DirectionId,
--		@StatusId, @QuoteId, @SalesmanUserId, @MasterShipmentDataId,
--		@FromPortId, @ToPortId, @PreCarriageFromPortId, @PreCarriageToPortId,
--		@OnCarriageFromPortId, @OnCarriageToPortId,
--		@House, @CustomFileNumber, @AWBCarrierTarrifReference,
--		@AgentId, @AgentReference1, @AgentReference2,
--		@ShipperId, @ShipperReference1, @ShipperReference2,
--		@ConsigneeId, @ConsigneeReference1, @ConsigneeReference2,
--		@CustomerId, @CustomerReference1, @CustomerReference2,
--		@Notify1Id,
--		@Notify2Id,
--		@IssuingCarrierAgentId,
--		@CustomAgentImportId, @CustomAgentImportReference,
--		@CustomAgentExportId, @CustomAgentExportReference,
--		@ShipperNotExporterId,
--		@ConsigneeNotImporterId,
--		@FreightForwarderId, @FreightForwarderReference,
--		@ConsolidatorId, @ConsolidatorReference,
--		@Field1, @Field2, @Field3, @Field4, @Field5, @Field6, @Field7, @Field8, @Field9, @Field10,
--		@CustomsDeclarationNumber, @ForwarderShipmentNumber, @TransportDocumentNumber,
--		@ReleasingAgentId, @ReleasingAgentReference1 , @ReleasingAgentReference2, @ProjectNumber,
--		@AMSBL, @WarehouseLegReference
--       END
--       CLOSE DataCursor
--       DEALLOCATE DataCursor
--    END
--	if (@Count > 0)
--	begin
--				update Shipments
--				set
--				SearchFields = #temp_Shipments.SearchFields
--				FROM Shipments
--				INNER JOIN #temp_Shipments
--				on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
--	end
--SET NOCOUNT OFF
--drop table #tempTable
--drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), '0eb7dd159b8e15fb3a4ba9e4dd98c2c9', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051540_AddVATUniquePartnerValues.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Code from VatUniquePartnerTypes where Code = 'ALL')
begin
insert into VatUniquePartnerTypes (Code, Name, SearchFields, ViewOrder)
values ('ALL', 'All Customers', 'ALL,All Customers', 0)
end
if not exists (select Code from VatUniquePartnerTypes where Code = 'CUS')
begin
insert into VatUniquePartnerTypes (Code, Name, SearchFields, ViewOrder)
values ('CUS', 'Active Customers Only', 'CUS,Active Customers Only', 0)
end
if not exists (select Code from VatUniquePartnerTypes where Code = 'POT')
begin
insert into VatUniquePartnerTypes (Code, Name, SearchFields, ViewOrder)
values ('POT', 'Potential Customers Only', 'POT,Potential Customers Only', 0)
end
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051540_AddVATUniquePartnerValues.sxml', GETDATE(), 'if not exists (select Code from VatUniquePartnerTypes where Code = ''ALL'')
begin
insert into VatUniquePartnerTypes (Code, Name, SearchFields, ViewOrder)
values (''ALL'', ''All Customers'', ''ALL,All Customers'', 0)
end
if not exists (select Code from VatUniquePartnerTypes where Code = ''CUS'')
begin
insert into VatUniquePartnerTypes (Code, Name, SearchFields, ViewOrder)
values (''CUS'', ''Active Customers Only'', ''CUS,Active Customers Only'', 0)
end
if not exists (select Code from VatUniquePartnerTypes where Code = ''POT'')
begin
insert into VatUniquePartnerTypes (Code, Name, SearchFields, ViewOrder)
values (''POT'', ''Potential Customers Only'', ''POT,Potential Customers Only'', 0)
end', DATEDIFF(MS,@StartTime,@EndTime), 'b993fc4e4b0a160bf91270b175302e0f', 3);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051545_SetVATUniquePartnerDefaultValue.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Tenants set VatUniquePartnerTypeCode = 'ALL'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051545_SetVATUniquePartnerDefaultValue.sxml', GETDATE(), 'update Tenants set VatUniquePartnerTypeCode = ''ALL''', DATEDIFF(MS,@StartTime,@EndTime), 'c385eb7d7d93a571f33003705585983b', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007061105_FillMissingPortCountryCode.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
UPDATE Ports SET CountryCode = (SELECT Code FROM Countries WHERE Id = CountryId) WHERE CountryCode IS NULL
UPDATE Ports SET CountryName = (SELECT EnglishName FROM Countries WHERE Id = CountryId) WHERE CountryName IS NULL
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007061105_FillMissingPortCountryCode.sxml', GETDATE(), 'UPDATE Ports SET CountryCode = (SELECT Code FROM Countries WHERE Id = CountryId) WHERE CountryCode IS NULL
UPDATE Ports SET CountryName = (SELECT EnglishName FROM Countries WHERE Id = CountryId) WHERE CountryName IS NULL', DATEDIFF(MS,@StartTime,@EndTime), 'a261682157a8891372c24bf22c0e218a', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007201431_FillWarehouseWeightClosedTables.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Code from WarehouseWeightMeasurements where Code = 'GRWT')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values ('GRWT', 'Gross Weight', 'GRWT,Gross Weight')
end
if not exists (select Code from WarehouseWeightMeasurements where Code = 'CHWT')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values ('CHWT', 'Chargeable Weight', 'CHWT,Chargeable Weight')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'NON')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('NON', 'None', 'NON,None', 'None')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'HAF')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('HAF', 'Half', 'HAF,Half', '0.5')
end
if not exists (select Code from WarehouseWeightRoundings where Code = 'ONE')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values ('ONE', 'One', 'ONE,One', '1')
end
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007201431_FillWarehouseWeightClosedTables.sxml', GETDATE(), 'if not exists (select Code from WarehouseWeightMeasurements where Code = ''GRWT'')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values (''GRWT'', ''Gross Weight'', ''GRWT,Gross Weight'')
end
if not exists (select Code from WarehouseWeightMeasurements where Code = ''CHWT'')
begin
insert into WarehouseWeightMeasurements (Code, Name, SearchFields)
values (''CHWT'', ''Chargeable Weight'', ''CHWT,Chargeable Weight'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''NON'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''NON'', ''None'', ''NON,None'', ''None'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''HAF'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''HAF'', ''Half'', ''HAF,Half'', ''0.5'')
end
if not exists (select Code from WarehouseWeightRoundings where Code = ''ONE'')
begin
insert into WarehouseWeightRoundings (Code, Name, SearchFields, Display)
values (''ONE'', ''One'', ''ONE,One'', ''1'')
end', DATEDIFF(MS,@StartTime,@EndTime), '5a31949cce6467cce78c87ba8d9dff83', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007201436_FillWarehouseFieldsDefaultValues.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Warehouses set AirWeightMeasurementCode = 'GRWT'
update Warehouses set OceanWeightMeasurementCode = 'GRWT'
update Warehouses set InlandWeightMeasurementCode = 'GRWT'
update Warehouses set AirWeightRoundingCode = 'NON'
update Warehouses set OceanWeightRoundingCode = 'NON'
update Warehouses set InlandWeightRoundingCode = 'NON'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007201436_FillWarehouseFieldsDefaultValues.sxml', GETDATE(), 'update Warehouses set AirWeightMeasurementCode = ''GRWT''
update Warehouses set OceanWeightMeasurementCode = ''GRWT''
update Warehouses set InlandWeightMeasurementCode = ''GRWT''
update Warehouses set AirWeightRoundingCode = ''NON''
update Warehouses set OceanWeightRoundingCode = ''NON''
update Warehouses set InlandWeightRoundingCode = ''NON''', DATEDIFF(MS,@StartTime,@EndTime), 'b17d9a558fa92bab0d7aece163282d61', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008051018_AddNewMeasurementAndChargesType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @MeasurementId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @VATTypeId as varchar(15)
declare @IATACodeId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
--Measurement
set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = 'STFE')
if (@MeasurementId is null)
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values('STFE', 'Storage Fee', 'Storage Fee', @MeasurementId, @Tenant, 0, 0, 0, 'STFE,Storage Fee,Storage Fee', 'Storage Fee')
end
--ChargesType
set @ChargesGroupCode = 'HNDCH'
set @ChargesGroupId = (select Id from ChargesGroups where Tenant = @Tenant and Code = 'HNDCH')
set @VATTypeId = (select Id from VatTypes where Tenant = @Tenant and Code = 'STD')
set @IATACodeId = (select Id from IATACodes where Code = 'SO')
if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = 'ISTOR')
begin
EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,'ChargesType'
insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable,
IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax, HasPickup, HasDelivery)
values('ISTOR', 'Import Storage', 'Import Storage', @ChargesTypeId, @Tenant, 0, 0, @ChargesGroupCode , @VATTypeId, 1,
0, 1, 1, 1, 0, 0, NULL, 0, 'AG',
0, @MeasurementId, NULL, 70,'ISTOR,Import Storage,Import Storage', NULL, NULL, 0,
NULL, NULL, NULL, NULL, NULL, NULL,
@ChargesGroupId ,NULL,0,0,
0,0,NULL,0,0,0,0,
NULL,NULL,0,0,0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'Measurement')
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'ChargesType')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008051018_AddNewMeasurementAndChargesType.sxml', GETDATE(), 'declare @Tenant as int
declare @MeasurementId as varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ChargesGroupId as varchar(15)
declare @ChargesGroupCode as varchar(5)
declare @VATTypeId as varchar(15)
declare @IATACodeId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
--Measurement
set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = ''STFE'')
if (@MeasurementId is null)
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,''Measurement''
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values(''STFE'', ''Storage Fee'', ''Storage Fee'', @MeasurementId, @Tenant, 0, 0, 0, ''STFE,Storage Fee,Storage Fee'', ''Storage Fee'')
end
--ChargesType
set @ChargesGroupCode = ''HNDCH''
set @ChargesGroupId = (select Id from ChargesGroups where Tenant = @Tenant and Code = ''HNDCH'')
set @VATTypeId = (select Id from VatTypes where Tenant = @Tenant and Code = ''STD'')
set @IATACodeId = (select Id from IATACodes where Code = ''SO'')
if not exists (select Id from ChargesTypes where Tenant = @Tenant and Code = ''ISTOR'')
begin
EXECUTE usp_GetNextTableIdValue @ChargesTypeId OUTPUT,''ChargesType''
insert into ChargesTypes(Code, EnglishName, LocalName, Id, Tenant, AddedManually, InActive, ChargesGroupCode, VatTypeId, IsReceivable,
IsPayable, IsAir, IsOcean, IsInland, IsAutoDisplayInShipment, IsAutoDisplayInConsolidation, Description, AWBPrintDescription, DueTypeCode,
IsAutoDisplayInQuote, MeasurementId, ContainerMeasurementId, ViewOrder, SearchFields, ReceivableAccountId, PayableAccountId, AccountingVATSplit,
ReceivableCreditAccount, PayableDebitAccount, ReceivablesChargesTypeExternalCode, IATACodeId, PayableDebitGLAcountId, ReceivableCreditGLAccountId,
ChargesGroupId,PayablesChargesTypeExternalCode,IsBackToBack,IsAutoDisplayInCustoms,
IsCustoms,IsExpense,SATExternalId,IsImport,IsDomestic,IsExport,IsDrop,
ReceivablesDefaultCurrencyId,PayablesDefaultCurrencyId,ApplyRegionalTax, HasPickup, HasDelivery)
values(''ISTOR'', ''Import Storage'', ''Import Storage'', @ChargesTypeId, @Tenant, 0, 0, @ChargesGroupCode , @VATTypeId, 1,
0, 1, 1, 1, 0, 0, NULL, 0, ''AG'',
0, @MeasurementId, NULL, 70,''ISTOR,Import Storage,Import Storage'', NULL, NULL, 0,
NULL, NULL, NULL, NULL, NULL, NULL,
@ChargesGroupId ,NULL,0,0,
0,0,NULL,0,0,0,0,
NULL,NULL,0,0,0)
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''Measurement'')
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''ChargesType'')', DATEDIFF(MS,@StartTime,@EndTime), 'b252e3857be84e566299715d80b06883', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008110941_AddNewMeasurement.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @MeasurementId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'PDCW')
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values('PDCW', 'Pickup/Delivery Chargeable weight', 'Pickup/Delivery Chargeable weight', @MeasurementId, @Tenant, 0, 0, 0, 'PDCW,Pickup/Delivery Chargeable weight,Pickup Delivery Chargeable weight', 'Pickup/Delivery Chargeable weight')
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008110941_AddNewMeasurement.sxml', GETDATE(), 'declare @Tenant as int
declare @MeasurementId as varchar(15)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
if not exists (select Id from Measurements where Tenant = @Tenant and Code = ''PDCW'')
begin
EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,''Measurement''
insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
values(''PDCW'', ''Pickup/Delivery Chargeable weight'', ''Pickup/Delivery Chargeable weight'', @MeasurementId, @Tenant, 0, 0, 0, ''PDCW,Pickup/Delivery Chargeable weight,Pickup Delivery Chargeable weight'', ''Pickup/Delivery Chargeable weight'')
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '34b9d9902a50650ec4ac4fa4aa87bfe1', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 20200812_SetIATACodeToSRForImportStorageCharge.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update ChargesTypes
set IATACodeId = (select Id from IATACodes where Code = 'SR')
where Code = 'ISTOR'
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = 'ChargesType')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('20200812_SetIATACodeToSRForImportStorageCharge.sxml', GETDATE(), 'update ChargesTypes
set IATACodeId = (select Id from IATACodes where Code = ''SR'')
where Code = ''ISTOR''
update ObjectTableLastUpdates set LastUpdateDate = GETDATE() where ObjectTableId = (select id from ObjectTables where Name = ''ChargesType'')', DATEDIFF(MS,@StartTime,@EndTime), '60d427a5e1ea664ef4e7d5766423a171', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008261425_SetToggleCodeForHorsesFeature.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if not exists (select Code from Toggles where Code = 'HRS')
begin
insert into Toggles (Code, Name, SearchFields)
values ('HRS', 'Horse', 'HRS,Horse')
end
update Features
set ToggleCode = 'HRS'
where Code = 'Horse.M.Horses'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008261425_SetToggleCodeForHorsesFeature.sxml', GETDATE(), 'if not exists (select Code from Toggles where Code = ''HRS'')
begin
insert into Toggles (Code, Name, SearchFields)
values (''HRS'', ''Horse'', ''HRS,Horse'')
end
update Features
set ToggleCode = ''HRS''
where Code = ''Horse.M.Horses''', DATEDIFF(MS,@StartTime,@EndTime), '3e01d0de48c070160eed409222a814c8', 2);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202008300915_AddHorsesEventTypesToTenants.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = 'QuoteClosingReason')
set @Code = 'HRIN'
set @Name = 'Set as Inactive'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = 'HRRC'
set @Name = 'Reactivated'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202008300915_AddHorsesEventTypesToTenants.sxml', GETDATE(), 'declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = ''QuoteClosingReason'')
set @Code = ''HRIN''
set @Name = ''Set as Inactive''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = ''HRRC''
set @Name = ''Reactivated''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '5f18abe5124698d5536097a0706ca9fb', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202009011450_ImportToUSADropMaman.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
delete from CustomsInterfaces where code = 'CMN'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202009011450_ImportToUSADropMaman.sxml', GETDATE(), 'delete from CustomsInterfaces where code = ''CMN''', DATEDIFF(MS,@StartTime,@EndTime), '830638ae226d14b30e478d0b45d1f45b', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From BuildSearchKeywordFunction.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N'[dbo].[BuildSearchKeywordFunction]'))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = 'CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit VARCHAR(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name NVARCHAR(255)
DECLARE @pos INT
if(@stringToSplit!='' '') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('' '', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('' '', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='' '')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='' '')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END'
EXEC(@dateString)
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('BuildSearchKeywordFunction.sxml', GETDATE(), 'IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N''[dbo].[BuildSearchKeywordFunction]''))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = ''CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit VARCHAR(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name NVARCHAR(255)
DECLARE @pos INT
if(@stringToSplit!='''' '''') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('''' '''', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('''' '''', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='''' '''')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='''' '''')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END''
EXEC(@dateString)', DATEDIFF(MS,@StartTime,@EndTime), '4245f55e25e3724f77cf861fca34af15', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From FillCardSearchData.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
TRUNCATE table CardSearches
If(OBJECT_ID('tempdb..#temp_CardSearches') Is Not Null)
Begin
Drop Table #temp_CardSearches
End
CREATE TABLE #temp_CardSearches
(
[Tenant] [int] NOT NULL,
[RecordDate] [datetime] NOT NULL,
[Keyword] [nvarchar](100) NULL,
[Weight] [int] NOT NULL,
[CardId] [varchar](15) NULL,
[PartnerTypeId] [varchar](2) not NULL,
[InActive] bit,
)
declare  @Tenant int
declare @Count as int
set @Count = 0;
declare  @CardId varchar(15)
declare  @EnglishName varchar(70)
declare  @LocalName nvarchar(100)
declare  @VatNumber varchar(20)
declare  @CityName nvarchar(25)
declare  @CountryName varchar(120)
declare  @Code varchar(15)
declare  @ReceivablesAccountingCard varchar(25)
declare  @PayablesAccountingCard varchar(25)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
Declare @SearchField nvarchar(max)
DECLARE CardCursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant, Code,EnglishName , LocalName , VatNumber ,CityName , CountryName , ReceivablesAccountingCard , PayablesAccountingCard , CreateDate ,UpdateDate , PartnerTypeId , InActive
From Cards
OPEN CardCursor FETCH NEXT FROM CardCursor INTO  @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive
WHILE @@FETCH_STATUS = 0
BEGIN
begin
set @Weight = 0
DECLARE  @newId varchar(100) ;
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
BEGIN TRY
if (@Code is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@Code , 90 , 90) t where KeyWord !=' ' end
if (@EnglishName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@EnglishName , 100 , 90) t where KeyWord !=' ' end
if (@LocalName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@LocalName , 100 , 90) t where KeyWord !=' ' end
if (@VatNumber is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@VatNumber , 100 , 100) t where KeyWord !=' ' end
if (@CountryName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CountryName , 50 , 50) t where KeyWord !=' ' end
if (@CityName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CityName , 40 , 40) t where KeyWord !=' ' end
if (@ReceivablesAccountingCard is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ReceivablesAccountingCard , 80 , 80) t where KeyWord !=' ' end
if (@PayablesAccountingCard is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@PayablesAccountingCard , 80 , 80) t where KeyWord !=' ' end
set @Count = @Count + 1;
if(@Count = 500000)
begin
insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive from #temp_CardSearches
truncate table #temp_CardSearches
set @Count = 0
end
END TRY
BEGIN CATCH
declare @Exception as varchar(4000)
set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);
set @Exception = @Exception + ' (@CardId: ' + @CardId +') '+ ' (@Tenant: ' + CAST(@Tenant as varchar(100)) + ' )'
RAISERROR(@Exception, 16, 3);
RETURN;
END CATCH
end
FETCH NEXT FROM CardCursor INTO @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive
END
CLOSE CardCursor
DEALLOCATE CardCursor
if (@Count > 0) begin  insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive from #temp_CardSearches end
drop table #temp_CardSearches
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('FillCardSearchData.sxml', GETDATE(), 'TRUNCATE table CardSearches
If(OBJECT_ID(''tempdb..#temp_CardSearches'') Is Not Null)
Begin
Drop Table #temp_CardSearches
End
CREATE TABLE #temp_CardSearches
(
[Tenant] [int] NOT NULL,
[RecordDate] [datetime] NOT NULL,
[Keyword] [nvarchar](100) NULL,
[Weight] [int] NOT NULL,
[CardId] [varchar](15) NULL,
[PartnerTypeId] [varchar](2) not NULL,
[InActive] bit,
)
declare  @Tenant int
declare @Count as int
set @Count = 0;
declare  @CardId varchar(15)
declare  @EnglishName varchar(70)
declare  @LocalName nvarchar(100)
declare  @VatNumber varchar(20)
declare  @CityName nvarchar(25)
declare  @CountryName varchar(120)
declare  @Code varchar(15)
declare  @ReceivablesAccountingCard varchar(25)
declare  @PayablesAccountingCard varchar(25)
declare  @CreateDate datetime
declare  @UpdateDate datetime
declare  @Weight int
declare  @PartnerTypeId varchar(2)
declare  @InActive bit
Declare @SearchField nvarchar(max)
DECLARE CardCursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant, Code,EnglishName , LocalName , VatNumber ,CityName , CountryName , ReceivablesAccountingCard , PayablesAccountingCard , CreateDate ,UpdateDate , PartnerTypeId , InActive
From Cards
OPEN CardCursor FETCH NEXT FROM CardCursor INTO  @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive
WHILE @@FETCH_STATUS = 0
BEGIN
begin
set @Weight = 0
DECLARE  @newId varchar(100) ;
declare  @RecordDate datetime
set @RecordDate = @UpdateDate;
if(@RecordDate is null) set @RecordDate = @CreateDate
BEGIN TRY
if (@Code is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@Code , 90 , 90) t where KeyWord !='' '' end
if (@EnglishName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@EnglishName , 100 , 90) t where KeyWord !='' '' end
if (@LocalName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@LocalName , 100 , 90) t where KeyWord !='' '' end
if (@VatNumber is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@VatNumber , 100 , 100) t where KeyWord !='' '' end
if (@CountryName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CountryName , 50 , 50) t where KeyWord !='' '' end
if (@CityName is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@CityName , 40 , 40) t where KeyWord !='' '' end
if (@ReceivablesAccountingCard is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@ReceivablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
if (@PayablesAccountingCard is not null)	begin 		 insert into #temp_CardSearches (Tenant, CardId  , RecordDate  , PartnerTypeId,InActive , Keyword , Weight) select  @Tenant, @CardId , @RecordDate  , @PartnerTypeId,@InActive ,  t.Keyword  , t.weight from dbo.BuildSearchKeywordFunction(@PayablesAccountingCard , 80 , 80) t where KeyWord !='' '' end
set @Count = @Count + 1;
if(@Count = 500000)
begin
insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive from #temp_CardSearches
truncate table #temp_CardSearches
set @Count = 0
end
END TRY
BEGIN CATCH
declare @Exception as varchar(4000)
set @Exception = (SELECT   ERROR_MESSAGE() AS ErrorMessage);
set @Exception = @Exception + '' (@CardId: '' + @CardId +'') ''+ '' (@Tenant: '' + CAST(@Tenant as varchar(100)) + '' )''
RAISERROR(@Exception, 16, 3);
RETURN;
END CATCH
end
FETCH NEXT FROM CardCursor INTO @CardId,@Tenant, @Code,@EnglishName , @LocalName , @VatNumber , @CityName , @CountryName , @ReceivablesAccountingCard , @PayablesAccountingCard , @CreateDate , @UpdateDate , @PartnerTypeId,@InActive
END
CLOSE CardCursor
DEALLOCATE CardCursor
if (@Count > 0) begin  insert into CardSearches (Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive) select  Tenant, CardId , Keyword , RecordDate , Weight , PartnerTypeId,InActive from #temp_CardSearches end
drop table #temp_CardSearches', DATEDIFF(MS,@StartTime,@EndTime), '6c91e4308f4a5bed0d0b0307de05ac72', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006291330_FillPaidDateOfInvoices.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
-- not used, run manually
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = '-- not used, run manually', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '9c7d4b76a39e0183af6d3eaef3c13a20', [Version] = 4 WHERE [SxmlFileName] = '202006291330_FillPaidDateOfInvoices.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011347_FillQuoteClosingReasonTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if exists (SELECT * FROM sys.tables WHERE name='QuoteClosingReasons')
begin
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = 'system@tenant'+ @TenantString + '.com'
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'EQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('EQ', 'Expensive Quote', 'EQ,Expensive Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'GS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('GS', 'Given directly to the Shipping Line', 'GS,Given directly to the Shipping Line', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LC')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LC', 'Lost to Competitor', 'LC,Lost to Competitor', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LS', 'Lack of Service in the Last Shipment', 'LS,Lack of Service in the Last Shipment', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'XQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('XQ', 'Expired Quote', 'XQ,Expired Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'BM')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('BM', 'Benchmarking', 'BM,Benchmarking', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LT')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LT', 'Long Term Project', 'LT,Long Term Project', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
End
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'if exists (SELECT * FROM sys.tables WHERE name=''QuoteClosingReasons'')
begin
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = ''system@tenant''+ @TenantString + ''.com''
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''EQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''EQ'', ''Expensive Quote'', ''EQ,Expensive Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''GS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''GS'', ''Given directly to the Shipping Line'', ''GS,Given directly to the Shipping Line'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LC'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LC'', ''Lost to Competitor'', ''LC,Lost to Competitor'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LS'', ''Lack of Service in the Last Shipment'', ''LS,Lack of Service in the Last Shipment'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''XQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''XQ'', ''Expired Quote'', ''XQ,Expired Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''BM'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''BM'', ''Benchmarking'', ''BM,Benchmarking'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LT'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LT'', ''Long Term Project'', ''LT,Long Term Project'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
End', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '9a21026461ef1ddc105994a664531592', [Version] = 3 WHERE [SxmlFileName] = '202006011347_FillQuoteClosingReasonTable.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011458_FillQuoteClosingReasonId.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
if exists (SELECT * FROM sys.tables WHERE name='QuoteClosingReasons')
begin
update Quotes
set QuoteClosingReasonId = (select Id from QuoteClosingReasons where Code = Quotes.QuoteClosingReasonCode and Tenant = Quotes.Tenant )
End
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'if exists (SELECT * FROM sys.tables WHERE name=''QuoteClosingReasons'')
begin
update Quotes
set QuoteClosingReasonId = (select Id from QuoteClosingReasons where Code = Quotes.QuoteClosingReasonCode and Tenant = Quotes.Tenant )
End', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '3e7a89b90aa642f4b6f0d8f49384920e', [Version] = 2 WHERE [SxmlFileName] = '202006011458_FillQuoteClosingReasonId.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006151500_FillAirQuoteShipmentTypeField.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '0f315678c7cb8ac070f70fd277f9a5c2', [Version] = 35435 WHERE [SxmlFileName] = '202006151500_FillAirQuoteShipmentTypeField.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006151503_FillQuoteShipmentSubTypeBackward.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'f51b19219b64a436bc97a4c53f011604', [Version] = 2 WHERE [SxmlFileName] = '202006151503_FillQuoteShipmentSubTypeBackward.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007091327_UpdateAirQuotesSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Quotes
set
ShipmentTypeId = 'Air',
ShipmentSubTypeId = (select top 1 Id from ShipmentSubTypes where Code = 'Air' and Tenant = Quotes.Tenant)
where TransportModeId = 'A'
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007091327_UpdateAirQuotesSubType.sxml', GETDATE(), 'update Quotes
set
ShipmentTypeId = ''Air'',
ShipmentSubTypeId = (select top 1 Id from ShipmentSubTypes where Code = ''Air'' and Tenant = Quotes.Tenant)
where TransportModeId = ''A''', DATEDIFF(MS,@StartTime,@EndTime), '78425fec5fbe9daa77a54e2713ece129', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006111515_AddShipmentSubTypesToTenants.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
declare @ShipmentTypeCode as varchar(4)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = 'system@tenant'+ @TenantString + '.com'
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'Air')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'Air')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'Air', 'Air', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'Air,Air')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'FCL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'FCLD')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'FCL', 'FCL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'FCL,FCL')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'LCL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'LCLD')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'LCL', 'LCL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'LCL,LCL')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'FTL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'FTL')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'FTL', 'FTL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'FTL,FTL')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'LTL')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'LTL')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'LTL', 'LTL', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'LTL,LTL')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'MyGI')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'MyGI')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'MyGI', 'My Groupage Inland', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'MyGI,My Groupage Inland')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = 'MyGO')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = 'MyGO')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'ShipmentSubType'
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  'MyGO', 'My Groupage Ocean', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, 'MyGO,My Groupage Ocean')
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
declare @ShipmentTypeCode as varchar(4)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = ''system@tenant''+ @TenantString + ''.com''
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''Air'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''Air'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''Air'', ''Air'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''Air,Air'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''FCL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''FCLD'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''FCL'', ''FCL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''FCL,FCL'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''LCL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''LCLD'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''LCL'', ''LCL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''LCL,LCL'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''FTL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''FTL'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''FTL'', ''FTL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''FTL,FTL'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''LTL'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''LTL'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''LTL'', ''LTL'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''LTL,LTL'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''MyGI'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''MyGI'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''MyGI'', ''My Groupage Inland'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''MyGI,My Groupage Inland'')
end
if not exists (select Id from ShipmentSubTypes where Tenant = @Tenant and Code = ''MyGO'')
begin
set @ShipmentTypeCode = (select Id from ShipmentTypes where Id = ''MyGO'')
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''ShipmentSubType''
insert into ShipmentSubTypes(Id, Tenant, Code, Name, ShipmentTypeCode, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive, SearchFields)
values(@EntityId, @Tenant,  ''MyGO'', ''My Groupage Ocean'', @ShipmentTypeCode, GETDATE(), GETDATE(), @UserId, @UserId, 0, ''MyGO,My Groupage Ocean'')
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = 'edf4b5245328396929db6714829ebe59', [Version] = 2 WHERE [SxmlFileName] = '202006111515_AddShipmentSubTypesToTenants.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006141227_FillShipmentSubTypeBackward.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'NULL', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '18f11896d7c6c546efed183e4048901a', [Version] = 4 WHERE [SxmlFileName] = '202006141227_FillShipmentSubTypeBackward.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007051322_UpdateNotAirShipmentsSubType.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
If(OBJECT_ID('tempdb..#tempTable') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID('tempdb..#temp_Shipments') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = 'O' and ShipmentTypeId = 'FCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FCL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'LCLD' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LCL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'O' and ShipmentTypeId = 'MyGO' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGO' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'FTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'FTL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'LTL' THEN (select top 1 Id from ShipmentSubTypes where Code = 'LTL' and Tenant = Shipments.Tenant)
WHEN TransportModeId = 'I' and ShipmentTypeId = 'MyGI' THEN (select top 1 Id from ShipmentSubTypes where Code = 'MyGI' and Tenant = Shipments.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Shipments where TransportModeId <> 'A'
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
drop table #tempTable
drop table #temp_Shipments
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007051322_UpdateNotAirShipmentsSubType.sxml', GETDATE(), 'If(OBJECT_ID(''tempdb..#tempTable'') Is Not Null)
Begin
Drop Table #tempTable
End
If(OBJECT_ID(''tempdb..#temp_Shipments'') Is Not Null)
Begin
Drop Table #temp_Shipments
End
CREATE TABLE #temp_Shipments (
Id varchar(15) not null ,
ShipmentSubTypeId varchar(15)  null,
ShipmentTypeId varchar(15)  null
)
select
Id,
Tenant,
TransportModeId,
ShipmentTypeId,
(
CASE
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''FCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FCL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''LCLD'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LCL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''O'' and ShipmentTypeId = ''MyGO'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGO'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''FTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''FTL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''LTL'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''LTL'' and Tenant = Shipments.Tenant)
WHEN TransportModeId = ''I'' and ShipmentTypeId = ''MyGI'' THEN (select top 1 Id from ShipmentSubTypes where Code = ''MyGI'' and Tenant = Shipments.Tenant)
END
) as ShipmentSubTypeId
into #tempTable
FROM Shipments where TransportModeId <> ''A''
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipmentTypeId as varchar(4)
declare @TransportModeId as varchar(4)
declare @ShipmentSubTypeId as varchar(15)
declare @Count as int
set @Count = 0;
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TransportModeId, ShipmentTypeId, ShipmentSubTypeId
FROM #tempTable
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
insert into #temp_Shipments(Id, ShipmentSubTypeId, ShipmentTypeId) values (@EntityId, @ShipmentSubTypeId, @ShipmentTypeId)
set @Count = @Count + 1;
if(@Count = 4000)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
truncate table #temp_Shipments
set @Count = 0
end
FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @TransportModeId, @ShipmentTypeId, @ShipmentSubTypeId
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
if (@Count > 0)
begin
update Shipments
set
ShipmentSubTypeId = #temp_Shipments.ShipmentSubTypeId,
ShipmentTypeId = #temp_Shipments.ShipmentTypeId
FROM Shipments
INNER JOIN #temp_Shipments
on Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS= #temp_Shipments.Id COLLATE SQL_Latin1_General_CP1_CI_AS
end
drop table #tempTable
drop table #temp_Shipments', DATEDIFF(MS,@StartTime,@EndTime), 'd83b69dccd89385b36d4782d9c487e31', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202007151230_FixNULLChargeableWeightForWarehouseEntries.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT ChargeableWeightUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND WarehouseEntries.TransportModeId = 'A'
Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT WeightMeasurementUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND (WarehouseEntries.TransportModeId = 'O' OR WarehouseEntries.TransportModeId = 'I')
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202007151230_FixNULLChargeableWeightForWarehouseEntries.sxml', GETDATE(), 'Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT ChargeableWeightUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND WarehouseEntries.TransportModeId = ''A''
Update WarehouseEntries SET ChargeableWeightUnitCode = (SELECT WeightMeasurementUnitCode FROM Tenants AS t WHERE WarehouseEntries.Tenant = t.Id) WHERE WarehouseEntries.ChargeableWeightUnitCode IS NULL AND (WarehouseEntries.TransportModeId = ''O'' OR WarehouseEntries.TransportModeId = ''I'')', DATEDIFF(MS,@StartTime,@EndTime), 'a6ef11f2fadee0049efaeb0d7cb2e1fb', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

