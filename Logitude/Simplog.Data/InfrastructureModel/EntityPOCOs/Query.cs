using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class Query
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public string ObjectTableId { get; set; }
        public bool SystemLevel { get; set; }
        public bool TenantLevel { get; set; }
        public string OriginalQueryId { get; set; }
        public string QuerySection { get; set; }
        public int IndexOrder { get; set; }
        public bool DisplayCount { get; set; }
        public bool IsAddNewEntityEnabled { get; set; }
        public string QueryGroupCode { get; set; }
        public string NameTextCodeId { get; set; }
        public string DefaultSortDirection { get; set; }
        public string DefaultSortColumn { get; set; }
        public string SpotlightDataTemplate { get; set; }
        public bool Internal { get; set; }
        public bool Customer { get; set; }
        public bool Agent { get; set; }
        public string FeatureId { get; set; }
        public string EditWizardName { get; set; }
        public string Perspective { get; set; }
        public bool IsHiddenFromView { get; set; }
        public bool IsNewFromTenantZeroOnly { get; set; }
        public string EditWizardComponentPath { get; set; }
        public bool SharedWithAll { get; set; }
        public bool SharedWithSpecificUsers { get; set; }
        public string SharedByUserId { get; set; }
        public bool SpotlightModeActivated { get; set; }
        public string NameTextCodeCode { get; set; }

        [ForeignKey("FeatureId")]
        public virtual Feature Feature { get; set; }

        [ForeignKey("NameTextCodeId")]
        public virtual TextCode NameTextCode { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("QueryGroupCode")]
        public virtual QueryGroup QueryGroup { get; set; }

        [ForeignKey("SharedByUserId")]
        public User SharedByUser { get; set; }

        public Query OriginalQuery { get; set; }
        public List<Query> CopiedQueries { get; set; }
    }
}