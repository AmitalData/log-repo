
drop VIEW [dbo].[QuoteFollowUpDataView]
go

/****** Object:  View [dbo].[QuoteFollowUpDataView]    Script Date: 02/21/2013 11:42:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[QuoteFollowUpDataView]
AS
SELECT        dbo.Quotes.Id, dbo.Quotes.Tenant, dbo.Quotes.QuoteNumber, dbo.Quotes.ShipperReference1, dbo.Quotes.ShipperReference2, dbo.Quotes.ConsigneeReference1, 
                         dbo.Quotes.ConsigneeReference2, dbo.Quotes.OpenDate, dbo.Quotes.Notes, dbo.Quotes.DescriptionOfGoods, dbo.Quotes.IsClosed, dbo.Quotes.ChargeableWeight, 
                         dbo.Quotes.GrossWeight, dbo.Quotes.GrossWeightInKG, dbo.Quotes.GrossWeightPerTon, dbo.Quotes.LastModified, dbo.Quotes.Field1, dbo.Quotes.Field2, dbo.Quotes.Field3, dbo.Quotes.Field4, dbo.Quotes.Field5, 
                         dbo.Quotes.Field6, dbo.Quotes.Field7, dbo.Quotes.Field8, dbo.Quotes.Field9, dbo.Quotes.Field10, dbo.Quotes.DimensionsUnitCode, 
                         dbo.Quotes.GrossWeightUnitCode, dbo.Quotes.Volume, dbo.Quotes.NumberOfContainers, dbo.Quotes.NumberOfPackages, dbo.Quotes.Ratio, 
                         dbo.Quotes.VolumeUnitCode, dbo.Quotes.ShipmentTypeId, dbo.Quotes.ShipperId, dbo.Quotes.ConsigneeId, 
						 dbo.Quotes.BusinessUnitId, dbo.Quotes.QuoteClosingReasonCode, dbo.Quotes.ValueOfGoods,
                         dbo.Quotes.ShipperContactId, dbo.Quotes.ConsigneeContactId, dbo.Quotes.FromPortId, dbo.Quotes.ToPortId, dbo.Quotes.ProductCode,
                         dbo.Quotes.IncotermId, dbo.Quotes.SalesmanUserId, dbo.Quotes.CreatedByUserId, dbo.Quotes.DirectionId, dbo.Quotes.TransportModeId, dbo.Quotes.IsDangerous, 
                         dbo.Quotes.ExpirationDays, dbo.Quotes.ExpirationDate, dbo.Quotes.VolumetricWeight, dbo.Quotes.StageId, dbo.Quotes.StageDueDate, dbo.Quotes.BranchId, dbo.Quotes.DepartmentId, 
                         dbo.Quotes.PackageType1Id, dbo.Quotes.PackageType2Id, dbo.Quotes.PackageType3Id, dbo.Quotes.PackageType4Id, dbo.Quotes.PackageType5Id, 
                         dbo.Quotes.PackageType1Quantity, dbo.Quotes.PackageType3Quantity, dbo.Quotes.PackageType2Quantity, dbo.Quotes.PackageType4Quantity, 
                         dbo.Quotes.PackageType5Quantity, dbo.Quotes.IsByKG, dbo.Quotes.IsByContainer, dbo.Quotes.QuoteTypeCode, dbo.Quotes.EstimateProfit, 
                         dbo.Quotes.EstimateProfitEdited, dbo.Quotes.MinimumFreightCost, dbo.Quotes.MinimumFreightSale, dbo.Quotes.MainCarriageCarrierId, 
                         dbo.Quotes.IsFreightBySteps, dbo.Quotes.IsCancelled, dbo.Quotes.CustomerId, 
                         dbo.Quotes.CustomerContactId, dbo.Quotes.CustomerReference1, dbo.Quotes.CustomerReference2, dbo.Quotes.QuoteCustomerTypeCode, dbo.Quotes.CustomerName,
                         dbo.Quotes.SaleCurrencyId, dbo.Quotes.ExchangeRate, dbo.Quotes.ShipperName, dbo.Quotes.Subject, dbo.Quotes.IsSubjectEdited,
						 dbo.Quotes.LastActivityDate, dbo.Quotes.LastActivitySubject, dbo.Quotes.LastActivityTypeCode,
						 dbo.Quotes.NextActivityDate, dbo.Quotes.NextActivitySubject, dbo.Quotes.NextActivityTypeCode,						 
						 dbo.Quotes.UpdateDate, dbo.Quotes.IsAutomaticallyClosed, dbo.Quotes.AutomaticallyCloseDate, dbo.Quotes.AutomaticallyCloseDays,
                         dbo.Quotes.ConsigneeName, dbo.Quotes.PickUpAddress, dbo.Quotes.DeliveryAddress, dbo.Quotes.RatingCode, dbo.Quotes.OpportunityId,
                         dbo.Quotes.IsFixedPrice, dbo.Quotes.SearchFields, dbo.Quotes.ChargeableWeightUnitCode, dbo.Quotes.NumberOfFollowUps,
                         dbo.Quotes.ConcurrencyGUID, dbo.Quotes.FromPartnerId, dbo.Quotes.ToPartnerId, dbo.Quotes.FromPartnerAddressId, dbo.Quotes.ToPartnerAddressId, 
                         dbo.FollowUps.Id AS FollowUpId, dbo.FollowUps.Date AS FollowUpDate, dbo.FollowUps.Notes AS FollowUpNotes, 
                         dbo.FollowUps.OwnerUserId AS FollowUpOwnerUserId, ShipperCards.EnglishName AS Shipper, ConsigneeCards.EnglishName AS Consignee, 
                         FollowUpTypes.FollowUpEnglishName AS FollowUpType, FromPorts.Code AS FromPortCode, FromPorts.EnglishName AS FromPortName, 
                         FromPortCountries.EnglishName AS FromPortCountry, ToPorts.Code AS ToPortCode, ToPorts.EnglishName AS ToPortName, 
                         ToPortCountries.EnglishName AS ToPortCountry, dbo.Stages.Name AS StageName, CreateByContacts.EnglishName AS CreatedByUser, 
                         OwnerContacts.EnglishName AS FollowUpOwner, dbo.QuoteTypes.Name AS QuoteTypeName, FollowUpTypes.Id AS FollowUpTypeId, 
                         OwnerContacts.Id AS FollowUpOwnerId, MainCarriageCarriers.EnglishName AS MainCarriageCarrierName, dbo.ShipmentTypes.Name AS ShipmentTypeName,
						 dbo.BusinessUnits.Name AS BusinessUnitName, dbo.QuoteClosingReasons.Name AS QuoteClosingReasonName,
						 dbo.Incoterms.Code as IncotermCode

FROM            dbo.Quotes INNER JOIN
                         dbo.FollowUps ON dbo.Quotes.Id = dbo.FollowUps.QuoteId LEFT OUTER JOIN
                         dbo.Cards AS ShipperCards ON dbo.Quotes.ShipperId = ShipperCards.Id LEFT OUTER JOIN
                         dbo.Cards AS ConsigneeCards ON dbo.Quotes.ConsigneeId = ConsigneeCards.Id LEFT OUTER JOIN
                         dbo.EventTypes AS FollowUpTypes ON dbo.FollowUps.EventTypeId = FollowUpTypes.Id LEFT OUTER JOIN
                         dbo.Ports AS FromPorts ON dbo.Quotes.FromPortId = FromPorts.Id LEFT OUTER JOIN
                         dbo.Ports AS ToPorts ON dbo.Quotes.ToPortId = ToPorts.Id LEFT OUTER JOIN
                         dbo.Countries AS FromPortCountries ON FromPorts.CountryId = FromPortCountries.Id LEFT OUTER JOIN
                         dbo.Countries AS ToPortCountries ON ToPorts.CountryId = ToPortCountries.Id LEFT OUTER JOIN
                         dbo.Stages ON dbo.Quotes.StageId = dbo.Stages.Id LEFT OUTER JOIN
                         dbo.Users AS CreateByUsers ON dbo.Quotes.CreatedByUserId = CreateByUsers.Id LEFT OUTER JOIN
                         dbo.Users AS OwnerUsers ON dbo.FollowUps.OwnerUserId = OwnerUsers.Id LEFT OUTER JOIN
                         dbo.Contacts AS CreateByContacts ON CreateByUsers.Id = CreateByContacts.Id LEFT OUTER JOIN
                         dbo.Contacts AS OwnerContacts ON OwnerUsers.Id = OwnerContacts.Id LEFT OUTER JOIN
                         dbo.QuoteTypes ON dbo.Quotes.QuoteTypeCode = dbo.QuoteTypes.Code LEFT OUTER JOIN						
                         dbo.Cards AS MainCarriageCarriers ON dbo.Quotes.MainCarriageCarrierId = MainCarriageCarriers.Id LEFT OUTER JOIN
                         dbo.ShipmentTypes ON dbo.Quotes.ShipmentTypeId = dbo.ShipmentTypes.Id LEFT OUTER JOIN
                         dbo.BusinessUnits ON dbo.Quotes.BusinessUnitId = dbo.BusinessUnits.Id LEFT OUTER JOIN
						 dbo.Incoterms ON dbo.Quotes.IncotermId = dbo.Incoterms.Id LEFT OUTER JOIN
                         dbo.QuoteClosingReasons ON dbo.Quotes.QuoteClosingReasonId = dbo.QuoteClosingReasons.Id


GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -384
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Quotes"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 275
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FollowUps"
            Begin Extent = 
               Top = 6
               Left = 313
               Bottom = 135
               Right = 509
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ShipperCards"
            Begin Extent = 
               Top = 6
               Left = 547
               Bottom = 135
               Right = 732
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ConsigneeCards"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 267
               Right = 223
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FollowUpTypes"
            Begin Extent = 
               Top = 138
               Left = 261
               Bottom = 267
               Right = 490
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "FromPorts"
            Begin Extent = 
               Top = 138
               Left = 528
               Bottom = 267
               Right = 701
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ToPorts"
            Begin Extent = 
               Top = 138
               Left = 739
               Bottom = 267
               Right = 912
            End
        ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'QuoteFollowUpDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FromPortCountries"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 399
               Right = 211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ToPortCountries"
            Begin Extent = 
               Top = 270
               Left = 249
               Bottom = 399
               Right = 422
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Stage"
            Begin Extent = 
               Top = 270
               Left = 460
               Bottom = 399
               Right = 630
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CreateByUsers"
            Begin Extent = 
               Top = 270
               Left = 668
               Bottom = 399
               Right = 838
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OwnerUsers"
            Begin Extent = 
               Top = 402
               Left = 38
               Bottom = 531
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CreateByContacts"
            Begin Extent = 
               Top = 402
               Left = 246
               Bottom = 531
               Right = 453
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "OwnerContacts"
            Begin Extent = 
               Top = 402
               Left = 491
               Bottom = 531
               Right = 698
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "QuoteTypes"
            Begin Extent = 
               Top = 402
               Left = 736
               Bottom = 514
               Right = 906
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MainCarriageCarriers"
            Begin Extent = 
               Top = 294
               Left = 944
               Bottom = 423
               Right = 1129
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ShipmentTypes"
            Begin Extent = 
               Top = 426
               Left = 944
               Bottom = 555
               Right = 1125
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2430
         Alias = 2040
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'QuoteFollowUpDataView'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'QuoteFollowUpDataView'
GO



