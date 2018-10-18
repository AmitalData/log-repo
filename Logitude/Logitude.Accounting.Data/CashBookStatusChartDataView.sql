
if exists(select 1 from sys.views where name='CashBookStatusChartDataView' and type='v')
drop VIEW [dbo].[CashBookStatusChartDataView] 
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CashBookStatusChartDataView]

AS

SELECT      CashBooks.Id,
			CashBooks.Tenant,
			 ARPaymentCheques.ValueDate ChequeValueDate,
			  ARPaymentCheques.LocalAmount ChequeAmount,
			   CashBooks.TotalAmount, CashBooks.CurrencyId,
				CashBooks.CashBookTypeCode, 
				CashBookLines.IsDeposited,
                  Currencies.Code as CurrencyCode
FROM				CashBooks inner join
                      Currencies ON CashBooks.CurrencyId = Currencies.Id left outer join
                      CashBookLines ON CashBookLines.CashBookId = CashBooks.Id left outer join
                      ARPaymentCheques ON CashBookLines.ARPChequeId = ARPaymentCheques.Id
WHERE  CashBookLines.IsDeposited = 0 or CashBookLines.IsDeposited is null   

GO


