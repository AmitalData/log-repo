using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class BluesnapContract
    {
        [Key]
        public string Id { get; set; }      
        public string Code { get; set; }
        public string Name { get; set; }
        public string ContractId { get; set; }
        public string SearchFields { get; set; }
        public bool InActive { get; set; }
        public string BluesnapContractTypeCode { get; set; }

        [ForeignKey("BluesnapContractTypeCode")]
        public virtual BluesnapContractType BluesnapContractType { get; set; }
    }
}
