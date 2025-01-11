using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ScreenPM
    {
        [Key]
        public string Id { get; set; }


        public string Code { get; set; }

        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }

        public string ObjectTableId { get; set; }

        public int Tenant { get; set; }

        public bool IsReadOnly { get; set; }
        public bool Inactive { get; set; }

        public string ObjectTableName { get; set; }
        public string Name { get; set; }
        public int UserTenant { get; set; }
        public string Type { get; set; }
        public string SortedByFieldCode { get; set; }
        public string SortedType { get; set; }
        public string SearchFields { get; set; }
        public string ChildScreenGrid { get; set; }
        public string RelatedScreenCode { get; set; }
        public bool IsHeaderScreen { get; set; }
        [Include]
        [Association("ScreenScreenField", "Id", "ScreenId")]
        public virtual List<ScreenFieldPM> ScreenFields { get; set; }

    }
}