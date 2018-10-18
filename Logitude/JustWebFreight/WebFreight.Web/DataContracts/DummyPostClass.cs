using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class DummyPostClass
    {
        [Key]
        public int Id { get; set; }
        public DateTime? PostDate { get; set; }
        public string Post { get; set; }
        public string OpportunityTopic { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
    }
}