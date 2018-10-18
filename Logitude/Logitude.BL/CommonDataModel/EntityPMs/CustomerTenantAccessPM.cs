using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
   public class CustomerTenantAccessPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public int CustomerTenant { get; set; }
        public DateTime? LastShipmentDate { get; set; }
        public string ContactName { get; set; }
        public string CompanyVat { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactMobile { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string UpdatedByUserId { get; set; }
        public string UpdatedByUserName { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string SearchFields { get; set; }
        public bool IsPrivateLabelCustomer { get; set; }
        public string CustomCompanyName { get; set; }
        public string StockTypeCode { get; set; }

        private List<CustomerTenantAccessCardPM> customerTenantAccessCards;
        [Include]
        [Association("CustomerTenantAccessCustomerTenantAccessCard", "Id", "CustomerTenantAccessId")]
        [Composition]
        public virtual List<CustomerTenantAccessCardPM> CustomerTenantAccessCards
        {
            get
            {

                if (this.customerTenantAccessCards == null)
                {
                    customerTenantAccessCards = new List<CustomerTenantAccessCardPM>();
                }
                return this.customerTenantAccessCards;
            }
            set
            {
                if (value != null)
                {
                    customerTenantAccessCards = value;
                }
            }
        }
    }
}
