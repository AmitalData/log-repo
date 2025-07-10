using AmitalCloud.Infrastructure.Domain.BaseClasses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CustomerCompetitorPM : BaseEntityPM
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string CompetitorId { get; set; }
        public string CustomerName { get; set; }
        public string CompetitorName { get; set; }
        public string CompetitorWebsite { get; set; }

        public string CompetitorStrengths { get; set; }
        public string CompetitorWeaknesses { get; set; }
        public string CompetitorOpportunity { get; set; }
        public string CompetitorThreat { get; set; }




        private List<CustomerCompetitorProductPM> customerCompetitorProducts;
        [Include]
        [Association("CustomerCompetitorProductCustomerCompetitor", "CustomerId", "CustomerId")]
        [Composition]
        public virtual List<CustomerCompetitorProductPM> CustomerCompetitorProducts
        {
            get
            {
                if (this.customerCompetitorProducts == null)
                {
                    customerCompetitorProducts = new List<CustomerCompetitorProductPM>();
                }
                return this.customerCompetitorProducts;
            }
            set
            {
                if (value != null)
                {
                    customerCompetitorProducts = value;
                }
            }
        }

    }
}
