using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Infrastructure.Data.Models.AuditLog;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ShipmentMapping
    {
        private static bool isMappingEntityPM;
        public static void MapConcurrencyFields(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, int packagesListCount, 
                                                bool isNewEntity, List<FieldChange> fieldChanges, bool isMappingPM = true)
        {
            if(entityPM.IsHybrid)
            {
                MapAllFields(entityPM, entityPoco, entityMasterData, packagesListCount);
            }

            else
            {
                isMappingEntityPM = isMappingPM;

                if (isNewEntity)
                {
                    MapConcurrencyFields_Champ(entityPM, entityPoco, entityMasterData);
                    MapConcurrencyFields_INTTRA(entityPM, entityPoco, entityMasterData);
                    MapConcurrencyFields_Client(entityPM, entityPoco, entityMasterData);
                }

                else
                {
                    if (entityPM.IsUpdatedByChampAnalyzer)
                    {
                        MapConcurrencyFields_Champ(entityPM, entityPoco, entityMasterData);
                    }

                    else if (entityPM.IsUpdatedByINTTRAAnalyzer)
                    {
                        MapConcurrencyFields_INTTRA(entityPM, entityPoco, entityMasterData);
                    }

                    else if (entityPM.IsUpdatedOceanInsightsAnalyzer)
                    {
                        MapConcurrencyFields_OCINS(entityPM, entityPoco, entityMasterData);
                    }

                    else if (entityPM.IsUpdatedVizionAnalyzer)
                    {
                        MapConcurrencyFields_VZN(entityPM, entityPoco, entityMasterData);
                    }

                    else
                    {
                        if (entityPM.IsDocsKPIsUpdatedFromWR)
                        {
                            MapConcurrencyFields_DocsIn(entityPM, entityPoco, entityMasterData);
                        }

                        MapConcurrencyFields_OnEdited(entityPM, entityPoco, entityMasterData, fieldChanges);
                        MapConcurrencyFields_Client(entityPM, entityPoco, entityMasterData);

                        if (packagesListCount == 0)
                        {
                            entityPoco.HasContainerException = false;
                            entityPoco.INTTRALastStatusDate = null;
                        }
                    }
                }

                MapCalculatedFields(entityPM, entityPoco, entityMasterData);
            }
        }

        private static void MapConcurrencyFields_VZN(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityPoco.OIConcurrencyGUID = entityPM.OINewConcurrencyGUID;
                    entityMasterData.MainCarriageATD = entityPM.MainCarriageATD;
                    entityMasterData.MainCarriageETD = entityPM.MainCarriageETD;
                    entityMasterData.MainCarriageETA = entityPM.MainCarriageETA;
                    entityMasterData.MainCarriageATA = entityPM.MainCarriageATA;
                    entityMasterData.MainCarriageCarrierNumber = entityPM.MainCarriageCarrierNumber;

                    entityMasterData.PreCarriageETD = entityPM.PreCarriageETD;
                    entityMasterData.PreCarriageATD = entityPM.PreCarriageATD;
                    entityMasterData.OnCarriageETA = entityPM.OnCarriageETA;
                    entityMasterData.OnCarriageATA = entityPM.OnCarriageATA;

                    entityMasterData.Transshipment1ETA = entityPM.Transshipment1ETA;
                    entityMasterData.Transshipment1ATA = entityPM.Transshipment1ATA;
                    entityMasterData.Transshipment1ETD = entityPM.Transshipment1ETD;
                    entityMasterData.Transshipment1ATD = entityPM.Transshipment1ATD;

                    entityMasterData.Transshipment2ETA = entityPM.Transshipment2ETA;
                    entityMasterData.Transshipment2ATA = entityPM.Transshipment2ATA;
                    entityMasterData.Transshipment2ETD = entityPM.Transshipment2ETD;
                    entityMasterData.Transshipment2ATD = entityPM.Transshipment2ATD;

                    entityMasterData.Transshipment3ETA = entityPM.Transshipment3ETA;
                    entityMasterData.Transshipment3ATA = entityPM.Transshipment3ATA;
                    entityMasterData.Transshipment3ETD = entityPM.Transshipment3ETD;
                    entityMasterData.Transshipment3ATD = entityPM.Transshipment3ATD;
                }
            }
        }
        private static void MapConcurrencyFields_Champ(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            entityPoco.IsFSRSent = entityPM.IsFSRSent;
            entityPoco.FNAReason = entityPM.FNAReason;
            entityPoco.FHLStatusCode = entityPM.FHLStatusCode;
            entityPoco.FHLStatusDate = entityPM.FHLStatusDate;
            entityPoco.CarrierLastStatusCode = entityPM.CarrierLastStatusCode;
            entityPoco.CarrierLastStatusDate = entityPM.CarrierLastStatusDate;

            entityPoco.NumberOfPackages = entityPM.NumberOfPackages;
            entityPoco.GrossWeight = entityPM.GrossWeight;
            entityPoco.ChargeableWeight = entityPM.ChargeableWeight;
            entityPoco.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;            

            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityMasterData.FWBStatusCode = entityPM.FWBStatusCode;
                    entityMasterData.FWBStatusDate = entityPM.FWBStatusDate;
                    entityMasterData.MainCarriageFromPortId = entityPM.MainCarriageFromPortId;
                    entityMasterData.MainCarriageToPortId = entityPM.MainCarriageToPortId;
                    entityMasterData.Transshipment1ToPortId = entityPM.Transshipment1ToPortId;
                    entityMasterData.Transshipment2ToPortId = entityPM.Transshipment2ToPortId;
                    entityMasterData.Transshipment3ToPortId = entityPM.Transshipment3ToPortId;
                    entityMasterData.MainCarriageFinalDestinationPortId = entityPM.MainCarriageFinalDestinationPortId;

                    entityMasterData.MainCarriageATD = entityPM.MainCarriageATD;
                    entityMasterData.MainCarriageETD = entityPM.MainCarriageETD;
                    entityMasterData.MainCarriageETA = entityPM.MainCarriageETA;
                    entityMasterData.MainCarriageATA = entityPM.MainCarriageATA;
                    entityMasterData.MainCarriageSTD = entityPM.MainCarriageSTD;
                    entityMasterData.MainCarriageSTA = entityPM.MainCarriageSTA;
                    entityMasterData.Transshipment1ATA = entityPM.Transshipment1ATA;
                    entityMasterData.Transshipment1ATD = entityPM.Transshipment1ATD;
                    entityMasterData.Transshipment1ETA = entityPM.Transshipment1ETA;
                    entityMasterData.Transshipment1ETD = entityPM.Transshipment1ETD;
                    entityMasterData.Transshipment1STD = entityPM.Transshipment1STD;
                    entityMasterData.Transshipment1STA = entityPM.Transshipment1STA;
                    entityMasterData.Transshipment2ATA = entityPM.Transshipment2ATA;
                    entityMasterData.Transshipment2ATD = entityPM.Transshipment2ATD;
                    entityMasterData.Transshipment2ETA = entityPM.Transshipment2ETA;
                    entityMasterData.Transshipment2ETD = entityPM.Transshipment2ETD;
                    entityMasterData.Transshipment2STD = entityPM.Transshipment2STD;
                    entityMasterData.Transshipment2STA = entityPM.Transshipment2STA;
                    entityMasterData.Transshipment3ATA = entityPM.Transshipment3ATA;
                    entityMasterData.Transshipment3ATD = entityPM.Transshipment3ATD;
                    entityMasterData.Transshipment3ETA = entityPM.Transshipment3ETA;
                    entityMasterData.Transshipment3ETD = entityPM.Transshipment3ETD;
                    entityMasterData.Transshipment3STD = entityPM.Transshipment3STD;
                    entityMasterData.Transshipment3STA = entityPM.Transshipment3STA;

                    entityMasterData.PreCarriageATA = entityPM.PreCarriageATA;
                    entityMasterData.PreCarriageATD = entityPM.PreCarriageATD;
                    entityMasterData.PreCarriageETA = entityPM.PreCarriageETA;
                    entityMasterData.PreCarriageETD = entityPM.PreCarriageETD;
                    entityMasterData.OnCarriageATA = entityPM.OnCarriageATA;
                    entityMasterData.OnCarriageATD = entityPM.OnCarriageATD;
                    entityMasterData.OnCarriageETA = entityPM.OnCarriageETA;
                    entityMasterData.OnCarriageETD = entityPM.OnCarriageETD;

                    FillEstimatedDatesFields(entityMasterData, entityPM);
                }
            }

            else
            {
                entityPoco.PreForwardingATA = entityPM.PreForwardingATA;
                entityPoco.PreForwardingATD = entityPM.PreForwardingATD;
                entityPoco.PreForwardingETA = entityPM.PreForwardingETA;
                entityPoco.PreForwardingETD = entityPM.PreForwardingETD;
                entityPoco.OnForwardingATA = entityPM.OnForwardingATA;
                entityPoco.OnForwardingATD = entityPM.OnForwardingATD;
                entityPoco.OnForwardingETA = entityPM.OnForwardingETA;
                entityPoco.OnForwardingETD = entityPM.OnForwardingETD;
            }
        }
        private static void MapConcurrencyFields_INTTRA(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            entityPoco.HasContainerException = entityPM.HasContainerException;
            entityPoco.INTTRALastStatusDate = entityPM.INTTRALastStatusDate;
            entityPoco.INTTRASIStatusCode = entityPM.INTTRASIStatusCode;
            entityPoco.INTTRASIStatusDate = entityPM.INTTRASIStatusDate;
            entityPoco.INTTRALastBookingResponse = entityPM.INTTRALastBookingResponse;
            entityPoco.INTTRABookingStatusCode = entityPM.INTTRABookingStatusCode;
            entityPoco.INTTRABookingTransStatusCode = entityPM.INTTRABookingTransStatusCode;
            entityPoco.INTTRALastEBbookingSendDate = entityPM.INTTRALastEBbookingSendDate;

            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityMasterData.MainCarriageATD = entityPM.MainCarriageATD;
                    entityMasterData.MainCarriageETD = entityPM.MainCarriageETD;
                    entityMasterData.MainCarriageETA = entityPM.MainCarriageETA;
                    entityMasterData.MainCarriageATA = entityPM.MainCarriageATA;
                    entityMasterData.Transshipment1ATA = entityPM.Transshipment1ATA;
                    entityMasterData.Transshipment1ATD = entityPM.Transshipment1ATD;
                    entityMasterData.Transshipment1ETA = entityPM.Transshipment1ETA;
                    entityMasterData.Transshipment1ETD = entityPM.Transshipment1ETD;
                    entityMasterData.Transshipment2ETD = entityPM.Transshipment2ETD;
                    entityMasterData.Transshipment2ETA = entityPM.Transshipment2ETA;
                    entityMasterData.Transshipment3ETD = entityPM.Transshipment3ETD;
                    entityMasterData.Transshipment3ETA = entityPM.Transshipment3ETA;
                    entityMasterData.BookingConfirmedBy = entityPM.BookingConfirmedBy;
                    entityMasterData.BookingConfirmationNumber = entityPM.BookingConfirmationNumber;
                    entityMasterData.MainCarriageCarrierNumber = entityPM.MainCarriageCarrierNumber;
                }
            }
        }
        private static void MapConcurrencyFields_OCINS(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityPoco.OIConcurrencyGUID = entityPM.OINewConcurrencyGUID;
                    entityMasterData.MainCarriageATD = entityPM.MainCarriageATD;
                    entityMasterData.MainCarriageETD = entityPM.MainCarriageETD;
                    entityMasterData.MainCarriageETA = entityPM.MainCarriageETA;
                    entityMasterData.MainCarriageATA = entityPM.MainCarriageATA;
                    entityMasterData.PreCarriageETD = entityPM.PreCarriageETD;
                    entityMasterData.PreCarriageATD = entityPM.PreCarriageATD;
                    entityMasterData.OnCarriageETA = entityPM.OnCarriageETA;
                    entityMasterData.OnCarriageATA = entityPM.OnCarriageATA;
                }
            }
        }
        private static void MapConcurrencyFields_Client(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            if (!entityPM.IsDocsKPIsUpdatedFromWR)
            {
                if (isMappingEntityPM)
                {
                    entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
                }

                entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            }

            entityPoco.LastSentByUserId = entityPM.LastSentByUserId;
            entityPoco.LastFSRStatusRequestDate = entityPM.LastFSRStatusRequestDate;
            entityPoco.CargonautFHLStatusCode = entityPM.CargonautFHLStatusCode;
            entityPoco.CargonautFHLStatusDate = entityPM.CargonautFHLStatusDate;
            entityPoco.INTTRASIError = entityPM.INTTRASIError;
            entityPoco.INTTRAContractNumber = entityPM.INTTRAContractNumber;
            entityPoco.INTTRAInstructions = entityPM.INTTRAInstructions;
            entityPoco.INTTRAComments = entityPM.INTTRAComments;
            entityPoco.INTTRADocumentQTY = entityPM.INTTRADocumentQTY;
            entityPoco.SIHasAttachList = entityPM.SIHasAttachList;
            entityPoco.INTTRAIsFreighted = entityPM.INTTRAIsFreighted;
            entityPoco.INTTRADocumentTypeCode = entityPM.INTTRADocumentTypeCode;
            entityPoco.INTTRABookingError = entityPM.INTTRABookingError;

            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityMasterData.CargonautFWBStatusCode = entityPM.CargonautFWBStatusCode;
                    entityMasterData.CargonautFWBStatusDate = entityPM.CargonautFWBStatusDate;

                    CalculateFinalDestinationPort(entityPM, entityMasterData);
                }
            }
        }

        private static void MapConcurrencyFields_OnEdited(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, List<FieldChange> fieldChanges)
        {
            var NumberOfPackages = GetConcurrencyFieldValue_Int(entityPM.NumberOfPackages_Original, entityPM.NumberOfPackages, entityPoco.NumberOfPackages);
            FieldChange.Add(entityPoco.NumberOfPackages, NumberOfPackages, nameof(entityPoco.NumberOfPackages), fieldChanges);
            entityPoco.NumberOfPackages = NumberOfPackages;

            var GrossWeight = GetConcurrencyFieldValue_Double(entityPM.GrossWeight_Original, entityPM.GrossWeight, entityPoco.GrossWeight);
            FieldChange.Add(entityPoco.GrossWeight, GrossWeight, nameof(entityPoco.GrossWeight), fieldChanges);
            entityPoco.GrossWeight = GrossWeight;

            var ChargeableWeight = GetConcurrencyFieldValue_Double(entityPM.ChargeableWeight_Original, entityPM.ChargeableWeight, entityPoco.ChargeableWeight);
            FieldChange.Add(entityPoco.ChargeableWeight, ChargeableWeight, nameof(entityPoco.ChargeableWeight), fieldChanges);
            entityPoco.ChargeableWeight = ChargeableWeight;

            var GrossWeightUnitCode = GetConcurrencyFieldValue_String(entityPM.GrossWeightUnitCode_Original, entityPM.GrossWeightUnitCode, entityPoco.GrossWeightUnitCode);
            FieldChange.Add(entityPoco.GrossWeightUnitCode, GrossWeightUnitCode, nameof(entityPoco.GrossWeightUnitCode), fieldChanges);
            entityPoco.GrossWeightUnitCode = GrossWeightUnitCode;

            var INTTRABookingStatusCode = GetConcurrencyFieldValue_String(entityPM.INTTRABookingStatusCode_Original, entityPM.INTTRABookingStatusCode, entityPoco.INTTRABookingStatusCode);
            FieldChange.Add(entityPoco.INTTRABookingStatusCode, INTTRABookingStatusCode, nameof(entityPoco.INTTRABookingStatusCode), fieldChanges);
            entityPoco.INTTRABookingStatusCode = INTTRABookingStatusCode;

            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    var MainCarriageFromPortId = GetConcurrencyFieldValue_String(entityPM.MAN_FromPortId_Original, entityPM.MainCarriageFromPortId, entityMasterData.MainCarriageFromPortId);
                    FieldChange.Add(entityMasterData.MainCarriageFromPortId, MainCarriageFromPortId, nameof(entityMasterData.MainCarriageFromPortId), fieldChanges);
                    entityMasterData.MainCarriageFromPortId = MainCarriageFromPortId;

                    var MainCarriageToPortId = GetConcurrencyFieldValue_String(entityPM.MainCarriageToPortId_Original, entityPM.MainCarriageToPortId, entityMasterData.MainCarriageToPortId);
                    FieldChange.Add(entityMasterData.MainCarriageToPortId, MainCarriageToPortId, nameof(entityMasterData.MainCarriageToPortId), fieldChanges);
                    entityMasterData.MainCarriageToPortId = MainCarriageToPortId;

                    var Transshipment1ToPortId = GetConcurrencyFieldValue_String(entityPM.TR1_ToPortId_Original, entityPM.Transshipment1ToPortId, entityMasterData.Transshipment1ToPortId);
                    FieldChange.Add(entityMasterData.Transshipment1ToPortId, Transshipment1ToPortId, nameof(entityMasterData.Transshipment1ToPortId), fieldChanges);
                    entityMasterData.Transshipment1ToPortId = Transshipment1ToPortId;

                    var Transshipment2ToPortId = GetConcurrencyFieldValue_String(entityPM.TR2_ToPortId_Original, entityPM.Transshipment2ToPortId, entityMasterData.Transshipment2ToPortId);
                    FieldChange.Add(entityMasterData.Transshipment2ToPortId, Transshipment2ToPortId, nameof(entityMasterData.Transshipment2ToPortId), fieldChanges);
                    entityMasterData.Transshipment2ToPortId = Transshipment2ToPortId;

                    var Transshipment3ToPortId = GetConcurrencyFieldValue_String(entityPM.TR3_ToPortId_Original, entityPM.Transshipment3ToPortId, entityMasterData.Transshipment3ToPortId);
                    FieldChange.Add(entityMasterData.Transshipment3ToPortId, Transshipment3ToPortId, nameof(entityMasterData.Transshipment3ToPortId), fieldChanges);
                    entityMasterData.Transshipment3ToPortId = Transshipment3ToPortId;

                    var MainCarriageFinalDestinationPortId = GetConcurrencyFieldValue_String(entityPM.FIN_PortId_Original, entityPM.MainCarriageFinalDestinationPortId, entityMasterData.MainCarriageFinalDestinationPortId);
                    FieldChange.Add(entityMasterData.MainCarriageFinalDestinationPortId, MainCarriageFinalDestinationPortId, nameof(entityMasterData.MainCarriageFinalDestinationPortId), fieldChanges);
                    entityMasterData.MainCarriageFinalDestinationPortId = MainCarriageFinalDestinationPortId;

                    var MainCarriageATD = GetConcurrencyFieldValue_Date(entityPM.MainCarriageATD_Original, entityPM.MainCarriageATD, entityMasterData.MainCarriageATD);
                    FieldChange.Add(entityMasterData.MainCarriageATD, MainCarriageATD, nameof(entityMasterData.MainCarriageATD), fieldChanges);
                    entityMasterData.MainCarriageATD = MainCarriageATD;

                    var MainCarriageETD = GetConcurrencyFieldValue_Date(entityPM.MainCarriageETD_Original, entityPM.MainCarriageETD, entityMasterData.MainCarriageETD);
                    FieldChange.Add(entityMasterData.MainCarriageETD, MainCarriageETD, nameof(entityMasterData.MainCarriageETD), fieldChanges);
                    entityMasterData.MainCarriageETD = MainCarriageETD;

                    var MainCarriageETA = GetConcurrencyFieldValue_Date(entityPM.MainCarriageETA_Original, entityPM.MainCarriageETA, entityMasterData.MainCarriageETA);
                    FieldChange.Add(entityMasterData.MainCarriageETA, MainCarriageETA, nameof(entityMasterData.MainCarriageETA), fieldChanges);
                    entityMasterData.MainCarriageETA = MainCarriageETA;

                    var MainCarriageATA = GetConcurrencyFieldValue_Date(entityPM.MainCarriageATA_Original, entityPM.MainCarriageATA, entityMasterData.MainCarriageATA);
                    FieldChange.Add(entityMasterData.MainCarriageATA, MainCarriageATA, nameof(entityMasterData.MainCarriageATA), fieldChanges);
                    entityMasterData.MainCarriageATA = MainCarriageATA;

                    var MainCarriageSTD = GetConcurrencyFieldValue_Date(entityPM.MainCarriageSTD_Original, entityPM.MainCarriageSTD, entityMasterData.MainCarriageSTD);
                    FieldChange.Add(entityMasterData.MainCarriageSTD, MainCarriageSTD, nameof(entityMasterData.MainCarriageSTD), fieldChanges);
                    entityMasterData.MainCarriageSTD = MainCarriageSTD;

                    var MainCarriageSTA = GetConcurrencyFieldValue_Date(entityPM.MainCarriageSTA_Original, entityPM.MainCarriageSTA, entityMasterData.MainCarriageSTA);
                    FieldChange.Add(entityMasterData.MainCarriageSTA, MainCarriageSTA, nameof(entityMasterData.MainCarriageSTA), fieldChanges);
                    entityMasterData.MainCarriageSTA = MainCarriageSTA;

                    var Transshipment1ATA = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ATA_Original, entityPM.Transshipment1ATA, entityMasterData.Transshipment1ATA);
                    FieldChange.Add(entityMasterData.Transshipment1ATA, Transshipment1ATA, nameof(entityMasterData.Transshipment1ATA), fieldChanges);
                    entityMasterData.Transshipment1ATA = Transshipment1ATA;

                    var Transshipment1ATD = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ATD_Original, entityPM.Transshipment1ATD, entityMasterData.Transshipment1ATD);
                    FieldChange.Add(entityMasterData.Transshipment1ATD, Transshipment1ATD, nameof(entityMasterData.Transshipment1ATD), fieldChanges);
                    entityMasterData.Transshipment1ATD = Transshipment1ATD;

                    var Transshipment1ETA = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ETA_Original, entityPM.Transshipment1ETA, entityMasterData.Transshipment1ETA);
                    FieldChange.Add(entityMasterData.Transshipment1ETA, Transshipment1ETA, nameof(entityMasterData.Transshipment1ETA), fieldChanges);
                    entityMasterData.Transshipment1ETA = Transshipment1ETA;

                    var Transshipment1ETD = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ETD_Original, entityPM.Transshipment1ETD, entityMasterData.Transshipment1ETD);
                    FieldChange.Add(entityMasterData.Transshipment1ETD, Transshipment1ETD, nameof(entityMasterData.Transshipment1ETD), fieldChanges);
                    entityMasterData.Transshipment1ETD = Transshipment1ETD;

                    var Transshipment1STD = GetConcurrencyFieldValue_Date(entityPM.Transshipment1STD_Original, entityPM.Transshipment1STD, entityMasterData.Transshipment1STD);
                    FieldChange.Add(entityMasterData.Transshipment1STD, Transshipment1STD, nameof(entityMasterData.Transshipment1STD), fieldChanges);
                    entityMasterData.Transshipment1STD = Transshipment1STD;

                    var Transshipment1STA = GetConcurrencyFieldValue_Date(entityPM.Transshipment1STA_Original, entityPM.Transshipment1STA, entityMasterData.Transshipment1STA);
                    FieldChange.Add(entityMasterData.Transshipment1STA, Transshipment1STA, nameof(entityMasterData.Transshipment1STA), fieldChanges);
                    entityMasterData.Transshipment1STA = Transshipment1STA;

                    var Transshipment2ATA = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ATA_Original, entityPM.Transshipment2ATA, entityMasterData.Transshipment2ATA);
                    FieldChange.Add(entityMasterData.Transshipment2ATA, Transshipment2ATA, nameof(entityMasterData.Transshipment2ATA), fieldChanges);
                    entityMasterData.Transshipment2ATA = Transshipment2ATA;

                    var Transshipment2ATD = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ATD_Original, entityPM.Transshipment2ATD, entityMasterData.Transshipment2ATD);
                    FieldChange.Add(entityMasterData.Transshipment2ATD, Transshipment2ATD, nameof(entityMasterData.Transshipment2ATD), fieldChanges);
                    entityMasterData.Transshipment2ATD = Transshipment2ATD;

                    var Transshipment2ETA = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ETA_Original, entityPM.Transshipment2ETA, entityMasterData.Transshipment2ETA);
                    FieldChange.Add(entityMasterData.Transshipment2ETA, Transshipment2ETA, nameof(entityMasterData.Transshipment2ETA), fieldChanges);
                    entityMasterData.Transshipment2ETA = Transshipment2ETA;

                    var Transshipment2ETD = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ETD_Original, entityPM.Transshipment2ETD, entityMasterData.Transshipment2ETD);
                    FieldChange.Add(entityMasterData.Transshipment2ETD, Transshipment2ETD, nameof(entityMasterData.Transshipment2ETD), fieldChanges);
                    entityMasterData.Transshipment2ETD = Transshipment2ETD;

                    var Transshipment2STD = GetConcurrencyFieldValue_Date(entityPM.Transshipment2STD_Original, entityPM.Transshipment2STD, entityMasterData.Transshipment2STD);
                    FieldChange.Add(entityMasterData.Transshipment2STD, Transshipment2STD, nameof(entityMasterData.Transshipment2STD), fieldChanges);
                    entityMasterData.Transshipment2STD = Transshipment2STD;

                    var Transshipment2STA = GetConcurrencyFieldValue_Date(entityPM.Transshipment2STA_Original, entityPM.Transshipment2STA, entityMasterData.Transshipment2STA);
                    FieldChange.Add(entityMasterData.Transshipment2STA, Transshipment2STA, nameof(entityMasterData.Transshipment2STA), fieldChanges);
                    entityMasterData.Transshipment2STA = Transshipment2STA;

                    var Transshipment3ATA = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ATA_Original, entityPM.Transshipment3ATA, entityMasterData.Transshipment3ATA);
                    FieldChange.Add(entityMasterData.Transshipment3ATA, Transshipment3ATA, nameof(entityMasterData.Transshipment3ATA), fieldChanges);
                    entityMasterData.Transshipment3ATA = Transshipment3ATA;

                    var Transshipment3ATD = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ATD_Original, entityPM.Transshipment3ATD, entityMasterData.Transshipment3ATD);
                    FieldChange.Add(entityMasterData.Transshipment3ATD, Transshipment3ATD, nameof(entityMasterData.Transshipment3ATD), fieldChanges);
                    entityMasterData.Transshipment3ATD = Transshipment3ATD;

                    var Transshipment3ETA = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ETA_Original, entityPM.Transshipment3ETA, entityMasterData.Transshipment3ETA);
                    FieldChange.Add(entityMasterData.Transshipment3ETA, Transshipment3ETA, nameof(entityMasterData.Transshipment3ETA), fieldChanges);
                    entityMasterData.Transshipment3ETA = Transshipment3ETA;

                    var Transshipment3ETD = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ETD_Original, entityPM.Transshipment3ETD, entityMasterData.Transshipment3ETD);
                    FieldChange.Add(entityMasterData.Transshipment3ETD, Transshipment3ETD, nameof(entityMasterData.Transshipment3ETD), fieldChanges);
                    entityMasterData.Transshipment3ETD = Transshipment3ETD;

                    var Transshipment3STD = GetConcurrencyFieldValue_Date(entityPM.Transshipment3STD_Original, entityPM.Transshipment3STD, entityMasterData.Transshipment3STD);
                    FieldChange.Add(entityMasterData.Transshipment3STD, Transshipment3STD, nameof(entityMasterData.Transshipment3STD), fieldChanges);
                    entityMasterData.Transshipment3STD = Transshipment3STD;

                    var Transshipment3STA = GetConcurrencyFieldValue_Date(entityPM.Transshipment3STA_Original, entityPM.Transshipment3STA, entityMasterData.Transshipment3STA);
                    FieldChange.Add(entityMasterData.Transshipment3STA, Transshipment3STA, nameof(entityMasterData.Transshipment3STA), fieldChanges);
                    entityMasterData.Transshipment3STA = Transshipment3STA;

                    var BookingConfirmedBy = GetConcurrencyFieldValue_String(entityPM.BookingConfirmedBy_Original, entityPM.BookingConfirmedBy, entityMasterData.BookingConfirmedBy);
                    FieldChange.Add(entityMasterData.BookingConfirmedBy, BookingConfirmedBy, nameof(entityMasterData.BookingConfirmedBy), fieldChanges);
                    entityMasterData.BookingConfirmedBy = BookingConfirmedBy;

                    var BookingConfirmationNumber = GetConcurrencyFieldValue_String(entityPM.BookingConfNumber_Original, entityPM.BookingConfirmationNumber, entityMasterData.BookingConfirmationNumber);
                    FieldChange.Add(entityMasterData.BookingConfirmationNumber, BookingConfirmationNumber, nameof(entityMasterData.BookingConfirmationNumber), fieldChanges);
                    entityMasterData.BookingConfirmationNumber = BookingConfirmationNumber;

                    var MainCarriageCarrierNumber = GetConcurrencyFieldValue_String(entityPM.MAN_CarrierNumber_Original, entityPM.MainCarriageCarrierNumber, entityMasterData.MainCarriageCarrierNumber);
                    FieldChange.Add(entityMasterData.MainCarriageCarrierNumber, MainCarriageCarrierNumber, nameof(entityMasterData.MainCarriageCarrierNumber), fieldChanges);
                    entityMasterData.MainCarriageCarrierNumber = MainCarriageCarrierNumber;

                    var PreCarriageATA = GetConcurrencyFieldValue_Date(entityPM.PreCarriageATA_Original, entityPM.PreCarriageATA, entityMasterData.PreCarriageATA);
                    FieldChange.Add(entityMasterData.PreCarriageATA, PreCarriageATA, nameof(entityMasterData.PreCarriageATA), fieldChanges);
                    entityMasterData.PreCarriageATA = PreCarriageATA;

                    var PreCarriageATD = GetConcurrencyFieldValue_Date(entityPM.PreCarriageATD_Original, entityPM.PreCarriageATD, entityMasterData.PreCarriageATD);
                    FieldChange.Add(entityMasterData.PreCarriageATD, PreCarriageATD, nameof(entityMasterData.PreCarriageATD), fieldChanges);
                    entityMasterData.PreCarriageATD = PreCarriageATD;

                    var PreCarriageETA = GetConcurrencyFieldValue_Date(entityPM.PreCarriageETA_Original, entityPM.PreCarriageETA, entityMasterData.PreCarriageETA);
                    FieldChange.Add(entityMasterData.PreCarriageETA, PreCarriageETA, nameof(entityMasterData.PreCarriageETA), fieldChanges);
                    entityMasterData.PreCarriageETA = PreCarriageETA;

                    var PreCarriageETD = GetConcurrencyFieldValue_Date(entityPM.PreCarriageETD_Original, entityPM.PreCarriageETD, entityMasterData.PreCarriageETD);
                    FieldChange.Add(entityMasterData.PreCarriageETD, PreCarriageETD, nameof(entityMasterData.PreCarriageETD), fieldChanges);
                    entityMasterData.PreCarriageETD = PreCarriageETD;

                    var OnCarriageATA = GetConcurrencyFieldValue_Date(entityPM.OnCarriageATA_Original, entityPM.OnCarriageATA, entityMasterData.OnCarriageATA);
                    FieldChange.Add(entityMasterData.OnCarriageATA, OnCarriageATA, nameof(entityMasterData.OnCarriageATA), fieldChanges);
                    entityMasterData.OnCarriageATA = OnCarriageATA;

                    var OnCarriageATD = GetConcurrencyFieldValue_Date(entityPM.OnCarriageATD_Original, entityPM.OnCarriageATD, entityMasterData.OnCarriageATD);
                    FieldChange.Add(entityMasterData.OnCarriageATD, OnCarriageATD, nameof(entityMasterData.OnCarriageATD), fieldChanges);
                    entityMasterData.OnCarriageATD = OnCarriageATD;

                    var OnCarriageETA = GetConcurrencyFieldValue_Date(entityPM.OnCarriageETA_Original, entityPM.OnCarriageETA, entityMasterData.OnCarriageETA);
                    FieldChange.Add(entityMasterData.OnCarriageETA, OnCarriageETA, nameof(entityMasterData.OnCarriageETA), fieldChanges);
                    entityMasterData.OnCarriageETA = OnCarriageETA;

                    var OnCarriageETD = GetConcurrencyFieldValue_Date(entityPM.OnCarriageETD_Original, entityPM.OnCarriageETD, entityMasterData.OnCarriageETD);
                    FieldChange.Add(entityMasterData.OnCarriageETD, OnCarriageETD, nameof(entityMasterData.OnCarriageETD), fieldChanges);
                    entityMasterData.OnCarriageETD = OnCarriageETD;
                }
            }
            else
            {
                var PreForwardingATA = GetConcurrencyFieldValue_Date(entityPM.PreForwardingATA_Original, entityPM.PreForwardingATA, entityPoco.PreForwardingATA);
                FieldChange.Add(entityPoco.PreForwardingATA, PreForwardingATA, nameof(entityPoco.PreForwardingATA), fieldChanges);
                entityPoco.PreForwardingATA = PreForwardingATA;

                var PreForwardingATD = GetConcurrencyFieldValue_Date(entityPM.PreForwardingATD_Original, entityPM.PreForwardingATD, entityPoco.PreForwardingATD);
                FieldChange.Add(entityPoco.PreForwardingATD, PreForwardingATD, nameof(entityPoco.PreForwardingATD), fieldChanges);
                entityPoco.PreForwardingATD = PreForwardingATD;

                var PreForwardingETA = GetConcurrencyFieldValue_Date(entityPM.PreForwardingETA_Original, entityPM.PreForwardingETA, entityPoco.PreForwardingETA);
                FieldChange.Add(entityPoco.PreForwardingETA, PreForwardingETA, nameof(entityPoco.PreForwardingETA), fieldChanges);
                entityPoco.PreForwardingETA = PreForwardingETA;

                var PreForwardingETD = GetConcurrencyFieldValue_Date(entityPM.PreForwardingETD_Original, entityPM.PreForwardingETD, entityPoco.PreForwardingETD);
                FieldChange.Add(entityPoco.PreForwardingETD, PreForwardingETD, nameof(entityPoco.PreForwardingETD), fieldChanges);
                entityPoco.PreForwardingETD = PreForwardingETD;

                var OnForwardingATA = GetConcurrencyFieldValue_Date(entityPM.OnForwardingATA_Original, entityPM.OnForwardingATA, entityPoco.OnForwardingATA);
                FieldChange.Add(entityPoco.OnForwardingATA, OnForwardingATA, nameof(entityPoco.OnForwardingATA), fieldChanges);
                entityPoco.OnForwardingATA = OnForwardingATA;

                var OnForwardingATD = GetConcurrencyFieldValue_Date(entityPM.OnForwardingATD_Original, entityPM.OnForwardingATD, entityPoco.OnForwardingATD);
                FieldChange.Add(entityPoco.OnForwardingATD, OnForwardingATD, nameof(entityPoco.OnForwardingATD), fieldChanges);
                entityPoco.OnForwardingATD = OnForwardingATD;

                var OnForwardingETA = GetConcurrencyFieldValue_Date(entityPM.OnForwardingETA_Original, entityPM.OnForwardingETA, entityPoco.OnForwardingETA);
                FieldChange.Add(entityPoco.OnForwardingETA, OnForwardingETA, nameof(entityPoco.OnForwardingETA), fieldChanges);
                entityPoco.OnForwardingETA = OnForwardingETA;

                var OnForwardingETD = GetConcurrencyFieldValue_Date(entityPM.OnForwardingETD_Original, entityPM.OnForwardingETD, entityPoco.OnForwardingETD);
                FieldChange.Add(entityPoco.OnForwardingETD, OnForwardingETD, nameof(entityPoco.OnForwardingETD), fieldChanges);
                entityPoco.OnForwardingETD = OnForwardingETD;
            }
        }
        private static void MapConcurrencyFields_DocsIn(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            entityPoco.IsPODReceived = entityPM.IsPODReceived;
            entityPoco.PODReceivedDate = entityPM.PODReceivedDate;
        }
        private static void MapCalculatedFields(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            if (isMappingEntityPM)
            {
                entityPoco.GrossWeightInKG = entityPM.GrossWeightInKG = GetWeightInKG(entityPM.GrossWeightUnitCode, entityPM.GrossWeight);
                entityPoco.GrossWeightPerTon = entityPM.GrossWeightPerTon = GetWeightInTon(entityPM.GrossWeightInKG);
                entityPoco.ChargeableWeightInKG = entityPM.ChargeableWeightInKG = GetWeightInKG(entityPM.ChargeableWeightUnitCode, entityPM.ChargeableWeight);
            }
        }
        private static void MapAllFields(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, int packagesListCount)
        {
            entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            entityPoco.IsFSRSent = entityPM.IsFSRSent;
            entityPoco.FNAReason = entityPM.FNAReason;
            entityPoco.FHLStatusCode = entityPM.FHLStatusCode;
            entityPoco.FHLStatusDate = entityPM.FHLStatusDate;
            entityPoco.CarrierLastStatusCode = entityPM.CarrierLastStatusCode;
            entityPoco.CarrierLastStatusDate = entityPM.CarrierLastStatusDate;
            entityPoco.NumberOfPackages = entityPM.NumberOfPackages;
            entityPoco.GrossWeight = entityPM.GrossWeight;
            entityPoco.ChargeableWeight = entityPM.ChargeableWeight;
            entityPoco.ChargeableWeightInKG = entityPM.ChargeableWeightInKG;
            entityPoco.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;            
            entityPoco.HasContainerException = entityPM.HasContainerException;
            entityPoco.INTTRALastStatusDate = entityPM.INTTRALastStatusDate;
            entityPoco.INTTRASIStatusCode = entityPM.INTTRASIStatusCode;
            entityPoco.INTTRASIStatusDate = entityPM.INTTRASIStatusDate;
            entityPoco.INTTRALastBookingResponse = entityPM.INTTRALastBookingResponse;
            entityPoco.INTTRABookingStatusCode = entityPM.INTTRABookingStatusCode;
            entityPoco.INTTRABookingTransStatusCode = entityPM.INTTRABookingTransStatusCode;
            entityPoco.LastSentByUserId = entityPM.LastSentByUserId;
            entityPoco.LastFSRStatusRequestDate = entityPM.LastFSRStatusRequestDate;
            entityPoco.CargonautFHLStatusCode = entityPM.CargonautFHLStatusCode;
            entityPoco.CargonautFHLStatusDate = entityPM.CargonautFHLStatusDate;
            entityPoco.INTTRASIError = entityPM.INTTRASIError;
            entityPoco.INTTRAContractNumber = entityPM.INTTRAContractNumber;
            entityPoco.INTTRAInstructions = entityPM.INTTRAInstructions;
            entityPoco.INTTRAComments = entityPM.INTTRAComments;
            entityPoco.INTTRADocumentQTY = entityPM.INTTRADocumentQTY;
            entityPoco.SIHasAttachList = entityPM.SIHasAttachList;
            entityPoco.INTTRAIsFreighted = entityPM.INTTRAIsFreighted;
            entityPoco.INTTRADocumentTypeCode = entityPM.INTTRADocumentTypeCode;
            entityPoco.INTTRABookingError = entityPM.INTTRABookingError;

            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityMasterData.FWBStatusCode = entityPM.FWBStatusCode;
                    entityMasterData.FWBStatusDate = entityPM.FWBStatusDate;
                    entityMasterData.MainCarriageFromPortId = entityPM.MainCarriageFromPortId;
                    entityMasterData.MainCarriageToPortId = entityPM.MainCarriageToPortId;
                    entityMasterData.Transshipment1ToPortId = entityPM.Transshipment1ToPortId;
                    entityMasterData.Transshipment2ToPortId = entityPM.Transshipment2ToPortId;
                    entityMasterData.Transshipment3ToPortId = entityPM.Transshipment3ToPortId;
                    entityMasterData.MainCarriageFinalDestinationPortId = entityPM.MainCarriageFinalDestinationPortId;
                    entityMasterData.MainCarriageATD = entityPM.MainCarriageATD;
                    entityMasterData.MainCarriageETD = entityPM.MainCarriageETD;
                    entityMasterData.MainCarriageETA = entityPM.MainCarriageETA;
                    entityMasterData.MainCarriageATA = entityPM.MainCarriageATA;
                    entityMasterData.MainCarriageSTD = entityPM.MainCarriageSTD;
                    entityMasterData.MainCarriageSTA = entityPM.MainCarriageSTA;
                    entityMasterData.Transshipment1ATA = entityPM.Transshipment1ATA;
                    entityMasterData.Transshipment1ATD = entityPM.Transshipment1ATD;
                    entityMasterData.Transshipment1ETA = entityPM.Transshipment1ETA;
                    entityMasterData.Transshipment1ETD = entityPM.Transshipment1ETD;
                    entityMasterData.Transshipment1STD = entityPM.Transshipment1STD;
                    entityMasterData.Transshipment1STA = entityPM.Transshipment1STA;
                    entityMasterData.Transshipment2ATA = entityPM.Transshipment2ATA;
                    entityMasterData.Transshipment2ATD = entityPM.Transshipment2ATD;
                    entityMasterData.Transshipment2ETA = entityPM.Transshipment2ETA;
                    entityMasterData.Transshipment2ETD = entityPM.Transshipment2ETD;
                    entityMasterData.Transshipment2STD = entityPM.Transshipment2STD;
                    entityMasterData.Transshipment2STA = entityPM.Transshipment2STA;
                    entityMasterData.Transshipment3ATA = entityPM.Transshipment3ATA;
                    entityMasterData.Transshipment3ATD = entityPM.Transshipment3ATD;
                    entityMasterData.Transshipment3ETA = entityPM.Transshipment3ETA;
                    entityMasterData.Transshipment3ETD = entityPM.Transshipment3ETD;
                    entityMasterData.Transshipment3STD = entityPM.Transshipment3STD;
                    entityMasterData.Transshipment3STA = entityPM.Transshipment3STA;
                    entityMasterData.BookingConfirmedBy = entityPM.BookingConfirmedBy;
                    entityMasterData.BookingConfirmationNumber = entityPM.BookingConfirmationNumber;
                    entityMasterData.MainCarriageCarrierNumber = entityPM.MainCarriageCarrierNumber;
                    entityMasterData.CargonautFWBStatusCode = entityPM.CargonautFWBStatusCode;
                    entityMasterData.CargonautFWBStatusDate = entityPM.CargonautFWBStatusDate;
                    entityMasterData.PreCarriageATA = entityPM.PreCarriageATA;
                    entityMasterData.PreCarriageATD = entityPM.PreCarriageATD;
                    entityMasterData.PreCarriageETA = entityPM.PreCarriageETA;
                    entityMasterData.PreCarriageETD = entityPM.PreCarriageETD;
                    entityMasterData.OnCarriageATA = entityPM.OnCarriageATA;
                    entityMasterData.OnCarriageATD = entityPM.OnCarriageATD;
                    entityMasterData.OnCarriageETA = entityPM.OnCarriageETA;
                    entityMasterData.OnCarriageETD = entityPM.OnCarriageETD;

                    CalculateFinalDestinationPort(entityPM, entityMasterData);
                    FillEstimatedDatesFields(entityMasterData, entityPM);
                }
            }

            else
            {
                entityPoco.PreForwardingATA = entityPM.PreForwardingATA;
                entityPoco.PreForwardingATD = entityPM.PreForwardingATD;
                entityPoco.PreForwardingETA = entityPM.PreForwardingETA;
                entityPoco.PreForwardingETD = entityPM.PreForwardingETD;
                entityPoco.OnForwardingATA = entityPM.OnForwardingATA;
                entityPoco.OnForwardingATD = entityPM.OnForwardingATD;
                entityPoco.OnForwardingETA = entityPM.OnForwardingETA;
                entityPoco.OnForwardingETD = entityPM.OnForwardingETD;
            }

            if (packagesListCount == 0)
            {
                entityPoco.HasContainerException = false;
                entityPoco.INTTRALastStatusDate = null;
            }

            MapCalculatedFields(entityPM, entityPoco, entityMasterData);
        }

        private static void CalculateFinalDestinationPort(ShipmentPM entityPM, ShipmentMasterData entityMasterData)
        {
            if (entityMasterData != null)
            {
                string myFinalDestinationPortId = null;

                if (entityMasterData.Transshipment3ToPortId != null)
                {
                    myFinalDestinationPortId = entityMasterData.Transshipment3ToPortId;
                }

                else if (entityMasterData.Transshipment2ToPortId != null)
                {
                    myFinalDestinationPortId = entityMasterData.Transshipment2ToPortId;
                }

                else if (entityMasterData.Transshipment1ToPortId != null)
                {
                    myFinalDestinationPortId = entityMasterData.Transshipment1ToPortId;
                }

                else
                {
                    myFinalDestinationPortId = entityMasterData.MainCarriageToPortId;
                }

                if (isMappingEntityPM)
                {
                    entityPM.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
                }

                entityMasterData.MainCarriageFinalDestinationPortId = myFinalDestinationPortId;
            }
        }
        private static void FillEstimatedDatesFields(ShipmentMasterData entityMasterData, ShipmentPM entityPM)
        {
            if (entityMasterData.MainCarriageSTD == null)
            {
                entityMasterData.MainCarriageSTD = entityPM.MainCarriageETD;
            }
            if (entityMasterData.MainCarriageSTA == null)
            {
                entityMasterData.MainCarriageSTA = entityPM.MainCarriageETA;
            }
            if (entityMasterData.Transshipment1STD == null)
            {
                entityMasterData.Transshipment1STD = entityPM.Transshipment1ETD;
            }
            if (entityMasterData.Transshipment1STA == null)
            {
                entityMasterData.Transshipment1STA = entityPM.Transshipment1ETA;
            }
            if (entityMasterData.Transshipment2STD == null)
            {
                entityMasterData.Transshipment2STD = entityPM.Transshipment2ETD;
            }
            if (entityMasterData.Transshipment2STA == null)
            {
                entityMasterData.Transshipment2STA = entityPM.Transshipment2ETA;
            }
            if (entityMasterData.Transshipment3STD == null)
            {
                entityMasterData.Transshipment3STD = entityPM.Transshipment3ETD;
            }
            if (entityMasterData.Transshipment3STA == null)
            {
                entityMasterData.Transshipment3STA = entityPM.Transshipment3ETA;
            }
        }
        private static string GetConcurrencyFieldValue_String(string oiginalValue, string newValue, string serverValue)
        {
            if (oiginalValue == null && newValue == null)
            {
                return serverValue;
            }

            else if (oiginalValue == null && newValue != null)
            {
                return newValue;
            }

            else if (oiginalValue != null && serverValue == null)
            {
                return newValue;
            }

            else if (oiginalValue != newValue)
            {
                return newValue;
            }

            else
            {
                return serverValue;
            }
        }
        private static int? GetConcurrencyFieldValue_Int(int? oiginalValue, int? newValue, int? serverValue)
        {
            if (oiginalValue == null && newValue == null)
            {
                return serverValue;
            }

            else if (oiginalValue == null && newValue != null)
            {
                return newValue;
            }

            else if (oiginalValue != null && serverValue == null)
            {
                return newValue;
            }

            else if (oiginalValue != newValue)
            {
                return newValue;
            }

            else
            {
                return serverValue;
            }
        }
        private static double? GetConcurrencyFieldValue_Double(double? oiginalValue, double? newValue, double? serverValue)
        {
            if (oiginalValue == null && newValue == null)
            {
                return serverValue;
            }

            else if (oiginalValue == null && newValue != null)
            {
                return newValue;
            }

            else if (oiginalValue != null && serverValue == null)
            {
                return newValue;
            }

            else if (oiginalValue != newValue)
            {
                return newValue;
            }

            else
            {
                return serverValue;
            }
        }
        private static DateTime? GetConcurrencyFieldValue_Date(DateTime? oiginalValue, DateTime? newValue, DateTime? serverValue)
        {
            if (oiginalValue == null && newValue == null)
            {
                return serverValue;
            }

            else if (oiginalValue == null && newValue != null)
            {
                return newValue;
            }

            else if (oiginalValue != null && serverValue == null)
            {
                return newValue;
            }

            else if (oiginalValue != newValue)
            {
                return newValue;
            }

            else
            {
                return serverValue;
            }
        }
    }
}
