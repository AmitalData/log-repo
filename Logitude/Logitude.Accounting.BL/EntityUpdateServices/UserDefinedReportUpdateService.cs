 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Accounting.BL.EntityQueryServices;

namespace Logitude.Accounting.BL.EntityUpdateServices
{ 
   public partial class UserDefinedReportUpdateService 
   {
        protected override void UpdateComposition(UserDefinedReportPM entityPM)
        {
            CalculatedChartsOfAccountUpdateService _CalculatedChartsOfAccountUpdateService = new CalculatedChartsOfAccountUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            _CalculatedChartsOfAccountUpdateService.UpdateMulti(entityPM.CalculatedChartsOfAccounts, entityPM.DeletedCalculatedChartsOfAccounts, entityPM, false);
        }

        protected override void OnCreating(UserDefinedReportPM entityPM, EntityPM entityParentPM)
        {

 
        }
        protected override void OnUpdating(UserDefinedReportPM entityPM, UserDefinedReport entityPOCO)
        { 
        
        
        
        }


        protected override void Trace(UserDefinedReportPM entityPM, UserDefinedReport entityPOCO, string changesXml)
        {
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                UpdateEventsForCreate(entityPM, entityPOCO);
            }
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                UpdateEventsForUpdate(entityPM, entityPOCO);
            }
            base.Trace(entityPM, entityPOCO, changesXml);
        }


        private void UpdateEventsForCreate(UserDefinedReportPM entityPM, UserDefinedReport entityPOCO)
        {
           
                CreateEvent("CREV", entityPM);
 
        }

        private void UpdateEventsForUpdate(UserDefinedReportPM entityPM, UserDefinedReport entityPOCO)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            string notes = "";

            if (entityPM.IsCancelled && !entityPOCO.IsCancelled)
            {
                CreateEvent("CLEV", entityPM);
            }

            if (entityPM.EnglishName != entityPOCO.EnglishName)
            {
                notes += TranslateTextsClass.Translate("UserDefinedReport.F.EnglishName", entityPOCO.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPOCO.Tenant, showLocals) + entityPOCO.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPOCO.Tenant, showLocals) + entityPM.EnglishName + Environment.NewLine;
            }

            if (entityPM.LocalName != entityPOCO.LocalName)
            {
                notes += TranslateTextsClass.Translate("UserDefinedReport.F.LocalName", entityPOCO.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPOCO.Tenant, showLocals) + entityPOCO.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPOCO.Tenant, showLocals) + entityPM.LocalName + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(notes))
            {
                notes +=Environment.NewLine;
            }

            notes +=  UpdateEnventsForCalculatedChartsOfAccounts(entityPM);

            if (!string.IsNullOrEmpty(notes))
            {
                CreateEvent("UPEV", entityPM, notes);
            }
        }


        private string UpdateEnventsForCalculatedChartsOfAccounts(UserDefinedReportPM entityPM)
        {
            string notes = "";
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update && entityPM.CalculatedChartsOfAccounts.Count > 0)
            {
                ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
                string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                ContactPM contactLocal = GetLoggedContact(entityPM.Tenant);
                bool showLocals = !contactLocal.DontShowLocal;
                foreach (var line in entityPM.CalculatedChartsOfAccounts)
                {

                    if (line.ChangeSetOp == ChangeSetOperation.Update)
                    {
                        CalculatedChartsOfAccountQueryService calculatedChartsOfAccountQueryService = new CalculatedChartsOfAccountQueryService(line.Tenant);
                        CalculatedChartsOfAccountPM calculatedChartsOfAccountPM = calculatedChartsOfAccountQueryService.GetSingle(line.Id, false, false);



                        notes += TranslateTextsClass.Translate("CalculatedChartsOfAccount", line.Tenant, showLocals) + " " + TranslateTextsClass.Translate("CalculatedChartsOfAccount.F.Line", line.Tenant, showLocals) + " " + line.Line + Environment.NewLine;

                        if (line.EnglishName != calculatedChartsOfAccountPM.EnglishName)
                        {
                            notes += TranslateTextsClass.Translate("CalculatedChartsOfAccount.F.EnglishName", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.EnglishName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + calculatedChartsOfAccountPM.EnglishName + Environment.NewLine;
                        }
                        if (line.LocalName != calculatedChartsOfAccountPM.LocalName)
                        {
                            notes += TranslateTextsClass.Translate("CalculatedChartsOfAccount.F.LocalName", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.LocalName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + calculatedChartsOfAccountPM.LocalName + Environment.NewLine;
                        }
                        if (line.ChartOfAccountTypeCode != calculatedChartsOfAccountPM.ChartOfAccountTypeCode)
                        {
                            ChartOfAccountsTypeQueryService chartOfAccountsTypeQuery = new ChartOfAccountsTypeQueryService(calculatedChartsOfAccountPM.Tenant);
                            ChartOfAccountsTypePM Old_chartOfAccountsTypePM = chartOfAccountsTypeQuery.GetSingle(calculatedChartsOfAccountPM.ChartOfAccountTypeCode, false, true);
                            ChartOfAccountsTypePM New_chartOfAccountsTypePM = chartOfAccountsTypeQuery.GetSingle(line.ChartOfAccountTypeCode, false, true);
                            string old_ChartName = showLocals ? Old_chartOfAccountsTypePM.LocalName : Old_chartOfAccountsTypePM.EnglishName;
                            string new_ChartName = showLocals ? New_chartOfAccountsTypePM.LocalName : New_chartOfAccountsTypePM.EnglishName;
                            notes += TranslateTextsClass.Translate("CalculatedChartsOfAccount.F.ChartOfAccountTypeCode", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + old_ChartName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + new_ChartName + Environment.NewLine;
                        }

                        if (line.IsCancelled != calculatedChartsOfAccountPM.IsCancelled)
                        {
                            notes += TranslateTextsClass.Translate("CalculatedChartsOfAccount.F.IsCancelled", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.IsCancelled + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + calculatedChartsOfAccountPM.IsCancelled + Environment.NewLine;
                        }


                        notes += Environment.NewLine;

                    }

                    notes += UpdateEnventsForCalculatedChartsOfAccountLines(line, showLocals);

                }

            }

            return notes;

        }

        private string UpdateEnventsForCalculatedChartsOfAccountLines(CalculatedChartsOfAccountPM entityPM, bool showLocals)
        {
            string notes = "";

            foreach (var line in entityPM.CalculatedChartsOfAccountLines)
            {

                if (line.ChangeSetOp == ChangeSetOperation.Update)
                {
                    CalculatedChartsOfAccountsLineQueryService calculatedChartsOfAccountsLineQueryService = new CalculatedChartsOfAccountsLineQueryService(line.Tenant);
                    CalculatedChartsOfAccountsLinePM calculatedChartsOfAccountsLinePM = calculatedChartsOfAccountsLineQueryService.GetSingle(line.Id, false, false);



                    notes += TranslateTextsClass.Translate("CalculatedChartsOfAccountsLine", line.Tenant, showLocals) + " " + TranslateTextsClass.Translate("CalculatedChartsOfAccount.F.Line", line.Tenant, showLocals) + " " + line.Line + Environment.NewLine;

                    if (line.LineTypeCode != calculatedChartsOfAccountsLinePM.LineTypeCode)
                    {
                        CalculatedChartsLineTypeQueryService calculatedChartsLineTypeQueryService = new CalculatedChartsLineTypeQueryService(calculatedChartsOfAccountsLinePM.Tenant);
                        CalculatedChartsLineTypePM Old_calculatedChartsLineType = calculatedChartsLineTypeQueryService.GetSingle(calculatedChartsOfAccountsLinePM.LineTypeCode, false, false);
                        CalculatedChartsLineTypePM New_calculatedChartsLineType = calculatedChartsLineTypeQueryService.GetSingle(line.LineTypeCode, false, false);
                        string old_LineTypeName = showLocals ? Old_calculatedChartsLineType.LocalName : Old_calculatedChartsLineType.EnglishName;
                        string new_LineTypeName = showLocals ? New_calculatedChartsLineType.LocalName : New_calculatedChartsLineType.EnglishName;
                        notes += TranslateTextsClass.Translate("CalculatedChartsOfAccountsLine.F.LineTypeCode", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + old_LineTypeName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + new_LineTypeName + Environment.NewLine;
                    }


                    if (line.ChartOfAccountId != calculatedChartsOfAccountsLinePM.ChartOfAccountId)
                    {
                        ChartOfAccountQueryService chartOfAccountQueryService = new ChartOfAccountQueryService(calculatedChartsOfAccountsLinePM.Tenant);
                        ChartOfAccountPM Old_chartOfAccountPM = chartOfAccountQueryService.GetSingle(calculatedChartsOfAccountsLinePM.ChartOfAccountId, false, false);
                        ChartOfAccountPM New_chartOfAccountPM = chartOfAccountQueryService.GetSingle(line.ChartOfAccountId, false, false);
                        string old_ChartName = showLocals ? Old_chartOfAccountPM.LocalName : Old_chartOfAccountPM.EnglishName;
                        string new_ChartName = showLocals ? New_chartOfAccountPM.LocalName : New_chartOfAccountPM.EnglishName;
                        notes += TranslateTextsClass.Translate("CalculatedChartsOfAccountsLine.F.ChartOfAccountTypeCode", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + old_ChartName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + new_ChartName + Environment.NewLine;
                    }

                    if (line.GLAccountId != calculatedChartsOfAccountsLinePM.GLAccountId)
                    {
                        GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(calculatedChartsOfAccountsLinePM.Tenant);
                        GLAccountPM Old_GLAccountPM = gLAccountQueryService.GetSingle(calculatedChartsOfAccountsLinePM.ChartOfAccountId, false, false);
                        GLAccountPM New_GLAccountPM = gLAccountQueryService.GetSingle(line.ChartOfAccountId, false, false);
                        string old_GLAccountName = showLocals ? Old_GLAccountPM.LocalName : Old_GLAccountPM.EnglishName;
                        string new_GLAccountName = showLocals ? New_GLAccountPM.LocalName : New_GLAccountPM.EnglishName;
                        notes += TranslateTextsClass.Translate("CalculatedChartsOfAccountsLine.F.ChartOfAccountTypeCode", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + old_GLAccountName + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + new_GLAccountName + Environment.NewLine;
                    }

                    if (line.IsCancelled != calculatedChartsOfAccountsLinePM.IsCancelled)
                    {
                        notes += TranslateTextsClass.Translate("CalculatedChartsOfAccountsLine.F.IsCancelled", line.Tenant, showLocals) + "," + TranslateTextsClass.Translate("Accounting.General.O.OldValue", line.Tenant, showLocals) + line.IsCancelled + TranslateTextsClass.Translate("Accounting.General.O.NewValue", line.Tenant, showLocals) + calculatedChartsOfAccountsLinePM.IsCancelled + Environment.NewLine;
                    }


                    notes += Environment.NewLine;
                }


            }

            return notes;
        }


        private void CreateEvent(string eventCode, UserDefinedReportPM entityPM, string Notes = null)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = entityPM.Id,
                Tenant = entityPM.Tenant,
                UserId = contact.Id,
                ObjectTableName = "UserDefinedReport",
                IsAddedManually = false,
                EventTypeCode = eventCode,
                Notes = Notes,
            });
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
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
	 