


--drop VIEW ProceduralFaultDeclarationView


--/****** Object:  View [dbo].[ProceduralFaultsDeclarationsView]    Script Date: 19/1/2015 13:14:33 ******/


CREATE VIEW ProceduralFaultDeclarationView
AS
SELECT        ProceduralFaults.Id, ProceduralFaults.Tenant, ProceduralFaults.ProceduralFaultNumber, 
                         ProceduralFaults.ProceduralFaultStatusCode, ProceduralFaults.CreateDate, ProceduralFaults.InputTypeCode, 
                         ProceduralFaults.InspectionTypeCode, ProceduralFaults.ProceduralFaultCode, 
                         ProceduralFaults.ProceduralFaultInputProcesCode AS ProceduralFaultInputProcesCode, ProceduralFaults.RansomViolationTypeCode, 
                         ProceduralFaults.RansomViolationSum, ProceduralFaults.Remarks, ProceduralFaults.IsCustomerResponsibility, 
                         ProceduralFaults.IsAgentProceduralFaultCountabl AS IsAgentProceduralFaultCountabl, 
                         ProceduralFaults.IsCustProceduralFaultCountabl AS IsCustProceduralFaultCountabl, ProceduralFaults.IsAgentResponsibility, 
                         ProceduralFaults.UpdateDate, ProceduralFaults.LeadingDocumentVersion, ProceduralFaults.Notes, 
                         ProceduralFaults.IsCancelled, ProceduralFaults.CancellationDate, ProceduralFaults.SearchFields, 
                         ProceduralFaults.DeclarationId, Declarations.CustomFileNo, Declarations.DeclarationNumber, Declarations.CustomerId, 
                         FaultInspectionTypes.LocalName AS FaultInspectionName, ProceduralFaultInSourceTypes.LocalName AS ProceduralFaultInputSourceName, 
                         ProceduralFaultTypes.LocalName AS ProceduralFaultTypeName, 
                         ProceduralFaultInProcessTypes.LocalName AS ProceduralFaultInProcesTypName, 
                         RansomViolationTypes.LocalName AS RansomViolationTypeName, ProceduralFaultStatuses.LocalName AS ProceduralFaultStatuseName, 
                         Cards.LocalName AS CustomerName
FROM            Cards INNER JOIN
                         Declarations ON Cards.Id = Declarations.CustomerId RIGHT OUTER JOIN
                         ProceduralFaults LEFT OUTER JOIN
                         ProceduralFaultStatuses ON ProceduralFaults.ProceduralFaultStatusCode = ProceduralFaultStatuses.Code AND 
                         ProceduralFaults.ProceduralFaultStatusCode = ProceduralFaultStatuses.Code LEFT OUTER JOIN
                         RansomViolationTypes ON ProceduralFaults.RansomViolationTypeCode = RansomViolationTypes.Code AND 
                         ProceduralFaults.RansomViolationTypeCode = RansomViolationTypes.Code LEFT OUTER JOIN
                         ProceduralFaultInProcessTypes ON 
                         ProceduralFaults.ProceduralFaultInputProcesCode = ProceduralFaultInProcessTypes.Code AND 
                         ProceduralFaults.ProceduralFaultInputProcesCode = ProceduralFaultInProcessTypes.Code LEFT OUTER JOIN
                         ProceduralFaultTypes ON ProceduralFaults.ProceduralFaultCode = ProceduralFaultTypes.Code AND 
                         ProceduralFaults.ProceduralFaultCode = ProceduralFaultTypes.Code LEFT OUTER JOIN
                         ProceduralFaultInSourceTypes ON ProceduralFaults.InputTypeCode = ProceduralFaultInSourceTypes.Code AND 
                         ProceduralFaults.InputTypeCode = ProceduralFaultInSourceTypes.Code LEFT OUTER JOIN
                         FaultInspectionTypes ON ProceduralFaults.InspectionTypeCode = FaultInspectionTypes.Code AND 
                         ProceduralFaults.InspectionTypeCode = FaultInspectionTypes.Code ON Declarations.Id = ProceduralFaults.DeclarationId

 