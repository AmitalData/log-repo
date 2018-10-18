using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class LogitudeMessagesTransmissionLogList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CCS { get; set; }
        public string AirlineCode { get; set; }
        public string MessageTypeCode { get; set; }
        public string Prefix { get; set; }
        public string AWBNumber { get; set; }
        public string HAWB { get; set; }
        public DateTime? SentDate { get; set; }
        public string Participant { get; set; }
        public string IATACode { get; set; }
        public string CASSCode { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int? Pieces { get; set; }
        public decimal? GrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public decimal? ChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public decimal? Volume { get; set; }
        public string VolumeUnitCode { get; set; }
        public string DescriptionOfGoods { get; set; }
        public bool DirectParticipant { get; set; }
        public bool IsUpdatedinAirlineTenant { get; set; }
        public string SearchFields { get; set; }
    }
}
