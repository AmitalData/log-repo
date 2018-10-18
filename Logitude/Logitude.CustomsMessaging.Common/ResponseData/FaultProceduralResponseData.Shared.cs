using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.ResponseData
{
    public class FaultProceduralResponseData : ResponseDataBase
    {
        public string ApplicationID { get; set; }
        public string ResponseStatusXML { get; set; }
        public List<FaultGeneralDetailResult> FaultGeneralDetailList { get; set; }
    }

    public class FaultGeneralDetailResult
    {
        public string ProceduralFaultID { get; set; }
        public string CustomsHouse { get; set; }
        public string CustomsHouseName { get; set; }
        public string DeclarationId { get; set; }
        public string ProceduralFaultCode { get; set; }
        public string ProceduralFaultCodeName { get; set; }
        public string CreateDate { get; set; }
        public string AgentExternalID { get; set; }
        public string ExternalID { get; set; }
        public string ProceduralFaultStatus { get; set; }
        public string ProceduralFaultStatusName { get; set; }
        public string ProceduralFaultInputProcessName { get; set; }
        public string ImporterExporterResponsibilityName { get; set; }
        public List<FaultAdittionalInformationResult> FaultAdittionalInformationList { get; set; }
        public List<FieldsPathtoFaultResult> FieldsPathtoFaultList { get; set; }
    }

    public class FaultAdittionalInformationResult
    {
        public string AgentInDeclarationName { get; set; }
        public string AgentResponsibilityID { get; set; }
        public string AgentResponsibilityName { get; set; }
        public string ResponsibilityID { get; set; }
        public string ResponsibilityName { get; set; }
        public string ProceduralFaultInputProcess { get; set; }
        public string ProceduralFaultInputProcessName { get; set; }
        public string RansomViolationType { get; set; }
        public string RansomViolationTypeName { get; set; }
        public string RansomViolationSum { get; set; }
        public string FelonyType { get; set; }
        public string FelonyTypeName { get; set; }
        public string Severity { get; set; }
        public string SeverityName { get; set; }
        public string Scoring { get; set; }
        public string ExporterImporterInDeclarationName { get; set; }
    }

    public class FieldsPathtoFaultResult
    {
        public string SequenceNumber { get; set; }
        public string DisplayPath { get; set; }
    }
}
