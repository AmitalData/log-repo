drop VIEW [dbo].[ProceduralFaultDeclarationView] 
go

/****** Object:  View [dbo].[ProceduralFaultsDeclarationsView]    Script Date: 19/1/2015 13:14:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ProceduralFaultDeclarationView]
AS
SELECT        Customs.ProceduralFaults.Id, Customs.ProceduralFaults.Tenant, Customs.ProceduralFaults.ProceduralFaultNumber, 
                         Customs.ProceduralFaults.ProceduralFaultStatusCode, Customs.ProceduralFaults.CreateDate, Customs.ProceduralFaults.InputTypeCode, 
                         Customs.ProceduralFaults.InspectionTypeCode, Customs.ProceduralFaults.ProceduralFaultCode, 
                         Customs.ProceduralFaults.ProceduralFaultInputProcesCode AS ProceduralFaultInputProcesCode, Customs.ProceduralFaults.RansomViolationTypeCode, 
                         Customs.ProceduralFaults.RansomViolationSum, Customs.ProceduralFaults.Remarks, Customs.ProceduralFaults.IsCustomerResponsibility, 
                         Customs.ProceduralFaults.IsAgentProceduralFaultCountabl AS IsAgentProceduralFaultCountabl, 
                         Customs.ProceduralFaults.IsCustProceduralFaultCountabl AS IsCustProceduralFaultCountabl, Customs.ProceduralFaults.IsAgentResponsibility, 
                         Customs.ProceduralFaults.UpdateDate, Customs.ProceduralFaults.LeadingDocumentVersion, Customs.ProceduralFaults.Notes, 
                         Customs.ProceduralFaults.IsCancelled, Customs.ProceduralFaults.CancellationDate, Customs.ProceduralFaults.SearchFields, 
                         Customs.ProceduralFaults.DeclarationId, Customs.Declarations.CustomFileNo, Customs.Declarations.DeclarationNumber, Customs.Declarations.CustomerId, 
                         Customs.FaultInspectionTypes.LocalName AS FaultInspectionName, Customs.ProceduralFaultInSourceTypes.LocalName AS ProceduralFaultInputSourceName, 
                         Customs.ProceduralFaultTypes.LocalName AS ProceduralFaultTypeName, 
                         Customs.ProceduralFaultInProcessTypes.LocalName AS ProceduralFaultInProcesTypName, 
                         Customs.RansomViolationTypes.LocalName AS RansomViolationTypeName, Customs.ProceduralFaultStatuses.LocalName AS ProceduralFaultStatuseName, 
                         dbo.Cards.LocalName AS CustomerName
FROM            dbo.Cards INNER JOIN
                         Customs.Declarations ON dbo.Cards.Id = Customs.Declarations.CustomerId RIGHT OUTER JOIN
                         Customs.ProceduralFaults LEFT OUTER JOIN
                         Customs.ProceduralFaultStatuses ON Customs.ProceduralFaults.ProceduralFaultStatusCode = Customs.ProceduralFaultStatuses.Code AND 
                         Customs.ProceduralFaults.ProceduralFaultStatusCode = Customs.ProceduralFaultStatuses.Code LEFT OUTER JOIN
                         Customs.RansomViolationTypes ON Customs.ProceduralFaults.RansomViolationTypeCode = Customs.RansomViolationTypes.Code AND 
                         Customs.ProceduralFaults.RansomViolationTypeCode = Customs.RansomViolationTypes.Code LEFT OUTER JOIN
                         Customs.ProceduralFaultInProcessTypes ON 
                         Customs.ProceduralFaults.ProceduralFaultInputProcesCode = Customs.ProceduralFaultInProcessTypes.Code AND 
                         Customs.ProceduralFaults.ProceduralFaultInputProcesCode = Customs.ProceduralFaultInProcessTypes.Code LEFT OUTER JOIN
                         Customs.ProceduralFaultTypes ON Customs.ProceduralFaults.ProceduralFaultCode = Customs.ProceduralFaultTypes.Code AND 
                         Customs.ProceduralFaults.ProceduralFaultCode = Customs.ProceduralFaultTypes.Code LEFT OUTER JOIN
                         Customs.ProceduralFaultInSourceTypes ON Customs.ProceduralFaults.InputTypeCode = Customs.ProceduralFaultInSourceTypes.Code AND 
                         Customs.ProceduralFaults.InputTypeCode = Customs.ProceduralFaultInSourceTypes.Code LEFT OUTER JOIN
                         Customs.FaultInspectionTypes ON Customs.ProceduralFaults.InspectionTypeCode = Customs.FaultInspectionTypes.Code AND 
                         Customs.ProceduralFaults.InspectionTypeCode = Customs.FaultInspectionTypes.Code ON Customs.Declarations.Id = Customs.ProceduralFaults.DeclarationId

						 where Customs.ProceduralFaults.Tenant=1
 Go