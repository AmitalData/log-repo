using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Logitude.BL.GlobalModel.EntityPMs
{
    public class BluesnapContractPM
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContractId { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
        public int Tenant { get; set; }
        public string BluesnapContractTypeCode { get; set; }
        public string BluesnapContractTypeName { get; set; }

    }
}
