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




                var factFields = new DWObjectFieldAdditionalFactService(new DWObjectFieldAdditionalFactArgs() { FactTableCode = DWOTId,Tenant = authToken.Tenant, GroupedByCategory = true }).DWObjectFieldPMs;
                var CategoryGroup = factFields.GroupBy(a => a.Category);
                var FactGroups = factFields.GroupBy(a => a.DWObjectTableCode);
                
                DWHSettingRepository dWHSettingRepository = new DWHSettingRepository(authToken.Tenant);  
                var isParentTenant =   dWHSettingRepository.IsParentTenant(authToken.Tenant);
                List<ObjectFieldPM> objectFieldPMs = new List<ObjectFieldPM>();
                if (!isParentTenant)
                {
                    objectFieldPMs = GetCustomObjectFields(DWOTId, authToken.Tenant);
                }

                var ParentFactIndex = 1;
                List<DWFieldsGroup> MyGroups = new List<DWFieldsGroup>(); 
                List<DWFactGroup> FactFieldsGroups = new List<DWFactGroup>();

                 
                foreach (var fact in FactGroups)
                {
                    DWFactGroup DWFactGroup = new DWFactGroup();
                    DWFactGroup.Key = fact.Key.Replace("_"," ");
                    DWFactGroup.FieldsGroupList = new List<DWFieldsGroup>();
                    DWFactGroup.Index = fact.Key == DWOTId? 1: ParentFactIndex + 1;
                    foreach (var item in CategoryGroup)
                    {
                        var factfields = item.Where(a => a.DWObjectTableCode == fact.Key).ToList();

                        if(factfields != null && factfields.Count !=0)
                        {
                            var MyGroup = new DWFieldsGroup();
                            MyGroup.Key = item.Key;
                            var FirstItem = factfields.Select(a => a).FirstOrDefault();
                            MyGroup.Index = FirstItem.CategoryIndex;
                            MyGroup.Fact = FirstItem.DWObjectTableCode;
                            MyGroup.FieldsList = factfields.Select(a => a).OrderBy(a => a.Name).ToList();

                            if (MyGroup.FieldsList != null && MyGroup.FieldsList.Count > 0)
                            {
                                if (MyGroup.Key == "Custom Fields" || MyGroup.Key == "CustomFields")
                                {
                                    ResolveDWCustomObjectFields(objectFieldPMs, MyGroup, authToken.Tenant);

                                    if (MyGroup.FieldsList.Where(d => d.DisplayInQueryBuilder).Any())
                                    {
                                        MyGroups.Add(MyGroup);
                                    }
                                }
                                else MyGroups.Add(MyGroup);

                                DWFactGroup.FieldsGroupList.Add(MyGroup);
                            }
                             
                        }
                    }

                    FactFieldsGroups.Add(DWFactGroup);
                    ParentFactIndex = ++ParentFactIndex;
                }
                  
                FactFieldsGroups = FactFieldsGroups.OrderBy(a => a.Index).ToList();
                MyGroups = RemoveFactInvoiceCustomFieldsCategory(DWOTId, MyGroups);
                 



                PerformanceLogger.AddServerExecutionTimeHeader(logKey); 

                return Request.CreateResponse(HttpStatusCode.OK, FactFieldsGroups);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private List<DWFieldsGroup> RemoveFactInvoiceCustomFieldsCategory(string factCode, List<DWFieldsGroup> MyGroups)
        {
            List<DWFieldsGroup> FactGroups = MyGroups;
            if (factCode == "Fact_Invoices")
            {
                DWFieldsGroup customFieldsCategroy = FactGroups.FirstOrDefault(categroy => categroy.Key == "Custom Fields");
                if (customFieldsCategroy != null)
                {
                    FactGroups.Remove(customFieldsCategroy);
                }
            }
            return FactGroups;
        }

        private List<ObjectFieldPM> GetCustomObjectFields(string DWOTId, int tenant)
        {
            List<ObjectFieldPM> objectFieldPMs = new List<ObjectFieldPM>();
            ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(tenant);
            DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(tenant);
            DWObjectTablePM dWObjectTablePM = dWObjectTableQuery.GetSinglePM(DWOTId, tenant);
            if (dWObjectTablePM != null && !string.IsNullOrEmpty(dWObjectTablePM.ObjectTableName)) objectFieldPMs = objectFieldQuery.GetCustomObjectFieldsByTenantAndObjectTable(tenant, dWObjectTablePM.ObjectTableName);

            //if (dWObjectTablePM != null && dWObjectTablePM.RecordType == "Master")
            //    objectFieldPMs.AddRange(objectFieldQuery.GetCustomObjectFieldsByTenantAndObjectTable(tenant, "Shipment"));

            return objectFieldPMs;
        }
        private void ResolveDWCustomObjectFields(List<ObjectFieldPM> objectFieldPMs, DWFieldsGroup MyGroup, int tenant)
        {
            if(objectFieldPMs!=null && objectFieldPMs.Count > 0) {
                foreach (var field in MyGroup.FieldsList.Where(d => d.IsCustom).ToList())
                {
                    ObjectFieldPM objectFieldPM = objectFieldPMs.Where(d => d.FieldName == field.Name).FirstOrDefault();
                    if (objectFieldPM != null)
                    {
                        if (objectFieldPM.DataTypeCode != "LookUp")
                        {
                            field.DisplayName = objectFieldPM.FullNameTextCodeDefaultText;//TranslateTextsClass.Translate(objectFieldPM.FullNameTextCodeCode, tenant);
                            field.DataTypeCode = objectFieldPM.DataTypeCode;
                            if (field.DataTypeCode == "Date")
                            {
                                field.DataTypeCode = "Dimension";
                                field.DimensionTableCode = "DIM_Dates";
                            }
                            else if (field.DataTypeCode == "PickList")
                            {
                                field.DataTypeCode = "Dimension";
                                field.DimensionTableCode = "DIM_CustomPickLists";
                                field.HideTree = true;
                                field.CustomPickListCode = objectFieldPM.CustomPickListCode;
                            }



                            field.DisplayInQueryBuilder = true;
                        }
                    }
                }
            }
        }

        public HttpResponseMessage getDWObjectFieldsWithChildren()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                DWObjectFieldQuery dWObjectFieldQuery = new DWObjectFieldQuery(authToken.Tenant);
                List<DWObjectFieldPM> dWObjectFieldPM = dWObjectFieldQuery.GetDWObjectFieldWithChildrenFieldsPMsByTenant(0).ToList();

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, dWObjectFieldPM);

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
        public string Fact { get; set; }
        public List<DWObjectFieldPM> FieldsList { get; set; }
    } 
    public class DWFactGroup
    {
        public string Key { get; set; }
        public int Index { get; set; }
        public List<DWFieldsGroup> FieldsGroupList { get; set; }
    }

}
