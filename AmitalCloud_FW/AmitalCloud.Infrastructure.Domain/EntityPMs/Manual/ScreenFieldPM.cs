using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ScreenFieldPM : BaseEntityPM
    {
        public ScreenFieldPM() : base (){ }
        public ScreenFieldPM(ScreenField entity) : base()
        {
            Id = entity.Id;
            Tenant = entity.Tenant;
            Column = entity.Column;
            Row = entity.Row;
            ScreenId = entity.ScreenId;
            ObjectFieldId = entity.ObjectFieldId;
            ObjectFieldName = entity.ObjectField?.FieldName;
            ScreenCode = entity.ScreenCode;
            ObjectFieldObjectTableName = entity.ObjectField?.ObjectTable?.Name;
            ObjectFieldCode = entity.ObjectField?.Code;
            SectionNumber = entity.SectionNumber;
        }   
        [Key]
        public string Id { get; set; }


        public int Tenant { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }

        public string ScreenId { get; set; }

        public string ObjectFieldId { get; set; }

        public string ObjectFieldName { get; set; }

        public string ScreenCode { get; set; }

        public string ObjectFieldObjectTableName { get; set; }

        public string ObjectFieldCode { get; set; }
        public int? SectionNumber { get; set; }

    }
}
