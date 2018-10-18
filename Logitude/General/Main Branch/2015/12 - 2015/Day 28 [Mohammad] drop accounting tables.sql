
drop table dbo.PeriodTypes
drop table dbo.AccountingPeriods
drop table dbo.AutomaticReconcileMethods
drop table dbo.AutomaticReconciles

drop table dbo.JournalLines
drop table dbo.Journals
drop table dbo.JournalTypes
drop table dbo.JournalStatusTypes
drop table dbo.JournalActionTypes
drop table dbo.AccountingEntities
drop table dbo.LedgerTransactions
drop table dbo.TestEntities
drop index dbo.GlAccounts.IX_ControlAccountId 
alter table dbo.GlAccounts drop constraint [FK_dbo.GLAccounts_dbo.ChartOfAccounts_ChartOfAccountsId]
alter table dbo.GlAccounts drop constraint [FK_dbo.GLAccounts_dbo.GLAccounts_ControlAccountId] 
drop table dbo.ChartOfAccounts
drop table dbo.GLAccountBalanceByYears
drop table dbo.GLAccountTotalByMonths
drop table dbo.GLAccounts


drop table dbo.RevenueExpenseTypes
drop table dbo.ReconcileMethods
drop table dbo.GLAccountTypes

drop table dbo.ChartOfAccountsTypes




drop table dbo.ReconciliationLines
drop table dbo.Reconciliations