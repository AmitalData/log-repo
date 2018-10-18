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
  
    [ServiceContract]
    public interface IActivityDWWcfService
    {
        [OperationContract]
        List<ActivitiyDW> GetActivitiesByDates(int tenant, DateTime fromDate, DateTime toDate, int skip, int take, ref Response response);

        [OperationContract]
        int GetActivitiesCountByDates(int tenant, DateTime fromDate, DateTime toDate, ref Response response);

        [OperationContract]
        List<ActivitiyDW> GetActivitiesByUpdateDate(int tenant, DateTime updateDate, int skip, int take, ref Response response);


        [OperationContract]
        int GetActivitiesCountByUpdateDate(int tenant, DateTime updateDate, ref Response response);
    }
}
