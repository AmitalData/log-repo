using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class AutomationArgs
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string AutomationsId { get; set; }
        public string RecipientValue { get; set; }
        public string RecipientType { get; set; }
        public int Order { get; set; }
    }
}