using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class AgentSharedManifestPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AgentReference { get; set; }
        public string Master { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ManifestXML { get; set; }
        public ManifestSL ManifestSL { get; set; }
        public string SearchFields { get; set; }
        public string TransportModeId { get; set; }
        public double? GrossWeight { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? TEU { get; set; }
        public int? PackagesQuantity { get; set; }
        public string ShipmentTypeId { get; set; }
        public string AgentId { get; set; }
        public string DirectionId { get; set; }
        public string FromPortId { get; set; }
        public string ToPortId { get; set; }
        public string StatusCode { get; set; }
        public string Routing { get; set; }
        public string StatusName { get; set; }
        public string TransportModeName { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentLevelName { get; set; }
        public bool CancelledBySenderAgent { get; set; }
        public List<SharedManifestTranslationPM> SharedManifestTranslations
        {
            get
            {
                if (sharedManifestTranslations == null)
                {
                    sharedManifestTranslations = new List<SharedManifestTranslationPM>();
                }
                return sharedManifestTranslations;
            }

            set
            {
                sharedManifestTranslations = value;
            }
        }

        List<SharedManifestTranslationPM> sharedManifestTranslations;
    }
}
