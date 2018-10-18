using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class MetaDataLastUpdateDates
    {
        [Key]
        public int Id { get; set; }
        public DateTime TranslationsSystemUpdateDateGMT { get; set; }
        public DateTime ObjectFieldsSystemUpdateDateGMT { get; set; }

        public DateTime TranslationsTenantUpdateDateGMT { get; set; }
        public DateTime ObjectFieldsTenantUpdateDateGMT { get; set; }
    }
}