using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.DataContracts
{
    public class SharedLogisticsStatusStatistics
    {
        [Key]
        public string Id { get; set; }

        public int InvitedCustomersCount { get; set; }
        public int NotInvitedCustomersCount { get; set; }
        public int ActivatedCustomersCount { get; set; }
        public int ActivatedCustomersForMobileCount { get; set; }

        public int InvitedAgentsCount { get; set; }
        public int NotInvitedAgentsCount { get; set; }
        public int ActivatedAgentsCount { get; set; }

        public int NotInvitedCToolPartnersCount { get; set; }
        public int InvitedCToolPartnersCount { get; set; }
        public int ActivatedCToolPartnersCount { get; set; }



    }
}
