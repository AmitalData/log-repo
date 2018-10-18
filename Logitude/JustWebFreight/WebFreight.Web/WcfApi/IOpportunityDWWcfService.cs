using Logitude.CRM.BL.EntityDws;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOpportunitiesServiceDW" in both code and config file together.
    [ServiceContract]
    public interface IOpportunityDWWcfService
    {
        [OperationContract]
        List<OpportunityDW> GetOpportunitiesByDates(int tenant, DateTime fromDate, DateTime toDate, int skip, int take, ref Response response);

        [OperationContract]
        List<OpportunityDW> GetOpportunitiesByUpdateDate(int tenant, DateTime updateDate, int skip, int take, ref Response response);

        [OperationContract]
        int GetOpportunitiesCountByDates(int tenant, DateTime fromDate, DateTime toDate, ref Response response);

        [OperationContract]
        int GetOpportunitiesCountByUpdateDate(int tenant, DateTime updateDate, ref Response response);
    }
}
