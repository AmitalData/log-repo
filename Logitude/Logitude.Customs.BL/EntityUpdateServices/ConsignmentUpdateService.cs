using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using System.Configuration;
using System.Globalization;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ConsignmentUpdateService : EntityUpdateService<Consignment, ConsignmentPM, DeclarationPM>
    {

        int? maxCounter;
        protected override void OnCreating(ConsignmentPM entityPM, DeclarationPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.Id;

            //if (entityPM.ConsignmentNumber == null)
            //{
            //    entityPM.ConsignmentNumber = CodeCounter.GetNumber("Customs.Consignment", entityPM.Tenant);
            //}
            if (!this.maxCounter.HasValue)
            {
                ICustomContext _Context = MainContext as CustomContext;
                var consignmentQueryService = new ConsignmentQueryService(_Context);

                this.maxCounter = consignmentQueryService.GetMaxCounterKey(entityPM.DeclarationId, entityPM.Tenant) ?? 0;
            }
            entityPM.Tenant = entityParentPM.Tenant;
            maxCounter = entityPM.ConsignmentNumber = maxCounter.Value + 1;

        }


        protected override void OnUpdating(ConsignmentPM entityPM, Consignment entityPOCO)
        {
            if (String.IsNullOrWhiteSpace(entityPM.UnloadPortCode))
            {
                if (!string.IsNullOrWhiteSpace(EntityPOCO.UnloadPortCode) )
                {
                    LogHowClearUnloadPort(entityPM, entityPOCO);
                }
                
            }

            UpdatePendingByKeyWords(entityPM);

            base.OnUpdating(entityPM, entityPOCO);
        }


        public void UpdatePendingByKeyWords(ConsignmentPM entityPM, Boolean IsAfterDeclarationCourierStatusInsert = false)
        {
            if (!String.IsNullOrWhiteSpace(entityPM.CargoDescription))
            {
                ConsignmentPM dbOccConsignmentPM = GetDBEntity(entityPM);
                if (IsAfterDeclarationCourierStatusInsert) dbOccConsignmentPM.CargoDescription = null;
                if (entityPM.CargoDescription != dbOccConsignmentPM.CargoDescription)
                {
                    List<string> pendingReasonCodeList = new List<string>();
                    ICustomContext context = MainContext as CustomContext;
                    DeclarationCourierStatusQueryService myDeclarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    DeclarationCourierStatusPM declarationCourierStatusPM = myDeclarationCourierStatusQueryService.GetSingle(entityPM.DeclarationId, true, false);
                    if (declarationCourierStatusPM != null)
                    {
                        var pendingByKeywordQueryService = new PendingByKeywordQueryService(entityPM.Tenant);
                        var courierReasonCodeList = pendingByKeywordQueryService.GetCourierPendingReasonCodeBykeyWords(
                            entityPM.CargoDescription,
                             "1"  /*תאור טובין*/,
                            entityPM.Tenant);
                        foreach (var courierReasonCode in courierReasonCodeList)
                        {

                            if (!String.IsNullOrWhiteSpace(courierReasonCode) && !pendingReasonCodeList.Contains(courierReasonCode))
                            {
                                pendingReasonCodeList.Add(courierReasonCode);
                                DeclarationPendingPM declarationPendingPM = new DeclarationPendingPM();
                                declarationPendingPM = declarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == entityPM.DeclarationId && r.CourierPendingReasonCode == courierReasonCode).FirstOrDefault();
                                if (declarationPendingPM != null)
                                {
                                    if (declarationPendingPM.Status != "A")
                                    {
                                        declarationPendingPM.ChangeSetOp = ChangeSetOperation.Update;
                                        declarationPendingPM.Status = "A";
                                        if (declarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                    }
                                }
                                else
                                {
                                    declarationPendingPM = new DeclarationPendingPM();
                                    declarationPendingPM.ChangeSetOp = ChangeSetOperation.Insert;
                                    declarationPendingPM.Status = "A";
                                    declarationPendingPM.DeclarationID = entityPM.DeclarationId;
                                    declarationPendingPM.Tenant = entityPM.Tenant;
                                    declarationPendingPM.CourierPendingReasonCode = courierReasonCode;
                                    declarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM);
                                    if (declarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) declarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                }
                            }
                        }
                        if (declarationCourierStatusPM != null && declarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                        {
                            declarationCourierStatusUpdateService.Update(declarationCourierStatusPM, true);
                        }
                    }
                }
            }
        }


        private ConsignmentPM GetDBEntity(ConsignmentPM entityPM)
        {
            var consignmentQueryService = new ConsignmentQueryService(entityPM.Tenant);
            var myDBEntity = consignmentQueryService.GetSingle(entityPM.DeclarationId,entityPM.ConsignmentNumber, true, false);
            return myDBEntity ?? new ConsignmentPM();
        }

        private void LogHowClearUnloadPort(ConsignmentPM entityPM, Consignment entityPOCO)
        {
            try
            {
                string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20180208.LogHowClearUnloadPortUntilDateyyyyMMdd"];
                if (string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    return;
                }

                DateTime stopLogAt = DateTime.MinValue; //new DateTime(2018, 02, 20);
                stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);

                if (DateTime.Now > stopLogAt)
                {
                    return;
                }



                string jsonPM = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(entityPM);
                string jsonPOCO = Logitude.Server.Tools.Utils.ProxyUtil.JsonConvertSerialize(EntityPOCO);

                var usr = AuthenticationUtil.ResolveUserIdentityName(entityPOCO.Tenant);
                var sb = new StringBuilder();
                sb
                    .AppendLine("ResolveUserIdentityName:" + usr)
                    .AppendLine("**Stack:")
                    .AppendLine(Environment.StackTrace)
                    .AppendLine("**PM:New:")
                    .AppendLine(jsonPM)
                    .AppendLine("**POCO:old:")
                    .AppendLine(jsonPOCO);

                LogitudeSettings.HandleLogMe
                    //(mess, err, suffix, stopLogAt)
                    (sb.ToString(), false, "HowClearUnloadPort", stopLogAt);

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void UpdateComposition(ConsignmentPM entityPM)
        {
            ConsignmentPackageUpdateService consignmentPackageUpdateService = new ConsignmentPackageUpdateService(MainContext,new Dictionary<string,Simplog.Server.Infrastructure.IContext>(),Tenant);
           consignmentPackageUpdateService.UpdateMulti(entityPM.ConsignmentPackages, entityPM.DeletedConsignmentPackages, entityPM, false);

             

            


            ConsignmentInternalTransitionUpdateService consignmentInternalTransitionUpdateService = new ConsignmentInternalTransitionUpdateService(MainContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), Tenant);
            consignmentInternalTransitionUpdateService.UpdateMulti(entityPM.ConsignmentInternalTransitions, entityPM.DeletedConsignmentInternalTransitions, entityPM, false);
            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(ConsignmentPM entityPM,DeclarationPM entityParentPM)
        {

            //if (entityPM.ChangeSetOp == ChangeSetOperation.Insert || entityPM.ChangeSetOp == ChangeSetOperation.Delete)
            //{
            //    SubmitChanges();
            //    ICustomContext context = MainContext as CustomContext;
            //    ConsignmentRepository consignmentRepository = new ConsignmentRepository(context);
            //    List<Consignment> consignments = consignmentRepository.GetMulti(new DeclarationKeys() { Id = entityPM.DeclarationId });
                
            //    int index = 0;
            //    foreach (Consignment item in consignments)
            //    {
            //        index += 1;
            //        item.SequenceNumeric = index;
            //        consignmentRepository.Update(item);
            //        if (item.DeclarationId == entityPM.DeclarationId && item.ConsignmentNumber == entityPM.ConsignmentNumber)
            //        {
            //            entityPM.SequenceNumeric = item.SequenceNumeric;
            //        }
            //    }
            //    consignmentRepository.SubmitChanges();
            //}
          
          
            
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.ConsignmentRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
