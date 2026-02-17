using System;
using System.Collections.Generic;
using System.Linq;

namespace WebFreight.Web.DataProviders
{
    public class ContainerDetailsVoyageDataProvider : BaseDataProvider
    {
        public string CustomerFilter { get; set; }
        public string DirectionFilter { get; set; }
        public string VoyageNumberFilter { get; set; }
        public string CreateDateFilter { get; set; }
        public string SalingDateFilter { get; set; }
        public int TotalContainersCount { get; set; }
                
        public List<ContainerVoyageDetailsGroup> GroupList { get; set; }
    }

    public class ContainerVoyageDetailsGroup
    {
        public string VoyageNumber { get; set; }
        public string VesselId { get; set; }
        public string ShippingLineId { get; set; }
        public string VesselName{ get; set; }
        public string ShippingLineName { get; set; }
        public int? TotalContainersCount { get; set; }
        public List<ContainerVoyageDetailsRecord> RecordsList { get; set; }
    }

    public class ContainerVoyageDetailsRecord
    {
        public string VoyageNumber { get; set; }
        public string VesselId { get; set; }
        public string ShippingLineId { get; set; }
        public string VesselName { get; set; }
        public string ShippingLineName { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public string SealNumber { get; set; }
        public int InsidePackagesCount { get; set; }        
        public string DescriptionOfGoods { get; set; }
        public double? GrossWeight { get; set; }
        public string LoadingPortCode { get; set; }
        public string DischargePortCode { get; set; }
        public string BookingNumber { get; set; }
        public double? REEFTemp { get; set; }
        public string IMO { get; set; }
        public string IMDGNumber { get; set; }
        public string IMOClass { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public string ReleasingAgentName { get; set; }
    }
}