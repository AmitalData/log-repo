using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using WebFreight.Web.DataContracts;
using Logitude.Server.Tools;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOpportunityWcfService" in both code and config file together.
    [ServiceContract]
    public interface IOpportunityWcfService
    {

        [OperationContract]
        List<OpportunityList> GetOpportunityList(string email, string searchText, int tenant, int skip, int take, OpportunityApiFilters filters, ref Response response);


        [OperationContract]
        CustomerList GetCustomerListByOpportunityId(string opportunityId, int tenant, ref Response response);

        OpportunityList GetOpportunityListById(string id, int tenant, ref Response response);
    }
}
