drop VIEW [dbo].[ImporterDepositionVendorsView] 
go

/****** Object:  View [dbo].[ImporterDepositionVendorsView]    Script Date: 10/11/2014 12:21:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ImporterDepositionVendorsView]
AS
SELECT        Customs.ImporterDespositions.ImporterDeclarationStatusCode, Customs.ImporterDespositions.ImporterlId, Customs.ImporterDespositions.VendorID, 
                         Customs.ImporterDespositions.StartDate, Customs.ImporterDespositions.EndDate, Customs.ImporterDespositions.NotesToAgent, 
                         Customs.ImporterDespositions.ErrorMessage, Customs.ImporterDespositions.Tenant, Customs.ImporterDespositions.Id AS Expr1, 
                         Customs.ImporterDespositions.DepositionNumber, Customs.CustomsVendors.VendorName, Customs.CustomsVendors.CountryCode, 
                         Customs.CustomsVendors.CityName, Customs.CustomsVendors.MainAddressLine, Customs.CustomsVendors.VATNumber, Customs.CustomsVendors.DunsNumber, 
                         Customs.CustomsVendors.VendorNumber, Customs.CustomsVendors.Id
FROM            Customs.ImporterDespositions INNER JOIN
                         Customs.CustomsVendors ON Customs.CustomsVendors.Id = Customs.ImporterDespositions.VendorID


GO
