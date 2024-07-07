using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Department
    {
        [Key]
        public string Id { get; set; }
        
        //[Required]
        public int Tenant { get; set; }

        public string Code { get; set; }

        //[Required]
        //[StringLength(40, ErrorMessage = "The maximum length of the english name is 40!")]
        //[Display(Name = "Name")]
        public string EnglishName { get; set; }

        //[Required]
        //[StringLength(40, ErrorMessage = "The maximum length of the local name is 40!")]
        //[Display(Name = "local Name")]
        public string LocalName { get; set; }

        //[StringLength(250, ErrorMessage = "The maximum length of the remarks is 250!")]
        public string Notes { get; set; }

        //[Required]
        //[Display(Name = "Inactive")]
        public bool InActive { get; set; }
        public string SearchFields { get; set; }
        public DateTime? AutomaticLastUpdateDate { get; set; }

        //public virtual  List<User> Users { get; set; }
        ////[Include]
        ////[Association("ShipmentDepartment", "Id", "DepartmentId")]
        //public List<Shipment> Shipments { get; set; }


        ////[Include]
        ////[Association("QuoteDepartment", "Id", "DepartmentId")]
        //public List<Quote> Quotes { get; set; }

        public string DirectionId { get; set; }


    }
}
