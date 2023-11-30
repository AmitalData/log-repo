using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DeploymentPackagePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DirectionId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool InActive { get; set; }
        public string Description { get; set; }
        public string SearchFields { get; set; }
        public string CreatedByUserName { get; set; }
        public string UpdatedByUserName { get; set; }
        public string VersionId { get; set; }
        public string DocumentId { get; set; }
        public bool IsExported { get; set; }
        public string PackageExecutionLogId { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public DeploymentPackageDetails DeploymentPackageDetails { get; set; }
    }

    public class DeploymentPackageDetails
    {
        public List<CustomFields> CustomFields { get; set; }
       public List<CustomPickListItem> CustomPickLists { get; set; }
    }

    public class CustomFields
    {
        public string Code { get; set; }
        public string FieldCode { get; set; }
        public string Name { get; set; }
        public string DefaultText { get; set; }
        public string DataTypeName { get; set; }
        public string ObjectTableName { get; set; }
        public string LookUpTableName { get; set; }
        public string HelpText { get; set; }
        public string CustomPickListCode { get; set; }
        public int NumberOfDigits { get; set; }
        public int DigitsAfterPoint { get; set; }
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public bool IsRequiered { get; set; }
        public bool DisplayOnly { get; set; }
        public bool MultiLine { get; set; }
        public string DefaultAdditionalFilters { get; set; }

    }
    public class CustomPickListItem
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public bool IsMultipleChoice { get; set; }
    }
}
