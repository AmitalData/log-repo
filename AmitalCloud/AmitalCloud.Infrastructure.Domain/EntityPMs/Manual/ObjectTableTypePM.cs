using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ObjectTableTypePM
    {
        private ObjectTableType objectTableType;

        public ObjectTableTypePM(ObjectTableType objectTableType)
        {
            this.objectTableType = objectTableType;
        }

        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}