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
        public static string GLAccountType = "1";
        public static string ChartsofAccountType = "2";
        public static ValidationResult IsUserDefinedReportValid(UserDefinedReportPM entityPM)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant);
            bool showLocals = !contact.DontShowLocal;
            ValidateIsReportHasAtLeastOneCalculatedChartsOfAccounts(entityPM, showLocals);
            ValidateCalculatedChartsOfAccountsLines(entityPM, showLocals);

            return null;
        }


        private static void ValidateCalculatedChartsOfAccountsLines(UserDefinedReportPM entityPM, bool showLocals)
        {
            foreach (CalculatedChartsOfAccountPM periodPM in entityPM.CalculatedChartsOfAccounts)
            {
                bool isPeriodUpdatedtAndNotDeletedorCancelled = periodPM.ChangeSetOp != ChangeSetOperation.None &&
                                                                periodPM.ChangeSetOp != ChangeSetOperation.Delete &&
                                                               !periodPM.IsCancelled;

                if (isPeriodUpdatedtAndNotDeletedorCancelled)
                {
                    ValidateRequiredFieldsOnChartsofAccount(periodPM, showLocals);
                    ValidateIsReportHasAtLeastOneCalculatedChartsOfAccountsLine(periodPM, showLocals);
                    ValidateRequiredFieldsOnCalculatedChartsOfAccountsLine(periodPM, showLocals);
                    ValidateCanUpdatedChartsOfAccountTypeCode(periodPM, showLocals);
                    ValidateIsGLAccountorChartofAccountAlreadyExist(periodPM, entityPM, showLocals);
                }
            }
        }
        private static void ValidateIsReportHasAtLeastOneCalculatedChartsOfAccounts(UserDefinedReportPM entityPM, bool showLocals)
        {
            bool isAtLeastOneNotDeletdeCalculatedChartsOfAccounts = false;
            foreach (CalculatedChartsOfAccountPM periodPM in entityPM.CalculatedChartsOfAccounts)
            {
                if (periodPM.ChangeSetOp != ChangeSetOperation.Delete && !periodPM.IsCancelled)
                {
                    isAtLeastOneNotDeletdeCalculatedChartsOfAccounts = true;
                }
            }
            if (!isAtLeastOneNotDeletdeCalculatedChartsOfAccounts)
                throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.TheTeportNeedsAtLeastOne", entityPM.Tenant, showLocals));
        }

        private static void ValidateIsGLAccountorChartofAccountAlreadyExist(CalculatedChartsOfAccountPM periodPM, UserDefinedReportPM entityPM, bool showLocals)
        {
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                bool isLineUpdatedtAndNotDeletedorCancelled = (LinePM.ChangeSetOp != ChangeSetOperation.None &&
                                                               LinePM.ChangeSetOp != ChangeSetOperation.Delete &&
                                                               !LinePM.IsCancelled);
                if (isLineUpdatedtAndNotDeletedorCancelled)
                {
                    UserDefinedReportValidatorArguments UserDefinedReportValidatorArguments = new UserDefinedReportValidatorArguments()
                    {
                        CurrentLinePM = LinePM,
                        CurrentperiodPM = periodPM,
                        ShowLocals = showLocals,
                    };
                    CheckAllLinesIsGLAccountorChartofAccountAlreadyExist(entityPM, UserDefinedReportValidatorArguments);
                }
            }

        }




        private static void ValidateRequiredFieldsOnChartsofAccount(CalculatedChartsOfAccountPM periodPM, bool showLocals)
        {
            string fieldIsRequierdText = TextCodesTranslator.TranslateText("General.M.FieldIsRequired", periodPM.Tenant, showLocals);
            string requiredLocalNameFiled = fieldIsRequierdText.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccount.F.LocalName", periodPM.Tenant, showLocals));
            bool isPeriodUpdatedtAndNotDeletedorCancelled = (periodPM.ChangeSetOp != ChangeSetOperation.None &&
                                                             periodPM.ChangeSetOp != ChangeSetOperation.Delete &&
                                                             !periodPM.IsCancelled);
            if (isPeriodUpdatedtAndNotDeletedorCancelled)
            {
                if (string.IsNullOrEmpty(periodPM.LocalName))
                {
                    throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " + requiredLocalNameFiled);

                }
            }

        }


        private static void ValidateIsReportHasAtLeastOneCalculatedChartsOfAccountsLine(CalculatedChartsOfAccountPM periodPM, bool showLocals)
        {
            bool isAtLeastOneNotDeletdeCalculatedChartsOfAccountsLine = false;
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                if (LinePM.ChangeSetOp != ChangeSetOperation.Delete && !LinePM.IsCancelled)
                {
                    isAtLeastOneNotDeletdeCalculatedChartsOfAccountsLine = true;
                    break;
                }
            }
            if (!isAtLeastOneNotDeletdeCalculatedChartsOfAccountsLine)
                throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                                    TextCodesTranslator.TranslateText("UserDefinedReport.O.DontHaveAnyLinesInThem.", periodPM.Tenant, showLocals));

        }

        private static void ValidateRequiredFieldsOnCalculatedChartsOfAccountsLine(CalculatedChartsOfAccountPM periodPM, bool showLocals)
        {
            string fieldIsRequierdText = TextCodesTranslator.TranslateText("General.M.FieldIsRequired", periodPM.Tenant, showLocals);
            string requiredChartsofAccountFiled = fieldIsRequierdText.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine.F.ChartOfAccountId", periodPM.Tenant, showLocals));
            string requiredGLAccountFiled = fieldIsRequierdText.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine.F.GLAccountId", periodPM.Tenant, showLocals));
            string requiredLineTypeFiled = fieldIsRequierdText.Replace("%FieldName", TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine.F.LineTypeCode", periodPM.Tenant, showLocals));
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                bool isLineUpdatedtAndNotDeletedorCancelled = (LinePM.ChangeSetOp != ChangeSetOperation.None &&
                                                               LinePM.ChangeSetOp != ChangeSetOperation.Delete &&
                                                              !LinePM.IsCancelled);
                if (isLineUpdatedtAndNotDeletedorCancelled)
                {
                    if (string.IsNullOrEmpty(LinePM.LineTypeCode))
                    {
                        throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                        TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " + requiredLineTypeFiled);
                    }
                    else if (LinePM.LineTypeCode == GLAccountType && string.IsNullOrEmpty(LinePM.GLAccountId))
                    {
                        throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                         TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " + requiredGLAccountFiled);
                    }
                    else if (LinePM.LineTypeCode == ChartsofAccountType && string.IsNullOrEmpty(LinePM.ChartOfAccountId))
                    {
                        throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                        TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " + requiredChartsofAccountFiled);
                    }
                }
            }
        }



        private static void ThrowValidationGLAccountAlreadyExist(UserDefinedReportValidatorArguments arguments)
        {
            throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " " + arguments.CurrentperiodPM.Line + " " +
                                TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " " + arguments.CurrentLinePM.Line + " " +
                                TextCodesTranslator.TranslateText("UserDefinedReport.O.ChildGLAccountsAlreadyIncluded", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " " + arguments.LinePM.Line + " " +
                                 TextCodesTranslator.TranslateText("UserDefinedReport.O.AndCantBeAddedAgain.", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " (" +
                                 TextCodesTranslator.TranslateText("CalculatedChartsOfAccount", arguments.PeriodPM.Tenant, arguments.ShowLocals) + ": " + arguments.PeriodPM.Line + " )");
        }

        private static void ThrowValidationChartsofAccountAlreadyExist(UserDefinedReportValidatorArguments arguments)
        {
            throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " " + arguments.CurrentperiodPM.Line + " " +
                                TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " " + arguments.CurrentLinePM.Line + " " +
                                TextCodesTranslator.TranslateText("UserDefinedReport.O.ChildGLAccountsAlreadyIncluded", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " " + arguments.LinePM.Line + " " +
                                 TextCodesTranslator.TranslateText("UserDefinedReport.O.AndCantBeAddedAgain.", arguments.PeriodPM.Tenant, arguments.ShowLocals) + " (" +
                                 TextCodesTranslator.TranslateText("CalculatedChartsOfAccount", arguments.PeriodPM.Tenant, arguments.ShowLocals) + ": " + arguments.PeriodPM.Line + " )");
        }

        private static void ValidateCanUpdatedChartsOfAccountTypeCode(CalculatedChartsOfAccountPM periodPM, bool showLocals)
        {
            foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
            {
                bool isLineUpdatedtAndNotDeletedorCancelled = (LinePM.ChangeSetOp != ChangeSetOperation.None &&
                                                              LinePM.ChangeSetOp != ChangeSetOperation.Delete &&
                                                              !LinePM.IsCancelled);
                if (isLineUpdatedtAndNotDeletedorCancelled)
                {
                    if (!string.IsNullOrEmpty(periodPM.ChartOfAccountTypeCode) &&
                        !string.IsNullOrEmpty(LinePM.ChartOfAccountTypeCode) &&
                        LinePM.ChartOfAccountTypeCode != periodPM.ChartOfAccountTypeCode)
                    {
                        throw new Exception(TextCodesTranslator.TranslateText("UserDefinedReport.O.CalculatedChartofAccount", periodPM.Tenant, showLocals) + " " + periodPM.Line + " " +
                                            TextCodesTranslator.TranslateText("CalculatedChartsOfAccountsLine", periodPM.Tenant, showLocals) + " " + LinePM.Line + " " +
                                            TextCodesTranslator.TranslateText("UserDefinedReport.O.ChartOfAccountTypeforthislinediffersfromtheChartofAccount", periodPM.Tenant, showLocals));

                    }

                }
            }

        }

        private static void CheckAllLinesIsGLAccountorChartofAccountAlreadyExist(UserDefinedReportPM entityPM, UserDefinedReportValidatorArguments userDefinedReportValidatorArguments)
        {
            string currentChartofAccountId = "";
            string currentGLAccountId = "";
            if (userDefinedReportValidatorArguments.CurrentLinePM.LineTypeCode == GLAccountType)
            {
                currentGLAccountId = userDefinedReportValidatorArguments.CurrentLinePM.GLAccountId;
                currentChartofAccountId = userDefinedReportValidatorArguments.CurrentLinePM.ChartOfAccountIdForValidate;
            }
            else if (userDefinedReportValidatorArguments.CurrentLinePM.LineTypeCode == ChartsofAccountType)
                currentChartofAccountId = userDefinedReportValidatorArguments.CurrentLinePM.ChartOfAccountId;
            foreach (CalculatedChartsOfAccountPM periodPM in entityPM.CalculatedChartsOfAccounts.Where(s => s.IsCancelled == false))
            {
                foreach (CalculatedChartsOfAccountsLinePM LinePM in periodPM.CalculatedChartsOfAccountLines)
                {
                    UserDefinedReportValidatorArguments Arguments = new UserDefinedReportValidatorArguments()
                    {
                        CurrentperiodPM = userDefinedReportValidatorArguments.CurrentperiodPM,
                        CurrentLinePM = userDefinedReportValidatorArguments.CurrentLinePM,
                        ShowLocals = userDefinedReportValidatorArguments.ShowLocals,
                        LinePM = LinePM,
                        PeriodPM = periodPM,
                    };
                    IsGLAccountorChartofAccountAlreadyExist(Arguments, currentChartofAccountId, currentGLAccountId);
                }
            }

        }

        private static void IsGLAccountorChartofAccountAlreadyExist(UserDefinedReportValidatorArguments userDefinedReportValidatorArguments,
                                                                    string currentChartofAccountId,
                                                                    string currentGLAccountId)
        {
            bool isCompareLineNotEqualCurrentLineAndNotCancelled = ((userDefinedReportValidatorArguments.CurrentLinePM.Line != userDefinedReportValidatorArguments.LinePM.Line ||
                                                                    userDefinedReportValidatorArguments.CurrentperiodPM.Line != userDefinedReportValidatorArguments.PeriodPM.Line) &&
                                                                    !userDefinedReportValidatorArguments.LinePM.IsCancelled);
            if (isCompareLineNotEqualCurrentLineAndNotCancelled)
            {
                if (userDefinedReportValidatorArguments.LinePM.LineTypeCode == GLAccountType)
                {
                    ValidateLineTypeGLAccount(userDefinedReportValidatorArguments, currentChartofAccountId, currentGLAccountId);
                }
                else if (userDefinedReportValidatorArguments.LinePM.LineTypeCode == ChartsofAccountType)
                {
                    ValidateLineTypeChartofAccount(userDefinedReportValidatorArguments, currentChartofAccountId);

                }
            }
        }
        public static void ValidateLineTypeChartofAccount(UserDefinedReportValidatorArguments userDefinedReportValidatorArguments,
                                         string currentChartofAccountId)
        {
            bool isCompareLineChartsofAccountEqualCurrentLineChartsofAccount = ((userDefinedReportValidatorArguments.CurrentLinePM.LineTypeCode == ChartsofAccountType &&
                                                                                      (!string.IsNullOrEmpty(currentChartofAccountId) &&
                                                                                       currentChartofAccountId == userDefinedReportValidatorArguments.LinePM.ChartOfAccountId)) ||
                                                                                      (userDefinedReportValidatorArguments.CurrentLinePM.LineTypeCode == GLAccountType &&
                                                                                      (!string.IsNullOrEmpty(currentChartofAccountId) &&
                                                                                      currentChartofAccountId == userDefinedReportValidatorArguments.LinePM.ChartOfAccountId)));

            if (isCompareLineChartsofAccountEqualCurrentLineChartsofAccount)
            {
                ThrowValidationChartsofAccountAlreadyExist(userDefinedReportValidatorArguments);
            }
        }

        public static void ValidateLineTypeGLAccount(UserDefinedReportValidatorArguments userDefinedReportValidatorArguments,
                                      string currentChartofAccountId,
                                      string currentGLAccountId)
        {
            if (userDefinedReportValidatorArguments.CurrentLinePM.LineTypeCode == GLAccountType)
            {
                bool isCompareLineGLAccountEqualCurrentLineGLAccount = ((!string.IsNullOrEmpty(currentGLAccountId) &&
                                                                        currentGLAccountId == userDefinedReportValidatorArguments.LinePM.GLAccountId));

                if (isCompareLineGLAccountEqualCurrentLineGLAccount)
                {
                    ThrowValidationGLAccountAlreadyExist(userDefinedReportValidatorArguments);
                }
            }
            if (userDefinedReportValidatorArguments.CurrentLinePM.LineTypeCode == ChartsofAccountType)
            {
                bool isCompareLineChartsofAccountEqualCurrentLineChartsofAccount = (!string.IsNullOrEmpty(currentChartofAccountId) &&
                                                                                    currentChartofAccountId == userDefinedReportValidatorArguments.LinePM.ChartOfAccountIdForValidate);
                if (isCompareLineChartsofAccountEqualCurrentLineChartsofAccount)
                {
                    ThrowValidationChartsofAccountAlreadyExist(userDefinedReportValidatorArguments);
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


    public class UserDefinedReportValidatorArguments
    {
        public bool ShowLocals { get; set; }
        public CalculatedChartsOfAccountsLinePM LinePM { get; set; }
        public CalculatedChartsOfAccountsLinePM CurrentLinePM { get; set; }
        public CalculatedChartsOfAccountPM CurrentperiodPM { get; set; }
        public CalculatedChartsOfAccountPM PeriodPM { get; set; }
    }
}