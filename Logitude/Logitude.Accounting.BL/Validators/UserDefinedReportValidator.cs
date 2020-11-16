using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Simplog.Server.Infrastructure;
using System.Runtime.InteropServices;

namespace Logitude.Accounting.BL.Validators
{
    public partial class UserDefinedReportValidator
    {
        // server side validations
        public static ValidationResult IsUserDefinedReportValid(UserDefinedReportPM entityPM)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            ValidateIsReportHasAtLeastOneCalculatedChartsOfAccountsAndValidateAllLinesRelated(entityPM, showLocals);

            return null;
        }

       

        private static void ValidateIsReportHasAtLeastOneCalculatedChartsOfAccountsAndValidateAllLinesRelated(UserDefinedReportPM entityPM, bool showLocals)
        {
            bool IsAtLeastOneNotDeletdeCalculatedChartsOfAccounts = false;
            foreach (CalculatedChartsOfAccountPM periodPM in entityPM.CalculatedChartsOfAccounts)
            {
                if (periodPM.ChangeSetOp != ChangeSetOperation.Delete && !periodPM.IsCancelled)
                {
                    IsAtLeastOneNotDeletdeCalculatedChartsOfAccounts = true;
                }
            }
            if (!IsAtLeastOneNotDeletdeCalculatedChartsOfAccounts)
                throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.TheTeportNeedsAtLeastOne", entityPM.Tenant, showLocals));

            foreach (CalculatedChartsOfAccountPM periodPM in entityPM.CalculatedChartsOfAccounts)
            {
                if (periodPM.ChangeSetOp != ChangeSetOperation.None &&  periodPM.ChangeSetOp != ChangeSetOperation.Delete && !periodPM.IsCancelled)
                {
                    ValidateRequiredFieldsOnChartsofAccount(periodPM, showLocals);
                    ValidateIsReportHasAtLeastOneCalculatedChartsOfAccountsLine(periodPM, showLocals);
                    ValidateRequiredFieldsOnCalculatedChartsOfAccountsLine(periodPM, showLocals);
                    ValidateCanUpdatedChartsOfAccountTypeCode(periodPM, showLocals);
                    ValidateIsGLAccountorChartofAccountAlreadyExist(periodPM, entityPM, showLocals);
                }
            } 
        }

        private static void ValidateIsGLAccountorChartofAccountAlreadyExist(CalculatedChartsOfAccountPM periodPM,UserDefinedReportPM entityPM, bool showLocals)
        {
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                if (LinePM.ChangeSetOp != ChangeSetOperation.None && LinePM.ChangeSetOp != ChangeSetOperation.Delete && !LinePM.IsCancelled)
                {
                    CheckIsGLAccountorChartofAccountAlreadyExist(entityPM, periodPM, LinePM, showLocals) ;
                }
            }

        }

      

 
        private static void ValidateRequiredFieldsOnChartsofAccount(CalculatedChartsOfAccountPM periodPM , bool showLocals)
        {
            string FIELD_IS_REQUIERD = TextCodesTranslator.TranslateText("General.M.FieldIsRequired", periodPM.Tenant, showLocals);
            string RequiredChartsofAccountTypeCodeFiled = FIELD_IS_REQUIERD.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccount.F.ChartOfAccountTypeEnglishName", periodPM.Tenant, showLocals));
            string RequiredLocalNameFiled = FIELD_IS_REQUIERD.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccount.F.LocalName", periodPM.Tenant, showLocals));
            if (periodPM.ChangeSetOp != ChangeSetOperation.None && periodPM.ChangeSetOp != ChangeSetOperation.Delete && !periodPM.IsCancelled)
            {
                if (string.IsNullOrEmpty(periodPM.ChartOfAccountTypeCode))
                {
                    throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " + RequiredChartsofAccountTypeCodeFiled);
                }
                else if (string.IsNullOrEmpty(periodPM.LocalName))
                {
                    throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " + RequiredLocalNameFiled);

                }
            }

        }
      

        private static void ValidateIsReportHasAtLeastOneCalculatedChartsOfAccountsLine(CalculatedChartsOfAccountPM periodPM, bool showLocals)
        {
            bool IsAtLeastOneNotDeletdeCalculatedChartsOfAccountsLine = false;
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                if (LinePM.ChangeSetOp != ChangeSetOperation.Delete && !LinePM.IsCancelled)
                {
                    IsAtLeastOneNotDeletdeCalculatedChartsOfAccountsLine = true;
                    break;
                }
            }
            if (!IsAtLeastOneNotDeletdeCalculatedChartsOfAccountsLine)
                throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals)+" " +periodPM.Line+" "+
                                    TextCodesTranslator.TranslateText("UserDefinedReport.O.DontHaveAnyLinesInThem.", periodPM.Tenant, showLocals));

        }

        private static void ValidateRequiredFieldsOnCalculatedChartsOfAccountsLine(CalculatedChartsOfAccountPM periodPM, bool showLocals)
        {
            string FIELD_IS_REQUIERD = TextCodesTranslator.TranslateText("General.M.FieldIsRequired", periodPM.Tenant, showLocals);
            string RequiredChartsofAccountFiled = FIELD_IS_REQUIERD.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine.F.ChartOfAccountId", periodPM.Tenant, showLocals));
            string RequiredGLAccountFiled = FIELD_IS_REQUIERD.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine.F.GLAccountId", periodPM.Tenant, showLocals));
            string RequiredLineTypeFiled = FIELD_IS_REQUIERD.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine.F.LineTypeCode", periodPM.Tenant, showLocals));
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                if (LinePM.ChangeSetOp != ChangeSetOperation.None &&  LinePM.ChangeSetOp != ChangeSetOperation.Delete && !LinePM.IsCancelled)
                {   if (string.IsNullOrEmpty(LinePM.LineTypeCode))
                    {    throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                         TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals)+" "+ LinePM.Line+" "+ RequiredLineTypeFiled);
                    }
                    else if(LinePM.LineTypeCode =="1" && string.IsNullOrEmpty(LinePM.GLAccountId))
                    {    throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                          TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " + RequiredGLAccountFiled);
                    }
                    else if (LinePM.LineTypeCode == "2" && string.IsNullOrEmpty(LinePM.ChartOfAccountId))
                    {     throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                          TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " + RequiredChartsofAccountFiled);
                    }
                }
            }
        }



        private static void ThrowValidationGLAccountAlreadyExist(CalculatedChartsOfAccountPM periodPM, CalculatedChartsOfAccountsLinePM CurrentLinePM, CalculatedChartsOfAccountPM CurrentperiodPM, CalculatedChartsOfAccountsLinePM LinePM, bool showLocals)
        {
            throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + CurrentperiodPM.Line + " " +
                                TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + CurrentLinePM.Line + " " +
                                TextCodesTranslator.TranslateText("UserDefinedReport.O.ChildGLAccountsAlreadyIncluded", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " +
                                 TextCodesTranslator.TranslateText("UserDefinedReport.O.AndCantBeAddedAgain.", periodPM.Tenant, showLocals) + " (" +
                                 TextCodesTranslator.TranslateText("CalculatedChartsOfAccount", periodPM.Tenant, showLocals) + ": " + periodPM.Line + " )") ;
        }

        private static void ThrowValidationChartsofAccountAlreadyExist(CalculatedChartsOfAccountPM periodPM, CalculatedChartsOfAccountsLinePM CurrentLinePM, CalculatedChartsOfAccountPM CurrentperiodPM, CalculatedChartsOfAccountsLinePM LinePM, bool showLocals)
        {
            throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + CurrentperiodPM.Line + " " +
                                TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + CurrentLinePM.Line + " " +
                                TextCodesTranslator.TranslateText("UserDefinedReport.O.ChildGLAccountsAlreadyIncluded", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " +
                                 TextCodesTranslator.TranslateText("UserDefinedReport.O.AndCantBeAddedAgain.", periodPM.Tenant, showLocals) + " (" +
                                 TextCodesTranslator.TranslateText("CalculatedChartsOfAccount", periodPM.Tenant, showLocals) + ": " + periodPM.Line + " )") ;
        }

        private static void ValidateCanUpdatedChartsOfAccountTypeCode(CalculatedChartsOfAccountPM periodPM, bool showLocals)
        {
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                if (LinePM.ChangeSetOp != ChangeSetOperation.None && LinePM.ChangeSetOp != ChangeSetOperation.Delete && !LinePM.IsCancelled)
                {
                    if (LinePM.ChartOfAccountTypeCode != periodPM.ChartOfAccountTypeCode)
                    {
                        throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                                       TextCodesTranslator.TranslateText("UserDefinedReport.O.CantUpdateTheChartofAccountType", periodPM.Tenant, showLocals));

                    }

                }
            }

        }

        private static void CheckIsGLAccountorChartofAccountAlreadyExist(UserDefinedReportPM entityPM, CalculatedChartsOfAccountPM CurrentperiodPM, CalculatedChartsOfAccountsLinePM CurrentLinePM, bool showLocals)
        {
            string CurrentChartofAccountId = "";
            string CurrentGLAccountId = "";
            if (CurrentLinePM.LineTypeCode == "1")
            {
                CurrentGLAccountId = CurrentLinePM.GLAccountId;
                CurrentChartofAccountId = CurrentLinePM.ChartOfAccountIdForValidate;
            }
            else if (CurrentLinePM.LineTypeCode == "2")
            {
                CurrentChartofAccountId = CurrentLinePM.ChartOfAccountId;
            }
            foreach (CalculatedChartsOfAccountPM periodPM in entityPM.CalculatedChartsOfAccounts)
            {
                foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
                {
                    if ((CurrentLinePM.Line != LinePM.Line || CurrentperiodPM .Line!= periodPM.Line) && !LinePM.IsCancelled)
                    {
                        if (LinePM.LineTypeCode == "1")
                        {
                            if (CurrentLinePM.LineTypeCode == "1")
                            {
                                if ((!string.IsNullOrEmpty(CurrentGLAccountId) && CurrentGLAccountId == LinePM.GLAccountId) ||
                                    (!string.IsNullOrEmpty(CurrentChartofAccountId) && CurrentChartofAccountId == LinePM.ChartOfAccountIdForValidate))
                                {
                                    ThrowValidationGLAccountAlreadyExist(periodPM, CurrentLinePM, CurrentperiodPM, LinePM, showLocals);
                                }
                            }
                            if (CurrentLinePM.LineTypeCode == "2")
                            {
                                if (!string.IsNullOrEmpty(CurrentChartofAccountId) && CurrentChartofAccountId == LinePM.ChartOfAccountIdForValidate)
                                {
                                    ThrowValidationChartsofAccountAlreadyExist(periodPM, CurrentLinePM, CurrentperiodPM, LinePM, showLocals);
                                }
                            }
                        }
                        else if (LinePM.LineTypeCode == "2")
                        {
                            if ((LinePM.LineTypeCode == "2" && (!string.IsNullOrEmpty(CurrentChartofAccountId) && CurrentChartofAccountId == LinePM.ChartOfAccountId)) ||
                                (LinePM.LineTypeCode == "1" && (!string.IsNullOrEmpty(CurrentChartofAccountId) && CurrentChartofAccountId == LinePM.ChartOfAccountIdForValidate)))
                            {
                                ThrowValidationChartsofAccountAlreadyExist(periodPM, CurrentLinePM, CurrentperiodPM, LinePM, showLocals);
                            }
                        }
                    }
                }
            }

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