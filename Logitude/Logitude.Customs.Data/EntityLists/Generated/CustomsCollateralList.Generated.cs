using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Logitude.Customs.Data.EntityLists
{
    [DataContract]
    public partial class CustomsCollateralList
    {

        [Key]
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string CollateralRequestNumber { get; set; }
        [DataMember]
        public DateTime? RequestValidityDate { get; set; }
        [DataMember]
        public DateTime? CollateralValidityDate { get; set; }
        [DataMember]
        public string CollateralRequestStatusCode { get; set; }
        [DataMember]
        public string RequestedCollateralTypeCode { get; set; }
        [DataMember]
        public string OrganizationUnitTypeCode { get; set; }
        [DataMember]
        public string CustomsHouseTypeCode { get; set; }
        [DataMember]
        public string WorkerName { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public string FileNo { get; set; }
        [DataMember]
        public string CustomsEntityTypeCode { get; set; }
        [DataMember]
        public string EntityIdKey1 { get; set; }
        [DataMember]
        public string EntityIdKey2 { get; set; }
        [DataMember]
        public string EntityIdKey3 { get; set; }
        [DataMember]
        public string CollateralRequestStatusName { get; set; }
        [DataMember]
        public string RequestedCollateralTypeName { get; set; }
        [DataMember]
        public string CustomsEntityTypeName { get; set; }
        [DataMember]
        public bool IncludingThirdPartyGuarantee { get; set; }
        [DataMember]
        public string DeclarationId { get; set; }
        [DataMember]
        public string SearchFields { get; set; }
        [DataMember]
        public string CustomsHouseTypeName { get; set; }
        [DataMember]
        public string OrganizationUnitTypeName { get; set; }
        [DataMember]
        public DateTime? CreateDateTime { get; set; }
        [DataMember]
        public bool IsClosed { get; set; }
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public bool IsAnswer { get; set; }
    }

}
