 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsRequestsSheetRepository:IRepository<CustomsRequestsSheet>
   {
        
		public List<CustomsRequestsSheet> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public CustomsRequestsSheet GetLastCRSByCustomfileStatusInterface(string customFileNumber, string requestStatusCode, string interfaceTypeCode, int tenant)
        {

            return (from a in context.CustomsRequestsSheets
                    where a.CustomFileNo == customFileNumber && a.Tenant == tenant
                    where a.RequestStatusCode == requestStatusCode
                    where a.InterfaceTypeCode == interfaceTypeCode
                    orderby a.RequestCreateDate descending
                    select a).First();
        }
        public CustomsRequestsSheet GetLastCRSByCustomfileStatusInterfaceFirstOrDefault(string customFileNumber, string requestStatusCode, string interfaceTypeCode, int tenant)
        {

            return (from a in context.CustomsRequestsSheets
                    where a.CustomFileNo == customFileNumber && a.Tenant == tenant
                    where a.RequestStatusCode == requestStatusCode
                    where a.InterfaceTypeCode == interfaceTypeCode
                    orderby a.RequestCreateDate descending
                    select a).FirstOrDefault();
        }

        public List<CustomsRequestsSheet> GetCustomsRequestsSheetByCustomFileNumber(string customFileNumber, int tenant)
        {

            return (from a in context.CustomsRequestsSheets.Include("User").Include("User.Contact").Include("InterfaceManagement").Include("CustomsRequestsSheetStatus")
                    where a.CustomFileNo == customFileNumber && a.Tenant == tenant
                    select a).ToList();
        }

        public List<CustomsRequestsSheet> GetEntityRequestsSheets(string objectTableId, string entityId, int tenant)
        {

            return (from a in context.CustomsRequestsSheets.Include("User").Include("User.Contact").Include("InterfaceManagement").Include("CustomsRequestsSheetStatus")
                    where a.ObjectTableId1 == objectTableId && a.EntityId1 == entityId && a.Tenant == tenant
                    select a).ToList();
        }

        public void GetWeeklyStatistic(int tenant, out int analyzefailed, out int sentFailed, out int answererror , out int total , out DateTime LastRequestFromDCA )
        {
            LastRequestFromDCA = DateTime.MinValue;

            //select count(*) from CUSTOMSREQUESTSSHEETS where requeststatuscode = '25' and interfacetypecode <> '9000'-- analyze failed

            //select count(*) from CUSTOMSREQUESTSSHEETS where requeststatuscode = '15'-- Sent Failed

            //select count(*) from CUSTOMSREQUESTSSHEETS where requeststatuscode = '22'-- Answer error


            total =analyzefailed = sentFailed = answererror = -1;
            var weekAgo = DateTime.Now.Date.AddDays(-7);
            var monthAgo = DateTime.Now.Date.AddMonths(-1);
            var qReqSheet =
                (from a in context.CustomsRequestsSheets
                 where
                 a.Tenant == tenant &&
                 a.RequestCreateDate >= weekAgo
                 select a);

            var qLastRequestFromDCA_monthAgo =
                (from a in context.CustomsRequestsSheets
                 where
                 a.Tenant == tenant &&
                 a.RequestCreateDate >= monthAgo
                 where a.IsDCA==true
                 orderby a.RequestCreateDate  descending
                 select a
                 );



            var qq = (
                 from a in qReqSheet
                 group a by 1 into gOpenLastWeek
                 select new
                 {
                     analyzefailed = gOpenLastWeek.Count(r => r.RequestStatusCode == "25" && r.InterfaceTypeCode != "9000"),
                     SentFailed = gOpenLastWeek.Count(r => r.RequestStatusCode == "15"),
                     answererror = gOpenLastWeek.Count(r => r.RequestStatusCode == "22"),
                     total = gOpenLastWeek.Count(),
                     LastRequestFromDCA = qLastRequestFromDCA_monthAgo.FirstOrDefault()
                 }
                 ).FirstOrDefault();
            if (qq != null)
            {
                analyzefailed = qq.analyzefailed;
                sentFailed = qq.SentFailed;
                answererror = qq.answererror;
                total = qq.total;
                if (qq.LastRequestFromDCA == null)
                {
                    LastRequestFromDCA = monthAgo;
                }
                else
                {
                    LastRequestFromDCA = qq.LastRequestFromDCA.RequestCreateDate.GetValueOrDefault();
                }
            }
        }


    }

}
   