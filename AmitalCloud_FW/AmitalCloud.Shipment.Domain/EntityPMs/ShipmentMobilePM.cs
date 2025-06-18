
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{



 public partial class ShipmentMobilePM : EntityPM
    {

        // [Key]
        public string Id  { get; set; }
        #region Fields

        public string ComputedStatusId { get; set; }
        public DateTime? ComputedStatusDate { get; set; }
        public string ForeignPartnerCountryCode { get; set; }
        public string ComputedStatusName { get; set; }
        public string ShipmentLevelCode { get; set; }
        public int? NumberOfContainers { get; set; }
        public string AgentReference1 { get; set; }
        public string AgentReference2 { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }

        public string ConsigneeReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }

        public string CustomsDeclarationNumber { get; set; }

        public string CustomFileNumber { get; set; }


        public string ExceptionDescription { get; set; }

        public DateTime? ExceptionDate { get; set; }
        public bool HasException { get; set; }
        public bool CustomConnectToShipment { get; set; }

        public string PreCarriageTransportModeId { get; set; }
        public string PreCarriageCarrierNumber { get; set; }

        public string PreCarriageCarrierCode { get; set; }
        public string MasterShipmentNumber { get; set; }
        public double? ChargeableWeightInKG { get; set; }

        public bool IsShipmentTracking{ get; set; }
        public double? GrossWeightInKG { get; set; }


        public double? ChargeableWeight { get; set; }


        public double? GrossWeight { get; set; }
        public string CurrentUserId { get; set; }
        public int Tenant { get; set; }
        public string CustomFileId { get; set; }

     
        public string ShipmentNumber { get; set; }

        public string PreCarriageFromPortCountryCode { get; set; }
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }


       public int? NumberOfPackages { get; set; }


        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }

        public string ShipmentTypeId { get; set; }


                    public string PreCarriageFromPortId { get; set; }
                    public string PreCarriageToPortId { get; set; }
                    public string PreCarriageCarrierId { get; set; }
    
                    public string PreCarriageCarrierName { get; set; }
               
                    public string PreCarriageFromPortCode { get; set; }
                    public string PreCarriageFromPortName { get; set; }
               
                    public string PreCarriageFromPortCountryName { get; set; }
                    public string PreCarriageToPortCode { get; set; }
                    public string PreCarriageToPortName { get; set; }
                    public string PreCarriageToPortCountryCode { get; set; }
                    public string PreCarriageToPortCountryName { get; set; }
                    public DateTime? PreCarriageETD { get; set; }
                    public DateTime? PreCarriageATD { get; set; }
                    public DateTime? PreCarriageETA { get; set; }
                    public DateTime? PreCarriageATA { get; set; }



        public string House { get; set; }


        public DateTime CreateDateTime { get; set; }




        public string LongMaster { get; set; }




        public string GrossWeightUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }



        public double? VolumetricWeight { get; set; }




        public double? Volume { get; set; }



        public bool GrossWeightEdited { get; set; }

        public string VolumeUnitCode { get; set; }

        string statusname;
        public string StatusName { get { return statusname; } set { statusname = value; } }



        public string OnCarriageToPortCountryCode { get; set; }
        public string OnCarriageToPortCountryName { get; set; }
        public string OnCarriageToPortCode { get; set; }
        public string OnCarriageToPortName { get; set; }
        public string OnCarriageToPortId { get; set; }
        public string OnCarriageFromPortId { get; set; }
        public string OnCarriageFromPortCode { get; set; }
        public string OnCarriageFromPortCountryCode { get; set; }
        public string OnCarriageFromPortName { get; set; }

        public string OnCarriageCarrierNumber { get; set; }
        public string OnCarriageCarrierName { get; set; }
                 
        public string OnCarriageCarrierCode { get; set; }
        public string OnCarriageTransportModeId { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATA { get; set; }

        public string FromCountryCode { get; set; }
        public string ToCountryCode { get; set; }




        #endregion



        #region Routings




        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageCarrierCode { get; set; }

        public string MainCarriageFromPortCode { get; set; }
        public string MainCarriageFromPortName { get; set; }

        public string MainCarriageFromPortCountryCode { get; set; }

        public string MainCarriageToPortCode { get; set; }
        public string MainCarriageToPortName { get; set; }
        public string MainCarriageToPortCountryCode { get; set; }




        public string MainCarriageCarrierNumber { get; set; }


        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageETA { get; set; }


        public DateTime? Transshipment1ATD { get; set; }
        public DateTime? Transshipment1ATA { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ETA { get; set; }
        public string Transshipment1FromPortId { get; set; }

        public string Transshipment1CarrierNumber { get; set; }

        public string Transshipment1ToPortId { get; set; }

        public string Transshipment1FromPortCode { get; set; }
        public string Transshipment1FromPortName { get; set; }

        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment1ToPortName { get; set; }
           public string Transshipment1FromPortCountryCode { get; set; }
        public string Transshipment1ToPortCountryCode { get; set; }

        public string Transshipment1CarrierName { get; set; }

        public string Transshipment1CarrierCode { get; set; }

        public DateTime? Transshipment2ATD { get; set; }
        public DateTime? Transshipment2ATA { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment2ETA { get; set; }

        public string Transshipment2FromPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public string Transshipment2CarrierNumber { get; set; }
        public string Transshipment2CarrierName { get; set; }
      

       public string Transshipment2CarrierCode { get; set; }
        public string Transshipment2FromPortCode { get; set; }
        public string Transshipment2FromPortName { get; set; }
        public string Transshipment2FromPortCountryCode { get; set; }

        public string Transshipment2ToPortCode { get; set; }
        public string Transshipment2ToPortName { get; set; }
       public string Transshipment2ToPortCountryCode { get; set; }


        public DateTime? Transshipment3ATD { get; set; }
        public DateTime? Transshipment3ATA { get; set; }
        public DateTime? Transshipment3ETD { get; set; }
        public DateTime? Transshipment3ETA { get; set; }



   
        public string Transshipment3FromPortId { get; set; }
        public string Transshipment3ToPortId { get; set; }
   
        public string Transshipment3CarrierNumber { get; set; }

        public string Transshipment3CarrierName { get; set; }

        public string Transshipment3CarrierCode { get; set; }
        public string Transshipment3FromPortCode { get; set; }
        public string Transshipment3FromPortName { get; set; }
        public string Transshipment3FromPortCountryCode { get; set; }

        public string Transshipment3ToPortCode { get; set; }
        public string Transshipment3ToPortName { get; set; }
        public string Transshipment3ToPortCountryCode { get; set; }





  
        public string FromPortName { get; set; }
      
        public string FromPortCountryName { get; set; }

      
        public string ToPortName { get; set; }
    
        public string ToPortCountryName { get; set; }
        #endregion


        #region Partners


               public string CustomerReference { get; set; }


        public string ShipperName { get; set; }
      
        public string ConsigneeName { get; set; }




        #endregion



        public string MainCarriageVesselName { get; set; }
        public string PreCarriageVesselName { get; set; }
        public string OnCarriageVesselName { get; set; }
        public string Transshipment1VesselName { get; set; }
        public string Transshipment2VesselName { get; set; }
        public string Transshipment3VesselName { get; set; }

        public string MobileShipmentReference { get; set; }

        public bool IsHideMainCarrier { get; set; }
        public bool IsHidePickDelivCarrier { get; set; }

        public DateTime? StatusDate { get; set; }

        public string CustomerReference1 { get; set; }

        public string CustomerReference2 { get; set; }



        private List<TraceEventPM> eventsLists;
        public virtual List<TraceEventPM> EventsLists
        {
            get
            {
                if (eventsLists == null)
                {
                    eventsLists = new List<TraceEventPM>();
                }

                return this.eventsLists;
            }
            set
            {
                if (value != null)
                {
                    eventsLists = value;
                }
            }
        }




        private List<ShipmentPartnerPM> partnerLists;
        public virtual List<ShipmentPartnerPM> PartnerLists
        {
            get
            {
                if (partnerLists == null)
                {
                    partnerLists = new List<ShipmentPartnerPM>();
                }

                return this.partnerLists;
            }
            set
            {
                if (value != null)
                {
                    partnerLists = value;
                }
            }
        }



        private List<SharedLogisticDocumentPM> documentLists;
        public virtual List<SharedLogisticDocumentPM> DocumentLists
        {
            get
            {
                if (documentLists == null)
                {
                    documentLists = new List<SharedLogisticDocumentPM>();
                }

                return this.documentLists;
            }
            set
            {
                if (value != null)
                {
                    documentLists = value;
                }
            }
        }





        private List<ShipmentPickUpPM> shipmentPickUps;
        public virtual List<ShipmentPickUpPM> ShipmentPickUps
        {
            get
            {
                if (shipmentPickUps == null)
                {
                    shipmentPickUps = new List<ShipmentPickUpPM>();
                }

                return this.shipmentPickUps;
            }
            set
            {
                if (value != null)
                {
                    shipmentPickUps = value;
                }
            }
        }
     
        private List<ShipmentDeliveryPM> shipmentDeliveries;
        public virtual List<ShipmentDeliveryPM> ShipmentDeliveries
        {
            get
            {
                if (shipmentDeliveries == null)
                {
                    shipmentDeliveries = new List<ShipmentDeliveryPM>();
                }

                return this.shipmentDeliveries;
            }
            set
            {
                if (value != null)
                {
                    shipmentDeliveries = value;
                }
            }
        }

        private List<ShipmentPackagePM> shipmentPackagePM;
        public virtual List<ShipmentPackagePM> ShipmentPackages
        {
            get
            {
                if (shipmentPackagePM == null)
                {
                    shipmentPackagePM = new List<ShipmentPackagePM>();
                }

                return this.shipmentPackagePM;
            }
            set
            {
                if (value != null)
                {
                    shipmentPackagePM = value;
                }
            }
        }

        private List<ShipmentOrderPackagePM> shipmentOrderPackagePM;
        public virtual List<ShipmentOrderPackagePM> ShipmentOrderPackages
        {
            get
            {
                if (shipmentOrderPackagePM == null)
                {
                    shipmentOrderPackagePM = new List<ShipmentOrderPackagePM>();
                }

                return this.shipmentOrderPackagePM;
            }
            set
            {
                if (value != null)
                {
                    shipmentOrderPackagePM = value;
                }
            }
        }
    }
}
