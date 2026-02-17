using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class TextCode
    {
        
        public string Code { get; set; }
        public string DefaultText { get; set; }
        public string ObjectTableId { get; set; }
        public string TextCodeTypeCode { get; set; }
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DefaultTextPlural { get; set; }
        public bool IsSpellChecked { get; set; }
        public DateTime? SpellCheckDate { get; set; }
        public string SpellCheckedByUserId { get; set; }
        public bool InActive { get; set; }
        public string LocalDefaultText { get; set; }

        [ForeignKey("SpellCheckedByUserId")]
        public virtual User SpellCheckedByUser { get; set; }
        //public List<ObjectTableTab> ObjectTableTabs { get; set; }

        //public List<ObjectTable> DescriptionObjectTables { get; set; }

        //public List<ObjectField> ListFieldLableObjectFields { get; set; }
        //[Include]
        //[Association("TextCodeTextCodeType", "TextCodeTypeCode", "Code", IsForeignKey = true)]
        [ForeignKey("TextCodeTypeCode")]
        public virtual TextCodeType TextCodeType { get; set; }
        //[Include]
        //[Association("TextCodeObjectTable", "ObjectTableId", "Id",IsForeignKey=true)]

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        ////[Include]
        ////[Association("TextCodeTranslation", "Id", "TextCodeId")] // by Jalal 11/10
        //public  List<Translation> Translations { get; set; }
        ////[Include]
        ////[Association("TextCodeObjectField1", "Id", "FieldLable")] // by Jalal 11/10
        //public List<ObjectField> FieldLableObjectFields { get; set; }
        ////[Include]
        ////[Association("TextCodeObjectField", "Id", "HelpText")] // by Jalal 11/10
        //public  List<ObjectField> HelpTextObjectFields { get; set; }
        //public List<ObjectField> FullFieldLableObjectFields { get; set; }
        //public List<MenuButton> MenuButtons { get; set; }
        //public List<Feature> Features { get; set; }
        //public List<Query> Queries { get; set; }

        //public List<Tip> Tips { get; set; }

    }
}
