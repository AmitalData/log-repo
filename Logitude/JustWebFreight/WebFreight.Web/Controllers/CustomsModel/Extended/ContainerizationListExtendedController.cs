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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using System.Transactions;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.BL;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.Messaging.Customs;
using static Logitude.Customs.Data.Repsitories.ContainerizationRepository;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public partial class ContainerizationListExtendedController : ApiController
    {
        public HttpResponseMessage CreateContainerizations(ContainerizationPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                var loggedUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
                ICustomContext customContext = CustomContext.GetContext(tenant);
                var CreateContainerizationBL = new CreateContainerization();
                List<ContainerizationDetails> ContainerizationList = CreateContainerizationBL.CreateContainerizations(entityPM);
                if (ContainerizationList != null)
                {


                    if (ContainerizationList.Count == 1)
                    {

                        SendContainerization(ContainerizationList[0].Tenant, SendRequestVIA.WebServiceInteractive, ContainerizationList[0].Id, loggedUserId);
                    }
                    else
                    {
                        if (ContainerizationList.Count == 0)
                        {
                            // return "Client List Is Empty (Count==0)";
                        }

                        string clientCode = "";
                        for (int i = 0; i < ContainerizationList.Count; i++)
                        {
                            SendContainerization(ContainerizationList[i].Tenant, SendRequestVIA.WebServiceBatch, ContainerizationList[i].Id, loggedUserId);
                        }
                    }
                }
                var listSendObj = new sendObj();
                listSendObj.list = ContainerizationList;
               
                return Request.CreateResponse(HttpStatusCode.OK, listSendObj);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public  class sendObj
        {
            public List<ContainerizationDetails> list { get; set; }
        }
    
       public void SendContainerization(int tenent, SendRequestVIA sendRequestVIA, string id, string LoggedUserId)
       {

            try
            {
                GenericRequestParams requestParams = new GenericRequestParams();
                requestParams.Tenant = tenent;
                requestParams.RequestVIA = sendRequestVIA;// event.RequestVIA;
                requestParams.ForcePersonalSign = false;//event.ForcePersonalSign;
                requestParams.LoggingEnabled = true;
                requestParams.LoggingEntityId = id;
                requestParams.LoggingUserId = LoggedUserId;
                requestParams.RequestName = "המכלה";
                requestParams.ResponseName = "המכלה תשובה";
                requestParams.MainInterfaceCode = "2450";
                requestParams.InterfaceTypeCode = "2450";
                requestParams.LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Containerization");


                if (sendRequestVIA == SendRequestVIA.WebServiceInteractive)
                {
                    var myRequestMessagingService = new SaveCC_MSG2450_ContainerizationMessageMessagingService();
                    var resData = myRequestMessagingService.Send(requestParams);
                }
                else
                {
                    using (var scopeNewCRS = TransactionFactory.GetNewTransaction()) { 
                        SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams, false);
                        scopeNewCRS.Complete();
                    }
                }
            }
            catch (Exception ex)
            {

            }
       
       }
       public HttpResponseMessage GetByFilters([FromUri] ApiQueryFilters filters)
       {
           try
           {
               string token = HttpContext.Current.Request.Headers["Token"];
               AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
               SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
               SecurityUtility.CheckContactFeature("Customs.Containerization", "READ", authToken.Tenant);
               int tenant = authToken.Tenant;
               if (filters.Tenant != null)
                   tenant = tenant;
       
               QueryOperations queryOperations = new QueryOperations()
               {
                   ObjectTableName = "Customs.Declaration",
                   PageIndex = filters.PageIndex,
                   PageSize = filters.PageSize,
                   QuerySection = "Customs.Declaration",
                   SortByColumnName = filters.SortBy,
                   SortDirectin = filters.SortDirection,
                   GetAll = filters.GetAll,
               };
       
               List<ObjectField> DeclarationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.Declaration", tenant);
               List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
       
               for (int i = 1; i <= 10; i++)
               {
                   object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                   object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                   object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                   object filterValue2 = null;
       
                   if (filterNameProp != null)
                   {
                       string filterName = filterNameProp.ToString();
                       string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
       
                       ObjectField field = DeclarationObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                       if (field != null)
                       {
                           string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                           object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
       
                           string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                           object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
       
                           queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                       }
                       else
                           queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                   }
               }
               if (!string.IsNullOrEmpty(filters.AdditionalFilters))
               {
                   JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                   var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);
       
                   foreach (QueryFilterItem filter in filters_list)
                   {
                       ObjectField field = DeclarationObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                       if (field != null)
                       {
                           string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                           object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);
       
                           string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                           object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);
       
                           queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                       }
                       else
                       {
                           queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                       }
                   }
               }
               ICustomContext MyContext = CustomContext.GetContext(tenant);
               DeclarationListQueryService declarationListQueryService = new DeclarationListQueryService(MyContext);
               List<DeclarationList> entityLists = declarationListQueryService.GetListForContainerization(queryOperations, tenant);
       
               ServiceResponse response = new ServiceResponse();
               if (filters.GetCount)
               {
                   int count = declarationListQueryService.GetListCount(queryOperations, tenant);
                   response.Count = count;
               }
       
               response.Result = entityLists;
               HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
               return reponseMessage;
       
           }
           catch (Exception ex)
           {
               return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
           }
       }
     }
}