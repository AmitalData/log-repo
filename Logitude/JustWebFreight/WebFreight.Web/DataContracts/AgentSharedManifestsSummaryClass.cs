using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class AgentSharedManifestsSummaryClass
    {

        public int AgentSharedManifestsCancelledCount { get; set; }
        public int AgentSharedManifestsAllCount { get; set; }
        public int AgentSharedManifestsInlandCount { get; set; }
        public int AgentSharedManifestsOceanCount { get; set; }
        public int AgentSharedManifestsAirCount { get; set; }



          

    }
}