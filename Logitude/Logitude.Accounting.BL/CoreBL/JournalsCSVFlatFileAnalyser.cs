using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
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

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalsCSVFlatFileAnalyser
    {
        private Opening_LineDTO_JCSV Opening_Line = null;
        // private Closing_LineDTO Closing_Line = null;
        private List<JournalSrcLineDTO> _JournalSrcLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IAccountingContext accountingContext;
        public CSVJournalFlatFileLoadResult MyCSVFlatFileLoadResult = new CSVJournalFlatFileLoadResult();
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
                _JournalSrcLinesDTO = CreateJournalSrcLinesDTOFromFile(FileContent, out tenantFromPage4Tester);

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
                ValidateFlatFile(tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(tenant);

                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(25)))
                {
                    int count = 0;
                    bool global_errors = false;
                    string text;
                    string text_44;
                    string text_2;
                    DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);
                    var usrid = AuthenticationUtil.ResolveUserId(tenant);
                    JournalSrcLineDTO j1stLineDTO = _JournalSrcLinesDTO.FirstOrDefault();
                    JournalPM journal = new JournalPM()
                    {
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                        Tenant = tenant,
                        ///journal.JournalNumber = "1";
                        CreateDate = @now,
                        AccountingDate = j1stLineDTO.AccountingDate,
                        TypeCode = "0", //== REGULAR  //"1" == TEMPLATE,
                        StatusCode = "1", // "2", //1=Waiting Approval, 2=Approved 
                        AccountingEntityCode = "1",// - Journal
                        AccountingEntityId = null, 
                        AccountingEntityReference = null, 

                        UpdateDate = @now,
                        ApproveDate = @now,

                        CreatedByUserId = usrid,
                        ApprovedByUserId = usrid,

                        ExternalNo = null,
                        ExternalSystem = null,
                        OriginalJournalId = null,


                    };


                    foreach (JournalSrcLineDTO jLineDTO in _JournalSrcLinesDTO)
                    {
                        count++;
                        bool errors = false;

                        journal.JournalLines.Add(new JournalLinePM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Tenant = tenant,
                            JournalId = journal.Id,
                            AccountingDate = jLineDTO.AccountingDate,
                            ActionCode = jLineDTO.ActionCode,
                            DebitAccountId = jLineDTO.DebitGLAccountId,
                            CreditAccountId = jLineDTO.CreditGLAccountId,
                            LocalAmount = jLineDTO.LocalAmount,
                            CurrencyCode  = jLineDTO.CurrencyCode,
                            ForeignAmount = jLineDTO.ForeignAmount,

                            DocumentDate = jLineDTO.DocumentDate,
                            DueDate = jLineDTO.DueDate,
                            Reference1 = jLineDTO.Rererence1,
                            Reference2 = jLineDTO.Rererence2,
                            Reference3 = jLineDTO.Rererence3,
                            Notes = jLineDTO.Notes,
                        });



                    }

                    var JournalUP = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    JournalUP.Update(journal, true);



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
                    scope.Complete();


                }

            }
            catch (Exception e)
            {
                string text = TranslateTextsClassTranslate("JournalsCSV.O.FailedWhilePerforming", 0, useLocal);

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


        private List<JournalSrcLineDTO> CreateJournalSrcLinesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            bool reading_Lines = false;
            bool finished = false;
            var JournalSrcLines = new List<JournalSrcLineDTO>();
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

                if (Opening_LineDTO_JCSV.RowType.Contains(rowtype))
                {
                    Opening_Line = Opening_LineDTO_JCSV.Create(rawLine);
                    reading_Lines = true;
                }
                else if (JournalSrcLineDTO.RowType.Contains(rowtype))
                {
                    if (!reading_Lines)
                    {

                        string text_3 = TranslateTextsClassTranslate("JournalsCSV.O.AccountLine", 0, useLocal);
                        string text_44 = TranslateTextsClassTranslate("JournalsCSV.O.AppearsBefore", 0, useLocal);
                        string text_2 = TranslateTextsClassTranslate("JournalsCSV.O.HeaderType", 0, useLocal);
                        throw new Exception($"{text_3} {rowtype} {text_44} {text_2} {Opening_LineDTO_JCSV.RowType} ");
                    }
                    JournalSrcLineDTO taxLine = JournalSrcLineDTO.Create(rawLine);
                    JournalSrcLines.Add(taxLine);

                }
                else
                {
                    string text = TranslateTextsClassTranslate("JournalsCSV.O.NotValidRowType", 0, useLocal);
                    throw new Exception($"{text}  {rawLine}");
                }
                if (finished)
                {
                    break;
                }
            }
            return JournalSrcLines;
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
            //    text = TranslateTextsClassTranslate("JournalsCSV.O.DeductionFileNumber", 0, useLocal);
            //    // Deduction File Number is undefined.
            //    throw new Exception(text);
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
            //    text = TranslateTextsClassTranslate("JournalsCSV.O.HeaderLine", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.NotEncountered", 0, useLocal);
            //    throw new Exception($"{text} {Opening_LineDTO_JCSV.RowType} {text_2}  ");
            //}
            //if (Closing_Line == null)
            //{
            //    text = TranslateTextsClassTranslate("JournalsCSV.O.StartingRowType", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.NotEncountered", 0, useLocal);
            //    throw new Exception($"{text} {Closing_LineDTO.RowType} {text_2}  ");
            //}

            //if (Closing_Line.DeductionFileNum != Opening_Line.DeductionFileNum)
            //{
            //    text = TranslateTextsClassTranslate("JournalsCSV.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.FinishingRowDeductionFile", 0, useLocal);
            //    throw new Exception($"{text} {Closing_Line.DeductionFileNum} {text_44}{text_2} {Opening_Line.DeductionFileNum} ");
            //}
            //string myDeduc = _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
            //if (Closing_Line.DeductionFileNum != myDeduc)
            //{
            //    text = TranslateTextsClassTranslate("JournalsCSV.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.OurDeductionFile", 0, useLocal);
            //    throw new Exception($"{text} {Closing_Line.DeductionFileNum} {text_44}{text_2} {myDeduc} ");
            //}

            //if (Opening_Line.TotalInvalidRecords + Opening_Line.TotalValidRecords != Opening_Line.TotalVendorNumber)
            //{
            //    text = TranslateTextsClassTranslate("JournalsCSV.O.FinishingRowTotals", 0, useLocal);
            //    throw new Exception($"{text} {Opening_Line.TotalInvalidRecords} + {Opening_Line.TotalValidRecords} != {Opening_Line.TotalVendorNumber} ");
            //}

            //if (Opening_Line.TotalValidRecords != _VendorLinesDTO.Count)
            //{
            //    text = TranslateTextsClassTranslate("JournalsCSV.O.FinishingRowTotalVendors", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.CountVendorRows", 0, useLocal);
            //    throw new Exception($"{text} {Opening_Line.TotalValidRecords} {text_44}{text_2} {_VendorLinesDTO.Count}");
            //}

            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            long count = 1;
            foreach (JournalSrcLineDTO jLine in _JournalSrcLinesDTO)
            {
                if (jLine.ActionCode != "2")
                {
                    if (String.IsNullOrEmpty(jLine.CreditGLAccount))
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.CreditGLAccount", 0, useLocal);
                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                    }
                    GLAccountPM creditPM = gLAccountQueryService.GetSinglePMByDisplayNumber(jLine.CreditGLAccount, tenant);
                    if (creditPM == null)
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.NotFound", 0, useLocal);
                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.CreditGLAccount", 0, useLocal);
                        this.AddErrorRow($"{text}{count} {text_2} {jLine.CreditGLAccount} {text_44}");
                    }
                    else
                    {
                        jLine.CreditGLAccountId = creditPM.Id;
                    }
                }
                if (jLine.ActionCode != "1")
                {
                    if (String.IsNullOrEmpty(jLine.DebitGLAccount))
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.DebitGLAccount", 0, useLocal);
                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                    }
                    GLAccountPM debitPM = gLAccountQueryService.GetSinglePMByDisplayNumber(jLine.DebitGLAccount, tenant);
                    if (debitPM == null)
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.NotFound", 0, useLocal);
                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.DebitGLAccount", 0, useLocal);
                        this.AddErrorRow($"{text}{count} {text_2} {jLine.DebitGLAccount} {text_44}");
                    }
                    else
                    {
                        jLine.DebitGLAccountId = debitPM.Id;
                    }
                }
                //if (String.IsNullOrWhiteSpace(jLine.LocalName) && String.IsNullOrWhiteSpace(jLine.EnglishName))
                //{
                //    text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.LocalName", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44}");
                //}
                //if (String.IsNullOrEmpty(jLine.ChartCode))
                //{
                //    text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.ChartCode", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}
                //if (!jLine.IsMulti && String.IsNullOrEmpty(jLine.CurrencyCode))
                //{
                //    text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.CurrencyCode", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}
                //if (!jLine.IsMulti && jLine.CurrencyCode == "##")
                //{
                //    text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.CurrencyCode", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}
                //if (jLine.IsMulti && !String.IsNullOrEmpty(jLine.CurrencyCode) && jLine.CurrencyCode != "##")
                //{
                //    text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.AccountIsaMulti", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_44} ");
                //}
                //if (jLine.RecoMethod == "1" && (jLine.IsMulti || jLine.CurrencyCode == "NIS"))
                //{
                //    text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("JournalsCSV.O.Wrong", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("JournalsCSV.O.ReconciliationMethod", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({jLine.InternalNumber}) {text_2} {text_44} ");
                //}

                count++;
            }

        }



    }


    public class CSVJournalFlatFileLoadResult
    {
        public List<string> SuccessAccountLineList = new List<string>();
        public List<string> ExceptionAccountLineList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateAccountLineLineAgainstDBErrors = new List<string>();
    }


    class Opening_LineDTO_JCSV
    {
        //public const string RowType = "ס"; // סוג כרטיס,... 
        public static List<String> RowType = new List<String>(new string[]
            { "ס", "A", });
        public string RawLine { get; set; }
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        internal static Opening_LineDTO_JCSV Create(string rawLine)
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
                string text = TranslateTextsClassTranslate("JournalsCSV.O.DoesntStartWithHeaderLine", 0, useLocal);
                throw new Exception($"{text} {RowType} ");
            }

            var rec = new Opening_LineDTO_JCSV();
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
    //            string text = TranslateTextsClassTranslate("JournalsCSV.O.DoesntStartWithRowType", 0, useLocal);
    //            throw new Exception($"{text} {RowType} ");
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




    class JournalSrcLineDTO
    {
        public static List<String> RowType = new List<String>(new string[]
            { "d", "c", "D", "C", "ז", "ח", "1", "2", "3"});

        public const string _EmptyDate = "00000000";
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public string RawLine { get; set; }

        public string ActionCode { get; private set; }
        public DateTime AccountingDate { get; private set; }
        public string AccountingDateString { get; private set; }
        public DateTime DocumentDate { get; private set; }
        public string DocumentDateString { get; private set; }
        public DateTime DueDate { get; private set; }
        public string DueDateString { get; private set; }
  


        public string CurrencyCode { get; private set; }

        public string DebitGLAccount { get; private set; }
        public string DebitGLAccountId { get; set; }
        public string CreditGLAccount { get; private set; }
        public string CreditGLAccountId { get; set; }
        public decimal LocalAmount { get; private set; }
        public decimal ForeignAmount { get; private set; }
        public decimal ExternalOpenAmount { get; private set; }
        public string Rererence1 { get; private set; }
        public string Rererence2 { get; private set; }
        public string Rererence3 { get; private set; }
        public string Notes { get; private set; }

        // public string Cancelled { get; private set; }
        // public bool IsCancelled { get; private set; }

        internal static JournalSrcLineDTO Create(string rawLine)
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
                string text = TranslateTextsClassTranslate("JournalsCSV.O.DoesntStartWithCoAType", 0, useLocal);
                throw new Exception($"{text} {RowType.ToString()} ");
            }

            var rec = new JournalSrcLineDTO();
            rec.RawLine = rawLine;

            string[] values = rawLine.Split(',').Select(sValue => sValue.Trim()).ToArray();
            int count = values.Count();
            if (count > 0) //rec.ActionCode = values[0];
            {
                switch (values[0])
                {
                    case "c":
                    case "C":
                    case "ז":
                    case "2": //  2 = credit in the input file
                        rec.ActionCode = "1";
                        break;
                    case "d":
                    case "D":
                    case "ח":
                    case "1": //  1 = debit in the input file
                        rec.ActionCode = "2";
                        break;
                    case "3":
                        rec.ActionCode = "3";
                        break;
                    default:
                        break;
                }
            }

            if (count > 1) rec.DebitGLAccount = values[1].TrimStart('G');
            if (count > 2) rec.CreditGLAccount = values[2].TrimStart('G');
            string txtDateTime = "";
            string fieldname = "";
            string pos = "";
            DateTime date = DateTime.MinValue;

            if (count > 3)
            {
                txtDateTime = values[3].Substring(0, 8);
                rec.AccountingDateString = txtDateTime;
                if (rec.AccountingDateString != _EmptyDate)
                {
                    fieldname = "AccountingDate";
                    pos = "0, 8";
                    date = JournalsCSVFlatFileAnalyser.TryGetDateTime(values[3], txtDateTime, fieldname, pos, format: "dd.MM.yy");
                    rec.AccountingDate = date;
                }
            }

            if (count > 4)
            {
                txtDateTime = values[4].Substring(0, 8);
                rec.DocumentDateString = txtDateTime;
                if (rec.DocumentDateString != _EmptyDate)
                {
                    fieldname = "DocumentDate";
                    pos = "0, 8";
                    date = JournalsCSVFlatFileAnalyser.TryGetDateTime(values[4], txtDateTime, fieldname, pos, format: "dd.MM.yy");
                    rec.DocumentDate = date;
                }
            }

            if (count > 5)
            {
                txtDateTime = values[5].Substring(0, 8);
                rec.DueDateString = txtDateTime;
                if (rec.DueDateString != _EmptyDate)
                {
                    fieldname = "DueDate";
                    pos = "0, 8";
                    date = JournalsCSVFlatFileAnalyser.TryGetDateTime(values[5], txtDateTime, fieldname, pos, format: "dd.MM.yy");
                    rec.DueDate = date;
                }
            }

            if (count > 6)
            {
                rec.LocalAmount = 0M;
                try
                {
                    rec.LocalAmount = decimal.Parse(values[6]);
                }
                catch (Exception e)
                { }
            }

            if (count > 7) rec.CurrencyCode = values[7].ToUpperInvariant();

            if (count > 8)
            {
                rec.ForeignAmount = 0M;
                try
                {
                    rec.ForeignAmount = decimal.Parse(values[8]);
                }
                catch (Exception e)
                { }
            }


            if (count > 9)
            {
                rec.ExternalOpenAmount = 0M;
                try
                {
                    rec.ExternalOpenAmount = decimal.Parse(values[9]);
                }
                catch (Exception e)
                { }
            }


            if (count > 10) rec.Rererence1 = values[10];
            if (count > 11) rec.Rererence2 = values[11];
            if (count > 12) rec.Rererence3 = values[12];
            if (count > 13) rec.Notes = values[13];




            //string txtDateTime = rawLine.Substring(11 - 1, 8);
            //string fieldname = "";
            //string pos = "";
            //DateTime date = DateTime.MinValue;
            //rec.ReferenceDateString = txtDateTime;
            //if (rec.ReferenceDateString != _EmptyDate)
            //{
            //    fieldname = "ReferenceDate";
            //    pos = "11 - 1, 8";
            //    date = JournalsCSVFlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
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
