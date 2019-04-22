using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BankAccountPages
{
    public class BankAccountPageAnalyzer
    {
        private List<BankPageDTO> _BankPagesDTO;
        List<TenantPagesOfAccountDTO> _TenantBankPagesDTO = new List<TenantPagesOfAccountDTO>();


        static string ConvertFromDosHeberwToWinHeberw(string FileContent862)
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
        public void Analyze(int? ptenant, string FileContent)
        {

            try
            {
                FileContent = ConvertFromDosHeberwToWinHeberw(FileContent);
                int? tenantFromPage4Tester = null;
                _BankPagesDTO = CreateBankPagesDTOFromFile(FileContent, out tenantFromPage4Tester);
                
                if (tenantFromPage4Tester.HasValue)
                {
                    ptenant = tenantFromPage4Tester.Value;
                }
                if (!ptenant.HasValue)
                {
                    throw new Exception("unable to find tenantFromPage4Tester ");
                }
                FixSignOfOpenCloseBalance();
            }
            catch (Exception e)
            {

                throw new Exception("LoadBankPageFromFile(FileContent) failed while CreateBankPagesDTOFromFile ", e);
            }
            int tenant = ptenant.Value;
            TenantBankPagesFilter(tenant, _BankPagesDTO);

            foreach (var validBankAccountDTO in _TenantBankPagesDTO)
            {
                foreach (var newPageOfBankAccountDTO in validBankAccountDTO.PagesOfAccount.OrderBy(r => r.MyBankAccountM.PageNo))
                {

                    var accurateBankAccount = _BankAccountQueryService.GetSingle(validBankAccountDTO.DBBankaccountPM.Id, false, false);

                    AnalyzeeNewPage(tenant, accurateBankAccount, newPageOfBankAccountDTO);

                }
            }

        }

        private void FixSignOfOpenCloseBalance()
        {
            /*
s             b                   a 
1(+        3749.39)(+       55828.03) == a>0 >> 1=-1* B

0(+        3749.39)(+       55828.03) ===a>0 >> 0=+1*B

1(+        3749.39)(-       55828.03) == a<0 >> 1=+1*b


0(+        3749.39)(-       55828.03) == a<0 >> 0 = -1*b

             */



            _BankPagesDTO.ForEach(BankPagesDTO => {

                var bankPageLineLast = BankPagesDTO.MyBankPageLines.Last();
                if (Math.Abs(bankPageLineLast.BalanceAfter) != Math.Abs(BankPagesDTO.MyBankAccountM.CloseBalance))
                {
                    throw new Exception($"FixSignOfOpenCloseBalance():Exception:Close:Abs({bankPageLineLast.BalanceAfter})!={BankPagesDTO.MyBankAccountM.CloseBalance}");

                }

                

                var bankPageLine1st = BankPagesDTO.MyBankPageLines.First();
                decimal realOpenBalance = +1*BankPagesDTO.MyBankAccountM.OpenBalance;
                if (realOpenBalance - bankPageLine1st.Amount== bankPageLine1st.BalanceAfter)
                {
                    realOpenBalance = -1 * realOpenBalance;
                }
                else
                {
                    realOpenBalance = -1 * BankPagesDTO.MyBankAccountM.OpenBalance;
                    if (realOpenBalance - bankPageLine1st.Amount == bankPageLine1st.BalanceAfter)
                    {
                        realOpenBalance = -1 * BankPagesDTO.MyBankAccountM.OpenBalance;
                    }
                    else
                    {
                        throw new Exception("unable to resolve sign open balance of rawline ");
                    }
                }

                

                BankPagesDTO.MyBankAccountM.OpenBalance = realOpenBalance;
                BankPagesDTO.MyBankAccountM.CloseBalance = bankPageLineLast.BalanceAfter;
                

                var sumAmount= BankPagesDTO.MyBankPageLines.Sum(bankPageLine => bankPageLine.Amount);
                if (BankPagesDTO.MyBankAccountM.CloseBalance !=
                BankPagesDTO.MyBankAccountM.OpenBalance + sumAmount)
                {
                    throw new Exception($"FixSignOfOpenCloseBalance():Exception:OpenBalance + sumAmount!=Close:Abs({BankPagesDTO.MyBankAccountM.OpenBalance + sumAmount})!={BankPagesDTO.MyBankAccountM.CloseBalance }");
                }
            

            });

        }

        private ReconcileExternalPageQueryService _ReconcileExternalPageQueryService;


        private void TenantBankPagesFilter(int tenant, List<BankPageDTO> bankPages)
        {

            _BankCodeQueryService = new BankCodeQueryService(tenant);
            _BankAccountQueryService = new BankAccountQueryService(tenant);
            _ReconcileExternalPageQueryService = new ReconcileExternalPageQueryService(tenant);

            var bankPagesGBAccountNumber = (from bp in bankPages
                                            group bp by new { bp.BankCode, bp.MyBankAccountM.AccountNumber }
                     );
            foreach (var pagesOfAccountGroup in bankPagesGBAccountNumber)
            {
                var BankCode = pagesOfAccountGroup.Key.BankCode;
                var AccountNumber = pagesOfAccountGroup.Key.AccountNumber;
                var pagesOfAccount = pagesOfAccountGroup.ToList();


                var bankcodePM = _BankCodeQueryService.GetSingleByCode(BankCode, tenant);
                if (bankcodePM == null)
                {
                    MyResultLoadBankPage.ValidateBankPageAgaintDBErrors.Add($"BankCode {BankCode} not exist in Tenant {pagesOfAccount.First().RawLine}");
                    continue;
                }
                var dbBankaccountPM = _BankAccountQueryService.GetBankAccountByBankIdAccNumber(bankcodePM.Id, AccountNumber, tenant);
                if (dbBankaccountPM == null)
                {
                    MyResultLoadBankPage.ValidateBankPageAgaintDBErrors.Add($"BankCode {BankCode}  ,AccountNumber {AccountNumber} not exist in Tenant  {pagesOfAccount.First().RawLine}");
                    continue;
                }
                _TenantBankPagesDTO.Add(new TenantPagesOfAccountDTO() { DBBankaccountPM = dbBankaccountPM, PagesOfAccount = pagesOfAccount });


            }
        }



        private void AnalyzeeNewPage(int tenant, BankAccountPM dbBankaccountPM, BankPageDTO newPageOfBankAccount)
        {
            string errorPageValidation = PageValidationClientSide(newPageOfBankAccount);
            if (!string.IsNullOrWhiteSpace(errorPageValidation))
            {
                MyResultLoadBankPage.ValidateBankPageAgaintDBErrors.Add($"PageValidationClientSide():BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  {errorPageValidation}  ");
                return;
            }
            //if (dbBankaccountPM.LastPageCloseBalance != newPageOfBankAccount.MyBankAccountM.OpenBalance)
            //{
            //    ValidateBankPageAgaintDBErrors.Add($"BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  dbBankaccountPM.LastPageCloseBalance {dbBankaccountPM.LastPageCloseBalance} != OpenBalance  {newPageOfBankAccount.MyBankAccountM.OpenBalance}");
            //    return;
            //}

            ReconcileExternalPagePM prevReconcileExternalPagePM = null;
            if (dbBankaccountPM.LastPageNumber!=null)
            {
                prevReconcileExternalPagePM=
                _ReconcileExternalPageQueryService.GetPrevPageNoByPageNo(int.Parse(dbBankaccountPM.LastPageNumber), dbBankaccountPM.Id, tenant);
            } 


            var newBankPageLines = newPageOfBankAccount.GetCopyOfBankPageLines();

            if (prevReconcileExternalPagePM != null)
            {

                if (prevReconcileExternalPagePM.CloseBalance != newPageOfBankAccount.MyBankAccountM.OpenBalance)
                {
                    MyResultLoadBankPage.ValidateBankPageAgaintDBErrors.Add($"BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  prevReconcileExternalPagePM.CloseBalance {prevReconcileExternalPagePM.CloseBalance} != OpenBalance  {newPageOfBankAccount.MyBankAccountM.OpenBalance}");
                    return;
                }

                if (prevReconcileExternalPagePM.ToDate >= newBankPageLines.First().ReferenceDate)
                {
                    MyResultLoadBankPage.ValidateBankPageAgaintDBErrors.Add($"BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  prevReconcileExternalPagePM.ToDate {prevReconcileExternalPagePM.ToDate } >= newBankPageLines.First().ReferenceDate{newBankPageLines.First().ReferenceDate}");
                    return;
                }

            }


            ReconcileExternalPagePM entityPM = MapReconcileExternalPagePM(tenant, newPageOfBankAccount, dbBankaccountPM);

            InsertBankPage(tenant, newPageOfBankAccount, entityPM);

        }

        private void InsertBankPage(int tenant, BankPageDTO newPageOfBankAccount, ReconcileExternalPagePM entityPM)
        {
            try
            {
                

                using (var scope = TransactionFactory.GetTransaction())
                {

                    var MyContext = AccountingContext.GetContext(entityPM.Tenant);
                    ReconcileExternalPageUpdateService service = new ReconcileExternalPageUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    service.Update(entityPM, true);

                    scope.Complete();
                    this.AddSuccessInsertBankPage(entityPM, newPageOfBankAccount);
                }
            }

            catch (Exception ex)
            {
                this.AddExceptionInsertBankPage(entityPM, newPageOfBankAccount, ex);
            }
        }

   

        private static ReconcileExternalPagePM MapReconcileExternalPagePM(int tenant,
            BankPageDTO newPageOfBankAccount,
             

             BankAccountPM dbBankaccountPM
            )
        {

            List<BankPageLineDTO> newBankPageLines = newPageOfBankAccount.GetCopyOfBankPageLines();
            var entityPM = new ReconcileExternalPagePM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = tenant,

                StatusCode = "2",// 2- Approved
                EntryTypeCode = "2", // 2	File	קובץ 

                PageNo = newPageOfBankAccount.MyBankAccountM.PageNo, //oncreating fill auto // 
                CreateDate = DateTime.Now,//oncreating fill auto // 
                CreatedByUserId = "",//oncreating fill it 


                GLAccountId = dbBankaccountPM.GLAccountId,
                BankAccountId = dbBankaccountPM.Id,

                StartBalance = newPageOfBankAccount.MyBankAccountM.OpenBalance,
                CloseBalance = newPageOfBankAccount.MyBankAccountM.CloseBalance,

                FromDate = newBankPageLines.First().ReferenceDate,
                ToDate = newBankPageLines.Last().ReferenceDate,



                ReconcileExternalPageLines = new List<ReconcileExternalPageLinePM>()
            };
            int lineCounterNumber = 1;
            newBankPageLines.ForEach(line =>
            {
                entityPM.ReconcileExternalPageLines.Add(new ReconcileExternalPageLinePM()
                {
                    Tenant = entityPM.Tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    LineNumber = lineCounterNumber++,

                    Amount = line.Amount,
                    ReferenceDate = line.ReferenceDate,
                    Reference = line.Reference


                });
            });
            return entityPM;
        }
        private BankCodeQueryService _BankCodeQueryService;
        private BankAccountQueryService _BankAccountQueryService;
        public ResultLoadBankPage MyResultLoadBankPage = new ResultLoadBankPage();
        private void AddExceptionInsertBankPage(ReconcileExternalPagePM entityPM, BankPageDTO newPageOfBankAccount, Exception ex)
        {

            var dataXml = ProxyUtil.JsonConvertSerialize(newPageOfBankAccount);

            this.MyResultLoadBankPage.ExceptionPageList.Add($"Exception insert Page BankCode:{newPageOfBankAccount.BankCode}/AccountNumber{newPageOfBankAccount.MyBankAccountM.AccountNumber}/{newPageOfBankAccount.MyBankAccountM.PageNo} >{ex.ToString()} " + 
                Environment.NewLine +
                dataXml);

        }

        private void AddSuccessInsertBankPage(ReconcileExternalPagePM entityPM, BankPageDTO newPageOfBankAccount)
        {
            this.MyResultLoadBankPage.SuccessPageList.Add($"Success insert Page BankCode:{newPageOfBankAccount.BankCode}/AccountNumber{newPageOfBankAccount.MyBankAccountM.AccountNumber}/{newPageOfBankAccount.MyBankAccountM.PageNo} =new DbId:{entityPM.Id}/DBPageNo:{entityPM.PageNo}  ");
        }

        private string PageValidationClientSide(BankPageDTO pageOfAccount)
        {
            var CopyOfBankPageLines = pageOfAccount.GetCopyOfBankPageLines();
            if (CopyOfBankPageLines.Count == 0)
            {
                return $"BankPageLines.Count == 0  (client sidecheck )";
            }
            var lineWithoutRef = CopyOfBankPageLines.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Reference));
            if (lineWithoutRef != null)
            {
                return $"lineWithoutRef  {lineWithoutRef.RawLine} (client sidecheck )";
            }
            //if (this.ToDate > todayDate || this.FromDate > todayDate) {
            var maxDate = CopyOfBankPageLines.Max(r => r.ReferenceDate);
            if (DateTime.Now.Date < maxDate)
            {
                return $"maxDate>todate  {maxDate} (client sidecheck )";
            }

            return null;
        }


        private List<BankPageDTO> CreateBankPagesDTOFromFile(string FileContent, out int? tenant)
        {
            tenant = null;
            var bankPages = new List<BankPageDTO>();
            var lines = FileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            BankPageDTO bankPage = null;
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
                var rowtype = rawLine.Substring(0, 3);
                switch (rowtype)
                {
                    case BankPageDTO.RowType:
                        {
                            bankPage = BankPageDTO.Create(rawLine);
                            bankPages.Add(bankPage);
                        }
                        break;
                    case BankAccountDTO.RowType:
                        {
                            if (bankPage == null)
                            {
                                throw new Exception($"{rawLine} BankAccountM.RowType arrived b4 BankPageM.RowType ");
                            }
                            bankPage.AddBankAccountM(rawLine);
                        }
                        break;

                    case BankPageLineDTO.RowType:
                        {
                            if (bankPage == null)
                            {
                                throw new Exception($"{rawLine} BankPageLineM.RowType arrived b4 BankPageM.RowType ");
                            }
                            bankPage.AddBankPageLineM(rawLine);
                        }
                        break;
                    default:
                        throw new Exception($"no valid RowType  {rawLine}");
                        break;
                }
            }
            return bankPages;
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
    public class ResultLoadBankPage
    {
        public List<string> SuccessPageList = new List<string>();
        public List<string> ExceptionPageList = new List<string>();
        public List<string> ValidateBankPageAgaintDBErrors = new List<string>();

    }
    class BankPageDTO
    {
        public const string RowType = "031";
        public string RawLine { get; set; }
        public BankAccountDTO MyBankAccountM { get; private set; }
        public List<BankPageLineDTO> MyBankPageLines { get; set; }
        public string BankCode { get; private set; }
        //public string BranchNumber { get; private set; }
        //public string AccountNumber { get; private set; }
        public DateTime CreateDate { get; private set; }


        internal static BankPageDTO Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                throw new Exception($"{rawLine} not start with  RowType={RowType} ");
            }

            var rec = new BankPageDTO();
            rec.RawLine = rawLine;
            rec.MyBankPageLines = new List<BankPageLineDTO>();
            rec.BankCode = rawLine.Substring(4 - 1, 3);
            rec.BankCode = (rec.BankCode ?? "").Trim();
            //rec.BranchNumber = rawLine.Substring(7 - 1, 4);
            //rec.AccountNumber = rawLine.Substring(7 - 1, 16);
            //rec.AccountNumber = (rec.AccountNumber ?? "").Trim();

            string txtDateTime = rawLine.Substring(70 - 1, 8 + 4);
            string fieldname = "CreateDateTime";
            string pos = "70 - 1, 8 + 4";
            DateTime date = BankAccountPageAnalyzer.TryGetDateTime(rawLine, txtDateTime, fieldname, pos);
            rec.CreateDate = date; ;



            return rec;
        }



        internal void AddBankAccountM(string rawLine)
        {
            if (this.MyBankAccountM != null)
            {
                throw new
                    Exception($"{rawLine} AddBankAccountM *2 - 032 arrived 2 times ??");
            }
            RawLine = rawLine;
            this.MyBankAccountM = BankAccountDTO.Create(rawLine);
            if (this.MyBankAccountM.BankCode != this.BankCode)
            {
                throw new
                    Exception($"{rawLine} AddBankAccountM (this.MyBankAccountM.BankCode != this.BankCode)");

            }



        }
        internal List<BankPageLineDTO> GetCopyOfBankPageLines()
        {
            return new List<BankPageLineDTO>(this.MyBankPageLines);
        }
        internal void AddBankPageLineM(string rawLine)
        {
            BankPageLineDTO myBankPageLineM = BankPageLineDTO.Create(rawLine);
            decimal totAmount = this.MyBankAccountM.OpenBalance + MyBankPageLines.Sum(r => r.Amount);
            decimal totAmountIncludeCurrPage = totAmount + myBankPageLineM.Amount;
            if (totAmountIncludeCurrPage != myBankPageLineM.BalanceAfter)
            {
                ///throw new Exception($"{rawLine} AddBankPageLineM (totAmountIncludeCurrPage!= myBankPageLineM.BalanceAfter)");
            }
            MyBankPageLines.Add(myBankPageLineM);
        }
    }
    class BankAccountDTO
    {
        public string RawLine { get; set; }
        public string BankCode { get; private set; }
        public string BranchNumber { get; private set; }
        public string AccountNumber { get; private set; }
        public int PageNo { get; private set; }

        public decimal OpenBalance { get; set; }

        public decimal CloseBalance { get; set; }

        //public DateTime FromDate { get; private set; }
        //public DateTime ToDate { get; private set; }

        public const string RowType = "032";

        internal static BankAccountDTO Create(string rawLine)
        {
            try
            {


                rawLine = rawLine ?? "";
                if (!rawLine.StartsWith(RowType))
                {
                    throw new Exception($"{rawLine} not start with  RowType={RowType} ");
                }
                var rec = new BankAccountDTO();
                rec.BankCode = rawLine.Substring(4 - 1, 3);
                rec.BankCode = (rec.BankCode ?? "").Trim();
                rec.BranchNumber = rawLine.Substring(7 - 1, 4).Trim();

                rec.AccountNumber = rawLine.Substring(11 - 1, 16).Trim();
                rec.PageNo = int.Parse(rawLine.Substring(50 - 1, 8));

                string OpenBalanceSign = rawLine.Substring(58 - 1, 1);

                rec.OpenBalance = decimal.Parse(rawLine.Substring(58, 16 - 1));
                if (OpenBalanceSign == "-")
                {
                    rec.OpenBalance = -1 * rec.OpenBalance;
                }
                string CloseBalanceSign = rawLine.Substring(74 - 1, 1);
                rec.CloseBalance = decimal.Parse(rawLine.Substring(74, 16 - 1));
                if (CloseBalanceSign == "-")
                {
                    rec.CloseBalance = -1 * rec.CloseBalance;
                }
                //string txtDateTime = rawLine.Substring(58 - 1, 16);
                //string fieldname = "FromDate";
                //string pos = "58 - 1, 16";
                //DateTime date = BankAccountPageAnalyzer.TryGetDateTime(rawLine, txtDateTime, fieldname, pos);
                //rec.FromDate = date; ;


                //txtDateTime = rawLine.Substring(74 - 1, 16);
                //fieldname = "ToDate";
                //pos = "74 - 1, 16";
                //date = BankAccountPageAnalyzer.TryGetDateTime(rawLine, txtDateTime, fieldname, pos);
                //rec.ToDate = date; ;
                return rec;

            }
            catch (Exception e)
            {

                throw new Exception($"BankAccountDTO Create({rawLine})", e);
            }
        }
    }
    class BankPageLineDTO
    {
        private BankPageLineDTO()
        {

        }
        public string RawLine { get; set; }
        public string Reference { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public decimal Amount { get; private set; }
        public decimal BalanceAfter { get; private set; }

        public const string RowType = "033";
        public static BankPageLineDTO Create(string rawLine)
        {
            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                throw new Exception($"{rawLine} not start with  RowType={RowType} ");
            }

            var rec = new BankPageLineDTO();
            rec.RawLine = rawLine;
            rec.Reference = rawLine.Substring(4 - 1, 16).Trim();

            string txtDateTime = rawLine.Substring(87 - 1, 8);
            string fieldname = "ReferenceDate";
            string pos = "87 - 1, 8";
            DateTime date = BankAccountPageAnalyzer.TryGetDateTime(rawLine, txtDateTime, fieldname, pos, format: "yyyyMMdd");
            rec.ReferenceDate = date; ;

            string sign = rawLine.Substring(112 - 1, 1);
            decimal Amount = decimal.Parse(rawLine.Substring(112, 16 - 1)); //Format 14.2
            if (sign == "-")
            {
                Amount = -1 * Amount;
            }
            rec.Amount = Amount;


            sign = rawLine.Substring(128 - 1, 1);
            Amount = decimal.Parse(rawLine.Substring(128, 16 - 1)); //Format 14.2
            if (sign == "-")
            {
                Amount = -1 * Amount;
            }
            rec.BalanceAfter = Amount;


            return rec;
        }
    }
    class TenantPagesOfAccountDTO
    {
        public BankAccountPM DBBankaccountPM { get; internal set; }
        public List<BankPageDTO> PagesOfAccount { get; internal set; }
    }

}
