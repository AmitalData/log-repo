using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

using System.ServiceModel.DomainServices.Server;
using Simplog.Server.Infrastructure;
using System.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsCollateralPM GetSingleCustomsCollateralPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsCollateralQuery = new CustomsCollateralQueryService(customContext);
            paymentOrderQueryService = new PaymentOrderQueryService(customContext);
            CustomsCollateralPM customsCollateral = customsCollateralQuery.GetSingle(id, true, false);
            CustomsCollateralsAnswerPM answer= customsCollateral.CustomsCollateralsAnswers.Where(d => d.PaymentOrderId != null).FirstOrDefault();
            if (answer != null)
            {
              PaymentOrderPM paymentOrder=  paymentOrderQueryService.GetSingle(answer.PaymentOrderId, false, false);
              customsCollateral.PaymentNumber = paymentOrder.PaymentNumber;
              customsCollateral.PaymentOrderId = paymentOrder.Id;
            }
            return customsCollateral;
        }

        public CustomsCollateralList GetSingleCustomsCollateralList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.CustomsCollateral", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsCollateralListQueryService listService = new CustomsCollateralListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsCollateralList> GetCustomsCollateralLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsCollateral", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsCollateralListQueryService listService = new CustomsCollateralListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsCollateralList> GetCustomsCollateralFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsCollateral", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsCollateralListQueryService listService = new CustomsCollateralListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsCollateralFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsCollateral", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsCollateralListQueryService queryService = new CustomsCollateralListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCustomsCollateral(CustomsCollateralPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsCollateral", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            CustomsCollateralUpdateService service = new CustomsCollateralUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (CustomsCollateralsAnswerPM answer in entityPm.CustomsCollateralsAnswers)
            {
                answer.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
            foreach (CustomsCollateralsConditionPM condition in entityPm.CustomsCollateralsConditions)
            {
                condition.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }

            service.Update(entityPm, true);

        }

        public void UpdateCustomsCollateral(CustomsCollateralPM currententityPm)
        {
            ////SecurityUtility.CheckContactFeature("Customs.CustomsCollateral", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsCollateralUpdateService service = new CustomsCollateralUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetCustomsCollateralsAnswerChangeSet(currententityPm);
            SetCustomsCollateralsConditionChangeSet(currententityPm);
           
            service.Update(currententityPm, true);

        }

        private void SetCustomsCollateralsAnswerChangeSet(CustomsCollateralPM currententityPm)
        {
            List<CustomsCollateralsAnswerPM> customsCollateralsAnswerchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.CustomsCollateralsAnswers).Cast<CustomsCollateralsAnswerPM>().ToList();
            foreach (CustomsCollateralsAnswerPM itemPM in customsCollateralsAnswerchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            CustomsCollateralsAnswerPM currentItemPM = currententityPm.CustomsCollateralsAnswers.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            SetCollateralRequestFileConditionChnagesSet(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            CustomsCollateralsAnswerPM currentItemPM = currententityPm.CustomsCollateralsAnswers.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            SetCollateralRequestFileConditionChnagesSet(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            CustomsCollateralsAnswerPM currentItemPM = new CustomsCollateralsAnswerPM() { ChangeSetOp = ChangeSetOperation.Delete, CustomsCollateralId = itemPM.CustomsCollateralId, LineNumber = itemPM.LineNumber };
                            currententityPm.DeletedCustomsCollateralsAnswers.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            CustomsCollateralsAnswerPM currentItemPM = currententityPm.CustomsCollateralsAnswers.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetCollateralRequestFileConditionChnagesSet(CustomsCollateralsAnswerPM currentEntityPM)
        {
            List<CollateralsRequestFileCondPM> collateralsRequestFileConditionChangeSet = ChangeSet.GetAssociatedChanges(currentEntityPM, d => d.CollateralsRequestFileConds).Cast<CollateralsRequestFileCondPM>().ToList();
            foreach (CollateralsRequestFileCondPM itemPM in collateralsRequestFileConditionChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            CollateralsRequestFileCondPM currentItemPM = currentEntityPM.CollateralsRequestFileConds.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.ConditionCode == itemPM.ConditionCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            CollateralsRequestFileCondPM currentItemPM = currentEntityPM.CollateralsRequestFileConds.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.ConditionCode == itemPM.ConditionCode && d.LineNumber == itemPM.LineNumber).FirstOrDefault();
                         
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            CollateralsRequestFileCondPM currentItemPM = new CollateralsRequestFileCondPM() { ChangeSetOp = ChangeSetOperation.Delete, CustomsCollateralId = itemPM.CustomsCollateralId, ConditionCode = itemPM.ConditionCode, LineNumber = itemPM.LineNumber };
                            currentEntityPM.DeletedCollateralsRequestFileConds.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            CollateralsRequestFileCondPM currentItemPM = currentEntityPM.CollateralsRequestFileConds.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.LineNumber == itemPM.LineNumber && d.ConditionCode == itemPM.ConditionCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetCustomsCollateralsConditionChangeSet(CustomsCollateralPM currententityPm)
        {
            List<CustomsCollateralsConditionPM> customsCollateralsConditionchangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.CustomsCollateralsConditions).Cast<CustomsCollateralsConditionPM>().ToList();
            foreach (CustomsCollateralsConditionPM itemPM in customsCollateralsConditionchangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            CustomsCollateralsConditionPM currentItemPM = currententityPm.CustomsCollateralsConditions.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.ConditionCode == itemPM.ConditionCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            CustomsCollateralsConditionPM currentItemPM = currententityPm.CustomsCollateralsConditions.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.ConditionCode == itemPM.ConditionCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            CustomsCollateralsConditionPM currentItemPM = new CustomsCollateralsConditionPM() { ChangeSetOp = ChangeSetOperation.Delete, CustomsCollateralId = itemPM.CustomsCollateralId, ConditionCode = itemPM.ConditionCode };
                            currententityPm.DeletedCustomsCollateralsConditions.Add(currentItemPM);
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Delete;
                            break;
                        }
                    default:
                        {
                            CustomsCollateralsConditionPM currentItemPM = currententityPm.CustomsCollateralsConditions.Where(d => d.CustomsCollateralId == itemPM.CustomsCollateralId && d.ConditionCode == itemPM.ConditionCode).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

 


        public void UpdateCustomsCollateralList(CustomsCollateralList list)
        {

        }

        public void DeleteCustomsCollateral(CustomsCollateralPM entityPM)
        {
            //SecurityUtility.CheckContactFeature("Customs.CustomsCollateral", "UPDATE", entityPM.Tenant);
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPM.Tenant);
            }
           
            CustomsCollateralUpdateService service = new CustomsCollateralUpdateService(customContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

            List<CustomsCollateralsAnswerPM> customsCollateralsAnswerPMchangeset = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomsCollateralsAnswers).Cast<CustomsCollateralsAnswerPM>().ToList();
            foreach (CustomsCollateralsAnswerPM answer in customsCollateralsAnswerPMchangeset)
            {
                CustomsCollateralsAnswerPM deletedAnswer = new CustomsCollateralsAnswerPM()
                {
                    ChangeSetOp = ChangeSetOperation.Delete,
                    CustomsCollateralId = answer.CustomsCollateralId,
                    Tenant = answer.Tenant,
                    LineNumber = answer.LineNumber,
                };
                entityPM.DeletedCustomsCollateralsAnswers.Add(deletedAnswer);
            }

            List<CustomsCollateralsConditionPM> customsCollateralsCoditionchangeset = ChangeSet.GetAssociatedChanges(entityPM, d => d.CustomsCollateralsConditions).Cast<CustomsCollateralsConditionPM>().ToList();
            foreach (CustomsCollateralsConditionPM condition in customsCollateralsCoditionchangeset)
            {
                CustomsCollateralsConditionPM deletedCondition = new CustomsCollateralsConditionPM()
                {
                    ChangeSetOp = ChangeSetOperation.Delete,
                    CustomsCollateralId = condition.CustomsCollateralId,
                    Tenant = condition.Tenant,
                    ConditionCode=condition.ConditionCode,
                };
                entityPM.DeletedCustomsCollateralsConditions.Add(deletedCondition);
            }


            service.Update(entityPM, true);
        }


    }
}