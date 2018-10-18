using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class AddressType
    {
        [Key]
        public string Id { get; set; }       
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}