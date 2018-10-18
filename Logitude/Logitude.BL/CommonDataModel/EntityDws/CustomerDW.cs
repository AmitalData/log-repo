using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityDws
{
   public class CustomerDW
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string Code { get; set; }
        public string CustomerStatusName { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string CountryID { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string ForwarderId { get; set; }
        public string CardId { get; set; }
        public string ForwarderName { get; set; }
        public string CustomsAgentId { get; set; }
        public string CustomsAgentName { get; set; }
        public string CustomerSizeID { get; set; }
        public string CustomerSizeName { get; set; }
        public string RankId { get; set; }
        public string RankName { get; set; }
        public string SalesmanUserId { get; set; }
        public string SalesmanUserName { get; set; }
        public bool ActivityWatch { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartWorkingDate { get; set; }

        public string   ClientForFollowUp { get; set; }
        public string ClientID { get; set; }
        public string ClientNameHEB { get; set; }
        public string PrimaryContactId { get; set; }
        public string EnglishName { get; set; }
        public string BusinessPhone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string TenantNumber { get; set; }
        public string Reseller { get; set; }

        public string CardType { get; set; }
        public bool IsCustomer { get; set; }

        private List<CustomerAdditionalServicePM>otherServices;
        [Include]
        [DataMember]
        public virtual List<CustomerAdditionalServicePM> OtherServices
        {
            get
            {
                if (this.otherServices == null)
                {
                    otherServices = new List<CustomerAdditionalServicePM>();
                }
                return this.otherServices;
            }
            set
            {
                if (value != null)
                {
                    otherServices = value;
                }
            }
        }




        private List<CustomerCompetitorPM> customerCompetitors;
        [Include]
        [DataMember]
        public virtual List<CustomerCompetitorPM> CustomerCompetitors
        {
            get
            {
                if (this.customerCompetitors == null)
                {
                    customerCompetitors = new List<CustomerCompetitorPM>();
                }
                return this.customerCompetitors;
            }
            set
            {
                if (value != null)
                {
                    customerCompetitors = value;
                }
            }
        }


    }


}
