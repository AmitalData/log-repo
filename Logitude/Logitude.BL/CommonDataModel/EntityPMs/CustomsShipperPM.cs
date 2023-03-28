using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
  public  class CustomsShipperPM : ObjectCustomFieldPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomsShipperCode { get; set; }
        public string ValidDepositionNumber { get; set; }
        public DateTime? ValidityStartDate { get; set; }
        public DateTime? ValidityEndDate { get; set; }
        public string SearchFields { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public string CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string ShipperVAT { get; set; }
        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CityName { get; set; }
        public bool IsChange { get; set; }
        private List<AddressPM> addresses;
        public virtual List<AddressPM> Addresses
        {
            get
            {
                if (addresses == null)
                {
                    addresses = new List<AddressPM>();
                }
                return addresses;
            }
            set { addresses = value; }
        }
    }
}
