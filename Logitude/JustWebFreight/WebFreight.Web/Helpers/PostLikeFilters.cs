using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class PostLikeFilters
    {
       
        public string PostId { get; set; }
        public string UserId { get; set; }
        public int Tenant { get; set; }
        public bool IsCancelled { get; set; }
    }
}