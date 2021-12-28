using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
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
    public class System1000FlatFileAnalyser
    {
        private StartingLineDTO StartingLine = null;
        private FinishingLineDTO FinishingLine = null;
        private List<VendorLineDTO> _VendorLinesDTO;
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IAccountingContext accountingContext;
        public ResultLoadFlatFile MyResultLoadFlatFile = new ResultLoadFlatFile();
        private IQueryable<CardGLAccountDataView> _AllVendorGLAccountCards;
        private ContactRepository _contactRep; 
        private string _resolveLoggingUserId; 
        private Contact _contact;
        private const bool useLocal = true;

        public void Analyse(int? ptenant, string FileContent,string LoggingUserId)
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
                    throw new ApplicationException("unable to find tenantFromPage4Tester ");
                }
                int tenant = ptenant.Value;
                _contactRep = new ContactRepository(tenant);
                _resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(tenant);
                _contact = _contactRep.GetSingleContactByEmail(_resolveLoggingUserId, tenant);
                accountingContext = AccountingContext.GetContext(tenant);
                _FullAccountingSettingPM = GetDeductionFileNumberFromAccSetting(accountingContext, tenant);
                ValidateFlatFile();

                _AllVendorGLAccountCards = GetQAllVendorGLAccountCards(accountingContext, tenant);

                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(180)))
                {
                    foreach (VendorLineDTO vendorLineDTO in _VendorLinesDTO)
                    {
                        {
                            AnalyseOneVendor(tenant, vendorLineDTO);

                        }
                    }
                    scope.Complete();
                    if (MyResultLoadFlatFile.ValidateVendorLineAgainstDBErrors.Count > 0)
                    {
                        String errorLines = "";
                        MyResultLoadFlatFile.ValidateVendorLineAgainstDBErrors.ForEach(item => errorLines += item.ToString() + "\n");
                        throw new ApplicationException($"{errorLines}");
                    }
                    if (MyResultLoadFlatFile.ErrorRowList.Count > 0)
                    {
                        String errorLines = "";
                        MyResultLoadFlatFile.ErrorRowList.ForEach(item => errorLines += item.ToString() + "\n");
                        throw new ApplicationException($"{errorLines}");
                    }
                    if (MyResultLoadFlatFile.ExceptionVendorList.Count > 0)
                    {
                        String errorLines = "";
                        MyResultLoadFlatFile.ExceptionVendorList.ForEach(item => errorLines += item.ToString() + "\n");
                        throw new ApplicationException($"{errorLines}");
                    }

                }
            }
            catch (Exception e)
            {
                string text = TranslateTextsClassTranslate("System1000.O.FailedWhilePerforming", 0, useLocal);

                throw new ApplicationException($"{text} ", e);
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
            GLAccountWithholdingTaxQueryService gLAccountWithholdingTaxQueryService = new GLAccountWithholdingTaxQueryService(tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);

            string vendorStripped = vendorLineDTO.VendorCode.TrimStart('0');
            string sentVatStripped = vendorLineDTO.SentVATNum.TrimStart('0');
            string locatedVatStripped = vendorLineDTO.LocatedVATNum.TrimStart('0');
            CardGLAccountDataView oneVendor = _AllVendorGLAccountCards.Where(p => p.DisplayNumber.Replace(" ", "").EndsWith(vendorStripped) 
                    && (p.VatNumber.Replace(" ", "").EndsWith(sentVatStripped) || p.VatNumber.Replace(" ", "").EndsWith(locatedVatStripped))).FirstOrDefault();
            if (oneVendor == null)
            {
                string text_44 = TranslateTextsClassTranslate("System1000.O.NotFound", 0, useLocal);
                string text_2 = TranslateTextsClassTranslate("System1000.O.VendorNo", 0, useLocal);
                MyResultLoadFlatFile.ValidateVendorLineAgainstDBErrors.Add($"{text_2} {vendorLineDTO.VendorCode} {text_44} ");
                return;
            }

            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(MyContext);
            GLAccountUpdateService gLAccountUpdateService = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            GLAccountPM gLAccountPM = gLAccountQueryService.GetSinglePM(oneVendor.Id, oneVendor.Tenant);
            if (gLAccountPM == null)
            {
                string text_44 = TranslateTextsClassTranslate("System1000.O.NotFound", 0, useLocal);
                string text_2 = TranslateTextsClassTranslate("System1000.O.VendorNo", 0, useLocal);
                string text_acc = TranslateTextsClassTranslate("GLTransactionReport.O.GLAccountNo", 0, useLocal);
                MyResultLoadFlatFile.ValidateVendorLineAgainstDBErrors.Add($"{text_2} {vendorLineDTO.VendorCode} - {text_acc} {text_44} ");
                return;
            }

            gLAccountQueryService.GetComposition(new GLAccountKeys() { Id = oneVendor.Id }, gLAccountPM);


            if (vendorLineDTO.DeductionPercentage == 100m) // 100m = no deduction  
            {
                // De-activate all current and future lines 
                DateTime date = DateTime.Today;
                gLAccountPM.GLAccountWithholdingTaxes.ForEach(taxLine => 
                {
                     if (!taxLine.Inactive && ((taxLine.FromDate < date && (taxLine.ToDate > date || taxLine.ToDate == date)) || taxLine.FromDate == date || taxLine.FromDate > date))
                     {
                        taxLine.Inactive = true;
                        taxLine.Changed = true;
                        taxLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        taxLine.CurrentContextTag = GLAccountWithholdingTaxUpdateService.RaiseEventWBLKConst;
                        this.AddSuccessUpdateVendorLine(taxLine, vendorLineDTO);
                        gLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    }
                });
                gLAccountUpdateService.Update(gLAccountPM, true);
           }
            else
            {
                // If there is a row with the same data in with the same values, do not create a new line.
                GLAccountWithholdingTaxPM existingLine = gLAccountPM.GLAccountWithholdingTaxes.Where(a => !a.Inactive && (vendorLineDTO.StartDate.HasValue && a.FromDate == vendorLineDTO.StartDate.Value)
                                                && (vendorLineDTO.EndDate.HasValue && a.ToDate == vendorLineDTO.EndDate.Value)).FirstOrDefault();
                if (existingLine != null && existingLine.Percentage == vendorLineDTO.DeductionPercentage)
                {
                    // do nothing;
                }
                else
                {
                    // De-activate all overlapping lines 
                    gLAccountPM.GLAccountWithholdingTaxes.ForEach(overLine =>
                    {
                        if (!overLine.Inactive && (!((vendorLineDTO.EndDate.HasValue && overLine.FromDate > vendorLineDTO.EndDate.Value)
                                                || (vendorLineDTO.StartDate.HasValue && overLine.ToDate < vendorLineDTO.StartDate.Value))) )
                        {
                            overLine.Inactive = true;
                            overLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                            overLine.Changed = true;
                            overLine.CurrentContextTag = GLAccountWithholdingTaxUpdateService.RaiseEventWLDAConst;
                            this.AddSuccessUpdateVendorLine(overLine, vendorLineDTO);
                        }
                    });



                    GLAccountWithholdingTaxPM newLine = CreateNewLineFromDTO(vendorLineDTO, oneVendor);
                    newLine.CurrentContextTag = GLAccountWithholdingTaxUpdateService.RaiseEventAWNCConst;
                    newLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    gLAccountPM.GLAccountWithholdingTaxes.Add(newLine);
                    gLAccountPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    this.AddSuccessInsertVendorLine(newLine, vendorLineDTO);
                    gLAccountUpdateService.Update(gLAccountPM, true);
               }
            }
        }


        private GLAccountWithholdingTaxPM CreateNewLineFromDTO(VendorLineDTO vendorLineDTO, CardGLAccountDataView oneVendor) 
        {
            GLAccountWithholdingTaxPM newLine = new GLAccountWithholdingTaxPM();

            newLine.GLAccountId = oneVendor.Id;
            newLine.Tenant = oneVendor.Tenant;
            newLine.CreateDate = DateTime.Now;
            newLine.CreatedByUserId = _contact.Id;
            newLine.Inactive = false;
            newLine.FromDate = vendorLineDTO.StartDate.Value;
            newLine.ToDate = vendorLineDTO.EndDate.Value;
            newLine.Percentage = Convert.ToInt32(vendorLineDTO.DeductionPercentage);
            if (newLine.Percentage < 0)
            {
                newLine.Percentage = 0;
            }
            if (newLine.Percentage > 100)
            {
                newLine.Percentage = 100;
            }
          //  newLine.LineNumber = nextLine;


            return newLine;
        }

        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }


        private FullAccountingSettingPM GetDeductionFileNumberFromAccSetting(IAccountingContext accountingContext, int tenant)
        {
            string text;
            var myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
            var myFullAccountingSettingPM = myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (myFullAccountingSettingPM == null)
            {
                throw new ApplicationException("No FullAccountingSettingPM  for tenant ");
            }
            if (string.IsNullOrWhiteSpace(myFullAccountingSettingPM.DeductionFileNumber))
            {
                //   throw new ApplicationException("No myFullAccountingSettingPM.DeductionFileNumber  for tenant ");
                text = TranslateTextsClassTranslate("System1000.O.DeductionFileNumber", 0, useLocal);
                // Deduction File Number is undefined.
                throw new ApplicationException(text);
            }
            return myFullAccountingSettingPM;
        }


        private void ValidateFlatFile()
        {
            string text;
            string text_2;
            string text_44;
            if (FinishingLine == null)
            {
                text = TranslateTextsClassTranslate("System1000.O.FinishingRowType", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("System1000.O.NotEncountered", 0, useLocal);
                throw new ApplicationException($"{text} {FinishingLineDTO.RowType} {text_2}  ");
            }
            if (StartingLine == null)
            {
                text = TranslateTextsClassTranslate("System1000.O.StartingRowType", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("System1000.O.NotEncountered", 0, useLocal);
                throw new ApplicationException($"{text} {StartingLineDTO.RowType} {text_2}  ");
            }

            if (StartingLine.DeductionFileNum != FinishingLine.DeductionFileNum)
            {
                text = TranslateTextsClassTranslate("System1000.O.StartingRowDeductionFile", 0, useLocal);
                text_44 = TranslateTextsClassTranslate("System1000.O.DiffersFrom", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("System1000.O.FinishingRowDeductionFile", 0, useLocal);
                throw new ApplicationException($"{text} {StartingLine.DeductionFileNum} {text_44}{text_2} {FinishingLine.DeductionFileNum} ");
            }
            string myDeduc = _FullAccountingSettingPM.DeductionFileNumber.Replace(" ", "").PadLeft(9, '0').Substring(0, 9);
            if (StartingLine.DeductionFileNum != myDeduc)
            {
                text = TranslateTextsClassTranslate("System1000.O.StartingRowDeductionFile", 0, useLocal);
                text_44 = TranslateTextsClassTranslate("System1000.O.DiffersFrom", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("System1000.O.OurDeductionFile", 0, useLocal);
                throw new ApplicationException($"{text} {StartingLine.DeductionFileNum} {text_44}{text_2} {myDeduc} ");
            }

            if (FinishingLine.TotalInvalidRecords + FinishingLine.TotalValidRecords != FinishingLine.TotalVendorNumber)
            {
                text = TranslateTextsClassTranslate("System1000.O.FinishingRowTotals", 0, useLocal);
                throw new ApplicationException($"{text} {FinishingLine.TotalInvalidRecords} + {FinishingLine.TotalValidRecords} != {FinishingLine.TotalVendorNumber} ");
            }

            if (FinishingLine.TotalValidRecords != _VendorLinesDTO.Count)
            {
                text = TranslateTextsClassTranslate("System1000.O.FinishingRowTotalVendors", 0, useLocal);
                text_44 = TranslateTextsClassTranslate("System1000.O.DiffersFrom", 0, useLocal);
                text_2 = TranslateTextsClassTranslate("System1000.O.CountVendorRows", 0, useLocal);
                throw new ApplicationException($"{text} {FinishingLine.TotalValidRecords} {text_44}{text_2} {_VendorLinesDTO.Count}");
            }


            long count = 1;
            foreach (VendorLineDTO vendorLine in _VendorLinesDTO)
            {
                if (vendorLine.VendorCode.TrimStart('0') == "")
                {
                    text = TranslateTextsClassTranslate("System1000.O.VendorLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("System1000.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("System1000.O.VendorCode", 0, useLocal);
                    this.AddErrorRow($"{text}{count} {text_2} {text_44}");
                }
                //if (vendorLine.LocatedDeductionFileNum.TrimStart('0') == "")
                //{
                //    text = TranslateTextsClassTranslate("System1000.O.VendorLineNo", 0, useLocal);
                //    text_44 = TranslateTextsClassTranslate("System1000.O.IsMissing", 0, useLocal);
                //    text_2 = TranslateTextsClassTranslate("System1000.O.LocatedDeductionFile", 0, useLocal);
                //    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode}) {text_2} {text_44}");
                //}
                if (vendorLine.LocatedVATNum.TrimStart('0') == "" && vendorLine.LocatedDeductionFileNum.TrimStart('0') == "")
                {
                    text = TranslateTextsClassTranslate("System1000.O.VendorLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("System1000.O.IsMissing", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("System1000.O.LocatedVATNumber", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode}) {text_2} {text_44}");
                }
                if (vendorLine.StartDateString == VendorLineDTO._EmptyDate && vendorLine.EndDateString != VendorLineDTO._EmptyDate)

                {
                    text = TranslateTextsClassTranslate("System1000.O.VendorLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("System1000.O.IsEmpty", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("System1000.O.StartDate", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode}) {text_2} {text_44} ");
                }
                if (vendorLine.StartDateString != VendorLineDTO._EmptyDate && vendorLine.EndDateString == VendorLineDTO._EmptyDate)

                {
                    text = TranslateTextsClassTranslate("System1000.O.VendorLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("System1000.O.IsEmpty", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("System1000.O.EndDate", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode}) {text_2} {text_44} ");
                }
                if (vendorLine.DeductionPercentage != 100m && vendorLine.StartDateString == VendorLineDTO._EmptyDate) // 100m = no deduction  
                {
                    text = TranslateTextsClassTranslate("System1000.O.VendorLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("System1000.O.IsEmpty", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("System1000.O.StartDate", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode})  {text_2} {text_44} ");
                }
                if (vendorLine.DeductionPercentage != 100m && vendorLine.EndDateString == VendorLineDTO._EmptyDate) // 100m = no deduction  
                {
                    text = TranslateTextsClassTranslate("System1000.O.VendorLineNo", 0, useLocal);
                    text_44 = TranslateTextsClassTranslate("System1000.O.IsEmpty", 0, useLocal);
                    text_2 = TranslateTextsClassTranslate("System1000.O.EndDate", 0, useLocal);
                    this.AddErrorRow($"{text}{count} ({vendorLine.VendorCode})  {text_2} {text_44} ");
                }


                count++;
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



        private void AddSuccessInsertVendorLine(GLAccountWithholdingTaxPM entityPM, VendorLineDTO newVendorLine)
        {
            this.MyResultLoadFlatFile.SuccessVendorList.Add($"Success insert Vendor Number:{newVendorLine.VendorCode} / Rate {newVendorLine.DeductionPercentage} / From {newVendorLine.StartDate} / To {newVendorLine.EndDate} =new DbId:{entityPM.Id}/DBFromDate:{entityPM.FromDate}  ");
        }

        private void AddSuccessUpdateVendorLine(GLAccountWithholdingTaxPM entityPM, VendorLineDTO newVendorLine)
        {
            this.MyResultLoadFlatFile.SuccessVendorList.Add($"Success insert Vendor Number:{newVendorLine.VendorCode} / Rate {newVendorLine.DeductionPercentage} / From {newVendorLine.StartDate} / To {newVendorLine.EndDate} =new DbId:{entityPM.Id}/DBFromDate:{entityPM.FromDate}  ");
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

                                string text_3 = TranslateTextsClassTranslate("System1000.O.VendorLineRowType", 0, useLocal);
                                string text_44 = TranslateTextsClassTranslate("System1000.O.AppearsBefore", 0, useLocal);
                                string text_2 = TranslateTextsClassTranslate("System1000.O.StartingRowType", 0, useLocal);
                                throw new ApplicationException($"{text_3} {VendorLineDTO.RowType} {text_44} {text_2} {StartingLineDTO.RowType} ");
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
                        string text = TranslateTextsClassTranslate("System1000.O.NotValidRowType", 0, useLocal);
                        throw new ApplicationException($"{text}  {rawLine}");
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
                    Exception($"{fieldname} should be  yyyyMMddHHmm  Substring({pos}) ={txtDateTime}  ");
            }

            return date;
        }


    }

    public class ResultLoadFlatFile
    {
        public List<string> SuccessVendorList = new List<string>();
        public List<string> ExceptionVendorList = new List<string>();
        public List<string> ErrorRowList = new List<string>();
        public List<string> ValidateVendorLineAgainstDBErrors = new List<string>();
    }


    class StartingLineDTO
    {
        public const string RowType = "A";
        public string RawLine { get; set; }
        public string DeductionFileNum { get; private set; }
        public DateTime CreateDate { get; private set; }
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        internal static StartingLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                string text = TranslateTextsClassTranslate("System1000.O.DoesntStartWithRowType", 0, useLocal);
                throw new ApplicationException($"{text} {RowType} ");
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
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

        public string RawLine { get; set; }
        public string DeductionFileNum { get; private set; }

        public long TotalVendorNumber { get; set; }
        public long TotalValidRecords { get; set; }
        public long TotalInvalidRecords { get; set; }

        internal static FinishingLineDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                string text = TranslateTextsClassTranslate("System1000.O.DoesntStartWithRowType", 0, useLocal);
                throw new ApplicationException($"{text} {RowType} ");
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
        private const bool useLocal = true;

        public static string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }

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
        public string ConfirmationCreateDateString { get; private set; }

        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
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
                string text = TranslateTextsClassTranslate("System1000.O.DoesntStartWithRowType", 0, useLocal);
                throw new ApplicationException($"{text} {RowType} ");
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

            rec.DeductionPercentage = decimal.Parse(rawLine.Substring(76 - 1, 2)); ////** only first 2-pos (out of 10) count; 99 is 0%, 00 is "no deduction" *** 

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
            if (rec.DeductionPercentage == 0m && !rec.StartDate.HasValue && !rec.EndDate.HasValue)
            {
                rec.DeductionPercentage = 100m;  // 100m = no deduction 
            }
            if (rec.DeductionPercentage == 99m)
            {
                rec.DeductionPercentage = 0m;
            }

            txtDateTime = rawLine.Substring(102 - 1, 8);
            rec.ConfirmationCreateDateString = txtDateTime;
            if (rec.EndDateString != _EmptyDate)
            {
                fieldname = "ConfirmationCreateDate";
                pos = "102 - 1, 8";
                date = System1000FlatFileAnalyser.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
                rec.ConfirmationCreateDate = date;
            }

            rec.ValidFor = rawLine.Substring(110 - 1, 3);
            rec.ValidForDeductionFile = rawLine.Substring(113 - 1, 9);

            rec.AmountLmit = decimal.Parse(rawLine.Substring(122 - 1, 10));

            rec.FileReported = rawLine.Substring(132 - 1, 9);

            return rec;
        }


     }

}
