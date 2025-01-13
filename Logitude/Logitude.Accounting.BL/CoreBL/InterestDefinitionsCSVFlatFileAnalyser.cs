using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
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
    public class InterestDefinitionsCSVFlatFileAnalyser
    {
        private Opening_LineDTO_ITDefCSV Opening_Line = null;
        private List<InterestDefinitionSrcLineDTO> _SrcLinesDTO;
        private IAccountingContext accountingContext;
        public  InterestDefinitionFlatFileLoadResult MyCSVFlatFileLoadResult = new InterestDefinitionFlatFileLoadResult();
        private const bool useLocal = true;
        private int _goodCount = 0;
        private bool _fatal = false;
        private List<InterestBasesTypeList> _bases = new List<InterestBasesTypeList>();

        public void Analyse(int? ptenant, string FileContent)
        {
            try
            {
                string fileContent = ConvertFromDosHebrewToWinHebrew(FileContent);
                int? tenantFromPage4Tester = null;
                _SrcLinesDTO = CreateDefSrcLinesDTOFromFile(FileContent, out tenantFromPage4Tester);

                if (tenantFromPage4Tester.HasValue)
                {
                    ptenant = tenantFromPage4Tester.Value;
                }
                if (!ptenant.HasValue)
                {
                    throw new ApplicationException("unable to find tenantFromPage4Tester ");
                }
                int tenant = ptenant.Value;


                accountingContext = AccountingContext.GetContext(tenant);

                InterestBasesTypeListQueryService interestBasesTypeListQueryService = new InterestBasesTypeListQueryService(accountingContext);
                _bases = interestBasesTypeListQueryService.GetList(tenant);

                ValidateFlatFile(tenant);
                if (!this._fatal && this._goodCount > 0)
                {
                    IAccountingContext MyContext = AccountingContext.GetContext(tenant);

                    GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(MyContext);
                    


                        int count = 0;

                        DateTime @now = TenantServerConfigration.GetCurrentDateTime(tenant);
                        var usrid = AuthenticationUtil.ResolveUserId(tenant);
                        GLAccountUpdateService gLAccountUpdateService = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                        foreach (InterestDefinitionSrcLineDTO defLine in _SrcLinesDTO)
                        {
                            if (!defLine.ErrorInLine) // && !defLine.AlreadyActivated)
                            {
                                count++;
                                
                                int maxline = 0;
                                bool updated = false;

                                GLAccountPM gLAccountPM = gLAccountQueryService.GetSingle(defLine.GLAccountId, true,true);
                                if (gLAccountPM.GLAccountInterestPeriods != null && gLAccountPM.GLAccountInterestPeriods.Count > 0)
                                {
                                    foreach (var period in gLAccountPM.GLAccountInterestPeriods)
                                    {
                                        if (period.LineNumber >  maxline)   maxline = period.LineNumber;
                                        if (period.PeriodStartDate == defLine.InterestActivationDate)
                                        {
                                            period.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                            period.StandardInterestRateBaseId = defLine.RegularId;
                                            period.StandardAddInterestPercent = defLine.RegularPercentage;

                                            period.ExceptionalInterestRateBaseId = defLine.ExceedingId;
                                            period.ExceptionalAddInterestPercent = defLine.ExceedingPercentage;

                                            period.CreditInterestRateBaseId = defLine.CreditId;
                                            period.CreditAddInterestPercent = defLine.CreditPercentage;

                                            period.UpdateDateTime = @now;
                                            period.UpdatedByUserId = usrid;
                                            updated = true;
                                        }
                                    }
                                }

                                if (!updated)
                                {
                                    GLAccountInterestPeriodPM newPeriodPM = new GLAccountInterestPeriodPM()
                                    {
                                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                                        GLAccountId = defLine.GLAccountId,
                                        Tenant = tenant,
                                        CreateDateTime = @now,
                                        UpdateDateTime = @now,
                                        CreatedByUserId = usrid,
                                        UpdatedByUserId = usrid,

                                        LineNumber = maxline + 1,
                                        PeriodStartDate = defLine.InterestActivationDate,

                                        StandardInterestRateBaseId = defLine.RegularId,
                                        StandardAddInterestPercent = defLine.RegularPercentage,

                                        ExceptionalInterestRateBaseId = defLine.ExceedingId,
                                        ExceptionalAddInterestPercent = defLine.ExceedingPercentage,

                                        CreditInterestRateBaseId = defLine.CreditId,
                                        CreditAddInterestPercent = defLine.CreditPercentage,
                                    };

                                    if (gLAccountPM.GLAccountInterestPeriods == null)
                                        gLAccountPM.GLAccountInterestPeriods = new List<GLAccountInterestPeriodPM>();

                                    gLAccountPM.GLAccountInterestPeriods.Add(newPeriodPM);

                                }
                                gLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                                gLAccountPM.InterestCalculationStartDate = defLine.InterestActivationDate;
                                gLAccountPM.UpdateDate = @now;
                                gLAccountPM.UpdatedByUserId = usrid;
                                gLAccountPM.InterestCreditLimit = defLine.InterestCreditLimit;
                                gLAccountPM.ActiveForInterest = true;
                                if (defLine.CreditBase != "0" && defLine.CreditPercentage != 0m) gLAccountPM.ActiveForInterestCreditInvoice = true;
                                using (var scope = TransactionFactory.GetTransaction())  // TimeSpan.FromMinutes(55)))
                                {

                                    GLAccountUpdateService updateService = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                                    gLAccountUpdateService.Update(gLAccountPM, true);
                                    scope.Complete();
                                }
                                
                            }

                        }

                        String errorLines = "";
                        if (MyCSVFlatFileLoadResult.ErrorRowList.Count > 0)
                        {
                            MyCSVFlatFileLoadResult.ErrorRowList.ForEach(item => errorLines += item.ToString() + "\n");
                        }
                        if (!String.IsNullOrEmpty(errorLines))
                        {
                            throw new ApplicationException($"{errorLines}");
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


        private List<InterestDefinitionSrcLineDTO> CreateDefSrcLinesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            //       bool reading_Lines = false;
            bool finished = false;
            var interestSrcLines = new List<InterestDefinitionSrcLineDTO>();
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

                string accountingCurrencyId = "";
                if (tenant.HasValue)
                {
                    TenantQuery tenantQuery = new TenantQuery(tenant.Value);
                    TenantPM tPM = tenantQuery.GetSinglePM(tenant.Value);
                    accountingCurrencyId = tPM.CurrencyId;
                }
                InterestDefinitionSrcLineDTO interestLine = InterestDefinitionSrcLineDTO.Create(rawLine, accountingCurrencyId);
                interestSrcLines.Add(interestLine);

                if (finished)
                {
                    break;
                }
            }
            return interestSrcLines;
        }


        public static DateTime TryGetDateTime(string rawLine, string txtDateTime, string fieldname, string pos, string @format = "dd.MM.yyyy")
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParseExact(txtDateTime, @format/*"dd.MM.yyyy"*/, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (date == DateTime.MinValue)
            {
                throw new
                    Exception($"{fieldname} should be in the {@format} format, while Substring({pos}) ={rawLine}  ");
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
            JournalQueryService journalQueryService = new JournalQueryService(tenant);
            InterestTransactionQueryService itQueryService = new InterestTransactionQueryService(tenant);

            long count = 1;
            foreach (InterestDefinitionSrcLineDTO defLine in _SrcLinesDTO)
            {
                if (!defLine.ErrorInLine)
                {
                    // GLAccount 
                    if (String.IsNullOrEmpty(defLine.GLAccount))
                    {
                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.GLAccount", 0, useLocal);
                        if (String.IsNullOrEmpty(text_2)) text_2 = "GLAccount Id";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                        defLine.ErrorInLine = true;
                    }
                    else
                    {
                        GLAccountPM glaccountPM = gLAccountQueryService.GetSinglePMByDisplayNumber(defLine.GLAccount, tenant);
                        if (glaccountPM == null)
                        {
                            text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                            if (String.IsNullOrEmpty(text)) text = "Line";

                            text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotFound", 0, useLocal);
                            if (String.IsNullOrEmpty(text_44)) text_44 = "not found";

                            text_2 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.GLAccount", 0, useLocal);
                            if (String.IsNullOrEmpty(text_2)) text_2 = "GLAccount Id";

                            this.AddErrorRow($"{text}{count} {text_2} {defLine.GLAccount} {text_44}");
                            defLine.ErrorInLine = true;
                        }
                        else
                        {
                            defLine.GLAccountId = glaccountPM.Id;
                            defLine.AlreadyActivated = glaccountPM.ActiveForInterest;
                        }
                    }

                    // RegularBase
                    if (String.IsNullOrEmpty(defLine.RegularBase))
                    {
                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = "Regular Percentage Base";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                        defLine.ErrorInLine = true;

                    }
                    else if (!_bases.Any(b => b.Code == defLine.RegularBase))
                    {

                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotFound", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "not found";

                        text_2 = "Percentage Base";

                        this.AddErrorRow($"{text}{count} {text_2} {defLine.RegularBase} {text_44}");
                        defLine.ErrorInLine = true;
                    }
                    else
                    {
                        defLine.RegularId = _bases.Where(b => b.Code == defLine.RegularBase).FirstOrDefault().Id;
                    }


                    // ExceedingBase
                    if (String.IsNullOrEmpty(defLine.ExceedingBase))
                    {
                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = "Exceeding Percentage Base";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                        defLine.ErrorInLine = true;

                    }
                    else if (!_bases.Any(b => b.Code == defLine.ExceedingBase))
                    {

                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotFound", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "not found";

                        text_2 = "Percentage Base";

                        this.AddErrorRow($"{text}{count} {text_2} {defLine.ExceedingBase} {text_44}");
                        defLine.ErrorInLine = true;
                    }
                    else
                    {
                        defLine.ExceedingId = _bases.Where(b => b.Code == defLine.ExceedingBase).FirstOrDefault().Id;
                    }


                    // CreditBase
                    if (String.IsNullOrEmpty(defLine.CreditBase))
                    {
                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.IsMissing", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "is missing";

                        text_2 = "Credit Percentage Base";

                        this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                        defLine.ErrorInLine = true;

                    }
                    else if (!_bases.Any(b => b.Code == defLine.CreditBase))
                    {

                        text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.Line", 0, useLocal);
                        if (String.IsNullOrEmpty(text)) text = "Line";

                        text_44 = TranslateTextsClassTranslate("InterestTransactionsCSV.O.NotFound", 0, useLocal);
                        if (String.IsNullOrEmpty(text_44)) text_44 = "not found";

                        text_2 = "Percentage Base";

                        this.AddErrorRow($"{text}{count} {text_2} {defLine.CreditBase} {text_44}");
                        defLine.ErrorInLine = true;
                    }
                    else
                    {
                        defLine.CreditId = _bases.Where(b => b.Code == defLine.CreditBase).FirstOrDefault().Id;
                    }

                }
                count++;
                if (!defLine.ErrorInLine)
                    this._goodCount++;
            }

        }



    }


    public class InterestDefinitionFlatFileLoadResult
    {
        public List<string> SuccessAccountLineList = new List<string>();
        public List<string> ExceptionAccountLineList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateAccountLineLineAgainstDBErrors = new List<string>();
    }


    class Opening_LineDTO_ITDefCSV
    {
        //public const string RowType = "ס"; // סוג כרטיס,... 
        //    public static List<String> RowType = new List<String>(new string[]
        //        { "ס", "A", "DebitCredit"});
        public string RawLine { get; set; }
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        internal static Opening_LineDTO_ITDefCSV Create(string rawLine)
        {

            //        rawLine = rawLine ?? "";
            //        bool startsWithRowTypeOk = false;
            //        string actualRowType = "";
            //        if (rawLine.Length >= 1)
            //       {
            //           actualRowType = rawLine.Split(',')[0]; ////rawLine.Substring(0, 1);
            //           if (RowType.Contains(actualRowType) || (actualRowType.Length >= 1 && RowType.Contains(actualRowType.Substring(0, 1)))) startsWithRowTypeOk = true;
            //       }

            //        if (!startsWithRowTypeOk || actualRowType == "")
            //        {
            //            string text = TranslateTextsClassTranslate("InterestTransactionsCSV.O.DoesntStartWithHeaderLine", 0, useLocal);
            //            if (String.IsNullOrEmpty(text)) text = "does not start with a Header Line";
            //            throw new ApplicationException($"{text} {RowType} ");
            //       }

            var rec = new Opening_LineDTO_ITDefCSV();
            rec.RawLine = rawLine;
            return rec;
        }
    }






    class InterestDefinitionSrcLineDTO
    {
        //      public static List<String> RowType = new List<String>(new string[]
        //          { "d", "c", "D", "C", "ז", "ח", "1", "2", "3"});

        public const string _EmptyDate = "00000000";
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public string RawLine { get; set; }
        public DateTime InterestActivationDate { get; private set; }
        public string InterestActivationDateString { get; private set; }
        public string RegularBase { get; private set; }
        public string ExceedingBase { get; private set; }
        public string CreditBase { get; private set; }
        public string RegularId { get; set; }
        public string ExceedingId { get; set; }
        public string CreditId { get; set; }
        public string GLAccount { get; private set; }  // DisplayNumber
        public string GLAccountId { get; set; }
        public decimal InterestCreditLimit { get; private set; }
        public decimal RegularPercentage { get; private set; }
        public decimal ExceedingPercentage { get; private set; }
        public decimal CreditPercentage { get; private set; }
        public bool ErrorInLine { get; set; }
        public bool AlreadyActivated { get; set; }



        internal static InterestDefinitionSrcLineDTO Create(string rawLine, string accountingCurrencyId)
        {

            rawLine = rawLine ?? "";
            string orig_rawLine = rawLine;
            if (rawLine.Contains("\""))
            {
                Regex regex = new Regex("\\\"(.*?)\\\"");
                string temp = regex.Replace(rawLine, m => m.Value.Replace(',', '@'));
                rawLine = temp.Replace("@", "").Replace("\"", "");
            }

            var rec = new InterestDefinitionSrcLineDTO();
            rec.RawLine = rawLine;
            rec.ErrorInLine = false;

            string[] values = rawLine.Split(',').Select(sValue => sValue.Trim()).ToArray();
            int count = values.Count();


            if (count > 0)
            {
                rec.GLAccount = values[0].TrimStart('H'); // DisplayNumber 
            }





            string txtDateTime = "";
            string fieldname = "";
            string pos = "";
            DateTime date = DateTime.MinValue;

            if (count > 1)
            {
                txtDateTime = values[1].TrimEnd(' ').Replace('/', '.');
                if (txtDateTime.Length >= 10)
                {
                    txtDateTime = txtDateTime.Substring(0, 10);
                    rec.InterestActivationDateString = txtDateTime;
                    if (rec.InterestActivationDateString != _EmptyDate)
                    {
                        fieldname = "InterestActivationDate";
                        pos = "0, 10";
                        date = InterestDefinitionsCSVFlatFileAnalyser.TryGetDateTime(values[2], txtDateTime, fieldname, pos, format: "dd.MM.yyyy");
                        rec.InterestActivationDate = date;
                    }
                }
                else
                {
                    if (txtDateTime.Length >= 8) txtDateTime = txtDateTime.Substring(0, 8);
                    rec.InterestActivationDateString = txtDateTime;
                    if (rec.InterestActivationDateString != _EmptyDate)
                    {
                        fieldname = "InterestActivationDate";
                        pos = "0, 8";
                        date = InterestDefinitionsCSVFlatFileAnalyser.TryGetDateTime(values[2], txtDateTime, fieldname, pos, format: "dd.MM.yy");
                        rec.InterestActivationDate = date;
                    }
                }
            }




            if (count > 2)
            {
                rec.InterestCreditLimit = 0M;
                try
                {
                    rec.InterestCreditLimit = decimal.Parse(values[2]);
                }
                catch (Exception e)
                { }
            }

            if (count > 3)
            {
                rec.RegularBase = values[3];
            }

            if (count > 4)
            {
                rec.RegularPercentage = 0M;
                try
                {
                    rec.RegularPercentage = decimal.Parse(values[4]);
                }
                catch (Exception e)
                { }
            }


            if (count > 5)
            {
                rec.ExceedingBase = values[5];
            }

            if (count > 6)
            {
                rec.ExceedingPercentage = 0M;
                try
                {
                    rec.ExceedingPercentage = decimal.Parse(values[6]);
                }
                catch (Exception e)
                { }
            }



            if (count > 7)
            {
                rec.CreditBase = values[7];
            }

            if (count > 8)
            {
                rec.CreditPercentage = 0M;
                try
                {
                    rec.CreditPercentage = decimal.Parse(values[8]);
                }
                catch (Exception e)
                { }
            }


            return rec;
        }


    }

}
