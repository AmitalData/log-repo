using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Contact = Simplog.Data.CommonDataModel.EntityPOCOs.Contact;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ChargeTypesCSVFlatFileAnalyser
    {
        private Opening_LineDTO_CTCSV Opening_Line = null;
        // private Closing_LineDTO Closing_Line = null;
        private List<ChargeTypeSrcLineDTO> _ChargeTypeSrcLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private ICommonDataContext objectContext; 
        private IAccountingContext accountingContext;
        public CSVChargeTypeFlatFileLoadResult MyCSVFlatFileLoadResult = new CSVChargeTypeFlatFileLoadResult();
        private ContactRepository _contactRep;
        private string _resolveLoggingUserId;
        private Contact _contact;
        private const bool useLocal = true;


        public void Analyse(int? ptenant, string FileContent)
        {
            try
            {
                string fileContent = ConvertFromDosHebrewToWinHebrew(FileContent);
                int? tenantFromPage4Tester = null;
                //     FileContent = fileContent.Replace("\"", "");
                _ChargeTypeSrcLinesDTO = CreateChargeTypeSrcLinesDTOFromFile(FileContent, out tenantFromPage4Tester);

                if (tenantFromPage4Tester.HasValue)
                {
                    ptenant = tenantFromPage4Tester.Value;
                }
                if (!ptenant.HasValue)
                {
                    throw new ApplicationException("unable to find tenantFromPage4Tester ");
                }
                int tenant = ptenant.Value;

                _contactRep = new ContactRepository(tenant);
                _resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
                _contact = _contactRep.GetSingleContactByEmail(_resolveLoggingUserId, tenant);
                objectContext = CommonDataContext.GetContext(tenant);
                accountingContext = AccountingContext.GetContext(tenant);
                _FullAccountingSettingPM = GetFullAccountingSettings(accountingContext, tenant);
                ValidateFlatFile(tenant);

                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);

                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(25)))
                {
                    ChargesTypePM chargeType;
                    int count = 0;
                    bool global_errors = false;
                    string text;
                    string text_44;
                    string text_2;
                    DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);

                    string measurementCode = "FIXD";
                    MeasurementQuery measurementQuery = new MeasurementQuery(tenant);
                    MeasurementPM measurementPM = measurementQuery.GetSinglePMByCode(measurementCode, tenant);
                    string measurementId = "";
                    if (measurementPM != null) measurementId = measurementPM.Id;

                    string chargesGroupCode = "NONE";
                    ChargesGroupQuery chargesGroupQuery = new ChargesGroupQuery(tenant);
                    ChargesGroupPM chargesGroupPM = chargesGroupQuery.GetSingleChargesGroupPMByCode(chargesGroupCode, tenant);
                    string chargesGroupId = "";
                    if (chargesGroupPM != null) chargesGroupId = chargesGroupPM.Id;

                    var usrid = AuthenticationUtil.ResolveUserId(tenant);

                    TenantQuery tenantQuery = new TenantQuery(tenant);
                    TenantPM tPM = tenantQuery.GetSinglePM(tenant);
                    string accountingCurrencyId = tPM.CurrencyId;

                    foreach (ChargeTypeSrcLineDTO ct1stLineDTO in _ChargeTypeSrcLinesDTO)
                    {
                        count++;
                        string recId = null;
                        string payId = null;
                        string vatTypeId = "";
                        string recInternal = "";
                        string payInternal = "";
                        bool error_on_this_line = false;
                        GLAccountQueryService gLAccountQueryService;
                        VatTypeQuery vatTypeQuery = new VatTypeQuery(tenant);
                        if (!String.IsNullOrWhiteSpace(ct1stLineDTO.ReceivableAccountingCard) || !String.IsNullOrWhiteSpace(ct1stLineDTO.PayableAccountingCard))
                        {
                            gLAccountQueryService = new GLAccountQueryService(accountingContext);
                            if (!String.IsNullOrWhiteSpace(ct1stLineDTO.ReceivableAccountingCard))
                            {
                                GLAccountPM recPM = gLAccountQueryService.GetByDisplayNumber(ct1stLineDTO.ReceivableAccountingCard, tenant).FirstOrDefault();
                                if (recPM == null || String.IsNullOrEmpty(recPM.Id))
                                {
                                    string text_rec = "Receivable GLAccount not found";// TranslateTextsClassTranslate("ChargeTypesCSV.O.NotValidRowType", 0, useLocal);

                                    this.AddErrorRow($"Line {count} {text_rec} {ct1stLineDTO.ReceivableAccountingCard}");
                                    //throw new ApplicationException($"{text_rec}  {ct1stLineDTO.ReceivableAccountingCard}");
                                    error_on_this_line = true;
                                }
                                else
                                {
                                    recId = recPM.Id;
                                    recInternal = recPM.InternalNumber;
                                }
                            }
                            if (!String.IsNullOrWhiteSpace(ct1stLineDTO.PayableAccountingCard))
                            {
                                GLAccountPM payPM = gLAccountQueryService.GetByDisplayNumber(ct1stLineDTO.PayableAccountingCard, tenant).FirstOrDefault();
                                if (payPM == null || String.IsNullOrEmpty(payPM.Id))
                                {
                                    string text_pay = "Payable GLAccount not found";// TranslateTextsClassTranslate("ChargeTypesCSV.O.NotValidRowType", 0, useLocal);
                                    this.AddErrorRow($"Line {count} {text_pay} {ct1stLineDTO.PayableAccountingCard}");
                                    //  throw new ApplicationException($"{text_pay}  {ct1stLineDTO.PayableAccountingCard}");
                                    error_on_this_line = true;
                                }
                                else
                                {
                                    payId = payPM.Id;
                                    payInternal = payPM.InternalNumber;
                                }
                            }
                        }
                        if (!String.IsNullOrWhiteSpace(ct1stLineDTO.VatType))
                        {
                            VatTypePM vatTypePM = vatTypeQuery.GetSinglePMByCode(ct1stLineDTO.VatType, tenant);
                            if (vatTypePM == null || String.IsNullOrEmpty(vatTypePM.Id))
                            {
                                string text_vat = "Vat Type not found";// TranslateTextsClassTranslate("ChargeTypesCSV.O.NotValidRowType", 0, useLocal);
                                this.AddErrorRow($"Line {count} {text_vat} {ct1stLineDTO.VatType}");
                                //     throw new ApplicationException($"{text_pay}  {ct1stLineDTO.VatType}");
                                error_on_this_line = true;
                            }
                            else
                            {
                                vatTypeId = vatTypePM.Id;
                            }

                        }


                        if (!error_on_this_line)
                        {
                            chargeType = new ChargesTypePM()

                            {
                                //ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                Tenant = tenant,
                                Code = ct1stLineDTO.Code,
                                LocalName = ct1stLineDTO.LocalName,
                                EnglishName = ct1stLineDTO.EnglishName,
                                ReceivableCreditGLAcountNumber = ct1stLineDTO.ReceivableAccountingCard,
                                PayableDebitGLAcountNumber = ct1stLineDTO.PayableAccountingCard,
                                VatTypeId = vatTypeId,

                                AddedManually = false,
                                InActive = false,
                                ChargesGroupCode = chargesGroupCode,
                                IsReceivable = ct1stLineDTO.IsReceivable,
                                IsPayable = ct1stLineDTO.IsPayable,
                                IsAir = true,
                                IsOcean = true,
                                IsInland = true,
                                IsAutoDisplayInShipment = false,
                                IsAutoDisplayInConsolidation = false,
                                Description = null, 
                                AWBPrintDescription = true,
                                DueTypeCode = "NO",

                                IsAutoDisplayInQuote = false,
                                MeasurementId = measurementId,
                                ContainerMeasurementCode = null, 


                                ContainerMeasurementId = null, 
                                ViewOrder = 100,
                                ReceivableAccountId = null, 
                                PayableAccountId = null, 
                                AccountingVATSplit = false,
                                ReceivableCreditAccount = recInternal,
                                PayableDebitAccount = payInternal,
                                ReceivablesChargesTypeExternalCode = null, 
                                IATACodeId = null, 
                                PayableDebitGLAcountId = payId,
                                ReceivableCreditGLAccountId = recId,
                                ChargesGroupId = chargesGroupId,
                                PayablesChargesTypeExternalCode = null, 
                                IsExpense = false,

                                IsBackToBack = false,
                                IsAutoDisplayInCustoms = false,
                                IsCustoms = false,
                                SATExternalId = null, 
                                IsImport = false,
                                IsDomestic = false,
                                IsExport = false,
                                IsDrop = false,
                                ReceivablesDefaultCurrencyId = accountingCurrencyId,
                                PayablesDefaultCurrencyId = accountingCurrencyId,
                                ApplyRegionalTax = false,
                                HasPickup = false,
                                HasDelivery = false,
                                IsDirectionRestricted = false,
                                IsActiveInExport = false,
                                IsActiveInImport = false,
                                IsActiveInDomestic = false,
                                IsActiveInDrop = false,

                            };




                            var ChargeTypeSvc = new ChargesTypeService(objectContext, tenant);
                            ChargeTypeSvc.Create(chargeType);
                        }
                        

                    }
                    scope.Complete();
                    if (MyCSVFlatFileLoadResult.ErrorRowList.Count > 0)
                    {
                        String errorLines = "";
                        MyCSVFlatFileLoadResult.ErrorRowList.ForEach(item => errorLines += item.ToString() + "\n");
                        throw new ApplicationException($"{errorLines}");
                    }
                    //    if (MyFlatFileLoadResult.ExceptionVendorList.Count > 0)
                    //    {
                    //        string text = MyFlatFileLoadResult.ExceptionVendorList.FirstOrDefault();
                    //        throw new ApplicationException($"{text}");
                    //    }

   


                }

            }
            catch (Exception e)
            {
                string text = "failed while performing";// TranslateTextsClassTranslate("ChargeTypesCSV.O.FailedWhilePerforming", 0, useLocal);

                throw new ApplicationException($"{text} ", e);
            }



        }

        static string ConvertFromDosHebrewToWinHebrew(string FileContent862)
        {
            var dosEnc = System.Text.Encoding.GetEncoding("DOS-862"); // ms-dos codepage ( US English )
                                                                      //var txt = File.ReadAllText(@"C: \Users\itzik\Desktop\zevel\Pages\pages.txt", dosEnc);

            byte[] dosBytes = dosEnc.GetBytes(FileContent862);
            var winHebrewEncoding = Encoding.GetEncoding("Windows-1255");

            var hebBytes = Encoding.Convert(dosEnc, winHebrewEncoding, dosBytes);
            string winHebrewString = winHebrewEncoding.GetString(hebBytes);
            return winHebrewString;
            //File.WriteAllBytes(@"C: \Users\itzik\Desktop\zevel\Pages\pages_win1255.txt", hebBytes);
            //File.WriteAllText(@"C: \Users\itzik\Desktop\zevel\Pages\pages_win1255.txt", hebrewString);

        }


        private void AddErrorRow(String errorLine)
        {

            this.MyCSVFlatFileLoadResult.ErrorRowList.Add(errorLine + " " + Environment.NewLine);

        }


        private List<ChargeTypeSrcLineDTO> CreateChargeTypeSrcLinesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            bool reading_Lines = false;
            bool finished = false;
            var ChargeTypeSrcLines = new List<ChargeTypeSrcLineDTO>();
            var lines = FileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string rawLine in lines)
            {
                if (rawLine.StartsWith("//"))//edi
                {
                    if (rawLine.StartsWith("//Tenant="))//for tester 
                    {
                        string tenantS = rawLine.Split(new string[] { "//Tenant=" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        tenant = int.Parse(tenantS);
                    }
                    continue;// remark do nothing ...
                }
                if (!reading_Lines)
                {
                    //   Opening Line 
                    reading_Lines = true;
                    continue;
                }
                else if (!String.IsNullOrWhiteSpace(rawLine))
                {
                    ChargeTypeSrcLineDTO chargeLine = ChargeTypeSrcLineDTO.Create(rawLine);
                    ChargeTypeSrcLines.Add(chargeLine);
                }
                //var rowtype = rawLine.Split(',')[0];///.Substring(0, 1);

                //if (Opening_LineDTO_CTCSV.RowType.Contains(rowtype) || Opening_LineDTO_CTCSV.RowType.Contains(rowtype.Substring(0, 1)))
                //{
                //    Opening_Line = Opening_LineDTO_CTCSV.Create(rawLine);
                //    reading_Lines = true;
                //}
                //else if (ChargeTypeSrcLineDTO.RowType.Contains(rowtype))
                //{
                //    if (!reading_Lines)
                //    {
                //        reading_Lines = true;
                //        //string text_3 = TranslateTextsClassTranslate("ChargeTypesCSV.O.AccountLine", 0, useLocal);
                //        //string text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.AppearsBefore", 0, useLocal);
                //        //string text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.HeaderType", 0, useLocal);
                //        //throw new ApplicationException($"{text_3} {rowtype} {text_44} {text_2} {Opening_LineDTO_CTCSV.RowType} ");
                //    }
                //    ChargeTypeSrcLineDTO taxLine = ChargeTypeSrcLineDTO.Create(rawLine);
                //    ChargeTypeSrcLines.Add(taxLine);

                //}
                //else
                //{
                //    string text = TranslateTextsClassTranslate("ChargeTypesCSV.O.NotValidRowType", 0, useLocal);
                //    throw new ApplicationException($"{text}  {rawLine}");
                //}
                if (finished)
                {
                    break;
                }
            }
            return ChargeTypeSrcLines;
        }


        public static DateTime TryGetDateTime(string rawLine, string txtDateTime, string fieldname, string pos, string @format = "yyyyMMddHHmm")
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParseExact(txtDateTime, @format/*"yyyyMMddHHmm"*/, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (date == DateTime.MinValue)
            {
                throw new
                    Exception($"{fieldname} should be  yyyyMMddHHmm  Substring({pos}) ={txtDateTime}  ");
            }

            return date;
        }



        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }


        private FullAccountingSettingPM GetFullAccountingSettings(IAccountingContext accountingContext, int tenant)
        {
            //string text;
            var myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
            var myFullAccountingSettingPM = myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (myFullAccountingSettingPM == null)
            {
                throw new ApplicationException("No FullAccountingSettingPM  for tenant ");
            }
            //if (string.IsNullOrWhiteSpace(myFullAccountingSettingPM.DeductionFileNumber))
            //{
            //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.DeductionFileNumber", 0, useLocal);
            //    // Deduction File Number is undefined.
            //    throw new ApplicationException(text);
            //}
            return myFullAccountingSettingPM;
        }


        private void ValidateFlatFile(int tenant)
        {
            string text;
            string text_2;
            string text_44;
            //if (Opening_Line == null)
            //{
            //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.HeaderLine", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.NotEncountered", 0, useLocal);
            //    throw new ApplicationException($"{text} {Opening_LineDTO_CTCSV.RowType} {text_2}  ");
            //}
            //if (Closing_Line == null)
            //{
            //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.StartingRowType", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.NotEncountered", 0, useLocal);
            //    throw new ApplicationException($"{text} {Closing_LineDTO.RowType} {text_2}  ");
            //}

            //if (Closing_Line.DeductionFileNum != Opening_Line.DeductionFileNum)
            //{
            //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.FinishingRowDeductionFile", 0, useLocal);
            //    throw new ApplicationException($"{text} {Closing_Line.DeductionFileNum} {text_44}{text_2} {Opening_Line.DeductionFileNum} ");
            //}
            //string myDeduc = _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
            //if (Closing_Line.DeductionFileNum != myDeduc)
            //{
            //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.OurDeductionFile", 0, useLocal);
            //    throw new ApplicationException($"{text} {Closing_Line.DeductionFileNum} {text_44}{text_2} {myDeduc} ");
            //}

            //if (Opening_Line.TotalInvalidRecords + Opening_Line.TotalValidRecords != Opening_Line.TotalVendorNumber)
            //{
            //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.FinishingRowTotals", 0, useLocal);
            //    throw new ApplicationException($"{text} {Opening_Line.TotalInvalidRecords} + {Opening_Line.TotalValidRecords} != {Opening_Line.TotalVendorNumber} ");
            //}

            //if (Opening_Line.TotalValidRecords != _VendorLinesDTO.Count)
            //{
            //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.FinishingRowTotalVendors", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.CountVendorRows", 0, useLocal);
            //    throw new ApplicationException($"{text} {Opening_Line.TotalValidRecords} {text_44}{text_2} {_VendorLinesDTO.Count}");
            //}

            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            long count = 1;
            foreach (ChargeTypeSrcLineDTO ctLine in _ChargeTypeSrcLinesDTO)
            {
                //if (ctLine.ActionCode != "2")
                //{
                //    if (String.IsNullOrEmpty(ctLine.CreditGLAccount))
                //    {
                //        text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //        text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.IsMissing", 0, useLocal);
                //        text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.CreditGLAccount", 0, useLocal);
                //        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                //    }
                //    GLAccountPM creditPM = gLAccountQueryService.GetSinglePMByInternalNumber(jLine.CreditGLAccount, tenant);
                //    if (creditPM == null)
                //    {
                //        text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //        text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.NotFound", 0, useLocal);
                //        text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.CreditGLAccount", 0, useLocal);
                //        this.AddErrorRow($"{text}{count} {text_2} {jLine.CreditGLAccount} {text_44}");
                //    }
                //    else
                //    {
                //        ctLine.CreditGLAccountId = creditPM.Id;
                //    }
                //}
                //if (ctLine.ActionCode != "1")
                //{
                //    if (String.IsNullOrEmpty(jLine.DebitGLAccount))
                //    {
                //        text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //        text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.IsMissing", 0, useLocal);
                //        text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.DebitGLAccount", 0, useLocal);
                //        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                //    }
                //    GLAccountPM debitPM = gLAccountQueryService.GetSinglePMByInternalNumber(jLine.DebitGLAccount, tenant);
                //    if (debitPM == null)
                //    {
                //        text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //        text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.NotFound", 0, useLocal);
                //        text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.DebitGLAccount", 0, useLocal);
                //        this.AddErrorRow($"{text}{count} {text_2} {jLine.DebitGLAccount} {text_44}");
                //    }
                //    else
                //    {
                //        jLine.DebitGLAccountId = debitPM.Id;
                //    }
                //}
                //if (String.IsNullOrWhiteSpace(jLine.LocalName) && String.IsNullOrWhiteSpace(jLine.EnglishName))
                //{
                //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.LocalName", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44}");
                //}
                //if (String.IsNullOrEmpty(jLine.ChartCode))
                //{
                //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChartCode", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}
                //if (!jLine.IsMulti && String.IsNullOrEmpty(jLine.CurrencyCode))
                //{
                //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.CurrencyCode", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}
                //if (!jLine.IsMulti && jLine.CurrencyCode == "##")
                //{
                //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.CurrencyCode", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}
                //if (jLine.IsMulti && !String.IsNullOrEmpty(jLine.CurrencyCode) && jLine.CurrencyCode != "##")
                //{
                //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.AccountIsaMulti", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_44} ");
                //}
                //if (jLine.RecoMethod == "1" && (jLine.IsMulti || jLine.CurrencyCode == "NIS"))
                //{
                //    text = TranslateTextsClassTranslate("ChargeTypesCSV.O.ChargeTypeLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ChargeTypesCSV.O.Wrong", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ChargeTypesCSV.O.ReconciliationMethod", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}

                count++;
            }

        }



    }


    public class CSVChargeTypeFlatFileLoadResult
    {
        public List<string> SuccessAccountLineList = new List<string>();
        public List<string> ExceptionAccountLineList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateAccountLineLineAgainstDBErrors = new List<string>();
    }


    class Opening_LineDTO_CTCSV
    {
        //public const string RowType = "ס"; // סוג כרטיס,... 
        public static List<String> RowType = new List<String>(new string[]
            { "ס", "A", "DebitCredit"});
        public string RawLine { get; set; }
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        internal static Opening_LineDTO_CTCSV Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            bool startsWithRowTypeOk = false;
            string actualRowType = "";
            if (rawLine.Length >= 1)
            {
                actualRowType = rawLine.Split(',')[0]; ////rawLine.Substring(0, 1);
                if (RowType.Contains(actualRowType) || RowType.Contains(actualRowType.Substring(0, 1))) startsWithRowTypeOk = true;
            }

            if (!startsWithRowTypeOk || actualRowType == "")
            {
                string text = TranslateTextsClassTranslate("ChargeTypesCSV.O.DoesntStartWithHeaderLine", 0, useLocal);
                throw new ApplicationException($"{text} {RowType} ");
            }

            var rec = new Opening_LineDTO_CTCSV();
            rec.RawLine = rawLine;
            return rec;
        }
    }

    //class Closing_LineDTO
    //{
    //    public const string RowType = "X";
    //    private const bool useLocal = true;

    //    public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
    //    {
    //        return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
    //    }

    //    public string RawLine { get; set; }
    //    //public string DeductionFileNum { get; private set; }

    //    //public long TotalVendorNumber { get; set; }
    //    //public long TotalValidRecords { get; set; }
    //    //public long TotalInvalidRecords { get; set; }

    //    internal static Closing_LineDTO Create(string rawLine)
    //    {

    //        rawLine = rawLine ?? "";
    //        if (!rawLine.StartsWith(RowType))
    //        {
    //            string text = TranslateTextsClassTranslate("ChargeTypesCSV.O.DoesntStartWithRowType", 0, useLocal);
    //            throw new ApplicationException($"{text} {RowType} ");
    //        }

    //        var rec = new Closing_LineDTO();
    //        rec.RawLine = rawLine;
    //        //rec.DeductionFileNum = rawLine.Substring(2 - 1, 9);

    //        //rec.TotalVendorNumber = long.Parse(rawLine.Substring(11 - 1, 4));
    //        //rec.TotalValidRecords = long.Parse(rawLine.Substring(15 - 1, 4));
    //        //rec.TotalInvalidRecords = long.Parse(rawLine.Substring(19 - 1, 4));


    //        return rec;
    //    }
    //}




    class ChargeTypeSrcLineDTO
    {
   //     public static List<String> RowType = new List<String>(new string[]
   //         { "d", "c", "D", "C", "ז", "ח", "1", "2", "3"});

        public const string _EmptyDate = "00000000";
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public string RawLine { get; set; }

        public string Code { get; private set; }
        public string LocalName { get; private set; }
        public string EnglishName { get; private set; }
        public string ReceivableAccountingCard { get; private set; }
        public string PayableAccountingCard { get; private set; }
        public string VatType { get; private set; }
        public bool IsReceivable { get; private set; }
        public bool IsPayable { get; private set; }


        internal static ChargeTypeSrcLineDTO Create(string rawLine)
        {
            
            rawLine = rawLine ?? "";
            string orig_rawLine = rawLine;
            if (rawLine.Contains("\""))
            {
                Regex regex = new Regex("\\\"(.*?)\\\"");
                string temp = regex.Replace(rawLine, m => m.Value.Replace(',', '@'));
                rawLine = temp.Replace("@", "").Replace("\"", "");
            }
            var rec = new ChargeTypeSrcLineDTO();
            rec.RawLine = rawLine;
            rec.IsReceivable = false;
            rec.IsPayable = false;

            string[] values = rawLine.Split(',').Select(sValue => sValue.Trim()).ToArray();
            int count = values.Count();
            if (count > 0)  
            {
                rec.Code = values[0];
            }

            if (count > 1)
            {
                rec.LocalName = values[1];
            }
            if (count > 2)
            {
                rec.EnglishName = values[2];
            }
            if (count > 3)
            {
                rec.ReceivableAccountingCard = values[3];
                if (!String.IsNullOrEmpty(rec.ReceivableAccountingCard)) rec.IsReceivable = true;
            }
            if (count > 4)
            {
                rec.PayableAccountingCard = values[4];
                if (!String.IsNullOrEmpty(rec.PayableAccountingCard)) rec.IsPayable = true;
            }
            if (count > 5)
            {
                rec.VatType = values[5];
            }



            return rec;
        }


    }

}
