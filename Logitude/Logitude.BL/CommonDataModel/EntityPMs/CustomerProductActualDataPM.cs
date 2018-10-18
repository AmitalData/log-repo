using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class CustomerProductActualDataPM
    {
        [Key]
        [DataMember]
        public string CustomerId { get; set; }
        [Key]
        [DataMember]
        public string ProductTypeCode { get; set; }
        [Key]
        [DataMember]
        public int Month { get; set; }
        [Key]
        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public decimal? TEU { get; set; }
        [DataMember]
        public int? NumberOfShipments { get; set; }
        [DataMember]
        public decimal? ChargeableWeight { get; set; }

        [DataMember]
        public decimal? Revenue { get; set; }

        [DataMember]
        public string MonthCode { get; set; }

        private List<CustomerProductLocationActualDataPM> productLocations;
        [Include]
        [Association("CustomerProductActualDataCustomerProductLocationActualData", "CustomerId,ProductTypeCode,Year,Month", "CustomerId,ProductTypeCode,Year,Month")]
        [Composition]
        public virtual List<CustomerProductLocationActualDataPM> ProductLocations
        {
            get
            {
                if (this.productLocations == null)
                {
                    productLocations = new List<CustomerProductLocationActualDataPM>();
                }

                return this.productLocations;
            }

            set
            {
                if (value != null)
                {
                    productLocations = value;
                }
            }
        }

        public ChangeSetOperation ChangeSetOp { get; set; }
        public List<CustomerProductLocationActualDataPM> LocationsChangeSet { get; set; }
    }
}
