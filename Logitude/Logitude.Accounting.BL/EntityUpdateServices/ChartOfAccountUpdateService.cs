using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
//using Logitude.BL.CommonDataModel.EntityPMs;
//using Logitude.BL.CommonDataModel.EntityQueries;
//using Logitude.BL.Security;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ChartOfAccountUpdateService : EntityUpdateService<ChartOfAccount, ChartOfAccountPM, EntityPM>
    {

        protected override void OnCreating(ChartOfAccountPM entityPM, EntityPM entityParentPM)
        {
            if (string.IsNullOrWhiteSpace(entityPM.ParentId))
            {
                entityPM.ParentId = null;
            }
            if (entityPM.Inactive == null)
            {
                entityPM.Inactive = false;
            }

            entityPM.Id = IdCounter.GetNumber("ChartOfAccount", entityPM.Tenant);
            //if (string.IsNullOrEmpty(entityPM.EnglishName))
            //{
            //    entityPM.EnglishName = "Empty";
            //}
            if (string.IsNullOrEmpty(entityPM.Code))
            {
                entityPM.Code = "Empty";
            }
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }

        protected override void OnUpdating(ChartOfAccountPM entityPM)
        {
            if (string.IsNullOrWhiteSpace(entityPM.ParentId))
            {
                entityPM.ParentId = null;
            }
            entityPM.SearchFields = entityPM.Code + "," + entityPM.EnglishName + "," + entityPM.LocalName;
        }

        protected override void Trace(ChartOfAccountPM entityPM, ChartOfAccount entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
               // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = contact.Id,
                    ObjectTableName = "ChartOfAccount",
                    IsAddedManually = false,
                    EventTypeCode = "CCR",
                    
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                //create trace event with updated type.
                if (entityPM.Code != entityPOCO.Code && (!String.IsNullOrEmpty(entityPM.Code) || !String.IsNullOrEmpty(entityPOCO.Code)))
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.Code.ToString() + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.Code.ToString();
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "ChartOfAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CUPD",
                        Notes=notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.EnglishName != entityPOCO.EnglishName && (!String.IsNullOrEmpty(entityPM.EnglishName) || !String.IsNullOrEmpty(entityPOCO.EnglishName)))
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.EnglishName;
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "ChartOfAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CUPD",
                        Notes = notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.LocalName != entityPOCO.LocalName && (!String.IsNullOrEmpty(entityPM.LocalName) || !String.IsNullOrEmpty(entityPOCO.LocalName)))
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + entityPOCO.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.LocalName;
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "ChartOfAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CUPD",
                        Notes = notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.Inactive != entityPOCO.Inactive && entityPM.Inactive == true)
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "ChartOfAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CUPD",
                        
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.Inactive != entityPOCO.Inactive && entityPM.Inactive == false)
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "ChartOfAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CUPD",
                        
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.TypeCode != entityPOCO.TypeCode && (!String.IsNullOrEmpty(entityPM.TypeCode) || !String.IsNullOrEmpty(entityPOCO.TypeCode)))
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);
                    ChartOfAccountsTypeRepository chartOfAccountTypeRepo = new ChartOfAccountsTypeRepository(entityPM.Tenant);
                    ChartOfAccountsType type = chartOfAccountTypeRepo.GetSingle(entityPOCO.TypeCode);

                    String notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + type.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.TypeName;
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "ChartOfAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CUPD",
                        Notes = notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
                if (entityPM.ParentId != entityPOCO.ParentId && (!String.IsNullOrEmpty(entityPM.ParentId) || !String.IsNullOrEmpty(entityPOCO.ParentId)))
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    // ContactPM loggedContact = LoggedContact(entityPM.Tenant);

                    String notes;
                    if (entityPOCO.ParentChartOfAccount != null)
                    {
                        IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
                        ChartOfAccountQueryService query = new ChartOfAccountQueryService(accountingContext);
                        ChartOfAccountPM oldChart = query.GetSingle(entityPOCO.ParentChartOfAccount.Id, false, true);
                        notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + oldChart.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.ParentName;
                    }
                    else
                    {
                        notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", 0) + " " + TranslateTextsClass.Translate("Accounting.General.O.NewValue", 0) + entityPM.ParentName;
                    }
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = contact.Id,
                        ObjectTableName = "ChartOfAccount",
                        IsAddedManually = false,
                        EventTypeCode = "CUPD",
                        Notes = notes,
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }


        protected override void Validate(ChartOfAccountPM entityPM)
        {
            ValidationResult result = ChartOfAccountValidator.IsChartOfAccountValid(entityPM);
            if (result != null)
            {
                throw new ApplicationException(result.ErrorMessage);
            }
            base.Validate(entityPM);
        }

        protected override void AfterUpdating(ChartOfAccountPM entityPM, EntityPM entityParentPM)
        {
            //
            // check child and his parents [All Levels]  - TASK 44804
            //
            string parentErrorMsg = ChartOfAccountValidator.CheckParentChild(entityPM.ParentId, entityPM.Id, entityPM.Tenant);
            if(!string.IsNullOrWhiteSpace(parentErrorMsg))
                throw new ApplicationException(parentErrorMsg);

        }



        public ContactPM GetLoggedContact(int tenant)
        {

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }

    }
}
