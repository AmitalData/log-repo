update QueueMessages set  Status=1 where QueueDefinitionCode ='AccountingJournalApproveWR' and Status=0

update [dbo].GLAccounts  set BalanceInLocalCurrency= null
truncate table [dbo].[GLAccountTotalByMonths]
truncate table   [dbo].ReconciliationLines
--select * from [dbo].Reconciliations
delete from  [dbo].Reconciliations
--select * from [dbo].LedgerTransactions
delete from  [dbo].LedgerTransactions
--update journals set queueid=null
delete from  [dbo].journallines
delete from  [dbo].Journals
