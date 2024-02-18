//using Logitude.CRM.BL.EntityPMs;
//using Logitude.CRM.BL.EntityQueryServices;
//using Logitude.CRM.BL.EntityUpdateServices;
//using Logitude.CRM.Data;
//using Simplog.Data.CommonDataModel.EntityPOCOs;
//using Simplog.Data.CommonDataModel.Repositories;
//using Simplog.Data.InfrastructureModel.EntityPOCOs;
//using Simplog.Data.InfrastructureModel.Repositories;
//using Simplog.Server.Infrastructure;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Security;

//namespace WebFreight.Web.App_Code.AngularJS_App_Code
//{
//    public class EmployeeGroupsController : ApiController
//    {

//        public HttpResponseMessage GetSingle(string id)
//        {
//            try
//            {
//                string token = HttpContext.Current.Request.Headers["Token"];
//                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", authToken.Tenant);

//                ICRMContext MyContext = CRMContext.GetContext(1);
//                EmployeeGroupQueryService employeeGroupQuery = new EmployeeGroupQueryService(MyContext);

//                EmployeeGroupPM employeeGroupPM = employeeGroupQuery.GetSingle(id, false, false);
//                return Request.CreateResponse(HttpStatusCode.OK, employeeGroupPM);
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
//            }
//        }


//        public HttpResponseMessage Put(EmployeeGroupPM entityPM)
//        {
//            try
//            {
//                string token = HttpContext.Current.Request.Headers["Token"];
//                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", authToken.Tenant);
                
//                ICRMContext MyContext = CRMContext.GetContext(entityPM.Tenant);
//                EmployeeGroupUpdateService service = new EmployeeGroupUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
//                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

//                //List<EmployeeGroupLinePM> EmployeeGroupLinesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.EmployeeGroupLines).Cast<EmployeeGroupLinePM>().ToList();
//                //foreach (EmployeeGroupLinePM EmployeeGroupLine in EmployeeGroupLinesChangeSet)
//                //{
//                //    entityPM.EmployeeGroupLines.Where(d => d.Id == EmployeeGroupLine.Id).FirstOrDefault().ChangeSetOp = ChangeSetOperation.Insert;
//                //}

//                service.Update(entityPM, true);

//                ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
//                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("EmployeeGroup", 0, true);
//                string email = HttpContext.Current.User.Identity.Name;
//                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
//                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
//                if (loggedContact != null)
//                {
//                    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
//                }

//                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
//            }

//            catch (Exception ex)
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
//            }
//        }


//        public HttpResponseMessage Post(EmployeeGroupPM entityPM)
//        {
//            try
//            {
//                string token = HttpContext.Current.Request.Headers["Token"];
//                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
//                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
//                SecurityUtility.CheckContactFeature("EmployeeGroup", "READ", authToken.Tenant);

//                ICRMContext MyContext = CRMContext.GetContext(entityPM.Tenant);
//                EmployeeGroupUpdateService service = new EmployeeGroupUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
//                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

//                //SetEmployeeGroupLineChangeSet(entityPM);
//                service.Update(entityPM, true);

//                ObjectTabelRepository objectTabelRepository = new ObjectTabelRepository(entityPM.Tenant);
//                ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("EmployeeGroup", 0, true);
//                string email = HttpContext.Current.User.Identity.Name;
//                ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
//                Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
//                if (loggedContact != null)
//                {
//                    ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
//                }
            
//                return Request.CreateResponse(HttpStatusCode.OK, entityPM);
//            }

//            catch (Exception ex)
//            {
//                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
//            }
//        }

//        // DELETE api/<controller>/5
//        public void Delete(int id)
//        {
//        }
//    }
//}