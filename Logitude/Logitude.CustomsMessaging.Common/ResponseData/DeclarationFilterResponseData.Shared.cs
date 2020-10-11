using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class DeclarationFilterResponseData : ResponseDataBase
    {
        public string ResponseStatusXML { get; set; }
        public string DeclarationID { get; set; }

        //public TPG_NG_8249_Web10_TPGDeclarationDetail myTPG_NG_8249_Web10_TPGDeclarationDetail { get; set; }
        public GeneralDetails GeneralDetailsData { get; set; }
        public List<Claim> ClaimList { get; set; }
        public List<Deficit> DeficitList { get; set; }
        public List<Guarantee> GuarenteeList { get; set; }

        public class GeneralDetails
        {
            public string customOfficeName { get; set; }
            public string customOfficeNumber { get; set; }
            public string declerationStatus { get; set; }
            public string externalID { get; set; }
            public bool externalIDSpecified { get; set; }
            public string name { get; set; }
            public string statusName { get; set; }
        }

        public class Claim
        {
            public int? agentExternalID { get; set; }
            public bool agentExternalIDSpecified { get; set; }
            public string agentName { get; set; }
            public decimal? claimAmount { get; set; }
            public bool claimAmountSpecified { get; set; }
            public string closeDate { get; set; }
            public bool closeDateSpecified { get; set; }
            public string createDate { get; set; }
            public string displayFileNumber { get; set; }
            public string fileNumber { get; set; }
            public string Numeral { get; set; }
            public int status { get; set; }
            public string statusName { get; set; }
            public string totalRefundAmount { get; set; }
            public bool totalRefundAmountSpecified { get; set; }
        }

        public class Deficit
        {
            public int? agentExternalID { get; set; }
            public bool agentExternalIDSpecified { get; set; }
            public string agentName { get; set; }
            public string closeDate { get; set; }
            public bool closeDateSpecified { get; set; }
            public string displayFileNumber { get; set; }
            public string estimatedBalance { get; set; }
            public string fileNumber { get; set; }
            public string Numeral { get; set; }
            public string productionDate { get; set; }
            public bool productionDateSpecified { get; set; }
            public int status { get; set; }
            public string statusName { get; set; }
            public string totalRefundAmount { get; set; }
            public bool totalRefundAmountSpecified { get; set; }
        }

        public class Guarantee
        {
            public int? agentExternalID { get; set; }
            public bool agentExternalIDSpecified { get; set; }
            public string agentName { get; set; }
            public decimal Amount { get; set; }
            public string displayFileNumber { get; set; }
            public string fileNumber { get; set; }
            public int fileType { get; set; }
            public string fileTypeName { get; set; }
            public int guaranteeStatus { get; set; }
            public string guaranteeStatusName { get; set; }
            public int Numeral { get; set; }
            public DateTime validity { get; set; }
        }
    }
}
