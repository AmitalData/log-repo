using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.CoreBL;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class RevaluationUpdateService : EntityUpdateService<Revaluation, RevaluationPM, EntityPM>
    {

        protected override void OnCreating(RevaluationPM entityPM, EntityPM entityParentPM)
        {
            //RevaluationListQueryService revaluationListQueryService = new RevaluationListQueryService(this.MainContext as IAccountingContext);
            //List<RevaluationList> revaluations = revaluationListQueryService.GetOpenRevaluationList(entityPM.Tenant);
            //if (revaluations != null && revaluations.Count > 0)
            //{
            //    throw new ApplicationException(TextCodesTranslator.TranslateText("Revaluations.Q.OpenRevaluations", entityPM.Tenant));
            //}

            if (String.IsNullOrEmpty(entityPM.Id) || entityPM.Id == "new") entityPM.Id = IdCounter.GetNumber("Revaluation", entityPM.Tenant);
            if (String.IsNullOrEmpty(entityPM.CreatedByUserId) || entityPM.CreatedByUserId == "new")
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                entityPM.CreatedByUserId = contact.Id;
            }
            entityPM.Status = null;
            if (entityPM.RevaluationNumber == 0) entityPM.RevaluationNumber = CodeCounter.GetNumber("Revaluation.Number", entityPM.Tenant);
            entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            entityPM.SearchFields = entityPM.RevaluationNumber.ToString();

            //Transaction.Current.TransactionCompleted += (sender, e) =>
            //{
            //    //SBQMessageService.CreateBasic<CustomsCommandEnum>(
            //    //    CustomsCommandEnum.CustomsCommandGetCustomRequestWR,
            //    //    tenant,
            //    //    InterfaceTypeCode,
            //    //    MyCustomsRequestsSheetPMId);
            //    using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())
            //    {
            //        var queueSendService = new Logitude.Server.Tools.QueueService.QueueSendService("RevaluationWorkerRole", entityPM.Id, new Server.Tools.QueueService.QueueSendModel() { Tenant = entityPM.Tenant });
            //        //queueSendService.InterfaceTypeCode = interfaceTypeCode;//this.GetType().FullName;
            //        //queueSendService.DebugMode = true;
            //      //  queueSendService.Tenant = entityPM.Tenant;

            //        //queueSendService.ProcessState = (int)CustomsRequestStepEnum.StartRequestParams;
            //        queueSendService.Send();
            //        scope.Complete();
            //    }
            //};
        }

        protected override void OnUpdating(RevaluationPM entityPM)
        {
            if (entityPM.RevaluationNumber == 0)
            {
                entityPM.RevaluationNumber = CodeCounter.GetNumber("Revaluation.Number", entityPM.Tenant);
                entityPM.SearchFields = entityPM.RevaluationNumber.ToString();
            }
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                ///////GetOpenRevaluationList
                //(from a in context.Revaluations
                //where a.Tenant == tenant && 
                //((a.Status == "" || a.Status != "2") && 
                //(a.RevaluationDate != null || a.RevaluationDate != DateTime.MinValue))

                if (
                    (entityPM.Status == "" || entityPM.Status != "2") &&
                (entityPM.RevaluationDate != null || entityPM.RevaluationDate != DateTime.MinValue))
                {
                 
                    var wrkr = new RevaluationBatch.RevaluationWorkerRole();
                    wrkr.EnQueue(entityPM.Tenant, entityPM.RevaluationNumber);


                }

            }
            base.OnUpdating(entityPM);
        }

        protected override void UpdateComposition(RevaluationPM entityPM)
        {
            base.UpdateComposition(entityPM);
        }

        protected override void Validate(RevaluationPM entityPM)
        {
            ValidationResult result = RevaluationValidator.IsRevaluationValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }


        public RevaluationPM CancelRevaluation(string revaluationId, int tenant)
        {
            var revaluationQueryService = new RevaluationQueryService(this.MainContext as IAccountingContext);
            var pm = revaluationQueryService.GetSingle(revaluationId, true, false);
            if (pm == null)
            {
                return null;
            }
            if (pm.Tenant != tenant)
            {
                return null;
            }
            //if (pm.IsCancelled)
            //{
            //    throw new ApplicationException("Revaluation already  Cancelled");
            //}
            //pm.IsCancelled = true;
            pm.ChangeSetOp = ChangeSetOperation.Update;
            this.Update(pm, true);

            return pm;
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        protected override void Trace(RevaluationPM entityPM, Revaluation entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            ContactPM contact = GetLoggedContact(entityPM.Tenant); // contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Revaluation",
                    Notes = changesXml
                });
            }
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Revaluation",
                    Notes = changesXml
                });

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "IPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Revaluation",
                    Notes = changesXml
                });
            }

        }
        protected override void AfterUpdating(RevaluationPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                if ( (entityPM.Status == "" || entityPM.Status != "2") && (entityPM.RevaluationDate != null || entityPM.RevaluationDate != DateTime.MinValue))
                {
                    RevaluationService.CreateRevaluationInBatch(entityPM.Id, entityPM.Tenant);


                }


            }
        }


        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

    }
}
