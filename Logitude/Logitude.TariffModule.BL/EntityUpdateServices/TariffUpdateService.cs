using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityQueryServices;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffUpdateService
    {
        protected override void OnCreating(TariffPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Tariff", entityPM.Tenant);
                entityPM.TariffNumber = CodeCounter.GetNumber("Tariff", entityPM.Tenant).ToString();
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                if (entityPM.PriceSteps == null)
                {
                    ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);
                    TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == entityPM.Tenant select d).FirstOrDefault();
                    if (iTariffSetting != null)
                    {
                        entityPM.PriceSteps = iTariffSetting.DefaultPriceSteps;
                    }
                }

                this.ValidateSurchargeUniqueSeller(entityPM);
            }
        }

        protected override void OnUpdating(TariffPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }

            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = myLoggedUserId;
                }

                this.CreateTariffVersion(entityPM);
            }

            if (entityPM.TypeCode == "ASC")
            {
                if (entityPM.TariffVersions.Where(d => d.StartDate != null || d.ExpirationDate != null).Any())
                {
                    throw new ApplicationException("Dates are not allowed in Surcharges versions");
                }

                if (entityPM.ActiveVersions.Where(d => d.StartDate != null || d.ExpirationDate != null).Any())
                {
                    throw new ApplicationException("Dates are not allowed in Surcharges versions");
                }
            }

            this.ValidateSurchargeUniqueSeller(entityPM);
 
            if (entityPM.IsApprovingDraftVersion)
            {
                this.ApproveDraftVersion(entityPM);
                entityPM.IsApprovingDraftVersion = false;
            }
        }

        protected override void UpdateComposition(TariffPM entityPM)
        {
            TariffVersionUpdateService tariffVersionUpdateService = new TariffVersionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffVersionUpdateService.UpdateMulti(entityPM.TariffVersions, entityPM.DeletedTariffVersions, entityPM, false);
        }

        protected override void Trace(TariffPM entityPM, Tariff entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
            
            if (entityPM.TariffLinesAdded)
            {
                TariffVersionPM tariffVersion = entityPM.TariffVersions.Where(p => p.IsDraft).FirstOrDefault();
                if (tariffVersion != null)
                {
                    int count = tariffVersion.TariffLines.Where(a => a.AddedManually == true).Count();
                    if (count > 0)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = entityPM.Tenant,
                            EventTypeCode = "TLAD",
                            UserId = contact.Id,
                            EntityId = entityPM.Id,
                            ObjectTableName = "Tariff",
                            Notes = "Tariff Lines manually added (" + count + " lines)"
                        });
                    }
                }

            }
            
            if (entityPM.SetAsInActive)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "SAIN",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }

            if (entityPM.SetAsReActive)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "REAC",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }

            if(entityPM.IsSurchargeUpdate)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "SUCU",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
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
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }
        }

        protected override void CheckConcurrency(TariffPM entityPM, Tariff entityPOCO)
        {
            if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID) && !entityPM.NewConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            {
                string msg = TranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
                throw new OptimisticConcurrencyException(msg);
            }

            //if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            //{
            //    if (!entityPM.ConcurrencyGUID.Equals(entityPOCO.ConcurrencyGUID))
            //    {
            //        string msg = CRMTranslateTextsClass.Translate("General.M.CantUpdateRecord", entityPM.Tenant);
            //        throw new OptimisticConcurrencyException(msg);
            //    }
            //}

            //base.CheckConcurrency(entityPM, entityPOCO);
        }

        private void CreateTariffVersion(TariffPM entityPM)
        {
            entityPM.LastVersion += 1;
            entityPM.LastStartDate = entityPM.StartDate;
            entityPM.LastExpirationDate = entityPM.ExpirationDate;

            TariffVersionPM tariffVersionPM = new TariffVersionPM()
            {
                TariffId = entityPM.Id,
                Tenant = entityPM.Tenant,
                CreateDate = entityPM.CreateDate,
                CreatedByUserId = entityPM.CreatedByUserId,
                StartDate = entityPM.StartDate,
                ExpirationDate = entityPM.ExpirationDate,
                Version = entityPM.LastVersion,
                SearchFields = entityPM.LastVersion.ToString(),
                ChangeSetOp = ChangeSetOperation.Insert,
                IsDraft = true,
            };

            if (entityPM.TypeCode == "ASC")
            {
                tariffVersionPM.StartDate = null;
                tariffVersionPM.ExpirationDate = null;
            }

            entityPM.TariffVersions.Add(tariffVersionPM);

            //TariffVersionUpdateService tariffVersionUpdateService = new TariffVersionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            //tariffVersionUpdateService.Update(tariffVersionPM, true);
        }

        private void ApproveDraftVersion(TariffPM entityPM)
        {
            bool isValid = true;
            TariffVersionPM iDraftVersion = entityPM.TariffVersions.Where(d => d.IsDraft).FirstOrDefault();

            if (iDraftVersion == null)
            {
                isValid = false;
                throw new ApplicationException("No Draft version to approve");
            }

            if (iDraftVersion.TariffLines.Where(d => d.HasErrors).Count() > 0)
            {
                isValid = false;
                throw new ApplicationException("Invalid Tariff Lines");
            }

            if (entityPM.TypeCode == "AFC")
            {
                if (iDraftVersion.ExpirationDate != null)
                {
                    if (iDraftVersion.ExpirationDate.Value.Date < entityPM.UpdateDate.Date)
                    {
                        isValid = false;
                        throw new ApplicationException("Approving past version is not allowed, please update the dates");
                    }
                }
            }

            if(isValid)
            {
                iDraftVersion.IsDraft = false;
                iDraftVersion.ApprovedByUserId = entityPM.UpdatedByUserId;
                iDraftVersion.ApproveDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                iDraftVersion.ChangeSetOp = ChangeSetOperation.Update;

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "VNAP",
                    UserId = iDraftVersion.ApprovedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = "Version " + iDraftVersion.Version + " approved",
                });

                if (entityPM.TypeCode == "ASC")
                {
                    TariffVersionPM iPreviousVersion = entityPM.ActiveVersions.OrderByDescending(o => o.CreateDate).FirstOrDefault();
                    if (iPreviousVersion != null)
                    {
                        foreach (TariffLinePM linePM in iDraftVersion.TariffLines)
                        {
                            if (linePM.StartDate != null)
                            {
                                var iPreviousLine = iPreviousVersion.TariffLines.Where(d => d.OriginPortId == linePM.OriginPortId && d.DestinationPortId == linePM.DestinationPortId).FirstOrDefault();
                                if (iPreviousLine != null)
                                {
                                    iPreviousLine.ExpirationDate = linePM.StartDate.Value.AddDays(-1);
                                    iPreviousLine.ChangeSetOp = ChangeSetOperation.Update;
                                }
                            }
                        }

                        TariffVersionUpdateService tariffVersionUpdateService = new TariffVersionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                        tariffVersionUpdateService.Update(iPreviousVersion, true);
                    }
                }
            }
        }

        private void ValidateSurchargeUniqueSeller(TariffPM entityPM)
        {
            ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);
            int iCount = (from d in iContext.Tariffs
                          where d.Tenant == entityPM.Tenant
                          && d.SellerId == entityPM.SellerId
                          && d.TypeCode == "ASC"
                          select d).Count();

            if (iCount > 1)
            {
                throw new ApplicationException("Tariff surcharge seller should be unique");
            }
        }
    }
}
