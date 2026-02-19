using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using static System.Net.Mime.MediaTypeNames;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalsCSVFlatFileAnalyser_ISL
    {
        private Opening_LineDTO_JCSV_ISL Opening_Line = null;

        private List<JournalSrcLineDTO_ISL> _JournalSrcLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IAccountingContext accountingContext;
        public CSVJournalFlatFileLoadResult_ISL MyCSVFlatFileLoadResult = new CSVJournalFlatFileLoadResult_ISL();
        private ContactRepository _contactRep;
        private string _resolveLoggingUserId;
        private Contact _contact;
        private const bool useLocal = true;
        private int DuplicatesSkippedCount = 0;


        public JournalAnalyseResult AnalyseWithSkip(int? ptenant, string FileContent)
        {
            JournalAnalyseResult journalAnalyseResult = new JournalAnalyseResult();
            journalAnalyseResult.JournalPM = this.Analyse(ptenant, FileContent);
            journalAnalyseResult.DuplicatesSkipped = this.DuplicatesSkippedCount;
            return journalAnalyseResult;
        }

        public JournalPM Analyse(int? ptenant, string FileContent)
        {
            try
            {
                string fileContent = ConvertFromDosHebrewToWinHebrew(FileContent);
                int? tenantFromPage4Tester = null;

                _JournalSrcLinesDTO = CreateJournalSrcLinesDTOFromFile(FileContent, out tenantFromPage4Tester);

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
                accountingContext = AccountingContext.GetContext(tenant);
                _FullAccountingSettingPM = GetFullAccountingSettings(accountingContext, tenant);
                ValidateFlatFile(tenant);
                if (!_JournalSrcLinesDTO.Where(ln => ln.SkipLine == false).Any() && _JournalSrcLinesDTO.Where(ln => ln.SkipLine == true).Any())
                {
                    string error_text = TranslateTextsClassTranslate("Journal.O.OnlyDuplicates", 0, useLocal);
                    if (String.IsNullOrEmpty(error_text)) error_text = "All lines in the file already exist in the system. The file was rejected.";
                    throw new ApplicationException(error_text);
                }
                if (this.MyCSVFlatFileLoadResult.ErrorRowList.Count == 0)
                {
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
                        JournalSrcLineDTO_ISL j1stLineDTO = _JournalSrcLinesDTO.FirstOrDefault();
                        JournalPM journal = new JournalPM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Tenant = tenant,
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


                        foreach (JournalSrcLineDTO_ISL jLineDTO in _JournalSrcLinesDTO.Where(ln => ln.SkipLine == false))
                        {

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
                                CurrencyCode = jLineDTO.CurrencyCode,
                                ForeignAmount = jLineDTO.ForeignAmount,

                                DocumentDate = jLineDTO.DocumentDate,
                                DueDate = jLineDTO.DueDate,
                                Reference1 = jLineDTO.Reference1,
                                Reference2 = jLineDTO.Reference2,
                                Reference3 = jLineDTO.Reference3,
                                Notes = jLineDTO.Notes,

                            });



                        }

                        var JournalUP = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                        JournalUP.Update(journal, true);



                        if (MyCSVFlatFileLoadResult.ErrorRowList.Count > 0)
                        {
                            string text_1 = MyCSVFlatFileLoadResult.ErrorRowList.FirstOrDefault();
                            throw new ApplicationException($"{text_1}");
                        }

                        scope.Complete();
                        return journal;


                    }
                }
                else
                {
                    JournalPM journal = new JournalPM();
                    String errorLines = "";
                    MyCSVFlatFileLoadResult.ErrorRowList.ForEach(item => errorLines += item.ToString() + "\n");
                    throw new ApplicationException($"{errorLines}");
                }

            }
            catch (Exception e)
            {
                throw;
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

        }


        private void AddErrorRow(String errorLine)
        {
            this.MyCSVFlatFileLoadResult.ErrorRowList.Add(errorLine + " " + Environment.NewLine);
        }

        private void AddAccountLineRow(String errorLine)
        {
            this.MyCSVFlatFileLoadResult.ValidateAccountLineList.Add(errorLine + " " + Environment.NewLine);
        }


        private List<JournalSrcLineDTO_ISL> CreateJournalSrcLinesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            bool reading_Lines = false;
            bool finished = false;
            var JournalSrcLines = new List<JournalSrcLineDTO_ISL>();

            var lines = FileContent
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            int currentLine = 0;
            string error_text = String.Empty;
            foreach (string rawLine in lines)
            {
                currentLine++;
                if (rawLine.StartsWith("//"))//edi
                {
                    if (rawLine.StartsWith("//Tenant="))//for tester 
                    {
                        string tenantS = rawLine.Split(new string[] { "//Tenant=" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        tenant = int.Parse(tenantS);
                    }
                    continue;// remark do nothing ...
                }
                var rowtype = rawLine.Split(',')[0];///.Substring(0, 1);

                if (!reading_Lines)
                {
                    reading_Lines = true;
                    continue;
                }
                else if (JournalSrcLineDTO_ISL.RowType.Contains(rowtype))
                {
                    if (!reading_Lines)
                    {
                        reading_Lines = true;
                    }
                    JournalSrcLineDTO_ISL taxLine = JournalSrcLineDTO_ISL.Create(rawLine, currentLine);
                    JournalSrcLines.Add(taxLine);

                }
                else if (error_text == String.Empty && !String.IsNullOrEmpty(rawLine))
                {
                    error_text = TranslateTextsClassTranslate("JournalsCSV.O.NotValidRowType", 0, useLocal);
                    if (String.IsNullOrEmpty(error_text)) error_text = "Not a valid Row Type";
                    error_text += $" {rowtype}  {rawLine}";
                    if (JournalSrcLines.Any()) // An error after some lines - throw on the spot
                    { 
                        throw new ApplicationException(error_text); 
                    }
                }
                if (finished)
                {
                    break;
                }
            }
            if (!JournalSrcLines.Any() & String.IsNullOrEmpty(error_text))
            {
                throw new ApplicationException(error_text);
            }
            return JournalSrcLines;
        }


        public static DateTime TryGetDateTime(string rawLine, string txtDateTime, string fieldname, string pos, int currentLine, string @format = "yyyyMMddHHmm")
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParseExact(txtDateTime, @format/*"yyyyMMddHHmm"*/, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (date == DateTime.MinValue)
            {
                throw new
                    Exception($"Line {currentLine}: {fieldname} should be {@format}  Substring({pos}) ={txtDateTime}  ");
            }

            return date;
        }



        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }


        private FullAccountingSettingPM GetFullAccountingSettings(IAccountingContext accountingContext, int tenant)
        {
            var myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
            var myFullAccountingSettingPM = myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (myFullAccountingSettingPM == null)
            {
                throw new ApplicationException("No FullAccountingSettingPM  for tenant ");
            }

            return myFullAccountingSettingPM;
        }


        private void ValidateFlatFile(int tenant)
        {
            string text;
            string text_2;
            string text_44;


            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            long count = 1;
            decimal totalCredit = 0;
			decimal totalDebit = 0;

            foreach (JournalSrcLineDTO_ISL jLine in _JournalSrcLinesDTO)
            {
                if (jLine.ActionCode != "2")
                {
                    if (String.IsNullOrEmpty(jLine.CreditGLAccount))
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Journal Line";

                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.CreditGLAccount", 0, useLocal);
                        if (String.IsNullOrEmpty(text_2)) text_2 = "Credit GLAccount Id";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                    }
                    GLAccountPM creditPM = gLAccountQueryService.GetSinglePMByDisplayNumber(jLine.CreditGLAccount, tenant);
                    if (creditPM == null)
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Journal Line";

                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.NotFound", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.CreditGLAccount", 0, useLocal);
                        if (String.IsNullOrEmpty(text_2)) text_2 = "Credit GLAccount Id";

                        this.AddErrorRow($"{text}{count} {text_2} {jLine.CreditGLAccount} {text_44}");
                    }
                    else
                    {
                        jLine.CreditGLAccountId = creditPM.Id;
                    }
                    if (jLine.DuplCheck)
                    {
                        jLine.SkipLine = CheckDuplicateRef(jLine.Reference1, jLine.CreditGLAccountId, tenant, jLine.CreditGLAccount);
                        if (jLine.SkipLine)
                            _JournalSrcLinesDTO.ForEach(jl => { if (jl != jLine && jl.Reference1 == jLine.Reference1) jl.SkipLine = true; });

                    }
                    if (!jLine.SkipLine) totalCredit += Math.Round(jLine.LocalAmount, 2);

				}
                if (!jLine.SkipLine && jLine.ActionCode != "1")
                {
                    if (String.IsNullOrEmpty(jLine.DebitGLAccount))
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Journal Line";

                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.DebitGLAccount", 0, useLocal);
                        if (String.IsNullOrEmpty(text_2)) text_2 = "Debit GLAccount Id";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                    }
                    GLAccountPM debitPM = gLAccountQueryService.GetSinglePMByDisplayNumber(jLine.DebitGLAccount, tenant);
                    if (debitPM == null)
                    {
                        text = TranslateTextsClassTranslate("JournalsCSV.O.JournalLine", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Journal Line";

                        text_44 = TranslateTextsClassTranslate("JournalsCSV.O.NotFound", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = TranslateTextsClassTranslate("JournalsCSV.O.DebitGLAccount", 0, useLocal);
                        if (String.IsNullOrEmpty(text_2)) text_2 = "Debit GLAccount Id";

                        this.AddErrorRow($"{text}{count} {text_2} {jLine.DebitGLAccount} {text_44}");
                    }
                    else
                    {
                        jLine.DebitGLAccountId = debitPM.Id;
                    }
                    totalDebit += Math.Round(jLine.LocalAmount, 2);

                }

                count++;
            }
            if(totalDebit != totalCredit) 
            {
				text = TranslateTextsClassTranslate("JournalsCSV.O.TotalCreditDebitNotEqual", 0, useLocal);
				if (String.IsNullOrEmpty(text)) text = $"Total debit lines: {totalDebit} is different from total credit lines: {totalCredit}. Please make sure that the rounded amounts are correct in the file and try again.";
                this.AddErrorRow(text);
                if (this.MyCSVFlatFileLoadResult.ValidateAccountLineList.Count > 0)
                {
                    this.AddErrorRow(string.Join(Environment.NewLine, this.MyCSVFlatFileLoadResult.ValidateAccountLineList));
                }
			}

		}

        private bool CheckDuplicateRef(string reference1, string gLAccountId, int tenant, string account)
        {
            JournalLineQueryService journalLineQueryService = new JournalLineQueryService(accountingContext);
            if (journalLineQueryService.ExistsJournalLineByReferenceCreditAccountId(reference1, gLAccountId, tenant))
            {
                string text = $"The reference: {reference1} already exists in the account: {account}.";
                this.AddAccountLineRow(text);
                this.DuplicatesSkippedCount += 1;
                return true;
            }
            return false;
        }
    }

    public class JournalAnalyseResult
    {
        public JournalPM JournalPM { get; set; }
        public int DuplicatesSkipped { get; set; }
    }

    public class CSVJournalFlatFileLoadResult_ISL
    {
        public List<string> SuccessAccountLineList = new List<string>();
        public List<string> ExceptionAccountLineList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateAccountLineList = new List<string>();
    }


    class Opening_LineDTO_JCSV_ISL
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

        internal static Opening_LineDTO_JCSV_ISL Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            bool startsWithRowTypeOk = false;
            string actualRowType = "";
            if (rawLine.Length >= 1)
            {
                actualRowType = rawLine.Split(',')[0];  
                if (RowType.Contains(actualRowType) || (actualRowType.Length >= 1 && RowType.Contains(actualRowType.Substring(0, 1)))) startsWithRowTypeOk = true;
            }

            if (!startsWithRowTypeOk || actualRowType == "")
            {
                string text = TranslateTextsClassTranslate("JournalsCSV.O.DoesntStartWithHeaderLine", 0, useLocal);
                if (String.IsNullOrEmpty(text)) text = "does not start with a Header Line";
                throw new ApplicationException($"{text} {RowType} ");
            }

            var rec = new Opening_LineDTO_JCSV_ISL();
            rec.RawLine = rawLine;
            return rec;
        }
    }




    class JournalSrcLineDTO_ISL
    {
        public static List<String> RowType = new List<String>(new string[]
            { "d", "c", "D", "C", "ז", "ח", "1", "2", "3", "DC", "CD"});

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
        public string Reference1 { get; private set; }
        public string Reference2 { get; private set; }
        public string Reference3 { get; private set; }
        public string Notes { get; private set; }
        public bool DuplCheck { get; private set; }
        public bool SkipLine { get; set; } = false;

        internal static JournalSrcLineDTO_ISL Create(string rawLine, int currentLine)
        {

            rawLine = rawLine ?? "";
            bool startsWithRowTypeOk = false;
            string actualRowType = "";
            bool is_cddc = false;
            string cddc = "";
            if (rawLine.Length >= 1)
            {

                if (rawLine.Length >= 2)
                {
                    cddc = rawLine.Substring(0, 2).ToUpperInvariant();
                    if (cddc == "CD")
                    {
                        actualRowType = "3";
                        is_cddc = true;
                        startsWithRowTypeOk = true;

                    }
                    else if (cddc == "DC")
                    {
                        actualRowType = "3";
                        is_cddc = true;
                        startsWithRowTypeOk = true;
                    }
                }
                if (!is_cddc)
                {
                    if (rawLine.Length >= 1) actualRowType = rawLine.Substring(0, 1);
                    if (RowType.Contains(actualRowType)) startsWithRowTypeOk = true;
                }
            }

            if (!startsWithRowTypeOk || actualRowType == "")
            {
                string text = TranslateTextsClassTranslate("JournalsCSV.O.DoesntStartWithCoAType", 0, useLocal);
                if (String.IsNullOrEmpty(text)) text = "does not start with an Action Type";
                throw new ApplicationException($"{text} {RowType.ToString()} ");
            }
            string orig_rawLine = rawLine;
            if (rawLine.Contains("\""))
            {
                Regex regex = new Regex("\\\"(.*?)\\\"");
                string temp = regex.Replace(rawLine, m => m.Value.Replace(',', '@'));
                rawLine = temp.Replace("@", "").Replace("\"", "");
            }
            var rec = new JournalSrcLineDTO_ISL();
            rec.RawLine = rawLine;

            string[] values = rawLine.Split(',').Select(sValue => sValue.Trim()).ToArray();
            int count = values.Count();
            if (count > 0) //rec.ActionCode = values[0];
            {
                if (is_cddc)
                {
                    rec.ActionCode = "3";
                }
                else
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
            }

            if (count > 1)
            {
                if (rec.ActionCode == "1") // credit 
                {
                    rec.CreditGLAccount = values[1].TrimStart('G');
                }
                else if (rec.ActionCode == "3" && cddc == "CD")
                {
                    rec.CreditGLAccount = values[1].TrimStart('G');
                }
                else if (rec.ActionCode == "3" && cddc == "DC")
                {
                    rec.DebitGLAccount = values[1].TrimStart('G');
                }
                else // debit
                {
                    rec.DebitGLAccount = values[1].TrimStart('G');
                }
            }
            if (count > 2)
            {
                if (rec.ActionCode == "1") // credit 
                {
                    rec.DebitGLAccount = values[2].TrimStart('G'); // opposite
                }
                else if (rec.ActionCode == "3" && cddc == "CD")
                {
                    rec.DebitGLAccount = values[2].TrimStart('G');
                }
                else if (rec.ActionCode == "3" && cddc == "DC")
                {
                    rec.CreditGLAccount = values[2].TrimStart('G');
                }
                else // debit
                {
                    rec.CreditGLAccount = values[2].TrimStart('G'); // opposite 
                }
            }
            string txtDateTime = "";
            string fieldname = "";
            string pos = "";
            DateTime date = DateTime.MinValue;

            if (count > 3)
            {
                txtDateTime = values[3].TrimEnd(' ');
                if (txtDateTime.Length >= 10)
                {
                    txtDateTime = txtDateTime.Substring(0, 10);
                    rec.AccountingDateString = txtDateTime;
                    if (rec.AccountingDateString != _EmptyDate)
                    {
                        fieldname = "AccountingDate";
                        pos = "0, 10";
                        date = JournalsCSVFlatFileAnalyser_ISL.TryGetDateTime(values[3], txtDateTime, fieldname, pos, currentLine, format: "dd.MM.yyyy");
                        rec.AccountingDate = date;
                    }
                }
                else
                {
                    if (txtDateTime.Length >= 8) txtDateTime = txtDateTime.Substring(0, 8);
                    rec.AccountingDateString = txtDateTime;
                    if (rec.AccountingDateString != _EmptyDate)
                    {
                        fieldname = "AccountingDate";
                        pos = "0, 8";
                        date = JournalsCSVFlatFileAnalyser_ISL.TryGetDateTime(values[3], txtDateTime, fieldname, pos, currentLine, format: "dd.MM.yy");
                        rec.AccountingDate = date;
                    }
                }

            }

            if (count > 4)
            {
                txtDateTime = values[4].TrimEnd(' ');
                if (txtDateTime.Length >= 10)
                {
                    txtDateTime = txtDateTime.Substring(0, 10);
                    rec.DocumentDateString = txtDateTime;
                    if (rec.DocumentDateString != _EmptyDate)
                    {
                        fieldname = "DocumentDate";
                        pos = "0, 10";
                        date = JournalsCSVFlatFileAnalyser_ISL.TryGetDateTime(values[4], txtDateTime, fieldname, pos, currentLine, format: "dd.MM.yyyy");
                        rec.DocumentDate = date;
                    }
                }
                else
                {
                    if (txtDateTime.Length >= 8) txtDateTime = txtDateTime.Substring(0, 8);
                    rec.DocumentDateString = txtDateTime;
                    if (rec.DocumentDateString != _EmptyDate)
                    {
                        fieldname = "DocumentDate";
                        pos = "0, 8";
                        date = JournalsCSVFlatFileAnalyser_ISL.TryGetDateTime(values[4], txtDateTime, fieldname, pos, currentLine, format: "dd.MM.yy");
                        rec.DocumentDate = date;
                    }
                }

            }

            if (count > 5)
            {
                txtDateTime = values[5].TrimEnd(' ');
                if (txtDateTime.Length >= 10)
                {
                    txtDateTime = txtDateTime.Substring(0, 10);
                    rec.DueDateString = txtDateTime;
                    if (rec.DueDateString != _EmptyDate)
                    {
                        fieldname = "DueDate";
                        pos = "0, 10";
                        date = JournalsCSVFlatFileAnalyser_ISL.TryGetDateTime(values[5], txtDateTime, fieldname, pos, currentLine, format: "dd.MM.yyyy");
                        rec.DueDate = date;
                    }
                }
                else
                {
                    if (txtDateTime.Length >= 8) txtDateTime = txtDateTime.Substring(0, 8);
                    rec.DueDateString = txtDateTime;
                    if (rec.DueDateString != _EmptyDate)
                    {
                        fieldname = "DueDate";
                        pos = "0, 8";
                        date = JournalsCSVFlatFileAnalyser_ISL.TryGetDateTime(values[5], txtDateTime, fieldname, pos, currentLine, format: "dd.MM.yy");
                        rec.DueDate = date;
                    }
                }


            }

            if (count > 6) rec.CurrencyCode = values[6].ToUpperInvariant();

            if (count > 7)
            {
                rec.ForeignAmount = 0M;
                try
                {
                    rec.ForeignAmount = decimal.Parse(values[7]);
                }
                catch (Exception e)
                { }
            }

            if (count > 8)
            {
                rec.LocalAmount = 0M;
                try
                {
                    rec.LocalAmount = decimal.Parse(values[8]);
                }
                catch (Exception e)
                { }
            }




            if (count > 9) rec.Reference1 = values[9];
            if (count > 10) rec.Reference2 = values[10];

            if (count > 11) rec.Notes = values[11];

            if (count > 12)
            { 
                string dupl_chk_str = values[12].TrimEnd(' ');
                if (!String.IsNullOrWhiteSpace(dupl_chk_str) && dupl_chk_str.ToUpperInvariant() == "Y") 
                {
                    rec.DuplCheck = true;
                }
                else
                {
                    rec.DuplCheck = false;
                }
            }

            return rec;
        }


    }

}
