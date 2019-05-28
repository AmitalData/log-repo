using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL;
using Logitude.Accounting.BL.Validators;

namespace Logitude.Accounting.BL.CoreBL.Mapping
{


    public abstract class JournalLineMappingBase
    {
        protected JournalPM _JournalPM;
        protected JournalLinePM _JournalLine;

        public enum MappingTypeEnum
        {
            none,
            Credit,
            Debit,
            DebitTax

        }
        public abstract MappingTypeEnum MyMappingTypeEnum
        {
            get ;//{ return MappingTypeEnum.none; }
        }

        public JournalLineMappingBase(JournalLinePM journalLine, JournalPM journalPM, IGLAccountDataProvider myGLAccountPMProvider)
        {
            _GLAccountPMProvider =myGLAccountPMProvider;
            this._JournalLine = journalLine;
            this._JournalPM = journalPM;
            ValidationError = new List<string>();
            //MyLedgerTransaction = new List<LedgerTransactionPM>();

        }
        public void DoIt()
        {
            CheckJournal();
            
            DefaultMapLedgerTransactionPMFromJournal();
            MapIt();



            MyLedgerTransaction.LocalAmountCredit = System.Math.Round(MyLedgerTransaction.LocalAmountCredit , 2);
            MyLedgerTransaction.LocalAmountDebit = System.Math.Round(MyLedgerTransaction.LocalAmountDebit , 2);
            MyLedgerTransaction.ForeignAmountCredit = System.Math.Round(MyLedgerTransaction.ForeignAmountCredit , 2);
            MyLedgerTransaction.ForeignAmountDebit = System.Math.Round(MyLedgerTransaction.ForeignAmountDebit , 2);



            MyLedgerTransaction.OpenAmount = System.Math.Round(MyLedgerTransaction.OpenAmount, 2);//new Code Not Test
            


            MyGLAccountTotalByMonth = DefaultMapGLAccountTotalByMounth(MyLedgerTransaction);
            AddGLAccountTotalByMounth(MyGLAccountTotalByMonth, MyLedgerTransaction);

            MyGLAccountTotalByMonth.ForeignAmountCredit = System.Math.Round(MyGLAccountTotalByMonth.ForeignAmountCredit,2);
            MyGLAccountTotalByMonth.ForeignAmountDebit = System.Math.Round(MyGLAccountTotalByMonth.ForeignAmountDebit,2);
            MyGLAccountTotalByMonth.LocalAmountCredit = System.Math.Round(MyGLAccountTotalByMonth.LocalAmountCredit, 2);
            MyGLAccountTotalByMonth.LocalAmountDebit = System.Math.Round(MyGLAccountTotalByMonth.LocalAmountDebit, 2);


        }






        public static decimal? ToDbRound2(decimal? dec)
        {
            if (dec.HasValue)
            {
                decimal my = dec.Value;
                return (System.Math.Round(my, 2) as decimal?);
            }
            else
            {
                return null;
            }

        }
        protected abstract void AddGLAccountTotalByMounth(GLAccountTotalByMonthPM currGLAccountTotalByMounth, LedgerTransactionPM currLedgerTransaction);

        private GLAccountTotalByMonthPM GetCurrGLAccountTotalByMounth(List<GLAccountTotalByMonthPM> MyGLAccountTotalByMonths)
        {
            GLAccountTotalByMonthPM currGLAccountTotalByMounth = (from a in MyGLAccountTotalByMonths
                                                                  where a.AccountId == MyLedgerTransaction.AccountId &&
                                                a.CurrencyId == MyLedgerTransaction.CurrencyId &&
                                                a.Year == MyLedgerTransaction.AccountingDate.Year &&
                                                a.Month == MyLedgerTransaction.AccountingDate.Month
                                                                  select a).FirstOrDefault();

            if (currGLAccountTotalByMounth == null)
            {


                GLAccountTotalByMonthPM myGLAccountTotalByMonth = DefaultMapGLAccountTotalByMounth(MyLedgerTransaction);

                MyGLAccountTotalByMonths.Add(myGLAccountTotalByMonth);

            }
            return currGLAccountTotalByMounth;
        }
        private void CheckJournal()
        {
            var journalLineKey = string.Join(",", new string[] { _JournalLine.Tenant.ToString(), _JournalLine.JournalId });
            var journalKey = string.Join(",", new string[] { _JournalPM.Tenant.ToString(), _JournalPM.Id });
            if (journalLineKey != journalKey)
            {
                throw new Exception("if (journalLineKey !=journalKey )");
            }
            switch (_JournalLine.EnsureSettingActionTypeCodeEnum())
            {
                case MyJournalActionTypeEnum.Credit:
                    if (this.MyMappingTypeEnum != MappingTypeEnum.Credit)
                    {
                        throw new Exception("this.MyMappingTypeEnum != MappingTypeEnum.Credit");
                    }
                    break;
                case MyJournalActionTypeEnum.Debit:
                    if (this.MyMappingTypeEnum != MappingTypeEnum.Debit)
                    {
                        throw new Exception("this.MyMappingTypeEnum != MappingTypeEnum.Debit");
                    }
                    break;
                case MyJournalActionTypeEnum.DebitAndCredit:
                    if (this.MyMappingTypeEnum == MappingTypeEnum.Debit || this.MyMappingTypeEnum == MappingTypeEnum.Credit)
                    {

                    }
                    else
                    {
                        throw new Exception("not if (this.MyMappingTypeEnum == MappingTypeEnum.Debit || this.MyMappingTypeEnum == MappingTypeEnum.Credit)");
                    }
                    break;
                case MyJournalActionTypeEnum.DebitCreditAndVatdeduction:
                    // all ok 
                    break;
                case MyJournalActionTypeEnum.NotValid:
                    throw new Exception("case MyJournalActionTypeEnum.NotValid");
                default:
                    throw new Exception("JournalApproveParser():JournalActionType is must ");
                    break;
            }
        }

        protected abstract void MapIt();


        protected static GLAccountTotalByMonthPM DefaultMapGLAccountTotalByMounth(LedgerTransactionPM myTransaction)
        {

            GLAccountTotalByMonthPM myGLAccountTotalByMonth = new GLAccountTotalByMonthPM();
            myGLAccountTotalByMonth.ChangeSetOp = ChangeSetOperation.Insert;
            myGLAccountTotalByMonth.AccountId = myTransaction.AccountId;
            myGLAccountTotalByMonth.CurrencyId = myTransaction.CurrencyId;
            myGLAccountTotalByMonth.CurrencyName = myTransaction.CurrencyCode;
            myGLAccountTotalByMonth.Year = myTransaction.AccountingDate.Year;
            myGLAccountTotalByMonth.Month = myTransaction.AccountingDate.Month;
            myGLAccountTotalByMonth.Tenant = myTransaction.Tenant;

            myGLAccountTotalByMonth.LocalAmountCredit = 0;
            myGLAccountTotalByMonth.LocalAmountDebit = 0;

            myGLAccountTotalByMonth.ForeignAmountDebit = 0;
            myGLAccountTotalByMonth.ForeignAmountCredit = 0;
            return myGLAccountTotalByMonth;
        }

        protected LedgerTransactionPM DefaultMapLedgerTransactionPMFromJournal()
        {

            MyLedgerTransaction = new LedgerTransactionPM();

            MyLedgerTransaction.ChangeSetOp = ChangeSetOperation.Insert;
            MyLedgerTransaction.Id = null;// GetIdCounter(journalLine.Tenant);// IdCounter.GetNumber("LedgerTransaction", entityPM.Tenant);
            MyLedgerTransaction.Tenant = _JournalLine.Tenant;
            MyLedgerTransaction.JournalId = _JournalLine.JournalId;
            MyLedgerTransaction.JournalLineNumber = _JournalLine.Line;
            MyLedgerTransaction.CreateDate = _JournalPM.CreateDate;
            MyLedgerTransaction.AccountId = null;

            //MyLedgerTransaction.AccountingDate = _JournalPM.AccountingDate;
            if (_JournalLine.AccountingDate == DateTime.MinValue)
            {
                throw new Exception("_JournalLine.AccountingDate is must");//20180111-Bug 44298: באג בתאריך חשבונאי בהעברת פקודת יומן לתנועה
            }
            MyLedgerTransaction.AccountingDate = _JournalLine.AccountingDate;//20180111-Bug 44298: באג בתאריך חשבונאי בהעברת פקודת יומן לתנועה
            

                MyLedgerTransaction.DocumentDate = _JournalLine.DocumentDate;
            MyLedgerTransaction.DueDate = //(DateTime)
                _JournalLine.DueDate;
            MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;

            //MyLedgerTransactionPM.LocalAmountDebit = 0;
            //MyLedgerTransactionPM.LocalAmountCredit = 0;

            //MyLedgerTransactionPM.ForeignAmountDebit = 0;
            //MyLedgerTransactionPM.ForeignAmountCredit = 0;

            //MyLedgerTransactionPM.ExchangeRate = 0;

            MyLedgerTransaction.Reference1 = _JournalLine.Reference1;
            MyLedgerTransaction.Reference2 = _JournalLine.Reference2;
            MyLedgerTransaction.Reference3 = _JournalLine.Reference3;
            MyLedgerTransaction.Notes = _JournalLine.Notes;



            var refs = new HashSet<string>();
            AddIfNotNull(refs, _JournalPM.JournalNumber);
            AddIfNotNull(refs, _JournalPM.AccountingEntityReference);
            AddIfNotNull(refs, _JournalLine.Reference1);
            AddIfNotNull(refs, _JournalLine.Reference2);
            AddIfNotNull(refs, _JournalLine.Reference3);
            AddIfNotNull(refs, _JournalLine.Notes);
            MyLedgerTransaction.SearchFields= string.Join(",", refs.ToArray());

            MyLedgerTransaction.IsExternalReconcile = _JournalLine.IsExternalReconcile/*.GetValueOrDefault()*/;

            return MyLedgerTransaction;
        }
        void AddIfNotNull(HashSet<string>  refs,string toadd)
        {
            if (!string.IsNullOrWhiteSpace(toadd))
            {
                refs.Add(toadd);

            }
        }

        public LedgerTransactionPM MyLedgerTransaction { get; protected set; }
        public GLAccountTotalByMonthPM MyGLAccountTotalByMonth { get; protected set; }
        public List<string> ValidationError { get; protected set; }
        //public string localAccountingCurrencyId { get; set; }

        protected IGLAccountDataProvider _GLAccountPMProvider;
        //public virtual GLAccountPM GetGLAccountPM(string AccountId, int Tenant);

        protected void MapExternalOpenAmount()
        {
            if (_JournalLine.ExternalOpenAmount.HasValue)
            {
                //if (_JournalLine.ExternalOpenAmount.GetValueOrDefault() == 0)
                //{
                //    MyLedgerTransaction.IsReconciled = true;
                //    MyLedgerTransaction.OpenAmount = 0;
                //}
                //else
                //{
                //    MyLedgerTransaction.IsReconciled = false;
                //    MyLedgerTransaction.OpenAmount = _JournalLine.ExternalOpenAmount.GetValueOrDefault();
                //}
                MyLedgerTransaction.OpenAmount = _JournalLine.ExternalOpenAmount.GetValueOrDefault();

            }
            MyLedgerTransaction.IsExternalReconcile = _JournalLine.IsExternalReconcile;
        }


    }

}
