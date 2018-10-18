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
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {

        private QuestionnaireQueryService questionnaireQuery;
      private QuestionnaireRepository questionnaireRepository;



        public QuestionnairePM GetSingleQuestionnairePM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            questionnaireQuery = new QuestionnaireQueryService(crmContext);
            QuestionnairePM entityPM = questionnaireQuery.GetSingle(id, true, false);
            return entityPM;
        }

        public QuestionnairePM GetSingleQuestionnairePMByVersion(string id, int version, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            questionnaireQuery = new QuestionnaireQueryService(crmContext);
            QuestionnaireQuestionQueryService questionsQuery = new QuestionnaireQuestionQueryService(crmContext);
            QuestionnairePM entityPM = questionnaireQuery.GetSingle(id, false, false);
            entityPM.QuestionnaireQuestions = questionsQuery.GetQuestionnaireQuestionByQuestioneerVersion(id, version, tenant);
            return entityPM;
        }



        public QuestionnaireList GetSingleQuestionnaireList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Questionnaire", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireListQueryService listService = new QuestionnaireListQueryService(crmContext);
            return listService.GetSingle(id);
        }


        public List<QuestionnaireList> GetQuestionnaireLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Questionnaire", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireListQueryService listService = new QuestionnaireListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<QuestionnaireList> GetActiveQuestionnaireLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Questionnaire", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireListQueryService listService = new QuestionnaireListQueryService(crmContext);
            return listService.GetList(tenant).Where(q => q.InActive == false).ToList();
        }


        public void UpdateQuestionnaire(QuestionnairePM entityPM)
        {            
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Questionnaire", "UPDATE", entityPM.Tenant);

            //QuestionnaireKeys keys = new QuestionnaireKeys() { Id = entityPM.Id };
            questionnaireRepository = new QuestionnaireRepository(entityPM.Tenant);
            //Questionnaire entity_Poco = questionnaireRepository.GetSingle(keys);           
            //SecurityUtility.CheckFeatureAccessLevel("Questionnaire", "UPDATE", entity_Poco.Id , null, entityPM.Tenant);
          
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPM.Tenant);
            }

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            QuestionnaireUpdateService service = new QuestionnaireUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);


            SetOpportQuestionnaireQuestionsChangeSet(entityPM);
      

            service.Update(entityPM, true);

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Questionnaire", 0, true);
            ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", entityPM.UpdatedByUserId);
        }

        private void SetOpportQuestionnaireQuestionsChangeSet(QuestionnairePM entityPM)
        {
            List<QuestionnaireQuestionPM> dataChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.QuestionnaireQuestions).Cast<QuestionnaireQuestionPM>().ToList();

            foreach (QuestionnaireQuestionPM itemPM in dataChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            QuestionnaireQuestionPM currentItemPM = entityPM.QuestionnaireQuestions.Where(d => d.QuestioneerId == itemPM.QuestioneerId && d.VersionNumber == itemPM.VersionNumber && d.QuestionNumber == itemPM.QuestionNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            QuestionnaireQuestionPM currentItemPM = entityPM.QuestionnaireQuestions.Where(d => d.QuestioneerId == itemPM.QuestioneerId && d.VersionNumber == itemPM.VersionNumber && d.QuestionNumber == itemPM.QuestionNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            QuestionnaireQuestionPM currentItemPM = new QuestionnaireQuestionPM() { ChangeSetOp = ChangeSetOperation.Delete, QuestioneerId = itemPM.QuestioneerId, VersionNumber = itemPM.VersionNumber, QuestionNumber = itemPM.QuestionNumber };
                            entityPM.DeletedQuestionnaireQuestions.Add(currentItemPM);
                            break;
                        }

                    default:
                        {
                            QuestionnaireQuestionPM currentItemPM = entityPM.QuestionnaireQuestions.Where(d => d.QuestioneerId == itemPM.QuestioneerId && d.VersionNumber == itemPM.VersionNumber && d.QuestionNumber == itemPM.QuestionNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

    
        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<QuestionnaireList> GetQuestionnaireFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Questionnaire", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireListQueryService listService = new QuestionnaireListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }



        public int GetQuestionnaireFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Questionnaire", "READ", tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            QuestionnaireListQueryService queryService = new QuestionnaireListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertQuestionnaire(QuestionnairePM entityPM)
        {
     
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("Questionnaire", "NEW", entityPM.Tenant);

                if (crmContext == null)
                {
                    crmContext = CRMContext.GetContext(entityPM.Tenant);
                }

                if (!entityPM.IsCopy)
                {

                    string Name = crmContext.Questionnaires.Where(c => c.Name == entityPM.Name && c.Tenant == entityPM.Tenant).Max(d => d.Name);
                    if (string.IsNullOrEmpty(Name))
                    {
                        QuestionnaireUpdateService service = new QuestionnaireUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        foreach (QuestionnaireQuestionPM questionnaireQuestionPM in entityPM.QuestionnaireQuestions)
                        {
                            questionnaireQuestionPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        }
                        service.Update(entityPM, true);
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("This Questionnaire Is Already Exist");
              
                       // throw new WebException("This Questionnaire Is Already Exist");
                    }

                }
                else
                {
                    QuestionnaireUpdateService service = new QuestionnaireUpdateService(crmContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    foreach (QuestionnaireQuestionPM questionnaireQuestionPM in entityPM.QuestionnaireQuestions)
                    {
                        questionnaireQuestionPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    }
                    service.Update(entityPM, true);

                }
        }


    }
}