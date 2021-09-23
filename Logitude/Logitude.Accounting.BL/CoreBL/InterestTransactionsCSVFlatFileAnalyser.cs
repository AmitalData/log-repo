using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
using System.Text.RegularExpressions;

namespace Logitude.Accounting.BL.CoreBL
{
    public class InterestTransactionsCSVFlatFileAnalyser
    {
        private Opening_LineDTO_ITCSV Opening_Line = null;
        // private Closing_LineDTO Closing_Line = null;
        private List<InterestTransactionSrcLineDTO> _SrcLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IAccountingContext accountingContext;
        public InterestTransactionFlatFileLoadResult MyCSVFlatFileLoadResult = new InterestTransactionFlatFileLoadResult();
        private ContactRepository _contactRep;
        private string _resolveLoggingUserId;
        private Contact _contact;
        private const bool useLocal = true;
        private int _goodCount = 0;
        private bool _fatal = false;

        public void Analyse(int? ptenant, string FileContent)
        {
            try
            {
                string fileContent = ConvertFromDosHebrewToWinHebrew(FileContent);
                int? tenantFromPage4Tester = null;
                //     FileContent = fileContent.Replace("\"", "");
                _SrcLinesDTO = CreateJournalSrcLinesDTOFromFile(FileContent, out tenantFromPage4Tester);

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
                if (!this._fatal)
                {
                    IAccountingContext MyContext = AccountingContext.GetContext(tenant);

                    using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(25)))
                    {
                        int count = 0;

                        DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);
                        var usrid = AuthenticationUtil.ResolveUserId(tenant);
                        InterestTransactionSrcLineDTO j1stLineDTO = _SrcLinesDTO.FirstOrDefault();
                        


                        foreach (InterestTransactionSrcLineDTO itLine in _SrcLinesDTO)
                        {
                            count++;
                            bool errors = false;

                            InterestTransactionPM newItPM = new InterestTransactionPM()
                            {
                                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                Tenant = tenant,

                                CreateDateTime = @now,
                                UpdateDateTime = @now,

                                GLAccountId = itLine.GLAccountId,
                                InterestEntityTypeCode = "3", //(Journal)
                                EntityId = itLine.JournalId,
                                OriginalEntityLineNumber = itLine.JournalLineNumber,

                                AccountEntityCode = null,
                                ForeignAmount = itLine.ForeignAmount,
                                LocalAmount = itLine.LocalAmount,
                                CurrencyId = itLine.CurrencyId,

                                InterestValueDate = itLine.InterestValueDate,
                                IsCancelled = false,
                                IsClosed = false,
                            };







                            var itSvc = new InterestTransactionUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                            itSvc.Update(newItPM, true);

                        }

                        if (MyCSVFlatFileLoadResult.ErrorRowList.Count > 0)
                        {
                            string text_1 = MyCSVFlatFileLoadResult.ErrorRowList.FirstOrDefault();
                            throw new ApplicationException($"{text_1}");
                        }
                        //    if (MyFlatFileLoadResult.ExceptionVendorList.Count > 0)
                        //    {
                        //        string text = MyFlatFileLoadResult.ExceptionVendorList.FirstOrDefault();
                        //        throw new ApplicationException($"{text}");
                        //    }
                        scope.Complete();

                    }
                }
                else
                {
                    String errorLines = "";
                    MyCSVFlatFileLoadResult.ErrorRowList.ForEach(item => errorLines += item.ToString() + "\n");
                    throw new ApplicationException($"{errorLines}");
                }

            }
            catch (Exception e)
            {
                string text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.FailedWhilePerforming", 0, useLocal);
                if (String.IsNullOrEmpty(text)) text = "failed while performing";

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


        private List<InterestTransactionSrcLineDTO> CreateJournalSrcLinesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            bool reading_Lines = false;
            bool finished = false;
            var interestSrcLines = new List<InterestTransactionSrcLineDTO>();
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
                var rowtype = rawLine.Split(',')[0];///.Substring(0, 1);

                if (!reading_Lines)
                {
                    reading_Lines = true;
                    continue;
                }
                else if (InterestTransactionSrcLineDTO.RowType.Contains(rowtype))
                {
                    if (!reading_Lines)
                    {
                        reading_Lines = true;
                    }
                    string accountingCurrencyId = "";
                    if (tenant.HasValue)
                    {
                        TenantQuery tenantQuery = new TenantQuery(tenant.Value);
                        TenantPM tPM = tenantQuery.GetSinglePM(tenant.Value);
                        accountingCurrencyId = tPM.CurrencyId;
                    }
                    InterestTransactionSrcLineDTO interestLine = InterestTransactionSrcLineDTO.Create(rawLine, accountingCurrencyId);
                    interestSrcLines.Add(interestLine);

                }
                else
                {
                    string text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotValidRowType", 0, useLocal);
                    if (String.IsNullOrEmpty(text)) text = "Not a valid Row Type";
                    throw new ApplicationException($"{text}  {rawLine}");
                }
                if (finished)
                {
                    break;
                }
            }
            return interestSrcLines;
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
            //    text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.DeductionFileNumber", 0, useLocal);
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

            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            InterestTransactionQueryService itQueryService = new InterestTransactionQueryService(tenant);

            long count = 1;
            foreach (InterestTransactionSrcLineDTO itLine in _SrcLinesDTO)
            {
                if (!itLine.ErrorInLine)
                {
                    if (String.IsNullOrEmpty(itLine.GLAccount))
                    {
                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.GLAccount", 0, useLocal);
                        if (String.IsNullOrEmpty(text_2)) text_2 = "GLAccount Id";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                        itLine.ErrorInLine = true;
                    }
                    else
                    {
                        GLAccountPM glaccountPM = gLAccountQueryService.GetSinglePMByDisplayNumber(itLine.GLAccount, tenant);
                        if (glaccountPM == null)
                        {
                            text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                            if (String.IsNullOrEmpty(text)) text = "Line";

                            text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotFound", 0, useLocal);
                            if (String.IsNullOrEmpty(text_44)) text_44 = "not found";

                            text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.GLAccount", 0, useLocal);
                            if (String.IsNullOrEmpty(text_2)) text_2 = "GLAccount Id";

                            this.AddErrorRow($"{text}{count} {text_2} {itLine.GLAccount} {text_44}");
                            itLine.ErrorInLine = true;
                        }
                        else
                        {
                            itLine.GLAccountId = glaccountPM.Id;
                        }
                    }
                    if (String.IsNullOrEmpty(itLine.ExternalNumber))
                    {
                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.JExternalNumber", 0, useLocal);
                        if (String.IsNullOrEmpty(text_2)) text_2 = "External Journal Number";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                        itLine.ErrorInLine = true;

                    }
                    else if (!String.IsNullOrEmpty(itLine.GLAccountId))
                    {
                        JournalPM journalPM = journalQueryService.GetSingleJournalByExternalNoAndExternalSystem(itLine.ExternalNumber, "AMITAL", tenant); // have to change that to YEAR:EX:TERNAL
                        if (journalPM == null)
                        {
                            text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                            if (String.IsNullOrEmpty(text)) text = "Line";

                            text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotFound", 0, useLocal);
                            if (String.IsNullOrEmpty(text_44)) text_44 = "not found";

                            text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Journal", 0, useLocal);
                            if (String.IsNullOrEmpty(text_2)) text_2 = "Journal";

                            this.AddErrorRow($"{text}{count} {text_2} {itLine.ExternalNumber} {text_44}");
                            itLine.ErrorInLine = true;
                        }
                        else
                        {
                            itLine.JournalId = journalPM.Id;
                            bool lineFound = false;
                            JournalLinePM jlPM = null;
                            foreach (JournalLinePM journalLine in journalPM.JournalLines)
                            {
                                if ((journalLine.ActionCode == "1" || journalLine.ActionCode == "3") && itLine.CreditAmount != 0m && journalLine.CreditAccountId == itLine.GLAccountId
                                    && (journalLine.Reference1 == itLine.Rererence || journalLine.Reference2 == itLine.Rererence || String.IsNullOrWhiteSpace(itLine.Rererence)))
                                {
                                    lineFound = true;
                                    jlPM = journalLine;
                                    break;
                                }
                                else if ((journalLine.ActionCode == "2" || journalLine.ActionCode == "3") && itLine.DebitAmount != 0m && journalLine.DebitAccountId == itLine.GLAccountId
                                    && (journalLine.Reference1 == itLine.Rererence || journalLine.Reference2 == itLine.Rererence || String.IsNullOrWhiteSpace(itLine.Rererence)))
                                {
                                    lineFound = true;
                                    jlPM = journalLine;
                                    break;
                                }
                            }
                            if (!lineFound)
                            {
                                text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                                if (String.IsNullOrEmpty(text)) text = "Line";

                                text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotFound", 0, useLocal);
                                if (String.IsNullOrEmpty(text_44)) text_44 = "not found";

                                text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.JournalLine", 0, useLocal);
                                if (String.IsNullOrEmpty(text_2)) text_2 = "Journal Line";

                                this.AddErrorRow($"{text}{count} {text_2} {itLine.ExternalNumber} {text_44}");
                                itLine.ErrorInLine = true;
                            }
                            else if (!itLine.ErrorInLine)
                            {
                                itLine.JournalId = jlPM.JournalId;
                                itLine.JournalLineNumber = jlPM.Line;
                                InterestTransactionUniqueConstraintFields uniqueConstraintFields = new InterestTransactionUniqueConstraintFields()
                                {
                                    GLAccountId = itLine.GLAccountId,
                                    Tenant = tenant,
                                    InterestEntityTypeCode = "3", //(Journal)
                                    EntityId = jlPM.JournalId,
                                    OriginalEntityLineNumber = jlPM.Line,
                                };
                                InterestTransactionPM itPM = itQueryService.GetTransactionByUniqueConstraintFields(uniqueConstraintFields);
                                if (itPM != null)
                                {
                                    text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                                    if (String.IsNullOrEmpty(text)) text = "Line";

                                    text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.AlreadyFound", 0, useLocal);
                                    if (String.IsNullOrEmpty(text_44)) text_44 = "already found";

                                    text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.TransactionLine", 0, useLocal);
                                    if (String.IsNullOrEmpty(text_2)) text_2 = "Journal Line";

                                    this.AddErrorRow($"{text}{count} {text_2} {itLine.ExternalNumber} {text_44} ({itPM.LocalAmount} ID={itPM.Id})");
                                    itLine.ErrorInLine = true;

                                }
                            }

                        }
                    }
                }
                count++;
                if (!itLine.ErrorInLine)
                    this._goodCount++;
            }

        }



    }


    public class InterestTransactionFlatFileLoadResult
    {
        public List<string> SuccessAccountLineList = new List<string>();
        public List<string> ExceptionAccountLineList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateAccountLineLineAgainstDBErrors = new List<string>();
    }


    class Opening_LineDTO_ITCSV
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

        internal static Opening_LineDTO_ITCSV Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            bool startsWithRowTypeOk = false;
            string actualRowType = "";
            if (rawLine.Length >= 1)
            {
                actualRowType = rawLine.Split(',')[0]; ////rawLine.Substring(0, 1);
                if (RowType.Contains(actualRowType) || (actualRowType.Length >= 1 && RowType.Contains(actualRowType.Substring(0, 1)))) startsWithRowTypeOk = true;
            }

            if (!startsWithRowTypeOk || actualRowType == "")
            {
                string text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.DoesntStartWithHeaderLine", 0, useLocal);
                if (String.IsNullOrEmpty(text)) text = "does not start with a Header Line";
                throw new ApplicationException($"{text} {RowType} ");
            }

            var rec = new Opening_LineDTO_ITCSV();
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
    //            string text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.DoesntStartWithRowType", 0, useLocal);
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




    class InterestTransactionSrcLineDTO
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


        public DateTime InterestValueDate { get; private set; }
        public DateTime AccountingDate { get; private set; }
        public string InterestValueDateString { get; private set; }
        public string AccountingDateString { get; private set; }

        public string JournalId { get; set; }

        public string CurrencyId { get; private set; }

        public string GLAccount { get; private set; }
        public string GLAccountName { get; set; }
        public string GLAccountId { get; set; }
        public decimal DebitAmount { get; private set; }
        public decimal CreditAmount { get; private set; }
        public decimal LocalAmount { get; private set; }
        public decimal ForeignAmount { get; private set; }
        public string Rererence { get; private set; }
        public string Remarks { get; private set; }
        public string ExternalNumber { get; private set; }

        public bool ErrorInLine { get;  set; }

        public int JournalLineNumber { get; set; }


        internal static InterestTransactionSrcLineDTO Create(string rawLine, string accountingCurrencyId)
        { 

            rawLine = rawLine ?? "";
            string orig_rawLine = rawLine;
            if (rawLine.Contains("\""))
            {
                Regex regex = new Regex("\\\"(.*?)\\\"");
                string temp = regex.Replace(rawLine, m => m.Value.Replace(',', '@'));
                rawLine = temp.Replace("@", "").Replace("\"", "");
            }

            var rec = new InterestTransactionSrcLineDTO();
            rec.RawLine = rawLine;
            rec.ErrorInLine = false;

            string[] values = rawLine.Split(',').Select(sValue => sValue.Trim()).ToArray();
            int count = values.Count();


            if (count > 0)
            {
                rec.GLAccount = values[0].TrimStart('H'); //.TrimStart('G');
            }


            if (count > 1)
            {
                rec.GLAccountName = values[1];
            }



            string txtDateTime = "";
            string fieldname = "";
            string pos = "";
            DateTime date = DateTime.MinValue;

            if (count > 2)
            {
                txtDateTime = values[2].TrimEnd(' ');
                if (txtDateTime.Length >= 10)
                {
                    txtDateTime = txtDateTime.Substring(0, 10);
                    rec.InterestValueDateString = txtDateTime;
                    if (rec.InterestValueDateString != _EmptyDate)
                    {
                        fieldname = "InterestValueDate";
                        pos = "0, 10";
                        date = InterestTransactionsCSVFlatFileAnalyser.TryGetDateTime(values[2], txtDateTime, fieldname, pos, format: "dd.MM.yyyy");
                        rec.InterestValueDate = date;
                    }
                }
                else
                {
                    if (txtDateTime.Length >= 8) txtDateTime = txtDateTime.Substring(0, 8);
                    rec.InterestValueDateString = txtDateTime;
                    if (rec.InterestValueDateString != _EmptyDate)
                    {
                        fieldname = "InterestValueDate";
                        pos = "0, 8";
                        date = InterestTransactionsCSVFlatFileAnalyser.TryGetDateTime(values[2], txtDateTime, fieldname, pos, format: "dd.MM.yy");
                        rec.InterestValueDate = date;
                    }
                }
            }




            if (count > 3)
            {
                rec.DebitAmount = 0M;
                try
                {
                    rec.DebitAmount = decimal.Parse(values[3]);
                }
                catch (Exception e)
                { }
            }

            if (count > 4)
            {
                rec.CreditAmount = 0M;
                try
                {
                    rec.CreditAmount = decimal.Parse(values[4]);
                }
                catch (Exception e)
                { }
            }

            rec.LocalAmount = rec.DebitAmount - rec.CreditAmount;
            rec.ForeignAmount = rec.DebitAmount - rec.CreditAmount;
            rec.CurrencyId = accountingCurrencyId;



            if (count > 5)
            {
                rec.Remarks = values[5];
            }

            if (count > 6)
            {
                rec.Rererence = values[6];
            }

            if (count > 7)
            {
                rec.ExternalNumber = values[7];
            }


            if (count > 8)
            {
                txtDateTime = values[8].TrimEnd(' ');
                if (txtDateTime.Length >= 10)
                {
                    txtDateTime = txtDateTime.Substring(0, 10);
                    rec.AccountingDateString = txtDateTime;
                    if (rec.AccountingDateString != _EmptyDate)
                    {
                        fieldname = "AccountingDate";
                        pos = "0, 10";
                        date = InterestTransactionsCSVFlatFileAnalyser.TryGetDateTime(values[2], txtDateTime, fieldname, pos, format: "dd.MM.yyyy");
                        rec.InterestValueDate = date;
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
                        date = InterestTransactionsCSVFlatFileAnalyser.TryGetDateTime(values[2], txtDateTime, fieldname, pos, format: "dd.MM.yy");
                        rec.InterestValueDate = date;
                    }
                }

                string yyyy = rec.InterestValueDate.Year.ToString();
                rec.ExternalNumber = yyyy + ":" + rec.ExternalNumber;
            }



            return rec;
        }


    }

}
