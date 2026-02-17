using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Rank
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SearchFields { get; set; }
        //public List<Customer> Clients { get; set; }


    }
}