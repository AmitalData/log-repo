using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class System1000FlatFileAnalyser
    {
        private StartingLineDTO StartingLine = null;
        private FinishingLineDTO FinishingLine = null;
        private List<VendorLineDTO> _VendorLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IAccountingContext accountingContext;
        public ResultLoadFlatFile MyResultLoadFlatFile = new ResultLoadFlatFile();
        private IQueryable<CardGLAccountDataView> _AllVendorGLAccountCards;



        public void Analyse(int? ptenant, string FileContent)
        {
            try
            {
                FileContent = ConvertFromDosHebrewToWinHebrew(FileContent);
                int? tenantFromPage4Tester = null;
                _VendorLinesDTO = CreateVendorLinesDTOFromFile(FileContent, out tenantFromPage4Tester);

                if (tenantFromPage4Tester.HasValue)
                {
                    ptenant = tenantFromPage4Tester.Value;
                }
                if (!ptenant.HasValue)
                {
                    throw new Exception("unable to find tenantFromPage4Tester ");
                }
                accountingContext = AccountingContext.GetContext(ptenant.Value);
                _FullAccountingSettingPM = GetDeductionFileNumberFromAccSetting(accountingContext, ptenant.Value);
                ValidateFlatFile();

            }
            catch (Exception e)
            {

                throw new Exception("LoadSystem1000FromFile(FileContent) failed while performing CreateVendorLinesDTOFromFile ", e);
            }
            int tenant = ptenant.Value;


            //TenantBankPagesFilter(tenant, _BankPagesDTO);
            _AllVendorGLAccountCards = GetQAllVendorGLAccountCards(accountingContext, tenant);

            using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
            {
                foreach (VendorLineDTO vendorLineDTO in _VendorLinesDTO)
                {
                    { 
                        AnalyseOneVendor(tenant, vendorLineDTO);

                    }
                }
                scope.Complete();
            }
        }

        private IQueryable<CardGLAccountDataView> GetQAllVendorGLAccountCards(IAccountingContext accountingContext, int tenant)
        {
            var myGLAccountQueryService = new GLAccountQueryService(accountingContext);
            var myVendorGLAccountCardList = myGLAccountQueryService.GetQAllVendorGLAccountCardsHavingDeduction(tenant);
            return myVendorGLAccountCardList;
        }



        private void AnalyseOneVendor(int tenant, VendorLineDTO vendorLineDTO)
        {
            CardGLAccountDataView oneVendor = _AllVendorGLAccountCards.Where(p => p.DisplayNumber.Replace(" ", "").PadLeft(15, '0').Substring(0, 15) == vendorLineDTO.VendorCode 
            && (p.VatNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9) == vendorLineDTO.SentVATNum || p.VatNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9) == vendorLineDTO.LocatedVATNum)).FirstOrDefault();
            if (oneVendor is null)
            {
                MyResultLoadFlatFile.ValidateVendorLineAgaintDBErrors.Add($"Vendor Number {vendorLineDTO.VendorCode} not found ");
                return;
            }
      
        }

        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }


        private FullAccountingSettingPM GetDeductionFileNumberFromAccSetting(IAccountingContext accountingContext, int tenant)
        {
            bool useLocal = true;
            string text;
            var myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
            var myFullAccountingSettingPM = myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (myFullAccountingSettingPM == null)
            {
                throw new Exception("No FullAccountingSettingPM  for tenant ");
            }
            if (string.IsNullOrWhiteSpace(myFullAccountingSettingPM.DeductionFileNumber))
            {
                //   throw new Exception("No myFullAccountingSettingPM.DeductionFileNumber  for tenant ");
                text = TranslateTextsClassTranslate("System1000.O.DeductionFileNumber", 0, useLocal);
                // Deduction File Number is undefined.
                throw new Exception(text);
            }
            return myFullAccountingSettingPM;
        }


        private void ValidateFlatFile()
        {
            if (FinishingLine == null)
            {
                throw new Exception($"Finishing Row Type {FinishingLineDTO.RowType} not encountered  ");
            }
            if (StartingLine == null)
            {
                throw new Exception($"Starting Row Type {StartingLineDTO.RowType} not encountered  ");
            }

            if (StartingLine.DeductionFileNum != FinishingLine.DeductionFileNum)
            {
                throw new Exception($"Starting Row Deduction File {StartingLine.DeductionFileNum} differs from Finishing Row Deduction File {FinishingLine.DeductionFileNum} ");
            }
            string myDeduc = _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
            if (StartingLine.DeductionFileNum != myDeduc)
            {
                throw new Exception($"Starting Row Deduction File {StartingLine.DeductionFileNum} differs from our Deduction File Number {myDeduc} ");
            }

            if (FinishingLine.TotalInvalidRecords + FinishingLine.TotalValidRecords != FinishingLine.TotalVendorNumber)
            {
                throw new Exception($"Finishing Row totals are not summing up together {FinishingLine.TotalInvalidRecords} + {FinishingLine.TotalValidRecords} != {FinishingLine.TotalVendorNumber} ");
            }

            if (FinishingLine.TotalVendorNumber != _VendorLinesDTO.Count)
            {
                throw new Exception($"Finishing Row Total Vendor Number {FinishingLine.TotalVendorNumber} differs from count of Vendor Rows ");
            }


            long count = 0;
            foreach (VendorLineDTO vendorLine in _VendorLinesDTO)
            {
                if (vendorLine.VendorCode.TrimStart('0') == "")
                {
                    this.AddErrorRow($"{vendorLine.RawLine} Vendor Line #{count} Vendor Code is missing");
                }
                if (vendorLine.LocatedDeductionFileNum.TrimStart('0') == "")
                {
                    this.AddErrorRow($"{vendorLine.RawLine} Vendor Line #{count} Located Deduction File is missing");
                }
                if (vendorLine.LocatedVATNum.TrimStart('0') == "")
                {
                    this.AddErrorRow($"{vendorLine.RawLine} Vendor Line #{count} Located VAT Number is missing");
                }
                if (vendorLine.StartDateString == VendorLineDTO._EmptyDate && vendorLine.EndDateString != VendorLineDTO._EmptyDate)

                {
                    this.AddErrorRow($"{vendorLine.RawLine} Vendor Line #{count} Start Date is empty ");
                }
                if (vendorLine.StartDateString != VendorLineDTO._EmptyDate && vendorLine.EndDateString == VendorLineDTO._EmptyDate)

                {
                    this.AddErrorRow($"{vendorLine.RawLine} Vendor Line #{count} End Date is empty ");
                }
                if (vendorLine.DeductionPercentage != 0m && vendorLine.StartDateString == VendorLineDTO._EmptyDate)
                {
                    this.AddErrorRow($"{vendorLine.RawLine} Vendor Line #{count}  Start Date is empty ");
                }
                if (vendorLine.DeductionPercentage != 0m && vendorLine.EndDateString == VendorLineDTO._EmptyDate)
                {
                    this.AddErrorRow($"{vendorLine.RawLine} Vendor Line #{count}  End Date is empty ");
                }


                count++;
            }
            throw new NotImplementedException();
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

        private void InsertVendorLine(int tenant, VendorLineDTO newVendorLine, GLAccountWithholdingTaxPM entityPM)
        {
            try
            {
                var MyContext = AccountingContext.GetContext(entityPM.Tenant);
                GLAccountWithholdingTaxUpdateService service = new GLAccountWithholdingTaxUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                service.Update(entityPM, true);

   //             //scope.Complete();
   //             this.AddSuccessInsertVendorLine(entityPM, newVendorLine);

            }

         catch (Exception ex)
            {
                this.AddExceptionInsertVendorLine(entityPM, newVendorLine, ex);
           }
        }

        private void AddExceptionInsertVendorLine(GLAccountWithholdingTaxPM entityPM, VendorLineDTO newLineOfVendorAccount, Exception ex)
        {

            var dataXml = ProxyUtil.JsonConvertSerialize(newLineOfVendorAccount);

            this.MyResultLoadFlatFile.ExceptionVendorList.Add($"Exception insert Vendor Number:{newLineOfVendorAccount.VendorCode}/AccountNumber{newLineOfVendorAccount.StartDate}/{newLineOfVendorAccount.EndDate} >{ex.ToString()} " +
                Environment.NewLine +
                dataXml);

        }

        private void AddErrorRow(String errorLine)
        {

             this.MyResultLoadFlatFile.ErrorRowList.Add(errorLine + " " +  Environment.NewLine);

        }



        private void AddSuccessInsertVendorLine(ReconcileExternalPagePM entityPM, VendorLineDTO newVendorLine)
        {
            this.MyResultLoadFlatFile.SuccessVendorList.Add($"Success insert Vendor Number:{newVendorLine.VendorCode} / Rate {newVendorLine.DeductionPercentage} / From {newVendorLine.StartDate} / To {newVendorLine.EndDate} =new DbId:{entityPM.Id}/DBPageNo:{entityPM.PageNo}  ");
        }

        private List<VendorLineDTO> CreateVendorLinesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            bool readingLines = false;
            bool finished = false;
            var VendorLines = new List<VendorLineDTO>();
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
                    case StartingLineDTO.RowType:
                        {
                            StartingLine = StartingLineDTO.Create(rawLine);
                            readingLines = true;
                        }
                        break;

                    case VendorLineDTO.RowType:
                        {
                            if (!readingLines)
                            {
                                throw new Exception($"{rawLine} Vendor Line Row Type {VendorLineDTO.RowType} appears before Starting Row Type {StartingLineDTO.RowType} ");
                            }
                            VendorLineDTO vendorLine = VendorLineDTO.Create(rawLine);
                            VendorLines.Add(vendorLine);
                        }
                        break;

                    case FinishingLineDTO.RowType:
                        {
                            FinishingLine = FinishingLineDTO.Create(rawLine);
                            finished = true;
                         }
                        break;
                    default:
                        throw new Exception($"not a valid Row Type  {rawLine}");
                        break;
                }
                if (finished)
                {
                    break;
                }
            }
            return VendorLines;
        }

        public static DateTime TryGetDateTime(string rawLine, string txtDateTime, string fieldname, string pos, string @format = "yyyyMMddHHmm")
        {
            DateTime date = DateTime.MinValue;
            DateTime.TryParseExact(txtDateTime, @format/*"yyyyMMddHHmm"*/, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (date == DateTime.MinValue)
            {
                throw new
                    Exception($"{rawLine} {fieldname} should be  yyyyMMddHHmm  Substring({pos}) ={txtDateTime}  ");
            }

            return date;
        }


    }

    public class ResultLoadFlatFile
    {
        public List<string> SuccessVendorList = new List<string>();
        public List<string> ExceptionVendorList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateVendorLineAgaintDBErrors = new List<string>();
    }


    class StartingLineDTO
    {
        public const string RowType = "A";
        public string RawLine { get; set; }
        public string DeductionFileNum { get; private set; }
        public DateTime CreateDate { get; private set; }

        internal static StartingLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                throw new Exception($"{rawLine} does not start with a Line Row Type {RowType} ");
            }

            var rec = new StartingLineDTO();
            rec.RawLine = rawLine;
            rec.DeductionFileNum = rawLine.Substring(2 - 1, 9);

            string txtDateTime = rawLine.Substring(11 - 1, 8);
            string fieldname = "CreateDate";
            string pos = "86 - 1, 8";
            DateTime date = System1000FlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
            rec.CreateDate = date;


            return rec;
        }
    }

    class FinishingLineDTO
    {
        public const string RowType = "Z";
        public string RawLine { get; set; }
        public string DeductionFileNum { get; private set; }

        public long TotalVendorNumber { get; set; }
        public long TotalValidRecords { get; set; }
        public long TotalInvalidRecords { get; set; }

        internal static FinishingLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.EndsWith(RowType))
            {
                throw new Exception($"{rawLine} does not End with a Line Row Type {RowType} ");
            }

            var rec = new FinishingLineDTO();
            rec.RawLine = rawLine;
            rec.DeductionFileNum = rawLine.Substring(2 - 1, 9);

            rec.TotalVendorNumber = long.Parse(rawLine.Substring(11 - 1, 4));
            rec.TotalValidRecords = long.Parse(rawLine.Substring(15 - 1, 4));
            rec.TotalInvalidRecords = long.Parse(rawLine.Substring(19 - 1, 4));


            return rec;
        }
    }

    class VendorLineDTO
    {
        public const string RowType = "B";
        public const string _EmptyDate = "00000000";

        public string RawLine { get; set; }

        public string VendorCode { get; private set; }
        public string VendorName { get; private set; }

        public string SentDeductionFileNum { get; private set; }
        public string SentVATNum { get; private set; }

        public string LocatedDeductionFileNum { get; private set; }
        public string LocatedVATNum { get; private set; }

        public string Confirmation { get; private set; }
        public decimal DeductionPercentage { get; private set; }
        public string StartDateString { get; private set; }
        public string EndDateString { get; private set; }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime ConfirmationCreateDate { get; private set; }

        public string ValidFor { get; private set; }
        public string ValidForDeductionFile { get; private set; }

        public decimal AmountLmit { get; private set; }
        public string FileReported { get; private set; }



        internal static VendorLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                throw new Exception($"{rawLine} does not start with a Line Row Type {RowType} ");
            }

            var rec = new VendorLineDTO();
            rec.RawLine = rawLine;
            rec.VendorCode = rawLine.Substring(2 - 1, 15);

            rec.SentDeductionFileNum = rawLine.Substring(17 - 1, 9);
            rec.SentVATNum = rawLine.Substring(26 - 1, 9);

            rec.LocatedDeductionFileNum = rawLine.Substring(35 - 1, 9);
            rec.LocatedVATNum = rawLine.Substring(44 - 1, 9);

            rec.VendorName = rawLine.Substring(53 - 1, 22);
            rec.Confirmation = rawLine.Substring(75 - 1, 1);

            rec.DeductionPercentage = decimal.Parse(rawLine.Substring(76 - 1, 10));

            string txtDateTime = rawLine.Substring(86 - 1, 8);
            string fieldname = "";
            string pos = "";
            DateTime date = DateTime.MinValue;
            rec.StartDateString = txtDateTime;
            if (rec.StartDateString != _EmptyDate)
            {
                fieldname = "StartDate";
                pos = "86 - 1, 8";
                date = System1000FlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
                rec.StartDate = date;
            }

            txtDateTime = rawLine.Substring(94 - 1, 8);
            rec.EndDateString = txtDateTime;
            if (rec.EndDateString != _EmptyDate)
            {
                fieldname = "EndDate";
                pos = "94 - 1, 8";
                date = System1000FlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
                rec.EndDate = date;
            }

            txtDateTime = rawLine.Substring(102 - 1, 8);
            fieldname = "ConfirmationCreateDate";
            pos = "102 - 1, 8";
            date = System1000FlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
            rec.ConfirmationCreateDate = date;

            rec.ValidFor = rawLine.Substring(110 - 1, 3);
            rec.ValidForDeductionFile = rawLine.Substring(113 - 1, 9);

            rec.AmountLmit = decimal.Parse(rawLine.Substring(122 - 1, 10));

            rec.FileReported = rawLine.Substring(132 - 1, 9);

            return rec;
        }


     }

}
