//using Logitude.CRM.BL.EntityPMs;
//using Logitude.CRM.BL.EntityQueryServices;
//using Logitude.CRM.BL.EntityUpdateServices;
//using Logitude.CRM.Data;
//using Logitude.CRM.Data.EntityListQueryServices;
//using Logitude.CRM.Data.EntityLists;
//using Logitude.CRM.Data.Repsitories;
//using Simplog.Server.Infrastructure;
//using Simplog.Server.Infrastructure.DataContracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel.DomainServices.Server;
//using System.Web;
//using WebFreight.Web.Helpers;
//using WebFreight.Web.Security;

//namespace WebFreight.Web.CRMModel.DomainServices
//{
//    public partial class CRMDomainService
//    {



//        private QuestionnaireQuestionQueryService questionnaireQuestionQuery;
      

//        public QuestionnaireQuestionPM GetSingleQuestionnaireQuestionPM(string questioneerid, int version , int questionnumber ,  int tenant)
//        {
//            crmContext = CRMContext.GetContext(tenant);
//            questionnaireQuestionQuery = new QuestionnaireQuestionQueryService(crmContext);
//            QuestionnaireQuestionPM entityPM = questionnaireQuestionQuery.GetSingle(questioneerid , version , questionnumber , false, false);
//            return entityPM;
//        }




       
//        public QuestionnaireQuestionList GetSingleQuestionnaireQuestionList(string questioneerid, int version , int questionnumber, int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            SecurityUtility.CheckContactFeature("QuestionnaireQuestion", "READ", tenant);

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(tenant);
//            }

//            QuestionnaireQuestionListQueryService listService = new QuestionnaireQuestionListQueryService(crmContext);
//            return listService.GetSingle(questioneerid , version , questionnumber);
//        }



     

//        public List<QuestionnaireQuestionList> GetSQuestionnaireQuestionLists(int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            SecurityUtility.CheckContactFeature("QuestionnaireQuestion", "READ", tenant);

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(tenant);
//            }

//            QuestionnaireQuestionListQueryService listService = new QuestionnaireQuestionListQueryService(crmContext);
//            return listService.GetList(tenant);
//        }


      
    
//        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
//        public List<QuestionnaireQuestionList> GetQuestionnaireQuestionsFilters(byte[] xmlFilters, int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            SecurityUtility.CheckContactFeature("QuestionnaireQuestion", "READ", tenant);

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(tenant);
//            }

//            QuestionnaireQuestionListQueryService listService = new QuestionnaireQuestionListQueryService(crmContext);
//            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
//            return listService.GetList(queryOperations, tenant);

//        }


    

//        public int GetQuestionnaireQuestionFiltersCount(byte[] xmlFilters, int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            SecurityUtility.CheckContactFeature("QuestionnaireQuestion", "READ", tenant);

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(tenant);
//            }

//            QuestionnaireQuestionListQueryService queryService = new QuestionnaireQuestionListQueryService(crmContext);
//            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
//            return queryService.GetListCount(queryOperations, tenant);
//        }



       

     
//        public void InsertQuestionnaireQuestion(QuestionnaireQuestionPM entityPM)
//        {
//          //  SecurityUtility.CheckContactFeature("QuestionnaireQuestion", "NEW", entityPM.Tenant);

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(entityPM.Tenant);
//            }

//            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
//            QuestionnaireQuestionUpdateService service = new QuestionnaireQuestionUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
//            service.Update(entityPM, true);
//        }






        
//        public void UpdateQuestionnaireQuestion(QuestionnaireQuestionPM entityPM)
//        {
//            SecurityUtility.CheckContactFeature("QuestionnaireQuestion", "UPDATE", entityPM.Tenant);

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(entityPM.Tenant);
//            }
//            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
//            QuestionnaireQuestionUpdateService service = new QuestionnaireQuestionUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
//            service.Update(entityPM, true);
//        }


//        public IQueryable<QuestionnaireQuestionPM> GetSQuestionnaireQuestionPMsByQuestioneerid(string questioneerid, int version,int tenant)
//        {
//            //SecurityUtility.AuthenticationOnTenant(tenant);
//           // SecurityUtility.CheckContactFeature("QuestionnaireQuestion", "READ", tenant);

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(tenant);
//            }

//            QuestionnaireQuestionQueryService listService = new QuestionnaireQuestionQueryService(crmContext);
//            return listService.GetQuestionnaireQuestionByQuestioneerid(questioneerid, version, tenant);
//        }


//        public void DeleteQuestionnaireQuestion(QuestionnaireQuestionPM questionnaireQuestionPM)
//        {

//            if (crmContext == null)
//            {
//                crmContext = CRMContext.GetContext(questionnaireQuestionPM.Tenant);
//            }

//            questionnaireQuestionPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
//            QuestionnaireQuestionUpdateService service = new QuestionnaireQuestionUpdateService(crmContext, new Dictionary<string, IContext>(), questionnaireQuestionPM.Tenant);
//            service.Update(questionnaireQuestionPM, true);

//        }
//    }
//}




