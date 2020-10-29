using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Logitude.Accounting.BL.CoreBL
{
    public class GLAccountsCSVFlatFileAnalyser
    {
        private Opening_LineDTO Opening_Line = null;
       // private Closing_LineDTO Closing_Line = null;
        private List<GLAccountSrcLineDTO> _GLAccountSrcLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IAccountingContext accountingContext;
        public CSVFlatFileLoadResult MyCSVFlatFileLoadResult = new CSVFlatFileLoadResult();
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
                FileContent = fileContent.Replace("\"", "");
                _GLAccountSrcLinesDTO = CreateGLAccountSrcLinesDTOFromFile(FileContent, out tenantFromPage4Tester);

                if (tenantFromPage4Tester.HasValue)
                {
                    ptenant = tenantFromPage4Tester.Value;
                }
                if (!ptenant.HasValue)
                {
                    throw new Exception("unable to find tenantFromPage4Tester ");
                }
                int tenant = ptenant.Value;

                _contactRep = new ContactRepository(tenant);
                _resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
                _contact = _contactRep.GetSingleContactByEmail(_resolveLoggingUserId, tenant);
                accountingContext = AccountingContext.GetContext(tenant);
                _FullAccountingSettingPM = GetFullAccountingSettings(accountingContext, tenant);
                ValidateFlatFile();

                IAccountingContext MyContext = AccountingContext.GetContext(tenant);

                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(55)))
                {
                    int count = 0;
                    bool global_errors = false;
                    string text;
                    string text_44;
                    string text_2;
                    List<GLAccountPM> newOrUpdLines = new List<GLAccountPM>();
                    foreach (GLAccountSrcLineDTO accLineDTO in _GLAccountSrcLinesDTO)
                    {
                        count++;
                        bool errors = false;
                        GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(MyContext);
                        GLAccountUpdateService gLAccountUpdateService = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

                        GLAccountPM gLAccountPM = gLAccountQueryService.GetByInternalNumber(accLineDTO.InternalNumber, tenant);
                        if (gLAccountPM == null)
                        {
                            gLAccountPM = new GLAccountPM()
                            {
                                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                Tenant = tenant,
                                InternalNumber = accLineDTO.InternalNumber,
                                //Id = "get",
                                //RevenueExpenseType = "get",
                                //ExcludeFromDeductionReport = false,
                                //CreateDate = DateTime.Now,
                                //UpdateDate = DateTime.Now,
                                //AllowEditChequePayToName = false,
                                //ActiveForInterest = false,
                                //ActiveForInterestCreditInvoice = false,
                                //Smallcashbook = false,
                                //DeductionTypeId = "get",
                                //DeductionFileTypeId = "get",
                                //AssessingOfficeCode = "get",
                                //CreatedByUserId = _resolveLoggingUserId,
                            };
                        }
                        else
                        {
                            gLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        }
                        gLAccountPM.DisplayNumber = accLineDTO.DisplayNumber;
                        gLAccountPM.ExternalDisplayNumber = accLineDTO.DisplayNumber;
                        if (gLAccountPM.IsMultiCurrency.HasValue)
                        {
                            gLAccountPM.OldIsMultiCurrency = gLAccountPM.IsMultiCurrency.Value;
                        }
                        gLAccountPM.IsMultiCurrency = accLineDTO.IsMulti;
                        if (!accLineDTO.IsMulti)
                        {
                            gLAccountPM.CurrencyCode = accLineDTO.CurrencyCode;
                            gLAccountPM.CurrencyId = null;
                        }
                        else
                        {
                            gLAccountPM.CurrencyCode = null;
                            gLAccountPM.CurrencyId = null;
                        }

                        ChartOfAccountQueryService chartOfAccountQueryService = new ChartOfAccountQueryService(MyContext);
                        ChartOfAccountPM chart = chartOfAccountQueryService.GetSinglePMByCode(accLineDTO.ChartCode, tenant);
                        if (chart == null)
                        {
                            text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                            text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.NotFound", 0, useLocal);
                            text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.ChartCode", 0, useLocal);
                            this.AddErrorRow($"{text}{count} ({accLineDTO.InternalNumber}) {text_2} {text_44} ");
                            gLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.None;
                            errors = true;
                        }
                        else
                        {
                            string chartType = chart.TypeCode; 
                            if (accLineDTO.ChartType != chartType)
                            {
                                text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                                text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.WrongM", 0, useLocal);
                                text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.ChartType", 0, useLocal);
                                this.AddErrorRow($"{text}{count} ({accLineDTO.InternalNumber}) {text_2} {text_44} ");
                                gLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.None;
                                errors = true;
                            }
                            else
                            {
                                gLAccountPM.ChartOfAccountsId = chart.Id;
                                gLAccountPM.ChartOfAccountsTypeCode = chart.TypeCode;
                                switch (chart.TypeCode)
                                {
                                    case "3":
                                        gLAccountPM.AccountTypeCode = "2";
                                        break;

                                    case "4":
                                        gLAccountPM.AccountTypeCode = "3";
                                        break;

                                    case "6":
                                        gLAccountPM.AccountTypeCode = "4";
                                        break;

                                    default:
                                        gLAccountPM.AccountTypeCode = "1";
                                        break;
                                }
                                switch (chart.TypeCode)
                                {
                                    case "1":
                                        gLAccountPM.RevenueExpenseType = "1";
                                        break;

                                    case "2":
                                        gLAccountPM.RevenueExpenseType = "2";
                                        break;

                                    default:
                                        gLAccountPM.RevenueExpenseType = "3";
                                        break;
                                }
                            }
                        }
                        if (!errors && !String.IsNullOrWhiteSpace(accLineDTO.LocalName))
                        {
                            gLAccountPM.LocalName = accLineDTO.LocalName;
                        }
                        if (!errors && !String.IsNullOrWhiteSpace(accLineDTO.EnglishName))
                        {
                            gLAccountPM.EnglishName = accLineDTO.EnglishName;
                        }
                        if (!errors)
                        {
                            gLAccountPM.IsVATExempt = accLineDTO.IsExempt;
                            gLAccountPM.ReconcileMethodCode = accLineDTO.RecoMethod;

                        }

                        //    var taxReportLine = new GLAccountSrcLinePM()
                        //    {
                        //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        //        Tenant = tenant,
                        //        GLAccountSrcId = taxReportId,
                        //        Line = nextLine,
                        //        IsExternalLine = true,
                        //        LineTypeCode = accLineDTO.LineTypeCode,
                        //        Reference = accLineDTO.Reference,
                        //        ReferenceDate = accLineDTO.ReferenceDate,
                        //        VatNumber = accLineDTO.VatNumber,
                        //        VatableInvoiceAmount = accLineDTO.VatableInvoiceAmount,
                        //        VatAmount = accLineDTO.VatAmount,
                        //        TotalInvoiceAmount = accLineDTO.VatableInvoiceAmount + accLineDTO.VatAmount,
                        //        OutputOrInput = accLineDTO.OutputOrInput,
                        //        ReferecneGroup = accLineDTO.ReferenceGroup,
                        //        UpdatedByUserId = _contact.Id,
                        //        //JournalId = "1-1027720",
                        //        TransmitStatusCode = "1",
                        //        IsEquipment = false,
                        //        LastUpdateDateTime = DateTime.Now,
                        //        GLAccountSrcDate = MyGLAccountSrcPM.GLAccountSrcMonth,
                        //    };

                        if (!errors)
                        {
                            newOrUpdLines.Add(gLAccountPM);
                        }
                        else
                        {
                            global_errors = true;
                        }
                    }

                    if (newOrUpdLines.Count == 0)
                    {
                        text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.NoLinesProcessed", 0, useLocal);
                        throw new Exception($"{text_44}");
                    }

                    GLAccountUpdateService updateService = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    //updateService.UpdateMulti(newOrUpdLines, null, null, true);
                    newOrUpdLines.ForEach(accPM =>
                        {
                            updateService.Update(accPM, true);
                        });

                    scope.Complete();

                    if (MyCSVFlatFileLoadResult.ErrorRowList.Count > 0)
                    {
                        string text_1 = MyCSVFlatFileLoadResult.ErrorRowList.FirstOrDefault();
                        throw new Exception($"{text_1}");
                    }
                    //    if (MyFlatFileLoadResult.ExceptionVendorList.Count > 0)
                    //    {
                    //        string text = MyFlatFileLoadResult.ExceptionVendorList.FirstOrDefault();
                    //        throw new Exception($"{text}");
                    //    }



                }

            }
            catch (Exception e)
            {
                string text = TranslateTextsClassTranslate("GLAccountsCSV.O.FailedWhilePerforming", 0, useLocal);

                throw new Exception($"{text} ", e);
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


        private List<GLAccountSrcLineDTO> CreateGLAccountSrcLinesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            bool reading_Lines = false;
            bool finished = false;
            var GLAccountSrcLines = new List<GLAccountSrcLineDTO>();
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
                var rowtype = rawLine.Substring(0, 1);
                switch (rowtype)
                {
                    case Opening_LineDTO.RowType:
                        {
                            Opening_Line = Opening_LineDTO.Create(rawLine);
                            reading_Lines = true;
                        }
                        break;

                    //case Closing_LineDTO.RowType:
                    //    {
                    //        Closing_Line = Closing_LineDTO.Create(rawLine);
                    //        finished = true;
                    //    }
                    //    break;

                    default:
                        if (GLAccountSrcLineDTO.RowType.Contains(rowtype))
                        {
                            if (!reading_Lines)
                            {

                                string text_3 = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLine", 0, useLocal);
                                string text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.AppearsBefore", 0, useLocal);
                                string text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.HeaderType", 0, useLocal);
                                throw new Exception($"{text_3} {rowtype} {text_44} {text_2} {Opening_LineDTO.RowType} ");
                            }
                            GLAccountSrcLineDTO taxLine = GLAccountSrcLineDTO.Create(rawLine);
                            GLAccountSrcLines.Add(taxLine);

                        }
                        else
                        {
                            string text = TranslateTextsClassTranslate("GLAccountsCSV.O.NotValidRowType", 0, useLocal);
                            throw new Exception($"{text}  {rawLine}");
                        }
                        break;
                }
                if (finished)
                {
                    break;
                }
            }
            return GLAccountSrcLines;
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
                throw new Exception("No FullAccountingSettingPM  for tenant ");
            }
            //if (string.IsNullOrWhiteSpace(myFullAccountingSettingPM.DeductionFileNumber))
            //{
            //    text = TranslateTextsClassTranslate("GLAccountsCSV.O.DeductionFileNumber", 0, useLocal);
            //    // Deduction File Number is undefined.
            //    throw new Exception(text);
            //}
            return myFullAccountingSettingPM;
        }


        private void ValidateFlatFile()
        {
            string text;
            string text_2;
            string text_44;
            if (Opening_Line == null)
            {
                text = TranslateTextsClassTranslate("GLAccountsCSV.O.HeaderLine", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.NotEncountered", 0, useLocal);
                throw new Exception($"{text} {Opening_LineDTO.RowType} {text_2}  ");
            }
            //if (Closing_Line == null)
            //{
            //    text = TranslateTextsClassTranslate("GLAccountsCSV.O.StartingRowType", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.NotEncountered", 0, useLocal);
            //    throw new Exception($"{text} {Closing_LineDTO.RowType} {text_2}  ");
            //}

            //if (Closing_Line.DeductionFileNum != Opening_Line.DeductionFileNum)
            //{
            //    text = TranslateTextsClassTranslate("GLAccountsCSV.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.FinishingRowDeductionFile", 0, useLocal);
            //    throw new Exception($"{text} {Closing_Line.DeductionFileNum} {text_44}{text_2} {Opening_Line.DeductionFileNum} ");
            //}
            //string myDeduc = _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
            //if (Closing_Line.DeductionFileNum != myDeduc)
            //{
            //    text = TranslateTextsClassTranslate("GLAccountsCSV.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.OurDeductionFile", 0, useLocal);
            //    throw new Exception($"{text} {Closing_Line.DeductionFileNum} {text_44}{text_2} {myDeduc} ");
            //}

            //if (Opening_Line.TotalInvalidRecords + Opening_Line.TotalValidRecords != Opening_Line.TotalVendorNumber)
            //{
            //    text = TranslateTextsClassTranslate("GLAccountsCSV.O.FinishingRowTotals", 0, useLocal);
            //    throw new Exception($"{text} {Opening_Line.TotalInvalidRecords} + {Opening_Line.TotalValidRecords} != {Opening_Line.TotalVendorNumber} ");
            //}

            //if (Opening_Line.TotalValidRecords != _VendorLinesDTO.Count)
            //{
            //    text = TranslateTextsClassTranslate("GLAccountsCSV.O.FinishingRowTotalVendors", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.CountVendorRows", 0, useLocal);
            //    throw new Exception($"{text} {Opening_Line.TotalValidRecords} {text_44}{text_2} {_VendorLinesDTO.Count}");
            //}


            long count = 1;
            foreach (GLAccountSrcLineDTO accLine in _GLAccountSrcLinesDTO)
            {
                if (String.IsNullOrEmpty(accLine.InternalNumber))
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.InternalNumber", 0, useLocal);
                    this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                }
                if (String.IsNullOrEmpty(accLine.DisplayNumber))
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.DisplayNumber", 0, useLocal);
                    this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                }
                if (String.IsNullOrWhiteSpace(accLine.LocalName) && String.IsNullOrWhiteSpace(accLine.EnglishName))
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.LocalName", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({accLine.InternalNumber}) {text_2} {text_44}");
                }
                if (String.IsNullOrEmpty(accLine.ChartCode))
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.ChartCode", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({accLine.InternalNumber}) {text_2} {text_44} ");
                }
                if (!accLine.IsMulti && String.IsNullOrEmpty(accLine.CurrencyCode))
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.CurrencyCode", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({accLine.InternalNumber}) {text_2} {text_44} ");
                }
                if (!accLine.IsMulti && accLine.CurrencyCode == "##")
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.CurrencyCode", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({accLine.InternalNumber}) {text_2} {text_44} ");
                }
                if (accLine.IsMulti && !String.IsNullOrEmpty(accLine.CurrencyCode) && accLine.CurrencyCode != "##")
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountIsaMulti", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({accLine.InternalNumber}) {text_44} ");
                }
                if (accLine.RecoMethod == "1" && (accLine.IsMulti || accLine.CurrencyCode == "NIS"))
                {
                    text = TranslateTextsClassTranslate("GLAccountsCSV.O.AccountLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("GLAccountsCSV.O.Wrong", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("GLAccountsCSV.O.ReconciliationMethod", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({accLine.InternalNumber}) {text_2} {text_44} ");
                }

                count++;
            }

        }



    }


    public class CSVFlatFileLoadResult
    {
        public List<string> SuccessAccountLineList = new List<string>();
        public List<string> ExceptionAccountLineList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateAccountLineLineAgainstDBErrors = new List<string>();
    }


    class Opening_LineDTO
    {
        public const string RowType = "ס"; // סוג כרטיס,... 
        public string RawLine { get; set; }
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        internal static Opening_LineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                string text = TranslateTextsClassTranslate("GLAccountsCSV.O.DoesntStartWithHeaderLine", 0, useLocal);
                throw new Exception($"{text} {RowType} ");
            }

            var rec = new Opening_LineDTO();
            rec.RawLine = rawLine;
            return rec;
        }
    }

    class Closing_LineDTO
    {
        public const string RowType = "X";
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public string RawLine { get; set; }
        //public string DeductionFileNum { get; private set; }

        //public long TotalVendorNumber { get; set; }
        //public long TotalValidRecords { get; set; }
        //public long TotalInvalidRecords { get; set; }

        internal static Closing_LineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                string text = TranslateTextsClassTranslate("GLAccountsCSV.O.DoesntStartWithRowType", 0, useLocal);
                throw new Exception($"{text} {RowType} ");
            }

            var rec = new Closing_LineDTO();
            rec.RawLine = rawLine;
            //rec.DeductionFileNum = rawLine.Substring(2 - 1, 9);

            //rec.TotalVendorNumber = long.Parse(rawLine.Substring(11 - 1, 4));
            //rec.TotalValidRecords = long.Parse(rawLine.Substring(15 - 1, 4));
            //rec.TotalInvalidRecords = long.Parse(rawLine.Substring(19 - 1, 4));


            return rec;
        }
    }




    class GLAccountSrcLineDTO
    {
        public static List<String> RowType = new List<String>(new string[]
            { "1", "2", "3", "4", "5", "6", "7", });

        public const string _EmptyDate = "00000000";
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public string RawLine { get; set; }

        public string ChartType { get; private set; }
        //public DateTime ReferenceDate { get; private set; }
        //public string ReferenceDateString { get; private set; }

        //public string ReferenceGroup { get; private set; }
        //public string Reference { get; private set; }
        //public decimal VatAmount { get; private set; }
        //public string InvoiceAmountSign { get; private set; }
        //public decimal VatableInvoiceAmount { get; private set; }
        //public string APS_Reference { get; private set; }
        //public string OutputOrInput { get; private set; }
        //public string LineTypeCode { get; private set; }
        public string InternalNumber { get; private set; }
        public string DisplayNumber { get; private set; }
        public string LocalName { get; private set; }
        public string EnglishName { get; private set; }
        public string ChartCode { get; private set; }
        public string CurrencyCode { get; private set; }
        public string MultiCurrency { get; private set; }
        public bool IsMulti { get; private set; }
        public string RecoMethod { get; private set; }
        public string Exempt { get; private set; }
        public bool IsExempt { get; private set; }
       // public string Cancelled { get; private set; }
       // public bool IsCancelled { get; private set; }

        internal static GLAccountSrcLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            bool startsWithRowTypeOk = false;
            string actualRowType = "";
            if (rawLine.Length >= 1)
            {
                actualRowType = rawLine.Substring(0, 1);
                if (RowType.Contains(actualRowType)) startsWithRowTypeOk = true;
            }

            if (!startsWithRowTypeOk || actualRowType == "")
            {
                string text = TranslateTextsClassTranslate("GLAccountsCSV.O.DoesntStartWithCoAType", 0, useLocal);
                throw new Exception($"{text} {RowType.ToString()} ");
            }

            var rec = new GLAccountSrcLineDTO();
            rec.RawLine = rawLine;

            string[] values = rawLine.Split(',').Select(sValue => sValue.Trim()).ToArray();
            int count = values.Count();
            if (count > 0) rec.ChartType = values[0];
            if (count > 1) rec.InternalNumber = values[1].TrimStart('0');
            if (count > 2) rec.DisplayNumber = values[2].TrimStart('0');
            if (count > 3) rec.LocalName = values[3];
            if (count > 4) rec.EnglishName = values[4];
            if (count > 5) rec.ChartCode = values[5].TrimStart('0');
            if (count > 6) rec.CurrencyCode = values[6].ToUpperInvariant();
            if (count > 7) 
            { 
                rec.MultiCurrency = values[7];
                rec.IsMulti = (rec.MultiCurrency == "1" || rec.MultiCurrency == "Y" || rec.MultiCurrency == "y");
            }
            if (count > 8) rec.RecoMethod = values[8];
            if (count > 9)
            {
                rec.Exempt = values[9];
                rec.IsExempt = (rec.Exempt == "1" || rec.Exempt == "Y" || rec.Exempt == "y");
            }
            //if (count > 10) 
            //{
            //    rec.Cancelled = values[10]; 
            //    rec.IsCancelled = (rec.Cancelled == "1" || rec.Cancelled == "Y" || rec.Cancelled == "y");
            //}


            //string txtDateTime = rawLine.Substring(11 - 1, 8);
            //string fieldname = "";
            //string pos = "";
            //DateTime date = DateTime.MinValue;
            //rec.ReferenceDateString = txtDateTime;
            //if (rec.ReferenceDateString != _EmptyDate)
            //{
            //    fieldname = "ReferenceDate";
            //    pos = "11 - 1, 8";
            //    date = GLAccountsCSVFlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
            //    rec.ReferenceDate = date;
            //}



            //rec.ReferenceGroup = rawLine.Substring(19 - 1, 4);
            //rec.Reference = rawLine.Substring(23 - 1, 9);
            //rec.VatAmount = 0M;
            //try
            //{
            //    rec.VatAmount = decimal.Parse(rawLine.Substring(32 - 1, 9));
            //}
            //catch (Exception e)
            //{ }

            //rec.InvoiceAmountSign = rawLine.Substring(41 - 1, 1);
            //rec.VatableInvoiceAmount = 0M;
            //try
            //{
            //    rec.VatableInvoiceAmount = decimal.Parse(rawLine.Substring(42 - 1, 10));
            //}
            //catch (Exception e)
            //{ }

            //if (rec.InvoiceAmountSign == "-") rec.VatableInvoiceAmount = -rec.VatableInvoiceAmount;

            //rec.APS_Reference = rawLine.Substring(52 - 1, 9);

            //if (actualRowType == "S" || actualRowType == "L" || actualRowType == "M" || actualRowType == "Y" || actualRowType == "I")
            //{
            //    rec.OutputOrInput = "O";
            //}
            //else
            //{
            //    rec.OutputOrInput = "I";
            //}

            //rec.LineTypeCode = actualRowType;

            return rec;
        }


    }

}
