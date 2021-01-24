using Logitude.BL.ShipmentsModel.EntityPMs;
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
        public static void MapConcurrencyFields(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, int packagesListCount, bool isNewEntity, bool isMappingPM = true)
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

                    else
                    {
                        MapConcurrencyFields_OnEdited(entityPM, entityPoco, entityMasterData);
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
            entityPoco.PreCarriageATA = entityPM.PreCarriageATA;
            entityPoco.PreCarriageATD = entityPM.PreCarriageATD;
            entityPoco.PreCarriageETA = entityPM.PreCarriageETA;
            entityPoco.PreCarriageETD = entityPM.PreCarriageETD;
            entityPoco.OnCarriageATA = entityPM.OnCarriageATA;
            entityPoco.OnCarriageATD = entityPM.OnCarriageATD;
            entityPoco.OnCarriageETA = entityPM.OnCarriageETA;
            entityPoco.OnCarriageETD = entityPM.OnCarriageETD;

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

                    FillEstimatedDatesFields(entityMasterData, entityPM);
                }
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
        private static void MapConcurrencyFields_Client(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            if (isMappingEntityPM)
            {
                entityPM.ConcurrencyGUID = entityPM.NewConcurrencyGUID;
            }

            entityPoco.ConcurrencyGUID = entityPM.NewConcurrencyGUID;


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
        private static void MapConcurrencyFields_OnEdited(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData)
        {
            entityPoco.NumberOfPackages = GetConcurrencyFieldValue_Int(entityPM.NumberOfPackages_Original, entityPM.NumberOfPackages, entityPoco.NumberOfPackages);
            entityPoco.GrossWeight = GetConcurrencyFieldValue_Double(entityPM.GrossWeight_Original, entityPM.GrossWeight, entityPoco.GrossWeight);
            entityPoco.ChargeableWeight = GetConcurrencyFieldValue_Double(entityPM.ChargeableWeight_Original, entityPM.ChargeableWeight, entityPoco.ChargeableWeight);
            entityPoco.GrossWeightUnitCode = GetConcurrencyFieldValue_String(entityPM.GrossWeightUnitCode_Original, entityPM.GrossWeightUnitCode, entityPoco.GrossWeightUnitCode);
            entityPoco.PreCarriageATA = GetConcurrencyFieldValue_Date(entityPM.PreCarriageATA_Original, entityPM.PreCarriageATA, entityPoco.PreCarriageATA);
            entityPoco.PreCarriageATD = GetConcurrencyFieldValue_Date(entityPM.PreCarriageATD_Original, entityPM.PreCarriageATD, entityPoco.PreCarriageATD);
            entityPoco.PreCarriageETA = GetConcurrencyFieldValue_Date(entityPM.PreCarriageETA_Original, entityPM.PreCarriageETA, entityPoco.PreCarriageETA);
            entityPoco.PreCarriageETD = GetConcurrencyFieldValue_Date(entityPM.PreCarriageETD_Original, entityPM.PreCarriageETD, entityPoco.PreCarriageETD);
            entityPoco.OnCarriageATA = GetConcurrencyFieldValue_Date(entityPM.OnCarriageATA_Original, entityPM.OnCarriageATA, entityPoco.OnCarriageATA);
            entityPoco.OnCarriageATD = GetConcurrencyFieldValue_Date(entityPM.OnCarriageATD_Original, entityPM.OnCarriageATD, entityPoco.OnCarriageATD);
            entityPoco.OnCarriageETA = GetConcurrencyFieldValue_Date(entityPM.OnCarriageETA_Original, entityPM.OnCarriageETA, entityPoco.OnCarriageETA);
            entityPoco.OnCarriageETD = GetConcurrencyFieldValue_Date(entityPM.OnCarriageETD_Original, entityPM.OnCarriageETD, entityPoco.OnCarriageETD);
            entityPoco.INTTRABookingStatusCode = GetConcurrencyFieldValue_String(entityPM.INTTRABookingStatusCode_Original, entityPM.INTTRABookingStatusCode, entityPoco.INTTRABookingStatusCode);

            if (entityPM.ShipmentLevelCode != "H")
            {
                if (entityMasterData != null)
                {
                    entityMasterData.MainCarriageFromPortId = GetConcurrencyFieldValue_String(entityPM.MAN_FromPortId_Original, entityPM.MainCarriageFromPortId, entityMasterData.MainCarriageFromPortId);
                    entityMasterData.MainCarriageToPortId = GetConcurrencyFieldValue_String(entityPM.MainCarriageToPortId_Original, entityPM.MainCarriageToPortId, entityMasterData.MainCarriageToPortId);
                    entityMasterData.Transshipment1ToPortId = GetConcurrencyFieldValue_String(entityPM.TR1_ToPortId_Original, entityPM.Transshipment1ToPortId, entityMasterData.Transshipment1ToPortId);
                    entityMasterData.Transshipment2ToPortId = GetConcurrencyFieldValue_String(entityPM.TR2_ToPortId_Original, entityPM.Transshipment2ToPortId, entityMasterData.Transshipment2ToPortId);
                    entityMasterData.Transshipment3ToPortId = GetConcurrencyFieldValue_String(entityPM.TR3_ToPortId_Original, entityPM.Transshipment3ToPortId, entityMasterData.Transshipment3ToPortId);
                    entityMasterData.MainCarriageFinalDestinationPortId = GetConcurrencyFieldValue_String(entityPM.FIN_PortId_Original, entityPM.MainCarriageFinalDestinationPortId, entityMasterData.MainCarriageFinalDestinationPortId);
                    entityMasterData.MainCarriageATD = GetConcurrencyFieldValue_Date(entityPM.MainCarriageATD_Original, entityPM.MainCarriageATD, entityMasterData.MainCarriageATD);
                    entityMasterData.MainCarriageETD = GetConcurrencyFieldValue_Date(entityPM.MainCarriageETD_Original, entityPM.MainCarriageETD, entityMasterData.MainCarriageETD);
                    entityMasterData.MainCarriageETA = GetConcurrencyFieldValue_Date(entityPM.MainCarriageETA_Original, entityPM.MainCarriageETA, entityMasterData.MainCarriageETA);
                    entityMasterData.MainCarriageATA = GetConcurrencyFieldValue_Date(entityPM.MainCarriageATA_Original, entityPM.MainCarriageATA, entityMasterData.MainCarriageATA);
                    entityMasterData.MainCarriageSTD = GetConcurrencyFieldValue_Date(entityPM.MainCarriageSTD_Original, entityPM.MainCarriageSTD, entityMasterData.MainCarriageSTD);
                    entityMasterData.MainCarriageSTA = GetConcurrencyFieldValue_Date(entityPM.MainCarriageSTA_Original, entityPM.MainCarriageSTA, entityMasterData.MainCarriageSTA);
                    entityMasterData.Transshipment1ATA = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ATA_Original, entityPM.Transshipment1ATA, entityMasterData.Transshipment1ATA);
                    entityMasterData.Transshipment1ATD = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ATD_Original, entityPM.Transshipment1ATD, entityMasterData.Transshipment1ATD);
                    entityMasterData.Transshipment1ETA = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ETA_Original, entityPM.Transshipment1ETA, entityMasterData.Transshipment1ETA);
                    entityMasterData.Transshipment1ETD = GetConcurrencyFieldValue_Date(entityPM.Transshipment1ETD_Original, entityPM.Transshipment1ETD, entityMasterData.Transshipment1ETD);
                    entityMasterData.Transshipment1STD = GetConcurrencyFieldValue_Date(entityPM.Transshipment1STD_Original, entityPM.Transshipment1STD, entityMasterData.Transshipment1STD);
                    entityMasterData.Transshipment1STA = GetConcurrencyFieldValue_Date(entityPM.Transshipment1STA_Original, entityPM.Transshipment1STA, entityMasterData.Transshipment1STA);
                    entityMasterData.Transshipment2ATA = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ATA_Original, entityPM.Transshipment2ATA, entityMasterData.Transshipment2ATA);
                    entityMasterData.Transshipment2ATD = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ATD_Original, entityPM.Transshipment2ATD, entityMasterData.Transshipment2ATD);
                    entityMasterData.Transshipment2ETA = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ETA_Original, entityPM.Transshipment2ETA, entityMasterData.Transshipment2ETA);
                    entityMasterData.Transshipment2ETD = GetConcurrencyFieldValue_Date(entityPM.Transshipment2ETD_Original, entityPM.Transshipment2ETD, entityMasterData.Transshipment2ETD);
                    entityMasterData.Transshipment2STD = GetConcurrencyFieldValue_Date(entityPM.Transshipment2STD_Original, entityPM.Transshipment2STD, entityMasterData.Transshipment2STD);
                    entityMasterData.Transshipment2STA = GetConcurrencyFieldValue_Date(entityPM.Transshipment2STA_Original, entityPM.Transshipment2STA, entityMasterData.Transshipment2STA);
                    entityMasterData.Transshipment3ATA = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ATA_Original, entityPM.Transshipment3ATA, entityMasterData.Transshipment3ATA);
                    entityMasterData.Transshipment3ATD = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ATD_Original, entityPM.Transshipment3ATD, entityMasterData.Transshipment3ATD);
                    entityMasterData.Transshipment3ETA = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ETA_Original, entityPM.Transshipment3ETA, entityMasterData.Transshipment3ETA);
                    entityMasterData.Transshipment3ETD = GetConcurrencyFieldValue_Date(entityPM.Transshipment3ETD_Original, entityPM.Transshipment3ETD, entityMasterData.Transshipment3ETD);
                    entityMasterData.Transshipment3STD = GetConcurrencyFieldValue_Date(entityPM.Transshipment3STD_Original, entityPM.Transshipment3STD, entityMasterData.Transshipment3STD);
                    entityMasterData.Transshipment3STA = GetConcurrencyFieldValue_Date(entityPM.Transshipment3STA_Original, entityPM.Transshipment3STA, entityMasterData.Transshipment3STA);
                    entityMasterData.BookingConfirmedBy = GetConcurrencyFieldValue_String(entityPM.BookingConfirmedBy_Original, entityPM.BookingConfirmedBy, entityMasterData.BookingConfirmedBy);
                    entityMasterData.BookingConfirmationNumber = GetConcurrencyFieldValue_String(entityPM.BookingConfNumber_Original, entityPM.BookingConfirmationNumber, entityMasterData.BookingConfirmationNumber);
                    entityMasterData.MainCarriageCarrierNumber = GetConcurrencyFieldValue_String(entityPM.MAN_CarrierNumber_Original, entityPM.MainCarriageCarrierNumber, entityMasterData.MainCarriageCarrierNumber);
                }
            }
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
            entityPoco.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
            entityPoco.PreCarriageATA = entityPM.PreCarriageATA;
            entityPoco.PreCarriageATD = entityPM.PreCarriageATD;
            entityPoco.PreCarriageETA = entityPM.PreCarriageETA;
            entityPoco.PreCarriageETD = entityPM.PreCarriageETD;
            entityPoco.OnCarriageATA = entityPM.OnCarriageATA;
            entityPoco.OnCarriageATD = entityPM.OnCarriageATD;
            entityPoco.OnCarriageETA = entityPM.OnCarriageETA;
            entityPoco.OnCarriageETD = entityPM.OnCarriageETD;
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
                    
                    CalculateFinalDestinationPort(entityPM, entityMasterData);
                    FillEstimatedDatesFields(entityMasterData, entityPM);
                }
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
