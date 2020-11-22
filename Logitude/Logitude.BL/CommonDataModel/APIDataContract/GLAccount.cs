using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.CommonDataModel.APIDataContract
{
    public class GLAccount
    {
        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public string InternalNumber { get; set; }

        public Currency Currency { get; set; }
        public ReconcileMethod ReconcileMethod { get; set; }
        public ChartOfAccount ChartOfAccount { get; set; }
    }

    public class ReconcileMethod
    {
        [XmlAttribute]
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
    }

    public class ChartOfAccount
    {
        [XmlAttribute]
        public string Code { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        public string LocalName { get; set; }
        public string EnglishName { get; set; }
    }

    public class GLAccountPM
    {
        public string Id { get; set; }
        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }
        public string EnglishName { get; set; }
        public bool? IsMultiCurrency { get; set; }

        public string CurrencyId { get; set; }
        public string ReconcileMethodCode { get; set; }
        public string ChartOfAccountCode { get; set; }
    }

    public class GLAccountQueryService
    {
        public GLAccountQueryService(int tenant)
        {
            
        }

        public GLAccount GLAccountCustomDataMapping(string Id, int Tenant, string ComputingPartnerName = "")
        {
            return new GLAccount();
        }

        public GLAccountPM GLAccountCustomDataMappingAndValidatin(GLAccount MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                return new GLAccountPM();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
