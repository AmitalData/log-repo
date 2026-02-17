using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Counters;
using System.Collections.Generic;
using Simplog.Server.Infrastructure;
using System;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class CashBookUpdateService : EntityUpdateService<CashBook, CashBookPM, EntityPM>
    {
        protected override void OnCreating(CashBookPM entityPM, EntityPM entityParentPM)
        {

            CashBookOnCreatingService cashBookOnCreatingUpdateService = new CashBookOnCreatingService(this.MainContext as IAccountingContext);
            cashBookOnCreatingUpdateService.OnCreating(entityPM, entityParentPM);

            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(CashBookPM entityPM)
        {
            CashBookOnUpdatingService cashBookOnUpdatingUpdateService = new CashBookOnUpdatingService(this.MainContext as IAccountingContext);
            cashBookOnUpdatingUpdateService.OnUpdating(entityPM);
            base.OnUpdating(entityPM);
        }

        protected override void UpdateComposition(CashBookPM entityPM)
        {
            CashBookLineUpdateService cashBookLineUpdateService = new CashBookLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            cashBookLineUpdateService.UpdateMulti(entityPM.CashBookLines, entityPM.DeletedCashBookLines, entityPM, false);
            base.UpdateComposition(entityPM);
        }

        protected override void Validate(CashBookPM entityPM)
        {
            ValidationResult result = CashBookValidator.IsCashBookValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

        protected override void Trace(CashBookPM entityPM, CashBook entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                

                if (entityPM.EnglishName != entityPOCO.EnglishName && (!String.IsNullOrEmpty(entityPM.EnglishName) || !String.IsNullOrEmpty(entityPOCO.EnglishName)))
                {
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.EnglishName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "CashBook",
                        IsAddedManually = false,
                        EventTypeCode = "ENCH",
                        Notes = notes,

                    });
                }
                if (entityPM.LocalName != entityPOCO.LocalName && (!String.IsNullOrEmpty(entityPM.LocalName) || !String.IsNullOrEmpty(entityPOCO.LocalName)))
                {
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName;
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "CashBook",
                        IsAddedManually = false,
                        EventTypeCode = "LCCH",
                        Notes = notes,

                    });
                }
                if (entityPM.AccountId != entityPOCO.AccountId && (!String.IsNullOrEmpty(entityPM.AccountId) || !String.IsNullOrEmpty(entityPOCO.AccountId)))
                {
                    String notes = "Account Changed";

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPEV",
                        UserId = contact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "CashBook",
                        Notes = notes
                    });
                }

            }
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "CashBook",
                    Notes = changesXml
                });
            }

        }

    }
}
