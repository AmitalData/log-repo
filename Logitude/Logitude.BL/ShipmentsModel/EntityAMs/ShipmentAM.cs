using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityAMs
{
    public class ShipmentAM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int ImporterTenant { get; set; }
        public string TransportModeId { get; set; }
        public string DirectionId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ForwarderShipmentNumber { get; set; }
        public string CustomerShipmentNumber { get; set; }
        public CodeProperties Branch { get; set; }
        public CodeProperties Department { get; set; }
        public CodeProperties Shipper { get; set; }
        public CodeProperties Consignee { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? OnCarriageATA { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public string StatusId { get; set; }
        public string StatusCode { get; set; }
        public DateTime? StatusDate { get; set; }
        public CodeProperties FromPort { get; set; }
        public CodeProperties ToPort { get; set; }
        public string ShipmentTypeId { get; set; }
        public string House { get; set; }
        public string DescriptionOfGoods { get; set; }
        public CodeProperties Customer { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeName { get; set; }
        public CodeProperties PreCarriageFromPort { get; set; }
        public CodeProperties PreCarriageToPort { get; set; }
        public CodeProperties OnCarriageToPort { get; set; }
        public string ConsigneeReference1 { get; set; }
        public string CustomerReference1 { get; set; }
        public string ConsigneeReference2 { get; set; }
        public string CustomerReference2 { get; set; }
        public string ShipmentCustomerTypeCode { get; set; }
        public bool IsCancelled { get; set; }
        public string CarrierTransportDocumentNumber { get; set; }
        public string ForwarderPartnerId { get; set; }
        public string FreightPrepaidCollectId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public bool IsOperationalClosed { get; set; }
        public string OriginalStatusCode { get; set; }
        public DateTime? OriginalStatusDate { get; set; }

        public bool HasException { get; set; }
        public DateTime? ExceptionDate { get; set; }
        public string ExceptionDescription { get; set; }

        public string DeclarationXMLData { get; set; }
  
        public bool IsImporterApprovalRequired { get; set; }
        public string VersionApproved { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public string Notes { get; set; }
        public string ShipmentAddtionalDataXML { get; set; }
        public int? ForwarderCode { get; set; }
        public int? Quantity { get; set; }
        public double? Weight { get; set; }
        public string Master { get; set; }
        public DateTime? CustomsClearanceDate { get; set; }
        public bool SendUpdatesToAgentEnabled { get; set; }
        public CodeProperties Incoterm { get; set; }
        public List<ShipmentPackageAM> ShipmentPackagesAM { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string ForwardingPartnerId { get; set; }
        public string ForwardingPartnerTenant { get; set; }
        public CodeProperties Agent { get; set; }
        public string CustomerReference3 { get; set; }


        /*
        HasException , ExceptionDate , ExceptionDescription
ShipmentNumber
MasterShipmentDataNumber
         */


    }
}
