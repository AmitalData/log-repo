using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTestDB
{
    class ClearDB
    {
        //https://drive.google.com/file/d/0B3IZ3w8Z6js5MkdyYnhmRXhsUzA/view
        string TheSql()
        {
            return
@"
delete fullaccountingsettings where tenant = 1
delete  ReconciliationLines where tenant = 1
delete  Reconciliations where tenant = 1
delete  LedgerTransactions where tenant = 1

delete  ReconciliationLines WHERE TENANT=1
delete  Reconciliations WHERE TENANT=1
delete journalReconciles where tenant = 1
delete journallines where tenant = 1
delete Journals where tenant = 1
delete CashBookLines where tenant = 1

delete CashBooks where tenant = 1

delete ReconcileExternalPages where tenant = 1
delete BankAccounts where tenant = 1

--update CashBooks set AccountId =''  where tenant = 1
--update BankAccounts set DeferredGLAccountId ='' where tenant = 1
--update BankAccounts set GLAccountId ='' where tenant = 1
delete BankDepositLines where tenant = 1
delete BankDeposits where tenant = 1
delete ARPaymentCheques where tenant = 1
update ChargesTypes set ReceivableCreditGLAccountId=''  where tenant = 1 and ReceivableCreditGLAccountId <>''
update ChargesTypes set PayableDebitGLAcountId='' where tenant = 1  and PayableDebitGLAcountId <> ''

delete GLAccountTotalByMonths where tenant = 1
delete GLAccountCurrencies where tenant = 1

update GLAccounts set controlAccountId =NULL where controlAccountId is not null

delete GLAccounts where (IsControlAccount is null or IsControlAccount=0) and tenant = 1 
and id not in (select VATInputsGLAccountId from  FullAccountingSettings where tenant = 1)
and id not in (select VATOutputGLAccountId from  FullAccountingSettings where tenant = 1)
and id not in (select RevenueExpenseGLAccountId from  FullAccountingSettings where tenant = 1)
 and id not in (select DeferredGLAccountId from BankAccounts) 
and id not in (select GLAccountId from BankAccounts) AND  id not in (select AccountId from CashBooks) 

update GLAccounts set PreviousChartOfAccountsId =NULL where PreviousChartOfAccountsId is not null

delete ChartOfAccounts where id not in (select ChartOfAccountsId from glaccounts where tenant = 1) AND Tenant = 1



update cards set glaccountid = '' where glaccountid is not null and tenant = 1
update GLAccountMoreDatas set BalanceInLocalCurrency = null,LocalBalanceInDue= null,NextDueDate=null where tenant = 1

update QueueMessages set Status='1'  where QueueDefinitionCode='AccountingJournalApproveWR' and status ='0'

delete   FROM [DBIdCounters]  WHERE [TableName] in ( 'LedgerTransaction',  'Journal' ,'GLAccount')
DELETE  FullAccountingSettings where tenant = 1
DELETE GLAccountMoreDatas WHERE TENANT=1
DELETE GLAccountS WHERE TENANT=1
DELETE ChartOfAccountS WHERE TENANT=1

";
        }

        String ClearStream()
        {
            return @"


UPDATE [dbo].[Journals] SET [QueueId]= NULL WHERE TENANT=1

update GLAccountMoreDatas set BalanceInLocalCurrency = 0 where  BalanceInLocalCurrency != 0 

delete  ReconciliationLines WHERE TENANT=1
delete  LedgerTransactions WHERE TENANT=1
delete   GLAccountTotalByMonths WHERE TENANT=1

select * from  GLAccountMoreDatas where  NextDueDate is not null
select * from  GLAccountMoreDatas where  BalanceInLocalCurrency != 0 


SELECT *  FROM [dbo].[Journals]  WHERE TENANT=1 AND  [QueueId] IS NOT NULL
SELECT *  FROM LedgerTransactions WHERE TENANT=1

SELECT *  FROM GLAccountTotalByMonths WHERE TENANT=1

delete from fullAccountingSettings WHERE TENANT=1

delete from GLAccountMoreDatas WHERE TENANT=1
delete from GLAccounts WHERE TENANT=1
delete from ChartOfAccounts WHERE TENANT=1

---delete AccountingPeriods where tenant = 1





";
        }
    }
}
