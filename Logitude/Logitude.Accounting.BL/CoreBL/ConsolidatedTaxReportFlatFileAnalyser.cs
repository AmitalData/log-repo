using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ConsolidatedTaxReportFlatFileAnalyser
    {
        private OpeningLineDTO OpeningLine = null;
        private ClosingLineDTO ClosingLine = null;
        private List<TaxReportLineDTO> _TaxReportLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IAccountingContext accountingContext;
        public  FlatFileLoadResult MyFlatFileLoadResult = new FlatFileLoadResult();
        private ContactRepository _contactRep;
        private string _resolveLoggingUserId;
        private Contact _contact;
        private  bool useLocal = true;
        private TaxReportPM MyTaxReportPM;


        public void Analyse(int? ptenant, string pTaxReportId, string FileContent)
        {
            try
            {
                useLocal = !(GetLoggedContact(ptenant.Value).DontShowLocal);
                FileContent = ConvertFromDosHebrewToWinHebrew(FileContent);
                int? tenantFromPage4Tester = null;
                string taxReportIdFromPage4Tester = "";
                _TaxReportLinesDTO = CreateTaxReportLinesDTOFromFile(FileContent, out tenantFromPage4Tester, out taxReportIdFromPage4Tester);

                if (tenantFromPage4Tester.HasValue)
                {
                    ptenant = tenantFromPage4Tester.Value;
                }
                if (!ptenant.HasValue)
                {
                    throw new ApplicationException("unable to find tenantFromPage4Tester ");
                }
                int tenant = ptenant.Value;

                if (!String.IsNullOrWhiteSpace(taxReportIdFromPage4Tester))
                {
                    pTaxReportId = taxReportIdFromPage4Tester;
                }
                if (String.IsNullOrWhiteSpace(pTaxReportId))
                {
                    throw new ApplicationException("unable to find taxReportIdFromPage4Tester ");
                }
                string taxReportId = pTaxReportId.Trim();
                _contactRep = new ContactRepository(tenant);
                _resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
                _contact = _contactRep.GetSingleContactByEmail(_resolveLoggingUserId, tenant);
                accountingContext = AccountingContext.GetContext(tenant);
                _FullAccountingSettingPM = GetFullAccountingSettings(accountingContext, tenant);
                ValidateFlatFile();

                IAccountingContext MyContext = AccountingContext.GetContext(tenant);
                TaxReportQueryService taxReportQueryService = new TaxReportQueryService(MyContext);
                MyTaxReportPM = taxReportQueryService.GetSingle(taxReportId, true, false);
                if (MyTaxReportPM == null)
                {
                    string text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.NotFound", 0, useLocal);
                    string text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.TaxReportId", 0, useLocal);
                    throw new ApplicationException($"{text_2} {taxReportId} {text_44} ");
                }
                if (MyTaxReportPM.StatusCode != "D" && MyTaxReportPM.StatusCode != "E")
                {
                    string text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.StatusError", 0, useLocal);
                    string text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.TaxReportId", 0, useLocal);
                    throw new ApplicationException($"{text_2} {taxReportId} {text_44} ");
                }
                int maxLine = 0;
                TaxReportLineListQueryService taxReportLineListQueryService = new TaxReportLineListQueryService(MyContext);
                List<TaxReportLineList> lines = taxReportLineListQueryService.GetReportLines(taxReportId, tenant).ToList();
                if (lines != null && lines.Count > 0)
                {
                    lines= lines.Where(d => !d.IsExternalLine).ToList();
                    maxLine = lines.Count > 0 ? lines.Max(l => l.Line) : 1;

                }

                int nextLine = maxLine;
                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(25)))
                {
                    List<TaxReportLinePM> newLines = new List<TaxReportLinePM>();
                    foreach (TaxReportLineDTO taxLineDTO in _TaxReportLinesDTO)
                    {
                        nextLine++;

                        decimal? v_vatable = null;
                        decimal? v_total;
                        switch (taxLineDTO.LineTypeCode)
                        {
                            case "S":
                                v_total = taxLineDTO.VatableInvoiceAmount;
                                if (taxLineDTO.VatAmount != 0m) v_vatable = taxLineDTO.VatableInvoiceAmount;
                                break;
                            case "M":
                            case "I":
                               // v_vatable = taxLineDTO.VatableInvoiceAmount;
                               // v_total = taxLineDTO.VatableInvoiceAmount + taxLineDTO.VatAmount;
                                v_total = taxLineDTO.VatableInvoiceAmount;
                                if (taxLineDTO.VatAmount != 0m) v_vatable = taxLineDTO.VatableInvoiceAmount;
                                break;
                            default:
                                v_total = taxLineDTO.VatableInvoiceAmount;
                                break;
                        }
                        if (!v_vatable.HasValue) v_vatable = 0m;
                        if (!v_total.HasValue) v_total = 0m;
                        var taxReportLine = new TaxReportLinePM()
                        {
                            ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                            Tenant = tenant,
                            TaxReportId = taxReportId,
                            Line = nextLine,
                            IsExternalLine = true,
                            LineTypeCode = taxLineDTO.LineTypeCode,
                            Reference = taxLineDTO.Reference,
                            ReferenceDate = taxLineDTO.ReferenceDate,
                            VatNumber = taxLineDTO.VatNumber,
                         //   VatableInvoiceAmount = taxLineDTO.VatableInvoiceAmount,
                            VatableInvoiceAmount = v_vatable,
                            VatAmount = taxLineDTO.VatAmount,
                          //  TotalInvoiceAmount = taxLineDTO.VatableInvoiceAmount + taxLineDTO.VatAmount,
                            TotalInvoiceAmount = v_total,
                            OutputOrInput = taxLineDTO.OutputOrInput,
                            ReferecneGroup = taxLineDTO.ReferenceGroup,
                            UpdatedByUserId = _contact.Id,
                            //JournalId = "1-1027720",
                            TransmitStatusCode = "1",
                            IsEquipment = false,
                            LastUpdateDateTime = DateTime.Now,
                            TaxReportDate = MyTaxReportPM.TaxReportMonth,
                        };
                        newLines.Add(taxReportLine);
                    } 

                    if (newLines.Count == 0)
                    {
                        string text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.NoLinesProcessed", 0, useLocal);
                        throw new ApplicationException($"{text_44}");
                    }
                    TaxReportLineQueryService taxReportLineQueryService = new TaxReportLineQueryService(MyContext);
                    List<TaxReportLinePM> externalLines = taxReportLineQueryService.GetAllExternalLines(tenant, taxReportId);
                    if (externalLines != null && externalLines.Count > 0)
                    {
                        externalLines.ForEach(line => { line.ChangeSetOp = ChangeSetOperation.Delete; });
                    }
                  
                    TaxReportLineUpdateService taxReportLineUpdateService = new TaxReportLineUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    taxReportLineUpdateService.UpdateMulti(new List<TaxReportLinePM>(), externalLines, MyTaxReportPM, true);
                    taxReportLineUpdateService.UpdateMulti(newLines, new List<TaxReportLinePM>(), MyTaxReportPM, true);

                    TaxReportPM RefreshedTaxReportPM = taxReportQueryService.GetSingle(taxReportId, true, false);
                    RefreshedTaxReportPM.ChangeSetOp = ChangeSetOperation.Update;

                    TaxReportUpdateService taxReportUpdateService = new TaxReportUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    taxReportUpdateService.Update(RefreshedTaxReportPM, true);


                    if (MyFlatFileLoadResult.ErrorRowList.Count > 0)
                    {
                        string text = MyFlatFileLoadResult.ErrorRowList.FirstOrDefault();
                        throw new ApplicationException($"{text}");
                    }
                    //    if (MyFlatFileLoadResult.ExceptionVendorList.Count > 0)
                    //    {
                    //        string text = MyFlatFileLoadResult.ExceptionVendorList.FirstOrDefault();
                    //        throw new ApplicationException($"{text}");
                    //    }
                    scope.Complete();


                }

            }
            catch (Exception e)
            {
                string text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.FailedWhilePerforming", 0, useLocal);

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


        public Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        private ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }


            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
        private void AddErrorRow(String errorLine)
        {

            this.MyFlatFileLoadResult.ErrorRowList.Add(errorLine + " " + Environment.NewLine);

        }


        private List<TaxReportLineDTO> CreateTaxReportLinesDTOFromFile(string FileContent, out int? tenant, out string taxReportId)
        {
            tenant = null;
            taxReportId = "";
            bool readingLines = false;
            bool finished = false;
            var TaxReportLines = new List<TaxReportLineDTO>();
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
                    else if (rawLine.StartsWith("//ReportId="))//for tester 
                    {
                        string taxReportIdS = rawLine.Split(new string[] { "//ReportId=" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        taxReportId = taxReportIdS.Trim();
                    }
                    continue;// remark do nothing ...
                }
                var rowtype = rawLine.Substring(0, 1);
                switch (rowtype)
                {
                    case OpeningLineDTO.RowType:
                        {
                            OpeningLine = OpeningLineDTO.Create(rawLine);
                            readingLines = true;
                        }
                        break;

                    case ClosingLineDTO.RowType:
                        {
                            ClosingLine = ClosingLineDTO.Create(rawLine);
                            finished = true;
                        }
                        break;

                    default:
                        if (TaxReportLineDTO.RowType.Contains(rowtype))
                        {
                            if (!readingLines)
                            {

                                string text_3 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.TaxLineRowType", 0, useLocal);
                                string text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.AppearsBefore", 0, useLocal);
                                string text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.StartingRowType", 0, useLocal);
                                throw new ApplicationException($"{text_3} {rowtype} {text_44} {text_2} {OpeningLineDTO.RowType} ");
                            }
                            TaxReportLineDTO taxLine = TaxReportLineDTO.Create(rawLine);
                            TaxReportLines.Add(taxLine);

                        }
                        else
                        {
                            string text = TranslateTextsClassTranslate("TaxReport.O.NotValidRowType", 0, useLocal);
                            throw new ApplicationException($"{text} ");
                        }
                        break;
                }
                if (finished)
                {
                    break;
                }
            }
            return TaxReportLines;
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
            //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.DeductionFileNumber", 0, useLocal);
            //    // Deduction File Number is undefined.
            //    throw new ApplicationException(text);
            //}
            return myFullAccountingSettingPM;
        }


        private void ValidateFlatFile()
        {
            string text;
            string text_2;
            string text_44;
            if (OpeningLine == null)
            {
                text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.FinishingRowType", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.NotEncountered", 0, useLocal);
                throw new ApplicationException($"{text} {OpeningLineDTO.RowType} {text_2}  ");
            }
            if (ClosingLine == null)
            {
                text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.StartingRowType", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.NotEncountered", 0, useLocal);
                throw new ApplicationException($"{text} {ClosingLineDTO.RowType} {text_2}  ");
            }

            //if (ClosingLine.DeductionFileNum != OpeningLine.DeductionFileNum)
            //{
            //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.FinishingRowDeductionFile", 0, useLocal);
            //    throw new ApplicationException($"{text} {ClosingLine.DeductionFileNum} {text_44}{text_2} {OpeningLine.DeductionFileNum} ");
            //}
            //string myDeduc = _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
            //if (ClosingLine.DeductionFileNum != myDeduc)
            //{
            //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.StartingRowDeductionFile", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.OurDeductionFile", 0, useLocal);
            //    throw new ApplicationException($"{text} {ClosingLine.DeductionFileNum} {text_44}{text_2} {myDeduc} ");
            //}

            //if (OpeningLine.TotalInvalidRecords + OpeningLine.TotalValidRecords != OpeningLine.TotalVendorNumber)
            //{
            //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.FinishingRowTotals", 0, useLocal);
            //    throw new ApplicationException($"{text} {OpeningLine.TotalInvalidRecords} + {OpeningLine.TotalValidRecords} != {OpeningLine.TotalVendorNumber} ");
            //}

            //if (OpeningLine.TotalValidRecords != _VendorLinesDTO.Count)
            //{
            //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.FinishingRowTotalVendors", 0, useLocal);
            //    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.DiffersFrom", 0, useLocal);
            //    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.CountVendorRows", 0, useLocal);
            //    throw new ApplicationException($"{text} {OpeningLine.TotalValidRecords} {text_44}{text_2} {_VendorLinesDTO.Count}");
            //}


            long count = 1;
            foreach (TaxReportLineDTO taxLine in _TaxReportLinesDTO)
            {
                if (taxLine.VatNumber.TrimStart('0') == "")
                {
                    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.TaxLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.OppositeVatNumber", 0, useLocal);
                    this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                }
                //if (taxLine.LocatedVATNum.TrimStart('0') == "" && taxLine.LocatedDeductionFileNum.TrimStart('0') == "")
                //{
                //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.VendorLineNo", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.LocatedVATNumber", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({taxLine.VendorCode}) {text_2} {text_44}");
                //}
                if (taxLine.ReferenceDateString == TaxReportLineDTO._EmptyDate)
                {
                    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.TaxineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.IsEmpty", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.ReferenceDate", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({taxLine.VatNumber}) {text_2} {text_44} ");
                }
                if (taxLine.Reference.TrimStart('0') == "")
                {
                    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.TaxLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.Reference", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({taxLine.VatNumber}) {text_2} {text_44} ");
                }
                //if (taxLine.StartDateString != VendorLineDTO._EmptyDate && taxLine.EndDateString == VendorLineDTO._EmptyDate)

                //{
                //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.VendorLineNo", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.IsEmpty", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.EndDate", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode}) {text_2} {text_44} ");
                //}
                //if (taxLine.DeductionPercentage != 100m && taxLine.StartDateString == VendorLineDTO._EmptyDate) // 100m = no deduction  
                //{
                //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.VendorLineNo", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.IsEmpty", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.StartDate", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode})  {text_2} {text_44} ");
                //}
                //if (taxLine.DeductionPercentage != 100m && taxLine.EndDateString == VendorLineDTO._EmptyDate) // 100m = no deduction  
                //{
                //    text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.VendorLineNo", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.IsEmpty", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.EndDate", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({taxLine.VendorCode})  {text_2} {text_44} ");
                //}


                count++;
            }

        }



    }


    public class FlatFileLoadResult
    {
        public List<string> SuccessTaxLineList = new List<string>();
        public List<string> ExceptionTaxLineList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateTaxLineLineAgainstDBErrors = new List<string>();
    }


    class OpeningLineDTO
    {
        public const string RowType = "O";
        public string RawLine { get; set; }
        //public string DeductionFileNum { get; private set; }
        //public DateTime CreateDate { get; private set; }
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        internal static OpeningLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                string text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.DoesntStartWithRowType", 0, useLocal);
                throw new ApplicationException($"{text} {RowType} ");
            }

            var rec = new OpeningLineDTO();
            rec.RawLine = rawLine;
            //rec.DeductionFileNum = rawLine.Substring(2 - 1, 9);

            //string txtDateTime = rawLine.Substring(11 - 1, 8);
            //string fieldname = "CreateDate";
            //string pos = "86 - 1, 8";
            //DateTime date = ConsolidatedTaxReportFlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
            //rec.CreateDate = date;


            return rec;
        }
    }

    class ClosingLineDTO
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

        internal static ClosingLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                string text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.DoesntStartWithRowType", 0, useLocal);
                throw new ApplicationException($"{text} {RowType} ");
            }

            var rec = new ClosingLineDTO();
            rec.RawLine = rawLine;
            //rec.DeductionFileNum = rawLine.Substring(2 - 1, 9);

            //rec.TotalVendorNumber = long.Parse(rawLine.Substring(11 - 1, 4));
            //rec.TotalValidRecords = long.Parse(rawLine.Substring(15 - 1, 4));
            //rec.TotalInvalidRecords = long.Parse(rawLine.Substring(19 - 1, 4));


            return rec;
        }
    }




    class TaxReportLineDTO
    {
        public static List<String> RowType = new List<String>(new string[]
            { "S", "L", "M", "Y", "I", "T", "C", "K", "R", "P", "H", });

        public const string _EmptyDate = "00000000";
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public string RawLine { get; set; }

        public string VatNumber { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public string ReferenceDateString { get; private set; }

        public string ReferenceGroup { get; private set; }
        public string Reference { get; private set; }
        public decimal VatAmount { get; private set; }
        public string InvoiceAmountSign { get; private set; }
        public decimal VatableInvoiceAmount { get; private set; }
        public string APS_Reference { get; private set; }
        public string OutputOrInput { get; private set; }
        public string LineTypeCode { get; private set; }

        internal static TaxReportLineDTO Create(string rawLine)
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
                string text = TranslateTextsClassTranslate("ConsolidatedTaxReport.O.DoesntStartWithRowType", 0, useLocal);
                throw new ApplicationException($"{text} {RowType.ToString()} ");
            }

            var rec = new TaxReportLineDTO();
            rec.RawLine = rawLine;
            rec.VatNumber = rawLine.Substring(2 - 1, 9);

            string txtDateTime = rawLine.Substring(11 - 1, 8);
            string fieldname = "";
            string pos = "";
            DateTime date = DateTime.MinValue;
            rec.ReferenceDateString = txtDateTime;
            if (rec.ReferenceDateString != _EmptyDate)
            {
                fieldname = "ReferenceDate";
                pos = "11 - 1, 8";
                date = ConsolidatedTaxReportFlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
                rec.ReferenceDate = date;
            }



            rec.ReferenceGroup = rawLine.Substring(19 - 1, 4);
            rec.Reference = rawLine.Substring(23 - 1, 9);
            rec.VatAmount = 0M;
            try
            {
                rec.VatAmount = decimal.Parse(rawLine.Substring(32 - 1, 9));
            }
            catch (Exception e)
            { }

            rec.InvoiceAmountSign = rawLine.Substring(41 - 1, 1);
            rec.VatableInvoiceAmount = 0M;
            try
            {
                rec.VatableInvoiceAmount = decimal.Parse(rawLine.Substring(42 - 1, 10));
            }
            catch (Exception e)
            { }

            if (rec.InvoiceAmountSign == "-")
            {
                rec.VatableInvoiceAmount = -rec.VatableInvoiceAmount;
                rec.VatAmount = -rec.VatAmount;
            }

            rec.APS_Reference = rawLine.Substring(52 - 1, 9);

            if (actualRowType == "S" || actualRowType == "L" || actualRowType == "M" || actualRowType == "Y" || actualRowType == "I")
            {
                rec.OutputOrInput = "O";
            }
            else
            {
                rec.OutputOrInput = "I";
            }

            rec.LineTypeCode = actualRowType;

            return rec;
        }


    }

}
