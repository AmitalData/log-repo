using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class TaxReportLineUpdateService
    {

        const string StatusCode_InvoiceNumberIsNotValid = "3";
        const string LineTypeCode_StandardFromIsraeliSupplier = "T";
        const string LineTypeCode_RegularTransactions = "S";
        const string LineType_RegularTransactionTransactions = "S";
        const string LineType_UnidentifiedCustomerTransactions = "L";
        const string LineType_SelfInvoiceTransactions = "M";
        const string StatusCode_VATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed = "9";
        const string TaxReportLineInputType = "I";
        const string ReferenceGroupDefaultValue = "0000";
        protected override void OnCreating(TaxReportLinePM entityPM, EntityPM entityParentPM)
        {
            entityPM.IsManuallyChanged = true;

            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(TaxReportLinePM entityPM, TaxReportLine entityPOCO)
        {
            SetReferenceFields(entityPM);
            SetTaxReportLineStatusCodeAndLineTypeCode(entityPM);
            Validate(entityPM);
            UpdateStatusByTransmitStatusCode(entityPM,entityPOCO);
        
          
            // TASK 43057
            if (this.EntityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                if (entityPM.IsManuallyChanged == true)
                    entityPM.IsManuallyChanged = false;
            }
            else
            {
                entityPM.IsManuallyChanged = true;
            }

            UpdateJournalJournalAdditionalData(entityPM);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                entityPM.UpdatedByUserId = GetLoggedContact(entityPM.Tenant).Id;
            }
            entityPM.LastUpdateDateTime = DateTime.Now;
            base.OnUpdating(entityPM, entityPOCO);
        }

        private static void UpdateJournalJournalAdditionalData(TaxReportLinePM taxReportLine)
        {
            JournalAdditionalDataPM journalAdditionalDataPM = GetJournalAdditionalDataPM(taxReportLine);
            if (journalAdditionalDataPM != null)
            {
                journalAdditionalDataPM = MapJournalAdditionalDataPM(journalAdditionalDataPM, taxReportLine);
                SaveChangesOnJournalAdditionalData(journalAdditionalDataPM);

            }
        }
        private static void SaveChangesOnJournalAdditionalData(JournalAdditionalDataPM journalAdditionalDataPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(journalAdditionalDataPM.Tenant);
            JournalAdditionalDataUpdateService journalAdditionalDataUpdateService = new JournalAdditionalDataUpdateService(MyContext, new Dictionary<string, IContext>(), journalAdditionalDataPM.Tenant);
            journalAdditionalDataUpdateService.Update(journalAdditionalDataPM, true);
        }
        private static JournalAdditionalDataPM MapJournalAdditionalDataPM(JournalAdditionalDataPM journalAdditionalDataPM, TaxReportLinePM taxReportLine)
        {
            journalAdditionalDataPM.TaxReportTransmitStatusCode = taxReportLine.TransmitStatusCode;
            journalAdditionalDataPM.TaxReportId = taxReportLine.TaxReportId;
            journalAdditionalDataPM.ChangeSetOp = ChangeSetOperation.Update;
            return journalAdditionalDataPM;
        }
        private static JournalAdditionalDataPM GetJournalAdditionalDataPM(TaxReportLinePM taxReportLine)
        {
            JournalAdditionalDataQueryService additionalDataQueryService = new JournalAdditionalDataQueryService(taxReportLine.Tenant);
            if (taxReportLine.OutputOrInput == TaxReportLineInputType)
            {
                return additionalDataQueryService.GetSingle(taxReportLine.JournalId, taxReportLine.JournalLineNumber, false, true);
            }
            else
            {
                return additionalDataQueryService.GetSingle(taxReportLine.JournalId, 1, false, true);
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
        private  void SetTaxReportLineStatusCodeAndLineTypeCode(TaxReportLinePM entityPM)
        {
       
            TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);           
            string vatNumber = tenantQuery.GetTenantVatNumber(entityPM.Tenant);         
            entityPM.StatusCode = "6";
            entityPM.VatNumber = ModifyVatNumberToValidLength(entityPM.VatNumber);
            string trimmedZeros = entityPM.VatNumber != null ? entityPM.VatNumber.Trim('0') : null;
            bool zerosVatNumber;
            CheckIfInvoiceNumberIsNotValid(entityPM);
            if (entityPM.OutputOrInput == "O")
            {
               
                if(entityPM.ReferenceDate.Value.Month != entityPM.TaxReportDate.Value.Month)
                {
                    entityPM.StatusCode = "7";
                }
                if (entityPM.VatNumber == vatNumber)
                {
                    entityPM.LineTypeCode = "M";

                }


                if (entityPM.VatNumber == null)
                {
                    entityPM.StatusCode = "1";
                }
                else if (entityPM.VatNumber != null)
                {
                    zerosVatNumber = trimmedZeros == "" ? true : false;
                    if (entityPM.VatNumber.Length > 9 || (zerosVatNumber && entityPM.VatNumber != "000000000"))
                    {
                        entityPM.StatusCode = "2";
                    }
                    else
                    {
                        if ( entityPM.VatNumber == "000000000")
                        {
                            entityPM.StatusCode  =entityPM.LineTypeCode =="K"? "6" :"2";
                            return;
                        }
                        else
                        {
                            var chars = Regex.Matches(entityPM.VatNumber, @"[^\d{9}$]");
                            if (chars.Count == 0)
                            {
                                var digit = LuhnAlgorithm.CalculateLuhnAlgorithm(entityPM.VatNumber);
                                if (digit != 0)
                                {
                                    entityPM.StatusCode = "2";
                                }

                            }
                            else
                            {
                                entityPM.StatusCode = "2";
                            }

                           

                        }

                    }
                }

                if (entityPM.VatableInvoiceAmount != null && entityPM.VatableInvoiceAmount != 0)
                {
                    //  string s = entityPM.VatableInvoiceAmount.ToString();
                    string[] amount = entityPM.VatableInvoiceAmount.ToString().Split('.');
                    if (amount.Count() > 1 && amount[1] != "00")
                    {
                        entityPM.StatusCode = "4";
                    }

                }


                if (entityPM.Reference != null)
                {
                    var chars = Regex.Matches(entityPM.Reference.Trim(), @"[^\d{9}$]");
                    if (chars.Count != 0)
                    {
                        entityPM.StatusCode = "3";
                    }

                }
                CheckVATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed(entityPM);


            }

            //for input line

            else
            {

                if (entityPM.VatNumber == null)
                {
                    entityPM.StatusCode = "1";
                }
                else if (entityPM.VatNumber != null)
                {
                    zerosVatNumber = trimmedZeros == "" ? true : false;
                    if (entityPM.VatNumber.Length > 9 || (zerosVatNumber && entityPM.VatNumber != "000000000"))
                    {
                        entityPM.StatusCode = "2";
                    }
                    else
                    {
                        if(entityPM.VatNumber == "000000000")
                        {
                            entityPM.StatusCode = entityPM.LineTypeCode == "K" ? "6" : "2";
                            return;
                        }
                       
                        else
                        {
                            var chars = Regex.Matches(entityPM.VatNumber, @"[^\d{9}$]");
                            if (chars.Count == 0)
                            {
                                var digit = LuhnAlgorithm.CalculateLuhnAlgorithm(entityPM.VatNumber);
                                if (digit != 0)
                                {
                                    entityPM.StatusCode = "2";
                                }

                            }
                            else
                            {
                                entityPM.StatusCode = "2";
                            }

                        }

                    }
                }

                if (entityPM.Reference != null)
                {
                    var chars = Regex.Matches(entityPM.Reference.Trim(), @"[^\d{9}$]");
                    if (chars.Count != 0)
                    {
                        entityPM.StatusCode = "3";
                    }

                }

                if (entityPM.VatableInvoiceAmount != 0 && entityPM.VatableInvoiceAmount != null)
                {
                    //string s = entityPM.VatableInvoiceAmount.ToString();
                    string[] amount = entityPM.VatableInvoiceAmount.ToString().Split('.');
                    if (amount.Count() > 1 && amount[1] != "00")
                    {
                        entityPM.StatusCode = "4";
                    }

                }

                CheckVATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed(entityPM);


            }


            }


        private void CheckVATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed(TaxReportLinePM entityPM)
        {
            if (entityPM.LineTypeCode == LineTypeCode_RegularTransactions ||
                   entityPM.LineTypeCode == LineType_UnidentifiedCustomerTransactions ||
                   entityPM.LineTypeCode == LineType_SelfInvoiceTransactions)
            {
                if (entityPM.VatAmount > 17 && entityPM.VatableInvoiceAmount != null && entityPM.VatableInvoiceAmount != 0)
                {
                    var percentage = entityPM.VatAmount / entityPM.VatableInvoiceAmount;
                    var STDvatTypePercentage = GetSTDPercentage(entityPM.Tenant);
                    if ((double?)percentage > STDvatTypePercentage + 0.5)
                    {
                        entityPM.StatusCode = StatusCode_VATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed;
                    }

                }
            }

            bool isTotalInvoiceAmountAndVatAmountHaveOppositeSigns = (entityPM.TotalInvoiceAmount > 0 && entityPM.VatAmount < 0) || (entityPM.TotalInvoiceAmount < 0 && entityPM.VatAmount > 0);
            if (isTotalInvoiceAmountAndVatAmountHaveOppositeSigns)
                entityPM.StatusCode = StatusCode_VATAmountInTheRecordIsHigherThanThePercentageOfVATAllowed;
        }
        private void UpdateStatusByTransmitStatusCode(TaxReportLinePM taxReportLinePM, TaxReportLine taxReportLine )
        {
            if(taxReportLinePM.ChangeSetOp == ChangeSetOperation.Update)
            {
                    if(taxReportLinePM.TransmitStatusCode != "0" && taxReportLinePM.StatusCode =="7")
                    {
                        taxReportLinePM.StatusCode = "6";
                    }
                
            }
        }
        private bool AreAllDigits(string s) => s.All(char.IsDigit);
        private bool AreAllDigitsZero(string s) => s.All(c=>c=='0');
        private void CheckIfInvoiceNumberIsNotValid(TaxReportLinePM entityPM)
        {
            if (string.IsNullOrEmpty(entityPM.Reference))
            {
                entityPM.StatusCode = StatusCode_InvoiceNumberIsNotValid;
            }
            else if (!AreAllDigits(entityPM.Reference))
            {
                entityPM.StatusCode = StatusCode_InvoiceNumberIsNotValid;
            }
            else if ((entityPM.LineTypeCode == LineTypeCode_StandardFromIsraeliSupplier ||
                      entityPM.LineTypeCode == LineTypeCode_RegularTransactions) &&
                      AreAllDigitsZero(entityPM.Reference))
            {
                entityPM.StatusCode = StatusCode_InvoiceNumberIsNotValid;

            }
        }
        private double? GetSTDPercentage(int tenant)
        {
            VatTypeQuery vatTypeQuery = new VatTypeQuery(tenant);
            VatTypePM vatType = vatTypeQuery.GetSinglePMByCode("STD", tenant);
            VatTypePercentageQuery vatTypePercentageQuery = new VatTypePercentageQuery(tenant);
            double? STDPercentage = vatTypePercentageQuery.GetVatTypePercentagesForVatType(tenant, vatType.Id).First().Percentage;
            if (STDPercentage != null && STDPercentage != 0)
                STDPercentage = STDPercentage / 100;
            return STDPercentage;
        }

        private string ModifyVatNumberToValidLength(string vatnumber)
        {
            string vatNumber = null;
            if (vatnumber != null ) {
                vatNumber= vatnumber.Trim();
                if(vatNumber.Length > 9)
                {
                    vatNumber = vatNumber.Substring(1, 9);
                }

            }

            return vatNumber;
        }
       
        protected override void Validate(TaxReportLinePM entityPM)
        {
            
           
            base.Validate(entityPM);
        }
    
        private static void SetReferenceFields(TaxReportLinePM taxreportLine)
        {
            if (taxreportLine.Reference == null) SetTaxReportLineReferenceGroup(ReferenceGroupDefaultValue, taxreportLine);
            else
            {
                taxreportLine.Reference = RemoveSpecialChars(taxreportLine.Reference);                
                bool containsLetters = CheckIfReferenceContainsLetters(taxreportLine.Reference);
                if (containsLetters)
                {                   
                    SetReferenceGroupForReferencesWithPrefex(taxreportLine);
                   
                }
                else SetTaxReportLineReferenceGroup(ReferenceGroupDefaultValue, taxreportLine);
                TrimMoreThan9Chars(taxreportLine);               
            }                    
        }
        private static bool CheckIfReferenceContainsLetters(string reference)
        {
            Regex alphabet = new Regex("([A-Za-z])");
          return alphabet.IsMatch(reference);
        }
        private static  void SetTaxReportLineReferenceGroup(string ReferenceGroup, TaxReportLinePM taxreportLine)
        {
            taxreportLine.ReferecneGroup = ReferenceGroup;
        }
        private static void SetReferenceGroupForReferencesWithPrefex(TaxReportLinePM taxreportLine)
        {
            SetTaxReportLineReferenceGroup(null, taxreportLine);
            for (int i = 0; i < taxreportLine.Reference.Length; i++)
            {
                string referenceChar = taxreportLine.Reference.Substring(i, 1);
                bool IsReferenceHasPrefix = CheckIfReferenceHasPrefex(taxreportLine.Reference, referenceChar);
                if (IsReferenceHasPrefix)
                {
                    SetTaxReportLineReferenceGroup(taxreportLine.ReferecneGroup + referenceChar, taxreportLine);
                }
                else
                {
                    taxreportLine.Reference = taxreportLine.Reference.Substring(i, taxreportLine.Reference.Length - i);
                    break;
                }

            }
        }
        private static bool CheckIfReferenceHasPrefex(string reference, string referenceChar)
        {           
            MatchCollection prefix = Regex.Matches(referenceChar, @"^[a-zA-Z]*$");
            return prefix.Count != 0 ? true : false;
        }
        private static string RemoveSpecialChars(string reference)
        {
            char[] charsToRemove = { '-', '/', '.', '*', '\\' };
            foreach (char c in charsToRemove)
            {
                reference = reference.Replace(c.ToString(), String.Empty);
            }

            return reference;
        }
       private static void  TrimMoreThan9Chars(TaxReportLinePM taxreportLine)
        {
            if (taxreportLine.Reference.Length > 9)
            {
                taxreportLine.Reference= taxreportLine.Reference.Substring(taxreportLine.Reference.Length - 9);
            }
        }
    }


}
