SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER function [dbo].[udf_ShipmentSearch]

      (@keywords nvarchar(4000))

returns table

as

  return(
  SELECT        dbo.Shipments.Id, dbo.Shipments.Tenant, dbo.Shipments.ShipmentNumber, dbo.Shipments.ShipperReference1, dbo.Shipments.ARInvoiceIssued, dbo.Shipments.CreditNoteIssued,
                         dbo.ShipmentMasterDatas.Tenant AS ShipmentMasterDataTenant, dbo.ShipmentMasterDatas.Id AS ShipmentMasterDataId, 
                         dbo.ShipmentMasterDatas.MainCarriageFromPortId, dbo.ShipmentMasterDatas.MainCarriageToPortId,
						 dbo.Shipments.ExceptionDescription,dbo.Shipments.ExceptionDate, dbo.Shipments.HasException, dbo.Shipments.ExceptionResolvedDescription,
						 dbo.Shipments.ComputedStatusId, dbo.Shipments.ComputedStatusDate, dbo.Shipments.OperationalCloseDate, dbo.Shipments.AccountingCloseDate,
						 dbo.Shipments.CustomConnectToShipment, dbo.Shipments.ForeignPartnerCountryCode,
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
                         dbo.ShipmentMasterDatas.MainCarriageCarrierNumber, dbo.ShipmentMasterDatas.MainCarriageETD, dbo.ShipmentMasterDatas.MainCarriageETA, 
                         dbo.ShipmentMasterDatas.MainCarriageFromAddressId, dbo.ShipmentMasterDatas.MainCarriageToAddressId,
						 dbo.ShipmentMasterDatas.MainCarriageATA, dbo.ShipmentMasterDatas.MainCarriageATD, dbo.Shipments.ShipperReference2, dbo.Shipments.ToPortId,                          
						 dbo.ShipmentMasterDatas.InterlineId, dbo.ShipmentMasterDatas.ManifestReason, dbo.ShipmentMasterDatas.ManifestStatusCode, dbo.ShipmentMasterDatas.AirlinePrefix,
						 dbo.Shipments.FromPortId, dbo.Shipments.MasterShipmentDataId, dbo.Shipments.ShipmentLevelCode, dbo.Shipments.NextETA, dbo.Shipments.NextETD,
						 dbo.Shipments.NumberOfInsidePackages, dbo.Shipments.NumberOfInsidePackagesDetails,
                         dbo.Shipments.NextLegCode, dbo.Shipments.AccountedReceivablesInProfitCurrency, dbo.Shipments.OpenReceivablesInProfitCurrency, 
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
                         dbo.Shipments.GrossWeightEdited, dbo.Shipments.Ratio, dbo.Shipments.PackagesQuantity, 
						 dbo.Shipments.NumberOfPackages, dbo.Shipments.NumberOfContainers, dbo.Shipments.VolumeInCBM, dbo.Shipments.NumberOfFollowUps,
                         dbo.Shipments.DimensionsUnitCode, dbo.Shipments.AgentReference2, dbo.Shipments.AgentReference1, dbo.Shipments.ShipperNotExporterContactId, 
                         dbo.Shipments.ConsigneeNotImporterContactId, dbo.Shipments.ConsigneeNotImporterAddressId, dbo.Shipments.ShipperNotExporterAddressId, 
                         dbo.Shipments.ConsigneeNotImporterId, dbo.Shipments.ShipperNotExporterId, dbo.Shipments.OtherPrepaidCollectId, dbo.Shipments.FreightPrepaidCollectId, 
                         dbo.Shipments.OrderIsDangerouseGoods, dbo.Shipments.BookingNumberOfPackages, dbo.Shipments.BookingVolume, dbo.Shipments.OrderGrossWeight, 
                         dbo.Shipments.Field10, dbo.Shipments.Field9, dbo.Shipments.Field8, dbo.Shipments.Field7, dbo.Shipments.Field6, dbo.Shipments.Field5, dbo.Shipments.Field4, 
                         dbo.Shipments.Field3, dbo.Shipments.Field2, dbo.Shipments.Field1, dbo.Shipments.GrossWeight, dbo.Shipments.ChargeableWeight, 
                         dbo.Shipments.IsOperationalClosed, dbo.Shipments.ConsigneeContactId, dbo.Shipments.AgentContactId, dbo.Shipments.AgentAddressId, 
                         dbo.Shipments.CustomAgentImportAddressId, dbo.Shipments.CustomAgentImportContactId, dbo.Shipments.ShipperContactId, dbo.Shipments.Notify2ContactId, 
                         dbo.Shipments.Notify1ContactId, dbo.Shipments.Notify2AddressId, dbo.Shipments.Notify1AddressId, dbo.Shipments.PreCarriageETD, 
                         dbo.Shipments.PreCarriageETA, dbo.Shipments.OnCarriageETA, dbo.Shipments.OnCarriageETD, dbo.Shipments.AgentId, dbo.Shipments.OnCarriageCarrierNumber, 
                         dbo.Shipments.OnCarriageATA, dbo.Shipments.OnCarriageATD, dbo.Shipments.OnCarriageToPortId, dbo.Shipments.OnCarriageFromPortId, 
                         dbo.Shipments.OnCarriageTransportModeId, dbo.Shipments.PreCarriageCarrierNumber, dbo.Shipments.PreCarriageATA, dbo.Shipments.PreCarriageATD, 
                         dbo.Shipments.PreCarriageToPortId, dbo.Shipments.PreCarriageFromPortId, dbo.Shipments.PreCarriageTransportModeId, dbo.Shipments.HAWBDate, 
                         dbo.Shipments.DescriptionOfGoods, dbo.Shipments.Notes, dbo.Shipments.DirectionId, dbo.Shipments.TransportModeId, dbo.Shipments.ConsigneeAddressId, 
                         dbo.Shipments.ShipperAddressId, dbo.Shipments.Notify2Id, dbo.Shipments.Notify1Id, dbo.Shipments.ConsigneeId, dbo.Shipments.CustomAgentImportId, 
                         dbo.Shipments.ShipperId, dbo.Shipments.ShipmentTypeId, dbo.Shipments.DepartmentId, dbo.Shipments.CreateDateTime, dbo.Shipments.SalesmanUserId, 
                         dbo.Shipments.IncotermId, dbo.Shipments.BranchId, dbo.Shipments.House, dbo.Shipments.ConsigneeReference2, dbo.Shipments.ConsigneeReference1, 
						 dbo.Shipments.ConsolidatorId,dbo.Shipments.ConsolidatorAddressId,dbo.Shipments.ConsolidatorContactId,dbo.Shipments.ConsolidatorReference,
						 dbo.Shipments.AccountManagerUserId,
						 dbo.Shipments.CustomsDeclarationNumber,
						 dbo.Shipments.ShipperName,
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
                         FreightForwarderCards.Notes AS FreightForwarderNote, ShipperCards.Notes AS ShipperNote, 
                         ConsigneeCards.EnglishName AS ConsigneeName, ConsigneeCards.Notes AS ConsigneeNote, AgentCards.EnglishName AS AgentName, 
                         AgentCards.Notes AS AgentNote, CustomAgentExportCards.EnglishName AS CustomAgentExportName, CustomAgentExportCards.Notes AS CustomAgentExportNote, 
                         CustomAgentImportCards.EnglishName AS CustomAgentImportName, CustomAgentImportCards.Notes AS CustomAgentImportNote, 
                         Notify1Cards.EnglishName AS Notify1Name, Notify1Cards.Notes AS Notify1Note, Notify2Cards.EnglishName AS Notify2Name, Notify2Cards.Notes AS Notify2Note, 
                         ShipperNotExporterCards.EnglishName AS ShipperNotExporterName, ShipperNotExporterCards.Notes AS ShipperNotExporterNote, 
                         ConsigneeNotImporterCards.EnglishName AS ConsigneeNotImporterName, ConsigneeNotImporterCards.Notes AS ConsigneeNotImporterNote, 
                         ToPorts.Code AS ToPortCode, ToPorts.EnglishName AS ToPortName, FromPorts.Code AS FromPortCode, FromPorts.EnglishName AS FromPortName, 
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
                         AWBCurrencies.Code AS AWBCurrencyCode, dbo.Branches.EnglishName AS BranchName, 
                         dbo.ShipmentLevels.Name AS ShipmentLevelName, 
                         		
						 dbo.Shipments.StatusId,											 
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
                         dbo.Shipments.SearchFields, MainCarriageAirline.Prefix AS MainCarriageAirlinePrefix, dbo.Shipments.CreatedByUserId,
                         dbo.Shipments.OpenPayablesInLocalCurrency, dbo.Shipments.AccountedPayablesInLocalCurrency, dbo.Shipments.OpenPayablesInProfitCurrency, 
                         dbo.Shipments.AccountedPayablesInProfitCurrency, dbo.Shipments.ChargeableWeightInKG, dbo.Shipments.GrossWeightInKG, 
                         dbo.Shipments.GrossWeightUnitCode, dbo.Shipments.ChargeableWeightUnitCode, dbo.Shipments.OrderVolumetricWeight, dbo.Shipments.VolumetricWeight, 
                         dbo.Shipments.Volume, dbo.Shipments.IssuingCarrierAgentId, dbo.Incoterms.Code AS IncotermCode,						 
						 
						 dbo.FHLStatus.Code AS FHLStatusCode,
						 dbo.FHLStatus.Name AS FHLStatusName,
						 dbo.Shipments.FHLStatusDate,
						 dbo.FWBStatus.Code AS FWBStatusCode,
						 dbo.FWBStatus.Name AS FWBStatusName,
						 dbo.ShipmentMasterDatas.FWBStatusDate,
						 dbo.Shipments.LastSentByUserId,

						 CargonautFHLStatus.Code AS CargonautFHLStatusCode,
						 CargonautFHLStatus.Name AS CargonautFHLStatusName,
						 dbo.Shipments.CargonautFHLStatusDate,
						 CargonautFWBStatus.Code AS CargonautFWBStatusCode,
						 CargonautFWBStatus.Name AS CargonautFWBStatusName,
						 dbo.ShipmentMasterDatas.CargonautFWBStatusDate,

						 dbo.Shipments.AWBPrint, CarrierLastStatuses.Name AS CarrierLastStatusName, dbo.Shipments.FNAReason, dbo.Shipments.Routing, dbo.ShipmentMasterDatas.TruckNumber, 
                         dbo.Shipments.AsAgreedFreight, dbo.Shipments.AsAgreedOtherCharges, dbo.Shipments.AccountNumber, 
                         dbo.Shipments.CarrierLastStatusCode, dbo.Shipments.CarrierLastStatusDate, dbo.Shipments.LastFSRStatusRequestDate, 
                         dbo.Shipments.FinalArrivalDate, FromPortCountries.Code AS FromPortCountryCode, 
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
						 HybridPartner.Name as PartnerName,
						 dbo.ShipmentMasterDatas.DepartureArrivalFromDate as DepartureArrivalFromDate,
						 dbo.ShipmentMasterDatas.DepartureArrivalToDate as DepartureArrivalToDate,
						 dbo.ShipmentMasterDatas.MainCarriageFinalDestinationETA as MainCarriageFinalDestinationETA,
						 dbo.ShipmentMasterDatas.MainCarriageFinalDestinationATA as MainCarriageFinalDestinationATA
						 

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
                         dbo.NextLegs ON dbo.Shipments.NextLegCode = dbo.NextLegs.Code LEFT OUTER JOIN
                         dbo.Directions ON dbo.Shipments.DirectionId = dbo.Directions.Id LEFT OUTER JOIN
                         dbo.TransportModes ON dbo.Shipments.TransportModeId = dbo.TransportModes.Id LEFT OUTER JOIN
                         dbo.ShipmentTypes ON dbo.Shipments.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
                         dbo.ShipmentReceivableStatus ON dbo.Shipments.ShipmentReceivableStatusCode = dbo.ShipmentReceivableStatus.Code LEFT OUTER JOIN
                         dbo.ShipmentPayableStatus ON dbo.Shipments.ShipmentPayableStatusCode = dbo.ShipmentPayableStatus.Code LEFT OUTER JOIN
                         dbo.EntityStatus ON dbo.Shipments.StatusId = dbo.EntityStatus.Id LEFT OUTER JOIN
                         dbo.Currencies AS AWBCurrencies ON dbo.Shipments.AWBCurrencyId = AWBCurrencies.Id LEFT OUTER JOIN
                         dbo.Branches ON dbo.Shipments.BranchId = dbo.Branches.Id LEFT OUTER JOIN
                         dbo.ShipmentLevels ON dbo.Shipments.ShipmentLevelCode = dbo.ShipmentLevels.Code LEFT OUTER JOIN
                         dbo.EntityStatus AS ShipmentMasterDataEntityStatus ON dbo.ShipmentMasterDatas.StatusId = ShipmentMasterDataEntityStatus.Id LEFT OUTER JOIN
                         dbo.Airlines AS MainCarriageAirline ON dbo.ShipmentMasterDatas.MainCarriageCarrierId = MainCarriageAirline.Id LEFT OUTER JOIN
                         dbo.Incoterms ON dbo.Shipments.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
                         
						 dbo.FHLStatus ON dbo.Shipments.FHLStatusCode = dbo.FHLStatus.Code LEFT OUTER JOIN
                         dbo.FWBStatus ON dbo.ShipmentMasterDatas.FWBStatusCode = dbo.FWBStatus.Code LEFT OUTER JOIN
                         dbo.FHLStatus AS CargonautFHLStatus ON dbo.Shipments.CargonautFHLStatusCode = CargonautFHLStatus.Code LEFT OUTER JOIN
                         dbo.FWBStatus AS CargonautFWBStatus ON dbo.ShipmentMasterDatas.CargonautFWBStatusCode = CargonautFWBStatus.Code LEFT OUTER JOIN
                         
						 dbo.AWBStatus AS CarrierLastStatuses ON dbo.Shipments.CarrierLastStatusCode = CarrierLastStatuses.Code LEFT OUTER JOIN
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
						 
						 dbo.Contacts AS LastSentByUserContact ON dbo.Shipments.LastSentByUserId = LastSentByUserContact.Id LEFT OUTER JOIN
						 dbo.Contacts AS CreatedByUserContact ON dbo.Shipments.CreatedByUserId = CreatedByUserContact.Id INNER JOIN
						 dbo.ShipmentComputedFields AS ShipmentComputedFields ON dbo.Shipments.Id = ShipmentComputedFields.Id where contains(dbo.Shipments.SearchFields,@keywords)
     
  ) 
