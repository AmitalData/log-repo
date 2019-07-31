
IF OBJECT_ID('[dbo].[AWBStocksDataView]', 'V') IS NOT NULL
drop VIEW [dbo].[AWBStocksDataView]
GO

IF OBJECT_ID('[dbo].[MessagingStockDataView]', 'V') IS NOT NULL

drop VIEW [dbo].[MessagingStockDataView]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[MessagingStockDataView]
AS
SELECT      
			 dbo.MessagingStocks.Id,				
			 dbo.MessagingStocks.TenantNumber,
			 dbo.MessagingStocks.StartDate,
			 dbo.MessagingStocks.EndDate,
			 dbo.MessagingStocks.Amount,	
			 dbo.MessagingStocks.Remaining,
			 dbo.MessagingStocks.IsCancelled,
			 dbo.MessagingStocks.Notes,
			 dbo.MessagingStocks.CreateDate,
			 dbo.MessagingStocks.UpdateDate,
			 dbo.MessagingStocks.CreatedByUserId,
			 dbo.MessagingStocks.UpdatedByUserId,
			 dbo.MessagingStocks.SearchFields,
			 dbo.MessagingStocks.TotalPrice,
			 dbo.MessagingStocks.StockType,
			 dbo.Tenants.Company as TenantName

FROM         dbo.MessagingStocks Inner join
             dbo.Tenants ON  dbo.MessagingStocks.TenantNumber = dbo.Tenants.Id 
