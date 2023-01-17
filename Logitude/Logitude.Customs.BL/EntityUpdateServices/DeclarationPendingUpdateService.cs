using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.BL.NotificationBL;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.Repsitories;
using System.Data;
using System.Data.Entity.Core;
using System.Runtime.Remoting.Contexts;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationPendingUpdateService: EntityUpdateService<DeclarationPending, DeclarationPendingPM, DeclarationCourierStatusPM>
    {
        public bool IsUpdateComposition { get; set; }
        protected override void OnCreating(DeclarationPendingPM entityPM, DeclarationCourierStatusPM entityParentPM)
        {
            if (entityParentPM != null)
            {
                entityPM.DeclarationID = entityParentPM.DeclarationId;
            }
            if(String.IsNullOrWhiteSpace(entityPM.Status))
            {
                entityPM.Status = "A";
            }
            if (entityPM.Tenant < 1 && entityParentPM != null)
            {
                entityPM.Tenant = entityParentPM.Tenant;
            }
            //base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(DeclarationPendingPM entityPM)
        {
            base.OnUpdating(entityPM);
        }
        protected override void AfterUpdating(DeclarationPendingPM entityPM, DeclarationCourierStatusPM entityParentPM)
        {
            ICustomContext context = MainContext as CustomContext;
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusPM _MyDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(entityPM.DeclarationID, true, false);


            if (!IsUpdateComposition)
            {
                
                if (_MyDeclarationCourierStatusPM != null)
                {
                    var prevCourierPendingReasonList = _MyDeclarationCourierStatusPM.CourierPendingReasonList;
                    DeclarationPendingQueryService myCourierPendingReasonQueryService = new DeclarationPendingQueryService(context);
                    List<DeclarationPendingPM> declarationPendingPMList = myCourierPendingReasonQueryService.GetDeclarationPendingsByDeclarationId(_MyDeclarationCourierStatusPM.DeclarationId, _MyDeclarationCourierStatusPM.Tenant);
                    _MyDeclarationCourierStatusPM.CourierPendingReasonList = null;
                    foreach (var declarationPending in declarationPendingPMList)
                    {
                        CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(entityPM.Tenant);
                        Boolean isActive = courierPendingReasonRepositoryRepository.IsActive(declarationPending.CourierPendingReasonCode, declarationPending.Tenant);
                        if (isActive)
                        {
                            if (declarationPending.Status == "A")
                            {
                                if (_MyDeclarationCourierStatusPM.CourierPendingReasonList == null)
                                {
                                    _MyDeclarationCourierStatusPM.CourierPendingReasonList = declarationPending.CourierPendingReasonCode;
                                }
                                else
                                {
                                    _MyDeclarationCourierStatusPM.CourierPendingReasonList = string.Concat(_MyDeclarationCourierStatusPM.CourierPendingReasonList, ",", declarationPending.CourierPendingReasonCode);
                                }
                            }
                        }
                    }
                    if (_MyDeclarationCourierStatusPM.CourierPendingReasonList != prevCourierPendingReasonList)
                    {
                        DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), _MyDeclarationCourierStatusPM.Tenant);
                        _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        declarationCourierStatusUpdateService.Update(_MyDeclarationCourierStatusPM, true);
                    }
                }
            }



            if (_MyDeclarationCourierStatusPM != null && entityPM.CourierPendingReasonCode != "900")
            {

           
                
                   
                    CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(entityPM.Tenant);
                    Boolean isActive = courierPendingReasonRepositoryRepository.IsActive(entityPM.CourierPendingReasonCode, entityPM.Tenant);
                    CourierPendingReason courierPendingReason = courierPendingReasonRepositoryRepository.GetByCode(entityPM.CourierPendingReasonCode, entityPM.Tenant);
                    CourierPendingReason Pending900 = courierPendingReasonRepositoryRepository.GetByCode("900", entityPM.Tenant);

                    if (courierPendingReason != null && courierPendingReason.RequiresPayment == true && isActive && !Pending900.Inactive)
                        {
                            DeclarationPendingPM declarationPendingPM_900 = null;
                            if (_MyDeclarationCourierStatusPM.DeclarationPendings != null && _MyDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
                            {
                                declarationPendingPM_900 = _MyDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == _MyDeclarationCourierStatusPM.DeclarationId && r.CourierPendingReasonCode == "900").FirstOrDefault();
                            }
                            if (declarationPendingPM_900 != null)
                            {
                                declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                declarationPendingPM_900.Status = "S";
                                if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                                LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 900 Set as Solved");

                            }
                            if (declarationPendingPM_900 == null)
                            {
                                declarationPendingPM_900 = new DeclarationPendingPM();
                                declarationPendingPM_900.CourierPendingReasonCode = "900";
                                declarationPendingPM_900.Status = "A";
                                declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Insert;
                                _MyDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_900);
                            }
                            else if (declarationPendingPM_900.Status != "A")
                            {
                                declarationPendingPM_900.ChangeSetOp = ChangeSetOperation.Update;
                                declarationPendingPM_900.Status = "A";
                            }
                            if (declarationPendingPM_900.ChangeSetOp != ChangeSetOperation.None)
                            {
                                LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code 900");
                                if (_MyDeclarationCourierStatusPM.ChangeSetOp != ChangeSetOperation.Update) _MyDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                            }
                       
                    
                   
                }

                if (_MyDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.Update)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
                    declarationCourierStatusUpdateService.Update(_MyDeclarationCourierStatusPM, true);
                }

            }

        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.DeclarationPendingRepository).FastDeleteMulti(entityKeyFields);
        }
    }
}
