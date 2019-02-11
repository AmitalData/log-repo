using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.Helpers;
using System.Transactions;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;

namespace WebFreight.Web.Controllers.InfrastructureModel.Generated.PMControllers
{


    public partial class DWObjectFieldsController : ApiController
    {


        public HttpResponseMessage GetDWObjectFieldsByDWTableId(string DWOTId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectField", "READ", authToken.Tenant);
                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenant(0, DWOTId).OrderBy(a => a.Name).ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, dWObjectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetDWObjectFieldsByDWTableIdGroupedByCategory(string DWOTId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectField", "READ", authToken.Tenant);
                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                //var Category1Group = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenant(0, DWOTId).GroupBy(a => a.Category1);
                //var Category2Group = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenant(0, DWOTId).GroupBy(a => a.Category2);
                var CategoryGroup = dWObjectFieldQuery.GetDWObjectFieldPMsByDWObjectTabelAndTenantGroupedByCategory(0, DWOTId).GroupBy(a => a.Category);

                List<DWFieldsGroup> MyGroups = new List<DWFieldsGroup>();
                foreach (var item in CategoryGroup)
                {
                    var MyKey = MyGroups.Where(a => a.Key == item.Key).FirstOrDefault();
                    if (MyKey == null)
                    {
                        var MyGroup = new DWFieldsGroup();
                        MyGroup.Key = item.Key;
                        var FirstItem = item.Select(a => a).FirstOrDefault();
                        if (FirstItem != null)
                        {
                            MyGroup.Index = FirstItem.CategoryIndex;
                        }
                       
                        MyGroup.FieldsList = item.Select(a => a).OrderBy(a => a.Name).ToList();

                        if (MyGroup.FieldsList != null && MyGroup.FieldsList.Count > 0)
                        {
                            MyGroups.Add(MyGroup);
                        }

                    }
                }
                MyGroups = MyGroups.OrderBy(a => a.Index).ToList();
                //foreach (var item in Category1Group)
                //{
                //    var MyKey = MyGroups.Where(a => a.Key == item.Key).FirstOrDefault();
                //    if (MyKey == null)
                //    {
                //        var MyGroup = new DWFieldsGroup();
                //        MyGroup.Key = item.Key;
                //        if (item.Key == null)
                //        {
                //            MyGroup.FieldsList = item.Where(a => a.Category2 == null).ToList();
                //        }
                //        else
                //        {
                //            MyGroup.FieldsList = item.Select(a => a).ToList();
                //        }
                //        if (MyGroup.FieldsList != null && MyGroup.FieldsList.Count > 0)
                //        {
                //            MyGroups.Add(MyGroup);
                //        }

                //    }
                //    else
                //    {
                //        //MyKey.FieldsList = MyKey.FieldsList.Concat(item.Select(a => a).ToList()).ToList();
                //        if (MyKey.Key == null)
                //        {
                //            MyKey.FieldsList = MyKey.FieldsList.Concat(item.Where(a => a.Category2 == null && (MyKey.FieldsList.Where(b => b.Id != a.Id).FirstOrDefault() == null)).ToList()).ToList();
                //        }
                //        else
                //        {
                //            MyKey.FieldsList = MyKey.FieldsList.Concat(item.Select(a => a).ToList()).ToList();
                //        }
                //    }
                //}
                //foreach (var item in Category2Group)
                //{
                //    var MyKey = MyGroups.Where(a => a.Key == item.Key).FirstOrDefault();
                //    if (MyKey == null)
                //    {
                //        var MyGroup = new DWFieldsGroup();
                //        MyGroup.Key = item.Key;
                //        if (item.Key == null)
                //        {
                //            MyGroup.FieldsList = item.Where(a => a.Category1 == null).ToList();
                //        }
                //        else
                //        {
                //            MyGroup.FieldsList = item.Select(a => a).ToList();
                //        }
                //        if (MyGroup.FieldsList != null && MyGroup.FieldsList.Count > 0)
                //        {
                //            MyGroups.Add(MyGroup);
                //        }
                //    }
                //    else
                //    {
                //        if (MyKey.Key == null)
                //        {
                //            MyKey.FieldsList = MyKey.FieldsList.Concat(item.Where(a => a.Category1 == null && (MyKey.FieldsList.Where(b => b.Id != a.Id).FirstOrDefault() == null)).ToList()).ToList();
                //        }
                //        else
                //        {
                //            MyKey.FieldsList = MyKey.FieldsList.Concat(item.Select(a => a).ToList()).ToList();
                //        }
                //    }
                //}
                //var NULLGroup = MyGroups.Where(a => a.Key == null).FirstOrDefault();
                //if (NULLGroup != null && NULLGroup.FieldsList.Count == 0)
                //{
                //    MyGroups.Remove(NULLGroup);
                //}

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, MyGroups);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage getDWObjectFieldsWithChildrenByDWTableId(string DWOTId)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                //SecurityUtility.CheckContactFeature("DWObjectField", "READ", authToken.Tenant);
                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldWithChildrenFieldsPMsByDWObjectTabelAndTenant(0, DWOTId).ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, dWObjectFieldPM);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }



    }

    public class DWFieldsGroup
    {
        public string Key { get; set; }
        public int Index { get; set; }
        public List<DWObjectFieldPM> FieldsList { get; set; }
    }
}
