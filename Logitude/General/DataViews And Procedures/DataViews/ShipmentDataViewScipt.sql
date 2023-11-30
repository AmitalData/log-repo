
drop VIEW [dbo].[ShipmentDataView]

/****** Object:  View [dbo].[ShipmentDataView]    Script Date: 09/17/2017 16:36:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create VIEW [dbo].[ShipmentDataView]

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
                         dbo.Shipments.Field41, dbo.Shipments.Field42, dbo.Shipments.Field43, dbo.Shipments.Field44, dbo.Shipments.Field45,
                         dbo.Shipments.Field46, dbo.Shipments.Field47, dbo.Shipments.Field48, dbo.Shipments.Field49, dbo.Shipments.Field50,
                         dbo.Shipments.Field51, dbo.Shipments.Field52, dbo.Shipments.Field53, dbo.Shipments.Field54, dbo.Shipments.Field55,
                         dbo.Shipments.Field56, dbo.Shipments.Field57, dbo.Shipments.Field58, dbo.Shipments.Field59, dbo.Shipments.Field60,
                         dbo.Shipments.Field61, dbo.Shipments.Field62, dbo.Shipments.Field63, dbo.Shipments.Field64, dbo.Shipments.Field65, 
                         dbo.Shipments.Field66, dbo.Shipments.Field67, dbo.Shipments.Field68, dbo.Shipments.Field69, dbo.Shipments.Field70,
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
						 dbo.Shipments.WarehouseLegLastFreeDate,dbo.Shipments.WarehouseLegExpectedReleaseDate,dbo.Shipments.WarehouseLegActualReleaseDate, dbo.Shipments.WarehouseLegRemarks, dbo.Shipments.WarehouseLegActualEntryDate,dbo.Shipments.WarehouseLegReference,
						 WarehouseLegCard.EnglishName AS WarehouseLegTerminalName,
						 dbo.Shipments.LastSharedEventId, dbo.Shipments.LastSharedEventLocation, dbo.Shipments.LastSharedEventNotes, dbo.Shipments.LastSharedEventDate,						 
                         dbo.EventTypes.EnglishName as LastSharedEventName,
						 WarehouseLegCard.CountryCode AS WarehouseLegAddressCountryCode,
						 WarehouseLegCard.CountryName AS WarehouseLegAddressCountryName,
						 dbo.Shipments.WarehouseStorageFreeDays AS WarehouseStorageFreeDays,
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
                         ShipmentComputedFields.IsDocumentsNeedApprove as IsDocumentsNeedApprove,
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
						CASE WHEN ( NOT ((dbo.ShipmentTypes.Name IS NULL) OR ((LEN(dbo.ShipmentTypes.Name)) = 0))) THEN CASE WHEN (dbo.ShipmentTypes.Name IS NULL) THEN N'' ELSE dbo.ShipmentTypes.Name END + N' ' + CASE WHEN (dbo.ShipmentLevels.Name IS NULL) THEN N'' ELSE dbo.ShipmentLevels.Name END ELSE dbo.ShipmentLevels.Name END AS ShipmentType, 
						CASE WHEN ( NOT ((MainCarriageFromPortId IS NULL) OR ((LEN(MainCarriageFromPortId)) = 0))) THEN MainCarriageFromPortId ELSE dbo.Shipments.FromPortId END AS FromPortId, 
						CASE WHEN ( NOT ((MainCarriageToPortId IS NULL) OR ((LEN(MainCarriageToPortId)) = 0))) THEN MainCarriageToPortId ELSE dbo.Shipments.ToPortId END AS ToPortId,
						CASE WHEN ( NOT ((MainCarriageFromPorts.Code IS NULL) OR ((LEN(MainCarriageFromPorts.Code)) = 0))) THEN MainCarriageFromPorts.Code ELSE FromPorts.Code END AS FromPort, 
						CASE WHEN ( NOT ((MainCarriageFromPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFromPorts.EnglishName)) = 0))) THEN MainCarriageFromPorts.EnglishName ELSE FromPorts.EnglishName END AS FromPortName, 
						CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.Code IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.Code)) = 0))) THEN MainCarriageFinalDestinationPorts.Code ELSE ToPorts.Code END AS ToPort, 
						CASE WHEN ( NOT ((MainCarriageFinalDestinationPorts.EnglishName IS NULL) OR ((LEN(MainCarriageFinalDestinationPorts.EnglishName)) = 0))) THEN MainCarriageFinalDestinationPorts.EnglishName ELSE ToPorts.EnglishName END AS ToPortName, 
						CASE WHEN ('A' = dbo.Shipments.TransportModeId) THEN CASE WHEN (MainCarriageCarrierCards.Code IS NULL) THEN N'' ELSE MainCarriageCarrierCards.Code END + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'' ELSE MainCarriageCarrierNumber END WHEN ('O' = dbo.Shipments.TransportModeId) THEN CASE WHEN (dbo.Vessels.EnglishName IS NULL) THEN N'' ELSE dbo.Vessels.EnglishName END + N'/' + CASE WHEN (MainCarriageCarrierNumber IS NULL) THEN N'' ELSE MainCarriageCarrierNumber END WHEN ('I' = dbo.Shipments.TransportModeId) THEN MainCarriageCarrierNumber END AS CarrierNumber, 
						CASE WHEN (MainCarriageATA IS NOT NULL) THEN MainCarriageATA ELSE MainCarriageETA END AS MainCarriageExpectedOrActual, 
						CASE WHEN (MainCarriageATA IS NOT NULL) THEN N'ATA' ELSE N'ETA' END AS MainCarriageETAOrATA, 
						CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Id ELSE dbo.EntityStatus.Id END ELSE dbo.EntityStatus.Id END AS StatusId, 
                        dbo.Shipments.ComputedStatusDate AS StatusDate, 
						CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN ShipmentMasterDataEntityStatus.Name ELSE dbo.EntityStatus.Name END ELSE dbo.EntityStatus.Name END AS StatusName, 
						CASE WHEN (MasterShipmentDataId IS NOT NULL) THEN CASE WHEN (ShipmentMasterDataEntityStatus.StatusWeight > dbo.EntityStatus.StatusWeight) THEN dbo.ShipmentMasterDatas.StatusLocation ELSE dbo.Shipments.StatusLocation END ELSE dbo.Shipments.StatusLocation END AS StatusLocation, 
						CASE WHEN ('A' = dbo.Shipments.TransportModeId) THEN CASE WHEN (( NOT ((AirlinePrefix IS NULL) OR ((LEN(AirlinePrefix)) = 0))) AND ( NOT ((Master IS NULL) OR ((LEN(Master)) = 0)))) THEN CASE WHEN (AirlinePrefix IS NULL) THEN N'' ELSE AirlinePrefix END + N'-' + CASE WHEN (Master IS NULL) THEN N'' ELSE Master END ELSE N'' END ELSE Master END AS LongMaster, 
						CAST( MissingDocumentsCount AS nvarchar(max)) + N' Missing' AS MissingDocumentsCountWords, 
						CASE WHEN (IsOperationalClosed = 1) THEN N'Archived' ELSE N'' END AS ArchivedText
						
						--CASE WHEN (dbo.Shipments.TransportModeId = 'I' AND dbo.Shipments.DirectionId = 'D') THEN MainCarriageFromAddressesStates.EnglishName
						--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = 'H' AND dbo.Shipments.MasterShipmentDataId is null) THEN FromPortsStates.EnglishName
						--ELSE MainCarriageFromPortsStates.EnglishName END) END AS MainCarriageFromState,

						--CASE WHEN (dbo.Shipments.TransportModeId = 'I' AND dbo.Shipments.DirectionId = 'D') THEN MainCarriageToAddressesStates.EnglishName
						--ELSE (CASE WHEN (dbo.Shipments.ShipmentLevelCode = 'H' AND dbo.Shipments.MasterShipmentDataId is null) THEN ToPortsStates.EnglishName
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
						 dbo.States AS MainCarriageFinalDestinationPortsStates ON MainCarriageFinalDestinationPorts.StateId = MainCarriageFinalDestinationPortsStates.Id


GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[45] 4[4] 2[42] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -1248
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Shipments"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 333
            End
            DisplayFlags = 280
            TopColumn = 235
         End
         Begin Table = "ShipmentMasterDatas"
            Begin Extent = 
               Top = 6
               Left = 371
               Bottom = 135
               Right = 672
            End
            DisplayFlags = 280
            TopColumn = 51
         End
         Begin Table = "MainCarriageFromPorts"
            Begin Extent = 
               Top = 6
               Left = 710
               Bottom = 135
               Right = 883
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageToPorts"
            Begin Extent = 
               Top = 6
               Left = 921
               Bottom = 135
               Right = 1094
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment1FromPorts"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 267
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment1ToPorts"
            Begin Extent = 
               Top = 138
               Left = 249
               Bottom = 267
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment2FromPorts"
            Begin Extent = 
               Top = 138
               Left = 460
               ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'Bottom = 267
               Right = 633
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment2ToPorts"
            Begin Extent = 
               Top = 138
               Left = 671
               Bottom = 267
               Right = 844
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Transshipment3FromPorts"
            Begin Extent = 
               Top = 138
               Left = 882
               Bottom = 267
               Right = 1055
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment3ToPorts"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 399
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageFinalDestinationPorts"
            Begin Extent = 
               Top = 1209
               Left = 1365
               Bottom = 1338
               Right = 1538
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PreCarriageFromPorts"
            Begin Extent = 
               Top = 270
               Left = 249
               Bottom = 399
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PreCarriageToPorts"
            Begin Extent = 
               Top = 270
               Left = 460
               Bottom = 399
               Right = 633
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OnCarriageFromPorts"
            Begin Extent = 
               Top = 270
               Left = 671
               Bottom = 399
               Right = 844
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OnCarriageToPorts"
            Begin Extent = 
               Top = 270
               Left = 882
               Bottom = 399
               Right = 1055
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ToPorts"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 531
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "FromPorts"
            Begin Extent = 
               Top = 402
               Left = 249
               Bottom = 531
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "CustomerCards"
            Begin Extent = 
               Top = 402
               Left = 460
               Bottom = 531
               Right = 636
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FreightForwarderCards"
            Begin Extent = 
               Top = 402
               Left = 674
               Bottom = 531
               Right = 850
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ShipperCards"
            Begin Extent = 
               Top = 402
               Left = 888
               Bottom = 531
               Right = 1064
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ConsigneeCards"
         ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane3', @value=N'   Begin Extent = 
               Top = 534
               Left = 38
               Bottom = 663
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AgentCards"
            Begin Extent = 
               Top = 534
               Left = 252
               Bottom = 663
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CustomAgentExportCards"
            Begin Extent = 
               Top = 534
               Left = 466
               Bottom = 663
               Right = 642
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CustomAgentImportCards"
            Begin Extent = 
               Top = 534
               Left = 680
               Bottom = 663
               Right = 856
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "Notify1Cards"
            Begin Extent = 
               Top = 534
               Left = 894
               Bottom = 663
               Right = 1070
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Notify2Cards"
            Begin Extent = 
               Top = 666
               Left = 38
               Bottom = 795
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ShipperNotExporterCards"
            Begin Extent = 
               Top = 666
               Left = 252
               Bottom = 795
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ConsigneeNotImporterCards"
            Begin Extent = 
               Top = 666
               Left = 466
               Bottom = 795
               Right = 642
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageFromCountries"
            Begin Extent = 
               Top = 666
               Left = 680
               Bottom = 795
               Right = 853
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageToCountries"
            Begin Extent = 
               Top = 666
               Left = 891
               Bottom = 795
               Right = 1064
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment1FromCountries"
            Begin Extent = 
               Top = 798
               Left = 38
               Bottom = 927
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment2FromCountries"
            Begin Extent = 
               Top = 798
               Left = 249
               Bottom = 927
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment3FromCountries"
            Begin Extent = 
               Top = 798
               Left = 460
               Bottom = 927
               Right = 633
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PreCarriageFromCountries"
            Begin Extent = 
               Top = 798
               Left = 671
               Bottom = 927
               Right = 844
            ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane4', @value=N'End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OnCarriageFromCountries"
            Begin Extent = 
               Top = 798
               Left = 882
               Bottom = 927
               Right = 1055
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment1ToCountries"
            Begin Extent = 
               Top = 930
               Left = 38
               Bottom = 1059
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment2ToCountries"
            Begin Extent = 
               Top = 930
               Left = 249
               Bottom = 1059
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment3ToCountries"
            Begin Extent = 
               Top = 930
               Left = 460
               Bottom = 1059
               Right = 633
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PreCarriageToCountries"
            Begin Extent = 
               Top = 930
               Left = 671
               Bottom = 1059
               Right = 844
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OnCarriageToCountries"
            Begin Extent = 
               Top = 930
               Left = 882
               Bottom = 1059
               Right = 1055
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FromPortCountries"
            Begin Extent = 
               Top = 1458
               Left = 38
               Bottom = 1587
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "ToPortCountries"
            Begin Extent = 
               Top = 1500
               Left = 457
               Bottom = 1629
               Right = 630
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "MainCarriageCarrierCards"
            Begin Extent = 
               Top = 1062
               Left = 38
               Bottom = 1191
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment1CarrierCards"
            Begin Extent = 
               Top = 1062
               Left = 252
               Bottom = 1191
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment2CarrierCards"
            Begin Extent = 
               Top = 1062
               Left = 466
               Bottom = 1191
               Right = 642
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Transshipment3CarrierCards"
            Begin Extent = 
               Top = 1062
               Left = 680
               Bottom = 1191
               Right = 856
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PreCarriageCarrierCards"
            Begin Extent = 
               Top = 1062
               Left = 894
               Bottom = 1191
               Right = 1070
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "On' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane5', @value=N'CarriageCarrierCards"
            Begin Extent = 
               Top = 1194
               Left = 38
               Bottom = 1323
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "NextLegs"
            Begin Extent = 
               Top = 1194
               Left = 252
               Bottom = 1289
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Directions"
            Begin Extent = 
               Top = 1194
               Left = 460
               Bottom = 1289
               Right = 630
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransportModes"
            Begin Extent = 
               Top = 1194
               Left = 668
               Bottom = 1289
               Right = 838
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ShipmentTypes"
            Begin Extent = 
               Top = 1194
               Left = 876
               Bottom = 1306
               Right = 1057
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ShipmentReceivableStatus"
            Begin Extent = 
               Top = 1290
               Left = 252
               Bottom = 1385
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ShipmentPayableStatus"
            Begin Extent = 
               Top = 1290
               Left = 460
               Bottom = 1385
               Right = 630
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "EntityStatus"
            Begin Extent = 
               Top = 1290
               Left = 668
               Bottom = 1419
               Right = 838
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AWBCurrencies"
            Begin Extent = 
               Top = 1308
               Left = 876
               Bottom = 1437
               Right = 1049
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Branches"
            Begin Extent = 
               Top = 1326
               Left = 38
               Bottom = 1455
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "ShipmentLevels"
            Begin Extent = 
               Top = 1223
               Left = 1095
               Bottom = 1318
               Right = 1265
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "ShipmentMasterDataEntityStatus"
            Begin Extent = 
               Top = 1320
               Left = 1087
               Bottom = 1449
               Right = 1327
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "MainCarriageAirline"
            Begin Extent = 
               Top = 1386
               Left = 246
               Bottom = 1515
               Right = 419
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Incoterms"
            Begin Extent = 
               Top = 486
               Left = 1102
               Bottom = 615
               Right = 1275
            End
            Dis' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane6', @value=N'playFlags = 280
            TopColumn = 1
         End
         Begin Table = "FHLStatus"
            Begin Extent = 
               Top = 1386
               Left = 457
               Bottom = 1498
               Right = 627
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FWBStatus"
            Begin Extent = 
               Top = 1422
               Left = 665
               Bottom = 1534
               Right = 835
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CarrierLastStatuses"
            Begin Extent = 
               Top = 1440
               Left = 873
               Bottom = 1552
               Right = 1043
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageFromAddresses"
            Begin Extent = 
               Top = 6
               Left = 1132
               Bottom = 135
               Right = 1309
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageToAddresses"
            Begin Extent = 
               Top = 138
               Left = 1093
               Bottom = 267
               Right = 1270
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageToPartners"
            Begin Extent = 
               Top = 1554
               Left = 668
               Bottom = 1683
               Right = 949
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageFromPartners"
            Begin Extent = 
               Top = 1554
               Left = 987
               Bottom = 1683
               Right = 1268
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageFromAddressCountries"
            Begin Extent = 
               Top = 1518
               Left = 249
               Bottom = 1647
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageToAddressCountries"
            Begin Extent = 
               Top = 1590
               Left = 38
               Bottom = 1719
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SpecialServicesTypes"
            Begin Extent = 
               Top = 1632
               Left = 460
               Bottom = 1761
               Right = 630
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Vessels"
            Begin Extent = 
               Top = 1446
               Left = 1365
               Bottom = 1575
               Right = 1538
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 17
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane7', @value=N'2610
         Alias = 3180
         Table = 3675
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=7 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ShipmentDataView'
GO


