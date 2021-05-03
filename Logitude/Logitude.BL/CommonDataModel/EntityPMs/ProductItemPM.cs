using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class ProductItemPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomerId { get; set; }
        public string SKU { get; set; }
        public string Remarks { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }

        private List<HTSCodePM> HTScodes;
        [Include]
        [Association("ProductItemHTScodesProduct", "Id", "ItemId")]
        [Composition]
        [DataMember]
        public virtual List<HTSCodePM> HTSCodes
        {
            get
            {
                if (HTScodes == null)
                {
                    HTScodes = new List<HTSCodePM>();
                }

                return HTScodes;
            }

            set
            {
                HTScodes = value;
            }
        }

        public string HTSCodeByCountry { get; set; }        
        public string ItemCode { get; set; }
        public List<HTSCodePM> HTSCodeChangeSet { get; set; }

    }
}
