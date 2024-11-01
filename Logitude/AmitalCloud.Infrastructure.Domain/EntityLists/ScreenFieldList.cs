using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityLists
{
    public class ScreenFieldList
    {
        [Key]
        public string Id { get; set; }


        public int Tenant { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }

        public string ScreenId { get; set; }
        public string ScreenCode { get; set; }

        public string ObjectFieldId { get; set; }

        public string ObjectTableName { get; set; }

        public string ComponentPath { get; set; }

        public string ObjectFieldCode { get; set; }
        public int? SectionNumber { get; set; }

    }
}