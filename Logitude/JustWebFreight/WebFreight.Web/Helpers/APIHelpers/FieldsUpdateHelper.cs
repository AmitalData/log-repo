using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class FieldsUpdateHelper
    {
        [Key]
        public int Tenant { get; set; }
        public List<FieldsTranslations> Items { get; set; }
    }
}