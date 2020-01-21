using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class QueryPM
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
        public string ObjectTableName { get; set; }
        public bool ObjectTableIsNewWizard { get; set; }
        public string ObjectTableNewWizardControlName { get; set; }
        public string QueryGroupCode { get; set; }
        public int QueryGroupIndexOrder { get; set; }
        public bool IsAddNewEntityEnabled { get; set; }
        public string NameTextCodeId { get; set; }
        public string NameTextCodeCode { get; set; }
        //public string QueryLabel { get; set; }
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
        public string NewViewName { get; set; }
        public string EditWizardComponentPath { get; set; }
        public bool SharedWithAll { get; set; }
        public bool SharedWithSpecificUsers { get; set; }
        public string SharedByUserId { get; set; }
        public string SharedByUserName { get; set; }
        public string SharedByUserEmail { get; set; }
        public string FeatureUniqeCode { get; set; }


        private List<SharedUserQueryPM> sharedUserQueries;
        [Include]
        [Association("QueryPMSharedUserQueryPM", "Id", "QueryId")]
        [Composition]
        [DataMember]
        public virtual List<SharedUserQueryPM> SharedUserQueries
        {
            get
            {
                if (sharedUserQueries == null)
                {
                    sharedUserQueries = new List<SharedUserQueryPM>();
                }

                return sharedUserQueries;
            }

            set
            {
                sharedUserQueries = value;
            }
        }
        public bool SpotlightModeActivated { get; set; }

    }
}
