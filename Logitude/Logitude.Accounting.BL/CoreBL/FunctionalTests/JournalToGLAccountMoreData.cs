
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.FunctionalTests
{
    public class JournalToGLAccountMoreData
    {
        private int _Tenant;

        public void BuildJournals(int tenant ,byte[] byteArrayXLS, string WorksheetName)
        {
            _Tenant = tenant;
            var util = new XLSUtil();
            DataTable XLSTable = util.GetDataTableFromWorkSheet(byteArrayXLS, "Journals");


            List<DataRow> list = XLSTable.Rows.Cast<DataRow>().ToList();
            var JournalInputRows = list
                .Where(r => !String.IsNullOrWhiteSpace(r.Field<String>("Counter"))).ToList();
            JournalInputRows = JournalInputRows.OrderBy(r => r.Field<String>("Counter")).ToList();
            ;
            var JournalInput = JournalInputRows.GroupBy(r => r.Field<String>("Counter")).Select(g => GetJournalPM(g)).ToList();

            bool @buildJournals = false;
            if (@buildJournals)
            {
                BuildJournals(tenant, JournalInput);
            }
            
            var GLAccountOutputRows = list
                .Where(r => !String.IsNullOrWhiteSpace(r.Field<String>("GLA")))
                .ToList()
                ;

            List<string> res = GLAccountOutputRows.Select(gLAccountOutput => CheckGLAccount(gLAccountOutput)).ToList();
            
        }

        private string CheckGLAccount(DataRow gLAccountOutput)
        {
            var DisplayNumber  = gLAccountOutput.Field<string>("GLA");
            var BalanceNIS=toDecimal(gLAccountOutput.Field<string>("BalanceNIS"));
            var BalanceNISDue = toDecimal(gLAccountOutput.Field<string>("BalanceNISDue"));
            
            var qs = new GLAccountQueryService(_Tenant);
            var glAccountPM = qs.GetSinglePMByDisplayNumber(DisplayNumber, _Tenant); ;
            if (glAccountPM == null)
            {
                throw new Exception($"GetGLAccountByDisplayNumber({DisplayNumber}, {_Tenant}) return null");
            }
            string res = null;
            if (glAccountPM.LocalBalanceInDue!= BalanceNISDue)
            {
                res = $"LocalBalanceInDue{glAccountPM.LocalBalanceInDue}!= BalanceNISDue{BalanceNISDue}";
            }
            if (glAccountPM.BalanceInLocalCurrency != BalanceNIS)
            {
                res = $"BalanceInLocalCurrency{glAccountPM.BalanceInLocalCurrency}!= BalanceNIS{BalanceNIS}";
            }
            return $"Card {DisplayNumber}:" + (res ?? " Totals r equal");
        }

        private static void BuildJournals(int tenant, List<JournalPM> JournalInput)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {
                foreach (var entityPM in JournalInput)
                {
                    var MyContext = AccountingContext.GetContext(tenant);
                    JournalQueryService mappingService = new JournalQueryService(tenant);



                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    JournalUpdateService service = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    service.Update(entityPM, true);

                }
                scope.Complete();

            }
        }

        private JournalPM GetJournalPM(IGrouping<string, DataRow> g)
        {
            var id = g.Key;

            var journaLines = g.Select(r => GetJournalLine(r)).ToList();
            var journal = new JournalPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = _Tenant,
                AccountingDate = journaLines.First().AccountingDate,
                StatusCodeEnum = JournalStatusTypePM.StatusCodeEnum.Approved,
                JournalLines = journaLines
            };
            return journal;
        }

        private JournalLinePM GetJournalLine(DataRow r)
        {
            var line = new JournalLinePM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                Tenant = _Tenant,
                ActionTypeCodeEnum = ToMyJournalActionTypeEnum(r.Field<String>("Debit/Credit")),
                CreditAccountId = GetCreditAccountId(r.Field<String>("Debit/Credit"), r.Field<String>("Account")),

                DebitAccountId = GetDebitAccountId(r.Field<String>("Debit/Credit"), r.Field<String>("Account")),

                AccountingDate = ToDate(r.Field<String>("AccountingDate")),
                DueDate = ToDate(r.Field<String>("ValueDate")),
                DocumentDate = ToDate(r.Field<String>("AccountingDate")),

                LocalAmount = toDecimal(r.Field<string>("AmountNIS")),
                CurrencyCode = r.Field<string>("Currency"),
                ForeignAmount = toDecimal(r.Field<string>("AmountForeign")),

                Reference1 = r.Field<string>("Reference1"),
                Reference2 = r.Field<string>("Reference2"),
                Reference3 = r.Field<string>("Reference3"),

            };
            return line;
        }

        private string GetDebitAccountId(string v1, string v2)
        {
            if (ToMyJournalActionTypeEnum(v1) == MyJournalActionTypeEnum.Credit)
            {
                return null;
            }
            var qs = new GLAccountQueryService(_Tenant);
            var glAccountPM = qs.GetSinglePMByDisplayNumber(v2, _Tenant); ;
            if (glAccountPM == null)
            {
                throw new Exception($"GetGLAccountByDisplayNumber({v2}, {_Tenant}) return null");
            }
            return glAccountPM.Id;
        }

        private string GetCreditAccountId(string v1, string v2)
        {
            if (ToMyJournalActionTypeEnum(v1) == MyJournalActionTypeEnum.Debit)
            {
                return null;
            }
            var qs = new GLAccountQueryService(_Tenant);
            var glAccountPM = qs.GetSinglePMByDisplayNumber(v2, _Tenant); ;
            if (glAccountPM == null)
            {
                throw new Exception($"GetGLAccountByDisplayNumber({v2}, {_Tenant}) return null");
            }
            return glAccountPM.Id;
        }

        private decimal toDecimal(string v)
        {
            decimal d;
            var done = decimal.TryParse(v, out d);
            if (!done)
            {
                throw new Exception("unable to cast decimal from val:{v}");
            }
            return d;

        }

        private MyJournalActionTypeEnum ToMyJournalActionTypeEnum(string v)
        {
            switch (v)
            {
                case "C": { return MyJournalActionTypeEnum.Credit; } break;
                case "D": { return MyJournalActionTypeEnum.Debit; } break;
                default:
                    throw new Exception("Debit/Credit must be D/C");
                    break;
            }

        }

        private DateTime ToDate(string v)
        {
            return
            DateTime.ParseExact(v, "dd.MM.yy", CultureInfo.InvariantCulture,
                                                        DateTimeStyles.None);
        }

       


    }

   
}