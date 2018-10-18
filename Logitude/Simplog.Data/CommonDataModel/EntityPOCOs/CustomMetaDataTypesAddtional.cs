using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomMetaDataTypesAddtional
    {

        [Key]
        [Column("Id", Order = 0)]
        public string Id { get; set; }

        [Key]
        [Column("Code", Order = 1)]
        public string Code { get; set; }

        public int Tenant { get; set; }

        public string DocumentsMetaDataTypesCode { get; set; }


    }
}
