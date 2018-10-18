//using Simplog.Data.InfrastructureModel.EntityPOCOs;
//using Simplog.Data.InfrastructureModel.Repositories;
//using Simplog.Server.Infrastructure.DataContracts;
//using Simplog.Server.Infrastructure.Helpers;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Serialization;

//using Simplog.Server.Infrastructure;
//using Logitude.Server.Tools.Helpers;
//using Logitude.Server.Tools.Interfaces;
//using Logitude.Server.Tools;

//using System.Web;
//using Simplog.Data.CommonDataModel.Repositories;
//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using Logitude.BL.Helpers;
//using System.Transactions;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Simplog.Data.CommonDataModel;
//using Logitude.BL.CommonDataModel;
//using Logitude.BL.CommonDataModel.EntityLists;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.CommonDataModel.Tools.EntityService;


//namespace Logitude.Services.Common.Controllers
//{
//    public partial class BranchesController : ApiController
//    {


//        public HttpResponseMessage GetSingle(string id)
//        {
//            try
//            {
//                string token = HttpContext.Current.Request.Headers["Token"];
//                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                // SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

//                // SecurityUtility.CheckContactFeature("Branch", "READ", authToken.Tenant);
//                BranchQuery branchQuery = new BranchQuery(authToken.Tenant);
//                BranchPM branchPM = branchQuery.GetSinglePM(id, authToken.Tenant);

//                return Request.CreateResponse(HttpStatusCode.OK, branchPM);

//            }
//            catch (Exception ex)
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, "");
//            }

//        }




//        public HttpResponseMessage Post(BranchPM entityPM)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    using (TransactionScope scope = TransactionFactory.GetTransaction())
//                    {
//                        string token = HttpContext.Current.Request.Headers["Token"];
//                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                        //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                        //SecurityUtility.CheckContactFeature("Branch", "NEW", authToken.Tenant);

//                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
//                        BranchService service = new BranchService(MyContext, entityPM.Tenant);
//                        service.Create(entityPM);

//                        //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
//                        // ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Branch", 0, true);
//                        //string email = HttpContext.Current.User.Identity.Name;
//                        // ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
//                        //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
//                        //if (loggedContact != null)
//                        //{
//                        //    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
//                        //}
//                        TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Branch");

//                        scope.Complete();
//                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
//                    }
//                }

//                catch (Exception ex)
//                {
//                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
//                }
//            }
//            else
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, "");
//            }
//        }


//        public HttpResponseMessage Put(BranchPM entityPM)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    using (TransactionScope scope = TransactionFactory.GetTransaction())
//                    {
//                        string token = HttpContext.Current.Request.Headers["Token"];
//                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                        //SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                        //SecurityUtility.CheckContactFeature("Branch", "UPDATE", authToken.Tenant);

//                        string entityName = "Branch" + entityPM.Id + entityPM.Tenant;
//                        string entityPmName = "BranchPM" + entityPM.Id + entityPM.Tenant;
//                        if (CacheManager.CacheWrapper.Get(entityName) != null)
//                        {
//                            CacheManager.CacheWrapper.Invalidate(entityName);
//                        }
//                        if (CacheManager.CacheWrapper.Get(entityPmName) != null)
//                        {
//                            CacheManager.CacheWrapper.Invalidate(entityPmName);
//                        }

//                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
//                        BranchService service = new BranchService(MyContext, entityPM.Tenant);

//                        service.Update(entityPM);

//                        //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
//                        //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Branch", 0, true);
//                        //string email = HttpContext.Current.User.Identity.Name;
//                        //ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
//                        //Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
//                        //if (loggedContact != null)
//                        //{
//                        //   ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
//                        //}

//                        TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Branch");

//                        scope.Complete();
//                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
//                    }
//                }

//                catch (Exception ex)
//                {
//                    return Request.CreateResponse(HttpStatusCode.BadRequest, "");
//                }
//            }
//            else
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, "");
//            }
//        }

//        // DELETE api/<controller>/5
//        public void Delete(int id)
//        {
//        }













//    }
//}
