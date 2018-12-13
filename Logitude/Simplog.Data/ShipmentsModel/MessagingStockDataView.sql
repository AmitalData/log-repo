drop VIEW [dbo].[AWBStocksDataView] 
go


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[AWBStocksDataView]
AS
SELECT      
			 dbo.AWBMessagingStocks.Id,				
			 dbo.AWBMessagingStocks.TenantNumber,
			 dbo.AWBMessagingStocks.StartDate,
			 dbo.AWBMessagingStocks.EndDate,
			 dbo.AWBMessagingStocks.Amount,	
			 dbo.AWBMessagingStocks.Remaining,
			 dbo.AWBMessagingStocks.IsCancelled,
			 dbo.AWBMessagingStocks.Notes,
			 dbo.AWBMessagingStocks.CreateDate,
			 dbo.AWBMessagingStocks.UpdateDate,
			 dbo.AWBMessagingStocks.CreatedByUserId,
			 dbo.AWBMessagingStocks.UpdatedByUserId,
			 dbo.AWBMessagingStocks.SearchFields,
			 dbo.AWBMessagingStocks.TotalPrice,
			 dbo.Tenants.Company as TenantName

FROM         dbo.AWBMessagingStocks Inner join
             dbo.Tenants ON  dbo.AWBMessagingStocks.TenantNumber = dbo.Tenants.Id 
