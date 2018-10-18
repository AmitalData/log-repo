using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        private QuestionnaireAnswerQueryService questionnaireAnswerQuery;
        private QuestionnaireAnswerRepository QuestionnaireAnswerRepository;



        public QuestionnaireAnswerPM GetSingleQuestionnaireAnswerPM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            questionnaireAnswerQuery = new QuestionnaireAnswerQueryService(crmContext);
            QuestionnaireAnswerPM entityPM = questionnaireAnswerQuery.GetSingle(id, false, false);
            return entityPM;
        }





        public QuestionnaireAnswerList GetSingleQuestionnaireAnswerList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuestionnaireAnswer", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireAnswerListQueryService listService = new QuestionnaireAnswerListQueryService(crmContext);
            return listService.GetSingle(id);
        }





        public List<QuestionnaireAnswerList> GetSQuestionnaireAnswerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuestionnaireAnswer", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireAnswerListQueryService listService = new QuestionnaireAnswerListQueryService(crmContext);
            return listService.GetList(tenant);
        }




        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<QuestionnaireAnswerList> GetQuestionnaireAnswersFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuestionnaireAnswer", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireAnswerListQueryService listService = new QuestionnaireAnswerListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }




        public int GetQuestionnaireAnswerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuestionnaireAnswer", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireAnswerListQueryService queryService = new QuestionnaireAnswerListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }



        public void InsertQuestionnaireAnswer(QuestionnaireAnswerPM entityPM)
        {

            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("QuestionnaireAnswer", "NEW", entityPM.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }


            QuestionnaireAnswerUpdateService service = new QuestionnaireAnswerUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (QuestionnaireAnswerLinePM questionnaireAnswerLinePM in entityPM.QuestionnaireAnswerLines)
            {
                questionnaireAnswerLinePM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            service.Update(entityPM, true);


        }




        //public void UpdateQuestionnaireAnswer(QuestionnaireAnswerPM entityPM)
        //{
        //    SecurityUtility.CheckContactFeature("QuestionnaireAnswer", "UPDATE", entityPM.Tenant);

        //    if (crmContext == null)
        //    {
        //        crmContext = CRMContext.GetContext(entityPM.Tenant);
        //    }
        //    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
        //    QuestionnaireAnswerUpdateService service = new QuestionnaireAnswerUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
        //    service.Update(entityPM, true);
        //}


        public void UpdateQuestionnaireAnswer(QuestionnaireAnswerPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("QuestionnaireAnswer", "UPDATE", entityPM.Tenant);

            //QuestionnaireAnswerRepository = new QuestionnaireAnswerRepository(entityPM.Tenant);
            //QuestionnaireAnswerKeys keys = new QuestionnaireAnswerKeys() { Id = entityPM.Id };
            //QuestionnaireAnswer entity_Poco = QuestionnaireAnswerRepository.GetSingle(keys);
            //SecurityUtility.CheckFeatureAccessLevel("QuestionnaireAnswer", "UPDATE", entity_Poco.Id, null, entityPM.Tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            QuestionnaireAnswerUpdateService service = new QuestionnaireAnswerUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);


            SetOpportQuestionnaireAnswerLinesChangeSet(entityPM);


            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("QuestionnaireAnswer", 0, true);
            //ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", entityPM.u);
        }


        private void SetOpportQuestionnaireAnswerLinesChangeSet(QuestionnaireAnswerPM entityPM)
        {
            List<QuestionnaireAnswerLinePM> dataChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.QuestionnaireAnswerLines).Cast<QuestionnaireAnswerLinePM>().ToList();

            foreach (QuestionnaireAnswerLinePM itemPM in dataChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            QuestionnaireAnswerLinePM currentItemPM = entityPM.QuestionnaireAnswerLines.Where(d => d.QuestionnaireAnswerId == itemPM.QuestionnaireAnswerId && d.QuestionNumber == itemPM.QuestionNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            QuestionnaireAnswerLinePM currentItemPM = entityPM.QuestionnaireAnswerLines.Where(d => d.QuestionnaireAnswerId == itemPM.QuestionnaireAnswerId && d.QuestionNumber == itemPM.QuestionNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            QuestionnaireAnswerLinePM currentItemPM = new QuestionnaireAnswerLinePM() { ChangeSetOp = ChangeSetOperation.Delete, QuestionnaireAnswerId = itemPM.QuestionnaireAnswerId, QuestionNumber = itemPM.QuestionNumber };
                            entityPM.DeletedQuestionnaireAnswerLines.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            QuestionnaireAnswerLinePM currentItemPM = entityPM.QuestionnaireAnswerLines.Where(d => d.QuestionnaireAnswerId == itemPM.QuestionnaireAnswerId && d.QuestionNumber == itemPM.QuestionNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }


   
        public QuestionnaireAnswerPM GetSQuestionnaireAnswerByQuestioneerIdAndVersion(string questioneerId, int tenant , string tableId , string entityId)
        {
            crmContext = CRMContext.GetContext(tenant);
            questionnaireAnswerQuery = new QuestionnaireAnswerQueryService(crmContext);
            QuestionnaireAnswerPM entityPM = questionnaireAnswerQuery.GetQuestionnaireAnswerPMByquestioneerIdAndversion(questioneerId, tenant, tableId, entityId);
            return entityPM;
   
        }

    }
}





