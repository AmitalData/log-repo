using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
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
        private List<BankPageM> _BankPages;

        public void Analyze(int tenant , string FileContent)
        {
            try
            {
                int? tenantFromPage4Tester = null;
                _BankPages = LoadBankPageFromFile(FileContent,out tenantFromPage4Tester);
                if (tenantFromPage4Tester.HasValue)
                {
                    tenant = tenantFromPage4Tester.Value;
                }
            }
            catch (Exception e)
            {

                throw new Exception("LoadBankPageFromFile(FileContent) failed ",e);
            }
            
            ValidateBankPageAgaintDB(tenant,_BankPages);
        }
        List<string> ValidateBankPageAgaintDBErrors = new List<string>();
        private ReconcileExternalPageQueryService _ReconcileExternalPageQueryService;
        

        private void ValidateBankPageAgaintDB(int tenant, List<BankPageM> bankPages)
        {

            var bankcodeQS = new BankCodeQueryService(tenant);
            var  bankaccountQS = new BankAccountQueryService(tenant);
            _ReconcileExternalPageQueryService = new ReconcileExternalPageQueryService(tenant);
            
            var bankPagesGBAccountNumber = (from bp in bankPages
                      group bp by new { bp.BankCode, bp.MyBankAccountM.AccountNumber }
                     );
            foreach (var pagesOfAccount in bankPagesGBAccountNumber)
            {
                
                var bankcodePM =bankcodeQS.GetSingle(pagesOfAccount.Key.BankCode, false, true);
                if (bankcodePM == null)
                {
                    ValidateBankPageAgaintDBErrors.Add($"BankCode {pagesOfAccount.Key.BankCode} not exist in Tenant {pagesOfAccount.First().RawLine}");
                    continue;
                }
                var dbBankaccountPM =bankaccountQS.GetBankAccountByBankIdAccNumber(bankcodePM.Id,pagesOfAccount.Key.AccountNumber,tenant);
                if (dbBankaccountPM == null)
                {
                    ValidateBankPageAgaintDBErrors.Add($"BankCode {pagesOfAccount.Key.BankCode}  ,AccountNumber {pagesOfAccount.Key.AccountNumber} not exist in Tenant  {pagesOfAccount.First().RawLine}" );
                    continue;
                }
                
                foreach (var newPageOfBankAccount in pagesOfAccount.OrderBy(r=>r.MyBankAccountM.PageNo))
                {
                    AnalyzeeNewPage(tenant, dbBankaccountPM, newPageOfBankAccount);
                    
                }
            }
        }

        private void AnalyzeeNewPage(int tenant,  BankAccountPM dbBankaccountPM, BankPageM newPageOfBankAccount)
        {
            string errorPageValidation = PageValidationClientSide(newPageOfBankAccount);
            if (!string.IsNullOrWhiteSpace(errorPageValidation))
            {
                ValidateBankPageAgaintDBErrors.Add($"PageValidationClientSide():BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  {errorPageValidation}  ");
                return;
            }
            if (dbBankaccountPM.LastPageCloseBalance != newPageOfBankAccount.MyBankAccountM.OpenBalance)
            {
                ValidateBankPageAgaintDBErrors.Add($"BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  dbBankaccountPM.LastPageCloseBalance {dbBankaccountPM.LastPageCloseBalance} != OpenBalance  {newPageOfBankAccount.MyBankAccountM.OpenBalance}");
                return;
            }
            var prevReconcileExternalPagePM = _ReconcileExternalPageQueryService.GetPrevPageNoByPageNo(int.Parse(dbBankaccountPM.LastPageNumber), dbBankaccountPM.Id, tenant);

            
            if (dbBankaccountPM.LastPageCloseBalance != newPageOfBankAccount.MyBankAccountM.OpenBalance)
            {
                ValidateBankPageAgaintDBErrors.Add($"BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  dbBankaccountPM.LastPageCloseBalance {dbBankaccountPM.LastPageCloseBalance} != OpenBalance  {newPageOfBankAccount.MyBankAccountM.OpenBalance}");
                return;
            }

            var newBankPageLines = newPageOfBankAccount.GetCopyOfBankPageLines();
            if (prevReconcileExternalPagePM != null)
            {
                
                if (prevReconcileExternalPagePM.ToDate >= newBankPageLines.First().ReferenceDate)
                {
                    ValidateBankPageAgaintDBErrors.Add($"BankCode {newPageOfBankAccount.BankCode}  ,AccountNumber {newPageOfBankAccount.MyBankAccountM.AccountNumber} pageNo {newPageOfBankAccount.MyBankAccountM.PageNo} >  prevReconcileExternalPagePM.ToDate {prevReconcileExternalPagePM.ToDate } >= newBankPageLines.First().ReferenceDate{newBankPageLines.First().ReferenceDate}");
                    return;
                }

            }


            ReconcileExternalPagePM entityPM = MapReconcileExternalPagePM(tenant, newPageOfBankAccount, newBankPageLines, dbBankaccountPM);

            try
            {
                using (var scope = TransactionFactory.GetTransaction())
                {

                    var MyContext = AccountingContext.GetContext(entityPM.Tenant);
                    ReconcileExternalPageUpdateService service = new ReconcileExternalPageUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                    entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    service.Update(entityPM, true);

                    scope.Complete();
                    this.AddSuccess(entityPM , newPageOfBankAccount);
                }
            }

            catch (Exception ex)
            {
                this.AddInsertException(entityPM, newPageOfBankAccount, ex);
            }

        }

        private static ReconcileExternalPagePM MapReconcileExternalPagePM(int tenant, 
            BankPageM newPageOfBankAccount, 
             List<BankPageLineM> newBankPageLines,
             
             BankAccountPM dbBankaccountPM
            )
        {

            
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
        public List<string> SuccessPageList = new List<string>();
        public List<string> ExceptionPageList = new List<string>();

        private void AddInsertException(ReconcileExternalPagePM entityPM, BankPageM newPageOfBankAccount, Exception ex)
        {
            this.SuccessPageList.Add($"Exception insert Page BankCode:{newPageOfBankAccount.BankCode}/AccountNumber{newPageOfBankAccount.MyBankAccountM.AccountNumber}/{newPageOfBankAccount.MyBankAccountM.PageNo} >{ex.ToString()} ");

        }
        
        private void AddSuccess(ReconcileExternalPagePM entityPM, BankPageM newPageOfBankAccount)
        {
            this.SuccessPageList.Add($"Success insert Page BankCode:{newPageOfBankAccount.BankCode}/AccountNumber{newPageOfBankAccount.MyBankAccountM.AccountNumber}/{newPageOfBankAccount.MyBankAccountM.PageNo} =new DbId:{entityPM.Id}/DBPageNo:{entityPM.PageNo}  ");
        }

        private string PageValidationClientSide(BankPageM pageOfAccount)
        {
            var CopyOfBankPageLines = pageOfAccount.GetCopyOfBankPageLines();
            if (CopyOfBankPageLines.Count == 0)
            {
                return $"BankPageLines.Count == 0  (client sidecheck )";
            }
            var lineWithoutRef= CopyOfBankPageLines.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Reference));
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

        
        private List<BankPageM> LoadBankPageFromFile(string FileContent,out int? tenant)
        {
            tenant = null;
            var bankPages = new List<BankPageM>();
            var lines = FileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            
            BankPageM bankPage = null;
            foreach (string rawLine in lines)
            {
                if (rawLine.StartsWith("//"))//edi
                {
                    if (rawLine.StartsWith("//Tenant="))//for tester 
                    {
                        string tenantS=rawLine.Split(new string[] { "//Tenant=" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        tenant = int.Parse(tenantS);
                    }
                    continue;// remark do nothing ...
                }
                var rowtype = rawLine.Substring(0, 3);
                switch (rowtype)
                {
                    case BankPageM.RowType:
                        {
                            bankPage = BankPageM.Create(rawLine);
                            bankPages.Add(bankPage);
                        }
                        break;
                    case BankAccountM.RowType:
                        {
                            if (bankPage == null)
                            {
                                throw new Exception($"{rawLine} BankAccountM.RowType arrived b4 BankPageM.RowType ");
                            }
                            bankPage.AddBankAccountM(rawLine);
                        }
                        break;

                    case BankPageLineM.RowType:
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

        public static DateTime TryGetDateTime(string rawLine, string txtDateTime, string fieldname, string pos,string @format= "yyyyMMddHHmm")
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
    class BankPageM
    {
        public const string RowType = "031";
        public string RawLine { get; set; }
        public BankAccountM MyBankAccountM { get; private set; }
        List<BankPageLineM> MyBankPageLines { get; set; }
        public string BankCode { get; private set; }
        //public string BranchNumber { get; private set; }
        //public string AccountNumber { get; private set; }
        public DateTime CreateDate { get; private set; }
        

        internal static BankPageM Create(string rawLine)
        {

            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                throw new Exception($"{rawLine} not start with  RowType={RowType} ");
            }
            
            var rec = new BankPageM();
            rec.RawLine = rawLine;
            rec.MyBankPageLines = new List<BankPageLineM>();
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
            this.MyBankAccountM = BankAccountM.Create(rawLine);
            if (this.MyBankAccountM.BankCode != this.BankCode)
            {
                throw new
                    Exception($"{rawLine} AddBankAccountM (this.MyBankAccountM.BankCode != this.BankCode)");

            }
            
            

        }
        internal List<BankPageLineM> GetCopyOfBankPageLines()
        {
            return new List<BankPageLineM>(this.MyBankPageLines);
        }
        internal void AddBankPageLineM(string rawLine)
        {
            BankPageLineM myBankPageLineM = BankPageLineM.Create(rawLine);
            decimal totAmount =this.MyBankAccountM.OpenBalance + MyBankPageLines.Sum(r => r.Amount);
            decimal totAmountIncludeCurrPage = totAmount + myBankPageLineM.Amount;
            if (totAmountIncludeCurrPage!= myBankPageLineM.BalanceAfter)
            {
                throw new
                    Exception($"{rawLine} AddBankPageLineM (totAmountIncludeCurrPage!= myBankPageLineM.BalanceAfter)");
            }
            MyBankPageLines.Add(myBankPageLineM);
        }
    }
    class BankAccountM
    {
        public string RawLine { get; set; }
        public string BankCode { get; private set; }
        public string BranchNumber { get; private set; }
        public string AccountNumber { get; private set; }
        public int PageNo { get; private set; }
        public decimal OpenBalance { get; private set; }
        public decimal CloseBalance { get; private set; }

        //public DateTime FromDate { get; private set; }
        //public DateTime ToDate { get; private set; }

        public const string RowType = "032";

        internal static BankAccountM Create(string rawLine)
        {
            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                throw new Exception($"{rawLine} not start with  RowType={RowType} ");
            }
            var rec = new BankAccountM();
            rec.BankCode= rawLine.Substring(4 - 1, 3);
            rec.BankCode = (rec.BankCode ?? "").Trim();
            rec.BranchNumber = rawLine.Substring(7 - 1, 4).Trim();

            rec.AccountNumber = rawLine.Substring(11 - 1, 16).Trim();
            rec.PageNo = int.Parse(rawLine.Substring(50 - 1, 8));


            rec.OpenBalance = decimal.Parse(rawLine.Substring(58 - 1, 16));
            rec.CloseBalance = decimal.Parse(rawLine.Substring(74 - 1, 16));
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
    }
    class BankPageLineM
    {
        private BankPageLineM()
        {

        }
        public string RawLine { get; set; }
        public string Reference { get; private set; }
        public DateTime ReferenceDate { get; private set; }
        public decimal Amount { get; private set; }
        public decimal BalanceAfter { get; private set; }

        public const string RowType = "033";
        public static BankPageLineM Create(string rawLine)
        {
            rawLine = rawLine ?? "";
            if (!rawLine.StartsWith(RowType))
            {
                throw new Exception($"{rawLine} not start with  RowType={RowType} ");
            }
            
            var rec = new BankPageLineM();
            rec.RawLine = rawLine;
            rec.Reference= rawLine.Substring(4 - 1, 16).Trim();

            string txtDateTime = rawLine.Substring(87 - 1, 8);
            string fieldname = "ReferenceDate";
            string pos = "87 - 1, 8";
            DateTime date = BankAccountPageAnalyzer.TryGetDateTime(rawLine, txtDateTime, fieldname, pos,format:  "yyyyMMdd");
            rec.ReferenceDate = date; ;

            string sign = rawLine.Substring(112 - 1, 1);
            decimal Amount =  decimal.Parse(rawLine.Substring(112 , 16-1)); //Format 14.2
            if (sign=="-")
            {
                Amount = -1 * Amount;
            }
            rec.Amount = Amount;


            sign = rawLine.Substring(128 - 1, 1);
            Amount=decimal.Parse(rawLine.Substring(128 , 16-1)); //Format 14.2
            if (sign == "-")
            {
                Amount = -1 * Amount;
            }
            rec.BalanceAfter = Amount;


            return rec;
        }
    }

}
