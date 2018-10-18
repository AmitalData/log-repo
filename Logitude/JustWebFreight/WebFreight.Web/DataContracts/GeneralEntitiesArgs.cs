using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.DataContracts
{
    public class GeneralEntitiesArgs
    {
        public List<AdvancedQueryFilterPM> AdvancedQueryFilterPMs { get; set; }
        public List<QueryColumnPM> QueryColumnsPMs { get; set; }
        public List<QueryColumnPM> RemovedQueryColumnsPMs { get; set; }
        public List<AdvancedQueryFilterPM> RemovedQueryFilters { get; set; }
        public QueryPM QueryPM { get; set; }
        public int Tenant { get; set; }
    }
}