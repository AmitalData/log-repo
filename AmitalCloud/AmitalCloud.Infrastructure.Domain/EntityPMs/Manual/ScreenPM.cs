using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ScreenPM : BaseEntityPM
    {
        public ScreenPM() : base() { }
        public ScreenPM(Screen entity) : base()
        {
            Id = entity.Id;
            Code = entity.Code;
            NumberOfRows = entity.NumberOfRows;
            NumberOfColumns = entity.NumberOfColumns;
            ObjectTableId = entity.ObjectTableId;
            Tenant = entity.Tenant;
            IsReadOnly = entity.IsReadOnly;
            Inactive = entity.Inactive;
            ObjectTableName = entity.ObjectTable?.Name;
            Name = entity.Name;
            //UserTenant = entity..UserTenant;
            Type = entity.Type;
            SortedByFieldCode = entity.SortedByFieldCode;
            SortedType = entity.SortedType;
            SearchFields = entity.SearchFields;
            //ChildScreenGrid = entity..ChildScreenGrid;
            RelatedScreenCode = entity.RelatedScreenCode;
            IsHeaderScreen = entity.IsHeaderScreen;
            ScreenFields = new List<ScreenFieldPM>();
            //foreach (var item in entity..ScreenFields)
            //{
            //    ScreenFields.Add(new ScreenFieldPM(item));
            //}
        }   
        private Screen headerScreen;


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